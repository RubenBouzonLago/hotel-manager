using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using TRABAJO_GRUPAL_AVALONIA.Core;

namespace TRABAJO_GRUPAL_AVALONIA.View
{
    public partial class AddClientWindow : UserControl
    {
        private Cliente _clienteSeleccionado;

        public AddClientWindow()
        {
            InitializeComponent();
            this.FindControl<Button>("BtnGuardar").Click += GuardarCliente;
            this.FindControl<Button>("BtnNuevo").Click += (s, e) => LimpiarFormulario();
            this.FindControl<Button>("BtnEditar").Click += EditarCliente;
            this.FindControl<Button>("BtnEliminar").Click += EliminarCliente;
            
            ActualizarLista();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ActualizarLista()
        {
            var lista = this.FindControl<ListBox>("LstClientes");
            if (lista != null)
            {
                // Crear una nueva lista para forzar la actualización visual en la interfaz
                lista.ItemsSource = null;
                lista.ItemsSource = new System.Collections.Generic.List<Cliente>(HotelManager.Clientes);
            }
        }

        private void LimpiarFormulario()
        {
            _clienteSeleccionado = null;
            this.FindControl<TextBox>("TxtDni").Text = "";
            this.FindControl<TextBox>("TxtNombre").Text = "";
            this.FindControl<TextBox>("TxtTelefono").Text = "";
            this.FindControl<TextBox>("TxtEmail").Text = "";
            this.FindControl<TextBox>("TxtDireccion").Text = "";
        }

        private void EditarCliente(object sender, RoutedEventArgs e)
        {
            var lista = this.FindControl<ListBox>("LstClientes");
            if (lista.SelectedItem is Cliente cliente)
            {
                _clienteSeleccionado = cliente;
                this.FindControl<TextBox>("TxtDni").Text = cliente.DNI;
                this.FindControl<TextBox>("TxtNombre").Text = cliente.Nombre;
                this.FindControl<TextBox>("TxtTelefono").Text = cliente.Telefono;
                this.FindControl<TextBox>("TxtEmail").Text = cliente.Email;
                this.FindControl<TextBox>("TxtDireccion").Text = cliente.Direccion;
            }
        }

        private void EliminarCliente(object sender, RoutedEventArgs e)
        {
            var lista = this.FindControl<ListBox>("LstClientes");
            if (lista.SelectedItem is Cliente cliente)
            {
                HotelManager.Clientes.Remove(cliente);
                HotelManager.GuardarDatos();
                ActualizarLista();
                LimpiarFormulario();
            }
        }

        private void GuardarCliente(object sender, RoutedEventArgs e)
        {
            this.FindControl<TextBlock>("TxtError").Text = "";
            var dni = this.FindControl<TextBox>("TxtDni").Text;
            var nombre = this.FindControl<TextBox>("TxtNombre").Text;
            var telefono = this.FindControl<TextBox>("TxtTelefono").Text;

            
            if (string.IsNullOrWhiteSpace(dni))
            {
                this.FindControl<TextBlock>("TxtError").Text = "El DNI es obligatorio.";
                return;
            }
            if (string.IsNullOrWhiteSpace(nombre))
            {
                this.FindControl<TextBlock>("TxtError").Text = "El nombre es obligatorio.";
                return;
            }
            if (string.IsNullOrWhiteSpace(telefono))
            {
                this.FindControl<TextBlock>("TxtError").Text = "El teléfono es obligatorio.";
                return;
            }
            

            if (!EsDniValido(dni))
            {
                this.FindControl<TextBlock>("TxtError").Text = "El formato del DNI no es válido (8 números y 1 letra).";
                return;
            }

            if (_clienteSeleccionado == null)
            {
                if (HotelManager.Clientes.Exists(c => c.DNI == dni))
                {
                    this.FindControl<TextBlock>("TxtError").Text = "Ya existe un cliente con ese DNI.";
                    return;
                }

                // Nuevo cliente
                var cliente = new Cliente
                {
                    DNI = dni,
                    Nombre = this.FindControl<TextBox>("TxtNombre").Text,
                    Telefono = this.FindControl<TextBox>("TxtTelefono").Text,
                    Email = this.FindControl<TextBox>("TxtEmail").Text,
                    Direccion = this.FindControl<TextBox>("TxtDireccion").Text
                };
                HotelManager.Clientes.Add(cliente);
            }
            else
            {
                if (_clienteSeleccionado.DNI != dni && HotelManager.Clientes.Exists(c => c.DNI == dni))
                {
                    this.FindControl<TextBlock>("TxtError").Text = "Ya existe un cliente con ese DNI.";
                    return;
                }

                // Editar existente
                _clienteSeleccionado.DNI = dni;
                _clienteSeleccionado.Nombre = this.FindControl<TextBox>("TxtNombre").Text;
                _clienteSeleccionado.Telefono = this.FindControl<TextBox>("TxtTelefono").Text;
                _clienteSeleccionado.Email = this.FindControl<TextBox>("TxtEmail").Text;
                _clienteSeleccionado.Direccion = this.FindControl<TextBox>("TxtDireccion").Text;
            }

            HotelManager.GuardarDatos();
            ActualizarLista();
            LimpiarFormulario();
        }

        private bool EsDniValido(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni) || dni.Length != 9) return false;
            
            string numeros = dni.Substring(0, 8);
            char letra = char.ToUpper(dni[8]);
            
            if (!int.TryParse(numeros, out int n)) return false;
            
            string letras = "TRWAGMYFPDXBNJZSQVHLCKE";
            char letraCorrecta = letras[n % 23];
            
            return letra == letraCorrecta;
        }
    }
}