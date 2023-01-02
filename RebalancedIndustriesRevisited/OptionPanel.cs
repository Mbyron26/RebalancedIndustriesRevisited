using ColossalFramework.UI;
using MbyronModsCommon;

namespace RebalancedIndustriesRevisited {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);
            new OptionPanel_General(panel.AddTabs(CommonLocale.OptionPanel_General, CommonLocale.OptionPanel_General, 0, 30).MainPanel, TypeWidth.NormalWidth);
            new AdvancedBase<Mod>(panel.AddTabs(CommonLocale.OptionPanel_Advanced, CommonLocale.OptionPanel_Advanced, 0, 30).MainPanel, TypeWidth.NormalWidth);
        }
    }

    public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
        public OptionPanel_General(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) {
            AddLocaleDropdown<OptionPanel>(ModInfo);
        }
    }
}
