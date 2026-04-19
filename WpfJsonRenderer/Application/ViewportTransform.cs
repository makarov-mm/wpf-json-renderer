namespace WpfJsonRenderer.Application
{
    public sealed class ViewportTransform
    {
        public double Scale { get; }
        public double OffsetX { get; }
        public double OffsetY { get; }

        public ViewportTransform(double scale, double offsetX, double offsetY)
        {
            Scale = scale;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }
    }
}
