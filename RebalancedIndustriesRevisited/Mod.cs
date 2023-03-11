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
        public override Version ModVersion => new(0, 7, 0);
        public override ulong ModID => 2911178252;
        public override ulong? BetaID => 2928683738;
        public override string Description => Localize.MOD_Description;

        public override void SetModCulture(CultureInfo cultureInfo) => Localize.Culture = cultureInfo;
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

        private List<IncompatibleModInfo> ConflictMods { get; set; } = new() {
            new IncompatibleModInfo(1562650024, @"Rebalanced Industries", true),
        };

        public override List<ModChangeLog> ChangeLog => new() {
            new ModChangeLog(new Version(0, 7, 0), new(2022, 3, 11), new List<string> {
                Localize.UpdateLog_V0_7_0FIX, Localize.UpdateLog_V0_7_0UPT
            }),
            new ModChangeLog(new Version(0, 6, 0), new(2022, 3, 8), new List<string> {
                Localize.UpdateLog_V0_6_0ADD,Localize.UpdateLog_V0_6_0OPT,Localize.UpdateLog_V0_6_0FIX,
            }),
            new ModChangeLog(new Version(0, 5, 0), new(2022, 2, 11), new List<string> {
                Localize.UpdateLog_V0_5_0ADD,Localize.UpdateLog_V0_5_0FIX1,Localize.UpdateLog_V0_5_0FIX2,
            }),
            new ModChangeLog(new Version(0, 4, 0), new(2022, 2, 5), new List<string> {
                Localize.UpdateLog_V0_4_0FIX1,Localize.UpdateLog_V0_4_0FIX2
            }),
            new ModChangeLog(new Version(0, 3, 0), new(2022, 1, 17), new List<string> {
                Localize.UpdateLog_V0_3_0FIX1,Localize.UpdateLog_V0_3_0FIX2,Localize.UpdateLog_V0_3_0FIX3,Localize.UpdateLog_V0_3_0FIX4,
            }),
            new ModChangeLog(new Version(0, 2, 0), new(2022, 1, 6), new List<string> {
                Localize.UpdateLog_V0_2_0UPT,Localize.UpdateLog_V0_2_0FIX
            }),
            new ModChangeLog(new Version(0, 1, 0), new(2022, 1, 2), new List<string> {
                Localize.UpdateLog_V0_1_0UPT1,Localize.UpdateLog_V0_1_0UPT2,Localize.UpdateLog_V0_1_0FIX
            })
        };
    }

}
