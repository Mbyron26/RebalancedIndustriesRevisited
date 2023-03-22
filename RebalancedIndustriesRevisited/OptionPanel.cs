using ColossalFramework.UI;
using MbyronModsCommon;
using UnityEngine;

namespace RebalancedIndustriesRevisited {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);
            new OptionPanel_General(panel.AddTab(CommonLocalize.OptionPanel_General, CommonLocalize.OptionPanel_General, 0, 30, 1.2f).MainPanel, TypeWidth.NormalWidth);
            new OptionPanel_Advanced(panel.AddTab(CommonLocalize.OptionPanel_Advanced, CommonLocalize.OptionPanel_Advanced, 0, 30, 1.2f).MainPanel, TypeWidth.NormalWidth);
        }
    }

    public class OptionPanel_Advanced : AdvancedBase<Mod, Config> {
        public OptionPanel_Advanced(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) { }
        protected override void ResetSettings() => ResetSettings<OptionPanel>();
    }

    public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
        private ToggleButton OriginalValueToggleButton;
        private ToggleButton BothValueToggleButton;
        private UILabel extractingFacilityLabel;
        private UILabel processingFacilityLabel;

        public OptionPanel_General(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) {
            var defaultWidth = (float)typeWidth;
            AddModInfoGroup(defaultWidth);
            AddTooltipBoxModeGroup(defaultWidth);
            AddProductionRateGroup(defaultWidth);
        }

        private void AddProductionRateGroup(float width) {
            OptionPanelTool.AddGroup(Parent, width, Localize.ProductionRate);
            OptionPanelTool.AddSliderGamma(CallBack(Config.Instance.ExtractingFacilityProductionRate, Localize.ExtractingFacilityMultiplier), null, 0.1f, 2, 0.1f, Config.Instance.ExtractingFacilityProductionRate, new Vector2(724, 20), (c, v) => {
                Config.Instance.ExtractingFacilityProductionRate = v;
                extractingFacilityLabel.text = CallBack(v, Localize.ExtractingFacilityMultiplier);
            }, out extractingFacilityLabel, out UILabel _, out UISlider _);
            OptionPanelTool.AddSliderGamma(CallBack(Config.Instance.ProcessingFacilityProductionRate, Localize.ProcessingFacilityMultiplier), null, 0.1f, 2, 0.1f, Config.Instance.ProcessingFacilityProductionRate, new Vector2(724, 20), (c, v) => {
                Config.Instance.ProcessingFacilityProductionRate = v;
                processingFacilityLabel.text = CallBack(v, Localize.ProcessingFacilityMultiplier);
            }, out processingFacilityLabel, out UILabel _, out UISlider _);
            OptionPanelTool.Reset();
        }

        private void AddTooltipBoxModeGroup(float width) {
            OptionPanelTool.AddGroup(Parent, width, Localize.TooltipBoxMode);
            OptionPanelTool.AddToggleButton(Config.Instance.OriginalValue, Localize.OriginalValue, null, _ => {
                Config.Instance.OriginalValue = _;
                BothValueToggleButton.IsChecked = !_;
            }, out UILabel _, out UILabel _, out OriginalValueToggleButton);
            OptionPanelTool.AddToggleButton(Config.Instance.BothValue, Localize.BothValue, null, _ => {
                Config.Instance.BothValue = _;
                OriginalValueToggleButton.IsChecked = !_;
            }, out UILabel _, out UILabel _, out BothValueToggleButton);
            OptionPanelTool.AddMinorLabel(Localize.ValueWarning);
            OriginalValueToggleButton.isEnabled = BothValueToggleButton.isEnabled = !(Manager.PrefabFlag);
            OptionPanelTool.Reset();
        }

        private void AddModInfoGroup(float width) {
            OptionPanelTool.AddGroup(Parent, width, CommonLocalize.OptionPanel_ModInfo);
            OptionPanelTool.AddLabel($"{CommonLocalize.OptionPanel_Version}: {ModMainInfo<Mod>.ModVersion}({ModMainInfo<Mod>.VersionType})", null, out UILabel _, out UILabel _);
            OptionPanelTool.AddDropDown(CommonLocalize.Language, null, GetLanguages().ToArray(), LanguagesIndex, 310, 30, out UILabel _, out UILabel _, out UIDropDown _, (v) => OnLanguageSelectedIndexChanged<OptionPanel>(v));
            OptionPanelTool.Reset();
        }

        static string CallBack(float value, string type) {
            if (Manager.PrefabFlag) {
                Manager.RefreshOutputRate();
            }
            if (value == 1) {
                return $"{type}: {Localize.Vanilla}";
            } else {
                return $"{type}: {value}";
            }
        }
    }
}
