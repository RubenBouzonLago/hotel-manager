using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using TRABAJO_GRUPAL_AVALONIA.Core;

namespace TRABAJO_GRUPAL_AVALONIA.View
{
    public partial class SearchWindow : UserControl
    {
        public SearchWindow()
        {
            InitializeComponent();
            
            this.FindControl<Button>("BtnBuscar").Click += BtnBuscar_Click;
            
            var cboTipo = this.FindControl<ComboBox>("CboTipoBusqueda");
            cboTipo.SelectionChanged += CboTipoBusqueda_SelectionChanged;

            CargarDatos();
            ActualizarVisibilidad(0); // Inicializar
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CargarDatos()
        {
            this.FindControl<ComboBox>("CboCliente").ItemsSource = HotelManager.Clientes;
            this.FindControl<ComboBox>("CboHabitacion").ItemsSource = HotelManager.Habitaciones;
        }

        private void CboTipoBusqueda_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var cbo = sender as ComboBox;
            ActualizarVisibilidad(cbo.SelectedIndex);
        }

        private void ActualizarVisibilidad(int index)
        {
            var pCliente = this.FindControl<StackPanel>("PanelCliente");
            var pHabitacion = this.FindControl<StackPanel>("PanelHabitacion");
            var pPiso = this.FindControl<StackPanel>("PanelPiso");
            var pFecha = this.FindControl<StackPanel>("PanelFecha");
            var pAnio = this.FindControl<StackPanel>("PanelAnio");

            pCliente.IsVisible = false;
            pHabitacion.IsVisible = false;
            pPiso.IsVisible = false;
            pFecha.IsVisible = false;
            pAnio.IsVisible = false;

            switch (index)
            {
                case 0: // Reservas Pendientes (Próximos 5 días)
                    pHabitacion.IsVisible = true;
                    break;
                case 1: // Disponibilidad
                    pPiso.IsVisible = true;
                    break;
                case 2: // Reservas por Persona
                    pCliente.IsVisible = true;
                    pAnio.IsVisible = true;
                    break;
                case 3: // Reservas por Habitación
                    pHabitacion.IsVisible = true;
                    pAnio.IsVisible = true;
                    break;
                case 4: // Ocupación
                    pFecha.IsVisible = true;
                    pAnio.IsVisible = true;
                    break;
            }
        }

        private void BtnBuscar_Click(object? sender, RoutedEventArgs e)
        {
            var index = this.FindControl<ComboBox>("CboTipoBusqueda").SelectedIndex;
            var resultados = new List<string>();

            try
            {
                switch (index)
                {
                    case 0: // Reservas Pendientes (Próximos 5 días)
                        resultados = BuscarReservasPendientes();
                        break;
                    case 1: // Disponibilidad
                        resultados = BuscarDisponibilidad();
                        break;
                    case 2: // Reservas por Persona
                        resultados = BuscarReservasPorPersona();
                        break;
                    case 3: // Reservas por Habitación
                        resultados = BuscarReservasPorHabitacion();
                        break;
                    case 4: // Ocupación
                        resultados = BuscarOcupacion();
                        break;
                }
            }
            catch (Exception ex)
            {
                resultados.Add($"Error en la búsqueda: {ex.Message}");
            }

            this.FindControl<ListBox>("LstResultados").ItemsSource = resultados;
        }

        private List<string> BuscarReservasPendientes()
        {
            var hoy = DateTime.Today;
            var limite = hoy.AddDays(5);
            
            var habSeleccionada = this.FindControl<ComboBox>("CboHabitacion").SelectedItem as Habitacion;

            var query = HotelManager.Reservas.Where(r => 
                r.FechaEntrada >= hoy && r.FechaEntrada <= limite);

            if (habSeleccionada != null)
            {
                query = query.Where(r => r.NumeroHabitacion == habSeleccionada.Numero);
            }

            return query.Select(r => FormatearReserva(r)).ToList();
        }

        private List<string> BuscarDisponibilidad()
        {
            // Habitaciones vacías HOY
            var hoy = DateTime.Today;
            var txtPiso = this.FindControl<TextBox>("TxtPiso").Text;

            // Habitaciones ocupadas hoy
            var ocupadasIds = HotelManager.Reservas
                .Where(r => r.FechaEntrada <= hoy && r.FechaSalida > hoy)
                .Select(r => r.NumeroHabitacion)
                .Distinct()
                .ToList();

            var disponibles = HotelManager.Habitaciones
                .Where(h => !ocupadasIds.Contains(h.Numero));

            if (!string.IsNullOrWhiteSpace(txtPiso) && int.TryParse(txtPiso, out int piso))
            {
                disponibles = disponibles.Where(h => h.ObtenerPiso() == piso);
            }

            return disponibles.Select(h => $"Habitación {h.Numero} ({h.Tipo}) - Piso {h.ObtenerPiso()}").ToList();
        }

        private List<string> BuscarReservasPorPersona()
        {
            var cliente = this.FindControl<ComboBox>("CboCliente").SelectedItem as Cliente;
            if (cliente == null) return new List<string> { "Seleccione un cliente." };

            var query = HotelManager.Reservas.Where(r => r.ClienteDNI == cliente.DNI);

            if (FiltrarPorAnio(out int anio))
            {
                query = query.Where(r => r.FechaEntrada.Year == anio);
            }

            return query.Select(r => FormatearReserva(r)).ToList();
        }

        private List<string> BuscarReservasPorHabitacion()
        {
            var hab = this.FindControl<ComboBox>("CboHabitacion").SelectedItem as Habitacion;
            
            var query = HotelManager.Reservas.AsEnumerable(); // Start with all

            if (hab != null)
            {
                query = query.Where(r => r.NumeroHabitacion == hab.Numero);
            }

            if (FiltrarPorAnio(out int anio))
            {
                query = query.Where(r => r.FechaEntrada.Year == anio);
            }

            return query.Select(r => FormatearReserva(r)).ToList();
        }

        private List<string> BuscarOcupacion()
        {
            var query = HotelManager.Reservas.AsEnumerable();

            if (FiltrarPorAnio(out int anio))
            {
                // Ocupación en todo el año (listado de reservas de ese año)
                query = query.Where(r => r.FechaEntrada.Year == anio || r.FechaSalida.Year == anio);
                return query.Select(r => $"Hab {r.NumeroHabitacion} ocupada por {r.NombreCliente} del {r.FechaEntrada:d} al {r.FechaSalida:d}").ToList();
            }
            else
            {
                // Ocupación en fecha específica
                var fecha = this.FindControl<DatePicker>("DpFecha").SelectedDate?.DateTime ?? DateTime.Today;
                
                query = query.Where(r => r.FechaEntrada <= fecha && r.FechaSalida > fecha);
                
                var resultados = query.Select(r => $"Habitación {r.NumeroHabitacion} ocupada por {r.NombreCliente} (Hasta: {r.FechaSalida:d})").ToList();
                
                if (resultados.Count == 0) return new List<string> { $"No hay habitaciones ocupadas el {fecha:d}" };
                return resultados;
            }
        }

        private bool FiltrarPorAnio(out int anio)
        {
            anio = 0;
            var chk = this.FindControl<CheckBox>("ChkFiltrarAnio").IsChecked == true;
            var txt = this.FindControl<TextBox>("TxtAnio").Text;
            
            if (chk && int.TryParse(txt, out int a))
            {
                anio = a;
                return true;
            }
            return false;
        }

        private string FormatearReserva(Reserva r)
        {
            return $"ID: {r.IdReserva} | Hab: {r.NumeroHabitacion} | Cliente: {r.NombreCliente} | Entrada: {r.FechaEntrada:d} | Salida: {r.FechaSalida:d}";
        }
    }
}
