
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controllers;

namespace Content.Client._Erida.Roadmap;

[UsedImplicitly]
public sealed class RoadmapUIController : UIController
{
    private RoadmapWindow _roadmapWindow = default!;

    public void OpenWindow()
    {
        EnsureWindow();

        _roadmapWindow.OpenCentered();
        _roadmapWindow.MoveToFront();
    }

    private void EnsureWindow()
    {
        if (_roadmapWindow is { Disposed: false })
            return;

        _roadmapWindow = UIManager.CreateWindow<RoadmapWindow>();
    }

    public void ToggleWindow()
    {
        EnsureWindow();

        if (_roadmapWindow.IsOpen)
        {
            _roadmapWindow.Close();
        }
        else
        {
            OpenWindow();
        }
    }
}
