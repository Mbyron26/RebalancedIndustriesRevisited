namespace RebalancedIndustriesRevisited.Patches;
using HarmonyLib;
using System.Reflection;

public class WarehouseAIPatch {
    public static MethodInfo GetOriginalGetMaxLoadSize() => AccessTools.Method(typeof(WarehouseAI), "GetMaxLoadSize");
    public static MethodInfo GetMaxLoadSizePrefix() => AccessTools.Method(typeof(WarehouseAIPatch), nameof(MaxLoadSizePrefix));
    public static bool MaxLoadSizePrefix(WarehouseAI __instance, ref int __result) {
        var storageType = __instance.m_storageType;
        if (SingletonManager<Manager>.Instance.IsRawMaterial(storageType)) {
            __result = (int)(Config.Instance.RawMaterialsLoadMultiplierFactor * 8000);
            return false;
        } else if (SingletonManager<Manager>.Instance.IsProcessedProduct(storageType)) {
            __result = (int)(Config.Instance.ProcessingMaterialsLoadMultiplierFactor * 8000);
            return false;
        }
        return true;
    }
}
