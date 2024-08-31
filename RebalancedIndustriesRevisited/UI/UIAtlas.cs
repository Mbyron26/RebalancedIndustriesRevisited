using ColossalFramework.UI;
using CSShared.Debug;
using CSShared.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace RebalancedIndustriesRevisited.UI;

internal static class UIAtlas {
    private static UITextureAtlas rebalancedIndustriesRevisitedAtlas;

    public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();
    public static string InGameButton => nameof(InGameButton);
    public static UITextureAtlas RebalancedIndustriesRevisitedAtlas {
        get {
            if (rebalancedIndustriesRevisitedAtlas is null) {
                rebalancedIndustriesRevisitedAtlas = CSShared.UI.UIUtils.CreateTextureAtlas(nameof(RebalancedIndustriesRevisitedAtlas), $"{AssemblyTools.CurrentAssemblyName}.UI.Resources.", SpriteParams);
                LogManager.GetLogger().Info("Initialized RebalancedIndustriesRevisitedAtlas");
            }
            return rebalancedIndustriesRevisitedAtlas;
        }
    }

    static UIAtlas() {
        SpriteParams[InGameButton] = new RectOffset(1, 1, 1, 1);
    }
}
