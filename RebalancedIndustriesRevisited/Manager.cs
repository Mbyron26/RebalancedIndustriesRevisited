using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using MbyronModsCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RebalancedIndustriesRevisited {
    public static class Manager {
        private const string IndustryFarmingPanel = nameof(IndustryFarmingPanel);
        private const string IndustryForestryPanel = nameof(IndustryForestryPanel);
        private const string IndustryOilPanel = nameof(IndustryOilPanel);
        private const string IndustryOrePanel = nameof(IndustryOrePanel);
        private const string IndustryUniqueFactoryPanel = nameof(IndustryUniqueFactoryPanel);
        private const string IndustryWarehousesPanel = nameof(IndustryWarehousesPanel);
        public static bool PrefabFlag { get; set; }
        private static List<UIButton> ButtonBuffer { get; set; } = new();

        public static void InitializePrefab() {
            if (PrefabFlag) {
                return;
            }
            PrefabFlag = true;

            ModLogger.ModLog("--------Start getting all building buttons--------", Config.Instance.DebugMode);
            GetAllButtons();
            ModLogger.ModLog("--------Get all building buttons done--------\n", Config.Instance.DebugMode);

            ModLogger.ModLog("--------Start rebinding building information--------");
            try {
                for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
                    if (PrefabCollection<BuildingInfo>.GetLoaded(i) != null) {
                        BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(i);
                        if (prefab.m_class.m_service == ItemClass.Service.PlayerIndustry && prefab.m_buildingAI is not null) {
                            RebindPrefab(prefab.m_buildingAI);
                        }
                    }
                }
                ModLogger.ModLog("--------Rebinding all building information done--------\n");
            }
            catch (Exception e) {
                ModLogger.ModLog(e.ToString());
            }

            DebugUtils.TimeCalculater(RefreshOutputRatePrefab);
        }

        private static Dictionary<string, int> ExtractingFacilityAIData { get; set; } = new();
        private static Dictionary<string, int> ProcessingFacilityAIData { get; set; } = new();
        public static void RefreshOutputRate() {
            for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
                if (PrefabCollection<BuildingInfo>.GetLoaded(i) != null) {
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(i);
                    if (prefab.m_class.m_service == ItemClass.Service.PlayerIndustry && prefab.m_buildingAI is not null) {
                        if (prefab.m_buildingAI is ExtractingFacilityAI ai1) {
                            if (ExtractingFacilityAIData.TryGetValue(ai1.name, out var value)) {
                                ai1.m_outputRate = (int)(value * Config.Instance.ExtractingFacilityProductionRate);
                            } else {
                                ModLogger.ModLog($"Couldn't found {ai1.name} in data buffer.");
                            }
                        } else if (prefab.m_buildingAI is ProcessingFacilityAI ai2) {
                            if (ProcessingFacilityAIData.TryGetValue(ai2.name, out var value)) {
                                ai2.m_outputRate = (int)(value * Config.Instance.ExtractingFacilityProductionRate);
                            } else {
                                ModLogger.ModLog($"Couldn't found {ai2.name} in data buffer.");
                            }
                        }
                    }
                }
            }
        }
        private static void RefreshOutputRatePrefab() {
            ModLogger.ModLog($"--------------------------------------------------------------------------------------");
            for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
                if (PrefabCollection<BuildingInfo>.GetLoaded(i) != null) {
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(i);
                    if (prefab.m_class.m_service == ItemClass.Service.PlayerIndustry && prefab.m_buildingAI is not null) {
                        if (prefab.m_buildingAI is ExtractingFacilityAI ai1) {
                            ExtractingFacilityAIData.Add(ai1.name, ai1.m_outputRate);
                            var raw1 = ai1.m_outputRate;
                            ai1.m_outputRate = (int)(ai1.m_outputRate * Config.Instance.ExtractingFacilityProductionRate);
                            ModLogger.ModLog($"{raw1} -> {ai1.m_outputRate} | {ai1.name}");
                        } else if (prefab.m_buildingAI is ProcessingFacilityAI ai2) {
                            ProcessingFacilityAIData.Add(ai2.name, ai2.m_outputRate);
                            var raw2 = ai2.m_outputRate;
                            ai2.m_outputRate = (int)(ai2.m_outputRate * Config.Instance.ProcessingFacilityProductionRate);
                            ModLogger.ModLog($"{raw2} -> {ai2.m_outputRate} | {ai2.name}");
                        }
                    }
                }
            }
            ModLogger.ModLog($"--------------------------------------------------------------------------------------");
        }

        private static void GetAllButtons() {
            if (!Config.Instance.DebugMode) return;
            GetButtons(IndustryFarmingPanel, _ => ButtonBuffer.Add(_));
            GetButtons(IndustryForestryPanel, _ => ButtonBuffer.Add(_));
            GetButtons(IndustryOilPanel, _ => ButtonBuffer.Add(_));
            GetButtons(IndustryOrePanel, _ => ButtonBuffer.Add(_));
            GetButtons(IndustryUniqueFactoryPanel, _ => ButtonBuffer.Add(_));
            GetButtons(IndustryWarehousesPanel, _ => ButtonBuffer.Add(_));
        }

        private static void GetButtons(string panelName, Action<UIButton> addButtons) {
            var targetPanel = UIView.Find<UIPanel>(panelName);
            if (targetPanel != null) {
                ModLogger.ModLog($"Found {targetPanel.name} succeed.");
                var scrollablePanel = targetPanel.Find<UIScrollablePanel>("ScrollablePanel");
                if (scrollablePanel != null) {
                    ModLogger.ModLog($"Found {targetPanel.name}.scrollablePanel succeed.");
                    foreach (var item in scrollablePanel.components) {
                        if (item is UIButton button) {
                            addButtons(button);
                            ModLogger.ModLog($"Got {button.name} succeed.", Config.Instance.DebugMode);
                        }
                    }
                } else {
                    ModLogger.ModLog($"Found {panelName}.scrollablePanel failed.");
                }
            } else {
                ModLogger.ModLog($"Couldn't find {targetPanel}");
            }
        }

        public static void RebindPrefab(BuildingAI ai) {
            if (ai is ExtractingFacilityAI extractingFacilityAI) {
                var name = extractingFacilityAI.name;
                var rawTruckCount = extractingFacilityAI.m_outputVehicleCount;
                var rawConstructionCost = extractingFacilityAI.m_constructionCost;
                var rawMaintenanceCost = extractingFacilityAI.m_maintenanceCost;
                var rawWorkSpace0 = extractingFacilityAI.m_workPlaceCount0;
                var rawWorkSpace1 = extractingFacilityAI.m_workPlaceCount1;
                var rawWorkSpace2 = extractingFacilityAI.m_workPlaceCount2;
                var rawWorkSpace3 = extractingFacilityAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkPlace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);
                IProfile profile = new ExtractingFacilityProfile(extractingFacilityAI);
                profile.SetTruckCount(ref extractingFacilityAI.m_outputVehicleCount);
                profile.SetConstructionCost(ref extractingFacilityAI.m_constructionCost);
                profile.SetMaintenanceCost(ref extractingFacilityAI.m_maintenanceCost);
                var newWorkPlace = profile.SetWorkPlace(ref extractingFacilityAI.m_workPlaceCount0, ref extractingFacilityAI.m_workPlaceCount1, ref extractingFacilityAI.m_workPlaceCount2, ref extractingFacilityAI.m_workPlaceCount3);
                ModLogger.ModLog($"Extracting Facility | Vehicle count: {rawTruckCount} -> {extractingFacilityAI.m_outputVehicleCount} | Construction cost: {rawConstructionCost} -> {extractingFacilityAI.m_constructionCost} | Maintenance cost: {rawMaintenanceCost} -> {extractingFacilityAI.m_maintenanceCost} | Work space: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {extractingFacilityAI.m_workPlaceCount0} {extractingFacilityAI.m_workPlaceCount1} {extractingFacilityAI.m_workPlaceCount2} {extractingFacilityAI.m_workPlaceCount3} | Building: {name}");
                var resource = extractingFacilityAI.NaturalResourceType switch {
                    NaturalResourceManager.Resource.Oil => IndustryOilPanel,
                    NaturalResourceManager.Resource.Ore => IndustryOrePanel,
                    NaturalResourceManager.Resource.Forest => IndustryForestryPanel,
                    NaturalResourceManager.Resource.Fertility => IndustryFarmingPanel,
                    _ => string.Empty
                };
                if (!resource.IsNullOrWhiteSpace()) {
                    try {
                        var panel = UIView.Find<UIPanel>(resource).Find<UIScrollablePanel>("ScrollablePanel");
                        if (panel is not null) {
                            var buttons = panel.components;
                            for (int i = 0; i < buttons.Count; i++) {
                                if (buttons[i].name == name) {
                                    var rawTooltip = buttons[i].tooltip;
                                    var newTooltip = rawTooltip;
                                    ModifyTruckCountString(rawTruckCount, extractingFacilityAI.m_outputVehicleCount, ref newTooltip);
                                    ModifyConstructionCostString(rawConstructionCost, extractingFacilityAI.m_constructionCost, extractingFacilityAI, ref newTooltip);
                                    ModifyMaintenanceCostString(rawMaintenanceCost, extractingFacilityAI.m_maintenanceCost, extractingFacilityAI, ref newTooltip);
                                    ModifyWorkSpaceString(rawWorkSpace, newWorkPlace, ref newTooltip);
                                    buttons[i].tooltip = newTooltip;
                                    ModLogger.ModLog($"Rebinding {name} tooltip:\n{rawTooltip} -> \n{buttons[i].tooltip}\n", Config.Instance.DebugMode);
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex) {
                        ModLogger.ModLog("Couldn't rebinding tooltip.", ex);
                    }
                }
            } else if (ai is UniqueFactoryAI uniqueFactoryAI) {
                var name = uniqueFactoryAI.name;
                var rawMaintenanceCost = uniqueFactoryAI.m_maintenanceCost;
                var rawWorkSpace0 = uniqueFactoryAI.m_workPlaceCount0;
                var rawWorkSpace1 = uniqueFactoryAI.m_workPlaceCount1;
                var rawWorkSpace2 = uniqueFactoryAI.m_workPlaceCount2;
                var rawWorkSpace3 = uniqueFactoryAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkPlace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);

                UniqueFactoryProfile profile = new(uniqueFactoryAI);
                profile.SetTruckCount(ref uniqueFactoryAI.m_outputVehicleCount);
                profile.SetConstructionCost(ref uniqueFactoryAI.m_constructionCost);
                profile.SetMaintenanceCost(ref uniqueFactoryAI.m_maintenanceCost);
                var newWorkPlace = profile.SetWorkPlace(ref uniqueFactoryAI.m_workPlaceCount0, ref uniqueFactoryAI.m_workPlaceCount1, ref uniqueFactoryAI.m_workPlaceCount2, ref uniqueFactoryAI.m_workPlaceCount3);
                if (profile.ProfileValue.CostsFactor != 1) {
                    ModLogger.ModLog($"Unique Factory | Maintenance cost: {rawMaintenanceCost} -> {uniqueFactoryAI.m_maintenanceCost} | Work space: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {uniqueFactoryAI.m_workPlaceCount0} {uniqueFactoryAI.m_workPlaceCount1} {uniqueFactoryAI.m_workPlaceCount2} {uniqueFactoryAI.m_workPlaceCount3} | Building: {name}");
                } else {
                    ModLogger.ModLog($"Unique Factory | No rebinding. | Building: {uniqueFactoryAI.name}");
                }
                try {
                    var panel = UIView.Find<UIPanel>(IndustryUniqueFactoryPanel).Find<UIScrollablePanel>("ScrollablePanel");
                    if (panel is not null) {
                        var buttons = panel.components;
                        for (int i = 0; i < buttons.Count; i++) {
                            if (buttons[i].name == name) {
                                var rawTooltip = buttons[i].tooltip;
                                string newTooltip = rawTooltip;
                                ModifyMaintenanceCostString(rawMaintenanceCost, uniqueFactoryAI.m_maintenanceCost, uniqueFactoryAI, ref newTooltip);
                                ModifyWorkSpaceString(rawWorkSpace, newWorkPlace, ref newTooltip);
                                buttons[i].tooltip = newTooltip;
                                ModLogger.ModLog($"Rebinding {name} tooltip:\n{rawTooltip} -> \n{buttons[i].tooltip}\n", Config.Instance.DebugMode);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    ModLogger.ModLog("Couldn't rebinding tooltip.", ex);
                }

            } else if (ai is ProcessingFacilityAI processingFacilityAI) {
                var name = processingFacilityAI.name;
                var rawTruckCount = processingFacilityAI.m_outputVehicleCount;
                var rawConstructionCost = processingFacilityAI.m_constructionCost;
                var rawMaintenanceCost = processingFacilityAI.m_maintenanceCost;
                var rawWorkSpace0 = processingFacilityAI.m_workPlaceCount0;
                var rawWorkSpace1 = processingFacilityAI.m_workPlaceCount1;
                var rawWorkSpace2 = processingFacilityAI.m_workPlaceCount2;
                var rawWorkSpace3 = processingFacilityAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkPlace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);
                IProfile profile = new ProcessingFacilityProfile(processingFacilityAI);
                profile.SetTruckCount(ref processingFacilityAI.m_outputVehicleCount);
                profile.SetConstructionCost(ref processingFacilityAI.m_constructionCost);
                profile.SetMaintenanceCost(ref processingFacilityAI.m_maintenanceCost);
                var newWorkPlace = profile.SetWorkPlace(ref processingFacilityAI.m_workPlaceCount0, ref processingFacilityAI.m_workPlaceCount1, ref processingFacilityAI.m_workPlaceCount2, ref processingFacilityAI.m_workPlaceCount3);
                ModLogger.ModLog($"Processing Facility | Vehicle count: {rawTruckCount} -> {processingFacilityAI.m_outputVehicleCount} | Construction cost: {rawConstructionCost} -> {processingFacilityAI.m_constructionCost} | Maintenance cost: {rawMaintenanceCost} -> {processingFacilityAI.m_maintenanceCost} | Work space: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {processingFacilityAI.m_workPlaceCount0} {processingFacilityAI.m_workPlaceCount1} {processingFacilityAI.m_workPlaceCount2} {processingFacilityAI.m_workPlaceCount3} | Building: {name}");
                var resource = processingFacilityAI.m_inputResource1 switch {
                    TransferManager.TransferReason.Oil => IndustryOilPanel,
                    TransferManager.TransferReason.Ore => IndustryOrePanel,
                    TransferManager.TransferReason.Logs => IndustryForestryPanel,
                    TransferManager.TransferReason.Grain => IndustryFarmingPanel,
                    _ => string.Empty
                };
                if (!resource.IsNullOrWhiteSpace()) {
                    try {
                        var panel = UIView.Find<UIPanel>(resource).Find<UIScrollablePanel>("ScrollablePanel");
                        if (panel is not null) {
                            var buttons = panel.components;
                            for (int i = 0; i < buttons.Count; i++) {
                                if (buttons[i].name == name) {
                                    var rawTooltip = buttons[i].tooltip;
                                    var newTooltip = rawTooltip;
                                    ModifyTruckCountString(rawTruckCount, processingFacilityAI.m_outputVehicleCount, ref newTooltip);
                                    ModifyConstructionCostString(rawConstructionCost, processingFacilityAI.m_constructionCost, processingFacilityAI, ref newTooltip);
                                    ModifyMaintenanceCostString(rawMaintenanceCost, processingFacilityAI.m_maintenanceCost, processingFacilityAI, ref newTooltip);
                                    ModifyWorkSpaceString(rawWorkSpace, newWorkPlace, ref newTooltip);
                                    buttons[i].tooltip = newTooltip;
                                    ModLogger.ModLog($"Rebinding {name} tooltip:\n{rawTooltip} -> \n{buttons[i].tooltip}\n", Config.Instance.DebugMode);
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex) {
                        ModLogger.ModLog("Couldn't rebinding tooltip.", ex);
                    }
                }

            } else if (ai is WarehouseAI warehouseAI) {
                var name = warehouseAI.name;
                var rawTruckCount = warehouseAI.m_truckCount;
                var rawConstructionCost = warehouseAI.m_constructionCost;
                var rawMaintenanceCost = warehouseAI.m_maintenanceCost;
                var rawWorkSpace0 = warehouseAI.m_workPlaceCount0;
                var rawWorkSpace1 = warehouseAI.m_workPlaceCount1;
                var rawWorkSpace2 = warehouseAI.m_workPlaceCount2;
                var rawWorkSpace3 = warehouseAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkPlace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);
                IProfile profile = new WarehouseProfile(warehouseAI);
                profile.SetTruckCount(ref warehouseAI.m_truckCount);
                profile.SetConstructionCost(ref warehouseAI.m_constructionCost);
                profile.SetMaintenanceCost(ref warehouseAI.m_maintenanceCost);
                var newWorkPlace = profile.SetWorkPlace(ref warehouseAI.m_workPlaceCount0, ref warehouseAI.m_workPlaceCount1, ref warehouseAI.m_workPlaceCount2, ref warehouseAI.m_workPlaceCount3);
                ModLogger.ModLog($"Warehouse | Vehicle count: {rawTruckCount} -> {warehouseAI.m_truckCount} | Construction cost: {rawConstructionCost} -> {warehouseAI.m_constructionCost} | Maintenance cost: {rawMaintenanceCost} -> {warehouseAI.m_maintenanceCost} | Work space0: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {warehouseAI.m_workPlaceCount0} {warehouseAI.m_workPlaceCount1} {warehouseAI.m_workPlaceCount2} {warehouseAI.m_workPlaceCount3} | Building: {name}");
                try {
                    var typePanel = warehouseAI.m_storageType switch {
                        TransferManager.TransferReason.Grain => IndustryFarmingPanel,
                        TransferManager.TransferReason.Logs => IndustryForestryPanel,
                        TransferManager.TransferReason.Ore => IndustryOrePanel,
                        TransferManager.TransferReason.Oil => IndustryOilPanel,
                        _ => IndustryWarehousesPanel
                    };
                    var panel = UIView.Find<UIPanel>(typePanel).Find<UIScrollablePanel>("ScrollablePanel");
                    if (panel is not null) {
                        var buttons = panel.components;
                        for (int i = 0; i < buttons.Count; i++) {
                            if (buttons[i].name == name) {
                                var rawTooltip = buttons[i].tooltip;
                                var newTooltip = rawTooltip;
                                ModifyTruckCountString(rawTruckCount, warehouseAI.m_truckCount, ref newTooltip);
                                ModifyConstructionCostString(rawConstructionCost, warehouseAI.m_constructionCost, warehouseAI, ref newTooltip);
                                ModifyMaintenanceCostString(rawMaintenanceCost, warehouseAI.m_maintenanceCost, warehouseAI, ref newTooltip);
                                ModifyWorkSpaceString(rawWorkSpace, newWorkPlace, ref newTooltip);
                                buttons[i].tooltip = newTooltip;
                                ModLogger.ModLog($"Rebinding {name} tooltip:\n{rawTooltip} -> \n{buttons[i].tooltip}\n", Config.Instance.DebugMode);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    ModLogger.ModLog("Couldn't rebinding tooltip.", ex);
                }
            }
        }

        private static void ModifyWorkSpaceString(WorkPlace rawWorkSpace, WorkPlace newWorkSpace, ref string tooltip) {
            var rawValue = rawWorkSpace.Sum();
            var newValue = newWorkSpace.Sum();
            if (rawValue != newValue) {
                var RawWorkSpaceString = LocaleFormatter.FormatGeneric("AIINFO_WORKPLACES_ACCUMULATION", new object[] { rawValue.ToString() });
                var WorkSpaceString = LocaleFormatter.FormatGeneric("AIINFO_WORKPLACES_ACCUMULATION", new object[] { newValue.ToString() });
                tooltip = Config.Instance.BothValue ? tooltip.Replace(RawWorkSpaceString, WorkSpaceString + AddOriginalValue(rawValue.ToString())) : tooltip.Replace(RawWorkSpaceString, WorkSpaceString);
            }
        }

        private static void ModifyMaintenanceCostString(int rawMaintenanceCost, int newMaintenanceCost, PlayerBuildingAI ai, ref string tooltip) {
            if (rawMaintenanceCost != newMaintenanceCost) {
                int result = rawMaintenanceCost * 100;
                Singleton<EconomyManager>.instance.m_EconomyWrapper.OnGetMaintenanceCost(ref result, ai.m_info.m_class.m_service, ai.m_info.m_class.m_subService, ai.m_info.m_class.m_level);
                var RawMaintenancenCostString = LocaleFormatter.FormatUpkeep(result, false);
                var MaintenanceCostString = LocaleFormatter.FormatUpkeep(ai.GetMaintenanceCost(), false);
                float num = Mathf.Abs(result * 0.0016f);
                tooltip = Config.Instance.BothValue ? tooltip.Replace(RawMaintenancenCostString, MaintenanceCostString + AddOriginalValue(num.ToString((!(num >= 10f)) ? Settings.moneyFormat : Settings.moneyFormatNoCents, LocaleManager.cultureInfo))) : tooltip.Replace(RawMaintenancenCostString, MaintenanceCostString);
            }
        }

        private static void ModifyConstructionCostString(int rawConstructionCost, int newConstructionCost, PlayerBuildingAI ai, ref string tooltip) {
            if (rawConstructionCost != newConstructionCost) {
                int result = rawConstructionCost * 100;
                Singleton<EconomyManager>.instance.m_EconomyWrapper.OnGetConstructionCost(ref result, ai.m_info.m_class.m_service, ai.m_info.m_class.m_subService, ai.m_info.m_class.m_level);
                var RawConstructionCostString = LocaleFormatter.FormatCost(result, false);
                var ConstructionCostString = LocaleFormatter.FormatCost(ai.GetConstructionCost(), false);
                tooltip = Config.Instance.BothValue ? tooltip.Replace(RawConstructionCostString, ConstructionCostString + AddOriginalValue((result * 0.01f).ToString(Settings.moneyFormatNoCents, LocaleManager.cultureInfo))) : tooltip.Replace(RawConstructionCostString, ConstructionCostString);
            }
        }

        private static void ModifyTruckCountString(int rawTruckCount, int newTruckCount, ref string tooltip) {
            if (rawTruckCount != newTruckCount) {
                tooltip = Config.Instance.BothValue ? tooltip.Replace(string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), rawTruckCount), string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), newTruckCount) + AddOriginalValue(rawTruckCount.ToString())) : tooltip.Replace(string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), rawTruckCount), string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), newTruckCount));
            }
        }

        private static string AddOriginalValue(string value) => $"({value})";

        public static void SetCargoCapacity(Vehicle data, CargoTruckAI cargoTruckAI, int capacity) {
            var type = (TransferManager.TransferReason)data.m_transferType;
            var categories = GetIndustriesType(type) switch {
                IndustriesType.Forestry => true,
                IndustriesType.Farming => true,
                IndustriesType.Ore => true,
                IndustriesType.Oil => true,
                _ => false
            };
            if (categories) {
                SetCargoCapacity(cargoTruckAI, capacity);
            }
        }
        public static void SetCargoCapacity(CargoTruckAI cargoTruckAI, int capacity) => cargoTruckAI.m_cargoCapacity = capacity;
        public static IndustriesType GetIndustriesType(TransferManager.TransferReason transferReason) => transferReason switch {
            TransferManager.TransferReason.Logs => IndustriesType.Forestry,
            TransferManager.TransferReason.Paper => IndustriesType.Forestry,
            TransferManager.TransferReason.PlanedTimber => IndustriesType.Forestry,
            TransferManager.TransferReason.Grain => IndustriesType.Farming,
            TransferManager.TransferReason.Flours => IndustriesType.Farming,
            TransferManager.TransferReason.AnimalProducts => IndustriesType.Farming,
            TransferManager.TransferReason.Ore => IndustriesType.Ore,
            TransferManager.TransferReason.Metals => IndustriesType.Ore,
            TransferManager.TransferReason.Glass => IndustriesType.Ore,
            TransferManager.TransferReason.Oil => IndustriesType.Oil,
            TransferManager.TransferReason.Plastics => IndustriesType.Oil,
            TransferManager.TransferReason.Petroleum => IndustriesType.Oil,
            _ => IndustriesType.None,
        };

    }

    public enum IndustriesType {
        None,
        Forestry,
        Farming,
        Ore,
        Oil,
    }

    public class WarehouseProfile : ProfileBase<WarehouseAI> {
        public const decimal TruckCountFactor = 0.5m;
        public const int MinWarehouseCapacity = 40000;

        public WarehouseProfile(WarehouseAI ai) {
            AI = ai;
            InitializePrefab();
        }

        public override void InitializePrefab() {
            NewConstructionCost = RawConstructionCost = AI.m_constructionCost;
            NewMaintenanceCost = RawMaintenanceCost = AI.m_maintenanceCost;
            NewTruckCount = RawTruckCount = AI.m_truckCount;
            NewWorkPlace = RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
            var costFactor = AI.m_storageType switch {
                TransferManager.TransferReason.Grain => 2m,
                TransferManager.TransferReason.Logs or
                TransferManager.TransferReason.Ore or
                TransferManager.TransferReason.Oil => 1m,
                _ => 0.5m
            };
            NewConstructionCost = Convert.ToInt32(NewConstructionCost / costFactor);
            NewMaintenanceCost = Convert.ToInt32(NewMaintenanceCost / costFactor);
            var truckFactor = GetTruckFactor(AI.m_storageType);
            if (IsRawWarehouse(AI.m_storageType)) {
                truckFactor = (truckFactor - 1) / 1.5m + 1;
                NewTruckCount = (int)Math.Ceiling(NewTruckCount / truckFactor);
                if (AI.m_storageCapacity <= MinWarehouseCapacity) {
                    NewTruckCount = EMath.Max(1, NewTruckCount);
                } else {
                    NewTruckCount = EMath.Max(2, NewTruckCount);
                }
            } else {
                NewTruckCount = (int)Math.Ceiling(NewTruckCount * truckFactor);
                if (AI.m_storageCapacity <= MinWarehouseCapacity) {
                    NewTruckCount = EMath.Max(1, NewTruckCount);
                } else {
                    NewTruckCount = EMath.Max(2, NewTruckCount);
                }
            }
            var workPlaceFactor = AI.m_storageType switch {
                TransferManager.TransferReason.Grain => 4m,
                TransferManager.TransferReason.Logs or
                TransferManager.TransferReason.Ore or
                TransferManager.TransferReason.Oil => 2m,
                _ => 1m
            };
            if (NewWorkPlace.Sum() > 10) {
                NewWorkPlace = new(Convert.ToInt32(Math.Round(NewWorkPlace.UneducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(NewWorkPlace.EducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(NewWorkPlace.WellEducatedWorkers / workPlaceFactor)), Convert.ToInt32(Math.Round(NewWorkPlace.HighlyEducatedWorkers / workPlaceFactor)));
            }

        }

        private static bool IsRawWarehouse(TransferManager.TransferReason material) => material switch {
            TransferManager.TransferReason.Grain or
            TransferManager.TransferReason.Logs or
            TransferManager.TransferReason.Ore or
            TransferManager.TransferReason.Oil => true,
            _ => false,
        };
    }

    public class ProcessingFacilityProfile : ProfileBase<ProcessingFacilityAI> {
        public static string[] ProcessorField { get; } = { "Animal Pasture 01", "Animal Pasture 02", "Cattle Shed 01" };

        public ProcessingFacilityProfile(ProcessingFacilityAI ai) {
            AI = ai;
            InitializePrefab();
        }

        public override void InitializePrefab() {
            NewConstructionCost = RawConstructionCost = AI.m_constructionCost;
            NewMaintenanceCost = RawMaintenanceCost = AI.m_maintenanceCost;
            NewTruckCount = RawTruckCount = AI.m_outputVehicleCount;
            NewWorkPlace = RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
            bool isField = false;
            foreach (var item in ProcessorField) {
                if (AI.name == item) {
                    isField = true;
                }
            }
            if (isField) {
                var costsFactor = 0.5m;
                var workersFactor = 0.35m;
                NewConstructionCost = (int)(RawConstructionCost * costsFactor);
                NewMaintenanceCost = (int)(RawMaintenanceCost * costsFactor);
                NewWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(AI.m_workPlaceCount0 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount1 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount2 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount3 * workersFactor)));
            }
            var truckFactor = GetTruckFactor(AI.m_outputResource);
            if (truckFactor != 0) {
                NewTruckCount = EMath.Clamp((int)Math.Ceiling(NewTruckCount / truckFactor), MinVehicles, NewTruckCount);
            }
        }

    }

    public class UniqueFactoryProfile : ProfileBase<UniqueFactoryAI> {
        public UniqueFactoryAIValue ProfileValue { get; private set; }

        public UniqueFactoryProfile(UniqueFactoryAI ai) {
            AI = ai;
            InitializePrefab();
        }

        public override void InitializePrefab() {
            RawConstructionCost = AI.m_constructionCost;
            RawMaintenanceCost = AI.m_maintenanceCost;
            RawTruckCount = AI.m_outputVehicleCount;
            RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
            ProfileValue = AI.name switch {
                "Furniture Factory 01" => new UniqueFactoryAIValue(320, new WorkPlace(25, 18, 8, 4)),
                "Bakery 01" => new UniqueFactoryAIValue(260, new WorkPlace(15, 9, 4, 2)),
                "Industrial Steel Plant 01" => new UniqueFactoryAIValue(1800, new WorkPlace(60, 45, 30, 5)),
                "Household Plastic Factory 01" => new UniqueFactoryAIValue(480, new WorkPlace(25, 18, 8, 4)),
                "Toy Factory 01" => new UniqueFactoryAIValue(760, new WorkPlace(25, 18, 8, 4)),
                "Printing Press 01" => new UniqueFactoryAIValue(560, new WorkPlace(22, 16, 8, 4)),
                "Lemonade Factory 01" => new UniqueFactoryAIValue(800, new WorkPlace(55, 35, 15, 5)),
                "Electronics Factory 01" => new UniqueFactoryAIValue(1800, new WorkPlace(55, 40, 20, 10)),
                "Clothing Factory 01" => new UniqueFactoryAIValue(840, new WorkPlace(35, 20, 10, 5)),
                "Petroleum Refinery 01" => new UniqueFactoryAIValue(2600, new WorkPlace(60, 45, 30, 15)),
                "Soft Paper Factory 01" => new UniqueFactoryAIValue(2200, new WorkPlace(60, 50, 12, 8)),
                "Car Factory 01" => new UniqueFactoryAIValue(3400, new WorkPlace(70, 60, 20, 10)),
                "Sneaker Factory 01" => new UniqueFactoryAIValue(1920, new WorkPlace(35, 30, 10, 5)),
                "Modular House Factory 01" => new UniqueFactoryAIValue(2400, new WorkPlace(70, 45, 15, 10)),
                "Food Factory 01" => new UniqueFactoryAIValue(1920, new WorkPlace(55, 35, 15, 5)),
                "Dry Dock 01" => new UniqueFactoryAIValue(3800, new WorkPlace(80, 50, 20, 10)),
                _ => new UniqueFactoryAIValue(1, new WorkPlace()),
            };
            NewConstructionCost = AI.m_constructionCost;
            NewMaintenanceCost = ProfileValue.CostsFactor != 1 ? ProfileValue.CostsFactor * 100 / 16 : AI.m_maintenanceCost;
            NewWorkPlace = ProfileValue.CostsFactor != 1 ? ProfileValue.WorkPlaceValue : new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
        }

        public struct UniqueFactoryAIValue {
            public int CostsFactor;
            public WorkPlace WorkPlaceValue;
            public UniqueFactoryAIValue(int costsFactor, WorkPlace workPlaceValue) {
                CostsFactor = costsFactor;
                WorkPlaceValue = workPlaceValue;
            }
        }

    }

    public class ExtractingFacilityProfile : ProfileBase<ExtractingFacilityAI> {
        public ExtractingFacilityProfile(ExtractingFacilityAI ai) {
            AI = ai;
            InitializePrefab();
        }

        public override void InitializePrefab() {
            RawConstructionCost = AI.m_constructionCost;
            RawMaintenanceCost = AI.m_maintenanceCost;
            RawTruckCount = AI.m_outputVehicleCount;
            RawWorkPlace = new WorkPlace(AI.m_workPlaceCount0, AI.m_workPlaceCount1, AI.m_workPlaceCount2, AI.m_workPlaceCount3);
            var costFactor = AI.m_outputResource == TransferManager.TransferReason.Grain ? 0.25m : 1m;
            NewConstructionCost = (int)Math.Ceiling(RawConstructionCost * costFactor);
            NewMaintenanceCost = (int)Math.Ceiling(RawMaintenanceCost * costFactor);
            var truckFactor = GetTruckFactor(AI.m_outputResource);
            if (truckFactor != 0) {
                NewTruckCount = EMath.Clamp((int)Math.Ceiling(RawTruckCount / truckFactor), MinVehicles, RawTruckCount);
            } else {
                NewTruckCount = RawTruckCount;
            }
            NewWorkPlace = RawWorkPlace;
            List<int> newWorkSpace = new();
            if (AI.m_outputResource == TransferManager.TransferReason.Grain) {
                var workPlace = new WorkPlace(2, 4, 1, 0);
                int workPlaceSum = (int)Math.Ceiling(Math.Sqrt(AI.m_info.m_cellLength * AI.m_info.m_cellWidth) / 2);
                if (workPlaceSum != 0) {
                    foreach (int item in workPlace) {
                        newWorkSpace.Add((int)(GetRatio(item, workPlace.Sum()) * workPlaceSum));
                    }
                    NewWorkPlace = new WorkPlace(newWorkSpace[0], newWorkSpace[1], newWorkSpace[2], newWorkSpace[3]);
                }
            } else {
                var workersFactor = 0.5m;
                NewWorkPlace = new WorkPlace(Convert.ToInt32(Math.Round(AI.m_workPlaceCount0 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount1 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount2 * workersFactor)), Convert.ToInt32(Math.Round(AI.m_workPlaceCount3 * workersFactor)));
            }
        }

    }

    public abstract class ProfileBase<TypeAI> : IProfile where TypeAI : PlayerBuildingAI {
        public const int MinVehicles = 2;

        public TypeAI AI { get; protected set; }
        public int RawConstructionCost { get; protected set; }
        public int NewConstructionCost { get; protected set; }
        public int RawMaintenanceCost { get; protected set; }
        public int NewMaintenanceCost { get; protected set; }
        public int RawTruckCount { get; protected set; }
        public int NewTruckCount { get; protected set; }
        public WorkPlace RawWorkPlace { get; protected set; }
        public WorkPlace NewWorkPlace { get; protected set; }

        public abstract void InitializePrefab();
        public virtual void SetConstructionCost(ref int constructionCost) => constructionCost = NewConstructionCost;
        public virtual void SetMaintenanceCost(ref int maintenanceCost) => maintenanceCost = NewMaintenanceCost;
        public virtual void SetTruckCount(ref int truckCount) => truckCount = NewTruckCount;
        public virtual WorkPlace SetWorkPlace(ref int uneducatedWorkers, ref int educatedWorkers, ref int wellEducatedWorkers, ref int highlyEducatedWorkers) {
            uneducatedWorkers = NewWorkPlace.UneducatedWorkers;
            educatedWorkers = NewWorkPlace.EducatedWorkers;
            wellEducatedWorkers = NewWorkPlace.WellEducatedWorkers;
            highlyEducatedWorkers = NewWorkPlace.HighlyEducatedWorkers;
            return NewWorkPlace;
        }

        protected static decimal GetTruckFactor(TransferManager.TransferReason material) => material switch {
            TransferManager.TransferReason.Grain => 3m,
            TransferManager.TransferReason.AnimalProducts => 1.5m,
            TransferManager.TransferReason.Flours => 1.5m,
            TransferManager.TransferReason.Logs => 2m,
            TransferManager.TransferReason.PlanedTimber => 2m,
            TransferManager.TransferReason.Paper => 2m,
            TransferManager.TransferReason.Ore => 1.5m,
            TransferManager.TransferReason.Metals => 1.5m,
            TransferManager.TransferReason.Glass => 1.5m,
            TransferManager.TransferReason.Oil => 3m,
            TransferManager.TransferReason.Plastics => 1.5m,
            TransferManager.TransferReason.Petroleum => 2m,
            _ => 0.5m
        };
        protected decimal GetRatio(decimal numerator, int denominator) => Math.Round(numerator / denominator, 2);
    }

    public interface IProfile {
        void InitializePrefab();
        void SetConstructionCost(ref int constructionCost);
        void SetMaintenanceCost(ref int maintenanceCost);
        void SetTruckCount(ref int truckCount);
        WorkPlace SetWorkPlace(ref int uneducatedWorkers, ref int educatedWorkers, ref int wellEducatedWorkers, ref int highlyEducatedWorkers);
    }

    public readonly struct WorkPlace : IEnumerable {
        public readonly int UneducatedWorkers;
        public readonly int EducatedWorkers;
        public readonly int WellEducatedWorkers;
        public readonly int HighlyEducatedWorkers;

        public WorkPlace(int uneducatedWorkers, int educatedWorkers, int wellEducatedWorkers, int highlyEducatedWorkers) {
            UneducatedWorkers = uneducatedWorkers;
            EducatedWorkers = educatedWorkers;
            WellEducatedWorkers = wellEducatedWorkers;
            HighlyEducatedWorkers = highlyEducatedWorkers;
        }

        public IEnumerator GetEnumerator() {
            yield return UneducatedWorkers;
            yield return EducatedWorkers;
            yield return WellEducatedWorkers;
            yield return HighlyEducatedWorkers;
        }

        public int Sum() => UneducatedWorkers + EducatedWorkers + WellEducatedWorkers + HighlyEducatedWorkers;
    }
}
