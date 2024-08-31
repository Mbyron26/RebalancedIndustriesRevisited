using ColossalFramework;
using ColossalFramework.UI;
using CSShared.Common;
using CSShared.ToolButton;
using CSShared.Tools;
using CSShared.UI.ControlPanel;
using UnityEngine;

namespace RebalancedIndustriesRevisited.UI;

internal class ToolButtonManager : UUIToolManagerBase<ToolButton, Mod, Config> {
    protected override Texture2D UUIIcon { get; } = CSShared.UI.UIUtils.LoadTextureFromAssembly($"{AssemblyTools.CurrentAssemblyName}.UI.Resources.InGameButton.png");
    protected override string Tooltip => SingletonMod<Mod>.Instance.ModName + $" ({SavedInputKey.ToLocalizedString("KEYNAME", Config.Instance.ControlPanelHotkey.Encode())})";

    protected override void InGameToolButtonToggle(bool isOn) => ControlPanelManager<Mod, ControlPanel>.CallPanel();
    protected override void UUIButtonToggle(bool isOn) => ControlPanelManager<Mod, ControlPanel>.CallPanel();
}

internal class ToolButton : ToolButtonBase<Config> {
    public override Vector2 DefaultPosition { get; set; } = GetDefaultPosition();
    public override void Start() {
        base.Start();
        fgAtlas = UIAtlas.RebalancedIndustriesRevisitedAtlas;
        offFgSprites.SetSprites(UIAtlas.InGameButton);
        onFgSprites.SetSprites(UIAtlas.InGameButton);
        renderFg = true;
    }
    private static Vector2 GetDefaultPosition() {
        Vector2 resolution = UIView.GetAView().GetScreenResolution();
        var pos = new Vector2(resolution.x - 60f, resolution.y * 3f / 4f + 180);
        return pos;
    }
}
