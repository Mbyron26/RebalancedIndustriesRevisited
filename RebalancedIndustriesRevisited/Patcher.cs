using HarmonyLib;

namespace RebalancedIndustriesRevisited {
    public static class Patcher {
        private const string HARMONYID = @"com.mbyron26.RebalancedIndustriesRevisited";
        public static void EnablePatches() {
            Harmony harmony = new(HARMONYID); 
            harmony.PatchAll();
        }
        public static void DisablePatches() {
            Harmony harmony = new(HARMONYID);
            harmony.UnpatchAll(HARMONYID);
        }
    }
}
