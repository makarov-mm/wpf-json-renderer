namespace WpfJsonRenderer.Domain
{
    public class TrianglePrimitive : PrimitiveBase
    {
        public Point2D A { get; }
        public Point2D B { get; }
        public Point2D C { get; }
        public bool IsFilled { get; }

        public TrianglePrimitive(Point2D a, Point2D b, Point2D c, bool isFilled, ArgbColor color)
            : base(color)
        {
            A = a;
            B = b;
            C = c;
            IsFilled = isFilled;
        }

        public override Rect2D GetBounds()
        {
            return Rect2D.FromPoints(A, B, C);
        }
    }
}
