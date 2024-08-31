using ColossalFramework.UI;
using CSShared.Debug;
using CSShared.Localization;

namespace RebalancedIndustriesRevisited;

public partial class Manager {
    public void ReloadCheck() {
        var loadButton = UIView.library.Get<PauseMenu>("PauseMenu")?.Find<UIPanel>("Menu")?.Find<UIButton>("LoadGame");
        if (loadButton is not null && loadButton.enabled) {
            loadButton.tooltip = ModLocalizationManager.Localize("LoadGame");
            loadButton.tooltipBox = UIView.library.Get("DefaultTooltip");
            loadButton.Disable();
            LogManager.GetLogger().Info("Disable LoadGame button");
        }
        var toMainMenu = UIView.library.Get<ExitConfirmPanel>("ExitConfirmPanel")?.Find<UIButton>("ToMainMenu");
        if (toMainMenu is not null && toMainMenu.enabled) {
            toMainMenu.tooltip = ModLocalizationManager.Localize("ToMainMenu");
            toMainMenu.tooltipBox = UIView.library.Get("DefaultTooltip");
            toMainMenu.Disable();
            LogManager.GetLogger().Info("Disable ToMainMenu button");
        }

    }
}
