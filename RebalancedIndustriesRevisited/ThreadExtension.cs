using CSShared.Extension;
using CSShared.UI.ControlPanel;
using RebalancedIndustriesRevisited.UI;

namespace RebalancedIndustriesRevisited;

public class ThreadExtension : ModThreadExtensionBase {
    private bool toggleControlPanel;

    public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
        AddCallOnceInvoke(Config.Instance.ControlPanelHotkey.IsPressed(), ref toggleControlPanel, ControlPanelManager<Mod, ControlPanel>.CallPanel);
    }
}
