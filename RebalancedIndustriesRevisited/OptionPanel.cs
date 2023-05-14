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
        var panel0 = OptionPanelHelper.AddSlider(Config.Instance.ExtractingFacilityProductionRate.ToString(), null, 0.1f, 2f, 0.1f, Config.Instance.ExtractingFacilityProductionRate, new Vector2(700, 16), (_) => Config.Instance.ExtractingFacilityProductionRate = _
        , ModLocalize.ExtractingFacilityMultiplier + ": ", gradientStyle: true);
        var panel1 = OptionPanelHelper.AddSlider(Config.Instance.ProcessingFacilityProductionRate.ToString(), null, 0.1f, 2f, 0.1f, Config.Instance.ProcessingFacilityProductionRate, new Vector2(700, 16), (_) => Config.Instance.ProcessingFacilityProductionRate = _, ModLocalize.ProcessingFacilityMultiplier + ": ", gradientStyle: true);
        OptionPanelHelper.Reset();
    }

    private string CallBack(float value, string type) {
        if (Manager.PrefabFlag) {
            Manager.RefreshOutputRate();
        }
        if (value == 1) {
            return $"{type}: {ModLocalize.Vanilla}";
        } else {
            return $"{type}: {value}";
        }
    }
}


//public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
//    private ToggleButton OriginalValueToggleButton;
//    private ToggleButton BothValueToggleButton;
//    private UILabel extractingFacilityLabel;
//    private UILabel processingFacilityLabel;

//    public OptionPanel_General(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) {
//        var defaultWidth = (float)typeWidth;
//        AddModInfoGroup(defaultWidth);
//        AddTooltipBoxModeGroup(defaultWidth);
//        AddProductionRateGroup(defaultWidth);
//    }

//    private void AddProductionRateGroup(float width) {
//        OptionPanelTool.AddGroup(Parent, width, Localize.ProductionRate);
//        OptionPanelTool.AddSliderGamma(CallBack(Config.Instance.ExtractingFacilityProductionRate, Localize.ExtractingFacilityMultiplier), null, 0.1f, 2, 0.1f, Config.Instance.ExtractingFacilityProductionRate, new Vector2(724, 20), (c, v) => {
//            Config.Instance.ExtractingFacilityProductionRate = v;
//            extractingFacilityLabel.text = CallBack(v, Localize.ExtractingFacilityMultiplier);
//        }, out extractingFacilityLabel, out UILabel _, out UISlider _);
//        OptionPanelTool.AddSliderGamma(CallBack(Config.Instance.ProcessingFacilityProductionRate, Localize.ProcessingFacilityMultiplier), null, 0.1f, 2, 0.1f, Config.Instance.ProcessingFacilityProductionRate, new Vector2(724, 20), (c, v) => {
//            Config.Instance.ProcessingFacilityProductionRate = v;
//            processingFacilityLabel.text = CallBack(v, Localize.ProcessingFacilityMultiplier);
//        }, out processingFacilityLabel, out UILabel _, out UISlider _);
//        OptionPanelTool.Reset();
//    }

//    private void AddTooltipBoxModeGroup(float width) {
//        OptionPanelTool.AddGroup(Parent, width, Localize.TooltipBoxMode);
//        OptionPanelTool.AddToggleButton(Config.Instance.OriginalValue, Localize.OriginalValue, null, _ => {
//            Config.Instance.OriginalValue = _;
//            BothValueToggleButton.IsChecked = !_;
//        }, out UILabel _, out UILabel _, out OriginalValueToggleButton);
//        OptionPanelTool.AddToggleButton(Config.Instance.BothValue, Localize.BothValue, null, _ => {
//            Config.Instance.BothValue = _;
//            OriginalValueToggleButton.IsChecked = !_;
//        }, out UILabel _, out UILabel _, out BothValueToggleButton);
//        OptionPanelTool.AddMinorLabel(Localize.ValueWarning);
//        OriginalValueToggleButton.isEnabled = BothValueToggleButton.isEnabled = !(Manager.PrefabFlag);
//        OptionPanelTool.Reset();
//    }



//    static string CallBack(float value, string type) {
//        if (Manager.PrefabFlag) {
//            Manager.RefreshOutputRate();
//        }
//        if (value == 1) {
//            return $"{type}: {ModLocalize.Vanilla}";
//        } else {
//            return $"{type}: {value}";
//        }
//    }
//}
