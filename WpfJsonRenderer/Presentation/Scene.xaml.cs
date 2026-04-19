using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Infrastructure;

namespace WpfJsonRenderer.Presentation
{
    public partial class Scene : UserControl
    {
        private const double ZoomStep = 1.1;
        private const double ZoomMin = 0.1;
        private const double ZoomMax = 20.0;

        private readonly PrimitiveRendererRegistry _rendererRegistry;
        private Matrix _matrix = Matrix.Identity;

        public event EventHandler<double> ZoomChanged;

        public double CurrentZoom => _matrix.M11;

        public Scene()
        {
            InitializeComponent();
            _rendererRegistry = AppServices.CreatePrimitiveRendererRegistry();
            PreviewMouseWheel += OnMouseWheel;
        }

        public void Render(Domain.Scene scene, ViewportTransform transform)
        {
            CanvasRoot.Children.Clear();

            var transformer = new CoordinateTransformer(transform);

            foreach (var primitive in scene.Primitives)
            {
                IPrimitiveRenderer renderer = _rendererRegistry.GetRendererFor(primitive);
                renderer.Render(primitive, CanvasRoot, transformer);
            }
        }

        public void ResetZoom()
        {
            _matrix = Matrix.Identity;
            ApplyTransform();
            ZoomChanged?.Invoke(this, 1.0);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ApplyZoom(e.Delta > 0 ? ZoomStep : 1.0 / ZoomStep);
            e.Handled = true;
        }

        private void ApplyTransform()
        {
            ViewTransform.Matrix = _matrix;
        }

        public void ZoomIn()
        {
            ApplyZoom(ZoomStep);
        }

        public void ZoomOut()
        {
            ApplyZoom(1.0 / ZoomStep);
        }

        private void ApplyZoom(double factor)
        {
            double newZoom = Math.Max(ZoomMin, Math.Min(ZoomMax, _matrix.M11 * factor));
            factor = newZoom / _matrix.M11;
            Point center = new Point(ActualWidth / 2.0, ActualHeight / 2.0);
            _matrix.ScaleAtPrepend(factor, factor, center.X, center.Y);
            ApplyTransform();
            ZoomChanged?.Invoke(this, _matrix.M11);
        }
    }
}
