using MbyronModsCommon;

namespace RebalancedIndustriesRevisited {
    public class Config : ModConfigBase<Config> {
        public bool OriginalValue { get; set; } = true;
        public bool BothValue { get; set; } = false;

        public float ExtractingFacilityProductionRate { get; set; } = 0.5f;
        public float ProcessingFacilityProductionRate { get; set; } = 0.5f;
    }
}
