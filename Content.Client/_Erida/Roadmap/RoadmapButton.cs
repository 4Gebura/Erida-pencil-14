using Robust.Client.UserInterface.Controls;

namespace Content.Client._Erida.Roadmap;

public sealed class RoadmapButton : Button
{

    public RoadmapButton()
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void EnteredTree()
    {
        base.EnteredTree();
        Text = Loc.GetString("roadmap-button");
    }

    protected override void ExitedTree()
    {
        base.ExitedTree();

    }
}
