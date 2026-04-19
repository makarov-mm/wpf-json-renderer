using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation
{
    public sealed class CircleRenderer : PrimitiveRendererBase
    {
        public override bool CanRender(PrimitiveBase primitive) => primitive is CirclePrimitive;

        public override void Render(PrimitiveBase primitive, Canvas canvas, CoordinateTransformer transformer)
        {
            var circle = (CirclePrimitive)primitive;

            Point center = transformer.Transform(circle.Center);
            double radius = transformer.ScaleLength(circle.Radius);
            double diameter = radius * 2.0;

            var shape = new Ellipse
            {
                Width = diameter,
                Height = diameter,
                Stroke = ToBrush(circle.Color),
                StrokeThickness = 1,
                Fill = circle.IsFilled ? ToBrush(circle.Color) : Transparent
            };

            Canvas.SetLeft(shape, center.X - radius);
            Canvas.SetTop(shape, center.Y - radius);

            canvas.Children.Add(shape);
        }
    }
}
