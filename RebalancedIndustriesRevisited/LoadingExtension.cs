using CSShared.Extension;
using CSShared.Manager;
using CSShared.UI.ControlPanel;
using ICities;
using RebalancedIndustriesRevisited.UI;

namespace RebalancedIndustriesRevisited;

public class LoadingExtension : ModLoadingExtension<Mod> {
    public override void LevelLoaded(LoadMode mode) {
        if ((mode == LoadMode.LoadGame || mode == LoadMode.LoadScenario || mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario)) {
            ManagerPool.GetOrCreateManager<Manager>().Update();
            ManagerPool.GetOrCreateManager<ToolButtonManager>().Enable();
            ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged += (_) => ManagerPool.GetOrCreateManager<ToolButtonManager>().UUIButtonIsPressed = _;
        }
        ManagerPool.GetOrCreateManager<Manager>().ReloadCheck();
    }

    public override void LevelUnloading() {
        ManagerPool.GetOrCreateManager<ToolButtonManager>().Disable();
        ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged -= (_) => ManagerPool.GetOrCreateManager<ToolButtonManager>().UUIButtonIsPressed = _;
    }
}
