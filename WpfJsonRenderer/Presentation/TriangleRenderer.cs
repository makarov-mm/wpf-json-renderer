using System.Windows.Media;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

public sealed class TriangleRenderer : PrimitiveRenderer<TrianglePrimitive>
{
    protected override void Render(TrianglePrimitive triangle, RenderContext context)
    {
        var geometry = new StreamGeometry();

        using (StreamGeometryContext figure = geometry.Open())
        {
            figure.BeginFigure(context.ToViewport(triangle.A), isFilled: triangle.IsFilled, isClosed: true);
            figure.LineTo(context.ToViewport(triangle.B), isStroked: true, isSmoothJoin: false);
            figure.LineTo(context.ToViewport(triangle.C), isStroked: true, isSmoothJoin: false);
        }

        geometry.Freeze();

        context.DrawingContext.DrawGeometry(
            triangle.IsFilled ? context.GetFill(triangle.Color) : null,
            context.GetStroke(triangle.Color),
            geometry);
    }
}
