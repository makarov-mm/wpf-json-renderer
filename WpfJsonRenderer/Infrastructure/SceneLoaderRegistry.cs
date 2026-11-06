using System.IO;

namespace WpfJsonRenderer.Infrastructure;

/// <summary>The available scene loaders; a loader is chosen by file extension.</summary>
public sealed class SceneLoaderRegistry(IEnumerable<ISceneLoader> loaders)
{
    public IReadOnlyList<ISceneLoader> Loaders { get; } = [.. loaders];

    /// <summary>Returns the loader for the file's extension, or <see langword="null"/> if the format is not supported.</summary>
    public ISceneLoader? FindLoader(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        return Loaders.FirstOrDefault(loader => loader.FileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase));
    }
}
