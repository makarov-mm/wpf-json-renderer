namespace WpfJsonRenderer.Domain
{
    public struct Rect2D
    {
        public double MinX { get; }
        public double MinY { get; }
        public double MaxX { get; }
        public double MaxY { get; }

        public Rect2D(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public double Width => MaxX - MinX;
        public double Height => MaxY - MinY;

        public static Rect2D FromPoints(params Point2D[] points)
        {
            double minX = points[0].X;
            double minY = points[0].Y;
            double maxX = points[0].X;
            double maxY = points[0].Y;

            foreach (var p in points)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }

            return new Rect2D(minX, minY, maxX, maxY);
        }

        public Rect2D Expand(Rect2D other)
        {
            return new Rect2D(
                MinX < other.MinX ? MinX : other.MinX,
                MinY < other.MinY ? MinY : other.MinY,
                MaxX > other.MaxX ? MaxX : other.MaxX,
                MaxY > other.MaxY ? MaxY : other.MaxY
            );
        }
    }
}
