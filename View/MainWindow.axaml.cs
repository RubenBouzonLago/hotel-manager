using Avalonia.Controls;
using Avalonia.Interactivity;
using TRABAJO_GRUPAL_AVALONIA.Core;

namespace TRABAJO_GRUPAL_AVALONIA.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HotelManager.CargarDatos(); // Cargar XML al inicio

            var content = this.FindControl<Border>("MainContent");

            // Eventos básicos
            this.FindControl<Button>("BtnHabitaciones").Click += (s, e) => 
            {
                content.Child = new AddRoomWindow();
            };
            this.FindControl<Button>("BtnClientes").Click += (s, e) => 
            {
                content.Child = new AddClientWindow();
            };
            this.FindControl<Button>("BtnReservas").Click += (s, e) => 
            {
                content.Child = new AddBookingWindow();
            };
            this.FindControl<Button>("BtnBusquedas").Click += (s, e) => 
            {
                content.Child = new SearchWindow();
            };
            this.FindControl<Button>("BtnGraficos").Click += (s, e) => 
            {
                content.Child = new GraphWindow();
            };
            this.FindControl<Button>("BtnSalir").Click += (s, e) => Close();
        }
    }
}