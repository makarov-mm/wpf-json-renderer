using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using WpfJsonRenderer.Domain;
using WpfJsonRenderer.Infrastructure;

namespace WpfJsonRenderer.Presentation;

public partial class MainWindow : Window
{
    public static readonly RoutedUICommand ResetZoomCommand = new("Reset Zoom", "ResetZoom", typeof(MainWindow));

    private readonly SceneLoaderRegistry _loaders;
    private readonly string _openFileFilter;
    private bool _isLoading;

    public MainWindow(SceneLoaderRegistry loaders, PrimitiveRendererRegistry renderers)
    {
        _loaders = loaders;
        _openFileFilter = BuildOpenFileFilter(loaders.Loaders);

        InitializeComponent();
        SceneView.Renderers = renderers;
    }

    /// <summary>Loads the scene on a background thread and shows it; errors are reported to the user.</summary>
    public async Task OpenSceneAsync(string filePath)
    {
        if (_isLoading)
        {
            return;
        }

        string fileName = Path.GetFileName(filePath);
        string previousStatus = StatusText.Text;

        _isLoading = true;
        StatusText.Text = $"Loading {fileName}...";

        try
        {
            ISceneLoader loader = _loaders.FindLoader(filePath)
                ?? throw new NotSupportedException($"'{fileName}' is not a supported scene file.");

            Scene scene = await Task.Run(() => loader.Load(filePath));

            SceneView.Scene = scene;
            SceneView.ResetZoom();
            StatusText.Text = $"Loaded: {fileName}  |  Objects: {scene.Primitives.Count}";
        }
        catch (Exception ex)
        {
            StatusText.Text = previousStatus;
            MessageBox.Show(this, ex.Message, "Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _isLoading = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    protected override void OnDragOver(DragEventArgs e)
    {
        base.OnDragOver(e);

        e.Effects = TryGetDroppedSceneFile(e, out _) ? DragDropEffects.Copy : DragDropEffects.None;
        e.Handled = true;
    }

    protected override async void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);

        if (TryGetDroppedSceneFile(e, out string? filePath))
        {
            e.Handled = true;
            await OpenSceneAsync(filePath);
        }
    }

    /// <summary>Accepts exactly one dropped file of a supported format, and only while nothing is loading.</summary>
    private bool TryGetDroppedSceneFile(DragEventArgs e, [NotNullWhen(true)] out string? filePath)
    {
        if (!_isLoading
            && e.Data.GetData(DataFormats.FileDrop) is string[] and [var file]
            && _loaders.FindLoader(file) is not null)
        {
            filePath = file;
            return true;
        }

        filePath = null;
        return false;
    }

    private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = !_isLoading;

    private async void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var dialog = new OpenFileDialog { Filter = _openFileFilter };

        if (dialog.ShowDialog(this) == true)
        {
            await OpenSceneAsync(dialog.FileName);
        }
    }

    private void ZoomInCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SceneView.ZoomIn();

    private void ZoomOutCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SceneView.ZoomOut();

    private void ResetZoomCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SceneView.ResetZoom();

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Close();

    private void AboutMenuItem_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show(
            this,
            "Created by Mykhailo Makarov (m.m.makarov@gmail.com)",
            "About",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

    /// <summary>Builds a file dialog filter with one entry per loader, plus "all supported" when there are several.</summary>
    private static string BuildOpenFileFilter(IReadOnlyList<ISceneLoader> loaders)
    {
        List<string> filters = [.. loaders.Select(loader => Filter(loader.FormatName, loader.FileExtensions))];

        if (loaders.Count > 1)
        {
            filters.Insert(0, Filter("All supported files", loaders.SelectMany(loader => loader.FileExtensions)));
        }

        return string.Join('|', filters);

        static string Filter(string name, IEnumerable<string> extensions)
        {
            string patterns = string.Join(';', extensions.Select(extension => $"*{extension}"));
            return $"{name} ({patterns})|{patterns}";
        }
    }
}
