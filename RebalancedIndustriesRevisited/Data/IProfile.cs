namespace RebalancedIndustriesRevisited.Data;

public interface IProfile {
    FacilityType BuildingType { get; }
    ProfileFlag ProfileTypeSet { get; }
    IndustrialCategory IndustrialCategory { get; }
    string Name { get; set; }
    bool Customized { get; }
    int ConstructionCost { get; set; }
    int ModDefaultConstructionCost { get; set; }
    int CustomizedConstructionCost { get; set; }
    int MaintenanceCost { get; set; }
    int ModDefaultMaintenanceCost { get; set; }
    int CustomizedMaintenanceCost { get; set; }
    int TruckCount { get; set; }
    int ModDefaultTruckCount { get; set; }
    int CustomizedTruckCount { get; set; }
    int OutputRate { get; set; }
    int ModDefaultOutputRate { get; set; }
    int CustomizedOutputRate { get; set; }
    WorkPlace WorkPlace { get; set; }
    WorkPlace ModDefaultWorkPlace { get; set; }
    WorkPlace CustomizedWorkPlace { get; set; }
    int StorageCapacity { get; set; }
    int ModDefaultStorageCapacity { get; set; }
    int CustomizedStorageCapacity { get; set; }
    int BoatCount { get; set; }
    int ModDefaultBoatCount { get; set; }
    int CustomizedBoatCount { get; set; }
    void GetPrefab();
    void SetModDefaults();
    void SetModCustomized();
    void SetGameDefaults();
}