using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

public sealed class CircleRenderer : PrimitiveRenderer<CirclePrimitive>
{
    protected override void Render(CirclePrimitive circle, RenderContext context)
    {
        double radius = context.ToViewportLength(circle.Radius);

        context.DrawingContext.DrawEllipse(
            circle.IsFilled ? context.GetFill(circle.Color) : null,
            context.GetStroke(circle.Color),
            context.ToViewport(circle.Center),
            radius,
            radius);
    }
}
