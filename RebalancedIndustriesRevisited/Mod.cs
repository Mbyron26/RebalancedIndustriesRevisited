using CitiesHarmony.API;
using ICities;
using MbyronModsCommon;
using System;
using System.Collections.Generic;

namespace RebalancedIndustriesRevisited {
    public class Mod : ModBase<Mod, OptionPanel, Config> {
        public override string SolidModName => "RebalancedIndustriesRevisited";
        public override string ModName => "Rebalanced Industries Revisited";
        public override Version ModVersion => new(0, 1, 0);
        public override ulong ModID => 0;
        public override string Description => Localize.MOD_Description;
        public override List<ModUpdateInfo> ModUpdateLogs { get; set; } = new() {
            new ModUpdateInfo(new Version(0, 1, 0), @"2022/01/02", new List<string> {
                "UpdateLog_V0_1_0UPT1","UpdateLog_V0_1_0UPT2","UpdateLog_V0_1_0FIX"
            }),
        };

        public override string GetLocale(string text) {
            return "";
        }
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

        private List<IncompatibleModInfo> ConflictMods { get; set; } = new() {
            new IncompatibleModInfo(1562650024, @"Rebalanced Industries", true),
        };


    }

}
