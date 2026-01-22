namespace TRABAJO_GRUPAL_AVALONIA.Core
{
    public class Cliente
    {
        public required string DNI { get; set; }
        public required string Nombre { get; set; }
        public required string Telefono { get; set; }
        public required string Email { get; set; }
        public required string Direccion { get; set; }

        public Cliente() { }
    }
}