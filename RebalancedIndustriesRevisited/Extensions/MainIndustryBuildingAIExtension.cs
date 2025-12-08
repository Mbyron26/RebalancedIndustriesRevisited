using RebalancedIndustriesRevisited.Data;

namespace RebalancedIndustriesRevisited.Extensions;

public static class MainIndustryBuildingAIExtension {
    public static WorkPlace GetWorkPlace(this MainIndustryBuildingAI mainIndustryBuildingAI) {
        return new WorkPlace(mainIndustryBuildingAI.m_workPlaceCount0, mainIndustryBuildingAI.m_workPlaceCount1, mainIndustryBuildingAI.m_workPlaceCount2, mainIndustryBuildingAI.m_workPlaceCount3);
    }

    public static void SetWorkPlace(this MainIndustryBuildingAI mainIndustryBuildingAI, WorkPlace workPlace) {
        mainIndustryBuildingAI.m_workPlaceCount0 = workPlace.UneducatedWorkers;
        mainIndustryBuildingAI.m_workPlaceCount1 = workPlace.EducatedWorkers;
        mainIndustryBuildingAI.m_workPlaceCount2 = workPlace.WellEducatedWorkers;
        mainIndustryBuildingAI.m_workPlaceCount3 = workPlace.HighlyEducatedWorkers;
    }
}