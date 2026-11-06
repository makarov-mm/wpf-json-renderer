using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

/// <summary>Dispatches each primitive to the renderer registered for its type.</summary>
public sealed class PrimitiveRendererRegistry(IEnumerable<IPrimitiveRenderer> renderers)
{
    private readonly Dictionary<Type, IPrimitiveRenderer> _renderers = renderers.ToDictionary(renderer => renderer.PrimitiveType);

    public void Render(Primitive primitive, RenderContext context)
    {
        if (!_renderers.TryGetValue(primitive.GetType(), out IPrimitiveRenderer? renderer))
        {
            throw new NotSupportedException($"No renderer is registered for {primitive.GetType().Name}.");
        }

        renderer.Render(primitive, context);
    }
}
