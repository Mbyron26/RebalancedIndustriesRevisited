using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using HarmonyLib;
using RebalancedIndustriesRevisited.ModSettings;

namespace RebalancedIndustriesRevisited.Patches;

public class ProcessingFacilityAIPatch {
    private static MethodInfo GetOutputLoadMethodInfo => AccessTools.Method(typeof(ProcessingFacilityAIPatch), nameof(GetOutputLoad));
    private static MethodInfo GetInputLoadMethodInfo => AccessTools.Method(typeof(ProcessingFacilityAIPatch), nameof(GetInputLoad));

    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.ApplyTranspiler(AccessTools.Method(typeof(ProcessingFacilityAI), "ProduceGoods"), AccessTools.Method(typeof(ProcessingFacilityAIPatch), nameof(ProduceGoodsTranspiler)));
        harmonyPatcher.ApplyTranspiler(typeof(ProcessingFacilityAI), "GetOutputBufferSize", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIGetOutputBufferSizeTranspiler), new[] { typeof(DistrictPolicies.Park), typeof(int) });
        harmonyPatcher.ApplyTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize1", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIGetInputBufferSize1Transpiler), new[] { typeof(DistrictPolicies.Park), typeof(int) });
        harmonyPatcher.ApplyTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize2", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIGetInputBufferSize2Transpiler), new[] { typeof(DistrictPolicies.Park), typeof(int) });
        harmonyPatcher.ApplyTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize3", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIGetInputBufferSize3Transpiler), new[] { typeof(DistrictPolicies.Park), typeof(int) });
        harmonyPatcher.ApplyTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize4", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIGetInputBufferSize4Transpiler), new[] { typeof(DistrictPolicies.Park), typeof(int) });
    }

    public static IEnumerable<CodeInstruction> ProduceGoodsTranspiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 4000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetInputLoadMethodInfo);
            }
            else if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
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

    private static int GetInputLoad() => (int)(Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().RawMaterialsLoadMultiplierFactor * 4000);
    private static int GetOutputLoad() => (int)(Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().ProcessingMaterialsLoadMultiplierFactor * 8000);
}
