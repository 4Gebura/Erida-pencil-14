using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Wires;

namespace Content.Shared._Erida.Lathe;

public sealed class UpgradeMachineSystem : EntitySystem
{
    [Dependency] private readonly EntityManager _entityManager = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<UpgradeStorageComponent, InteractUsingEvent>(OnInteractUsingEvent);
        SubscribeLocalEvent<UpgradeStorageComponent, UpgradeMachineAfterEvent>(OnUpgradeMachineAfterEvent);
    }

    public void OnInteractUsingEvent(Entity<UpgradeStorageComponent> ent, ref InteractUsingEvent args)
    {
        var wiresComponent = CompOrNull<WiresPanelComponent>(args.Target);
        var upgradePartComponent = CompOrNull<UpgradeMachinePartComponent>(args.Used);

        if (args.Handled
            || upgradePartComponent == null
            || wiresComponent == null
            || !wiresComponent.Open)
        {
            return;
        }

        if (ent.Comp.Storage.Count >= ent.Comp.UpgradeLimit)
        {
            var canBeReplaced = false;
            foreach (EntityUid entUid in ent.Comp.Storage)
            {
                var upgradePartComponentInserted = CompOrNull<UpgradeMachinePartComponent>(entUid);
                if (upgradePartComponentInserted!.Tier < upgradePartComponent.Tier)
                {
                    canBeReplaced = true;
                    break;
                }
            }
            if (!canBeReplaced)
            {
                var targetName = _entityManager.GetComponent<MetaDataComponent>(args.Target).EntityName;
                var itemName = _entityManager.GetComponent<MetaDataComponent>(args.Used).EntityName;
                _popupSystem.PopupEntity(Loc.GetString("machine-part-cant-be-upgraded", ("targetName", targetName), ("itemName", itemName)), args.User);
                return;
            }
        }

        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, upgradePartComponent.UpgradeTime, new UpgradeMachineAfterEvent(),
            args.Target, target: args.Target, used: args.Used)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = true,
        };

        if (!_doAfter.TryStartDoAfter(doAfterArgs))
            return;
    }

    private void OnUpgradeMachineAfterEvent(Entity<UpgradeStorageComponent> ent, ref UpgradeMachineAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        var ev = new UpgradeMachineEvent(args.Used!.Value, args.Target!.Value);
        RaiseLocalEvent(ent.Owner, ref ev);
    }
}
