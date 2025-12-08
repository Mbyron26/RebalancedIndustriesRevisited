using System;
using CSLModsCommon.Logging;
using Newtonsoft.Json;

namespace RebalancedIndustriesRevisited.Data;

public abstract class ProfileBase<TypePrefab> : IProfile where TypePrefab : PlayerBuildingAI {
    protected const int MinTruckCount = 2;
    protected int _customizedConstructionCost;
    protected int _customizedMaintenanceCost;
    protected int _customizedTruckCount;
    protected int _customizedOutputRate;
    protected int _customizedStorageCapacity;
    protected WorkPlace _customizedWorkPlace = new();

    [JsonIgnore] public abstract FacilityType BuildingType { get; }
    protected ILog Logger { get; } = LogManager.GetLogger();
    protected TypePrefab Prefab { get; set; }
    [JsonIgnore] public ProfileType ProfileTypeSet => GetProfileTypeSet();
    [JsonIgnore] public abstract IndustrialCategory IndustrialCategory { get; protected set; }
    [JsonIgnore] public string Name { get; set; }
    [JsonIgnore] public bool Customized { get; protected set; }
    [JsonIgnore]  public int ConstructionCost { get; set; }
    [JsonIgnore] public int ModDefaultConstructionCost { get; set; }
    public virtual int CustomizedConstructionCost { get; set; }
    [JsonIgnore] public int MaintenanceCost { get; set; }
    [JsonIgnore] public int ModDefaultMaintenanceCost { get; set; }
    public virtual int CustomizedMaintenanceCost { get; set; }
    [JsonIgnore] public int TruckCount { get; set; }
    [JsonIgnore] public int ModDefaultTruckCount { get; set; }
    public virtual int CustomizedTruckCount { get; set; }
    [JsonIgnore] public int OutputRate { get; set; }
    [JsonIgnore] public int ModDefaultOutputRate { get; set; }
    public virtual int CustomizedOutputRate { get; set; }
    [JsonIgnore] public WorkPlace WorkPlace { get; set; } = new();
    [JsonIgnore] public WorkPlace ModDefaultWorkPlace { get; set; } = new();
    public virtual WorkPlace CustomizedWorkPlace { get; set; }
    [JsonIgnore] public int StorageCapacity { get; set; }
    [JsonIgnore] public int ModDefaultStorageCapacity { get; set; }
    public virtual int CustomizedStorageCapacity { get; set; }

    public abstract void GetPrefab();

    protected virtual ProfileType GetProfileTypeSet() {
        if (_customizedConstructionCost == ConstructionCost && _customizedMaintenanceCost == MaintenanceCost && _customizedTruckCount == TruckCount && _customizedOutputRate == OutputRate && _customizedWorkPlace.Equals(WorkPlace))
            return ProfileType.GameDefault;
        if (_customizedConstructionCost == ModDefaultConstructionCost && _customizedMaintenanceCost == ModDefaultMaintenanceCost && _customizedTruckCount == ModDefaultTruckCount && _customizedOutputRate == ModDefaultOutputRate && _customizedWorkPlace.Equals(ModDefaultWorkPlace)) {
            if (BuildingType == FacilityType.WarehouseFacility) {
                return _customizedStorageCapacity != ModDefaultStorageCapacity ? ProfileType.Customized : ProfileType.ModDefault;
            }

            return ProfileType.ModDefault;
        }

        return ProfileType.Customized;
    }

    public void SetFromLoadData(IProfile profile) {
        CustomizedConstructionCost = profile.CustomizedConstructionCost;
        CustomizedMaintenanceCost = profile.CustomizedMaintenanceCost;
        CustomizedTruckCount = profile.CustomizedTruckCount;
        CustomizedOutputRate = profile.CustomizedOutputRate;
        CustomizedWorkPlace = profile.CustomizedWorkPlace;
        if (profile.BuildingType == FacilityType.WarehouseFacility) {
            CustomizedStorageCapacity = profile.CustomizedStorageCapacity;
        }
    }

    public virtual void SetFromModData() { }

    public virtual void SetGameDefaults() {
        CustomizedConstructionCost = ConstructionCost;
        CustomizedMaintenanceCost = MaintenanceCost;
        CustomizedTruckCount = TruckCount;
        CustomizedOutputRate = OutputRate;
        CustomizedWorkPlace = WorkPlace;
        if (BuildingType == FacilityType.WarehouseFacility) {
            CustomizedStorageCapacity = StorageCapacity;
        }
    }

    public virtual void SetModDefaults() {
        SetConstructionCost();
        SetMaintenanceCost();
        SetTruckCount();
        SetOutputRate();
        SetWorkPlace();
        SetStorageCapacity();
    }

    public virtual void SetModCustomized() { }

    protected virtual void SetWorkPlace() {
        CustomizedWorkPlace = ModDefaultWorkPlace;
    }

    protected virtual void SetStorageCapacity() {
        CustomizedStorageCapacity = ModDefaultStorageCapacity;
    }

    protected virtual void SetOutputRate() {
        CustomizedOutputRate = ModDefaultOutputRate;
    }

    protected virtual void SetTruckCount() {
        CustomizedTruckCount = ModDefaultTruckCount;
    }

    protected virtual void SetMaintenanceCost() {
        CustomizedMaintenanceCost = ModDefaultMaintenanceCost;
    }

    protected virtual void SetConstructionCost() {
        CustomizedConstructionCost = ModDefaultConstructionCost;
    }

    public virtual void Validate() {
        if (ModDefaultConstructionCost != CustomizedConstructionCost || ModDefaultMaintenanceCost != CustomizedMaintenanceCost || ModDefaultTruckCount != CustomizedTruckCount || ModDefaultOutputRate != CustomizedOutputRate || ModDefaultWorkPlace != CustomizedWorkPlace || ModDefaultStorageCapacity != CustomizedStorageCapacity) {
            Customized = true;
        }
    }

    public virtual void OutputInfo() { }

    protected float GetTruckFactor(TransferManager.TransferReason material) => material switch {
        TransferManager.TransferReason.Grain => 0.3f,
        TransferManager.TransferReason.AnimalProducts => 0.6f,
        TransferManager.TransferReason.Flours => 0.7f,
        TransferManager.TransferReason.Logs => 0.5f,
        TransferManager.TransferReason.PlanedTimber => 0.5f,
        TransferManager.TransferReason.Paper => 0.5f,
        TransferManager.TransferReason.Ore => 0.7f,
        TransferManager.TransferReason.Metals => 0.7f,
        TransferManager.TransferReason.Glass => 0.7f,
        TransferManager.TransferReason.Oil => 0.3f,
        TransferManager.TransferReason.Plastics => 0.7f,
        TransferManager.TransferReason.Petroleum => 0.5f,
        _ => 1
    };

    protected decimal GetRatio(decimal numerator, int denominator) => denominator == 0 ? 1 : Math.Round(numerator / denominator, 2);
}