using ColossalFramework;
using ColossalFramework.UI;
using CSLModsCommon;
using CSLModsCommon.Manager;
using CSLModsCommon.UI.Atlas;
using RebalancedIndustriesRevisited.UI;
using UnityEngine;

namespace RebalancedIndustriesRevisited.Managers;

public class GameInfoPanelManager : ManagerBase {
    private ModManager _modManager;
    private UIButton _cityServiceWorldInfoPanelButton;
    private InGameToolButtonManager _inGameToolButtonManager;

    public bool CityServiceWorldInfoPanelButtonVisible {
        get => _cityServiceWorldInfoPanelButton.isVisible;
        set {
            if (_cityServiceWorldInfoPanelButton is not null) {
                _cityServiceWorldInfoPanelButton.isVisible = value;
            }
        }
    }

    protected override void OnCreate() {
        base.OnCreate();
        _modManager = Domain.GetManager<ModManager>();
        _inGameToolButtonManager = Domain.GetOrCreateManager<InGameToolButtonManager>();
    }

    protected override void OnGameLoaded(LoadContext context) {
        base.OnGameLoaded(context);
        var cityServiceWorldInfoPanel = UIView.library.Get<CityServiceWorldInfoPanel>(nameof(CityServiceWorldInfoPanel));
        _cityServiceWorldInfoPanelButton = AddEntryButtonToInfoPanel(cityServiceWorldInfoPanel);
        if (_cityServiceWorldInfoPanelButton is not null) {
            _cityServiceWorldInfoPanelButton.isVisible = false;
        }

        AddEntryButtonToInfoPanel(UIView.library.Get<WarehouseWorldInfoPanel>(nameof(WarehouseWorldInfoPanel)));
        AddEntryButtonToInfoPanel(UIView.library.Get<UniqueFactoryWorldInfoPanel>(nameof(UniqueFactoryWorldInfoPanel)));
    }

    public void OnSetWorldInfoPanelButton() {
        var currentInstanceId = WorldInfoPanel.GetCurrentInstanceID();
        if (currentInstanceId == InstanceID.Empty) return;
        if (!Singleton<BuildingManager>.exists) {
            Logger.Verbose("BuildingManager isn't initialized, return");
            return;
        }

        var building = Singleton<BuildingManager>.instance.m_buildings.m_buffer[currentInstanceId.Building];
        var buildingInfo = building.Info;
        var buildingAIType = buildingInfo.m_buildingAI.GetType();
        if (buildingInfo.m_class.m_subService is ItemClass.SubService.PlayerIndustryFarming or ItemClass.SubService.PlayerIndustryForestry or ItemClass.SubService.PlayerIndustryOil or ItemClass.SubService.PlayerIndustryOre || buildingInfo.m_buildingAI is not null && buildingAIType == typeof(FishingHarborAI)) {
            // Campus Industries Housing Mod Compatibility
            var aiName = buildingAIType.Name;
            if (buildingAIType == typeof(AuxiliaryBuildingAI) || aiName == "DormsAI" || aiName == "BarracksAI") {
                CityServiceWorldInfoPanelButtonVisible = false;
            }
            else {
                CityServiceWorldInfoPanelButtonVisible = true;
            }
        }
        else {
            CityServiceWorldInfoPanelButtonVisible = false;
        }

        Logger.Verbose($"Set CityServiceWorldInfoPanel visible: {CityServiceWorldInfoPanelButtonVisible}, m_subService:{buildingInfo.m_class.m_subService.ToString()}");
    }

    private UIButton AddEntryButtonToInfoPanel(BuildingWorldInfoPanel buildingWorldInfoPanel) {
        var wrapper = buildingWorldInfoPanel.Find("Wrapper");
        UIComponent buttonPanel = null;
        if (wrapper is not null) {
            buttonPanel = wrapper.Find("MainSectionPanel")?.Find("MainBottom")?.Find("ButtonPanels")?.Find("ActionButtons")?.Find("ActionPanelPanel")?.Find("ActionPanel");
            if (buttonPanel != null) {
                Logger.Info("Added entry button to CityServiceWorldInfoPanel");
            }
        }
        else if (buildingWorldInfoPanel.Find("ActionPanel") is UIPanel actionPanel) {
            buttonPanel = actionPanel;
            Logger.Info("Added entry button to WarehouseWorldInfoPanel");
        }
        else if (buildingWorldInfoPanel.Find("Misc") is UIPanel misc) {
            buttonPanel = misc;
            Logger.Info("Added entry button to UniqueFactoryWorldInfoPanel");
        }

        var targetButton = AddButton(buttonPanel, true);
        return targetButton;
    }

    private UIButton AddButton(UIComponent parent, bool isUniqueFactoryWorldInfoPanel = false) {
        if (parent is null) return null;
        var button = parent.AddUIComponent<UIButton>();
        button.name = "RebalancedIndustriesRevisitedButton";
        button.size = new Vector2(42, 42);
        button.atlas = Atlases.InGame;
        button.normalBgSprite = "GenericPanelLight";
        button.color = new Color32(219, 219, 219, 255);
        button.pressedColor = button.color;
        button.focusedColor = Color.white;
        button.tooltip = _modManager.ModName;
        var buttonSprite = button.AddUIComponent<UISprite>();
        buttonSprite.size = button.size;
        buttonSprite.atlas = ModAtlasLoader.ModAtlas;
        buttonSprite.spriteName = ModAtlasLoader.InGameButton;
        buttonSprite.CenterToParent();
        if (isUniqueFactoryWorldInfoPanel) {
            button.relativePosition = new Vector3(18, button.relativePosition.y);
        }

        button.eventClicked += (component, _) => {
            _inGameToolButtonManager.OnForcePanelOpen();
            component.Unfocus();
        };
        return button;
    }
}