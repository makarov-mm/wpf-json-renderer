using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Application
{
    public class ViewportTransformCalculator
    {
        private const double Padding = 20.0;

        public ViewportTransform Calculate(Rect2D bounds, double viewportWidth, double viewportHeight)
        {
            if (viewportWidth <= 0 || viewportHeight <= 0)
            {
                return new ViewportTransform(1.0, 0.0, 0.0);
            }

            double contentWidth = bounds.Width;
            double contentHeight = bounds.Height;

            if (contentWidth <= 0) contentWidth = 1;
            if (contentHeight <= 0) contentHeight = 1;

            double availableWidth = viewportWidth - 2 * Padding;
            double availableHeight = viewportHeight - 2 * Padding;

            if (availableWidth <= 0) availableWidth = viewportWidth;
            if (availableHeight <= 0) availableHeight = viewportHeight;

            double scaleX = availableWidth / contentWidth;
            double scaleY = availableHeight / contentHeight;
            double scale = scaleX < scaleY ? scaleX : scaleY;

            if (scale > 1.0)
                scale = 1.0;

            double renderedWidth = contentWidth * scale;
            double renderedHeight = contentHeight * scale;

            double offsetX = (viewportWidth - renderedWidth) / 2.0 - bounds.MinX * scale;
            double offsetY = (viewportHeight - renderedHeight) / 2.0 + bounds.MaxY * scale;

            return new ViewportTransform(scale, offsetX, offsetY);
        }
    }
}
