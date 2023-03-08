using ColossalFramework.UI;
using MbyronModsCommon;
using UnityEngine;

namespace RebalancedIndustriesRevisited {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);
            new OptionPanel_General(panel.AddTabs(CommonLocale.OptionPanel_General, CommonLocale.OptionPanel_General, 0, 30).MainPanel, TypeWidth.NormalWidth);
            new AdvancedBase<Mod, Config>(panel.AddTabs(CommonLocale.OptionPanel_Advanced, CommonLocale.OptionPanel_Advanced, 0, 30).MainPanel, TypeWidth.NormalWidth);
        }
    }

    public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
        private UICheckBox OriginalValueCheckBox { get; set; }
        private UICheckBox BothValueCheckBox { get; set; }
        private UILabel ExtractingFacilityLabel { get; set; }
        private UILabel ProcessingFacilityLabel { get; set; }

        public OptionPanel_General(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) {
            AddLocaleDropdown<OptionPanel>(ModInfo);

            var tooltipBoxMode = OptionPanelCard.AddCard(parent, typeWidth, Localize.TooltipBoxMode, out _);
            OriginalValueCheckBox = CustomCheckBox.AddCheckBox(tooltipBoxMode, Localize.OriginalValue, Config.Instance.OriginalValue, 680f, (_) => {
                Config.Instance.OriginalValue = _;
                BothValueCheckBox.isChecked = !_;
            });
            BothValueCheckBox = CustomCheckBox.AddCheckBox(tooltipBoxMode, Localize.BothValue, Config.Instance.BothValue, 680f, (_) => {
                Config.Instance.BothValue = _;
                OriginalValueCheckBox.isChecked = !_;
            });
            CustomLabel.AddLabel(tooltipBoxMode, Localize.ValueWarning, 680f, color: UIColor.Yellow);
            OriginalValueCheckBox.isEnabled = BothValueCheckBox.isEnabled = !(Manager.PrefabFlag);

            var productionRate = OptionPanelCard.AddCard(parent, typeWidth, Localize.ProductionRate, out _);
            CustomLabel.AddLabel(productionRate, Localize.ProductionRateWarning, 700, 1, Color.yellow);
            ExtractingFacilityLabel = CustomLabel.AddLabel(productionRate, CallBack(Config.Instance.ExtractingFacilityProductionRate, Localize.ExtractingFacilityMultiplier), 700, 1, Color.white);
            CustomSlider.AddSliderA(productionRate, new Vector2(680, 20), 0.1f, 2, 0.1f, Config.Instance.ExtractingFacilityProductionRate, (c, v) => {
                Config.Instance.ExtractingFacilityProductionRate = v;
                ExtractingFacilityLabel.text = CallBack(v, Localize.ExtractingFacilityMultiplier);
            });
            ProcessingFacilityLabel = CustomLabel.AddLabel(productionRate, CallBack(Config.Instance.ProcessingFacilityProductionRate, Localize.ProcessingFacilityMultiplier), 700, 1, Color.white);
            CustomSlider.AddSliderA(productionRate, new Vector2(680, 20), 0.1f, 2, 0.1f, Config.Instance.ProcessingFacilityProductionRate, (c, v) => {
                Config.Instance.ProcessingFacilityProductionRate = v;
                ProcessingFacilityLabel.text = CallBack(v, Localize.ProcessingFacilityMultiplier);
            });

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
