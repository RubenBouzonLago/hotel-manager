using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using TRABAJO_GRUPAL_AVALONIA.Core;

namespace TRABAJO_GRUPAL_AVALONIA.View
{
    public partial class AddBookingWindow : UserControl
    {
        private Reserva _reservaSeleccionada;

        public AddBookingWindow()
        {
            InitializeComponent();
            
            this.FindControl<Button>("BtnGuardar").Click += GuardarReserva;
            this.FindControl<Button>("BtnNuevo").Click += (s, e) => LimpiarFormulario();
            this.FindControl<Button>("BtnEditar").Click += EditarReserva;
            this.FindControl<Button>("BtnEliminar").Click += EliminarReserva;

            CargarCombos();
            ActualizarLista();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CargarCombos()
        {
            var cboCliente = this.FindControl<ComboBox>("CboCliente");
            cboCliente.ItemsSource = HotelManager.Clientes;

            var cboHabitacion = this.FindControl<ComboBox>("CboHabitacion");
            cboHabitacion.ItemsSource = HotelManager.Habitaciones;
        }

        private void ActualizarLista()
        {
            var lista = this.FindControl<ListBox>("LstReservas");
            if (lista != null)
            {
                lista.ItemsSource = null;
                lista.ItemsSource = new List<Reserva>(HotelManager.Reservas);
            }
        }

        private void LimpiarFormulario()
        {
            _reservaSeleccionada = null;
            this.FindControl<ComboBox>("CboCliente").SelectedIndex = -1;
            this.FindControl<ComboBox>("CboHabitacion").SelectedIndex = -1;
            this.FindControl<DatePicker>("DpEntrada").SelectedDate = null;
            this.FindControl<DatePicker>("DpSalida").SelectedDate = null;
            this.FindControl<CheckBox>("ChkGaraje").IsChecked = false;
            this.FindControl<TextBox>("TxtImporte").Text = "";
            this.FindControl<TextBox>("TxtIVA").Text = "21";
        }

        private void EditarReserva(object sender, RoutedEventArgs e)
        {
            var lista = this.FindControl<ListBox>("LstReservas");
            if (lista.SelectedItem is Reserva res)
            {
                _reservaSeleccionada = res;
                
                // Seleccionar cliente
                var cboCliente = this.FindControl<ComboBox>("CboCliente");
                foreach(Cliente c in cboCliente.ItemsSource)
                {
                    if(c.DNI == res.ClienteDNI)
                    {
                        cboCliente.SelectedItem = c;
                        break;
                    }
                }

                // Seleccionar habitacion
                var cboHab = this.FindControl<ComboBox>("CboHabitacion");
                foreach(Habitacion h in cboHab.ItemsSource)
                {
                    if(h.Numero == res.NumeroHabitacion)
                    {
                        cboHab.SelectedItem = h;
                        break;
                    }
                }

                this.FindControl<DatePicker>("DpEntrada").SelectedDate = res.FechaEntrada;
                this.FindControl<DatePicker>("DpSalida").SelectedDate = res.FechaSalida;
                this.FindControl<CheckBox>("ChkGaraje").IsChecked = res.Garaje;
                this.FindControl<TextBox>("TxtImporte").Text = res.ImportePorDia.ToString();
                this.FindControl<TextBox>("TxtIVA").Text = res.IVA.ToString();
            }
        }

        private void EliminarReserva(object sender, RoutedEventArgs e)
        {
            var lista = this.FindControl<ListBox>("LstReservas");
            if (lista.SelectedItem is Reserva res)
            {
                HotelManager.Reservas.Remove(res);
                HotelManager.GuardarDatos();
                ActualizarLista();
                LimpiarFormulario();
            }
        }

        private void GuardarReserva(object sender, RoutedEventArgs e)
        {
            try
            {
                this.FindControl<TextBlock>("TxtError").Text = "";
                var cboCliente = this.FindControl<ComboBox>("CboCliente");
                var cboHab = this.FindControl<ComboBox>("CboHabitacion");
                
                if (cboCliente.SelectedItem == null || cboHab.SelectedItem == null) 
                {
                    this.FindControl<TextBlock>("TxtError").Text = "Selecciona cliente y habitación.";
                    return;
                }

                var cliente = (Cliente)cboCliente.SelectedItem;
                var habitacion = (Habitacion)cboHab.SelectedItem;

                var fechaEntrada = this.FindControl<DatePicker>("DpEntrada").SelectedDate;
                var fechaSalida = this.FindControl<DatePicker>("DpSalida").SelectedDate;
                
                if (!fechaEntrada.HasValue || !fechaSalida.HasValue) 
                {
                    this.FindControl<TextBlock>("TxtError").Text = "Selecciona fechas de entrada y salida.";
                    return;
                }
                
                // Validar fechas
                if (fechaEntrada.Value.DateTime >= fechaSalida.Value.DateTime) 
                {
                    this.FindControl<TextBlock>("TxtError").Text = "La fecha de salida debe ser posterior a la entrada.";
                    return;
                }

                // Validar disponibilidad
                foreach (var r in HotelManager.Reservas)
                {
                    if (_reservaSeleccionada != null && r.IdReserva == _reservaSeleccionada.IdReserva) continue;

                    if (r.NumeroHabitacion == habitacion.Numero)
                    {
                        // Comprobar solapamiento
                        if (fechaEntrada.Value.DateTime < r.FechaSalida && fechaSalida.Value.DateTime > r.FechaEntrada)
                        {
                            this.FindControl<TextBlock>("TxtError").Text = "La habitación está ocupada en esas fechas.";
                            return;
                        }
                    }
                }

                decimal.TryParse(this.FindControl<TextBox>("TxtImporte").Text, out decimal importe);
                decimal.TryParse(this.FindControl<TextBox>("TxtIVA").Text, out decimal iva);
                bool garaje = this.FindControl<CheckBox>("ChkGaraje").IsChecked ?? false;

                if (_reservaSeleccionada == null)
                {
                    var nuevaReserva = new Reserva
                    {
                        ClienteDNI = cliente.DNI,
                        NumeroHabitacion = habitacion.Numero,
                        FechaEntrada = fechaEntrada.Value.DateTime,
                        FechaSalida = fechaSalida.Value.DateTime,
                        Garaje = garaje,
                        ImportePorDia = importe,
                        IVA = iva
                    };
                    nuevaReserva.GenerarId();
                    HotelManager.Reservas.Add(nuevaReserva);
                }
                else
                {
                    _reservaSeleccionada.ClienteDNI = cliente.DNI;
                    _reservaSeleccionada.NumeroHabitacion = habitacion.Numero;
                    _reservaSeleccionada.FechaEntrada = fechaEntrada.Value.DateTime;
                    _reservaSeleccionada.FechaSalida = fechaSalida.Value.DateTime;
                    _reservaSeleccionada.Garaje = garaje;
                    _reservaSeleccionada.ImportePorDia = importe;
                    _reservaSeleccionada.IVA = iva;
                    _reservaSeleccionada.GenerarId(); // Regenerar ID por si cambiaron fechas o habitación
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
    }
}