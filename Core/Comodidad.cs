namespace TRABAJO_GRUPAL_AVALONIA.Core
{
    public class Comodidad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public Comodidad() { }

        public Comodidad(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }
        
        public override string ToString()
        {
            return Nombre;
        }
    }
}
