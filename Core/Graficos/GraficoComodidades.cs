using System.Collections.Generic;
using System.Linq;

namespace TRABAJO_GRUPAL_AVALONIA.Core.Graficos
{
    public class GraficoComodidades : GraficoBase
    {
        public override void Generar(string periodo, string parametro)
        {
            // Contar cuántas habitaciones tienen cada comodidad
            // Las comodidades están en HotelManager.Habitaciones -> Comodidades (List<Comodidad>)
            
            var conteo = new Dictionary<string, int>();

            foreach (var hab in HotelManager.Habitaciones)
            {
                foreach (var com in hab.Comodidades)
                {
                    if (conteo.ContainsKey(com.Nombre))
                        conteo[com.Nombre]++;
                    else
                        conteo[com.Nombre] = 1;
                }
            }

            var etiquetas = conteo.Keys.ToList();
            var valores = conteo.Values.Select(v => (double)v).ToList();

            DibujarBarras(etiquetas, valores, "Habitaciones por Comodidad");
        }
    }
}
