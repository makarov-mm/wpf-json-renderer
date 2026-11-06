namespace WpfJsonRenderer.Domain;

/// <summary>Base type of all scene primitives.</summary>
public abstract record Primitive(ArgbColor Color)
{
    /// <summary>Axis-aligned bounding box of the primitive's geometry.</summary>
    public abstract Rect2D Bounds { get; }
}
