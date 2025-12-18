using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using TRABAJO_GRUPAL_AVALONIA.Core;

namespace TRABAJO_GRUPAL_AVALONIA.View
{
    public partial class AddRoomWindow : UserControl
    {
        private Habitacion _habitacionSeleccionada;

        public AddRoomWindow()
        {
            InitializeComponent();
            
            this.FindControl<Button>("BtnAdd").Click += GuardarHabitacion;
            this.FindControl<Button>("BtnNuevo").Click += (s, e) => LimpiarFormulario();
            this.FindControl<Button>("BtnEditar").Click += EditarHabitacion;
            this.FindControl<Button>("BtnEliminar").Click += EliminarHabitacion;
            
            // Eventos de comodidades
            this.FindControl<Button>("BtnAddComodidad").Click += AddComodidad;
            this.FindControl<Button>("BtnDelComodidad").Click += DelComodidad;

            ActualizarLista();
            ActualizarComodidades();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ActualizarLista()
        {
            var lista = this.FindControl<ListBox>("LstHabitaciones");
            if (lista != null)
            {
                // Crear una nueva lista para forzar la actualización visual en la interfaz
                lista.ItemsSource = null;
                lista.ItemsSource = new System.Collections.Generic.List<Habitacion>(HotelManager.Habitaciones);
            }
        }

        private void ActualizarComodidades()
        {
            var lista = this.FindControl<ListBox>("LstComodidades");
            if (lista != null)
            {
                lista.ItemsSource = null;
                lista.ItemsSource = new System.Collections.Generic.List<Comodidad>(HotelManager.ComodidadesDisponibles);
            }
        }

        private void AddComodidad(object sender, RoutedEventArgs e)
        {
            var nombre = this.FindControl<TextBox>("TxtComodidadNombre").Text;
            var desc = this.FindControl<TextBox>("TxtComodidadDesc").Text;

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                HotelManager.ComodidadesDisponibles.Add(new Comodidad(nombre, desc));
                HotelManager.GuardarDatos();
                ActualizarComodidades();
                
                this.FindControl<TextBox>("TxtComodidadNombre").Text = "";
                this.FindControl<TextBox>("TxtComodidadDesc").Text = "";
            }
        }

        private void DelComodidad(object sender, RoutedEventArgs e)
        {
            var lista = this.FindControl<ListBox>("LstComodidades");
            // Eliminar los items seleccionados de la lista global
            // Copiamos la lista de seleccionados para evitar errores de modificación durante iteración
            var seleccionados = new System.Collections.Generic.List<object>();
            foreach (var item in lista.SelectedItems)
            {
                seleccionados.Add(item);
            }

            foreach (Comodidad c in seleccionados)
            {
                HotelManager.ComodidadesDisponibles.Remove(c);
            }

            if (seleccionados.Count > 0)
            {
                HotelManager.GuardarDatos();
                ActualizarComodidades();
            }
        }

        private void LimpiarFormulario()
        {
            _habitacionSeleccionada = null;
            this.FindControl<TextBox>("TxtNumero").Text = "";
            this.FindControl<ComboBox>("CboTipo").SelectedIndex = -1;
            this.FindControl<DatePicker>("DpRenovacion").SelectedDate = null;
            this.FindControl<DatePicker>("DpReserva").SelectedDate = null;
            this.FindControl<ListBox>("LstComodidades").SelectedItems.Clear();
        }

        private void EditarHabitacion(object sender, RoutedEventArgs e)
        {
            var lista = this.FindControl<ListBox>("LstHabitaciones");
            if (lista.SelectedItem is Habitacion hab)
            {
                _habitacionSeleccionada = hab;
                this.FindControl<TextBox>("TxtNumero").Text = hab.Numero.ToString();
                this.FindControl<ComboBox>("CboTipo").SelectedIndex = (int)hab.Tipo;
                
                if (hab.UltimaRenovacion != DateTime.MinValue)
                    this.FindControl<DatePicker>("DpRenovacion").SelectedDate = hab.UltimaRenovacion;
                
                if (hab.UltimaReserva != DateTime.MinValue)
                    this.FindControl<DatePicker>("DpReserva").SelectedDate = hab.UltimaReserva;

                // Cargar comodidades seleccionadas
                var lstComodidades = this.FindControl<ListBox>("LstComodidades");
                lstComodidades.SelectedItems.Clear();
                
                // Recorremos las disponibles y si el nombre coincide con alguna de la habitación, la seleccionamos
                foreach (Comodidad disponible in HotelManager.ComodidadesDisponibles)
                {
                    if (hab.Comodidades.Exists(c => c.Nombre == disponible.Nombre))
                    {
                        lstComodidades.SelectedItems.Add(disponible);
                    }
                }
            }
        }

        private void EliminarHabitacion(object sender, RoutedEventArgs e)
        {
            var lista = this.FindControl<ListBox>("LstHabitaciones");
            if (lista.SelectedItem is Habitacion hab)
            {
                HotelManager.Habitaciones.Remove(hab);
                HotelManager.GuardarDatos();
                ActualizarLista();
                LimpiarFormulario();
            }
        }

        private void GuardarHabitacion(object sender, RoutedEventArgs e)
        {
            try
            {
                this.FindControl<TextBlock>("TxtError").Text = ""; // Limpiar errores
                var txtNum = this.FindControl<TextBox>("TxtNumero").Text;
                var cboTipo = this.FindControl<ComboBox>("CboTipo").SelectedIndex;
                
                if(!int.TryParse(txtNum, out int numero)) 
                {
                    this.FindControl<TextBlock>("TxtError").Text = "El número de habitación debe ser numérico.";
                    return;
                }

                // Validar formato pnn
                if (!Habitacion.EsNumeroValido(numero))
                {
                    this.FindControl<TextBlock>("TxtError").Text = "Número inválido. Debe ser de 3 dígitos (pnn) y los dos últimos entre 01-99.";
                    return;
                }

                if (_habitacionSeleccionada == null)
                {
                    // Validar duplicados
                    if (HotelManager.Habitaciones.Exists(h => h.Numero == numero)) 
                    {
                        this.FindControl<TextBlock>("TxtError").Text = "Ya existe una habitación con ese número.";
                        return;
                    }

                    // Nueva habitación
                    var nuevaHab = new Habitacion(numero, (TipoHabitacion)cboTipo);
                    ActualizarDatosHabitacion(nuevaHab);
                    HotelManager.Habitaciones.Add(nuevaHab);
                }
                else
                {
                    // Editar existente
                    if (_habitacionSeleccionada.Numero != numero && HotelManager.Habitaciones.Exists(h => h.Numero == numero)) 
                    {
                        this.FindControl<TextBlock>("TxtError").Text = "Ya existe una habitación con ese número.";
                        return;
                    }
                    
                    // Restricción: No se puede cambiar el tipo
                    if (_habitacionSeleccionada.Tipo != (TipoHabitacion)cboTipo)
                    {
                         this.FindControl<ComboBox>("CboTipo").SelectedIndex = (int)_habitacionSeleccionada.Tipo;
                         this.FindControl<TextBlock>("TxtError").Text = "No se puede cambiar el tipo de habitación.";
                         return;
                    }

                    _habitacionSeleccionada.Numero = numero;
                    ActualizarDatosHabitacion(_habitacionSeleccionada);
                }

                HotelManager.GuardarDatos();
                ActualizarLista();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                this.FindControl<TextBlock>("TxtError").Text = "Error: " + ex.Message;
            }
        }

        private void ActualizarDatosHabitacion(Habitacion hab)
        {
            var dateRenovacion = this.FindControl<DatePicker>("DpRenovacion").SelectedDate;
            if(dateRenovacion.HasValue) hab.UltimaRenovacion = dateRenovacion.Value.DateTime;

            var dateReserva = this.FindControl<DatePicker>("DpReserva").SelectedDate;
            if(dateReserva.HasValue) hab.UltimaReserva = dateReserva.Value.DateTime;

            hab.Comodidades.Clear();
            var lstComodidades = this.FindControl<ListBox>("LstComodidades");
            
            foreach (Comodidad c in lstComodidades.SelectedItems)
            {
                hab.Comodidades.Add(c);
            }
        }
    }
}