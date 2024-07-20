namespace RebalancedIndustriesRevisited;
using System;
using System.Collections.Generic;

public partial class Manager {
    private Dictionary<string, ExtractingFacilityProfile> ExtractingFacilityAICache { get; set; }
    private Dictionary<string, UniqueFactoryProfile> UniqueFactoryAICache { get; set; }
    private Dictionary<string, ProcessingFacilityProfile> ProcessingFacilityAICache { get; set; }
    private Dictionary<string, WarehouseProfile> WarehouseAICache { get; set; }

    private void InitLoader() {
        ExtractingFacilityAICache = new();
        UniqueFactoryAICache = new();
        ProcessingFacilityAICache = new();
        WarehouseAICache = new();
        RebindPrefab();
    }

    private void DeInitLoader() {
        ExtractingFacilityAICache = null;
        UniqueFactoryAICache = null;
        ProcessingFacilityAICache = null;
        WarehouseAICache = null;
    }

    private void RebindPrefab() {
        try {
            for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
                var loaded = PrefabCollection<BuildingInfo>.GetLoaded(i);
                if (loaded is not null && loaded.m_class.m_service == ItemClass.Service.PlayerIndustry && loaded.m_buildingAI is not null) {
                    var ai = loaded.m_buildingAI;
                    if (ai is ExtractingFacilityAI extractingFacilityAI) {
                        var profile = new ExtractingFacilityProfile(extractingFacilityAI);
                        profile.RebindParameter();
                        ExtractingFacilityAICache.Add(extractingFacilityAI.name, profile);
                    }
                    else if (ai is UniqueFactoryAI uniqueFactoryAI) {
                        var profile = new UniqueFactoryProfile(uniqueFactoryAI);
                        profile.RebindParameter();
                        UniqueFactoryAICache.Add(uniqueFactoryAI.name, profile);
                    }
                    else if (ai is ProcessingFacilityAI processingFacilityAI) {
                        var profile = new ProcessingFacilityProfile(processingFacilityAI);
                        profile.RebindParameter();
                        ProcessingFacilityAICache.Add(processingFacilityAI.name, profile);
                    }
                    else if (ai is WarehouseAI warehouseAI) {
                        var profile = new WarehouseProfile(warehouseAI);
                        profile.RebindParameter();
                        WarehouseAICache.Add(warehouseAI.name, profile);
                    }
                }
            }
        }
        catch (Exception e) {
            Mod.Log.Error(e, "Rebind prefab failed");
        }
    }

}
