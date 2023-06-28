namespace RebalancedIndustriesRevisited;

public partial class Manager {
    public void SetCargoCapacity(Vehicle data, CargoTruckAI cargoTruckAI, int capacity) {
        var type = (TransferManager.TransferReason)data.m_transferType;
        var categories = GetIndustriesType(type) switch {
            IndustriesType.Forestry => true,
            IndustriesType.Farming => true,
            IndustriesType.Ore => true,
            IndustriesType.Oil => true,
            _ => false
        };
        if (categories) {
            SetCargoCapacity(cargoTruckAI, capacity);
        }
    }
    public void SetCargoCapacity(Vehicle data, ref CargoTruckAI cargoTruckAI) {
        var type = (TransferManager.TransferReason)data.m_transferType;
        if (IsRawMaterial(type)) {
            cargoTruckAI.m_cargoCapacity = (int)(8000 * Config.Instance.RawMaterialsLoadMultiplierFactor);
        } else if (IsProcessedProduct(type)) {
            cargoTruckAI.m_cargoCapacity = (int)(8000 * Config.Instance.ProcessingMaterialsLoadMultiplierFactor);
        }
    }
    public  bool IsRawMaterial(TransferManager.TransferReason transferReason) => transferReason == TransferManager.TransferReason.Logs || transferReason == TransferManager.TransferReason.Grain || transferReason == TransferManager.TransferReason.Ore || transferReason == TransferManager.TransferReason.Oil;
    public  bool IsProcessedProduct(TransferManager.TransferReason transferReason) => transferReason == TransferManager.TransferReason.Paper || transferReason == TransferManager.TransferReason.PlanedTimber || transferReason == TransferManager.TransferReason.Flours || transferReason == TransferManager.TransferReason.AnimalProducts || transferReason == TransferManager.TransferReason.Metals || transferReason == TransferManager.TransferReason.Glass || transferReason == TransferManager.TransferReason.Plastics || transferReason == TransferManager.TransferReason.Petroleum || transferReason == TransferManager.TransferReason.LuxuryProducts;
    public  void SetCargoCapacity(CargoTruckAI cargoTruckAI, int capacity) => cargoTruckAI.m_cargoCapacity = capacity;
    public  IndustriesType GetIndustriesType(TransferManager.TransferReason transferReason) => transferReason switch {
        TransferManager.TransferReason.Logs or TransferManager.TransferReason.Paper or TransferManager.TransferReason.PlanedTimber => IndustriesType.Forestry,
        TransferManager.TransferReason.Grain or TransferManager.TransferReason.Flours or TransferManager.TransferReason.AnimalProducts => IndustriesType.Farming,
        TransferManager.TransferReason.Ore or TransferManager.TransferReason.Metals or TransferManager.TransferReason.Glass => IndustriesType.Ore,
        TransferManager.TransferReason.Oil or TransferManager.TransferReason.Plastics or TransferManager.TransferReason.Petroleum => IndustriesType.Oil,
        _ => IndustriesType.None,
    };
}

public enum IndustriesType {
    None,
    Forestry,
    Farming,
    Ore,
    Oil,
}