using System;
using System.Collections.Generic;
using CSLModsCommon;
using CSLModsCommon.Compatibility;
using CSLModsCommon.Localization;
using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using RebalancedIndustriesRevisited.ModSettings;
using RebalancedIndustriesRevisited.Patches;

namespace RebalancedIndustriesRevisited.Managers;

public class ModManager : PatchModManagerBase {
    public override string ModName => "Rebalanced Industries Revisited";
    public override string RowDescription => "Rebalances Industries DLC, reduce traffic flow, increase cargo loading and more.";
    public override DateTime VersionDate { get; } = new(2025, 12, 16);
    public override string ModTranslationURL => "https://crowdin.com/project/rebalanced-industries-revisite";
    public override string ModSteamURL => "https://steamcommunity.com/sharedfiles/filedetails/?id=2911178252";

    protected override void OnUpdateMangers(UpdateManager updateManager) {
        base.OnUpdateMangers(updateManager);
        updateManager.UpdateAt<ControlPanelManager>(UpdatePhase.Default);
        updateManager.UpdateAt<FacilityManager>(UpdatePhase.Serialize);
        updateManager.UpdateAt<GameInfoPanelManager>(UpdatePhase.Default);
        updateManager.UpdateAt<InGameToolButtonManager>(UpdatePhase.Default);
        updateManager.UpdateAt<ReloadManager>(UpdatePhase.Default);
    }

    protected override void OnCreateSettings(SettingManager settingManager) {
        settingManager.Load<ModSetting>();
    }

    protected override void RegisterPatches(HarmonyPatcher harmonyPatcher) {
        CargoCapacityPatch.Patch(harmonyPatcher);
        DistrictParkPatch.Patch(harmonyPatcher);
        ExtractingFacilityAIPatch.Patch(harmonyPatcher);
        ProcessingFacilityAIPatch.Patch(harmonyPatcher);
        WarehouseAIPatch.Patch(harmonyPatcher);
        CityServiceWorldInfoPanelPatch.Patch(harmonyPatcher);
    }

    protected override void AddVersionModRule(IVersionModRule rule) {
        base.AddVersionModRule(rule);
        rule.Set(1, 20, 1, 1);
    }

    protected override void AddIncompatibleModRule(IIncompatibleModRule rule) {
        base.AddIncompatibleModRule(rule);
        rule.Add("CSL_RebalancedIndustriesDistricts", IncompatibilityModLevel.EnableNotAllowed, "Rebalanced Industries");
    }

    protected override List<ChangelogCollection> GenerateChangelogs() => [
        new(new Version(1, 0, 2), new DateTime(2025, 12, 16)),
        new(new Version(1, 0, 1), new DateTime(2025, 12, 14)),
        new ChangelogCollection(new Version(1, 0, 0), new DateTime(2025, 12, 8)).AddEntry(ChangelogFlag.Updated, new FormattedString(nameof(SharedTranslations.UpdatedToCSLModsCommon), "1.0"))
            .AddEntry(ChangelogFlag.Updated, new FormattedString(nameof(SharedTranslations.UpdatedToGameVersion), "1.20.1"))
    ];
}