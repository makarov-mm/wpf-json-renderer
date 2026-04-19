using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation
{
    public sealed class LineRenderer : PrimitiveRendererBase
    {
        public override bool CanRender(PrimitiveBase primitive) => primitive is LinePrimitive;

        public override void Render(PrimitiveBase primitive, Canvas canvas, CoordinateTransformer transformer)
        {
            var line = (LinePrimitive)primitive;

            Point a = transformer.Transform(line.A);
            Point b = transformer.Transform(line.B);

            var shape = new Line
            {
                X1 = a.X,
                Y1 = a.Y,
                X2 = b.X,
                Y2 = b.Y,
                Stroke = ToBrush(line.Color),
                StrokeThickness = 1
            };

            canvas.Children.Add(shape);
        }
    }
}
