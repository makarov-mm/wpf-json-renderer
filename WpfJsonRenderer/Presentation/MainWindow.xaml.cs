using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfJsonRenderer.Application;
using WpfJsonRenderer.Infrastructure;

namespace WpfJsonRenderer.Presentation
{
    public partial class MainWindow : Window
    {
        private readonly SceneLoaderRegistry _sceneLoaderRegistry;
        private Domain.Scene _currentScene;
        private string _currentSceneFileName = null;

        public MainWindow()
        {
            InitializeComponent();
            _sceneLoaderRegistry = AppServices.CreateSceneLoaderRegistry();
            BuildOpenMenuItems();
            KeyDown += MainWindow_KeyDown;
        }

        private void MenuHelpItem_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                this,
                "Created by Mykhailo Makarov ( m.m.makarov@gmail.com )",
                "About",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void MenuExitItem_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuResetZoom_OnClick(object sender, RoutedEventArgs e)
        {
            SceneView.ResetZoom();
        }

        private void SceneView_OnZoomChanged(object sender, double zoom)
        {
            ZoomTextBlock.Text = $"Zoom: {zoom * 100:F0}%";
        }

        private void BuildOpenMenuItems()
        {
            OpenMenuItem.Items.Clear();

            foreach (var loader in _sceneLoaderRegistry.Loaders)
            {
                var menuItem = new MenuItem
                {
                    Header = loader.DisplayName,
                    Tag = loader
                };

                menuItem.Click += OpenWithLoaderMenuItem_Click;
                OpenMenuItem.Items.Add(menuItem);
            }

            if (OpenMenuItem.Items.Count == 0)
            {
                OpenMenuItem.Items.Add(new MenuItem
                {
                    Header = "(No loaders available)",
                    IsEnabled = false
                });
            }
        }

        private void OpenWithLoaderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ISceneLoader loader = sender is MenuItem menuItem ? menuItem.Tag as ISceneLoader : null;

            if (loader is null)
            {
                return;
            }

            var dialog = new OpenFileDialog
            {
                Filter = loader.FileDialogFilter,
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(this) != true)
            {
                return;
            }

            try
            {
                _currentScene = loader.Load(dialog.FileName);
                _currentSceneFileName = dialog.FileName;
                SceneView.ResetZoom();
                RenderCurrentScene();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "Loading Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void RenderCurrentScene()
        {
            if (_currentScene is null)
            {
                return;
            }

            double width = SceneView.ActualWidth;
            double height = SceneView.ActualHeight;

            if (width <= 0 || height <= 0)
            {
                return;
            }

            var bounds = _currentScene.GetBounds();
            var transformCalculator = new ViewportTransformCalculator();
            var transform = transformCalculator.Calculate(bounds, width, height);

            SceneView.Render(_currentScene, transform);

            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            if (!string.IsNullOrEmpty(_currentSceneFileName))
            {
                string fileName = Path.GetFileName(_currentSceneFileName);
                int count = _currentScene?.Primitives.Count ?? 0;
                StatusTextBlock.Text = $"Loaded: {fileName}  |  Objects: {count}";
            }
        }

        private void SceneView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_currentScene is null)
            {
                return;
            }

            if (e.NewSize.Width <= 0 || e.NewSize.Height <= 0)
            {
                return;
            }

            RenderCurrentScene();
        }

        private void MenuZoomIn_OnClick(object sender, RoutedEventArgs e)
        {
            SceneView.ZoomIn();
        }

        private void MenuZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            SceneView.ZoomOut();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.OemPlus:
                    case Key.Add:
                        SceneView.ZoomIn();
                        break;
                    
                    case Key.OemMinus:
                    case Key.Subtract:
                        SceneView.ZoomOut();
                        break;
                    
                    case Key.D0:
                    case Key.NumPad0:
                        SceneView.ResetZoom();
                        break;
                }
            }
        }
    }
}
