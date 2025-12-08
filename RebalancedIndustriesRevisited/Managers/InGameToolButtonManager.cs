using System;
using CSLModsCommon.KeyBindings;
using CSLModsCommon.Manager;
using CSLModsCommon.ToolButton;
using RebalancedIndustriesRevisited.ModSettings;
using RebalancedIndustriesRevisited.UI;

namespace RebalancedIndustriesRevisited.Managers;

internal class InGameToolButtonManager : InGameToolManagerBase {
    private const string InfoPanelButton = "InfoPanelButton";
    private ModSetting _modSetting;

    protected override KeyBinding ToggleKeyBinding => _modSetting.ControlPanelToggle;

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
    }

    protected override void Enable() {
        base.Enable();
        RegisterTriggerSource(InfoPanelButton, val => IsInGameButtonOn = IsUnifiedUIButtonOn = val);
    }

    protected override void Disable() {
        base.Disable();
        UnregisterTriggerSource(InfoPanelButton);
    }

    protected override PanelManagerBase CreatePanelManager() => Domain.GetOrCreateManager<ControlPanelManager>();

    protected override Type GetToolButtonType() => typeof(ToolButton);
}