using System.Windows;
using System.Windows.Media;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

/// <summary>
/// State shared by the primitive renderers during one render pass: the target <see cref="DrawingContext"/>,
/// the scene-to-viewport transform and frozen brushes and pens, created once per color.
/// </summary>
public sealed class RenderContext(DrawingContext drawingContext, ViewportTransform transform)
{
    private const double StrokeThickness = 1.0;

    private readonly Dictionary<ArgbColor, (Brush Fill, Pen Stroke)> _paints = [];

    public DrawingContext DrawingContext { get; } = drawingContext;

    public Point ToViewport(Point2D point)
    {
        Point2D transformed = transform.Apply(point);
        return new Point(transformed.X, transformed.Y);
    }

    public double ToViewportLength(double length) => transform.ScaleLength(length);

    public Brush GetFill(ArgbColor color) => GetPaint(color).Fill;

    public Pen GetStroke(ArgbColor color) => GetPaint(color).Stroke;

    private (Brush Fill, Pen Stroke) GetPaint(ArgbColor color)
    {
        if (!_paints.TryGetValue(color, out var paint))
        {
            var brush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            brush.Freeze();

            var pen = new Pen(brush, StrokeThickness);
            pen.Freeze();

            _paints[color] = paint = (brush, pen);
        }

        return paint;
    }
}
