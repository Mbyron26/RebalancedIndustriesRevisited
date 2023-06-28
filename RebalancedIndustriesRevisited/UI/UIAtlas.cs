namespace RebalancedIndustriesRevisited.UI;
using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

internal static class UIAtlas {
    private static UITextureAtlas rebalancedIndustriesRevisitedAtlas;

    public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();
    public static string InGameButton => nameof(InGameButton);
    public static UITextureAtlas RebalancedIndustriesRevisitedAtlas {
        get {
            if (rebalancedIndustriesRevisitedAtlas is null) {
                rebalancedIndustriesRevisitedAtlas = MbyronModsCommon.UI.UIUtils.CreateTextureAtlas(nameof(RebalancedIndustriesRevisitedAtlas), $"{AssemblyUtils.CurrentAssemblyName}.UI.Resources.", SpriteParams);
                InternalLogger.Log("Initialized RebalancedIndustriesRevisitedAtlas");
            }
            return rebalancedIndustriesRevisitedAtlas;
        }
    }

    static UIAtlas() {
        SpriteParams[InGameButton] = new RectOffset(1, 1, 1, 1);
    }
}
