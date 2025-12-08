using System.IO;
using CSLModsCommon.Localization;
using CSLModsCommon.Setting;
using CSLModsCommon.ToolButton;
using CSLModsCommon.UI.Containers;
using CSLModsCommon.UI.Dialogs;
using CSLModsCommon.UI.OptionsPanel;
using RebalancedIndustriesRevisited.Localization;
using RebalancedIndustriesRevisited.Managers;
using RebalancedIndustriesRevisited.ModSettings;

namespace RebalancedIndustriesRevisited.UI;

public class OptionsPanel : OptionsPanelBase {
    private ModSetting _modSetting;

    private readonly string _facilitySettingsPath = Path.Combine(FileLocationAttribute.DefaultDirectory, "FacilitySetting") + ".json";

    protected override void CacheManagers() {
        base.CacheManagers();
        _modSetting = _settingManager.GetSetting<ModSetting>();
    }

    protected override void FillKeyBindingPage(ScrollContainer page) {
        base.FillKeyBindingPage(page);
        AddSection(page).AddKeyBinding(_modSetting.ControlPanelToggle, SharedTranslations.ToggleControlPanel, SharedTranslations.ToggleControlPanelDescription);
    }

    protected override InGameToolManagerBase GetInGameToolManager() => _domain.GetOrCreateManager<InGameToolButtonManager>();

    protected override void FillGeneralPage(ScrollContainer page) {
        base.FillGeneralPage(page);

        AddInGameToolButtonSection(value => _domain.GetOrCreateManager<InGameToolButtonManager>().OnButtonStatuesChanged(value));

        AddSection(page).AddButton(Translations.OtherFunctionsMajor, Translations.OtherFunctionsMinor, Translations.OpenControlPanel, null, 30, _ => _domain.GetOrCreateManager<ControlPanelManager>().TogglePanel());

        AddSection(page).AddButton(Translations.ResetIndustriesCustomizedSettings, null, SharedTranslations.Reset, null, 30, _ => OnResetIndustriesCustomizedSettingsButtonClick());
    }

    private void OnResetIndustriesCustomizedSettingsButtonClick() {
        var hasCustomizedSettings = File.Exists(_facilitySettingsPath);
        if (!hasCustomizedSettings) {
            _dialogManager.Show<OkDialog>().AddContent(_modManagerBase.ModName, Translations.NoCustomizedIndustriesSettings);
            return;
        }

        _dialogManager.Show<ConfirmDialog>().AddContent(_modManagerBase.ModName, Translations.SureToResetCustomizedIndustriesSettings, OnResetIndustriesCustomizedSettingsButtonClickConfirm);
    }

    private void OnResetIndustriesCustomizedSettingsButtonClickConfirm() {
        File.Delete(_facilitySettingsPath);
        _logger.Info("Deleted customized industries settings file.");
        _dialogManager.Show<OkDialog>().AddContent(_modManagerBase.ModName, Translations.ResetCompleted);
    }
}