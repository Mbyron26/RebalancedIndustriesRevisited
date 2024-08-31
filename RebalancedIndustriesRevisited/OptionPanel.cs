using CSShared.Common;
using CSShared.Manager;
using CSShared.UI;
using CSShared.UI.ControlPanel;
using CSShared.UI.OptionPanel;
using RebalancedIndustriesRevisited.UI;

namespace RebalancedIndustriesRevisited;

public class OptionPanel : OptionPanelBase<Mod, Config, OptionPanel> {
    private CustomUIToggleButton originalValueToggleButton;
    private CustomUIToggleButton bothValueToggleButton;

    protected override void FillHotkeyContainer() {
        base.FillHotkeyContainer();
        OptionPanelHelper.AddGroup(HotkeyContainer, Localize("OptionPanel_Hotkeys"));
        OptionPanelHelper.AddKeymapping(Localize("ShowControlPanel"), Config.Instance.ControlPanelHotkey);
        OptionPanelHelper.Reset();
    }

    protected override void OnModLocaleChanged() => ControlPanelManager<Mod, ControlPanel>.OnLocaleChanged();
    protected override void FillGeneralContainer() {
        AddTooltipBoxModeOptions();
        AddToolButtonOptions<ToolButtonManager>();
        AddOtherFunctionOptions();
    }

    protected override void ToolButtonDropDownCallBack(int value) {
        base.ToolButtonDropDownCallBack(value);
        if (!SingletonMod<Mod>.Instance.IsLevelLoaded) {
            return;
        }
        ManagerPool.GetOrCreateManager<ToolButtonManager>().Disable();
        ManagerPool.GetOrCreateManager<ToolButtonManager>().Enable();
    }

    private void AddOtherFunctionOptions() {
        OptionPanelHelper.AddGroup(GeneralContainer, null);
        OptionPanelHelper.AddButton(Localize("OtherFunctionsMajor"), Localize("OtherFunctionsMinor"), Localize("OpenControlPanel"), null, 30, () => {
            ControlPanelManager<Mod, ControlPanel>.CallPanel();
        });
        OptionPanelHelper.Reset();
    }


    private void AddTooltipBoxModeOptions() {
        OptionPanelHelper.AddGroup(GeneralContainer, Localize("TooltipBoxMode"));
        var panel0 = OptionPanelHelper.AddToggle(Config.Instance.OriginalValue, Localize("OriginalValue"), null, _ => {
            Config.Instance.OriginalValue = _;
            bothValueToggleButton.IsOn = !_;
        });
        originalValueToggleButton = panel0.Child as CustomUIToggleButton;
        var panel1 = OptionPanelHelper.AddToggle(Config.Instance.BothValue, Localize("BothValue"), null, _ => {
            Config.Instance.BothValue = _;
            originalValueToggleButton.IsOn = !_;
        });
        bothValueToggleButton = panel1.Child as CustomUIToggleButton;
        OptionPanelHelper.AddMinorLabel(Localize("ValueWarning"));
        panel0.isEnabled = panel1.isEnabled = !SingletonMod<Mod>.Instance.IsLevelLoaded;
        OptionPanelHelper.Reset();
    }
}