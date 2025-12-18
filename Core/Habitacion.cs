using System;
using System.Collections.Generic;

namespace TRABAJO_GRUPAL_AVALONIA.Core
{
    public enum TipoHabitacion { Matrimonial, Doble, Individual }

    public class Habitacion
    {
        // Identificador de 3 dígitos (pnn)
        public int Numero { get; set; } 
        public TipoHabitacion Tipo { get; set; }
        public DateTime UltimaRenovacion { get; set; }
        public DateTime UltimaReserva { get; set; } 
        public List<Comodidad> Comodidades { get; set; } = new List<Comodidad>(); 

        [System.Xml.Serialization.XmlIgnore]
        public string ComodidadesString 
        {
            get 
            {
                if (Comodidades == null || Comodidades.Count == 0) return "Sin comodidades";
                var nombres = new List<string>();
                foreach(var c in Comodidades) nombres.Add(c.Nombre);
                return string.Join(", ", nombres);
            }
        }

        // Constructor vacío para serialización XML
        public Habitacion() { }

        public Habitacion(int numero, TipoHabitacion tipo)
        {
            if (!EsNumeroValido(numero))
                throw new ArgumentException("El número debe ser de 3 dígitos (pnn) y los dos últimos entre 01-99.");
            
            Numero = numero;
            Tipo = tipo;
        }

        // Valida la regla pnn: piso (p) y número (nn) entre 01 y 99
        public static bool EsNumeroValido(int numero)
        {
            if (numero < 101 || numero > 999) return false;
            int nn = numero % 100;
            return nn >= 1 && nn <= 99;
        }

        public int ObtenerPiso() => Numero / 100; 
    }
}