using ColossalFramework.UI;

namespace RebalancedIndustriesRevisited;

public partial class Manager {
    public void ReloadCheck() {
        var loadButton = UIView.library.Get<PauseMenu>("PauseMenu")?.Find<UIPanel>("Menu")?.Find<UIButton>("LoadGame");
        if (loadButton is not null && loadButton.enabled) {
            loadButton.tooltip = Localize.LoadGame;
            loadButton.tooltipBox = UIView.library.Get("DefaultTooltip");
            loadButton.Disable();
            Mod.Log.Info("Disable LoadGame button");
        }
        var toMainMenu = UIView.library.Get<ExitConfirmPanel>("ExitConfirmPanel")?.Find<UIButton>("ToMainMenu");
        if (toMainMenu is not null && toMainMenu.enabled) {
            toMainMenu.tooltip = Localize.ToMainMenu;
            toMainMenu.tooltipBox = UIView.library.Get("DefaultTooltip");
            toMainMenu.Disable();
            Mod.Log.Info("Disable ToMainMenu button");
        }

    }
}
