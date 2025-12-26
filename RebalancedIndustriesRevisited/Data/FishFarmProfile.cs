using Newtonsoft.Json;
using RebalancedIndustriesRevisited.Extensions;

namespace RebalancedIndustriesRevisited.Data;

public class FishFarmProfile : ProfileBase<FishFarmAI> {
    public override FacilityType BuildingType => FacilityType.FishFarm;
    public override IndustrialCategory IndustrialCategory { get; protected set; } = IndustrialCategory.Fishing;

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
                Prefab.m_productionRate = _customizedOutputRate;
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

    public FishFarmProfile(FishFarmAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public sealed override void GetPrefab() {
        if (Prefab == null) return;

        Name = Prefab.name;
        _customizedConstructionCost = ModDefaultConstructionCost = ConstructionCost = Prefab.m_constructionCost;
        _customizedMaintenanceCost = ModDefaultMaintenanceCost = MaintenanceCost = Prefab.m_maintenanceCost;
        _customizedOutputRate = ModDefaultOutputRate = OutputRate = Prefab.m_productionRate;
        _customizedTruckCount = ModDefaultTruckCount = TruckCount = Prefab.m_outputVehicleCount;
        _customizedWorkPlace = ModDefaultWorkPlace = WorkPlace = Prefab.GetWorkPlace();
    }

    public override void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultTruckCount != CustomizedTruckCount || ModDefaultOutputRate != CustomizedOutputRate || ModDefaultWorkPlace != CustomizedWorkPlace) {
            Customized = true;
        }
        else {
            Customized = false;
        }
    }

    public override void SetFromLoadData(IProfile profile) {
        CustomizedConstructionCost = profile.CustomizedConstructionCost;
        CustomizedMaintenanceCost = profile.CustomizedMaintenanceCost;
        CustomizedTruckCount = profile.CustomizedTruckCount;
        CustomizedOutputRate = profile.CustomizedOutputRate;
        CustomizedWorkPlace = profile.CustomizedWorkPlace;
    }

    public override void SetFromModData() { }

    public override void SetGameDefaults() {
        CustomizedConstructionCost = ConstructionCost;
        CustomizedMaintenanceCost = MaintenanceCost;
        CustomizedTruckCount = TruckCount;
        CustomizedOutputRate = OutputRate;
        CustomizedWorkPlace = WorkPlace;
    }

    public override void SetModDefaults() {
        CustomizedConstructionCost = ModDefaultConstructionCost;
        CustomizedMaintenanceCost = ModDefaultMaintenanceCost;
        CustomizedTruckCount = ModDefaultTruckCount;
        CustomizedOutputRate = ModDefaultOutputRate;
        CustomizedWorkPlace = ModDefaultWorkPlace;
    }

    public override void OutputInfo() {
        Logger.Info($"FishFarm | Construction cost: {ConstructionCost} -> {Prefab.m_constructionCost} | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Vehicle count: {TruckCount} -> {Prefab.m_outputVehicleCount} | Output rate: {OutputRate} -> {Prefab.m_productionRate} | Work space: {WorkPlace.ToString()} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | Building: {Name}");
    }
}