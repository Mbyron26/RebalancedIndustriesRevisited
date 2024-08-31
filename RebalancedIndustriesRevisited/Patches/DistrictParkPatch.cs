using ColossalFramework;
using CSShared.Debug;
using CSShared.Patch;
using HarmonyLib;

namespace RebalancedIndustriesRevisited.Patches;

public class DistrictParkPatch {
    private static bool[] Initialized { get; set; } = new bool[] { false, false, false, false, false, false };

    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(DistrictPark), "IndustrySimulationStep"), AccessTools.Method(typeof(DistrictParkPatch), nameof(IndustrySimulationStepPrefix)));
    }

    public static void IndustrySimulationStepPrefix(byte parkID) {
        if (Singleton<DistrictManager>.exists) {
            var districtManager = Singleton<DistrictManager>.instance;
            var districtPark = districtManager.m_parks.m_buffer[parkID];
            var level = (uint)districtPark.m_parkLevel;
            if (DistrictPark.IsIndustryType(districtPark.m_parkType)) {
                if (Initialized[level]) return;
                Initialized[level] = true;
                var rawWorkerLevelUpRequirement = districtManager.m_properties.m_parkProperties.m_industryLevelInfo[level].m_workerLevelupRequirement;
                var newRequirement = level switch {
                    1 => 75,
                    2 => 200,
                    3 => 400,
                    4 => 650,
                    _ => 0
                };
                districtManager.m_properties.m_parkProperties.m_industryLevelInfo[level].m_workerLevelupRequirement = newRequirement;
                LogManager.GetLogger().Info($"Rebinding worker level up requirement, park type: {districtPark.m_parkType}, level: {level}, level up requirement: {rawWorkerLevelUpRequirement} -> {newRequirement}.");
            }
        }

    }
}
