namespace RebalancedIndustriesRevisited;
using ColossalFramework.UI;

public partial class Manager {
    public const int MinTruckCount = 2;

    public void RefreshTruckCount() {
        if (!IsInit) {
            return;
        }
        ExtractingFacilityAICache.ForEach(_ => _.Value.RebindTruckCount());
        ProcessingFacilityAICache.ForEach(_ => _.Value.RebindTruckCount());
        WarehouseAICache.ForEach(_ => _.Value.RebindTruckCount());
    }

    public float GetWarehouseTruckFactor(TransferManager.TransferReason material) => material switch {
        TransferManager.TransferReason.Grain => Config.Instance.CropsWarehouseTruckMultiplierFactor,
        TransferManager.TransferReason.Logs => Config.Instance.RawForestProductsWarehouseTruckMultiplierFactor,
        TransferManager.TransferReason.Ore => Config.Instance.OreWarehouseTruckMultiplierFactor,
        TransferManager.TransferReason.Oil => Config.Instance.OilWarehouseTruckMultiplierFactor,
        _ => Config.Instance.WarehouseTruckMultiplierFactor
    };

    public float GetTruckFactor(TransferManager.TransferReason material) => material switch {
        TransferManager.TransferReason.Grain => Config.Instance.GrainFactor,
        TransferManager.TransferReason.AnimalProducts => Config.Instance.AnimalProductsFactor,
        TransferManager.TransferReason.Flours => Config.Instance.FloursFactor,
        TransferManager.TransferReason.Logs => Config.Instance.LogsFactor,
        TransferManager.TransferReason.PlanedTimber => Config.Instance.PlanedTimberFactor,
        TransferManager.TransferReason.Paper => Config.Instance.PaperFactor,
        TransferManager.TransferReason.Ore => Config.Instance.OreFactor,
        TransferManager.TransferReason.Metals => Config.Instance.MetalsFactor,
        TransferManager.TransferReason.Glass => Config.Instance.GlassFactor,
        TransferManager.TransferReason.Oil => Config.Instance.OilFactor,
        TransferManager.TransferReason.Plastics => Config.Instance.PlasticsFactor,
        TransferManager.TransferReason.Petroleum => Config.Instance.PetroleumFactor,
        _ => 1
    };

}
