using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfJsonRenderer.Infrastructure
{
    public sealed class SceneLoaderRegistry
    {
        private readonly List<ISceneLoader> _loaders;

        public SceneLoaderRegistry(IEnumerable<ISceneLoader> loaders)
        {
            if (loaders is null)
            {
                throw new ArgumentNullException(nameof(loaders));
            }
            
            _loaders = loaders.ToList();
        }

        public IReadOnlyList<ISceneLoader> Loaders => _loaders;

        public ISceneLoader GetLoaderFor(string filePath)
        {
            ISceneLoader loader = _loaders.FirstOrDefault(x => x.CanLoad(filePath));
            
            if (loader is null)
            {
                throw new NotSupportedException("Unsupported file format.");
            }

            return loader;
        }
    }
}
