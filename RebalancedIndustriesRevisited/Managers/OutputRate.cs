using ColossalFramework.UI;

namespace RebalancedIndustriesRevisited;

public partial class Manager {
    public void RefreshOutputRate() {
        RefreshExtractingFacilityOutputRate();
        RefreshProcessingFacilityOutputRate();
    }

    public void RefreshExtractingFacilityOutputRate() {
        if (!IsInit) {
            return;
        }
        ExtractingFacilityAICache.ForEach(_ => _.Value.RebindOutputRate());
    }
    public void RefreshProcessingFacilityOutputRate() {
        if (!IsInit) {
            return;
        }
        ProcessingFacilityAICache.ForEach(_ => _.Value.RebindOutputRate());
    }
}
