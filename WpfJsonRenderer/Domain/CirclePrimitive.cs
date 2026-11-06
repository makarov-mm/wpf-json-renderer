namespace WpfJsonRenderer.Domain;

/// <summary>A circle, outlined or filled with <see cref="Primitive.Color"/>.</summary>
public sealed record CirclePrimitive(Point2D Center, double Radius, bool IsFilled, ArgbColor Color) : Primitive(Color)
{
    public override Rect2D Bounds => new(Center.X - Radius, Center.Y - Radius, Center.X + Radius, Center.Y + Radius);
}
