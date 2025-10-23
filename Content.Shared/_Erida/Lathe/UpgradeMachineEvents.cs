using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._Erida.Lathe
{
    [Serializable, NetSerializable]
    public sealed partial class UpgradeMachineAfterEvent : DoAfterEvent
    {
        public override DoAfterEvent Clone() => this;
    }

    [ByRefEvent]
    public record struct UpgradeMachineEvent(EntityUid ItemUid, EntityUid TargetUid);
}
