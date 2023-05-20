namespace RebalancedIndustriesRevisited.Patches;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

public class ProcessingFacilityAIPatch {
    private static MethodInfo GetOutputLoadMethodInfo => AccessTools.Method(typeof(ProcessingFacilityAIPatch), nameof(GetOutputLoad));
    private static MethodInfo GetInputLoadMethodInfo => AccessTools.Method(typeof(ProcessingFacilityAIPatch), nameof(GetInputLoad));
    public static MethodInfo GetOriginalProduceGoods() => AccessTools.Method(typeof(ProcessingFacilityAI), "ProduceGoods");
    public static MethodInfo GetProduceGoodsTranspiler() => AccessTools.Method(typeof(ProcessingFacilityAIPatch), nameof(ProduceGoodsTranspiler));

    public static IEnumerable<CodeInstruction> ProduceGoodsTranspiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 4000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetInputLoadMethodInfo);
            } else if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    public static IEnumerable<CodeInstruction> ProcessingFacilityAIGetInputBufferSize1Transpiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    public static IEnumerable<CodeInstruction> ProcessingFacilityAIGetInputBufferSize2Transpiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    public static IEnumerable<CodeInstruction> ProcessingFacilityAIGetInputBufferSize3Transpiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    public static IEnumerable<CodeInstruction> ProcessingFacilityAIGetInputBufferSize4Transpiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    public static IEnumerable<CodeInstruction> ProcessingFacilityAIGetOutputBufferSizeTranspiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    private static int GetInputLoad() => (int)(Config.Instance.RawMaterialsLoadMultiplierFactor * 4000);
    private static int GetOutputLoad() => (int)(Config.Instance.ProcessingMaterialsLoadMultiplierFactor * 8000);
}
