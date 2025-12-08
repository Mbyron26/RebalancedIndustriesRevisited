using RebalancedIndustriesRevisited.Data;

namespace RebalancedIndustriesRevisited.Extensions;

public static class WarehouseAIExtension {
    public static WorkPlace GetWorkPlace(this WarehouseAI warehouseAI) {
        return new WorkPlace(warehouseAI.m_workPlaceCount0, warehouseAI.m_workPlaceCount1, warehouseAI.m_workPlaceCount2, warehouseAI.m_workPlaceCount3);
    }

    public static void SetWorkPlace(this WarehouseAI warehouseAI, WorkPlace workPlace) {
        warehouseAI.m_workPlaceCount0 = workPlace.UneducatedWorkers;
        warehouseAI.m_workPlaceCount1 = workPlace.EducatedWorkers;
        warehouseAI.m_workPlaceCount2 = workPlace.WellEducatedWorkers;
        warehouseAI.m_workPlaceCount3 = workPlace.HighlyEducatedWorkers;
    }
}