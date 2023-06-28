namespace RebalancedIndustriesRevisited.Patches;
using HarmonyLib;
using System.Reflection;

public class CargoCapacityPatch {
    public static MethodInfo GetOriginalSetSource() => AccessTools.Method(typeof(CargoTruckAI), nameof(CargoTruckAI.SetSource));
    public static MethodInfo GetSetSourcePrefix() => AccessTools.Method(typeof(CargoCapacityPatch), nameof(SetSourcePrefix));
    public static void SetSourcePrefix(ref Vehicle data, CargoTruckAI __instance) => SingletonManager<Manager>.Instance.SetCargoCapacity(data, ref __instance);
}
