namespace Content.Shared._Erida.Lathe
{
    [RegisterComponent]
    public sealed partial class UpgradeMachinePartComponent : Component
    {
        [DataField("Tier")]
        public int Tier = 1;

        [DataField("SpeedModifier")]
        public float SpeedModifier = 1;

        [DataField("MaterialModifier")]
        public float MaterialModifier = 1;

        [DataField]
        public TimeSpan UpgradeTime = TimeSpan.FromSeconds(5.0f);
    }
}
