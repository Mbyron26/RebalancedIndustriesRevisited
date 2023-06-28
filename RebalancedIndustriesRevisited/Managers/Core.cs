namespace RebalancedIndustriesRevisited;
using MbyronModsCommon;

public partial class Manager : SingletonManager<Manager> {
    public override bool IsInit { get; set; }

    public override void Init() {
        InitTooltipString();
        InitLoader();
        InitTooltipString();
        IsInit = true;
    }

    public override void DeInit() {
        DeInitTooltipString();
        DeInitLoader();
        InitTooltipString();
        IsInit = false;
    }
}
