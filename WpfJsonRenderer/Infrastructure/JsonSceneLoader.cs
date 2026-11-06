using System.IO;
using System.Text.Json;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Infrastructure;

/// <summary>
/// Loads a scene from a JSON array of primitives, for example:
/// <code>
/// [
///   { "type": "line", "a": "-50; 0", "b": "50; 0", "color": "255; 255; 0; 0" },
///   { "type": "circle", "center": "0; 0", "radius": 30.5, "filled": true, "color": "128; 0; 0; 255" },
///   { "type": "triangle", "a": "0; 0", "b": "10,5; 0", "c": "0; 10", "filled": false, "color": "255; 0; 0; 0" }
/// ]
/// </code>
/// </summary>
public sealed class JsonSceneLoader : ISceneLoader
{
    public string FormatName => "JSON files";

    public IReadOnlyList<string> FileExtensions { get; } = [".json"];

    public Scene Load(string filePath)
    {
        JsonPrimitiveDto?[] items = ReadItems(filePath);
        var primitives = new Primitive[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            try
            {
                primitives[i] = CreatePrimitive(items[i]);
            }
            catch (FormatException ex)
            {
                throw new InvalidDataException($"Item #{i + 1}: {ex.Message}", ex);
            }
        }

        return new Scene(primitives);
    }

    private static JsonPrimitiveDto?[] ReadItems(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);

        try
        {
            return JsonSerializer.Deserialize(stream, SceneJsonContext.Default.JsonPrimitiveDtoArray)
                ?? throw new InvalidDataException("The file must contain a JSON array of primitives.");
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Invalid JSON: {ex.Message}", ex);
        }
    }

    private static Primitive CreatePrimitive(JsonPrimitiveDto? item)
    {
        if (item is null)
        {
            throw new FormatException("The item is null.");
        }

        return item.Type?.Trim().ToLowerInvariant() switch
        {
            "line" => new LinePrimitive(
                ReadPoint(item.A, "a"),
                ReadPoint(item.B, "b"),
                ReadColor(item.Color)),

            "circle" => new CirclePrimitive(
                ReadPoint(item.Center, "center"),
                ReadRadius(item.Radius),
                item.Filled ?? false,
                ReadColor(item.Color)),

            "triangle" => new TrianglePrimitive(
                ReadPoint(item.A, "a"),
                ReadPoint(item.B, "b"),
                ReadPoint(item.C, "c"),
                item.Filled ?? false,
                ReadColor(item.Color)),

            null => throw Missing("type"),
            _ => throw new FormatException($"Unknown primitive type '{item.Type}'. Supported types: line, circle, triangle."),
        };
    }

    private static Point2D ReadPoint(string? value, string property) =>
        SceneValueParser.TryParsePoint(value ?? throw Missing(property), out Point2D point)
            ? point
            : throw Invalid(property, value, "X; Y");

    private static ArgbColor ReadColor(string? value) =>
        SceneValueParser.TryParseColor(value ?? throw Missing("color"), out ArgbColor color)
            ? color
            : throw Invalid("color", value, "A; R; G; B (0-255 each)");

    private static double ReadRadius(double? value) => value switch
    {
        null => throw Missing("radius"),
        double radius when radius >= 0 && double.IsFinite(radius) => radius,
        _ => throw new FormatException($"Property 'radius' must be a non-negative number, but was {value}."),
    };

    private static FormatException Missing(string property) => new($"Property '{property}' is missing.");

    private static FormatException Invalid(string property, string value, string expectedFormat) =>
        new($"Property '{property}' has an invalid value '{value}'. Expected format: '{expectedFormat}'.");
}
