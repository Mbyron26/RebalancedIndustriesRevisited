using CSShared.Manager;
using CSShared.Patch;
using HarmonyLib;

namespace RebalancedIndustriesRevisited.Patches;

public static class CargoCapacityPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(CargoTruckAI), nameof(CargoTruckAI.SetSource)), AccessTools.Method(typeof(CargoCapacityPatch), nameof(SetSourcePrefix)));

    public static void SetSourcePrefix(ref Vehicle data, CargoTruckAI __instance) => ManagerPool.GetOrCreateManager<Manager>().SetCargoCapacity(data, ref __instance);
}
