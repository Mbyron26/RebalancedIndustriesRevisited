using ColossalFramework.UI;
using CSLModsCommon.Localization;
using CSLModsCommon.Manager;
using CSLModsCommon.UI;
using CSLModsCommon.UI.Atlas;
using CSLModsCommon.UI.Buttons.RadioButtons;
using CSLModsCommon.UI.Containers;
using CSLModsCommon.UI.ControlPanel;
using CSLModsCommon.UI.Labels;
using RebalancedIndustriesRevisited.Data;
using RebalancedIndustriesRevisited.Localization;
using RebalancedIndustriesRevisited.Managers;
using RebalancedIndustriesRevisited.ModSettings;
using UnityEngine;

namespace RebalancedIndustriesRevisited.UI;

internal class ControlPanel : ControlPanelBase {
    private const string General = nameof(General);
    private const string Config = nameof(Config);

    private ModSetting _modSetting;
    private FacilityManager _facilityManager;
    private InGameToolButtonManager _inGameToolButtonManager;
    private ScrollContainer _generalPage;
    private ScrollContainer _configPage;
    private IProfile _profile;
    private IntValueField _constructionCostField;
    private IntValueField _maintenanceCostField;
    private IntValueField _outputRateField;
    private IntValueField _truckCountField;
    private IntValueField _customizedStorageCapacityField;
    private IntValueField _uneducatedWorkersField;
    private IntValueField _educatedWorkersField;
    private IntValueField _wellEducatedWorkersField;
    private IntValueField _highlyEducatedWorkersField;
    private Label _rawMaterialsLoadMultiplierFactorHeaderElement;
    private Label _processingMaterialsLoadMultiplierFactorHeaderElement;
    private SettingsSection _facilityPropertiesSection;
    private IntValueField _customizedBoatCountField;
    private Label _extractingFacilityProductionRateHeaderElement;
    private Label _processingFacilityProductionRateHeaderElement;
    private Label _uniqueFacilityProductionRateHeaderElement;
    private Label _outputRateDescriptionElement;

    private Vector2 SliderSize { get; } = new(388, 16);

    protected override void CacheManagers() {
        base.CacheManagers();
        _modSetting = _domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
        _facilityManager = _domain.GetOrCreateManager<FacilityManager>();
        _domain.GetOrCreateManager<ControlPanelManager>();
        _inGameToolButtonManager = _domain.GetOrCreateManager<InGameToolButtonManager>();
    }

    protected override void OnAwake() {
        base.OnAwake();

        FillCustomizationPage();
        FillGeneralPage();
    }

    protected override void SetSelectedTab() {
        base.SetSelectedTab();
        _tabGroupLogic.SelectPage(0);
    }

    protected override void OnCloseButtonClicked(UIComponent component, UIMouseEventParameter eventParam) {
        _inGameToolButtonManager.OnPanelClosed();
    }

    private void FillCustomizationPage() {
        _profile = _facilityManager.GetProfile();
        if (_profile is null) return;

        _configPage = AddPage(Config, Translations.Config);

        #region FacilityType

        var facilityTypeSection = AddSection(_configPage);

        if (_profile.IndustrialCategory != IndustrialCategory.Unknown) {
            facilityTypeSection.AddButton($"{Translations.FacilityType}: {GetBuildingType()}", null, string.Empty, 30, 30, null, v => {
                v.Control.BgAtlas = Atlases.InGame;
                v.Control.BgSprites.SetValues(_profile.IndustrialCategory switch {
                    IndustrialCategory.Oil => "SubBarIndustryOil",
                    IndustrialCategory.Ore => "SubBarIndustryOre",
                    IndustrialCategory.Forestry => "SubBarIndustryForestry",
                    IndustrialCategory.Farming => "SubBarIndustryFarming",
                    IndustrialCategory.Fishing => "SubBarIndustryFishing",
                    _ => string.Empty
                });
                v.Control.BgColors.SetValues(UIColors.White);
            });
        }
        else {
            facilityTypeSection.AddLabel(null, $"{Translations.FacilityType}: {GetBuildingType()}", beforeLayoutAction: card => card.Direction = FlexDirection.Column);
        }

        facilityTypeSection.AddLabel(null, $"{Translations.FacilityName}: {_facilityManager.GetCurrentBuildingName()}", beforeLayoutAction: card => card.Direction = FlexDirection.Column);

        facilityTypeSection.AddRadioGroup(Translations.ConfigurationType, null, unlockRadioGroup => {
            RadioGroupLogic<ProfileFlag>.Create()
                .AddRange(
                    new RadioButtonItem<ProfileFlag>(ProfileFlag.GameDefault, unlockRadioGroup.AddOption(Translations.GameDefault)),
                    new RadioButtonItem<ProfileFlag>(ProfileFlag.ModDefault, unlockRadioGroup.AddOption(Translations.ModDefault)),
                    new RadioButtonItem<ProfileFlag>(ProfileFlag.Customized, unlockRadioGroup.AddOption(Translations.Custom))
                )
                .SetDefault(v => v.Value == _profile.ProfileTypeSet)
                .SelectionChanged += OnProfileSelectionChanged;
        });

        #endregion

        #region FacilityProperties

        _facilityPropertiesSection = AddSection(_configPage);

        _constructionCostField = _facilityPropertiesSection.AddIntField(Translations.ConstructionCost, GetMinorText(_profile.ConstructionCost, _profile.ModDefaultConstructionCost), _profile.CustomizedConstructionCost, 0, 100000, 100, v => _profile.CustomizedConstructionCost = v).Control;
        _maintenanceCostField = _facilityPropertiesSection.AddIntField(Translations.MaintenanceCost, GetMinorText(_profile.MaintenanceCost, _profile.ModDefaultMaintenanceCost), _profile.CustomizedMaintenanceCost, 0, 100000, 100, v => _profile.CustomizedMaintenanceCost = v).Control;
        if (_profile.BuildingType != FacilityType.MainIndustryBuilding) {
            _truckCountField = _facilityPropertiesSection.AddIntField(Translations.OutputTruckCount, GetMinorText(_profile.TruckCount, _profile.ModDefaultTruckCount), _profile.CustomizedTruckCount, 0, 100, 1, v => _profile.CustomizedTruckCount = v).Control;
        }

        if (_profile.BuildingType != FacilityType.WarehouseFacility && _profile.BuildingType != FacilityType.MainIndustryBuilding) {
            if (_profile.BuildingType != FacilityType.FishingHarbor)
                _outputRateField = _facilityPropertiesSection.AddIntField(Translations.OutputRate, GetMinorText(_profile.OutputRate, _profile.ModDefaultOutputRate), _profile.CustomizedOutputRate, 0, 50000, 100, v => _profile.CustomizedOutputRate = v, beforeLayoutAction: v => _outputRateDescriptionElement = v.DescriptionElement).Control;
        }


        if (_profile.BuildingType == FacilityType.WarehouseFacility) {
            _customizedStorageCapacityField = _facilityPropertiesSection.AddIntField(Translations.StorageCapacity, GetMinorText(_profile.StorageCapacity / 1000, _profile.ModDefaultStorageCapacity / 1000), _profile.CustomizedStorageCapacity / 1000, 0, 6000, 10, v => _profile.CustomizedStorageCapacity = v * 1000).Control;
        }

        if (_profile.BuildingType == FacilityType.FishingHarbor) {
            _customizedBoatCountField = _facilityPropertiesSection.AddIntField(Translations.BoatCount, GetMinorText(_profile.BoatCount, _profile.ModDefaultBoatCount), _profile.CustomizedBoatCount, 0, 100, 1, v => _profile.CustomizedBoatCount = v).Control;
        }

        _uneducatedWorkersField = _facilityPropertiesSection.AddIntField(Translations.UneducatedWorkers, GetMinorText(_profile.WorkPlace.UneducatedWorkers, _profile.ModDefaultWorkPlace.UneducatedWorkers), _profile.CustomizedWorkPlace.UneducatedWorkers, 0, 100, 1, v => _profile.CustomizedWorkPlace = new WorkPlace(v, _profile.CustomizedWorkPlace.EducatedWorkers, _profile.CustomizedWorkPlace.WellEducatedWorkers, _profile.CustomizedWorkPlace.HighlyEducatedWorkers)).Control;
        _educatedWorkersField = _facilityPropertiesSection.AddIntField(Translations.EducatedWorkers, GetMinorText(_profile.WorkPlace.EducatedWorkers, _profile.ModDefaultWorkPlace.EducatedWorkers), _profile.CustomizedWorkPlace.EducatedWorkers, 0, 100, 1, v => _profile.CustomizedWorkPlace = new WorkPlace(_profile.CustomizedWorkPlace.UneducatedWorkers, v, _profile.CustomizedWorkPlace.WellEducatedWorkers, _profile.CustomizedWorkPlace.HighlyEducatedWorkers)).Control;
        _wellEducatedWorkersField = _facilityPropertiesSection.AddIntField(Translations.WellEducatedWorkers, GetMinorText(_profile.WorkPlace.WellEducatedWorkers, _profile.ModDefaultWorkPlace.WellEducatedWorkers), _profile.CustomizedWorkPlace.WellEducatedWorkers, 0, 100, 1, v => _profile.CustomizedWorkPlace = new WorkPlace(_profile.CustomizedWorkPlace.UneducatedWorkers, _profile.CustomizedWorkPlace.EducatedWorkers, v, _profile.CustomizedWorkPlace.HighlyEducatedWorkers)).Control;
        _highlyEducatedWorkersField = _facilityPropertiesSection.AddIntField(Translations.HighlyEducatedWorkers, GetMinorText(_profile.WorkPlace.HighlyEducatedWorkers, _profile.ModDefaultWorkPlace.HighlyEducatedWorkers), _profile.CustomizedWorkPlace.HighlyEducatedWorkers, 0, 100, 1, v => _profile.CustomizedWorkPlace = new WorkPlace(_profile.CustomizedWorkPlace.UneducatedWorkers, _profile.CustomizedWorkPlace.EducatedWorkers, _profile.CustomizedWorkPlace.WellEducatedWorkers, v)).Control;

        _facilityPropertiesSection.isEnabled = _profile.ProfileTypeSet == ProfileFlag.Customized;

        #endregion
    }

    private void OnProfileSelectionChanged(RadioButtonItem<ProfileFlag> obj) {
        _facilityPropertiesSection.isEnabled = false;
        var value = obj.Value;
        if (value == ProfileFlag.GameDefault) {
            _profile.SetGameDefaults();
        }
        else if (value == ProfileFlag.ModDefault) {
            _profile.SetModDefaults();
        }
        else {
            _profile.SetModCustomized();
            _facilityPropertiesSection.isEnabled = true;
        }

        RefreshProfile();
    }

    private string GetBuildingType() => _profile.BuildingType switch {
        FacilityType.MainIndustryBuilding => Translations.IndustrialMainBuilding,
        FacilityType.ExtractingFacility => Translations.ExtractingFacility,
        FacilityType.ProcessingFacility => Translations.ProcessingFacility,
        FacilityType.UniqueFacility => Translations.UniqueFacility,
        FacilityType.FishingHarbor => Translations.FishingHarbor,
        _ => Translations.WarehouseFacility,
    };

    private void RefreshProfile() {
        _constructionCostField.Value = _profile.CustomizedConstructionCost;
        _maintenanceCostField.Value = _profile.CustomizedMaintenanceCost;
        if (_outputRateField is not null) {
            _outputRateField.Value = _profile.CustomizedOutputRate;
        }

        if (_truckCountField is not null)
            _truckCountField.Value = _profile.CustomizedTruckCount;
        if (_customizedStorageCapacityField is not null) {
            _customizedStorageCapacityField.Value = _profile.CustomizedStorageCapacity / 1000;
        }

        if (_customizedBoatCountField is not null) {
            _customizedBoatCountField.Value = _profile.CustomizedBoatCount;
        }

        _uneducatedWorkersField.Value = _profile.WorkPlace.UneducatedWorkers;
        _educatedWorkersField.Value = _profile.WorkPlace.EducatedWorkers;
        _wellEducatedWorkersField.Value = _profile.WorkPlace.WellEducatedWorkers;
        _highlyEducatedWorkersField.Value = _profile.WorkPlace.HighlyEducatedWorkers;
    }

    private void RefreshOutputRate() {
        if (_profile is null || _outputRateField is null) return;
        _outputRateField.Value = _profile.CustomizedOutputRate;
        if (_outputRateDescriptionElement is not null) {
            _outputRateDescriptionElement.Text = GetMinorText(_profile.OutputRate, _profile.ModDefaultOutputRate);
        }
    }

    private string GetMinorText(int gameValue, int modValue) => $"{Translations.GameDefault}: {gameValue.ToString()}, {Translations.ModDefault}: {modValue.ToString()}";

    private void FillGeneralPage() {
        _generalPage = AddPage(General, SharedTranslations.General);

        var loadMultiplierFactorSection = AddSection(_generalPage, Translations.LoadMultiplierFactor, Translations.LoadMultiplierFactorMinor);

        loadMultiplierFactorSection.AddSlider(RawMaterialsLoadMultiplierString, null, 0.5f, 2f, 0.1f, _modSetting.RawMaterialsLoadMultiplierFactor, SliderSize, f => {
            _modSetting.RawMaterialsLoadMultiplierFactor = f;
            _rawMaterialsLoadMultiplierFactorHeaderElement.Text = RawMaterialsLoadMultiplierString;
        }, gradientStyle: true, beforeLayoutAction: c => { _rawMaterialsLoadMultiplierFactorHeaderElement = c.HeaderElement; });

        loadMultiplierFactorSection.AddSlider(ProcessingMaterialsLoadMultiplierString, null, 0.5f, 2f, 0.1f, _modSetting.ProcessingMaterialsLoadMultiplierFactor, SliderSize, f => {
            _modSetting.ProcessingMaterialsLoadMultiplierFactor = f;
            _processingMaterialsLoadMultiplierFactorHeaderElement.Text = ProcessingMaterialsLoadMultiplierString;
        }, gradientStyle: true, card => { _processingMaterialsLoadMultiplierFactorHeaderElement = card.HeaderElement; });

        var productionRateMultiplierFactorSection = AddSection(_generalPage, Translations.ProductionRateMultiplier, Translations.ProductionRateMultiplierDescription);
        productionRateMultiplierFactorSection.AddSlider(ExtractingFacilityProductionRateMultiplierString, null, 0.2f, 2f, 0.1f, _modSetting.ExtractingFacilityProductionRateMultiplier, SliderSize, f => {
            _modSetting.ExtractingFacilityProductionRateMultiplier = f;
            _extractingFacilityProductionRateHeaderElement.Text = ExtractingFacilityProductionRateMultiplierString;
            _facilityManager.ModifyExtractingFacilityProductionRate();
            RefreshOutputRate();
        }, gradientStyle: true, beforeLayoutAction: c => { _extractingFacilityProductionRateHeaderElement = c.HeaderElement; });
        productionRateMultiplierFactorSection.AddSlider(ProcessingFacilityProductionRateMultiplierString, null, 0.2f, 2f, 0.1f, _modSetting.ProcessingFacilityProductionRateMultiplier, SliderSize, f => {
            _modSetting.ProcessingFacilityProductionRateMultiplier = f;
            _processingFacilityProductionRateHeaderElement.Text = ProcessingFacilityProductionRateMultiplierString;
            _facilityManager.ModifyProcessingFacilityProductionRate();
            RefreshOutputRate();
        }, gradientStyle: true, beforeLayoutAction: c => { _processingFacilityProductionRateHeaderElement = c.HeaderElement; });
        productionRateMultiplierFactorSection.AddSlider(UniqueFacilityProductionRateMultiplierString, null, 0.2f, 2f, 0.1f, _modSetting.UniqueFacilityProductionRateMultiplier, SliderSize, f => {
            _modSetting.UniqueFacilityProductionRateMultiplier = f;
            _uniqueFacilityProductionRateHeaderElement.Text = UniqueFacilityProductionRateMultiplierString;
            _facilityManager.ModifyUniqueFactoryProductionRate();
            RefreshOutputRate();
        }, gradientStyle: true, beforeLayoutAction: c => _uniqueFacilityProductionRateHeaderElement = c.HeaderElement);
    }

    private string UniqueFacilityProductionRateMultiplierString => $"{Translations.UniqueFacility}: {_modSetting.UniqueFacilityProductionRateMultiplier}";
    private string ProcessingFacilityProductionRateMultiplierString => $"{Translations.ProcessingFacility}: {_modSetting.ProcessingFacilityProductionRateMultiplier}";
    private string ExtractingFacilityProductionRateMultiplierString => $"{Translations.ExtractingFacility}: {_modSetting.ExtractingFacilityProductionRateMultiplier}";
    private string ProcessingMaterialsLoadMultiplierString => $"{Translations.ProcessingFacility}: {_modSetting.ProcessingMaterialsLoadMultiplierFactor}";
    private string RawMaterialsLoadMultiplierString => $"{Translations.ExtractingFacility}: {_modSetting.RawMaterialsLoadMultiplierFactor}";
}