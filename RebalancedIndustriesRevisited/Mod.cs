using CSShared.Common;
using CSShared.Patch;
using CSShared.UI.OptionPanel;
using ICities;
using RebalancedIndustriesRevisited.Patches;
using System;
using System.Collections.Generic;

namespace RebalancedIndustriesRevisited;

public class Mod : ModPatcherBase<Mod, Config> {
    public override string ModName => "Rebalanced Industries Revisited";
    public override ulong StableID => 2911178252;
    public override ulong? BetaID => 2928683738;
    public override string Description => Localize("MOD_Description");
#if BETA_DEBUG
    public override BuildVersion VersionType => BuildVersion.BetaDebug;
#elif BETA_RELEASE
    public override BuildVersion VersionType => BuildVersion.BetaRelease;
#elif STABLE_DEBUG
    public override BuildVersion VersionType => BuildVersion.StableDebug;
#else
    public override BuildVersion VersionType => BuildVersion.StableRelease;
#endif

    protected override void SettingsUI(UIHelperBase helper) => OptionPanelManager<Mod, OptionPanel>.SettingsUI(helper);

    protected override void PatchAction(HarmonyPatcher harmonyPatcher) {
        CargoCapacityPatch.Patch(harmonyPatcher);
        DistrictParkPatch.Patch(harmonyPatcher);
        ExtractingFacilityAIPatch.Patch(harmonyPatcher);
        ProcessingFacilityAIPatch.Patch(harmonyPatcher);
        WarehouseAIPatch.Patch(harmonyPatcher);
    }

    public override List<ConflictModInfo> ConflictMods { get; set; } = new() {
        new ConflictModInfo(1562650024, @"Rebalanced Industries", true),
    };

    public override List<ChangelogInfo> Changelog => new() {
        new ChangelogInfo(new Version(0, 9, 2), new(2024, 8, 31), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("Changelog_0_9_2_0")),
            new(ChangelogFlag.Updated, Localize("Changelog_0_9_2_1")),
            new(ChangelogFlag.Translation, Localize("Changelog_0_9_2_2")),
        }),
        new ChangelogInfo(new Version(0, 9, 1), new(2024, 7, 20), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("Changelog_0_9_1_0")),
            new(ChangelogFlag.Fixed, Localize("Changelog_0_9_1_1")),
        }),
        new ChangelogInfo(new Version(0, 9, 0), new(2023, 6, 24), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("UpdateLog_V0_9UPT0")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V0_9ADD0")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V0_9ADD1")),
            new(ChangelogFlag.Optimized, Localize("UpdateLog_V0_9OPT0")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_9FIX0")),
        }),
        new ChangelogInfo(new Version(0, 8, 0), new(2023, 5, 23), new List<ChangelogContent> {
            new(ChangelogFlag.Updated,Localize("Changelog_0_8_0_0")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V0_8_0ADD")),
            new(ChangelogFlag.Updated, Localize("UpdateLog_V0_8_0UPT")),
            new(ChangelogFlag.Optimized, Localize("UpdateLog_V0_8_0OPT")),
            new(ChangelogFlag.Translation, Localize("UpdateLog_V0_8_0TRA")),
            new(ChangelogFlag.Translation, Localize("UpdateLog_V0_8_0TRA1"))
        }),
        new ChangelogInfo(new Version(0, 7, 1), new(2023, 3, 22), new List<ChangelogContent> {
            new(ChangelogFlag.Updated,Localize("Changelog_0_7_1_0")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V0_7_1ADD"))
        }),
        new ChangelogInfo(new Version(0, 7, 0), new(2023, 3, 11), new List<ChangelogContent> {
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_7_0FIX")),
            new(ChangelogFlag.Updated, Localize("UpdateLog_V0_7_0UPT")),
        }),
        new ChangelogInfo(new Version(0, 6, 0), new(2023, 3, 8), new List<ChangelogContent> {
            new(ChangelogFlag.Added, Localize("UpdateLog_V0_6_0ADD")),
            new(ChangelogFlag.Optimized, Localize("UpdateLog_V0_6_0OPT")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_6_0FIX")),
        }),
        new ChangelogInfo(new Version(0, 5, 0), new(2023, 2, 11), new List<ChangelogContent> {
            new(ChangelogFlag.Added, Localize("UpdateLog_V0_5_0ADD")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_5_0FIX1")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_5_0FIX2")),
        }),
        new ChangelogInfo(new Version(0, 4, 0), new(2023, 2, 5), new List<ChangelogContent> {
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_4_0FIX1")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_4_0FIX2")),
        }),
        new ChangelogInfo(new Version(0, 3, 0), new(2023, 1, 17), new List<ChangelogContent> {
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_3_0FIX1")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_3_0FIX2")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_3_0FIX3")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_3_0FIX4")),
        }),
        new ChangelogInfo(new Version(0, 2, 0), new(2023, 1, 6), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("UpdateLog_V0_2_0UPT")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_2_0FIX")),
        }),
        new ChangelogInfo(new Version(0, 1, 0), new(2023, 1, 2), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("UpdateLog_V0_1_0UPT1")),
            new(ChangelogFlag.Updated, Localize("UpdateLog_V0_1_0UPT2")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V0_1_0FIX")),
        })
    };
}
