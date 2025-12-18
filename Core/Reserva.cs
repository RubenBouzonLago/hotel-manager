using System;

namespace TRABAJO_GRUPAL_AVALONIA.Core
{
    public class Reserva
    {
        public string IdReserva { get; set; }
        public string ClienteDNI { get; set; }
        public int NumeroHabitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public bool Garaje { get; set; } 
        public decimal ImportePorDia { get; set; }
        public decimal IVA { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public string NombreCliente 
        {
            get 
            {
                var cliente = HotelManager.Clientes.Find(c => c.DNI == ClienteDNI);
                return cliente != null ? cliente.Nombre : "Desconocido";
            }
        }

        public Reserva() { }

        // Método para generar ID único: aaaammddhhh 
        public void GenerarId()
        {
            IdReserva = $"{FechaEntrada:yyyyMMdd}{NumeroHabitacion:000}";
        }
    }
}