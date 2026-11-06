using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

public sealed class LineRenderer : PrimitiveRenderer<LinePrimitive>
{
    protected override void Render(LinePrimitive line, RenderContext context) =>
        context.DrawingContext.DrawLine(
            context.GetStroke(line.Color),
            context.ToViewport(line.A),
            context.ToViewport(line.B));
}
