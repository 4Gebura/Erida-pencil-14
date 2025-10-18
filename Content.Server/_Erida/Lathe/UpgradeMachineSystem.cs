using Content.Shared._Erida.Lathe;
using Content.Shared.Lathe;
using Content.Shared.Popups;
using Robust.Shared.Containers;

namespace Content.Server._Erida.Lathe;

public sealed class UpgradeMachineSystem : EntitySystem
{
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly EntityManager _entityManager = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;



    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<UpgradeStorageComponent, UpgradeMachineEvent>(OnUpgradeMachineEvent);
        SubscribeLocalEvent<UpgradeStorageComponent, ComponentShutdown>(OnUpgradeStorageShutdown);
        SubscribeLocalEvent<UpgradeStorageComponent, ComponentInit>(OnUpgradeStorageInit);
    }

    private void OnUpgradeStorageInit(Entity<UpgradeStorageComponent> ent, ref ComponentInit args)
    {
        var lathComponent = CompOrNull<LatheComponent>(ent.Owner);

        if (lathComponent != null)
        {
            ent.Comp.BaseMaterialModifier = lathComponent.TimeMultiplier;
            ent.Comp.BaseSpeedModifier = lathComponent.MaterialUseMultiplier;
        }
    }

    private void OnUpgradeStorageShutdown(Entity<UpgradeStorageComponent> ent, ref ComponentShutdown args)
    {
        foreach (EntityUid entUid in ent.Comp.Storage)
        {
            _container.Remove(entUid, ent.Comp.Container);
            ent.Comp.Storage.Remove(entUid);
        }
    }

    private void InsertMachinePart(EntityUid itemUid, Entity<UpgradeStorageComponent> ent)
    {
        _container.Insert(itemUid, ent.Comp.Container);
        ent.Comp.Storage.Add(itemUid);

        var lathComponent = CompOrNull<LatheComponent>(ent.Owner);

        var newSpeedModifier = ent.Comp.BaseSpeedModifier;
        var newMaterialModifier = ent.Comp.BaseMaterialModifier;

        if (lathComponent != null)
        {
            foreach (EntityUid entUid in ent.Comp.Storage)
            {
                var upgradePartComponentInserted = CompOrNull<UpgradeMachinePartComponent>(entUid);

                newSpeedModifier = upgradePartComponentInserted!.SpeedModifier * newSpeedModifier;
                newMaterialModifier = upgradePartComponentInserted!.MaterialModifier * newMaterialModifier;
            }

            lathComponent.MaterialUseMultiplier = newMaterialModifier;
            lathComponent.TimeMultiplier = newSpeedModifier;
        }

        var targetName = _entityManager.GetComponent<MetaDataComponent>(ent.Owner).EntityName;
        var itemName = _entityManager.GetComponent<MetaDataComponent>(itemUid).EntityName;

        _popupSystem.PopupEntity(Loc.GetString("machine-part-upgraded", ("targetName", targetName), ("itemName", itemName)), ent.Owner);
    }
    private void OnUpgradeMachineEvent(Entity<UpgradeStorageComponent> ent, ref UpgradeMachineEvent args)
    {
        ent.Comp.Container = _container.EnsureContainer<Container>(ent.Owner, ent.Comp.ContainerId);
        var upgradePartComponent = CompOrNull<UpgradeMachinePartComponent>(args.ItemUid);
        if (ent.Comp.Storage.Count >= ent.Comp.UpgradeLimit)
        {
            foreach (EntityUid entUid in ent.Comp.Storage)
            {
                var upgradePartComponentInserted = CompOrNull<UpgradeMachinePartComponent>(entUid);

                if (upgradePartComponentInserted!.Tier < upgradePartComponent!.Tier)
                {
                    _container.Remove(entUid, ent.Comp.Container);
                    ent.Comp.Storage.Remove(entUid);
                    InsertMachinePart(args.ItemUid, ent);
                    break;
                }
            }
        }
        else
        {
            InsertMachinePart(args.ItemUid, ent);
        }
    }
}
