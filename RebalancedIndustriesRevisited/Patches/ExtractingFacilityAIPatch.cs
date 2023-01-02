using HarmonyLib;
using MbyronModsCommon;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RebalancedIndustriesRevisited {
    [HarmonyPatch(typeof(ExtractingFacilityAI))]
    public class ExtractingFacilityAIPatch {
        [HarmonyTranspiler]
        [HarmonyPatch("ProduceGoods")]
        public static IEnumerable<CodeInstruction> ExtractingFacilityAI_ProduceGoodsTranspiler(IEnumerable<CodeInstruction> instructions) {
            IEnumerator<CodeInstruction> targetEnumerator = instructions.GetEnumerator();
            while (targetEnumerator.MoveNext()) {
                var instruction = targetEnumerator.Current;
                if (instruction.Is(OpCodes.Ldc_I4, 0x1F40)) {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, 0x3e80);
                    ModLogger.ModLog("ExtractingFacilityAI ProduceGoods Transpiler succeed.");
                } else {
                    yield return instruction;
                }
            }

#if EarlyTest
            //var codes = instructions.ToList();
            //codes[727].operand = 0x3e80;
            //return codes.AsEnumerable();
#endif

        }
    }
}
