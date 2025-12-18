using System.Collections.Generic;
using System.Linq;

namespace TRABAJO_GRUPAL_AVALONIA.Core.Graficos
{
    public class GraficoOcupacionCliente : GraficoBase
    {
        public override IEnumerable<string> ObtenerClientes()
        {
            return HotelManager.Clientes.Select(c => c.DNI);
        }

        public override void Generar(string periodo, string dniCliente)
        {
            if (string.IsNullOrEmpty(dniCliente)) return;

            var reservas = HotelManager.Reservas.Where(r => r.ClienteDNI == dniCliente).ToList();
            var etiquetas = new List<string>();
            var valores = new List<double>();

            if (periodo == "Mes a Mes")
            {
                var agrupado = reservas
                    .GroupBy(r => r.FechaEntrada.ToString("MMM yy"))
                    .Select(g => new { Mes = g.Key, Cantidad = g.Count() })
                    .ToList();

                etiquetas = agrupado.Select(x => x.Mes).ToList();
                valores = agrupado.Select(x => (double)x.Cantidad).ToList();
            }
            else
            {
                etiquetas.Add("Total");
                valores.Add(reservas.Count);
            }

            DibujarBarras(etiquetas, valores, "Reservas Cliente");
        }
    }
}
