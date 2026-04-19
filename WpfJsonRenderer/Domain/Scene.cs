using System.Collections.Generic;

namespace WpfJsonRenderer.Domain
{
    public class Scene
    {
        public IReadOnlyList<PrimitiveBase> Primitives { get; }

        public Scene(IReadOnlyList<PrimitiveBase> primitives)
        {
            Primitives = primitives;
        }

        public Rect2D GetBounds()
        {
            if (Primitives.Count == 0)
            {
                return new Rect2D(0, 0, 0, 0);
            }

            Rect2D bounds = Primitives[0].GetBounds();

            for (int i = 1; i < Primitives.Count; i++)
            {
                bounds = bounds.Expand(Primitives[i].GetBounds());
            }

            return bounds;
        }
    }
}
