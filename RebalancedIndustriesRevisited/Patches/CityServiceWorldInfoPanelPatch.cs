using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using HarmonyLib;
using RebalancedIndustriesRevisited.Managers;

namespace RebalancedIndustriesRevisited.Patches;

public static class CityServiceWorldInfoPanelPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.ApplyPostfix(AccessTools.Method(typeof(CityServiceWorldInfoPanel), "OnSetTarget"), AccessTools.Method(typeof(CityServiceWorldInfoPanelPatch), nameof(OnSetTargetPostfix)));

    public static void OnSetTargetPostfix() {
        Domain.DefaultDomain.GetOrCreateManager<GameInfoPanelManager>().OnSetWorldInfoPanelButton();
    }
}