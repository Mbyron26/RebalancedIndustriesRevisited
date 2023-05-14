namespace RebalancedIndustriesRevisited;
using ICities;
using MbyronModsCommon;
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
        CompatibilityCheck.IncompatibleMods = ConflictMods;
        CompatibilityCheck.CheckCompatibility();
        ExternalLogger.OutputPluginsList();
    }
    public override void OnLevelLoaded(LoadMode mode) {
        base.OnLevelLoaded(mode);
        if (!(mode == LoadMode.LoadGame || mode == LoadMode.LoadScenario || mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario)) {
            return;
        }
#if BETA_DEBUG
        DebugUtils.TimeCalculater(Manager.InitializePrefab);
#else
        Manager.InitializePrefab();
#endif
    }
    protected override void SettingsUI(UIHelperBase helper) => OptionPanelManager<Mod, OptionPanel>.SettingsUI(helper);

    private List<IncompatibleModInfo> ConflictMods { get; set; } = new() {
        new IncompatibleModInfo(1562650024, @"Rebalanced Industries", true),
    };

    public override List<ModChangeLog> ChangeLog => new() {
        new ModChangeLog(new Version(0, 8, 0), new(2022, 5, 14), new List<LogString> {
            new(LogFlag.Updated, Localize.UpdateLog_V0_8_0UPT),
            new(LogFlag.Translation, Localize.UpdateLog_V0_8_0TRA)
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
