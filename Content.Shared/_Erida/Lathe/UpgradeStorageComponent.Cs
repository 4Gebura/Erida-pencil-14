using Robust.Shared.Containers;
using Robust.Shared.GameStates;

namespace Content.Shared._Erida.Lathe
{
    [RegisterComponent]
    public sealed partial class UpgradeStorageComponent : Component
    {
        [ViewVariables(VVAccess.ReadOnly)]
        public List<EntityUid> Storage = new List<EntityUid>();

        [DataField]
        public int UpgradeLimit = 1;

        [DataField]
        public float BaseSpeedModifier = 1;

        [DataField]
        public float BaseMaterialModifier = 1;

        [DataField]
        public bool isFirstUpdate = true;

        public Container Container = default!;
        public string ContainerId = "UpgradeStorage";
    }
}
