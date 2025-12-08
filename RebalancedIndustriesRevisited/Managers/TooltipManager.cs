using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using CSLModsCommon.Manager;
using RebalancedIndustriesRevisited.Data;
using RebalancedIndustriesRevisited.ModSettings;
using UnityEngine;

namespace RebalancedIndustriesRevisited.Managers;

public class TooltipManager : ManagerBase {
    private const string IndustryFarmingPanel = nameof(IndustryFarmingPanel);
    private const string IndustryForestryPanel = nameof(IndustryForestryPanel);
    private const string IndustryOilPanel = nameof(IndustryOilPanel);
    private const string IndustryOrePanel = nameof(IndustryOrePanel);
    private const string IndustryUniqueFactoryPanel = nameof(IndustryUniqueFactoryPanel);
    private const string IndustryWarehousesPanel = nameof(IndustryWarehousesPanel);
    private ModSetting _modSetting;

    private List<string> IndustryPanel { get; set; }
    public Dictionary<string, UIButton> IndustryPanelButtons { get; private set; }

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
    }


    public void InitTooltipString() {
        IndustryPanelButtons = new Dictionary<string, UIButton>();
        IndustryPanel = new List<string> {
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

    private void GetAllButtons() => IndustryPanel.ForEach(GetButtons);

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
                Logger.Error($"Couldn't find {panelName}.scrollablePanel");
            }
        }
        else {
            Logger.Error($"Couldn't find targetPanel");
        }
    }

    public void ModifyWorkSpaceString(WorkPlace rawWorkSpace, WorkPlace newWorkSpace, ref string tooltip) {
        var rawValue = rawWorkSpace.Sum();
        var newValue = newWorkSpace.Sum();
        if (rawValue == newValue) return;
        var rawWorkSpaceString = LocaleFormatter.FormatGeneric("AIINFO_WORKPLACES_ACCUMULATION", new object[] { rawValue.ToString() });
        var workSpaceString = LocaleFormatter.FormatGeneric("AIINFO_WORKPLACES_ACCUMULATION", new object[] { newValue.ToString() });
        tooltip = _modSetting.BothValue ? tooltip.Replace(rawWorkSpaceString, workSpaceString + AddOriginalValue(rawValue.ToString())) : tooltip.Replace(rawWorkSpaceString, workSpaceString);
    }

    public void ModifyMaintenanceCostString(int rawMaintenanceCost, int newMaintenanceCost, PlayerBuildingAI ai, ref string tooltip) {
        if (rawMaintenanceCost != newMaintenanceCost) {
            int result = rawMaintenanceCost * 100;
            Singleton<EconomyManager>.instance.m_EconomyWrapper.OnGetMaintenanceCost(ref result, ai.m_info.m_class.m_service, ai.m_info.m_class.m_subService, ai.m_info.m_class.m_level);
            var rawMaintenanceCostString = LocaleFormatter.FormatUpkeep(result, false);
            var maintenanceCostString = LocaleFormatter.FormatUpkeep(ai.GetMaintenanceCost(), false);
            var num = Mathf.Abs(result * 0.0016f);
            tooltip = _modSetting.BothValue ? tooltip.Replace(rawMaintenanceCostString, maintenanceCostString + AddOriginalValue(num.ToString((!(num >= 10f)) ?  Settings.moneyFormat : Settings.moneyFormatNoCents, LocaleManager.cultureInfo))) : tooltip.Replace(rawMaintenanceCostString, maintenanceCostString);
        }
    }

    public void ModifyConstructionCostString(int rawConstructionCost, int newConstructionCost, PlayerBuildingAI ai, ref string tooltip) {
        if (rawConstructionCost == newConstructionCost) return;
        var result = rawConstructionCost * 100;
        Singleton<EconomyManager>.instance.m_EconomyWrapper.OnGetConstructionCost(ref result, ai.m_info.m_class.m_service, ai.m_info.m_class.m_subService, ai.m_info.m_class.m_level);
        var rawConstructionCostString = LocaleFormatter.FormatCost(result, false);
        var constructionCostString = LocaleFormatter.FormatCost(ai.GetConstructionCost(), false);
        tooltip = _modSetting.BothValue ? tooltip.Replace(rawConstructionCostString, constructionCostString + AddOriginalValue((result * 0.01f).ToString(Settings.moneyFormatNoCents, LocaleManager.cultureInfo))) : tooltip.Replace(rawConstructionCostString, constructionCostString);
    }

    public void ModifyTruckCountString(int rawTruckCount, int newTruckCount, ref string tooltip) {
        if (rawTruckCount != newTruckCount) {
            tooltip = _modSetting.BothValue ? tooltip.Replace(string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), rawTruckCount), string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), newTruckCount) + AddOriginalValue(rawTruckCount.ToString())) : tooltip.Replace(string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), rawTruckCount), string.Format(Locale.Get("AIINFO_INDUSTRY_VEHICLE_COUNT"), newTruckCount));
        }
    }

    private string AddOriginalValue(string value) => $"({value})";
}