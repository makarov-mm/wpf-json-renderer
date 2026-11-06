using System.IO;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Infrastructure;

/// <summary>Reads a <see cref="Scene"/> from a file of one particular format.</summary>
public interface ISceneLoader
{
    /// <summary>Format name shown in the open file dialog, e.g. "JSON files".</summary>
    string FormatName { get; }

    /// <summary>File extensions handled by the loader, including the leading dot, e.g. ".json".</summary>
    IReadOnlyList<string> FileExtensions { get; }

    /// <summary>Loads the scene from <paramref name="filePath"/>.</summary>
    /// <exception cref="IOException">The file cannot be read.</exception>
    /// <exception cref="InvalidDataException">The file content is not a valid scene.</exception>
    Scene Load(string filePath);
}
