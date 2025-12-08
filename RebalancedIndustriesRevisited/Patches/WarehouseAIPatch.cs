using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using HarmonyLib;
using RebalancedIndustriesRevisited.Managers;
using RebalancedIndustriesRevisited.ModSettings;

namespace RebalancedIndustriesRevisited.Patches;

public class WarehouseAIPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.ApplyPrefix(AccessTools.Method(typeof(WarehouseAI), "GetMaxLoadSize"), AccessTools.Method(typeof(WarehouseAIPatch), nameof(MaxLoadSizePrefix)));
    }

    public static bool MaxLoadSizePrefix(WarehouseAI __instance, ref int __result) {
        var storageType = __instance.m_storageType;
        if (Domain.DefaultDomain.GetOrCreateManager<FacilityManager>().IsRawMaterial(storageType)) {
            __result = (int)(Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().RawMaterialsLoadMultiplierFactor * 8000);
            return false;
        }
        else if (Domain.DefaultDomain.GetOrCreateManager<FacilityManager>().IsProcessedProduct(storageType)) {
            __result = (int)(Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().ProcessingMaterialsLoadMultiplierFactor * 8000);
            return false;
        }
        return true;
    }
}
