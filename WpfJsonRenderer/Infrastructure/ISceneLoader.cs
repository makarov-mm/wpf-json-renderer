using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Infrastructure
{
    public interface ISceneLoader
    {
        string DisplayName { get; }
        string FileDialogFilter { get; }
        bool CanLoad(string filePath);
        Scene Load(string filePath);
    }
}
