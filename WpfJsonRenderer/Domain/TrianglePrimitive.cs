namespace WpfJsonRenderer.Domain;

/// <summary>A triangle, outlined or filled with <see cref="Primitive.Color"/>.</summary>
public sealed record TrianglePrimitive(Point2D A, Point2D B, Point2D C, bool IsFilled, ArgbColor Color) : Primitive(Color)
{
    public override Rect2D Bounds => Rect2D.FromPoints(A, B, C);
}
