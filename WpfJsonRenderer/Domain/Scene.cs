namespace WpfJsonRenderer.Domain;

/// <summary>An immutable, ordered set of primitives (later ones are drawn on top).</summary>
public sealed class Scene
{
    public Scene(IEnumerable<Primitive> primitives)
    {
        Primitives = [.. primitives];
        Bounds = ComputeBounds(Primitives);
    }

    public IReadOnlyList<Primitive> Primitives { get; }

    /// <summary>Bounding box of all primitives, computed once; an empty rectangle for an empty scene.</summary>
    public Rect2D Bounds { get; }

    public bool IsEmpty => Primitives.Count == 0;

    private static Rect2D ComputeBounds(IReadOnlyList<Primitive> primitives)
    {
        if (primitives.Count == 0)
        {
            return default;
        }

        Rect2D bounds = primitives[0].Bounds;

        for (int i = 1; i < primitives.Count; i++)
        {
            bounds = bounds.Union(primitives[i].Bounds);
        }

        return bounds;
    }
}
