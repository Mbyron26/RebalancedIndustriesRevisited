using HarmonyLib;

namespace RebalancedIndustriesRevisited {
    [HarmonyPatch(typeof(WarehouseAI), "GetMaxLoadSize")]
    public class WarehouseAIPatch {
        public static bool Prefix(ref int __result) {
            __result = 16000;
            return false;
        }
    }
}
