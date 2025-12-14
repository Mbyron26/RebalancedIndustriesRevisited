using CSLModsCommon.Logging;
using RebalancedIndustriesRevisited.Extensions;

namespace RebalancedIndustriesRevisited.Data;

public class FishingHarborProfile : ProfileBase<FishingHarborAI> {
    private int _customizedBoatCount;

    public override FacilityType BuildingType => FacilityType.FishingHarbor;
    public override IndustrialCategory IndustrialCategory { get; protected set; } = IndustrialCategory.Fishing;

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

    public override int CustomizedBoatCount {
        get => _customizedBoatCount;
        set {
            _customizedBoatCount = value;
            if (Prefab is not null)
                Prefab.m_boatCount = _customizedBoatCount;
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

    public FishingHarborProfile(FishingHarborAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public override void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultTruckCount != CustomizedTruckCount || ModDefaultWorkPlace != CustomizedWorkPlace || ModDefaultBoatCount != CustomizedBoatCount) {
            Customized = true;
        }
        else {
            Customized = false;
        }
    }

    public sealed override void GetPrefab() {
        if (Prefab is null) return;
        
        Name = Prefab.name;
        _customizedConstructionCost = ModDefaultConstructionCost = ConstructionCost = Prefab.m_constructionCost;
        _customizedMaintenanceCost = ModDefaultMaintenanceCost = MaintenanceCost = Prefab.m_maintenanceCost;
        _customizedTruckCount = ModDefaultTruckCount = TruckCount = Prefab.m_outputVehicleCount;
        _customizedBoatCount = ModDefaultBoatCount = BoatCount = Prefab.m_boatCount;
        _customizedWorkPlace = ModDefaultWorkPlace = WorkPlace = Prefab.GetWorkPlace();
    }

    public override void OutputInfo() {
        LogManager.GetLogger().Info($"FishingHarbor | Vehicle count: {TruckCount} -> {Prefab.m_outputVehicleCount} | Construction cost: {ConstructionCost} -> {Prefab.m_constructionCost} | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Work space: {WorkPlace.SimpleString()} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | BoatCount: {BoatCount} -> {Prefab.m_boatCount} | Building: {Name}");
    }
}