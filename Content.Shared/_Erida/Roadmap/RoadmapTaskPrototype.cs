using Robust.Shared.Prototypes;

namespace Content.Shared._Erida.Roadmap;

/// <summary>
/// Prototype for Roadmap Tasks
/// </summary>
[Prototype]
public sealed partial class RoadmapTaskPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = string.Empty;

    /// <summary>
    /// Task name to display
    /// </summary>
    [DataField(required: true)]
    public string Name = string.Empty;

    /// <summary>
    /// Detailed description to display
    /// </summary>
    [DataField(required: true)]
    public string Description = string.Empty;

    /// <summary>
    /// Roadmap Category to be added to
    /// </summary>
    [DataField(required: true)]
    public RoadmapCategories Category;

    [DataField]
    public int Order { get; set; }
}

public enum RoadmapCategories
{
    Primary,
    LongRange,
    Idea
}
