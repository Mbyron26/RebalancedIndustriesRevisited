using CSLModsCommon.KeyBindings;
using CSLModsCommon.Setting;
using UnityEngine;

namespace RebalancedIndustriesRevisited.ModSettings;

[FileLocation(nameof(RebalancedIndustriesRevisited) + nameof(ModSetting))]
public class ModSetting : ModSettingBase {
    public bool BothValue { get; set; }

    public float RawMaterialsLoadMultiplierFactor { get; set; } = 2f;
    public float ProcessingMaterialsLoadMultiplierFactor { get; set; } = 2f;

    public KeyBinding ControlPanelToggle { get; set; } = new(new KeyCombination(KeyCode.R, true, true, false));

    public override void SetDefaults() {
        base.SetDefaults();
        BothValue = false;
        RawMaterialsLoadMultiplierFactor = 2f;
        ProcessingMaterialsLoadMultiplierFactor = 2f;
        ControlPanelToggle.Reset();
    }
}