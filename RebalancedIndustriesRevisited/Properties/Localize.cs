namespace RebalancedIndustriesRevisited
{
	public class Localize
	{
		public static System.Globalization.CultureInfo Culture {get; set;}
		public static MbyronModsCommon.LocalizeManager LocaleManager {get;} = new MbyronModsCommon.LocalizeManager("Localize", typeof(Localize).Assembly);

		/// <summary>
		/// Animal products
		/// </summary>
		public static string AnimalProducts => LocaleManager.GetString("AnimalProducts", Culture);

		/// <summary>
		/// Display both corrected value and original value
		/// </summary>
		public static string BothValue => LocaleManager.GetString("BothValue", Culture);

		/// <summary>
		/// Crops
		/// </summary>
		public static string Crops => LocaleManager.GetString("Crops", Culture);

		/// <summary>
		/// Multiplier factor of the truck count in extracting facilities and processing facilities
		/// </summary>
		public static string EPMultiplierFactor => LocaleManager.GetString("EPMultiplierFactor", Culture);

		/// <summary>
		/// Extracting facility multiplier factor
		/// </summary>
		public static string ExtractingFacilityMultiplierFactor => LocaleManager.GetString("ExtractingFacilityMultiplierFactor", Culture);

		/// <summary>
		/// Flour
		/// </summary>
		public static string Flour => LocaleManager.GetString("Flour", Culture);

		/// <summary>
		/// Glass
		/// </summary>
		public static string Glass => LocaleManager.GetString("Glass", Culture);

		/// <summary>
		/// Industry warehouse truck count multiplier factor
		/// </summary>
		public static string IndustryWarehouseMultiplierFactor => LocaleManager.GetString("IndustryWarehouseMultiplierFactor", Culture);

		/// <summary>
		/// Load Multiplier Factor
		/// </summary>
		public static string LoadMultiplierFactor => LocaleManager.GetString("LoadMultiplierFactor", Culture);

		/// <summary>
		/// The adjustment of this option will change the load capacity of the vehicle, higher value means that 
		/// </summary>
		public static string LoadMultiplierFactorMinor => LocaleManager.GetString("LoadMultiplierFactorMinor", Culture);

		/// <summary>
		/// Metals
		/// </summary>
		public static string Metals => LocaleManager.GetString("Metals", Culture);

		/// <summary>
		/// Rebalances Industries DLC, reduce traffic flow, increase cargo loading and more.
		/// </summary>
		public static string MOD_Description => LocaleManager.GetString("MOD_Description", Culture);

		/// <summary>
		/// Oil
		/// </summary>
		public static string Oil => LocaleManager.GetString("Oil", Culture);

		/// <summary>
		/// Open Control Panel
		/// </summary>
		public static string OpenControlPanel => LocaleManager.GetString("OpenControlPanel", Culture);

		/// <summary>
		/// Ore
		/// </summary>
		public static string Ore => LocaleManager.GetString("Ore", Culture);

		/// <summary>
		/// Only display corrected value
		/// </summary>
		public static string OriginalValue => LocaleManager.GetString("OriginalValue", Culture);

		/// <summary>
		/// Other functions
		/// </summary>
		public static string OtherFunctionsMajor => LocaleManager.GetString("OtherFunctionsMajor", Culture);

		/// <summary>
		/// Use the shortcut keys or the UUI button to invoke the Control Panel, if you want to adjust the optio
		/// </summary>
		public static string OtherFunctionsMinor => LocaleManager.GetString("OtherFunctionsMinor", Culture);

		/// <summary>
		/// Paper
		/// </summary>
		public static string Paper => LocaleManager.GetString("Paper", Culture);

		/// <summary>
		/// Petroleum
		/// </summary>
		public static string Petroleum => LocaleManager.GetString("Petroleum", Culture);

		/// <summary>
		/// Planed timber
		/// </summary>
		public static string PlanedTimber => LocaleManager.GetString("PlanedTimber", Culture);

		/// <summary>
		/// Plastics
		/// </summary>
		public static string Plastics => LocaleManager.GetString("Plastics", Culture);

		/// <summary>
		/// Processing facility multiplier factor
		/// </summary>
		public static string ProcessingFacilityMultiplierFactor => LocaleManager.GetString("ProcessingFacilityMultiplierFactor", Culture);

		/// <summary>
		/// Processing materials load multiplier factor
		/// </summary>
		public static string ProcessingMaterialsLoadMultiplierFactor => LocaleManager.GetString("ProcessingMaterialsLoadMultiplierFactor", Culture);

		/// <summary>
		/// Production Rate
		/// </summary>
		public static string ProductionRate => LocaleManager.GetString("ProductionRate", Culture);

		/// <summary>
		/// This option can adjust the production rate of the extracting facilities and processing facilities, a
		/// </summary>
		public static string ProductionRateMinor => LocaleManager.GetString("ProductionRateMinor", Culture);

		/// <summary>
		/// This options can be used to regulate the output rate of industrial facilities. Reducing the output r
		/// </summary>
		public static string ProductionRateWarning => LocaleManager.GetString("ProductionRateWarning", Culture);

		/// <summary>
		/// Raw forest products
		/// </summary>
		public static string RawForestProducts => LocaleManager.GetString("RawForestProducts", Culture);

		/// <summary>
		/// Raw materials load multiplier factor
		/// </summary>
		public static string RawMaterialsLoadMultiplierFactor => LocaleManager.GetString("RawMaterialsLoadMultiplierFactor", Culture);

		/// <summary>
		/// Tooltip Box Mode
		/// </summary>
		public static string TooltipBoxMode => LocaleManager.GetString("TooltipBoxMode", Culture);

		/// <summary>
		/// Truck
		/// </summary>
		public static string Truck => LocaleManager.GetString("Truck", Culture);

		/// <summary>
		/// [FIX]Fixed incorrect display of vehicle count.
		/// </summary>
		public static string UpdateLog_V0_1_0FIX => LocaleManager.GetString("UpdateLog_V0_1_0FIX", Culture);

		/// <summary>
		/// [UPT]Using new algorithms to rebalances Industries.
		/// </summary>
		public static string UpdateLog_V0_1_0UPT1 => LocaleManager.GetString("UpdateLog_V0_1_0UPT1", Culture);

		/// <summary>
		/// [UPT]Using the latest Harmony API.
		/// </summary>
		public static string UpdateLog_V0_1_0UPT2 => LocaleManager.GetString("UpdateLog_V0_1_0UPT2", Culture);

		/// <summary>
		/// [FIX]Fix some processing buildings appear not enough buyer or not enough raw materials issues.
		/// </summary>
		public static string UpdateLog_V0_2_0FIX => LocaleManager.GetString("UpdateLog_V0_2_0FIX", Culture);

		/// <summary>
		/// [UPT]Update buffer recognition and rebinding logic.
		/// </summary>
		public static string UpdateLog_V0_2_0UPT => LocaleManager.GetString("UpdateLog_V0_2_0UPT", Culture);

		/// <summary>
		/// [FIX]Fixed an issue where non-vanilla buildings were not spawns vehicles correctly.
		/// </summary>
		public static string UpdateLog_V0_3_0FIX1 => LocaleManager.GetString("UpdateLog_V0_3_0FIX1", Culture);

		/// <summary>
		/// [FIX]Fixed an issue where work space was incorrect.
		/// </summary>
		public static string UpdateLog_V0_3_0FIX2 => LocaleManager.GetString("UpdateLog_V0_3_0FIX2", Culture);

		/// <summary>
		/// [FIX]Fixed compatibility issues with IMT and IPT mod.
		/// </summary>
		public static string UpdateLog_V0_3_0FIX3 => LocaleManager.GetString("UpdateLog_V0_3_0FIX3", Culture);

		/// <summary>
		/// [FIX]Fixed the exception throwing issues.
		/// </summary>
		public static string UpdateLog_V0_3_0FIX4 => LocaleManager.GetString("UpdateLog_V0_3_0FIX4", Culture);

		/// <summary>
		/// [FIX]Fixed incorrect maintenance costs for unique factories.
		/// </summary>
		public static string UpdateLog_V0_4_0FIX1 => LocaleManager.GetString("UpdateLog_V0_4_0FIX1", Culture);

		/// <summary>
		/// [FIX]Fixed issues where hovering over a building pops up tooltipbox with incorrect construction cost
		/// </summary>
		public static string UpdateLog_V0_4_0FIX2 => LocaleManager.GetString("UpdateLog_V0_4_0FIX2", Culture);

		/// <summary>
		/// [ADD]Added tooltip box mode option, allows display both corrected value and original value.
		/// </summary>
		public static string UpdateLog_V0_5_0ADD => LocaleManager.GetString("UpdateLog_V0_5_0ADD", Culture);

		/// <summary>
		/// [FIX]Fixed tooltip box for some buildings still not showing the corrected value.
		/// </summary>
		public static string UpdateLog_V0_5_0FIX1 => LocaleManager.GetString("UpdateLog_V0_5_0FIX1", Culture);

		/// <summary>
		/// [FIX]Fixed worker levelup requirements are too high issues.
		/// </summary>
		public static string UpdateLog_V0_5_0FIX2 => LocaleManager.GetString("UpdateLog_V0_5_0FIX2", Culture);

		/// <summary>
		/// [ADD]Added Extracting facility and processing facility output rate regulation.
		/// </summary>
		public static string UpdateLog_V0_6_0ADD => LocaleManager.GetString("UpdateLog_V0_6_0ADD", Culture);

		/// <summary>
		/// [FIX]Fixed some farm buildings overemployment issues.
		/// </summary>
		public static string UpdateLog_V0_6_0FIX => LocaleManager.GetString("UpdateLog_V0_6_0FIX", Culture);

		/// <summary>
		/// [OPT]Optimized rebinding prefab speed.
		/// </summary>
		public static string UpdateLog_V0_6_0OPT => LocaleManager.GetString("UpdateLog_V0_6_0OPT", Culture);

		/// <summary>
		/// [FIX]Fixed incorrect number of vehicles in the unique factory.
		/// </summary>
		public static string UpdateLog_V0_7_0FIX => LocaleManager.GetString("UpdateLog_V0_7_0FIX", Culture);

		/// <summary>
		/// [UPT]Updated framework and UI style.
		/// </summary>
		public static string UpdateLog_V0_7_0UPT => LocaleManager.GetString("UpdateLog_V0_7_0UPT", Culture);

		/// <summary>
		/// [ADD]Added advanced option for reset mod config.
		/// </summary>
		public static string UpdateLog_V0_7_1ADD => LocaleManager.GetString("UpdateLog_V0_7_1ADD", Culture);

		/// <summary>
		/// Added materials load control function.
		/// </summary>
		public static string UpdateLog_V0_8_0ADD => LocaleManager.GetString("UpdateLog_V0_8_0ADD", Culture);

		/// <summary>
		/// Optimized input/output buffer size control logic.
		/// </summary>
		public static string UpdateLog_V0_8_0OPT => LocaleManager.GetString("UpdateLog_V0_8_0OPT", Culture);

		/// <summary>
		/// Added Korean translation.
		/// </summary>
		public static string UpdateLog_V0_8_0TRA => LocaleManager.GetString("UpdateLog_V0_8_0TRA", Culture);

		/// <summary>
		/// Added Czech translation.
		/// </summary>
		public static string UpdateLog_V0_8_0TRA1 => LocaleManager.GetString("UpdateLog_V0_8_0TRA1", Culture);

		/// <summary>
		/// Updated to the latest common framework.
		/// </summary>
		public static string UpdateLog_V0_8_0UPT => LocaleManager.GetString("UpdateLog_V0_8_0UPT", Culture);

		/// <summary>
		/// Added control panel to provide more convenient control entry.
		/// </summary>
		public static string UpdateLog_V0_9ADD0 => LocaleManager.GetString("UpdateLog_V0_9ADD0", Culture);

		/// <summary>
		/// Added options to control the number of trucks in industrial facilities.
		/// </summary>
		public static string UpdateLog_V0_9ADD1 => LocaleManager.GetString("UpdateLog_V0_9ADD1", Culture);

		/// <summary>
		/// Fixed exception throws that can be caused by Customize It Extended.
		/// </summary>
		public static string UpdateLog_V0_9FIX0 => LocaleManager.GetString("UpdateLog_V0_9FIX0", Culture);

		/// <summary>
		/// Optimized rebinding logic handling.
		/// </summary>
		public static string UpdateLog_V0_9OPT0 => LocaleManager.GetString("UpdateLog_V0_9OPT0", Culture);

		/// <summary>
		/// Updated to support game version 1.17.1
		/// </summary>
		public static string UpdateLog_V0_9UPT0 => LocaleManager.GetString("UpdateLog_V0_9UPT0", Culture);

		/// <summary>
		/// Display both corrected value and original value: corrected value (original value), above options nee
		/// </summary>
		public static string ValueWarning => LocaleManager.GetString("ValueWarning", Culture);

		/// <summary>
		/// vanilla
		/// </summary>
		public static string Vanilla => LocaleManager.GetString("Vanilla", Culture);

		/// <summary>
		/// Warehouse truck count multiplier factor
		/// </summary>
		public static string WarehouseMultiplierFactor => LocaleManager.GetString("WarehouseMultiplierFactor", Culture);
	}
}