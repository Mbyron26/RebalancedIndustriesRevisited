using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using HarmonyLib;
using RebalancedIndustriesRevisited.Managers;

namespace RebalancedIndustriesRevisited.Patches;

public static class CargoCapacityPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.ApplyPrefix(AccessTools.Method(typeof(CargoTruckAI), nameof(CargoTruckAI.SetSource)), AccessTools.Method(typeof(CargoCapacityPatch), nameof(SetSourcePrefix)));

    public static void SetSourcePrefix(ref Vehicle data, CargoTruckAI __instance) => Domain.DefaultDomain.GetOrCreateManager<FacilityManager>().SetCargoCapacity(data, ref __instance);
}