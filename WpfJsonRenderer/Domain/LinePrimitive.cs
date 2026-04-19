namespace WpfJsonRenderer.Domain
{
    public class LinePrimitive : PrimitiveBase
    {
        public Point2D A { get; }
        public Point2D B { get; }

        public LinePrimitive(Point2D a, Point2D b, ArgbColor color)
            : base(color)
        {
            A = a;
            B = b;
        }

        public override Rect2D GetBounds()
        {
            return Rect2D.FromPoints(A, B);
        }
    }
}
