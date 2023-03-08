using CitiesHarmony.API;
using ICities;
using MbyronModsCommon;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RebalancedIndustriesRevisited {
    public class Mod : ModBase<Mod, OptionPanel, Config> {
        public override string SolidModName => "RebalancedIndustriesRevisited";
        public override string ModName => "Rebalanced Industries Revisited";
        public override Version ModVersion => new(0, 6, 0);
        public override ulong ModID => 2911178252;
        public override ulong? BetaID => 2928683738;
        public override string Description => Localize.MOD_Description;
        
        public override void SetModCulture(CultureInfo cultureInfo) =>  Localize.Culture = cultureInfo;
        public override string GetLocale(string text) => Localize.ResourceManager.GetString(text, ModCulture);
        public override void IntroActions() {
            base.IntroActions();
            CompatibilityCheck.IncompatibleMods = ConflictMods;
            CompatibilityCheck.CheckCompatibility();
            ModLogger.OutputPluginsList();
        }
        public override void OnEnabled() {
            base.OnEnabled();
            HarmonyHelper.DoOnHarmonyReady(Patcher.EnablePatches);
        }
        public override void OnDisabled() {
            base.OnDisabled();
            if (HarmonyHelper.IsHarmonyInstalled) {
                Patcher.DisablePatches();
            }
        }
        public override void OnLevelLoaded(LoadMode mode) {
            base.OnLevelLoaded(mode);
            if (!(mode == LoadMode.LoadGame || mode == LoadMode.LoadScenario || mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario)) {
                return;
            }
#if DEBUG
            DebugUtils.TimeCalculater(Manager.InitializePrefab);
#else
            Manager.InitializePrefab();
#endif
        }
        public override List<ModUpdateInfo> ModUpdateLogs { get; set; } = new List<ModUpdateInfo>() {
            new ModUpdateInfo(new Version(0, 6, 0), @"2022/03/08", new List<string> {
                "UpdateLog_V0_6_0ADD","UpdateLog_V0_6_0OPT","UpdateLog_V0_6_0FIX",
            }),
            new ModUpdateInfo(new Version(0, 5, 0), @"2022/02/11", new List<string> {
                "UpdateLog_V0_5_0ADD","UpdateLog_V0_5_0FIX1","UpdateLog_V0_5_0FIX2",
            }),
            new ModUpdateInfo(new Version(0, 4, 0), @"2022/02/05", new List<string> {
                "UpdateLog_V0_4_0FIX1","UpdateLog_V0_4_0FIX2",
            }),
            new ModUpdateInfo(new Version(0, 3, 0), @"2022/01/17", new List<string> {
                "UpdateLog_V0_3_0FIX1","UpdateLog_V0_3_0FIX2","UpdateLog_V0_3_0FIX3","UpdateLog_V0_3_0FIX4"
            }),
             new ModUpdateInfo(new Version(0, 2, 0), @"2022/01/06", new List<string> {
                "UpdateLog_V0_2_0UPT","UpdateLog_V0_2_0FIX"
            }),
            new ModUpdateInfo(new Version(0, 1, 0), @"2022/01/02", new List<string> {
                "UpdateLog_V0_1_0UPT1","UpdateLog_V0_1_0UPT2","UpdateLog_V0_1_0FIX"
            }),
        };
        private List<IncompatibleModInfo> ConflictMods { get; set; } = new() {
            new IncompatibleModInfo(1562650024, @"Rebalanced Industries", true),
        };


    }

}
