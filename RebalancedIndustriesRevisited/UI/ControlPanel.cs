namespace RebalancedIndustriesRevisited.UI;
using MbyronModsCommon;
using MbyronModsCommon.UI;
using UnityEngine;
using ModLocalize = Localize;

internal class ControlPanel : ControlPanelBase<Mod, ControlPanel> {
    private CustomUITabContainer tabContainer;

    private Vector2 ContainerSize => new(PorpertyPanelWidth, 514);
    private CustomUIScrollablePanel TruckCountContainer { get; set; }
    private CustomUIScrollablePanel GeneralContainer { get; set; }
    private Vector2 SliderSize { get; } = new(388, 16);

    protected override void InitComponents() {
        base.InitComponents();
        AddTabContainer();
        AddGeneralContainer();
        AddTruckCountContainer();
        ControlPanelManager<Mod, ControlPanel>.EventPanelClosing += (_) => {
            if (SingletonManager<ToolButtonManager>.Instance.InGameToolButton is not null) {
                SingletonManager<ToolButtonManager>.Instance.InGameToolButton.IsOn = false;
            }
        };
    }

    private void AddTruckCountContainer() {
        TruckCountContainer = AddTab(ModLocalize.Truck);
        ControlPanelHelper.AddGroup(TruckCountContainer, PorpertyPanelWidth, ModLocalize.EPMultiplierFactor);
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Crops, null, 80, Config.Instance.GrainFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.GrainFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.AnimalProducts, null, 80, Config.Instance.AnimalProductsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.AnimalProductsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Flour, null, 80, Config.Instance.FloursFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.FloursFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.RawForestProducts, null, 80, Config.Instance.LogsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.LogsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.PlanedTimber, null, 80, Config.Instance.PlanedTimberFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PlanedTimberFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Paper, null, 80, Config.Instance.PaperFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PaperFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Ore, null, 80, Config.Instance.OreFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.OreFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Metals, null, 80, Config.Instance.MetalsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.MetalsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Glass, null, 80, Config.Instance.GlassFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.GlassFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Oil, null, 80, Config.Instance.OilFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.OilFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Plastics, null, 80, Config.Instance.PlasticsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PlasticsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Petroleum, null, 80, Config.Instance.PetroleumFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PetroleumFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(TruckCountContainer, PorpertyPanelWidth, ModLocalize.IndustryWarehouseMultiplierFactor);
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.RawForestProducts, null, 80, Config.Instance.RawForestProductsWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.RawForestProductsWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Crops, null, 80, Config.Instance.CropsWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.CropsWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Ore, null, 80, Config.Instance.OreWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.OreWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.Ore, null, 80, Config.Instance.OilWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.OilWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(TruckCountContainer, PorpertyPanelWidth, null);
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.WarehouseMultiplierFactor, null, 80, Config.Instance.WarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.WarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(TruckCountContainer, PorpertyPanelWidth, null);
        ControlPanelHelper.AddField<UIFloatValueField, float>(ModLocalize.UniqueFactoryTruckMultiplierFactor, null, 80, Config.Instance.UniqueFactoryTruckMultiplierFactor, 0.1f, 0.2f, 4f, (_) => {
            Config.Instance.UniqueFactoryTruckMultiplierFactor = _;
            SingletonManager<Manager>.Instance.RefreshUniqueFactoryTruckCount();
        });
        ControlPanelHelper.Reset();

    }

    private CustomUILabel rawMaterialsLoadMultiplierFactorLabel;
    private CustomUILabel processingMaterialsLoadMultiplierFactorLabel;
    private CustomUILabel extractingFacilityProductionRateLabel;
    private CustomUILabel processingFacilityProductionRateLabel;

    private void AddGeneralContainer() {
        GeneralContainer = AddTab(CommonLocalize.OptionPanel_General);

        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, ModLocalize.LoadMultiplierFactor);
        var panel0 = ControlPanelHelper.AddSlider(RawMaterialsLoadMultiplierFactorString, null, 0.5f, 2f, 0.1f, Config.Instance.RawMaterialsLoadMultiplierFactor, SliderSize, (_) => {
            Config.Instance.RawMaterialsLoadMultiplierFactor = _;
            rawMaterialsLoadMultiplierFactorLabel.Text = RawMaterialsLoadMultiplierFactorString;
        }, gradientStyle: true);
        rawMaterialsLoadMultiplierFactorLabel = panel0.MajorLabel;
        var panel1 = ControlPanelHelper.AddSlider(ProcessingMaterialsLoadMultiplierFactorString, null, 0.5f, 2f, 0.1f, Config.Instance.ProcessingMaterialsLoadMultiplierFactor, SliderSize, (_) => {
            Config.Instance.ProcessingMaterialsLoadMultiplierFactor = _;
            processingMaterialsLoadMultiplierFactorLabel.Text = ProcessingMaterialsLoadMultiplierFactorString;
        }, gradientStyle: true);
        processingMaterialsLoadMultiplierFactorLabel = panel1.MajorLabel;
        ControlPanelHelper.AddMinorLabel(ModLocalize.LoadMultiplierFactorMinor);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, ModLocalize.ProductionRate);
        var panel2 = ControlPanelHelper.AddSlider(ExtractingFacilityProductionRate, null, 0.1f, 2f, 0.1f, Config.Instance.ExtractingFacilityProductionRate, SliderSize, (_) => {
            Config.Instance.ExtractingFacilityProductionRate = _;
            extractingFacilityProductionRateLabel.Text = ExtractingFacilityProductionRate;
            SingletonManager<Manager>.Instance.RefreshExtractingFacilityOutputRate();

        }, gradientStyle: true);
        extractingFacilityProductionRateLabel = panel2.MajorLabel;
        var panel3 = ControlPanelHelper.AddSlider(ProcessingFacilityProductionRate, null, 0.1f, 2f, 0.1f, Config.Instance.ProcessingFacilityProductionRate, SliderSize, (_) => {
            Config.Instance.ProcessingFacilityProductionRate = _;
            processingFacilityProductionRateLabel.Text = ProcessingFacilityProductionRate;
            SingletonManager<Manager>.Instance.RefreshProcessingFacilityOutputRate();
        }, gradientStyle: true);
        processingFacilityProductionRateLabel = panel3.MajorLabel;
        ControlPanelHelper.AddMinorLabel(ModLocalize.ProductionRateMinor);
        ControlPanelHelper.Reset();
    }

    private string ProcessingMaterialsLoadMultiplierFactorString => $"{ModLocalize.ProcessingMaterialsLoadMultiplierFactor}: {Config.Instance.ProcessingMaterialsLoadMultiplierFactor}";
    private string RawMaterialsLoadMultiplierFactorString => $"{ModLocalize.RawMaterialsLoadMultiplierFactor}: {Config.Instance.RawMaterialsLoadMultiplierFactor}";
    private string ExtractingFacilityProductionRate => $"{ModLocalize.ExtractingFacilityMultiplierFactor}: {Config.Instance.ExtractingFacilityProductionRate}";
    private string ProcessingFacilityProductionRate => $"{ModLocalize.ProcessingFacilityMultiplierFactor}: {Config.Instance.ProcessingFacilityProductionRate}";
    private void RefreshTruckCount() => SingletonManager<Manager>.Instance.RefreshTruckCount();

    private CustomUIScrollablePanel AddTab(string text) => tabContainer.AddContainer(text, this);

    private void AddTabContainer() {
        tabContainer = AddUIComponent<CustomUITabContainer>();
        tabContainer.size = new Vector2(PorpertyPanelWidth, 24);
        tabContainer.Gap = 2;
        tabContainer.Atlas = CustomUIAtlas.MbyronModsAtlas;
        tabContainer.BgSprite = CustomUIAtlas.RoundedRectangle2;
        tabContainer.BgNormalColor = CustomUIColor.CPPrimaryBg;
        tabContainer.relativePosition = new Vector2(16, CaptionHeight);
        tabContainer.EventTabAdded += (_) => {
            _.TextScale = 0.9f;
            _.SetDefaultControlPanelStyle();
            _.TextPadding = new RectOffset(0, 0, 2, 0);
        };
        tabContainer.EventContainerAdded += (_) => {
            _.size = ContainerSize;
            _.autoLayoutPadding = new RectOffset(0, 0, 5, 10);
            var scrollbar0 = UIScrollbarHelper.AddScrollbar(this, _, new Vector2(8, 514));
            scrollbar0.thumbObject.color = CustomUIColor.CPPrimaryBg;
            scrollbar0.relativePosition = new Vector2(width - 8, CaptionHeight + 30);
            _.relativePosition = new Vector2(16, CaptionHeight + 30);
        };
    }
}
