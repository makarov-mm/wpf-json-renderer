using System;
using System.Collections.Generic;
using System.Linq;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Presentation
{
    public sealed class PrimitiveRendererRegistry
    {
        private readonly List<IPrimitiveRenderer> _renderers;

        public PrimitiveRendererRegistry(IEnumerable<IPrimitiveRenderer> renderers)
        {
            if (renderers is null)
            {
                throw new ArgumentNullException(nameof(renderers));
            }

            _renderers = renderers.ToList();
        }

        public IPrimitiveRenderer GetRendererFor(PrimitiveBase primitive)
        {
            IPrimitiveRenderer renderer = _renderers.FirstOrDefault(r => r.CanRender(primitive));

            if (renderer is null)
            {
                throw new NotSupportedException($"No renderer found for primitive type: {primitive.GetType().Name}.");
            }

            return renderer;
        }
    }
}
