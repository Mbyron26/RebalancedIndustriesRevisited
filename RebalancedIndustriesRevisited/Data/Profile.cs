namespace RebalancedIndustriesRevisited;
using ColossalFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarehouseProfile : ProfileBase<WarehouseAI> {
    public const int MinWarehouseCapacity = 40000;

    public WarehouseProfile(WarehouseAI ai) {
        AI = ai;
        InitializePrefab();
    }

    public override void InitializePrefab() {
        Name = AI.name;
        NewConstructionCost = RawConstructionCost = AI.m_constructionCost;
        NewMaintenanceCost = RawMaintenanceCost = AI.m_maintenanceCost;
        NewTruckCount = RawTruckCount = AI.m_truckCount;
        NewWorkPlace = RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
    }

    public override void RebindParameter() {
        RebindConstructionCost();
        RebindMaintenanceCost();
        RebindWorkPlace();
        RebindTruckCount();
        RebindTooltip();
        OutputInfo();
    }

    private void RebindConstructionCost() => AI.m_constructionCost = NewConstructionCost = Convert.ToInt32(RawConstructionCost * GetCostFactor());

    private void RebindMaintenanceCost() => AI.m_maintenanceCost = NewMaintenanceCost = Convert.ToInt32(RawMaintenanceCost * GetCostFactor());

    private decimal GetCostFactor() => (AI.m_storageType == TransferManager.TransferReason.Grain) ? 0.5m : 1;

    private void RebindWorkPlace() {
        var workPlaceFactor = AI.m_storageType switch {
            TransferManager.TransferReason.Grain => 4m,
            TransferManager.TransferReason.Logs or
            TransferManager.TransferReason.Ore or
            TransferManager.TransferReason.Oil => 2m,
            _ => 1m
        };
        if (NewWorkPlace.Sum() > 10) {
            NewWorkPlace = new(Convert.ToInt32(Math.Round(NewWorkPlace.UneducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(NewWorkPlace.EducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(NewWorkPlace.WellEducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(NewWorkPlace.HighlyEducatedWorkers / workPlaceFactor)));
        }
        AI.m_workPlaceCount0 = NewWorkPlace.UneducatedWorkers;
        AI.m_workPlaceCount1 = NewWorkPlace.EducatedWorkers;
        AI.m_workPlaceCount2 = NewWorkPlace.WellEducatedWorkers;
        AI.m_workPlaceCount3 = NewWorkPlace.HighlyEducatedWorkers;
    }

    public void RebindTruckCount() {
        var factor = SingletonManager<Manager>.Instance.GetWarehouseTruckFactor(AI.m_storageType);
        NewTruckCount = (int)Math.Ceiling(RawTruckCount * factor);
        NewTruckCount = AI.m_storageCapacity <= MinWarehouseCapacity ? Mathf.Max(1, NewTruckCount) : Mathf.Max(2, NewTruckCount);
        AI.m_truckCount = NewTruckCount;
    }

    public void RebindTooltip() {
        if (SingletonManager<Manager>.Instance.IndustryPanelButtons.TryGetValue(Name, out UIButton button)) {
            var rawTooltip = button.tooltip;
            var newTooltip = rawTooltip;
            SingletonManager<Manager>.Instance.ModifyTruckCountString(RawTruckCount, AI.m_truckCount, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyConstructionCostString(RawConstructionCost, AI.m_constructionCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyMaintenanceCostString(RawMaintenanceCost, AI.m_maintenanceCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyWorkSpaceString(RawWorkPlace, NewWorkPlace, ref newTooltip);
            button.tooltip = newTooltip;
            ExternalLogger.DebugMode<Config>($"Rebinding {Name} tooltip:\n{rawTooltip} -> \n{button.tooltip}\n");
        }
    }

    public override void OutputInfo() {
        ExternalLogger.DebugMode<Config>($"Warehouse | Vehicle count: {RawTruckCount} -> {AI.m_truckCount} | Construction cost: {RawConstructionCost} -> {AI.m_constructionCost} | Maintenance cost: {RawMaintenanceCost} -> {AI.m_maintenanceCost} | Work space: {RawWorkPlace.UneducatedWorkers} {RawWorkPlace.EducatedWorkers} {RawWorkPlace.WellEducatedWorkers} {RawWorkPlace.HighlyEducatedWorkers} -> {AI.m_workPlaceCount0} {AI.m_workPlaceCount1} {AI.m_workPlaceCount2} {AI.m_workPlaceCount3} | Building: {Name}");
    }

}

public class ProcessingFacilityProfile : ProfileBase<ProcessingFacilityAI> {
    public static string[] ProcessorField { get; } = { "Animal Pasture 01", "Animal Pasture 02", "Cattle Shed 01" };
    public int RawOutputRate { get; set; }
    public int NewOutputRate { get; set; }

    public ProcessingFacilityProfile(ProcessingFacilityAI ai) {
        AI = ai;
        InitializePrefab();
    }

    public override void InitializePrefab() {
        Name = AI.name;
        NewConstructionCost = RawConstructionCost = AI.m_constructionCost;
        NewMaintenanceCost = RawMaintenanceCost = AI.m_maintenanceCost;
        NewTruckCount = RawTruckCount = AI.m_outputVehicleCount;
        NewWorkPlace = RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
        NewOutputRate = RawOutputRate = AI.m_outputRate;
    }

    private bool IsSpecificBuilding() => ProcessorField.Any(_ => _ == Name);

    public override void RebindParameter() {
        RebindConstructionCost();
        RebindMaintenanceCost();
        RebindWorkPlace();
        RebindTruckCount();
        RebindOutputRate();
        RebindTooltip();
        OutputInfo();
    }

    public void RebindOutputRate() {
        if (RawOutputRate == 0) {
            RawOutputRate = 700;
            InternalLogger.Error($"Raw output raw is zero, fixed to 700, building: {Name}");
        }
        AI.m_outputRate = NewOutputRate = (int)(RawOutputRate * Config.Instance.ProcessingFacilityProductionRate);
    }

    private void RebindConstructionCost() {
        if (!IsSpecificBuilding())
            return;
        AI.m_constructionCost = NewConstructionCost = (int)(RawConstructionCost * 0.5m);
    }

    private void RebindMaintenanceCost() {
        if (!IsSpecificBuilding())
            return;
        AI.m_maintenanceCost = NewMaintenanceCost = (int)(RawMaintenanceCost * 0.5m);
    }

    private void RebindWorkPlace() {
        if (!IsSpecificBuilding())
            return;
        var workersFactor = 0.35m;
        NewWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(AI.m_workPlaceCount0 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount1 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount2 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount3 * workersFactor)));
        AI.m_workPlaceCount0 = NewWorkPlace.UneducatedWorkers;
        AI.m_workPlaceCount1 = NewWorkPlace.EducatedWorkers;
        AI.m_workPlaceCount2 = NewWorkPlace.WellEducatedWorkers;
        AI.m_workPlaceCount3 = NewWorkPlace.HighlyEducatedWorkers;
    }

    public void RebindTruckCount() {
        var truckFactor = SingletonManager<Manager>.Instance.GetTruckFactor(AI.m_outputResource);
        if (truckFactor != 1) {
            AI.m_outputVehicleCount = NewTruckCount = Mathf.Max((int)Math.Ceiling(RawTruckCount * truckFactor), Manager.MinTruckCount);
        }
    }

    public void RebindTooltip() {
        var isIndustry = AI.m_inputResource1 switch {
            TransferManager.TransferReason.Oil or TransferManager.TransferReason.Ore or TransferManager.TransferReason.Logs or TransferManager.TransferReason.Grain => true,
            _ => false
        };
        if (isIndustry && SingletonManager<Manager>.Instance.IndustryPanelButtons.TryGetValue(Name, out UIButton button)) {
            var rawTooltip = button.tooltip;
            var newTooltip = rawTooltip;
            SingletonManager<Manager>.Instance.ModifyTruckCountString(RawTruckCount, AI.m_outputVehicleCount, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyConstructionCostString(RawConstructionCost, AI.m_constructionCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyMaintenanceCostString(RawMaintenanceCost, AI.m_maintenanceCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyWorkSpaceString(RawWorkPlace, NewWorkPlace, ref newTooltip);
            button.tooltip = newTooltip;
            ExternalLogger.DebugMode<Config>($"Rebinding {Name} tooltip:\n{rawTooltip} -> \n{button.tooltip}\n");
        }
    }

    public override void OutputInfo() {
        ExternalLogger.DebugMode<Config>($"Processing Facility | Vehicle count: {RawTruckCount} -> {AI.m_outputVehicleCount} | Construction cost: {RawConstructionCost} -> {AI.m_constructionCost} | Maintenance cost: {RawMaintenanceCost} -> {AI.m_maintenanceCost} | Work space: {RawWorkPlace.UneducatedWorkers} {RawWorkPlace.EducatedWorkers} {RawWorkPlace.WellEducatedWorkers} {RawWorkPlace.HighlyEducatedWorkers} -> {AI.m_workPlaceCount0} {AI.m_workPlaceCount1} {AI.m_workPlaceCount2} {AI.m_workPlaceCount3} | Building: {Name}");
    }
}

public class UniqueFactoryProfile : ProfileBase<UniqueFactoryAI> {
    public UniqueFactoryAIValue ProfileValue { get; private set; }

    public UniqueFactoryProfile(UniqueFactoryAI ai) {
        AI = ai;
        InitializePrefab();
    }

    public override void InitializePrefab() {
        Name = AI.name;
        NewConstructionCost = RawConstructionCost = AI.m_constructionCost;
        NewMaintenanceCost = RawMaintenanceCost = AI.m_maintenanceCost;
        NewTruckCount = RawTruckCount = AI.m_outputVehicleCount;
        RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
        ProfileValue = AI.name switch {
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
            _ => new UniqueFactoryAIValue(1, new WorkPlace()),
        };
    }

    public override void RebindParameter() {
        RebindMaintenanceCost();
        RebindWorkPlace();
        RebindTruckCount();
        RebindTooltip();
        OutputInfo();
    }

    private void RebindMaintenanceCost() {
        if (ProfileValue.CostsFactor == 1)
            return;
        AI.m_maintenanceCost = NewMaintenanceCost = ProfileValue.CostsFactor * 100 / 16;
    }

    private void RebindWorkPlace() {
        if (ProfileValue.CostsFactor == 1)
            return;
        NewWorkPlace = ProfileValue.WorkPlaceValue;
        AI.m_workPlaceCount0 = NewWorkPlace.UneducatedWorkers;
        AI.m_workPlaceCount1 = NewWorkPlace.EducatedWorkers;
        AI.m_workPlaceCount2 = NewWorkPlace.WellEducatedWorkers;
        AI.m_workPlaceCount3 = NewWorkPlace.HighlyEducatedWorkers;
    }

    public void RebindTruckCount() => AI.m_outputVehicleCount = NewTruckCount = Mathf.Max(Mathf.FloorToInt(Config.Instance.UniqueFactoryTruckMultiplierFactor * RawTruckCount), 1);

    public void RebindTooltip() {
        if (SingletonManager<Manager>.Instance.IndustryPanelButtons.TryGetValue(Name, out UIButton button)) {
            var rawTooltip = button.tooltip;
            var newTooltip = rawTooltip;
            SingletonManager<Manager>.Instance.ModifyTruckCountString(RawTruckCount, AI.m_outputVehicleCount, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyConstructionCostString(RawConstructionCost, AI.m_constructionCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyMaintenanceCostString(RawMaintenanceCost, AI.m_maintenanceCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyWorkSpaceString(RawWorkPlace, NewWorkPlace, ref newTooltip);
            button.tooltip = newTooltip;
            ExternalLogger.DebugMode<Config>($"Rebinding {Name} tooltip:\n{rawTooltip} -> \n{button.tooltip}\n");
        }
    }

    public override void OutputInfo() {
        ExternalLogger.DebugMode<Config>($"Unique Factory | Maintenance cost: {RawMaintenanceCost} -> {AI.m_maintenanceCost} | Work space: {RawWorkPlace.UneducatedWorkers} {RawWorkPlace.EducatedWorkers} {RawWorkPlace.WellEducatedWorkers} {RawWorkPlace.HighlyEducatedWorkers} -> {AI.m_workPlaceCount0} {AI.m_workPlaceCount1} {AI.m_workPlaceCount2} {AI.m_workPlaceCount3} | Building: {Name}");
    }

    public struct UniqueFactoryAIValue {
        public int CostsFactor;
        public WorkPlace WorkPlaceValue;
        public UniqueFactoryAIValue(int costsFactor, WorkPlace workPlaceValue) {
            CostsFactor = costsFactor;
            WorkPlaceValue = workPlaceValue;
        }
    }

}

public class ExtractingFacilityProfile : ProfileBase<ExtractingFacilityAI> {
    public int RawOutputRate { get; set; }
    public int NewOutputRate { get; set; }

    public ExtractingFacilityProfile(ExtractingFacilityAI ai) {
        AI = ai;
        InitializePrefab();
    }

    public override void InitializePrefab() {
        Name = AI.name;
        NewConstructionCost = RawConstructionCost = AI.m_constructionCost;
        NewMaintenanceCost = RawMaintenanceCost = AI.m_maintenanceCost;
        NewTruckCount = RawTruckCount = AI.m_outputVehicleCount;
        NewWorkPlace = RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
        NewOutputRate = RawOutputRate = AI.m_outputRate;
    }

    public override void RebindParameter() {
        RebindConstructionCost();
        RebindMaintenanceCost();
        RebindWorkPlace();
        RebindTruckCount();
        RebindOutputRate();
        RebindTooltip();
        OutputInfo();
    }

    public void RebindOutputRate() {
        if (RawOutputRate == 0) {
            RawOutputRate = 700;
            InternalLogger.Error($"Raw output rate is zero, fixed to 700, building: {Name}");
        }
        AI.m_outputRate = NewOutputRate = (int)(RawOutputRate * Config.Instance.ExtractingFacilityProductionRate);
    }

    private void RebindConstructionCost() => AI.m_constructionCost = NewConstructionCost = (int)Math.Ceiling(RawConstructionCost * GetCostFactor());

    private void RebindMaintenanceCost() => AI.m_maintenanceCost = NewMaintenanceCost = (int)Math.Ceiling(RawMaintenanceCost * GetCostFactor());

    private decimal GetCostFactor() => AI.m_outputResource == TransferManager.TransferReason.Grain ? 0.25m : 1m;

    private void RebindWorkPlace() {
        List<int> newWorkSpace = new();
        if (AI.m_outputResource == TransferManager.TransferReason.Grain) {
            var workPlace = new WorkPlace(2, 4, 1, 0);
            int workPlaceSum = (int)Math.Ceiling(Math.Sqrt(AI.m_info.m_cellLength * AI.m_info.m_cellWidth) / 2);
            if (workPlaceSum != 0) {
                foreach (int item in workPlace) {
                    newWorkSpace.Add((int)(GetRatio(item, workPlace.Sum()) * workPlaceSum));
                }
                NewWorkPlace = new WorkPlace(newWorkSpace[0], newWorkSpace[1], newWorkSpace[2], newWorkSpace[3]);
            }
        } else {
            var workersFactor = 0.5m;
            NewWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(AI.m_workPlaceCount0 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount1 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount2 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount3 * workersFactor)));
        }
        AI.m_workPlaceCount0 = NewWorkPlace.UneducatedWorkers;
        AI.m_workPlaceCount1 = NewWorkPlace.EducatedWorkers;
        AI.m_workPlaceCount2 = NewWorkPlace.WellEducatedWorkers;
        AI.m_workPlaceCount3 = NewWorkPlace.HighlyEducatedWorkers;
    }

    public void RebindTruckCount() {
        var truckFactor = SingletonManager<Manager>.Instance.GetTruckFactor(AI.m_outputResource);
        if (truckFactor != 1) {
            AI.m_outputVehicleCount = NewTruckCount = Mathf.Max((int)Math.Ceiling(RawTruckCount * truckFactor), Manager.MinTruckCount);
        }
    }

    public void RebindTooltip() {
        var isIndustry = AI.NaturalResourceType switch {
            NaturalResourceManager.Resource.Oil or NaturalResourceManager.Resource.Ore or NaturalResourceManager.Resource.Forest or NaturalResourceManager.Resource.Fertility => true,
            _ => false
        };
        if (isIndustry && SingletonManager<Manager>.Instance.IndustryPanelButtons.TryGetValue(Name, out UIButton button)) {
            var rawTooltip = button.tooltip;
            var newTooltip = rawTooltip;
            SingletonManager<Manager>.Instance.ModifyTruckCountString(RawTruckCount, AI.m_outputVehicleCount, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyConstructionCostString(RawConstructionCost, AI.m_constructionCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyMaintenanceCostString(RawMaintenanceCost, AI.m_maintenanceCost, AI, ref newTooltip);
            SingletonManager<Manager>.Instance.ModifyWorkSpaceString(RawWorkPlace, NewWorkPlace, ref newTooltip);
            button.tooltip = newTooltip;
            ExternalLogger.DebugMode<Config>($"Rebinding {Name} tooltip:\n{rawTooltip} -> \n{button.tooltip}\n");
        }
    }

    public override void OutputInfo() {
        ExternalLogger.DebugMode<Config>($"Extracting Facility | Vehicle count: {RawTruckCount} -> {AI.m_outputVehicleCount} | Construction cost: {RawConstructionCost} -> {AI.m_constructionCost} | Output rate: {RawOutputRate} -> {AI.m_outputRate} | Maintenance cost: {RawMaintenanceCost} -> {AI.m_maintenanceCost} | Work space: {RawWorkPlace.UneducatedWorkers} {RawWorkPlace.EducatedWorkers} {RawWorkPlace.WellEducatedWorkers} {RawWorkPlace.HighlyEducatedWorkers} -> {AI.m_workPlaceCount0} {AI.m_workPlaceCount1} {AI.m_workPlaceCount2} {AI.m_workPlaceCount3} | Building: {Name}");
    }
}

public abstract class ProfileBase<TypeAI> : IProfile where TypeAI : PlayerBuildingAI {
    public const int MinVehicles = 2;

    public TypeAI AI { get; protected set; }
    public int RawConstructionCost { get; protected set; }
    public int NewConstructionCost { get; protected set; }
    public int RawMaintenanceCost { get; protected set; }
    public int NewMaintenanceCost { get; protected set; }
    public int RawTruckCount { get; protected set; }
    public int NewTruckCount { get; protected set; }
    public WorkPlace RawWorkPlace { get; protected set; }
    public WorkPlace NewWorkPlace { get; protected set; }
    public string Name { get; protected set; }

    public abstract void InitializePrefab();
    public virtual void SetConstructionCost(ref int constructionCost) => constructionCost = NewConstructionCost;
    public virtual void SetMaintenanceCost(ref int maintenanceCost) => maintenanceCost = NewMaintenanceCost;
    public virtual void SetTruckCount(ref int truckCount) => truckCount = NewTruckCount;
    public virtual WorkPlace SetWorkPlace(ref int uneducatedWorkers, ref int educatedWorkers, ref int wellEducatedWorkers, ref int highlyEducatedWorkers) {
        uneducatedWorkers = NewWorkPlace.UneducatedWorkers;
        educatedWorkers = NewWorkPlace.EducatedWorkers;
        wellEducatedWorkers = NewWorkPlace.WellEducatedWorkers;
        highlyEducatedWorkers = NewWorkPlace.HighlyEducatedWorkers;
        return NewWorkPlace;
    }
    public virtual void OutputInfo() { }

    protected static decimal GetTruckFactor(TransferManager.TransferReason material) => material switch {
        TransferManager.TransferReason.Grain => 3m,
        TransferManager.TransferReason.AnimalProducts => 1.5m,
        TransferManager.TransferReason.Flours => 1.5m,
        TransferManager.TransferReason.Logs => 2m,
        TransferManager.TransferReason.PlanedTimber => 2m,
        TransferManager.TransferReason.Paper => 2m,
        TransferManager.TransferReason.Ore => 1.5m,
        TransferManager.TransferReason.Metals => 1.5m,
        TransferManager.TransferReason.Glass => 1.5m,
        TransferManager.TransferReason.Oil => 3m,
        TransferManager.TransferReason.Plastics => 1.5m,
        TransferManager.TransferReason.Petroleum => 2m,
        _ => 0.5m
    };
    protected decimal GetRatio(decimal numerator, int denominator) => Math.Round(numerator / denominator, 2);
    public abstract void RebindParameter();
}

public interface IProfile {
    string Name { get; }
    void InitializePrefab();
    void RebindParameter();
    void SetConstructionCost(ref int constructionCost);
    void SetMaintenanceCost(ref int maintenanceCost);
    void SetTruckCount(ref int truckCount);
    WorkPlace SetWorkPlace(ref int uneducatedWorkers, ref int educatedWorkers, ref int wellEducatedWorkers, ref int highlyEducatedWorkers);
}

public readonly struct WorkPlace : IEnumerable {
    public readonly int UneducatedWorkers;
    public readonly int EducatedWorkers;
    public readonly int WellEducatedWorkers;
    public readonly int HighlyEducatedWorkers;

    public WorkPlace(int uneducatedWorkers, int educatedWorkers, int wellEducatedWorkers, int highlyEducatedWorkers) {
        UneducatedWorkers = uneducatedWorkers;
        EducatedWorkers = educatedWorkers;
        WellEducatedWorkers = wellEducatedWorkers;
        HighlyEducatedWorkers = highlyEducatedWorkers;
    }

    public IEnumerator GetEnumerator() {
        yield return UneducatedWorkers;
        yield return EducatedWorkers;
        yield return WellEducatedWorkers;
        yield return HighlyEducatedWorkers;
    }

    public int Sum() => UneducatedWorkers + EducatedWorkers + WellEducatedWorkers + HighlyEducatedWorkers;
}
