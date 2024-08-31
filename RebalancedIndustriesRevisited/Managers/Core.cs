using CSShared.Manager;

namespace RebalancedIndustriesRevisited;

public partial class Manager : IManager {
    public bool IsInit { get; set; }

    public void OnCreated() { }

    public void Update() {
        InitTooltipString();
        InitLoader();
        InitTooltipString();
        IsInit = true;
    }

    public void Reset() {
        DeInitTooltipString();
        DeInitLoader();
        InitTooltipString();
        IsInit = false;
    }

    public void OnReleased() { }
}
