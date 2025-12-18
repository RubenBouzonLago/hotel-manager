using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TRABAJO_GRUPAL_AVALONIA.Core
{
    // Esta clase actuará como repositorio global para simplificar
    public static class HotelManager
    {
        public static List<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
        public static List<Cliente> Clientes { get; set; } = new List<Cliente>();
        public static List<Reserva> Reservas { get; set; } = new List<Reserva>();
        public static List<Comodidad> ComodidadesDisponibles { get; set; } = new List<Comodidad>();

        private const string FILE_HABITACIONES = "habitaciones.xml";
        private const string FILE_CLIENTES = "clientes.xml";
        private const string FILE_COMODIDADES = "comodidades.xml";
        private const string FILE_RESERVAS = "reservas.xml";

        public static void GuardarDatos()
        {
            GuardarXml(FILE_HABITACIONES, Habitaciones);
            GuardarXml(FILE_CLIENTES, Clientes);
            GuardarXml(FILE_COMODIDADES, ComodidadesDisponibles);
            GuardarXml(FILE_RESERVAS, Reservas);
        }

        public static void CargarDatos()
        {
            Habitaciones = CargarXml<List<Habitacion>>(FILE_HABITACIONES) ?? new List<Habitacion>();
            Clientes = CargarXml<List<Cliente>>(FILE_CLIENTES) ?? new List<Cliente>();
            ComodidadesDisponibles = CargarXml<List<Comodidad>>(FILE_COMODIDADES) ?? new List<Comodidad>();
            Reservas = CargarXml<List<Reserva>>(FILE_RESERVAS) ?? new List<Reserva>();
            
            // Datos por defecto si no hay comodidades
            if (ComodidadesDisponibles.Count == 0)
            {
                ComodidadesDisponibles.Add(new Comodidad("Wifi", "Internet de alta velocidad"));
                ComodidadesDisponibles.Add(new Comodidad("Caja Fuerte", "Seguridad para objetos de valor"));
                ComodidadesDisponibles.Add(new Comodidad("Mini-bar", "Bebidas y snacks"));
                ComodidadesDisponibles.Add(new Comodidad("Baño", "Baño privado completo"));
                ComodidadesDisponibles.Add(new Comodidad("Cocina", "Cocina equipada"));
                ComodidadesDisponibles.Add(new Comodidad("TV", "Televisión por cable"));
                GuardarDatos();
            }
        }

        // Métodos genéricos para XML 
        private static void GuardarXml<T>(string path, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, data);
            }
        }

        private static T CargarXml<T>(string path)
        {
            if (!File.Exists(path)) return default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StreamReader(path))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}