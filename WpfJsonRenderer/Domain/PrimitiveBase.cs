namespace WpfJsonRenderer.Domain
{
    public abstract class PrimitiveBase
    {
        public ArgbColor Color { get; }

        protected PrimitiveBase(ArgbColor color)
        {
            Color = color;
        }

        public abstract Rect2D GetBounds();
    }
}
