using System;
using System.Collections.Generic;
using System.Linq;

namespace TRABAJO_GRUPAL_AVALONIA.Core.Graficos
{
    public class GraficoOcupacionGeneral : GraficoBase
    {
        public override void Generar(string periodo, string parametro)
        {
            var reservas = HotelManager.Reservas;
            var etiquetas = new List<string>();
            var valores = new List<double>();

            if (periodo == "Mes a Mes")
            {
                // Agrupar por mes del año actual (o últimos 12 meses)
                // Simplificación: Agrupar por Mes/Año de FechaEntrada
                var agrupado = reservas
                    .GroupBy(r => r.FechaEntrada.ToString("MMM yy"))
                    .Select(g => new { Mes = g.Key, Cantidad = g.Count() })
                    .ToList();

                etiquetas = agrupado.Select(x => x.Mes).ToList();
                valores = agrupado.Select(x => (double)x.Cantidad).ToList();
            }
            else // Total
            {
                etiquetas.Add("Total Reservas");
                valores.Add(reservas.Count);
            }

            DibujarBarras(etiquetas, valores, "Reservas");
        }
    }
}
