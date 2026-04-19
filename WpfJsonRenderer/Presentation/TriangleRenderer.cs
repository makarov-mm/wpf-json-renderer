using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation
{
    public sealed class TriangleRenderer : PrimitiveRendererBase
    {
        public override bool CanRender(PrimitiveBase primitive) => primitive is TrianglePrimitive;

        public override void Render(PrimitiveBase primitive, Canvas canvas, CoordinateTransformer transformer)
        {
            var triangle = (TrianglePrimitive)primitive;

            Point a = transformer.Transform(triangle.A);
            Point b = transformer.Transform(triangle.B);
            Point c = transformer.Transform(triangle.C);

            var shape = new Polygon
            {
                Stroke = ToBrush(triangle.Color),
                StrokeThickness = 1,
                Fill = triangle.IsFilled ? ToBrush(triangle.Color) : Transparent,
            };

            shape.Points.Add(a);
            shape.Points.Add(b);
            shape.Points.Add(c);

            canvas.Children.Add(shape);
        }
    }
}
