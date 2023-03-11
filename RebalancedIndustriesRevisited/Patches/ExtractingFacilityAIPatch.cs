using HarmonyLib;
using MbyronModsCommon;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace RebalancedIndustriesRevisited {
    [HarmonyPatch(typeof(ExtractingFacilityAI))]
    public class ExtractingFacilityAIPatch {
        [HarmonyTranspiler]
        [HarmonyPatch("ProduceGoods")]
        public static IEnumerable<CodeInstruction> ExtractingFacilityAI_ProduceGoodsTranspiler(IEnumerable<CodeInstruction> instructions) {
            bool flag = false;
            const int outcomingThreshold = 16000;
            IEnumerator<CodeInstruction> targetEnumerator = instructions.GetEnumerator();
            while (targetEnumerator.MoveNext()) {
                var instruction = targetEnumerator.Current;
                if (instruction.Is(OpCodes.Ldc_I4, 0x1F40) && !flag) {
                    flag = true;
                    yield return new CodeInstruction(OpCodes.Ldc_I4, outcomingThreshold);
                    ModLogger.ModLog("ExtractingFacilityAI ProduceGoods Transpiler succeed.");
                } else {
                    yield return instruction;
                }
            }

        }

        [HarmonyPostfix]
        [HarmonyPatch("GetOutputBufferSize", new Type[] { typeof(DistrictPolicies.Park), typeof(int) })]
        public static void Postfix(ref int __result) {
            if (__result != 0) {
                __result = Mathf.Clamp(__result * 1000 / 1000 * 2, 20000, 60000);
            }
        }

    }
}
