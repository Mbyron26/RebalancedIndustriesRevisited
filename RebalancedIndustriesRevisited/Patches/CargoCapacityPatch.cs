namespace RebalancedIndustriesRevisited.Patches;
using HarmonyLib;

public static class CargoCapacityPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(CargoTruckAI), nameof(CargoTruckAI.SetSource)), AccessTools.Method(typeof(CargoCapacityPatch), nameof(SetSourcePrefix)));

    public static void SetSourcePrefix(ref Vehicle data, CargoTruckAI __instance) => SingletonManager<Manager>.Instance.SetCargoCapacity(data, ref __instance);
}
