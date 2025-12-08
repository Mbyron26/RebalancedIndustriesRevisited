using RebalancedIndustriesRevisited.Extensions;

namespace RebalancedIndustriesRevisited.Data;

public class MainIndustryBuildingProfile : ProfileBase<MainIndustryBuildingAI> {
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

    public MainIndustryBuildingProfile() { }

    public MainIndustryBuildingProfile(MainIndustryBuildingAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public override void GetPrefab() {
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

    public override void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultWorkPlace != CustomizedWorkPlace)
            Customized = true;
        else
            Customized = false;
    }
}