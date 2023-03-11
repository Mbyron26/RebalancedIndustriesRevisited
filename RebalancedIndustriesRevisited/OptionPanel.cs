using ColossalFramework.UI;
using MbyronModsCommon;
using UnityEngine;

namespace RebalancedIndustriesRevisited {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);
            new OptionPanel_General(panel.AddTab(CommonLocalize.OptionPanel_General, CommonLocalize.OptionPanel_General, 0, 30, 1.2f).MainPanel, TypeWidth.ShrinkageWidth);
            new AdvancedBase<Mod, Config>(panel.AddTab(CommonLocalize.OptionPanel_Advanced, CommonLocalize.OptionPanel_Advanced, 0, 30, 1.2f).MainPanel, TypeWidth.NormalWidth);
        }
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
            OptionPanelTools.Instance.AddGroup(Parent, width, Localize.ProductionRate);
            OptionPanelTools.Instance.AddSliderGamma(CallBack(Config.Instance.ExtractingFacilityProductionRate, Localize.ExtractingFacilityMultiplier), null, 0.1f, 2, 0.1f, Config.Instance.ExtractingFacilityProductionRate, new Vector2(724, 20), (c, v) => {
                Config.Instance.ExtractingFacilityProductionRate = v;
                extractingFacilityLabel.text = CallBack(v, Localize.ExtractingFacilityMultiplier);
            }, out extractingFacilityLabel, out UILabel _, out UISlider _);
            OptionPanelTools.Instance.AddSliderGamma(CallBack(Config.Instance.ProcessingFacilityProductionRate, Localize.ProcessingFacilityMultiplier), null, 0.1f, 2, 0.1f, Config.Instance.ProcessingFacilityProductionRate, new Vector2(724, 20), (c, v) => {
                Config.Instance.ProcessingFacilityProductionRate = v;
                processingFacilityLabel.text = CallBack(v, Localize.ProcessingFacilityMultiplier);
            }, out processingFacilityLabel, out UILabel _, out UISlider _);
            OptionPanelTools.Instance.Reset();
        }

        private void AddTooltipBoxModeGroup(float width) {
            OptionPanelTools.Instance.AddGroup(Parent, width, Localize.TooltipBoxMode);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.OriginalValue, Localize.OriginalValue, null, _ => {
                Config.Instance.OriginalValue = _;
                BothValueToggleButton.IsChecked = !_;
            }, out UILabel _, out UILabel _, out OriginalValueToggleButton);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.BothValue, Localize.BothValue, null, _ => {
                Config.Instance.BothValue = _;
                OriginalValueToggleButton.IsChecked = !_;
            }, out UILabel _, out UILabel _, out BothValueToggleButton);
            OptionPanelTools.Instance.AddMinorLabel(Localize.ValueWarning);
            OriginalValueToggleButton.isEnabled = BothValueToggleButton.isEnabled = !(Manager.PrefabFlag);
            OptionPanelTools.Instance.Reset();
        }

        private void AddModInfoGroup(float width) {
            OptionPanelTools.Instance.AddGroup(Parent, width, CommonLocalize.OptionPanel_ModInfo);
            OptionPanelTools.Instance.AddLabel($"{CommonLocalize.OptionPanel_Version}: {ModMainInfo<Mod>.ModVersion}", null, out UILabel _, out UILabel _);
            OptionPanelTools.Instance.AddDropDown(CommonLocalize.Language, null, GetLanguages().ToArray(), LanguagesIndex, 310, 30, 1f, out UILabel _, out UILabel _, out UIDropDown dropDown, new RectOffset(6, 10, 6, 0), new RectOffset(6, 6, 4, 0));
            dropDown.eventSelectedIndexChanged += (c, v) => OnLanguageSelectedIndexChanged<OptionPanel>(v);
            OptionPanelTools.Instance.Reset();
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
