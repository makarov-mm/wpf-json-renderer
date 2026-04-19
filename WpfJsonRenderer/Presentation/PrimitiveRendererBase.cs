using System.Windows.Media;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation
{
    public abstract class PrimitiveRendererBase : IPrimitiveRenderer
    {
        public abstract bool CanRender(PrimitiveBase primitive);
        public abstract void Render(PrimitiveBase primitive, System.Windows.Controls.Canvas canvas, Application.CoordinateTransformer transformer);

        protected static SolidColorBrush ToBrush(ArgbColor color)
        {
            return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        protected static SolidColorBrush Transparent => Brushes.Transparent;
    }
}
