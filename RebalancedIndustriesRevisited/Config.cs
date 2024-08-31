using CSShared.Common;
using UnityEngine;

namespace RebalancedIndustriesRevisited;

public class Config : SingletonConfig<Config> {
    public bool OriginalValue { get; set; } = true;
    public bool BothValue { get; set; } = false;
    public float ExtractingFacilityProductionRate { get; set; } = 0.5f;
    public float ProcessingFacilityProductionRate { get; set; } = 0.5f;

    public float RawMaterialsLoadMultiplierFactor { get; set; } = 2f;
    public float ProcessingMaterialsLoadMultiplierFactor { get; set; } = 2f;

    public float RawForestProductsWarehouseTruckMultiplierFactor { get; set; } = 0.6f;
    public float CropsWarehouseTruckMultiplierFactor { get; set; } = 0.5f;
    public float OreWarehouseTruckMultiplierFactor { get; set; } = 0.8f;
    public float OilWarehouseTruckMultiplierFactor { get; set; } = 0.5f;
    public float WarehouseTruckMultiplierFactor { get; set; } = 0.5f;
    public float UniqueFactoryTruckMultiplierFactor { get; set; } = 1f;

    public float GrainFactor { get; set; } = 0.3f;
    public float AnimalProductsFactor { get; set; } = 0.6f;
    public float FloursFactor { get; set; } = 0.7f;
    public float LogsFactor { get; set; } = 0.5f;
    public float PlanedTimberFactor { get; set; } = 0.5f;
    public float PaperFactor { get; set; } = 0.5f;
    public float OreFactor { get; set; } = 0.7f;
    public float MetalsFactor { get; set; } = 0.7f;
    public float GlassFactor { get; set; } = 0.7f;
    public float OilFactor { get; set; } = 0.3f;
    public float PlasticsFactor { get; set; } = 0.7f;
    public float PetroleumFactor { get; set; } = 0.5f;

    public KeyBinding ControlPanelHotkey { get; set; } = new KeyBinding(KeyCode.R, true, true, false);
}