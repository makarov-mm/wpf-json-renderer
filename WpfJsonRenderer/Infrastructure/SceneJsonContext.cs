using System.Text.Json;
using System.Text.Json.Serialization;

namespace WpfJsonRenderer.Infrastructure;

/// <summary>
/// JSON metadata for scene files, generated at compile time instead of using runtime reflection.
/// Reading is lenient: property names are case-insensitive, comments and trailing commas are allowed.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    AllowTrailingCommas = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(JsonPrimitiveDto[]))]
internal sealed partial class SceneJsonContext : JsonSerializerContext;
