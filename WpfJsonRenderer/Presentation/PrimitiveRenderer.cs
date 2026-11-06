using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

/// <summary>Strongly typed base class for renderers of <typeparamref name="TPrimitive"/>.</summary>
public abstract class PrimitiveRenderer<TPrimitive> : IPrimitiveRenderer
    where TPrimitive : Primitive
{
    Type IPrimitiveRenderer.PrimitiveType => typeof(TPrimitive);

    void IPrimitiveRenderer.Render(Primitive primitive, RenderContext context) => Render((TPrimitive)primitive, context);

    protected abstract void Render(TPrimitive primitive, RenderContext context);
}
