using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation;

/// <summary>
/// Draws a <see cref="Domain.Scene"/> fitted into the control and magnified by <see cref="Zoom"/>
/// around the control's center. Primitives are drawn directly into the <see cref="DrawingContext"/>,
/// so a scene of any size costs no UI elements and is re-fitted automatically on every resize.
/// </summary>
public sealed class SceneView : FrameworkElement
{
    public const double MinZoom = 0.1;
    public const double MaxZoom = 20.0;

    /// <summary>Zoom factor of one zoom command or one mouse wheel notch.</summary>
    private const double ZoomStep = 1.1;

    public static readonly DependencyProperty SceneProperty = DependencyProperty.Register(
        nameof(Scene),
        typeof(Scene),
        typeof(SceneView),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
        nameof(Zoom),
        typeof(double),
        typeof(SceneView),
        new FrameworkPropertyMetadata(
            1.0,
            FrameworkPropertyMetadataOptions.AffectsRender,
            propertyChangedCallback: null,
            coerceValueCallback: CoerceZoom));

    public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(
        typeof(SceneView),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public SceneView()
    {
        // Zoomed-in content must not spill over the neighboring controls.
        ClipToBounds = true;
    }

    public Scene? Scene
    {
        get => (Scene?)GetValue(SceneProperty);
        set => SetValue(SceneProperty, value);
    }

    /// <summary>Magnification on top of the fit-to-viewport scale, clamped to [<see cref="MinZoom"/>, <see cref="MaxZoom"/>].</summary>
    public double Zoom
    {
        get => (double)GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public Brush? Background
    {
        get => (Brush?)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <summary>Renderers for the scene primitives; nothing but the background is drawn until they are set.</summary>
    public PrimitiveRendererRegistry? Renderers
    {
        get;
        set
        {
            field = value;
            InvalidateVisual();
        }
    }

    public void ZoomIn() => Zoom *= ZoomStep;

    public void ZoomOut() => Zoom /= ZoomStep;

    public void ResetZoom() => Zoom = 1.0;

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        // High-resolution wheels and touchpads report fractions of a notch.
        Zoom *= Math.Pow(ZoomStep, (double)e.Delta / Mouse.MouseWheelDeltaForOneLine);
        e.Handled = true;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        Size size = RenderSize;

        // The transparent fallback keeps the whole area hit-testable (mouse wheel, drag and drop).
        drawingContext.DrawRectangle(Background ?? Brushes.Transparent, null, new Rect(size));

        if (Scene is not { IsEmpty: false } scene || Renderers is not { } renderers || size.Width <= 0 || size.Height <= 0)
        {
            return;
        }

        var context = new RenderContext(drawingContext, ViewportTransform.Fit(scene.Bounds, size.Width, size.Height));

        var zoom = new ScaleTransform(Zoom, Zoom, size.Width / 2, size.Height / 2);
        zoom.Freeze();

        drawingContext.PushTransform(zoom);

        foreach (Primitive primitive in scene.Primitives)
        {
            renderers.Render(primitive, context);
        }

        drawingContext.Pop();
    }

    private static object CoerceZoom(DependencyObject d, object baseValue) => Math.Clamp((double)baseValue, MinZoom, MaxZoom);
}
