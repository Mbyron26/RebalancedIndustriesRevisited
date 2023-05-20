namespace RebalancedIndustriesRevisited;
using ModLocalize = Localize;
using MbyronModsCommon;
using MbyronModsCommon.UI;
using UnityEngine;

public class OptionPanel : OptionPanelBase<Mod, Config, OptionPanel> {
    private CustomUIToggleButton originalValueToggleButton;
    private CustomUIToggleButton bothValueToggleButton;

    protected override void AddExtraContainer() { }
    protected override void FillGeneralContainer() {
        AddTooltipBoxModeOptionsProperty();
        AddProductionRateOptionsProperty();
        AddLoadMultiplierFactorProperty();
    }

    private void AddLoadMultiplierFactorProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, ModLocalize.LoadMultiplierFactor);
        OptionPanelHelper.AddSlider(Config.Instance.RawMaterialsLoadMultiplierFactor.ToString(), null, 0.5f, 2f, 0.1f, Config.Instance.RawMaterialsLoadMultiplierFactor, new Vector2(700, 16), (_) => {
            Config.Instance.RawMaterialsLoadMultiplierFactor = _;
            CallBack();
        }, ModLocalize.RawMaterialsLoadMultiplierFactor + ": ", gradientStyle: true);
        OptionPanelHelper.AddSlider(Config.Instance.ProcessingMaterialsLoadMultiplierFactor.ToString(), null, 0.5f, 2f, 0.1f, Config.Instance.ProcessingMaterialsLoadMultiplierFactor, new Vector2(700, 16), (_) => {
            Config.Instance.ProcessingMaterialsLoadMultiplierFactor = _;
            CallBack();
        }, ModLocalize.ProcessingMaterialsLoadMultiplierFactor + ": ", gradientStyle: true);
        OptionPanelHelper.Reset();
    }

    private void AddProductionRateOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, ModLocalize.TooltipBoxMode);
        var panel0 = OptionPanelHelper.AddToggle(Config.Instance.OriginalValue, ModLocalize.OriginalValue, null, _ => {
            Config.Instance.OriginalValue = _;
            bothValueToggleButton.IsOn = !_;
        });
        originalValueToggleButton = panel0.Child as CustomUIToggleButton;
        var panel1 = OptionPanelHelper.AddToggle(Config.Instance.BothValue, ModLocalize.BothValue, null, _ => {
            Config.Instance.BothValue = _;
            originalValueToggleButton.IsOn = !_;
        });
        bothValueToggleButton = panel1.Child as CustomUIToggleButton;
        OptionPanelHelper.AddMinorLabel(ModLocalize.ValueWarning);
        panel0.isEnabled = panel1.isEnabled = !Manager.PrefabFlag;
        OptionPanelHelper.Reset();
    }

    private void AddTooltipBoxModeOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, ModLocalize.ProductionRate);
        var panel0 = OptionPanelHelper.AddSlider(Config.Instance.ExtractingFacilityProductionRate.ToString(), null, 0.1f, 2f, 0.1f, Config.Instance.ExtractingFacilityProductionRate, new Vector2(700, 16), (_) => {
            Config.Instance.ExtractingFacilityProductionRate = _;
            CallBack();
        }
        , ModLocalize.ExtractingFacilityMultiplierFactor + ": ", gradientStyle: true);
        var panel1 = OptionPanelHelper.AddSlider(Config.Instance.ProcessingFacilityProductionRate.ToString(), null, 0.1f, 2f, 0.1f, Config.Instance.ProcessingFacilityProductionRate, new Vector2(700, 16), (_) => {
            Config.Instance.ProcessingFacilityProductionRate = _;
            CallBack();
        }, ModLocalize.ProcessingFacilityMultiplierFactor + ": ", gradientStyle: true);
        OptionPanelHelper.Reset();
    }
    private static void CallBack() {
        if (Manager.PrefabFlag) {
            Manager.RefreshOutputRate();
        }
    }
}
