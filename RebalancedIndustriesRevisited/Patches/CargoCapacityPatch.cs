using HarmonyLib;

namespace RebalancedIndustriesRevisited {
    [HarmonyPatch(typeof(CargoTruckAI), nameof(CargoTruckAI.SetSource))]
    public class CargoCapacityPatch {
        public static void Prefix(ref Vehicle data, CargoTruckAI __instance) {
            Manager.SetCargoCapacity(data, __instance, 16000);
        }

    }
}
