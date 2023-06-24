namespace RebalancedIndustriesRevisited;
using RebalancedIndustriesRevisited.UI;
using ICities;

public class LoadingExtension : ModLoadingExtension<Mod> {
    public override void LevelLoaded(LoadMode mode) {
        if ((mode == LoadMode.LoadGame || mode == LoadMode.LoadScenario || mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario)) {
            SingletonManager<Manager>.Instance.Init();
            SingletonTool<ToolButtonManager>.Instance.Init();
            ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged += (_) => SingletonTool<ToolButtonManager>.Instance.UUIButtonIsPressed = _;
        }
    }

    public override void LevelUnloading() {
        SingletonTool<ToolButtonManager>.Instance.DeInit();
        ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged -= (_) => SingletonTool<ToolButtonManager>.Instance.UUIButtonIsPressed = _;
    }
}
