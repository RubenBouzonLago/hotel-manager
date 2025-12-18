using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using TRABAJO_GRUPAL_AVALONIA.Core.Graficos;

namespace TRABAJO_GRUPAL_AVALONIA.View
{
    public partial class GraphWindow : UserControl
    {
        public GraphWindow()
        {
            InitializeComponent();

            // Inicializar controles
            var combo = this.FindControl<ComboBox>("GraphTypeCombo");
            combo.ItemsSource = new[]
            {
                "Ocupación General",
                "Ocupación por Cliente",
                "Ocupación por Habitación",
                "Comodidades"
            };

            // Seleccionar por defecto
            combo.SelectedIndex = 0;
            this.FindControl<ComboBox>("PeriodCombo").SelectedIndex = 0;

            combo.SelectionChanged += GraphTypeCombo_SelectionChanged;
            this.FindControl<Button>("GenerateButton").Click += GenerateButton_Click;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void GraphTypeCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            string? selected = combo?.SelectedItem as string;
            var paramCombo = this.FindControl<ComboBox>("ParamCombo");

            if (selected == "Ocupación por Cliente")
            {
                var graf = new GraficoOcupacionCliente();
                paramCombo.ItemsSource = graf.ObtenerClientes().ToArray();
                paramCombo.IsEnabled = true;
                if (paramCombo.ItemCount > 0) paramCombo.SelectedIndex = 0;
            }
            else if (selected == "Ocupación por Habitación")
            {
                var graf = new GraficoOcupacionHabitacion();
                paramCombo.ItemsSource = graf.ObtenerHabitaciones().ToArray();
                paramCombo.IsEnabled = true;
                if (paramCombo.ItemCount > 0) paramCombo.SelectedIndex = 0;
            }
            else
            {
                paramCombo.ItemsSource = new List<string>();
                paramCombo.IsEnabled = false;
            }
        }

        private void GenerateButton_Click(object? sender, RoutedEventArgs e)
        {
            var periodCombo = this.FindControl<ComboBox>("PeriodCombo");
            var paramCombo = this.FindControl<ComboBox>("ParamCombo");
            var graphTypeCombo = this.FindControl<ComboBox>("GraphTypeCombo");
            var plotControl = this.FindControl<Canvas>("PlotControl");

            string periodo = (periodCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Mes a Mes";
            string? parametro = paramCombo.IsEnabled ? paramCombo.SelectedItem as string : null;

            string selected = graphTypeCombo.SelectedItem as string ?? "Ocupación General";

            GraficoBase grafico = selected switch
            {
                "Ocupación General" => new GraficoOcupacionGeneral(),
                "Ocupación por Cliente" => new GraficoOcupacionCliente(),
                "Ocupación por Habitación" => new GraficoOcupacionHabitacion(),
                "Comodidades" => new GraficoComodidades(),
                _ => new GraficoOcupacionGeneral()
            };

            // Asignar control de plot
            grafico.SetPlotControl(plotControl);

            // Generar
            grafico.Generar(periodo, parametro ?? string.Empty);
        }
    }
}
