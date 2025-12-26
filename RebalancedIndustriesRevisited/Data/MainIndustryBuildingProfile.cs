using Newtonsoft.Json;
using RebalancedIndustriesRevisited.Extensions;

namespace RebalancedIndustriesRevisited.Data;

public class MainIndustryBuildingProfile : ProfileBase<MainIndustryBuildingAI> {
    [JsonIgnore] public override int CustomizedBoatCount { get; set; }
    [JsonIgnore] public override int CustomizedOutputRate { get; set; }
    [JsonIgnore] public override int CustomizedStorageCapacity { get; set; }
    [JsonIgnore] public override int CustomizedTruckCount { get; set; }

    public override WorkPlace CustomizedWorkPlace {
        get => _customizedWorkPlace;
        set {
            _customizedWorkPlace = value;
            Prefab?.SetWorkPlace(_customizedWorkPlace);
            Validate();
        }
    }

    public override FacilityType BuildingType => FacilityType.MainIndustryBuilding;
    public override IndustrialCategory IndustrialCategory { get; protected set; }

    public MainIndustryBuildingProfile(MainIndustryBuildingAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public sealed override void GetPrefab() {
        if (Prefab == null) return;

        Name = Prefab.name;
        _customizedConstructionCost = ModDefaultConstructionCost = ConstructionCost = Prefab.m_constructionCost;
        _customizedMaintenanceCost = ModDefaultMaintenanceCost = MaintenanceCost = Prefab.m_maintenanceCost;
        _customizedWorkPlace = ModDefaultWorkPlace = WorkPlace = Prefab.GetWorkPlace();
        IndustrialCategory = Prefab.m_industryType switch {
            DistrictPark.ParkType.Oil => IndustrialCategory.Oil,
            DistrictPark.ParkType.Ore => IndustrialCategory.Ore,
            DistrictPark.ParkType.Forestry => IndustrialCategory.Forestry,
            DistrictPark.ParkType.Farming => IndustrialCategory.Farming,
            _ => IndustrialCategory.Unknown
        };
    }

    public override void SetFromLoadData(IProfile profile) {
        CustomizedConstructionCost = profile.CustomizedConstructionCost;
        CustomizedMaintenanceCost = profile.CustomizedMaintenanceCost;
        CustomizedWorkPlace = profile.CustomizedWorkPlace;
    }

    public override void SetFromModData() { }

    public override void SetGameDefaults() {
        CustomizedConstructionCost = ConstructionCost;
        CustomizedMaintenanceCost = MaintenanceCost;
        CustomizedWorkPlace = WorkPlace;
    }

    public override void SetModDefaults() {
        CustomizedConstructionCost = ModDefaultConstructionCost;
        CustomizedMaintenanceCost = ModDefaultMaintenanceCost;
        CustomizedWorkPlace = ModDefaultWorkPlace;
    }

    public override void OutputInfo() {
        Logger.Info($"MainIndustryBuildingAI | Construction cost: {ConstructionCost} -> {Prefab.m_constructionCost} | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Work space: {WorkPlace.ToString()} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | Building: {Name}");
    }

    public override void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultWorkPlace != CustomizedWorkPlace)
            Customized = true;
        else
            Customized = false;
    }
}