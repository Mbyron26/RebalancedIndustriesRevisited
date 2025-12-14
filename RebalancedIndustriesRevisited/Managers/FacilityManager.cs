using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using CSLModsCommon;
using CSLModsCommon.KeyBindings;
using CSLModsCommon.Manager;
using CSLModsCommon.Serialization;
using CSLModsCommon.Utilities;
using RebalancedIndustriesRevisited.Data;
using RebalancedIndustriesRevisited.ModSettings;
using NaturalResource = RebalancedIndustriesRevisited.Data.NaturalResource;

namespace RebalancedIndustriesRevisited.Managers;

public class FacilityManager : ManagerBase, ISerializable {
    private SettingManager _settingManager;

    private ModSetting _modSetting;
    private InGameToolButtonManager _inGameToolButtonManager;
    private KeyBindingManager _keyBindingManager;

    private Dictionary<string, ExtractingFacilityProfile> _extractingFacilityModifies = new();
    private Dictionary<string, ProcessingFacilityProfile> _processingFacilityModifies = new();
    private Dictionary<string, UniqueFactoryProfile> _uniqueFacilityModifies = new();
    private Dictionary<string, WarehouseProfile> _warehouseModifies = new();
    private Dictionary<string, MainIndustryBuildingProfile> _mainIndustryBuildingModifies = new();
    private Dictionary<string, FishingHarborProfile> _fishingHarborModifies = new();

    private List<ExtractingFacilityProfile> ExtractingFacilityPrefabBuffer { get; set; }
    private List<ProcessingFacilityProfile> ProcessingFacilityPrefabBuffer { get; set; }
    private List<UniqueFactoryProfile> UniqueFacilityPrefabBuffer { get; set; }
    private List<WarehouseProfile> WarehousePrefabBuffer { get; set; }
    private List<MainIndustryBuildingProfile> MainIndustryPrefabBuffer { get; set; }
    private List<FishingHarborProfile> FishingHarborProfileBuffer { get; set; }

    protected override void OnCreate() {
        base.OnCreate();
        _settingManager = Domain.GetOrCreateManager<SettingManager>();
        _modSetting = _settingManager.GetSetting<ModSetting>();
        _inGameToolButtonManager = Domain.GetOrCreateManager<InGameToolButtonManager>();
        ExtractingFacilityPrefabBuffer = [];
        ProcessingFacilityPrefabBuffer = [];
        UniqueFacilityPrefabBuffer = [];
        WarehousePrefabBuffer = [];
        MainIndustryPrefabBuffer = [];
        FishingHarborProfileBuffer = [];
        _keyBindingManager = Domain.GetOrCreateManager<KeyBindingManager>();
    }

    protected override void OnGameLoaded(LoadContext context) {
        base.OnGameLoaded(context);
        _keyBindingManager.Register("ControlPanelToggle", _modSetting.ControlPanelToggle, _inGameToolButtonManager.OnKeyBindingToggle, KeyBindingContext.InGame);
    }

    protected override void OnGameUnloaded() {
        base.OnGameUnloaded();
        _keyBindingManager.Unregister("ControlPanelToggle");
    }

    public bool HasCachePrefab() => GetProfile() is not null;

    public string GetCurrentBuildingName() {
        if (CurrentMode == GameMode.MainMenu) return string.Empty;
        var buildingManager = Singleton<BuildingManager>.instance;
        var building = WorldInfoPanel.GetCurrentInstanceID().Building;
        return buildingManager.GetBuildingName(building, InstanceID.Empty);
    }

    public IProfile GetProfile() {
        if (CurrentMode == GameMode.MainMenu) return null;
        var buildingManager = Singleton<BuildingManager>.instance;
        var building = WorldInfoPanel.GetCurrentInstanceID().Building;
        var buildingAI = buildingManager.m_buildings.m_buffer[building].Info.m_buildingAI;
        if (buildingAI is null)
            return null;
        var type = buildingAI.GetType();
        if (type == typeof(ExtractingFacilityAI)) {
            return ExtractingFacilityPrefabBuffer.FirstOrDefault(v => v.Name == buildingAI.name);
        }

        if (type == typeof(UniqueFactoryAI)) {
            return UniqueFacilityPrefabBuffer.FirstOrDefault(v => v.Name == buildingAI.name);
        }

        if (type == typeof(ProcessingFacilityAI)) {
            return ProcessingFacilityPrefabBuffer.FirstOrDefault(v => v.Name == buildingAI.name);
        }

        if (type == typeof(WarehouseAI)) {
            return WarehousePrefabBuffer.FirstOrDefault(v => v.Name == buildingAI.name);
        }

        if (type == typeof(MainIndustryBuildingAI)) {
            return MainIndustryPrefabBuffer.FirstOrDefault(v => v.Name == buildingAI.name);
        }

        if (type == typeof(FishingHarborAI)) {
            return FishingHarborProfileBuffer.FirstOrDefault(v => v.Name == buildingAI.name);
        }

        return null;
    }

    public void SetCargoCapacity(Vehicle data, ref CargoTruckAI cargoTruckAI) {
        var type = (TransferManager.TransferReason)data.m_transferType;
        if (IsRawMaterial(type)) {
            cargoTruckAI.m_cargoCapacity = (int)(8000 * _modSetting.RawMaterialsLoadMultiplierFactor);
        }
        else if (IsProcessedProduct(type)) {
            cargoTruckAI.m_cargoCapacity = (int)(8000 * _modSetting.ProcessingMaterialsLoadMultiplierFactor);
        }
    }

    public bool IsRawMaterial(TransferManager.TransferReason transferReason) => transferReason == TransferManager.TransferReason.Logs || transferReason == TransferManager.TransferReason.Grain || transferReason == TransferManager.TransferReason.Ore || transferReason == TransferManager.TransferReason.Oil;

    public bool IsProcessedProduct(TransferManager.TransferReason transferReason) => transferReason == TransferManager.TransferReason.Paper || transferReason == TransferManager.TransferReason.PlanedTimber || transferReason == TransferManager.TransferReason.Flours || transferReason == TransferManager.TransferReason.AnimalProducts || transferReason == TransferManager.TransferReason.Metals || transferReason == TransferManager.TransferReason.Glass || transferReason == TransferManager.TransferReason.Plastics || transferReason == TransferManager.TransferReason.Petroleum || transferReason == TransferManager.TransferReason.LuxuryProducts;

    public NaturalResource GetIndustriesType(TransferManager.TransferReason transferReason) => transferReason switch {
        TransferManager.TransferReason.Logs or TransferManager.TransferReason.Paper or TransferManager.TransferReason.PlanedTimber => NaturalResource.Forestry,
        TransferManager.TransferReason.Grain or TransferManager.TransferReason.Flours or TransferManager.TransferReason.AnimalProducts => NaturalResource.Farming,
        TransferManager.TransferReason.Ore or TransferManager.TransferReason.Metals or TransferManager.TransferReason.Glass => NaturalResource.Ore,
        TransferManager.TransferReason.Oil or TransferManager.TransferReason.Plastics or TransferManager.TransferReason.Petroleum => NaturalResource.Oil,
        _ => NaturalResource.None,
    };

    public void ModifyUniqueFactoryProductionRate() {
        foreach (var uniqueFactoryProfile in UniqueFacilityPrefabBuffer) {
            uniqueFactoryProfile?.ModifyProductionRate(_modSetting.UniqueFacilityProductionRateMultiplier);
        }
    }

    public void ModifyExtractingFacilityProductionRate() {
        foreach (var extractingFacilityProfile in ExtractingFacilityPrefabBuffer) {
            extractingFacilityProfile?.ModifyProductionRate(_modSetting.ExtractingFacilityProductionRateMultiplier);
        }
    }

    public void ModifyProcessingFacilityProductionRate() {
        foreach (var processingFacilityProfile in ProcessingFacilityPrefabBuffer) {
            processingFacilityProfile?.ModifyProductionRate(_modSetting.ProcessingFacilityProductionRateMultiplier);
        }
    }

    public void Serialize(IWriter writer) {
        using var performanceCounter = PerformanceCounter.Start(c => Logger.Debug($"Serialize time spent: {c.ReportMilliseconds}"));

        _extractingFacilityModifies.Clear();
        _processingFacilityModifies.Clear();
        _uniqueFacilityModifies.Clear();
        _warehouseModifies.Clear();
        _mainIndustryBuildingModifies.Clear();
        _fishingHarborModifies.Clear();

        foreach (var extractingFacilityProfile in ExtractingFacilityPrefabBuffer) {
            extractingFacilityProfile.Validate();
            if (extractingFacilityProfile.Customized) {
                _extractingFacilityModifies.Add(extractingFacilityProfile.Name, extractingFacilityProfile);
            }
        }

        foreach (var processingFacilityProfile in ProcessingFacilityPrefabBuffer) {
            processingFacilityProfile.Validate();
            if (processingFacilityProfile.Customized) {
                _processingFacilityModifies.Add(processingFacilityProfile.Name, processingFacilityProfile);
            }
        }

        foreach (var uniqueFacilityProfile in UniqueFacilityPrefabBuffer) {
            uniqueFacilityProfile.Validate();
            if (uniqueFacilityProfile.Customized) {
                _uniqueFacilityModifies.Add(uniqueFacilityProfile.Name, uniqueFacilityProfile);
            }
        }

        foreach (var warehouseProfile in WarehousePrefabBuffer) {
            warehouseProfile.Validate();
            if (warehouseProfile.Customized) {
                _warehouseModifies.Add(warehouseProfile.Name, warehouseProfile);
            }
        }

        foreach (var mainIndustryBuildingProfile in MainIndustryPrefabBuffer) {
            mainIndustryBuildingProfile.Validate();
            if (mainIndustryBuildingProfile.Customized) {
                _mainIndustryBuildingModifies.Add(mainIndustryBuildingProfile.Name, mainIndustryBuildingProfile);
            }
        }

        foreach (var fishingHarborProfile in FishingHarborProfileBuffer) {
            fishingHarborProfile.Validate();
            if (fishingHarborProfile.Customized) {
                _fishingHarborModifies.Add(fishingHarborProfile.Name, fishingHarborProfile);
            }
        }

        writer.StartWrite("FacilitySetting", "FacilitySetting");
        writer.WriteProperty(nameof(_extractingFacilityModifies), _extractingFacilityModifies);
        writer.WriteProperty(nameof(_processingFacilityModifies), _processingFacilityModifies);
        writer.WriteProperty(nameof(_uniqueFacilityModifies), _uniqueFacilityModifies);
        writer.WriteProperty(nameof(_warehouseModifies), _warehouseModifies);
        writer.WriteProperty(nameof(_mainIndustryBuildingModifies), _mainIndustryBuildingModifies);
        writer.WriteProperty(nameof(_fishingHarborModifies), _fishingHarborModifies);
        writer.EndWrite();
    }

    public void Deserialize(IReader reader) {
        using var performanceCounter = PerformanceCounter.Start(c => Logger.Debug($"Deserialize time spent: {c.ReportMilliseconds}"));

        reader.StartRead("FacilitySetting", "FacilitySetting");
        reader.GetProperty(nameof(_extractingFacilityModifies), ref _extractingFacilityModifies);
        reader.GetProperty(nameof(_processingFacilityModifies), ref _processingFacilityModifies);
        reader.GetProperty(nameof(_uniqueFacilityModifies), ref _uniqueFacilityModifies);
        reader.GetProperty(nameof(_warehouseModifies), ref _warehouseModifies);
        reader.GetProperty(nameof(_mainIndustryBuildingModifies), ref _mainIndustryBuildingModifies);
        reader.GetProperty(nameof(_fishingHarborModifies), ref _fishingHarborModifies);
        reader.EndRead();

        for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
            var loaded = PrefabCollection<BuildingInfo>.GetLoaded(i);
            if (loaded == null || loaded.m_class == null) continue;
            if (loaded.m_class.m_service != ItemClass.Service.PlayerIndustry && loaded.m_class.m_service != ItemClass.Service.Fishing) continue;
            var ai = loaded.m_buildingAI;
            if (ai == null) continue;
            var type = ai.GetType();

            if (type == typeof(ExtractingFacilityAI)) {
                var extractingFacilityAI = ai as ExtractingFacilityAI;
                if (extractingFacilityAI == null) continue;

                var profile = new ExtractingFacilityProfile((ExtractingFacilityAI)ai);
                if (_extractingFacilityModifies.TryGetValue(profile.Name, out var modifies))
                    profile.SetFromLoadData(modifies);
                else {
                    profile.SetFromModData();
                    profile.ModifyProductionRate(_modSetting.ExtractingFacilityProductionRateMultiplier);
                }

                ExtractingFacilityPrefabBuffer.Add(profile);
                Logger.Debug($"Added extracting facility prefab to buffer: {profile.Name}");
                continue;
            }

            if (type == typeof(UniqueFactoryAI)) {
                var uniqueFactoryAI = ai as UniqueFactoryAI;
                if (uniqueFactoryAI == null) continue;
                var profile = new UniqueFactoryProfile((UniqueFactoryAI)ai);
                if (_uniqueFacilityModifies.TryGetValue(profile.Name, out var modifies))
                    profile.SetFromLoadData(modifies);
                else {
                    profile.SetFromModData();
                    profile.ModifyProductionRate(_modSetting.UniqueFacilityProductionRateMultiplier);
                }

                UniqueFacilityPrefabBuffer.Add(profile);
                Logger.Debug($"Added unique factory prefab to buffer: {profile.Name} {ai.m_info.name}");
                continue;
            }

            if (type == typeof(ProcessingFacilityAI)) {
                var processingFacilityAI = ai as ProcessingFacilityAI;
                if (processingFacilityAI == null) continue;

                var profile = new ProcessingFacilityProfile((ProcessingFacilityAI)ai);
                if (_processingFacilityModifies.TryGetValue(profile.Name, out var modifies))
                    profile.SetFromLoadData(modifies);
                else {
                    profile.SetFromModData();
                    profile.ModifyProductionRate(_modSetting.ProcessingFacilityProductionRateMultiplier);
                }

                ProcessingFacilityPrefabBuffer.Add(profile);
                Logger.Debug($"Added processing facility prefab to buffer: {profile.Name}");
                continue;
            }

            if (type == typeof(WarehouseAI)) {
                var warehouseAI = ai as WarehouseAI;
                if (warehouseAI == null) continue;

                var profile = new WarehouseProfile((WarehouseAI)ai);
                if (_warehouseModifies.TryGetValue(profile.Name, out var modifies))
                    profile.SetFromLoadData(modifies);
                else
                    profile.SetFromModData();
                WarehousePrefabBuffer.Add(profile);
                Logger.Info($"Added warehouse prefab to buffer: {profile.Name}");
                continue;
            }

            if (type == typeof(MainIndustryBuildingAI)) {
                var mainIndustryBuildingAI = ai as MainIndustryBuildingAI;
                if (mainIndustryBuildingAI == null) continue;

                var profile = new MainIndustryBuildingProfile((MainIndustryBuildingAI)ai);
                if (_mainIndustryBuildingModifies.TryGetValue(profile.Name, out var modifies))
                    profile.SetFromLoadData(modifies);
                else
                    profile.SetModDefaults();
                MainIndustryPrefabBuffer.Add(profile);
                Logger.Debug($"Added main industry building prefab to buffer: {profile.Name}");
                continue;
            }

            if (type == typeof(FishingHarborAI)) {
                var fishingHarborAI = ai as FishingHarborAI;
                if (fishingHarborAI == null) continue;

                var profile = new FishingHarborProfile((FishingHarborAI)ai);
                if (_fishingHarborModifies.TryGetValue(profile.Name, out var modifies))
                    profile.SetFromLoadData(modifies);
                else
                    profile.SetModDefaults();
                FishingHarborProfileBuffer.Add(profile);
                Logger.Debug($"Added fishing harbor prefab to buffer: {profile.Name}");
                continue;
            }
        }
    }
}