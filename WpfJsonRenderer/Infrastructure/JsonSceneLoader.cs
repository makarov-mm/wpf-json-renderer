using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Infrastructure
{
    public class JsonSceneLoader : ISceneLoader
    {
        public string DisplayName => "JSON...";
        public string FileDialogFilter => "JSON files (*.json)|*.json";

        public bool CanLoad(string filePath)
        {
            return filePath != null &&
                   filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase);
        }

        public Scene Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<JsonPrimitiveDto> items = JsonConvert.DeserializeObject<List<JsonPrimitiveDto>>(json);

            if (items is null)
            {
                throw new InvalidOperationException("Unable to deserialize scene.");
            }

            return new Scene(items.Select(CreatePrimitive).ToList());
        }

        private PrimitiveBase CreatePrimitive(JsonPrimitiveDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            string type = (dto.type ?? string.Empty).Trim().ToLowerInvariant();
            ArgbColor color = ColorParser.Parse(dto.color);

            switch (type)
            {
                case "line":
                    return new LinePrimitive(
                        CoordinateParser.ParsePoint(dto.a),
                        CoordinateParser.ParsePoint(dto.b),
                        color);

                case "circle":
                    return new CirclePrimitive(
                        CoordinateParser.ParsePoint(dto.center),
                        dto.radius ?? 0.0,
                        dto.filled ?? false,
                        color);

                case "triangle":
                    return new TrianglePrimitive(
                        CoordinateParser.ParsePoint(dto.a),
                        CoordinateParser.ParsePoint(dto.b),
                        CoordinateParser.ParsePoint(dto.c),
                        dto.filled ?? false,
                        color);

                default:
                    throw new NotSupportedException($"Unsupported primitive type: {dto.type}.");
            }
        }
    }
}
