using System;
using System.Linq;
using RebalancedIndustriesRevisited.Extensions;
using UnityEngine;

namespace RebalancedIndustriesRevisited.Data;

public class ProcessingFacilityProfile : ProfileBase<ProcessingFacilityAI> {
    public override FacilityType BuildingType => FacilityType.ProcessingFacility;
    public override IndustrialCategory IndustrialCategory { get; protected set; }

    public static string[] ProcessorField { get; } = { "Animal Pasture 01", "Animal Pasture 02", "Cattle Shed 01" };
    public float ProcessingFacilityProductionRate => 0.5f;

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
                Prefab.m_outputVehicleCount = _customizedTruckCount;
            Validate();
        }
    }

    public override int CustomizedOutputRate {
        get => _customizedOutputRate;
        set {
            _customizedOutputRate = value;
            if (Prefab is not null)
                Prefab.m_outputRate = _customizedOutputRate;
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

    public ProcessingFacilityProfile() { }

    public ProcessingFacilityProfile(ProcessingFacilityAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public override void GetPrefab() {
        Name = Prefab.name;
        _customizedConstructionCost = ModDefaultConstructionCost = ConstructionCost = Prefab.m_constructionCost;
        _customizedMaintenanceCost = ModDefaultMaintenanceCost = MaintenanceCost = Prefab.m_maintenanceCost;
        _customizedTruckCount = ModDefaultTruckCount = TruckCount = Prefab.m_outputVehicleCount;
        _customizedWorkPlace = ModDefaultWorkPlace = WorkPlace = Prefab.GetWorkPlace();
        _customizedOutputRate = ModDefaultOutputRate = OutputRate = Prefab.m_outputRate;
        IndustrialCategory = Prefab.m_industryType switch {
            DistrictPark.ParkType.Oil => IndustrialCategory.Oil,
            DistrictPark.ParkType.Ore => IndustrialCategory.Ore,
            DistrictPark.ParkType.Forestry => IndustrialCategory.Forestry,
            DistrictPark.ParkType.Farming => IndustrialCategory.Farming,
            _ => IndustrialCategory.Unknown
        };
    }

    public override void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultTruckCount != CustomizedTruckCount || ModDefaultOutputRate != CustomizedOutputRate || ModDefaultWorkPlace != CustomizedWorkPlace)
            Customized = true;
        else
            Customized = false;
    }

    public override void SetFromModData() {
        if (IsSpecificBuilding())
            ModDefaultConstructionCost = CustomizedConstructionCost = (int)(ConstructionCost * 0.5m);
        if (IsSpecificBuilding())
            ModDefaultMaintenanceCost = CustomizedMaintenanceCost = (int)(MaintenanceCost * 0.5m);
        var truckFactor = GetTruckFactor(Prefab.m_outputResource);
        ModDefaultTruckCount = CustomizedTruckCount = Mathf.Max((int)Math.Ceiling(TruckCount * truckFactor), MinTruckCount);
        if (OutputRate == 0) {
            OutputRate = 700;
            Logger.Error($"Processing facility raw output raw is zero, fixed to 700, building: {Name}");
        }

        ModDefaultOutputRate = CustomizedOutputRate = (int)(OutputRate * ProcessingFacilityProductionRate);
        if (!IsSpecificBuilding())
            return;
        var workersFactor = 0.35m;
        ModDefaultWorkPlace = CustomizedWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount0 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount1 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount2 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount3 * workersFactor)));
    }

    private bool IsSpecificBuilding() {
        return ProcessorField.Any(s => s == Name);
    }

    // public void RebindTooltip() {
    //     var isIndustry = Prefab.m_inputResource1 switch {
    //         TransferManager.TransferReason.Oil or TransferManager.TransferReason.Ore or TransferManager.TransferReason.Logs or TransferManager.TransferReason.Grain => true,
    //         _ => false
    //     };
    //     if (isIndustry && ManagerPool.GetOrCreateManager<Manager>().IndustryPanelButtons.TryGetValue(Name, out UIButton button)) {
    //         var rawTooltip = button.tooltip;
    //         var newTooltip = rawTooltip;
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyTruckCountString(TruckCount, Prefab.m_outputVehicleCount, ref newTooltip);
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyConstructionCostString(ConstructionCost, Prefab.m_constructionCost, Prefab, ref newTooltip);
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyMaintenanceCostString(MaintenanceCost, Prefab.m_maintenanceCost, Prefab, ref newTooltip);
    //         ManagerPool.GetOrCreateManager<Manager>().ModifyWorkSpaceString(WorkPlace, CustomizedWorkPlace, ref newTooltip);
    //         button.tooltip = newTooltip;
    //         // LogManager.GetLogger().Info($"Rebinding {Name} tooltip:\n{rawTooltip} -> \n{button.tooltip}\n");
    //     }
    // }

    public override void OutputInfo() {
        Logger.Info($"Processing Facility | Vehicle count: {TruckCount} -> {Prefab.m_outputVehicleCount} | Construction cost: {ConstructionCost} -> {Prefab.m_constructionCost} | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Work space: {WorkPlace.UneducatedWorkers} {WorkPlace.EducatedWorkers} {WorkPlace.WellEducatedWorkers} {WorkPlace.HighlyEducatedWorkers} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | Building: {Name}");
    }
}