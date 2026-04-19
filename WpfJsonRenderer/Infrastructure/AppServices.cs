using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WpfJsonRenderer.Presentation;

namespace WpfJsonRenderer.Infrastructure
{
    public static class AppServices
    {
        public static SceneLoaderRegistry CreateSceneLoaderRegistry()
        {
            IEnumerable<Type> loaders = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
                .Where(t =>
                {
                    Type[] interfaces = t.GetInterfaces();
                    return interfaces.Any(i => i == typeof(ISceneLoader));
                });
            
            ISceneLoader[] loaderInstances = loaders
                .Select(loaderType => (ISceneLoader)Activator.CreateInstance(loaderType))
                .ToArray();
            
            return new SceneLoaderRegistry(loaderInstances);
        }

        public static PrimitiveRendererRegistry CreatePrimitiveRendererRegistry()
        {
            IEnumerable<Type> renderers = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IPrimitiveRenderer)));

            IPrimitiveRenderer[] rendererInstances = renderers
                .Select(t => (IPrimitiveRenderer)Activator.CreateInstance(t))
                .ToArray();

            return new PrimitiveRendererRegistry(rendererInstances);
        }
    }
}
