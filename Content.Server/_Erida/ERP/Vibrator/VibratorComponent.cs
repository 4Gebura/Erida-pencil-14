
using Robust.Shared.Audio;

namespace Content.Server._Erida.ERP.Vibrator;

[RegisterComponent]
public sealed partial class VibratorComponent : Component
{
    [ViewVariables]
    public TimeSpan NextEmoteTime = TimeSpan.MinValue;

    [ViewVariables]
    public TimeSpan Interval = TimeSpan.FromSeconds(4);


    [ViewVariables]
    public EntityUid? User = null;

    [ViewVariables(VVAccess.ReadWrite)]
    public bool IsActive = false;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int JitterProbablity = 40;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool IsTogglable = false;

    [DataField]
    public SoundSpecifier? VibrationSound;

    public EntityUid? Stream;

    /// <summary>
    /// Active vibration arousal amount.
    /// </summary>
    [DataField]
    public float ActiveArousalAmount = 15f;

    /// <summary>
    /// Equip/Unequip arousal amount.
    /// </summary>
    [DataField]
    public float ArousalAmount = 10f;
}
