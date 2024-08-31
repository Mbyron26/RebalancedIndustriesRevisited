using CSShared.Manager;
using CSShared.Patch;
using HarmonyLib;

namespace RebalancedIndustriesRevisited.Patches;

public class WarehouseAIPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(WarehouseAI), "GetMaxLoadSize"), AccessTools.Method(typeof(WarehouseAIPatch), nameof(MaxLoadSizePrefix)));
    }

    public static bool MaxLoadSizePrefix(WarehouseAI __instance, ref int __result) {
        var storageType = __instance.m_storageType;
        if (ManagerPool.GetOrCreateManager<Manager>().IsRawMaterial(storageType)) {
            __result = (int)(Config.Instance.RawMaterialsLoadMultiplierFactor * 8000);
            return false;
        }
        else if (ManagerPool.GetOrCreateManager<Manager>().IsProcessedProduct(storageType)) {
            __result = (int)(Config.Instance.ProcessingMaterialsLoadMultiplierFactor * 8000);
            return false;
        }
        return true;
    }
}
