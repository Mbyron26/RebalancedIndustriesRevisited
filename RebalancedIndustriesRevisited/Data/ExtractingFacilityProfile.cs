using System;
using RebalancedIndustriesRevisited.Extensions;
using UnityEngine;

namespace RebalancedIndustriesRevisited.Data;

public class ExtractingFacilityProfile : ProfileBase<ExtractingFacilityAI> {
    public override FacilityType BuildingType => FacilityType.ExtractingFacility;
    public override IndustrialCategory IndustrialCategory { get; protected set; }

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

    public ExtractingFacilityProfile(ExtractingFacilityAI prefab) {
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
        IndustrialCategory = Prefab.NaturalResourceType switch {
            NaturalResourceManager.Resource.Oil => IndustrialCategory.Oil,
            NaturalResourceManager.Resource.Ore => IndustrialCategory.Ore,
            NaturalResourceManager.Resource.Forest => IndustrialCategory.Forestry,
            NaturalResourceManager.Resource.Fertility => IndustrialCategory.Farming,
            _ => IndustrialCategory.Unknown
        };
    }

    public override void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultTruckCount != CustomizedTruckCount || ModDefaultOutputRate != CustomizedOutputRate || ModDefaultWorkPlace != CustomizedWorkPlace) {
            Customized = true;
        }
        else {
            Customized = false;
        }
    }
    
    public override void OutputInfo() {
        Logger.Info($"Extracting Facility | Vehicle count: {TruckCount} -> {Prefab.m_outputVehicleCount} | Construction cost: {ConstructionCost} -> {Prefab.m_constructionCost} | Output rate: {OutputRate} -> {Prefab.m_outputRate} | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Work space: {WorkPlace.UneducatedWorkers} {WorkPlace.EducatedWorkers} {WorkPlace.WellEducatedWorkers} {WorkPlace.HighlyEducatedWorkers} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | Building: {Name}");
    }

    public override void SetFromModData() {
        ModDefaultConstructionCost = CustomizedConstructionCost = (int)Math.Ceiling(ConstructionCost * GetCostFactor());
        ModDefaultMaintenanceCost = CustomizedMaintenanceCost = (int)Math.Ceiling(MaintenanceCost * GetCostFactor());
        ModDefaultTruckCount = CustomizedTruckCount = Mathf.Max((int)Math.Ceiling(TruckCount * GetTruckFactor(Prefab.m_outputResource)), MinTruckCount);
        if (OutputRate == 0) {
            OutputRate = 700;
            Logger.Info($"Extracting facility raw output rate is zero, fixed to 700, building: {Name}");
        }
        
        if (Prefab.m_outputResource == TransferManager.TransferReason.Grain) {
            var workPlace = new WorkPlace(2, 4, 1, 0);
            var workPlaceSum = (int)Math.Ceiling(Math.Sqrt(Prefab.m_info.m_cellLength * Prefab.m_info.m_cellWidth) / 2);
            if (workPlaceSum != 0) {
                ModDefaultWorkPlace = CustomizedWorkPlace = new WorkPlace((int)(GetRatio(workPlace.UneducatedWorkers, workPlace.Sum()) * workPlaceSum), (int)(GetRatio(workPlace.EducatedWorkers, workPlace.Sum()) * workPlaceSum), (int)(GetRatio(workPlace.WellEducatedWorkers, workPlace.Sum()) * workPlaceSum), (int)(GetRatio(workPlace.HighlyEducatedWorkers, workPlace.Sum()) * workPlaceSum));
            }
        }
        else {
            const decimal workersFactor = 0.5m;
            ModDefaultWorkPlace = CustomizedWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount0 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount1 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount2 * workersFactor)), Convert.ToInt32(Math.Round(Prefab.m_workPlaceCount3 * workersFactor)));
        }
    }

    public void ModifyProductionRate(float factor) => ModDefaultOutputRate = CustomizedOutputRate = (int)(OutputRate * factor);

    private decimal GetCostFactor() => Prefab.m_outputResource == TransferManager.TransferReason.Grain ? 0.25m : 1m;
}