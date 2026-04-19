namespace WpfJsonRenderer.Domain
{
    public class CirclePrimitive : PrimitiveBase
    {
        public Point2D Center { get; }
        public double Radius { get; }
        public bool IsFilled { get; }

        public CirclePrimitive(Point2D center, double radius, bool isFilled, ArgbColor color)
            : base(color)
        {
            Center = center;
            Radius = radius;
            IsFilled = isFilled;
        }

        public override Rect2D GetBounds()
        {
            return new Rect2D(
                Center.X - Radius,
                Center.Y - Radius,
                Center.X + Radius,
                Center.Y + Radius
            );
        }
    }
}
