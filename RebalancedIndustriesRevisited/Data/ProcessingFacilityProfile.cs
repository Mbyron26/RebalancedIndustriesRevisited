using System;
using System.Linq;
using Newtonsoft.Json;
using RebalancedIndustriesRevisited.Extensions;
using UnityEngine;

namespace RebalancedIndustriesRevisited.Data;

public class ProcessingFacilityProfile : ProfileBase<ProcessingFacilityAI> {
    private static string[] ProcessorField { get; } = ["Animal Pasture 01", "Animal Pasture 02", "Cattle Shed 01"];

    public override FacilityType BuildingType => FacilityType.ProcessingFacility;
    public override IndustrialCategory IndustrialCategory { get; protected set; }
    [JsonIgnore] public override int CustomizedBoatCount { get; set; }
    [JsonIgnore] public override int CustomizedStorageCapacity { get; set; }

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

    public ProcessingFacilityProfile(ProcessingFacilityAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public sealed override void GetPrefab() {
        if (Prefab == null) return;

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

    public override void SetFromLoadData(IProfile profile) {
        CustomizedConstructionCost = profile.CustomizedConstructionCost;
        CustomizedMaintenanceCost = profile.CustomizedMaintenanceCost;
        CustomizedTruckCount = profile.CustomizedTruckCount;
        CustomizedOutputRate = profile.CustomizedOutputRate;
        CustomizedWorkPlace = profile.CustomizedWorkPlace;
    }

    public override void SetModDefaults() {
        CustomizedConstructionCost = ModDefaultConstructionCost;
        CustomizedMaintenanceCost = ModDefaultMaintenanceCost;
        CustomizedTruckCount = ModDefaultTruckCount;
        CustomizedOutputRate = ModDefaultOutputRate;
        CustomizedWorkPlace = ModDefaultWorkPlace;
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

        if (!IsSpecificBuilding())
            return;
        var workersFactor = 0.35m;
        ModDefaultWorkPlace = CustomizedWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount0 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount1 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount2 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount3 * workersFactor)));
    }

    public override void SetGameDefaults() {
        CustomizedConstructionCost = ConstructionCost;
        CustomizedMaintenanceCost = MaintenanceCost;
        CustomizedTruckCount = TruckCount;
        CustomizedOutputRate = OutputRate;
        CustomizedWorkPlace = WorkPlace;
    }

    public override void OutputInfo() {
        Logger.Info($"Processing Facility | Vehicle count: {TruckCount} -> {Prefab.m_outputVehicleCount} | Construction cost: {ConstructionCost} -> {Prefab.m_constructionCost} | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Work space: {WorkPlace.ToString()} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | Building: {Name}");
    }

    public void ModifyProductionRate(float factor) => ModDefaultOutputRate = CustomizedOutputRate = (int)(OutputRate * factor);

    private bool IsSpecificBuilding() => ProcessorField.Any(s => s == Name);
}