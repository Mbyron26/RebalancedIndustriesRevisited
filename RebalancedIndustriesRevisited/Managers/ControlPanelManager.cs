using System;
using CSLModsCommon.Manager;
using RebalancedIndustriesRevisited.ModSettings;
using RebalancedIndustriesRevisited.UI;

namespace RebalancedIndustriesRevisited.Managers;

internal class ControlPanelManager : ControlPanelManagerBase {
    private SettingManager _settingManager;
    private ModSetting _modSetting;

    public override bool UsingAnimation => false;

    protected override void OnCreate() {
        base.OnCreate();
        _settingManager = Domain.GetOrCreateManager<SettingManager>();
        _modSetting = _settingManager.GetSetting<ModSetting>();
    }

    protected override void OnBeforePanelDestroyed() {
        base.OnBeforePanelDestroyed();
        _settingManager.Save(_modSetting);
    }

    public override Type ResisterPanelType() => typeof(ControlPanel);
}