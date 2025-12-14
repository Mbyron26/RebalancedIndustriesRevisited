using RebalancedIndustriesRevisited.Data;

namespace RebalancedIndustriesRevisited.Extensions;

public static class FishingHarborAIExtension {
    public static WorkPlace GetWorkPlace(this FishingHarborAI ai) => new(ai.m_workPlaceCount0, ai.m_workPlaceCount1, ai.m_workPlaceCount2, ai.m_workPlaceCount3);

    public static void SetWorkPlace(this FishingHarborAI ai, WorkPlace workPlace) {
        ai.m_workPlaceCount0 = workPlace.UneducatedWorkers;
        ai.m_workPlaceCount1 = workPlace.EducatedWorkers;
        ai.m_workPlaceCount2 = workPlace.WellEducatedWorkers;
        ai.m_workPlaceCount3 = workPlace.HighlyEducatedWorkers;
    }
}