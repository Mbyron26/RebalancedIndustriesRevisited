using CSShared.Manager;
using CSShared.UI;
using CSShared.UI.ControlPanel;
using UnityEngine;

namespace RebalancedIndustriesRevisited.UI;

internal class ControlPanel : ControlPanelBase<Mod, ControlPanel> {
    private CustomUITabContainer tabContainer;

    private Vector2 ContainerSize => new(PropertyPanelWidth, 514);
    private CustomUIScrollablePanel TruckCountContainer { get; set; }
    private CustomUIScrollablePanel GeneralContainer { get; set; }
    private Vector2 SliderSize { get; } = new(388, 16);

    protected override void InitComponents() {
        base.InitComponents();
        AddTabContainer();
        AddGeneralContainer();
        AddTruckCountContainer();
        ControlPanelManager<Mod, ControlPanel>.EventPanelClosing += (_) => {
            if (ManagerPool.GetOrCreateManager<ToolButtonManager>().InGameToolButton is not null) {
                ManagerPool.GetOrCreateManager<ToolButtonManager>().InGameToolButton.IsOn = false;
            }
        };
    }

    private void AddTruckCountContainer() {
        TruckCountContainer = AddTab(Localize("Truck"));
        ControlPanelHelper.AddGroup(TruckCountContainer, PropertyPanelWidth, Localize("EPMultiplierFactor"));
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Crops"), null, 80, Config.Instance.GrainFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.GrainFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("AnimalProducts"), null, 80, Config.Instance.AnimalProductsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.AnimalProductsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Flour"), null, 80, Config.Instance.FloursFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.FloursFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("RawForestProducts"), null, 80, Config.Instance.LogsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.LogsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("PlanedTimber"), null, 80, Config.Instance.PlanedTimberFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PlanedTimberFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Paper"), null, 80, Config.Instance.PaperFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PaperFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Ore"), null, 80, Config.Instance.OreFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.OreFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Metals"), null, 80, Config.Instance.MetalsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.MetalsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Glass"), null, 80, Config.Instance.GlassFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.GlassFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Oil"), null, 80, Config.Instance.OilFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.OilFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Plastics"), null, 80, Config.Instance.PlasticsFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PlasticsFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Petroleum"), null, 80, Config.Instance.PetroleumFactor, 0.1f, 0.2f, 4f, (_) => { Config.Instance.PetroleumFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(TruckCountContainer, PropertyPanelWidth, Localize("IndustryWarehouseMultiplierFactor"));
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("RawForestProducts"), null, 80, Config.Instance.RawForestProductsWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.RawForestProductsWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Crops"), null, 80, Config.Instance.CropsWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.CropsWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Ore"), null, 80, Config.Instance.OreWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.OreWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("Oil"), null, 80, Config.Instance.OilWarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.OilWarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(TruckCountContainer, PropertyPanelWidth, null);
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("WarehouseMultiplierFactor"), null, 80, Config.Instance.WarehouseTruckMultiplierFactor, 0.1f, 0.2f, 2f, (_) => { Config.Instance.WarehouseTruckMultiplierFactor = _; RefreshTruckCount(); });
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(TruckCountContainer, PropertyPanelWidth, null);
        ControlPanelHelper.AddField<UIFloatValueField, float>(Localize("UniqueFactoryTruckMultiplierFactor"), null, 80, Config.Instance.UniqueFactoryTruckMultiplierFactor, 0.1f, 0.2f, 4f, (_) => {
            Config.Instance.UniqueFactoryTruckMultiplierFactor = _;
            ManagerPool.GetOrCreateManager<Manager>().RefreshUniqueFactoryTruckCount();
        });
        ControlPanelHelper.Reset();

    }

    private CustomUILabel rawMaterialsLoadMultiplierFactorLabel;
    private CustomUILabel processingMaterialsLoadMultiplierFactorLabel;
    private CustomUILabel extractingFacilityProductionRateLabel;
    private CustomUILabel processingFacilityProductionRateLabel;

    private void AddGeneralContainer() {
        GeneralContainer = AddTab(Localize("OptionPanel_General"));
        ControlPanelHelper.AddGroup(GeneralContainer, PropertyPanelWidth, Localize("LoadMultiplierFactor"));
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
        ControlPanelHelper.AddMinorLabel(Localize("LoadMultiplierFactorMinor"));
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(GeneralContainer, PropertyPanelWidth, Localize("ProductionRate"));
        var panel2 = ControlPanelHelper.AddSlider(ExtractingFacilityProductionRate, null, 0.1f, 2f, 0.1f, Config.Instance.ExtractingFacilityProductionRate, SliderSize, (_) => {
            Config.Instance.ExtractingFacilityProductionRate = _;
            extractingFacilityProductionRateLabel.Text = ExtractingFacilityProductionRate;
            ManagerPool.GetOrCreateManager<Manager>().RefreshExtractingFacilityOutputRate();

        }, gradientStyle: true);
        extractingFacilityProductionRateLabel = panel2.MajorLabel;
        var panel3 = ControlPanelHelper.AddSlider(ProcessingFacilityProductionRate, null, 0.1f, 2f, 0.1f, Config.Instance.ProcessingFacilityProductionRate, SliderSize, (_) => {
            Config.Instance.ProcessingFacilityProductionRate = _;
            processingFacilityProductionRateLabel.Text = ProcessingFacilityProductionRate;
            ManagerPool.GetOrCreateManager<Manager>().RefreshProcessingFacilityOutputRate();
        }, gradientStyle: true);
        processingFacilityProductionRateLabel = panel3.MajorLabel;
        ControlPanelHelper.AddMinorLabel(Localize("ProductionRateMinor"));
        ControlPanelHelper.Reset();
    }

    private string ProcessingMaterialsLoadMultiplierFactorString => $"{Localize("ProcessingFacility")}: {Config.Instance.ProcessingMaterialsLoadMultiplierFactor}";
    private string RawMaterialsLoadMultiplierFactorString => $"{Localize("ExtractingFacility")}: {Config.Instance.RawMaterialsLoadMultiplierFactor}";
    private string ExtractingFacilityProductionRate => $"{Localize("ExtractingFacilityMultiplierFactor")}: {Config.Instance.ExtractingFacilityProductionRate}";
    private string ProcessingFacilityProductionRate => $"{Localize("ProcessingFacilityMultiplierFactor")}: {Config.Instance.ProcessingFacilityProductionRate}";
    private void RefreshTruckCount() => ManagerPool.GetOrCreateManager<Manager>().RefreshTruckCount();

    private CustomUIScrollablePanel AddTab(string text) => tabContainer.AddContainer(text, this);

    private void AddTabContainer() {
        tabContainer = AddUIComponent<CustomUITabContainer>();
        tabContainer.size = new Vector2(PropertyPanelWidth, 24);
        tabContainer.Gap = 2;
        tabContainer.Atlas = CustomUIAtlas.CSSharedAtlas;
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
            _.autoLayoutPadding = new RectOffset(0, 0, 5, 16);
            var scrollbar0 = UIScrollbarHelper.AddScrollbar(this, _, new Vector2(8, 514));
            scrollbar0.thumbObject.color = CustomUIColor.CPPrimaryBg;
            scrollbar0.relativePosition = new Vector2(width - 8, CaptionHeight + 30);
            _.relativePosition = new Vector2(16, CaptionHeight + 30);
        };
    }
}
