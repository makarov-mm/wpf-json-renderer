using System.Windows;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Application
{
    public sealed class CoordinateTransformer
    {
        private readonly ViewportTransform _transform;

        public CoordinateTransformer(ViewportTransform transform)
        {
            _transform = transform;
        }

        public Point Transform(Point2D point)
        {
            double x = point.X * _transform.Scale + _transform.OffsetX;
            double y = -point.Y * _transform.Scale + _transform.OffsetY;
            return new Point(x, y);
        }

        public double ScaleLength(double value)
        {
            return value * _transform.Scale;
        }
    }
}
