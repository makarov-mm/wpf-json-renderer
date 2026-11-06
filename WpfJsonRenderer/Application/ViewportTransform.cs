using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Application;

/// <summary>
/// Maps scene coordinates (Y axis up) to viewport coordinates (Y axis down):
/// <c>x' = x * Scale + OffsetX</c>, <c>y' = -y * Scale + OffsetY</c>.
/// </summary>
public readonly record struct ViewportTransform(double Scale, double OffsetX, double OffsetY)
{
    /// <summary>Margin kept between the scene and the viewport edges, in viewport units.</summary>
    private const double Padding = 20.0;

    /// <summary>Scenes are only scaled down to fit; smaller scenes are shown at their natural size.</summary>
    private const double MaxScale = 1.0;

    public static ViewportTransform Identity => new(1.0, 0.0, 0.0);

    public Point2D Apply(Point2D point) => new(point.X * Scale + OffsetX, -point.Y * Scale + OffsetY);

    public double ScaleLength(double length) => length * Scale;

    /// <summary>
    /// Returns the transform that centers <paramref name="bounds"/> in a viewport of the given size,
    /// scaled uniformly (aspect ratio preserved) so that the scene fits inside the padding.
    /// </summary>
    public static ViewportTransform Fit(Rect2D bounds, double viewportWidth, double viewportHeight)
    {
        if (viewportWidth <= 0 || viewportHeight <= 0)
        {
            return Identity;
        }

        double scale = Math.Min(
            Math.Min(
                WithoutPadding(viewportWidth) / NonZero(bounds.Width),
                WithoutPadding(viewportHeight) / NonZero(bounds.Height)),
            MaxScale);

        Point2D center = bounds.Center;

        return new ViewportTransform(
            scale,
            viewportWidth / 2 - center.X * scale,
            viewportHeight / 2 + center.Y * scale);

        // Tiny viewports are used in full rather than collapsing to a negative size.
        static double WithoutPadding(double size) => size > 2 * Padding ? size - 2 * Padding : size;

        // A degenerate extent (e.g. a single horizontal line) must not cause a division by zero.
        static double NonZero(double extent) => extent > 0 ? extent : 1.0;
    }
}
