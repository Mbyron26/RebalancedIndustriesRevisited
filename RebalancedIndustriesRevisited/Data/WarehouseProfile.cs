using System;
using CSLModsCommon.Logging;
using RebalancedIndustriesRevisited.Extensions;
using UnityEngine;

namespace RebalancedIndustriesRevisited.Data;

public class WarehouseProfile : ProfileBase<WarehouseAI> {
    public override FacilityType BuildingType => FacilityType.WarehouseFacility;
    public override IndustrialCategory IndustrialCategory { get; protected set; }
    public const int MinWarehouseCapacity = 40000;

    public override int CustomizedConstructionCost {
        get => _customizedConstructionCost;
        set {
            _customizedConstructionCost = value;
            if (Prefab is not null)
                Prefab.m_constructionCost = _customizedConstructionCost;
            Validate();
        }
    }

    public override int CustomizedMaintenanceCost {
        get => _customizedMaintenanceCost;
        set {
            _customizedMaintenanceCost = value;
            if (Prefab is not null)
                Prefab.m_maintenanceCost = _customizedMaintenanceCost;
            Validate();
        }
    }

    public override int CustomizedTruckCount {
        get => _customizedTruckCount;
        set {
            _customizedTruckCount = value;
            if (Prefab is not null)
                Prefab.m_truckCount = _customizedTruckCount;
            Validate();
        }
    }

    public override int CustomizedOutputRate {
        get => _customizedOutputRate;
        set {
            _customizedOutputRate = value;
            Validate();
        }
    }

    public override WorkPlace CustomizedWorkPlace {
        get => _customizedWorkPlace;
        set {
            _customizedWorkPlace = value;
            Prefab?.SetWorkPlace(_customizedWorkPlace);
            Validate();
        }
    }

    public override int CustomizedStorageCapacity {
        get => _customizedStorageCapacity;
        set {
            _customizedStorageCapacity = value;
            if (Prefab is not null)
                Prefab.m_storageCapacity = Mathf.Clamp(_customizedStorageCapacity, 0, 6500000);
            Validate();
        }
    }

    public WarehouseProfile(WarehouseAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public override void GetPrefab() {
        Name = Prefab.name;
        _customizedConstructionCost = ModDefaultConstructionCost = ConstructionCost = Prefab.m_constructionCost;
        _customizedMaintenanceCost = ModDefaultMaintenanceCost = MaintenanceCost = Prefab.m_maintenanceCost;
        _customizedTruckCount = ModDefaultTruckCount = TruckCount = Prefab.m_truckCount;
        _customizedWorkPlace = ModDefaultWorkPlace = WorkPlace = Prefab.GetWorkPlace();
        _customizedStorageCapacity = ModDefaultStorageCapacity = StorageCapacity = Prefab.m_storageCapacity;
        IndustrialCategory = Prefab.m_storageType switch {
            TransferManager.TransferReason.Logs or TransferManager.TransferReason.Paper or TransferManager.TransferReason.PlanedTimber => IndustrialCategory.Forestry,
            TransferManager.TransferReason.Grain or TransferManager.TransferReason.Flours or TransferManager.TransferReason.AnimalProducts => IndustrialCategory.Farming,
            TransferManager.TransferReason.Ore or TransferManager.TransferReason.Metals or TransferManager.TransferReason.Glass => IndustrialCategory.Ore,
            TransferManager.TransferReason.Oil or TransferManager.TransferReason.Plastics or TransferManager.TransferReason.Petroleum => IndustrialCategory.Oil,
            _ => IndustrialCategory.Unknown
        };
    }

    public override void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultTruckCount != CustomizedTruckCount || ModDefaultWorkPlace != CustomizedWorkPlace || ModDefaultStorageCapacity != CustomizedStorageCapacity)
            Customized = true;
        else
            Customized = false;
    }

    private float GetWarehouseTruckFactor() {
        return Prefab.m_storageType == TransferManager.TransferReason.Ore ? 0.8f : 0.5f;
    }

    private decimal GetCostFactor() {
        return Prefab.m_storageType == TransferManager.TransferReason.Grain ? 0.5m : 1;
    }

    public override void SetFromModData() {
        ModDefaultConstructionCost = CustomizedConstructionCost = Convert.ToInt32(ConstructionCost * GetCostFactor());
        ModDefaultMaintenanceCost = CustomizedMaintenanceCost = Convert.ToInt32(MaintenanceCost * GetCostFactor());
        var factor = GetWarehouseTruckFactor();
        ModDefaultTruckCount = CustomizedTruckCount = (int)Math.Ceiling(TruckCount * factor);
        ModDefaultTruckCount = CustomizedTruckCount = Prefab.m_storageCapacity <= MinWarehouseCapacity ? Mathf.Max(1, CustomizedTruckCount) : Mathf.Max(2, CustomizedTruckCount);
        var workPlaceFactor = Prefab.m_storageType switch {
            TransferManager.TransferReason.Grain => 4m,
            TransferManager.TransferReason.Logs or
                TransferManager.TransferReason.Ore or
                TransferManager.TransferReason.Oil => 2m,
            _ => 1m
        };
        if (CustomizedWorkPlace.Sum() > 10) ModDefaultWorkPlace = CustomizedWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(CustomizedWorkPlace.UneducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(CustomizedWorkPlace.EducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(CustomizedWorkPlace.WellEducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(CustomizedWorkPlace.HighlyEducatedWorkers / workPlaceFactor)));
    }


    // public void RebindTooltip() {
    //     if (ManagerPool.GetOrCreateManager<Manager>().IndustryPanelButtons.TryGetValue(Name, out UIButton button)) {
    //         var rawTooltip = button.tooltip;
    //         var newTooltip = rawTooltip;
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyTruckCountString(TruckCount, Prefab.m_truckCount, ref newTooltip);
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyConstructionCostString(ConstructionCost, Prefab.m_constructionCost, Prefab, ref newTooltip);
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyMaintenanceCostString(MaintenanceCost, Prefab.m_maintenanceCost, Prefab, ref newTooltip);
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyWorkSpaceString(WorkPlace, CustomizedWorkPlace, ref newTooltip);
    //         button.tooltip = newTooltip;
    //         // LogManager.GetLogger().Info($"Rebinding {Name} tooltip:\n{rawTooltip} -> \n{button.tooltip}\n");
    //     }
    // }

    public override void OutputInfo() {
        LogManager.GetLogger().Info($"Warehouse | Vehicle count: {TruckCount} -> {Prefab.m_truckCount} | Construction cost: {ConstructionCost} -> {Prefab.m_constructionCost} | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Work space: {WorkPlace.UneducatedWorkers} {WorkPlace.EducatedWorkers} {WorkPlace.WellEducatedWorkers} {WorkPlace.HighlyEducatedWorkers} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | Building: {Name}");
    }
}