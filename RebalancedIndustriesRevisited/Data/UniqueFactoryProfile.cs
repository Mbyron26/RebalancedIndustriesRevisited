using CSLModsCommon.Logging;
using RebalancedIndustriesRevisited.Extensions;
using UnityEngine;

namespace RebalancedIndustriesRevisited.Data;

public class UniqueFactoryProfile : ProfileBase<UniqueFactoryAI> {
    public override FacilityType BuildingType => FacilityType.UniqueFacility;
    public override IndustrialCategory IndustrialCategory { get; protected set; }
    public UniqueFactoryAIValue ProfileValue { get; private set; }

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

    public UniqueFactoryProfile(UniqueFactoryAI prefab) {
        Prefab = prefab;
        GetPrefab();
    }

    public sealed override void GetPrefab() {
        if (Prefab == null) return;

        Name = Prefab.name;
        _customizedConstructionCost = ModDefaultConstructionCost = ConstructionCost = Prefab.m_constructionCost;
        _customizedMaintenanceCost = ModDefaultMaintenanceCost = MaintenanceCost = Prefab.m_maintenanceCost;
        _customizedTruckCount = ModDefaultTruckCount = TruckCount = Prefab.m_outputVehicleCount;
        _customizedWorkPlace = ModDefaultWorkPlace = WorkPlace = new WorkPlace(Prefab.m_workPlaceCount0, Prefab.m_workPlaceCount1, Prefab.m_workPlaceCount2, Prefab.m_workPlaceCount3);
        _customizedOutputRate = ModDefaultOutputRate = OutputRate = Prefab.m_outputRate;
        ProfileValue = Prefab.name switch {
            "Furniture Factory 01" => new UniqueFactoryAIValue(320, new WorkPlace(25, 18, 8, 4)),
            "Bakery 01" => new UniqueFactoryAIValue(260, new WorkPlace(15, 9, 4, 2)),
            "Industrial Steel Plant 01" => new UniqueFactoryAIValue(1800, new WorkPlace(60, 45, 30, 5)),
            "Household Plastic Factory 01" => new UniqueFactoryAIValue(480, new WorkPlace(25, 18, 8, 4)),
            "Toy Factory 01" => new UniqueFactoryAIValue(760, new WorkPlace(25, 18, 8, 4)),
            "Printing Press 01" => new UniqueFactoryAIValue(560, new WorkPlace(22, 16, 8, 4)),
            "Lemonade Factory 01" => new UniqueFactoryAIValue(800, new WorkPlace(55, 35, 15, 5)),
            "Electronics Factory 01" => new UniqueFactoryAIValue(1800, new WorkPlace(55, 40, 20, 10)),
            "Clothing Factory 01" => new UniqueFactoryAIValue(840, new WorkPlace(35, 20, 10, 5)),
            "Petroleum Refinery 01" => new UniqueFactoryAIValue(2600, new WorkPlace(60, 45, 30, 15)),
            "Soft Paper Factory 01" => new UniqueFactoryAIValue(2200, new WorkPlace(60, 50, 12, 8)),
            "Car Factory 01" => new UniqueFactoryAIValue(3400, new WorkPlace(70, 60, 20, 10)),
            "Sneaker Factory 01" => new UniqueFactoryAIValue(1920, new WorkPlace(35, 30, 10, 5)),
            "Modular House Factory 01" => new UniqueFactoryAIValue(2400, new WorkPlace(70, 45, 15, 10)),
            "Food Factory 01" => new UniqueFactoryAIValue(1920, new WorkPlace(55, 35, 15, 5)),
            "Dry Dock 01" => new UniqueFactoryAIValue(3800, new WorkPlace(80, 50, 20, 10)),
            _ => new UniqueFactoryAIValue(1, new WorkPlace())
        };
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
        if (ProfileValue.CostsFactor != 1)
            ModDefaultMaintenanceCost = CustomizedMaintenanceCost = ProfileValue.CostsFactor * 100 / 16;
        if (Name != "Dry Dock 01")
            CustomizedTruckCount = Mathf.Max(Mathf.FloorToInt(1f * TruckCount), 1);
        if (ProfileValue.CostsFactor == 1)
            return;
        ModDefaultWorkPlace = CustomizedWorkPlace = ProfileValue.WorkPlaceValue;
        Prefab.m_workPlaceCount0 = CustomizedWorkPlace.UneducatedWorkers;
        Prefab.m_workPlaceCount1 = CustomizedWorkPlace.EducatedWorkers;
        Prefab.m_workPlaceCount2 = CustomizedWorkPlace.WellEducatedWorkers;
        Prefab.m_workPlaceCount3 = CustomizedWorkPlace.HighlyEducatedWorkers;
    }

    public override void OutputInfo() {
        LogManager.GetLogger().Info($"Unique Factory | Maintenance cost: {MaintenanceCost} -> {Prefab.m_maintenanceCost} | Work space: {WorkPlace.UneducatedWorkers} {WorkPlace.EducatedWorkers} {WorkPlace.WellEducatedWorkers} {WorkPlace.HighlyEducatedWorkers} -> {Prefab.m_workPlaceCount0} {Prefab.m_workPlaceCount1} {Prefab.m_workPlaceCount2} {Prefab.m_workPlaceCount3} | Building: {Name}");
    }

    public void ModifyProductionRate(float factor) => ModDefaultOutputRate = CustomizedOutputRate = (int)(OutputRate * factor);

    public struct UniqueFactoryAIValue {
        public int CostsFactor;
        public WorkPlace WorkPlaceValue;

        public UniqueFactoryAIValue(int costsFactor, WorkPlace workPlaceValue) {
            CostsFactor = costsFactor;
            WorkPlaceValue = workPlaceValue;
        }
    }
}