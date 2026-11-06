# Vector Scene Renderer

A simple 2D vector graphics viewer implemented in **C# 14 / WPF (.NET 10)**.

The application loads a scene description from a JSON file and renders basic geometric primitives such as lines, circles, and triangles.

[![GitHub Sponsors](https://img.shields.io/github/sponsors/makarov-mm?style=flat&logo=github)](https://github.com/sponsors/makarov-mm)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
![C#](https://img.shields.io/badge/C%23-239120)
![.NET](https://img.shields.io/badge/.NET-10-512BD4)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0A66C2?logo=linkedin&logoColor=white)](https://www.linkedin.com/in/makarov-mm/)
[![Threads](https://img.shields.io/badge/Threads-000000?logo=threads&logoColor=white)](https://www.threads.net/@m.m.makarov)
[![Instagram](https://img.shields.io/badge/Instagram-E4405F?logo=instagram&logoColor=white)](https://www.instagram.com/m.m.makarov/)
![Windows](https://img.shields.io/badge/Windows-0078D6?logo=windows&logoColor=white)

---

## Screenshot

![Application Screenshot](screenshot.png)

---

## Features

- Load a scene from a JSON file: **File → Open** (Ctrl+O), drag and drop onto the window, or pass the file on the command line
- Render:
  - Lines
  - Circles (with optional fill)
  - Triangles (with optional fill)
- Automatic fitting: scenes larger than the window are scaled down, smaller ones keep their natural size
- Aspect ratio preservation and centering
- Zoom with the mouse wheel or Ctrl++ / Ctrl+- (Ctrl+0 resets)
- Scenes are loaded in the background and drawn straight into a `DrawingContext`, so large files stay responsive
- Clear error messages for invalid files (which item, which property, what format was expected)
- Clean separation between domain logic and rendering
- Extensible architecture

---

## How to Run

Requirements: Windows and the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

1. Open `WpfJsonRenderer.slnx` in **Visual Studio 2026** (or Rider), or run `dotnet run --project WpfJsonRenderer`
2. Use **File → Open** to load a JSON scene, for example [`TestData.json`](TestData.json)

---

## Architecture Overview

The project is structured into several layers:

### Domain
- Immutable core types: `Point2D`, `ArgbColor`, `Rect2D`, the primitives and `Scene`
- No dependency on the UI framework

### Application
- `ViewportTransform`: fitting the scene into the viewport and converting coordinates

### Infrastructure
- Scene loading: `ISceneLoader`, `SceneLoaderRegistry`, `JsonSceneLoader` (System.Text.Json with source generation)
- Parsing of point and color values

### Presentation
- WPF UI: `MainWindow` and the `SceneView` control
- Rendering logic: one renderer per primitive type

`App.xaml.cs` is the composition root where loaders and renderers are registered.

---

## Rendering Details

The renderer performs:

- Conversion from Cartesian coordinates (Y-up) to screen coordinates (Y-down)
- Uniform scaling to fit the available window, keeping a 20 px margin
- Centering of the scene
- Zooming around the center of the view
- Drawing with WPF `DrawingContext` primitives and frozen, shared brushes and pens

---

## Extensibility

### Add new primitive

1. Add a record deriving from `Primitive` in the Domain layer
2. Add a `PrimitiveRenderer<T>` in Presentation and register it in `App.xaml.cs`
3. Read the new type in `JsonSceneLoader`

---

### Add new file format

1. Implement `ISceneLoader`
2. Register it in `App.xaml.cs`
3. The Open dialog and drag and drop will support it automatically

---

## Scene Format

A scene is a JSON array of primitives, drawn in order:

```json
[
  {
    "type": "line",
    "a": "-50; 0",
    "b": "50; 0",
    "color": "255; 255; 50; 50"
  },
  {
    "type": "circle",
    "center": "0; 0",
    "radius": 30.0,
    "filled": true,
    "color": "180; 50; 200; 50"
  },
  {
    "type": "triangle",
    "a": "35,0; 0,0",
    "b": "24,7; 24,7",
    "c": "64,7; 26,8",
    "filled": false,
    "color": "220; 255; 80; 80"
  }
]
```

| Type       | Properties                                   |
|------------|----------------------------------------------|
| `line`     | `a`, `b`, `color`                            |
| `circle`   | `center`, `radius`, `filled`, `color`        |
| `triangle` | `a`, `b`, `c`, `filled`, `color`             |

- Points are written as `"X; Y"`; the decimal separator may be `,` or `.` (`"12,5; -3"` or `"12.5; -3"`)
- Colors are written as `"A; R; G; B"` with components from 0 to 255
- `filled` is optional and defaults to `false`
- Property names are case-insensitive; comments and trailing commas are allowed

## License

MIT License

Copyright (c) 2026 Mykhailo Makarov

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Support

If you found this project interesting or useful, you can support my work:

[![GitHub Sponsors](https://img.shields.io/github/sponsors/makarov-mm?style=flat&logo=github)](https://github.com/sponsors/makarov-mm)
