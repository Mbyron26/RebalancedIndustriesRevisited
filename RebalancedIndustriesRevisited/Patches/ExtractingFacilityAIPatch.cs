﻿using CSShared.Patch;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace RebalancedIndustriesRevisited.Patches;

public class ExtractingFacilityAIPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.TranspilerPatching(typeof(ExtractingFacilityAI), "ProduceGoods", typeof(ExtractingFacilityAIPatch), nameof(ExtractingFacilityAIProduceGoodsTranspiler));
        harmonyPatcher.TranspilerPatching(typeof(ExtractingFacilityAI), "GetOutputBufferSize", typeof(ExtractingFacilityAIPatch), nameof(ExtractingFacilityAIGetOutputBufferSizeTranspiler), new Type[] { typeof(DistrictPolicies.Park), typeof(int) });
    }

    private static MethodInfo GetOutputLoadMethodInfo => AccessTools.Method(typeof(ExtractingFacilityAIPatch), nameof(GetOutputLoad));

    public static IEnumerable<CodeInstruction> ExtractingFacilityAIProduceGoodsTranspiler(IEnumerable<CodeInstruction> instructions) {
        bool flag = false;
        IEnumerator<CodeInstruction> targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 8000) && !flag) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    public static IEnumerable<CodeInstruction> ExtractingFacilityAIGetOutputBufferSizeTranspiler(IEnumerable<CodeInstruction> instructions) {
        var targetEnumerator = instructions.GetEnumerator();
        while (targetEnumerator.MoveNext()) {
            var instruction = targetEnumerator.Current;
            if (instruction.Is(OpCodes.Ldc_I4, 8000)) {
                instruction = new CodeInstruction(OpCodes.Call, GetOutputLoadMethodInfo);
            }
            yield return instruction;
        }
    }

    private static int GetOutputLoad() => (int)(Config.Instance.RawMaterialsLoadMultiplierFactor * 8000);
}
