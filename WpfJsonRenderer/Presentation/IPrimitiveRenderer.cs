using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

/// <summary>Draws primitives of one type.</summary>
public interface IPrimitiveRenderer
{
    /// <summary>The exact primitive type this renderer draws.</summary>
    Type PrimitiveType { get; }

    void Render(Primitive primitive, RenderContext context);
}
