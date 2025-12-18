using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;
using System.Linq;

namespace TRABAJO_GRUPAL_AVALONIA.Core.Graficos
{
    public abstract class GraficoBase
    {
        protected Canvas _canvas;

        public void SetPlotControl(Canvas canvas)
        {
            _canvas = canvas;
        }

        public abstract void Generar(string periodo, string parametro);

        // Métodos auxiliares para dibujar
        protected void DibujarBarras(List<string> etiquetas, List<double> valores, string tituloEjeY)
        {
            if (_canvas == null) return;
            _canvas.Children.Clear();

            if (valores.Count == 0) return;

            double width = _canvas.Bounds.Width;
            double height = _canvas.Bounds.Height;
            double margin = 40;
            
            double chartWidth = width - 2 * margin;
            double chartHeight = height - 2 * margin;

            double maxVal = valores.Max();
            if (maxVal == 0) maxVal = 1;

            double barWidth = (chartWidth / valores.Count) * 0.6;
            double spacing = (chartWidth / valores.Count) * 0.4;

            // Ejes
            var ejeX = new Line
            {
                StartPoint = new Avalonia.Point(margin, height - margin),
                EndPoint = new Avalonia.Point(width - margin, height - margin),
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            _canvas.Children.Add(ejeX);

            var ejeY = new Line
            {
                StartPoint = new Avalonia.Point(margin, height - margin),
                EndPoint = new Avalonia.Point(margin, margin),
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            _canvas.Children.Add(ejeY);

            // Dibujar barras
            for (int i = 0; i < valores.Count; i++)
            {
                double val = valores[i];
                double barHeight = (val / maxVal) * chartHeight;

                double x = margin + (i * (barWidth + spacing)) + (spacing / 2);
                double y = (height - margin) - barHeight;

                var rect = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = Brushes.CornflowerBlue,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                _canvas.Children.Add(rect);

                // Etiqueta valor
                var txtVal = new TextBlock
                {
                    Text = val.ToString(),
                    FontSize = 10,
                    Foreground = Brushes.Black
                };
                Canvas.SetLeft(txtVal, x + (barWidth / 2) - 5);
                Canvas.SetTop(txtVal, y - 15);
                _canvas.Children.Add(txtVal);

                // Etiqueta eje X
                var txtLabel = new TextBlock
                {
                    Text = etiquetas[i],
                    FontSize = 10,
                    Foreground = Brushes.Black,
                    TextWrapping = TextWrapping.Wrap,
                    Width = barWidth,
                    TextAlignment = TextAlignment.Center
                };
                Canvas.SetLeft(txtLabel, x);
                Canvas.SetTop(txtLabel, height - margin + 5);
                _canvas.Children.Add(txtLabel);
            }
        }
        
        public virtual IEnumerable<string> ObtenerClientes() => new List<string>();
        public virtual IEnumerable<string> ObtenerHabitaciones() => new List<string>();
    }
}
