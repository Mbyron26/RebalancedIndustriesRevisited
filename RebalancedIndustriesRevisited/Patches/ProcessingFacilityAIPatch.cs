using HarmonyLib;
using MbyronModsCommon;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace RebalancedIndustriesRevisited {

    [HarmonyPatch(typeof(ProcessingFacilityAI))]
    public class ProcessingFacilityAIPatch {
        [HarmonyTranspiler]
        [HarmonyPatch("ProduceGoods")]
        public static IEnumerable<CodeInstruction> ProcessingFacilityAI_ProduceGoodsTranspiler(IEnumerable<CodeInstruction> instructions/*, ILGenerator iL*/) {
            //MethodInfo prefabCount = AccessTools.Method(typeof(PlayerBuildingAI), nameof(PlayerBuildingAI.GetProductionRate));
            //LocalBuilder transferOffer = iL.DeclareLocal(typeof(TransferManager.TransferOffer));
            bool flag1 = false;
            const int incomingThreshold = 17000;
            const int outcomingThreshold = 16000;
            var targetEnumerator = instructions.GetEnumerator();
            while (targetEnumerator.MoveNext()) {
                var instruction = targetEnumerator.Current;
                if (instruction.Is(OpCodes.Ldc_I4, 0x1F40) && !flag1) {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, incomingThreshold/*0x4268*/);
                    flag1 = true;
                    ModLogger.ModLog("ProcessingFacilityAI ProduceGoods Transpiler1 succeed.");
                } else if (instruction.Is(OpCodes.Ldc_I4, 0x1F40)) {
                    targetEnumerator.MoveNext();
                    var next = targetEnumerator.Current;
                    if (next.opcode == OpCodes.Blt) {
                        targetEnumerator.MoveNext();
                        var next1 = targetEnumerator.Current;
                        targetEnumerator.MoveNext();
                        var next2 = targetEnumerator.Current;
                        targetEnumerator.MoveNext();
                        var next3 = targetEnumerator.Current;
                        if (next3.opcode == OpCodes.Bge) {
                            yield return new CodeInstruction(OpCodes.Ldc_I4, outcomingThreshold);
                            yield return next;
                            yield return next1;
                            yield return next2;
                            yield return next3;
                            ModLogger.ModLog("ProcessingFacilityAI ProduceGoods Transpiler2 succeed.");
                        } else {
                            yield return instruction;
                            yield return next;
                            yield return next1;
                            yield return next2;
                            yield return next3;
                        }

                    } else {
                        yield return instruction;
                        yield return next;
                    }
                } else {
                    yield return instruction;
                }
            }

#if EarlyTest
            //var codes = instructions.ToList();
            //codes[1348].operand = 0x3e80;
            //return codes.AsEnumerable();
#endif

        }


        //[HarmonyPrefix]
        //[HarmonyPatch("GetInputBufferSize1", new Type[] { typeof(DistrictPolicies.Park), typeof(int) })]
        //public static void Prefix1(ProcessingFacilityAI __instance) {
        //    if (__instance.m_inputRate1 == 200) {
        //        __instance.m_inputRate1 = 625;
        //    } else if (__instance.m_inputRate1 == 300) {
        //        __instance.m_inputRate1 = 875;
        //    } else if (__instance.m_inputRate1 == 150) {
        //        __instance.m_inputRate1 = 562;
        //    } else if (__instance.m_inputRate1 == 250) {
        //        __instance.m_inputRate1 = 750;
        //    }
        //}

        //[HarmonyPrefix]
        //[HarmonyPatch("GetOutputBufferSize", new Type[] { typeof(DistrictPolicies.Park), typeof(int) })]
        //public static void Prefix2(ProcessingFacilityAI __instance) {
        //    if (__instance.m_outputRate == 200) {
        //        __instance.m_outputRate = 625;
        //    } else if (__instance.m_outputRate == 400) {
        //        __instance.m_outputRate = 1125;
        //    } else if (__instance.m_outputRate == 150) {
        //        __instance.m_outputRate = 562;
        //    } else if (__instance.m_outputRate == 250) {
        //        __instance.m_outputRate = 750;
        //    }
        //    //SetOutputRate(ref __instance.m_outputRate);
        //}

        //public static void SetOutputRate(ref int outputRate) {
            
        //    if (outputRate <= 250) {
        //        var modified = ((outputRate * 32 + 8000) * 100 + 50) / 100 * 2;
        //        outputRate = ((modified * 100 - 50) / 100 - 8000) / 32;
        //    }
        //}


#if POSSIBLE_DEPRECATED
        private static FieldInfo inputRate1Field = AccessTools.Field(typeof(ProcessingFacilityAI), nameof(ProcessingFacilityAI.m_inputRate1));
        private static MethodInfo retargetingInputRate1 = AccessTools.Method(typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIPatch.RebindInputRate1));
        [HarmonyTranspiler]
        [HarmonyPatch("ProduceGoods", new Type[] { typeof(DistrictPolicies.Park), typeof(int) })]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var found = false;
            foreach (var instruction in instructions) {
                if (instruction.StoresField(inputRate1Field)) {
                    yield return new CodeInstruction(OpCodes.Call, retargetingInputRate1);
                    found = true;
                }
                yield return instruction;
            }
            if (found is false) {
                ModLogger.ModLog("Couldn't find inputRate1Field");
            }
        }
        public static int RebindInputRate1() => 300;
#endif
    }



}
