using System.Windows;
using WpfJsonRenderer.Infrastructure;
using WpfJsonRenderer.Presentation;

namespace WpfJsonRenderer;

/// <summary>
/// Composition root: registers the scene loaders and primitive renderers and opens the main window.
/// </summary>
public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var loaders = new SceneLoaderRegistry([new JsonSceneLoader()]);
        var renderers = new PrimitiveRendererRegistry([new LineRenderer(), new CircleRenderer(), new TriangleRenderer()]);

        var mainWindow = new MainWindow(loaders, renderers);
        mainWindow.Show();

        // A file passed on the command line ("Open with..." in Explorer) is opened right away.
        if (e.Args is [var filePath, ..])
        {
            _ = mainWindow.OpenSceneAsync(filePath);
        }
    }
}
