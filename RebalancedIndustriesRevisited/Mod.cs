global using MbyronModsCommon;
namespace RebalancedIndustriesRevisited;
using ICities;
using RebalancedIndustriesRevisited.Patches;
using System;
using System.Collections.Generic;
using System.Globalization;

public class Mod : ModPatcherBase<Mod, Config> {
    public override string ModName => "Rebalanced Industries Revisited";
    public override ulong StableID => 2911178252;
    public override ulong? BetaID => 2928683738;
    public override string Description => Localize.MOD_Description;
#if BETA_DEBUG
    public override BuildVersion VersionType => BuildVersion.BetaDebug;
#elif BETA_RELEASE
    public override BuildVersion VersionType => BuildVersion.BetaRelease;
#elif STABLE_DEBUG
    public override BuildVersion VersionType => BuildVersion.StableDebug;
#else
    public override BuildVersion VersionType => BuildVersion.StableRelease;
#endif

    public override void SetModCulture(CultureInfo cultureInfo) => Localize.Culture = cultureInfo;
    public override void IntroActions() {
        base.IntroActions();
        ExternalLogger.OutputPluginsList();
    }

    protected override void SettingsUI(UIHelperBase helper) => OptionPanelManager<Mod, OptionPanel>.SettingsUI(helper);

    protected override void PatchAction() {
        AddTranspiler(typeof(ExtractingFacilityAI), "ProduceGoods", typeof(ExtractingFacilityAIPatch), nameof(ExtractingFacilityAIPatch.ExtractingFacilityAIProduceGoodsTranspiler));
        AddTranspiler(typeof(ExtractingFacilityAI), "GetOutputBufferSize", typeof(ExtractingFacilityAIPatch), nameof(ExtractingFacilityAIPatch.ExtractingFacilityAIGetOutputBufferSizeTranspiler), new Type[] { typeof(DistrictPolicies.Park), typeof(int) });
        AddPrefix(CargoCapacityPatch.GetOriginalSetSource(), CargoCapacityPatch.GetSetSourcePrefix());
        AddTranspiler(ProcessingFacilityAIPatch.GetOriginalProduceGoods(), ProcessingFacilityAIPatch.GetProduceGoodsTranspiler());
        AddTranspiler(typeof(ProcessingFacilityAI), "GetOutputBufferSize", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIPatch.ProcessingFacilityAIGetOutputBufferSizeTranspiler), new Type[] { typeof(DistrictPolicies.Park), typeof(int) });
        AddTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize1", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIPatch.ProcessingFacilityAIGetInputBufferSize1Transpiler), new Type[] { typeof(DistrictPolicies.Park), typeof(int) });
        AddTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize2", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIPatch.ProcessingFacilityAIGetInputBufferSize2Transpiler), new Type[] { typeof(DistrictPolicies.Park), typeof(int) });
        AddTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize3", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIPatch.ProcessingFacilityAIGetInputBufferSize3Transpiler), new Type[] { typeof(DistrictPolicies.Park), typeof(int) });
        AddTranspiler(typeof(ProcessingFacilityAI), "GetInputBufferSize4", typeof(ProcessingFacilityAIPatch), nameof(ProcessingFacilityAIPatch.ProcessingFacilityAIGetInputBufferSize4Transpiler), new Type[] { typeof(DistrictPolicies.Park), typeof(int) });
        AddPrefix(WarehouseAIPatch.GetOriginalGetMaxLoadSize(), WarehouseAIPatch.GetMaxLoadSizePrefix());
    }

    public override List<ConflictModInfo> ConflictMods { get; set; } = new() {
        new ConflictModInfo(1562650024, @"Rebalanced Industries", true),
    };

    public override List<ModChangeLog> ChangeLog => new() {
        new ModChangeLog(new Version(0, 9, 0), new(2022, 6, 24), new List<LogString> {
            new(LogFlag.Updated, Localize.UpdateLog_V0_9UPT0),
            new(LogFlag.Added, Localize.UpdateLog_V0_9ADD0),
            new(LogFlag.Added, Localize.UpdateLog_V0_9ADD1),
            new(LogFlag.Optimized, Localize.UpdateLog_V0_9OPT0),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_9FIX0),
        }),
        new ModChangeLog(new Version(0, 8, 0), new(2022, 5, 23), new List<LogString> {
            new(LogFlag.Updated,"Updated to support game version 1.17.0"),
            new(LogFlag.Added, Localize.UpdateLog_V0_8_0ADD),
            new(LogFlag.Updated, Localize.UpdateLog_V0_8_0UPT),
            new(LogFlag.Optimized, Localize.UpdateLog_V0_8_0OPT),
            new(LogFlag.Translation, Localize.UpdateLog_V0_8_0TRA),
            new(LogFlag.Translation, Localize.UpdateLog_V0_8_0TRA1)
        }),
        new ModChangeLog(new Version(0, 7, 1), new(2022, 3, 22), new List<LogString> {
            new(LogFlag.Updated,"[UPT]Updated to support game version 1.16.1"),
            new(LogFlag.Added, Localize.UpdateLog_V0_7_1ADD)
        }),
        new ModChangeLog(new Version(0, 7, 0), new(2022, 3, 11), new List<LogString> {
            new(LogFlag.Fixed, Localize.UpdateLog_V0_7_0FIX),
            new(LogFlag.Updated, Localize.UpdateLog_V0_7_0UPT),
        }),
        new ModChangeLog(new Version(0, 6, 0), new(2022, 3, 8), new List<LogString> {
            new(LogFlag.Added, Localize.UpdateLog_V0_6_0ADD),
            new(LogFlag.Optimized, Localize.UpdateLog_V0_6_0OPT),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_6_0FIX),
        }),
        new ModChangeLog(new Version(0, 5, 0), new(2022, 2, 11), new List<LogString> {
            new(LogFlag.Added, Localize.UpdateLog_V0_5_0ADD),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_5_0FIX1),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_5_0FIX2),
        }),
        new ModChangeLog(new Version(0, 4, 0), new(2022, 2, 5), new List<LogString> {
            new(LogFlag.Fixed, Localize.UpdateLog_V0_4_0FIX1),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_4_0FIX2),
        }),
        new ModChangeLog(new Version(0, 3, 0), new(2022, 1, 17), new List<LogString> {
            new(LogFlag.Fixed, Localize.UpdateLog_V0_3_0FIX1),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_3_0FIX2),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_3_0FIX3),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_3_0FIX4),
        }),
        new ModChangeLog(new Version(0, 2, 0), new(2022, 1, 6), new List<LogString> {
            new(LogFlag.Updated, Localize.UpdateLog_V0_2_0UPT),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_2_0FIX),
        }),
        new ModChangeLog(new Version(0, 1, 0), new(2022, 1, 2), new List<LogString> {
            new(LogFlag.Updated, Localize.UpdateLog_V0_1_0UPT1),
            new(LogFlag.Updated, Localize.UpdateLog_V0_1_0UPT2),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_1_0FIX),
        })
    };
}
