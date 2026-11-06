namespace WpfJsonRenderer.Domain;

/// <summary>An axis-aligned rectangle in scene coordinates.</summary>
public readonly record struct Rect2D(double MinX, double MinY, double MaxX, double MaxY)
{
    public double Width => MaxX - MinX;

    public double Height => MaxY - MinY;

    public Point2D Center => new((MinX + MaxX) / 2, (MinY + MaxY) / 2);

    /// <summary>Returns the smallest rectangle that contains all <paramref name="points"/>.</summary>
    public static Rect2D FromPoints(params ReadOnlySpan<Point2D> points)
    {
        if (points.IsEmpty)
        {
            throw new ArgumentException("At least one point is required.", nameof(points));
        }

        double minX = points[0].X, maxX = minX;
        double minY = points[0].Y, maxY = minY;

        foreach (Point2D point in points[1..])
        {
            minX = Math.Min(minX, point.X);
            maxX = Math.Max(maxX, point.X);
            minY = Math.Min(minY, point.Y);
            maxY = Math.Max(maxY, point.Y);
        }

        return new Rect2D(minX, minY, maxX, maxY);
    }

    /// <summary>Returns the smallest rectangle that contains both this rectangle and <paramref name="other"/>.</summary>
    public Rect2D Union(Rect2D other) => new(
        Math.Min(MinX, other.MinX),
        Math.Min(MinY, other.MinY),
        Math.Max(MaxX, other.MaxX),
        Math.Max(MaxY, other.MaxY));
}
