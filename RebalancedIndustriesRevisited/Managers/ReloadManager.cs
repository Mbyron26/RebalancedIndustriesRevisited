using ColossalFramework.UI;
using CSLModsCommon;
using CSLModsCommon.Manager;
using RebalancedIndustriesRevisited.Localization;

namespace RebalancedIndustriesRevisited.Managers;

public class ReloadManager : ManagerBase {
    protected override void OnGameLoaded(LoadContext context) {
        base.OnGameLoaded(context);
        var loadButton = UIView.library.Get<PauseMenu>("PauseMenu")?.Find<UIPanel>("Menu")?.Find<UIButton>("LoadGame");
        if (loadButton is not null && loadButton.enabled) {
            loadButton.tooltip = Translations.LoadGame;
            loadButton.tooltipBox = UIView.library.Get("DefaultTooltip");
            loadButton.Disable();
            Logger.Info("Disable LoadGame button");
        }

        var toMainMenu = UIView.library.Get<ExitConfirmPanel>("ExitConfirmPanel")?.Find<UIButton>("ToMainMenu");
        if (toMainMenu is null || !toMainMenu.enabled) return;
        toMainMenu.tooltip = Translations.ToMainMenu;
        toMainMenu.tooltipBox = UIView.library.Get("DefaultTooltip");
        toMainMenu.Disable();
        Logger.Info("Disable ToMainMenu button");
    }
}