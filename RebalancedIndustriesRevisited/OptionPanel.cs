using ColossalFramework;
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
        }
    }
}
