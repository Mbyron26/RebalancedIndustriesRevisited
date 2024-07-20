namespace RebalancedIndustriesRevisited.Patches;
using HarmonyLib;

public class WarehouseAIPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(WarehouseAI), "GetMaxLoadSize"), AccessTools.Method(typeof(WarehouseAIPatch), nameof(MaxLoadSizePrefix)));
    }

    public static bool MaxLoadSizePrefix(WarehouseAI __instance, ref int __result) {
        var storageType = __instance.m_storageType;
        if (SingletonManager<Manager>.Instance.IsRawMaterial(storageType)) {
            __result = (int)(Config.Instance.RawMaterialsLoadMultiplierFactor * 8000);
            return false;
        }
        else if (SingletonManager<Manager>.Instance.IsProcessedProduct(storageType)) {
            __result = (int)(Config.Instance.ProcessingMaterialsLoadMultiplierFactor * 8000);
            return false;
        }
        return true;
    }
}
