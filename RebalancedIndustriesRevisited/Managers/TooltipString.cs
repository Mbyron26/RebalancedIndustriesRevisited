namespace RebalancedIndustriesRevisited;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ColossalFramework;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Manager {
    private const string IndustryFarmingPanel = nameof(IndustryFarmingPanel);
    private const string IndustryForestryPanel = nameof(IndustryForestryPanel);
    private const string IndustryOilPanel = nameof(IndustryOilPanel);
    private const string IndustryOrePanel = nameof(IndustryOrePanel);
    private const string IndustryUniqueFactoryPanel = nameof(IndustryUniqueFactoryPanel);
    private const string IndustryWarehousesPanel = nameof(IndustryWarehousesPanel);

    private List<string> IndustryPanel { get; set; }
    public Dictionary<string, UIButton> IndustryPanelButtons { get; private set; }

    [Obsolete]
    public void RefreshAllTooltip() {
        if (!IsInit)
            return;
        DeInitTooltipString();
        InitTooltipString();
        ExtractingFacilityAICache.ForEach(_ => _.Value.RebindTooltip());
        ProcessingFacilityAICache.ForEach(_ => _.Value.RebindTooltip());
        UniqueFactoryAICache.ForEach(_ => _.Value.RebindTooltip());
        WarehouseAICache.ForEach(_ => _.Value.RebindTooltip());
    }

    public void InitTooltipString() {
        IndustryPanelButtons = new();
        IndustryPanel = new() {
            IndustryFarmingPanel,
            IndustryForestryPanel,
            IndustryOilPanel,
            IndustryOrePanel,
            IndustryUniqueFactoryPanel,
            IndustryWarehousesPanel
        };
        GetAllButtons();
    }

    public void DeInitTooltipString() {
        IndustryPanelButtons = null;
        IndustryPanel = null;
    }

    private void GetAllButtons() => IndustryPanel.ForEach(_ => GetButtons(_));

    private void GetButtons(string panelName) {
        var targetPanel = UIView.Find<UIPanel>(panelName);
        if (targetPanel is not null) {
            var scrollablePanel = targetPanel.Find<UIScrollablePanel>("ScrollablePanel");
            if (scrollablePanel is not null) {
                foreach (var item in scrollablePanel.components) {
                    if (item is UIButton button) {
                        IndustryPanelButtons.Add(button.name, button);
                    }
                }
            }
            else {
                Mod.Log.Error($"Couldn't find {panelName}.scrollablePanel");
            }
        }
        else {
            Mod.Log.Error($"Couldn't find {targetPanel}");
        }
    }

    public void ModifyWorkSpaceString(WorkPlace rawWorkSpace, WorkPlace newWorkSpace, ref string tooltip) {
        var rawValue = rawWorkSpace.Sum();
        var newValue = newWorkSpace.Sum();
        if (rawValue != newValue) {
            var RawWorkSpaceString = LocaleFormatter.FormatGeneric("AIINFO_WORKPLACES_ACCUMULATION", new object[] { rawValue.ToString() });
            var WorkSpaceString = LocaleFormatter.FormatGeneric("AIINFO_WORKPLACES_ACCUMULATION", new object[] { newValue.ToString() });
            tooltip = Config.Instance.BothValue ? tooltip.Replace(RawWorkSpaceString, WorkSpaceString + AddOriginalValue(rawValue.ToString())) : tooltip.Replace(RawWorkSpaceString, WorkSpaceString);
        }
    }

    public void ModifyMaintenanceCostString(int rawMaintenanceCost, int newMaintenanceCost, PlayerBuildingAI ai, ref string tooltip) {
        if (rawMaintenanceCost != newMaintenanceCost) {
            int result = rawMaintenanceCost * 100;
            Singleton<EconomyManager>.instance.m_EconomyWrapper.OnGetMaintenanceCost(ref result, ai.m_info.m_class.m_service, ai.m_info.m_class.m_subService, ai.m_info.m_class.m_level);
            var RawMaintenancenCostString = LocaleFormatter.FormatUpkeep(result, false);
            var MaintenanceCostString = LocaleFormatter.FormatUpkeep(ai.GetMaintenanceCost(), false);
            float num = Mathf.Abs(result * 0.0016f);
            tooltip = Config.Instance.BothValue ? tooltip.Replace(RawMaintenancenCostString, MaintenanceCostString + AddOriginalValue(num.ToString((!(num >= 10f)) ? Settings.moneyFormat : Settings.moneyFormatNoCents, LocaleManager.cultureInfo))) : tooltip.Replace(RawMaintenancenCostString, MaintenanceCostString);
        }
    }

    public void ModifyConstructionCostString(int rawConstructionCost, int newConstructionCost, PlayerBuildingAI ai, ref string tooltip) {
        if (rawConstructionCost != newConstructionCost) {
            int result = rawConstructionCost * 100;
            Singleton<EconomyManager>.instance.m_EconomyWrapper.OnGetConstructionCost(ref result, ai.m_info.m_class.m_service, ai.m_info.m_class.m_subService, ai.m_info.m_class.m_level);
            var RawConstructionCostString = LocaleFormatter.FormatCost(result, false);
            var ConstructionCostString = LocaleFormatter.FormatCost(ai.GetConstructionCost(), false);
            tooltip = Config.Instance.BothValue ? tooltip.Replace(RawConstructionCostString, ConstructionCostString + AddOriginalValue((result * 0.01f).ToString(Settings.moneyFormatNoCents, LocaleManager.cultureInfo))) : tooltip.Replace(RawConstructionCostString, ConstructionCostString);
        }
    }

    public void ModifyTruckCountString(int rawTruckCount, int newTruckCount, ref string tooltip) {
        if (rawTruckCount != newTruckCount) {
            tooltip = Config.Instance.BothValue ? tooltip.Replace(string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), rawTruckCount), string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), newTruckCount) + AddOriginalValue(rawTruckCount.ToString())) : tooltip.Replace(string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), rawTruckCount), string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), newTruckCount));
        }
    }

    private string AddOriginalValue(string value) => $"({value})";
}
