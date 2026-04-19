using System.Windows.Controls;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation
{
    public interface IPrimitiveRenderer
    {
        bool CanRender(PrimitiveBase primitive);
        void Render(PrimitiveBase primitive, Canvas canvas, CoordinateTransformer transformer);
    }
}
