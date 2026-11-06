namespace WpfJsonRenderer.Domain;

/// <summary>A line segment from <see cref="A"/> to <see cref="B"/>.</summary>
public sealed record LinePrimitive(Point2D A, Point2D B, ArgbColor Color) : Primitive(Color)
{
    public override Rect2D Bounds => Rect2D.FromPoints(A, B);
}
