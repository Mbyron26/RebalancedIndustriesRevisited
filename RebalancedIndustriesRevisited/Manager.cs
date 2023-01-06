﻿using ColossalFramework.Globalization;
using MbyronModsCommon;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RebalancedIndustriesRevisited {
    public static class Manager {
        public static ProfileBase SingletonProfileObject { get; set; }
        public static bool PrefabFlag { get; set; }
        public static void InitializePrefab() {
            if (PrefabFlag) {
                return;
            }
            PrefabFlag = true;
            ModLogger.ModLog("--------Start deploying--------");
            try {
                for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
                    if (PrefabCollection<BuildingInfo>.GetLoaded(i) != null) {
                        BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(i);
                        if (prefab.m_class.m_service == ItemClass.Service.PlayerIndustry && prefab.m_buildingAI is not null) {
                            RebindPrefab(prefab.m_buildingAI);
                        }
                    }
                }
                ModLogger.ModLog("--------Deployment succeeded--------");
                ModLogger.ModLog("Try reloading localized tooltip.");
                LocaleManager.ForceReload();
                ModLogger.ModLog("Reloaded localized tooltip succeed.");
            }
            catch (Exception e) {
                ModLogger.ModLog(e.ToString());
            }

        }

        public static void RebindPrefab(BuildingAI ai) {
            if (ai is ExtractingFacilityAI extractingFacilityAI) {
                var name = extractingFacilityAI.name;
                var rawTruckCount = extractingFacilityAI.m_outputVehicleCount;
                var rawConstructionCost = extractingFacilityAI.m_constructionCost;
                var rawMaintenanceCost = extractingFacilityAI.m_maintenanceCost;
                var rawWorkSpace0 = extractingFacilityAI.m_workPlaceCount0;
                var rawWorkSpace1 = extractingFacilityAI.m_workPlaceCount1;
                var rawWorkSpace2 = extractingFacilityAI.m_workPlaceCount2;
                var rawWorkSpace3 = extractingFacilityAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkSpace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);
                InitializeProfile(extractingFacilityAI);
                SingletonProfileObject.InitializePrefab(TypeAI.ExtractingFacilityAI, extractingFacilityAI.m_outputResource, extractingFacilityAI.m_outputVehicleCount);
                extractingFacilityAI.m_outputVehicleCount = SingletonProfileObject.GetTruck();
                extractingFacilityAI.m_constructionCost = SingletonProfileObject.GetCost(rawConstructionCost);
                extractingFacilityAI.m_maintenanceCost = SingletonProfileObject.GetCost(rawMaintenanceCost);
                var newWorkSpace = SingletonProfileObject.GetWorkSpace(rawWorkSpace);
                extractingFacilityAI.m_workPlaceCount0 = newWorkSpace.UneducatedWorkers;
                extractingFacilityAI.m_workPlaceCount1 = newWorkSpace.EducatedWorkers;
                extractingFacilityAI.m_workPlaceCount2 = newWorkSpace.WellEducatedWorkers;
                extractingFacilityAI.m_workPlaceCount3 = newWorkSpace.HighlyEducatedWorkers;
                ModLogger.ModLog($"Extracting Facility | Vehicle count: {rawTruckCount} -> {extractingFacilityAI.m_outputVehicleCount} | Construction cost: {rawConstructionCost} -> {extractingFacilityAI.m_constructionCost} | Maintenance cost: {rawMaintenanceCost} -> {extractingFacilityAI.m_maintenanceCost} | Work space: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {extractingFacilityAI.m_workPlaceCount0} {extractingFacilityAI.m_workPlaceCount1} {extractingFacilityAI.m_workPlaceCount2} {extractingFacilityAI.m_workPlaceCount3} | Building: {name}");
            } else if (ai is UniqueFactoryAI uniqueFactoryAI) {
                var name = uniqueFactoryAI.name;
                var rawMaintenanceCost = uniqueFactoryAI.m_maintenanceCost;
                var rawWorkSpace0 = uniqueFactoryAI.m_workPlaceCount0;
                var rawWorkSpace1 = uniqueFactoryAI.m_workPlaceCount1;
                var rawWorkSpace2 = uniqueFactoryAI.m_workPlaceCount2;
                var rawWorkSpace3 = uniqueFactoryAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkSpace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);
                var rawInputRate1 = uniqueFactoryAI.m_inputRate1;
                var rawInputRate2 = uniqueFactoryAI.m_inputRate2;
                var rawInputRate3 = uniqueFactoryAI.m_inputRate3;
                var rawInputRate4 = uniqueFactoryAI.m_inputRate4;
                var rawOutputRate = uniqueFactoryAI.m_outputRate;
                InitializeProfile(uniqueFactoryAI);
                SingletonProfileObject.InitializePrefab(TypeAI.UniqueFactoryAI, uniqueFactoryAI.m_outputResource, uniqueFactoryAI.m_outputVehicleCount);
                if (SingletonProfileObject.Flag == TypeProfile.Constant) {
                    var newWorkSpace = SingletonProfileObject.GetWorkSpace(rawWorkSpace);
                    uniqueFactoryAI.m_maintenanceCost = SingletonProfileObject.GetCost(rawMaintenanceCost);
                    uniqueFactoryAI.m_workPlaceCount0 = newWorkSpace.UneducatedWorkers;
                    uniqueFactoryAI.m_workPlaceCount1 = newWorkSpace.EducatedWorkers;
                    uniqueFactoryAI.m_workPlaceCount2 = newWorkSpace.WellEducatedWorkers;
                    uniqueFactoryAI.m_workPlaceCount3 = newWorkSpace.HighlyEducatedWorkers;
                    SingletonProfileObject.SetProcessingFacilityBufferSize(ref uniqueFactoryAI.m_inputRate1, ref uniqueFactoryAI.m_inputRate2, ref uniqueFactoryAI.m_inputRate3, ref uniqueFactoryAI.m_inputRate4);
                    SingletonProfileObject.SetProcessingFacilityBufferSize(ref uniqueFactoryAI.m_outputRate);
                    ModLogger.ModLog($"Unique Factory | Maintenance cost: {rawMaintenanceCost} -> {uniqueFactoryAI.m_maintenanceCost} | Work space: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {uniqueFactoryAI.m_workPlaceCount0} {uniqueFactoryAI.m_workPlaceCount1} {uniqueFactoryAI.m_workPlaceCount2} {uniqueFactoryAI.m_workPlaceCount3} | InputRate: {rawInputRate1} {rawInputRate2} {rawInputRate3} {rawInputRate4} -> {uniqueFactoryAI.m_inputRate1} {uniqueFactoryAI.m_inputRate2} {uniqueFactoryAI.m_inputRate3} {uniqueFactoryAI.m_inputRate4} | OutputRate: {rawOutputRate} -> {uniqueFactoryAI.m_outputRate} | Building: {name}");
                } else {
                    ModLogger.ModLog($"Unique Factory | No rebinding. | Building: {uniqueFactoryAI.name}");
                }

            } else if (ai is ProcessingFacilityAI processingFacilityAI) {
                var name = processingFacilityAI.name;
                var rawTruckCount = processingFacilityAI.m_outputVehicleCount;
                var rawConstructionCost = processingFacilityAI.m_constructionCost;
                var rawMaintenanceCost = processingFacilityAI.m_maintenanceCost;
                var rawWorkSpace0 = processingFacilityAI.m_workPlaceCount0;
                var rawWorkSpace1 = processingFacilityAI.m_workPlaceCount1;
                var rawWorkSpace2 = processingFacilityAI.m_workPlaceCount2;
                var rawWorkSpace3 = processingFacilityAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkSpace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);
                var rawInputRate1 = processingFacilityAI.m_inputRate1;
                var rawInputRate2 = processingFacilityAI.m_inputRate2;
                var rawInputRate3 = processingFacilityAI.m_inputRate3;
                var rawInputRate4 = processingFacilityAI.m_inputRate4;
                var rawOnputRate = processingFacilityAI.m_outputRate;
                InitializeProfile(processingFacilityAI);
                SingletonProfileObject.InitializePrefab(TypeAI.ProcessingFacilityAI, processingFacilityAI.m_outputResource, processingFacilityAI.m_outputVehicleCount);
                processingFacilityAI.m_outputVehicleCount = SingletonProfileObject.GetTruck();
                var newWorkSpace = SingletonProfileObject.GetWorkSpace(rawWorkSpace);
                processingFacilityAI.m_constructionCost = SingletonProfileObject.GetCost(rawConstructionCost);
                processingFacilityAI.m_maintenanceCost = SingletonProfileObject.GetCost(rawMaintenanceCost);
                processingFacilityAI.m_workPlaceCount0 = newWorkSpace.UneducatedWorkers;
                processingFacilityAI.m_workPlaceCount1 = newWorkSpace.EducatedWorkers;
                processingFacilityAI.m_workPlaceCount2 = newWorkSpace.WellEducatedWorkers;
                processingFacilityAI.m_workPlaceCount3 = newWorkSpace.HighlyEducatedWorkers;
                SingletonProfileObject.SetProcessingFacilityBufferSize(ref processingFacilityAI.m_inputRate1, ref processingFacilityAI.m_inputRate2, ref processingFacilityAI.m_inputRate3, ref processingFacilityAI.m_inputRate4);
                SingletonProfileObject.SetProcessingFacilityBufferSize(ref processingFacilityAI.m_outputRate);
                ModLogger.ModLog($"Processing Facility | Vehicle count: {rawTruckCount} -> {processingFacilityAI.m_outputVehicleCount} | Construction cost: {rawConstructionCost} -> {processingFacilityAI.m_constructionCost} | Maintenance cost: {rawMaintenanceCost} -> {processingFacilityAI.m_maintenanceCost} | Work space: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {processingFacilityAI.m_workPlaceCount0} {processingFacilityAI.m_workPlaceCount1} {processingFacilityAI.m_workPlaceCount2} {processingFacilityAI.m_workPlaceCount3} | InputRate: {rawInputRate1} {rawInputRate2} {rawInputRate3} {rawInputRate4} -> {processingFacilityAI.m_inputRate1} {processingFacilityAI.m_inputRate2} {processingFacilityAI.m_inputRate3} {processingFacilityAI.m_inputRate4} | OutputRate: {rawOnputRate} -> {processingFacilityAI.m_outputRate} | Building: {name}");
            } else if (ai is WarehouseAI warehouseAI) {
                var name = warehouseAI.name;
                var rawTruckCount = warehouseAI.m_truckCount;
                var rawConstructionCost = warehouseAI.m_constructionCost;
                var rawMaintenanceCost = warehouseAI.m_maintenanceCost;
                var rawWorkSpace0 = warehouseAI.m_workPlaceCount0;
                var rawWorkSpace1 = warehouseAI.m_workPlaceCount1;
                var rawWorkSpace2 = warehouseAI.m_workPlaceCount2;
                var rawWorkSpace3 = warehouseAI.m_workPlaceCount3;
                var rawWorkSpace = new WorkSpace(rawWorkSpace0, rawWorkSpace1, rawWorkSpace2, rawWorkSpace3);
                InitializeProfile(warehouseAI);
                SingletonProfileObject.InitializePrefab(TypeAI.WarehouseAI, warehouseAI.m_storageType, rawTruckCount);
                SingletonProfileObject.WarehouseCapacity = warehouseAI.m_storageCapacity;
                warehouseAI.m_truckCount = SingletonProfileObject.GetTruck();
                var newWorkSpace = SingletonProfileObject.GetWorkSpace(rawWorkSpace);
                warehouseAI.m_constructionCost = SingletonProfileObject.GetCost(rawConstructionCost);
                warehouseAI.m_maintenanceCost = SingletonProfileObject.GetCost(rawMaintenanceCost);
                warehouseAI.m_workPlaceCount0 = newWorkSpace.UneducatedWorkers;
                warehouseAI.m_workPlaceCount1 = newWorkSpace.EducatedWorkers;
                warehouseAI.m_workPlaceCount2 = newWorkSpace.WellEducatedWorkers;
                warehouseAI.m_workPlaceCount3 = newWorkSpace.HighlyEducatedWorkers;
                ModLogger.ModLog($"Warehouse | Vehicle count: {rawTruckCount} -> {warehouseAI.m_truckCount} | Construction cost: {rawConstructionCost} -> {warehouseAI.m_constructionCost} | Maintenance cost: {rawMaintenanceCost} -> {warehouseAI.m_maintenanceCost} | Work space0: {rawWorkSpace0} {rawWorkSpace1} {rawWorkSpace2} {rawWorkSpace3} -> {warehouseAI.m_workPlaceCount0} {warehouseAI.m_workPlaceCount1} {warehouseAI.m_workPlaceCount2} {warehouseAI.m_workPlaceCount3} | Building: {name}");
            }
        }

        public static void InitializeProfile(BuildingAI ai) {
            if (SingletonProfileObject is not null) {
                SingletonProfileObject = null;
            }
            if (ai is ExtractingFacilityAI) {
                if (IsField(ai)) {
                    SingletonProfileObject = new RatioProfile(0.25m, 1m, new WorkSpace(2, 4, 1, 0));
                } else {
                    SingletonProfileObject = new ProfileBase(1m, 0.5m);
                }
            } else if (ai is UniqueFactoryAI) {
                SingletonProfileObject = ai.name switch {
                    "Furniture Factory 01" => new ConstantValueProfile(320, new WorkSpace(25, 18, 8, 4)),
                    "Bakery 01" => new ConstantValueProfile(260, new WorkSpace(15, 9, 4, 2)),
                    "Industrial Steel Plant 01" => new ConstantValueProfile(1800, new WorkSpace(60, 45, 30, 5)),
                    "Household Plastic Factory 01" => new ConstantValueProfile(480, new WorkSpace(25, 18, 8, 4)),
                    "Toy Factory 01" => new ConstantValueProfile(760, new WorkSpace(25, 18, 8, 4)),
                    "Printing Press 01" => new ConstantValueProfile(560, new WorkSpace(22, 16, 8, 4)),
                    "Lemonade Factory 01" => new ConstantValueProfile(800, new WorkSpace(55, 35, 15, 5)),
                    "Electronics Factory 01" => new ConstantValueProfile(1800, new WorkSpace(55, 40, 20, 10)),
                    "Clothing Factory 01" => new ConstantValueProfile(840, new WorkSpace(35, 20, 10, 5)),
                    "Petroleum Refinery 01" => new ConstantValueProfile(2600, new WorkSpace(60, 45, 30, 15)),
                    "Soft Paper Factory 01" => new ConstantValueProfile(2200, new WorkSpace(60, 50, 12, 8)),
                    "Car Factory 01" => new ConstantValueProfile(3400, new WorkSpace(70, 60, 20, 10)),
                    "Sneaker Factory 01" => new ConstantValueProfile(1920, new WorkSpace(35, 30, 10, 5)),
                    "Modular House Factory 01" => new ConstantValueProfile(2400, new WorkSpace(70, 45, 15, 10)),
                    "Food Factory 01" => new ConstantValueProfile(1920, new WorkSpace(55, 35, 15, 5)),
                    "Dry Dock 01" => new ConstantValueProfile(3800, new WorkSpace(80, 50, 20, 10)),
                    _ => new ProfileBase(),
                };
            } else if (ai is ProcessingFacilityAI) {
                if (IsField(ai)) {
                    SingletonProfileObject = new RatioProfile(0.5m, 0.35m, new WorkSpace(2, 4, 1, 0));
                } else {
                    SingletonProfileObject = new ProfileBase(1m, 1m);
                }
            } else if (ai is WarehouseAI warehouseAI) {
                SingletonProfileObject = warehouseAI.m_storageType switch {
                    TransferManager.TransferReason.Grain => new RatioProfile(0.5m, 0.25m, new WorkSpace(8, 4, 2, 1)),
                    TransferManager.TransferReason.Logs or
                    TransferManager.TransferReason.Ore or
                    TransferManager.TransferReason.Oil => new RatioProfile(1m, 0.5m, new WorkSpace(8, 4, 2, 1)),
                    _ => new RatioProfile(2m, 1m, new WorkSpace(8, 4, 2, 1)),
                };
            } else SingletonProfileObject = new ProfileBase(1m, 1m);
        }


        public static string[] ProcessorField = { "Animal Pasture 01", "Animal Pasture 02", "Cattle Shed 01" };
        public static bool IsField(BuildingAI buildingAI) {
            if (buildingAI is ExtractingFacilityAI extractingFacilityAI) {
                if (extractingFacilityAI.m_outputResource == TransferManager.TransferReason.Grain) {
                    return true;
                }
            }
            if (buildingAI is ProcessingFacilityAI processingFacilityAI) {
                foreach (var item in ProcessorField) {
                    if (processingFacilityAI.name == item) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void SetCargoCapacity(Vehicle data, CargoTruckAI cargoTruckAI, int capacity) {
            var type = (TransferManager.TransferReason)data.m_transferType;
            var categories = GetIndustriesType(type) switch {
                IndustriesType.Forestry => true,
                IndustriesType.Farming => true,
                IndustriesType.Ore => true,
                IndustriesType.Oil => true,
                _ => false
            };
            if (categories) {
                SetCargoCapacity(cargoTruckAI, capacity);
            }
        }
        public static void SetCargoCapacity(CargoTruckAI cargoTruckAI, int capacity) => cargoTruckAI.m_cargoCapacity = capacity;
        public static IndustriesType GetIndustriesType(TransferManager.TransferReason transferReason) =>
            transferReason switch {
                TransferManager.TransferReason.Logs => IndustriesType.Forestry,
                TransferManager.TransferReason.Paper => IndustriesType.Forestry,
                TransferManager.TransferReason.PlanedTimber => IndustriesType.Forestry,
                TransferManager.TransferReason.Grain => IndustriesType.Farming,
                TransferManager.TransferReason.Flours => IndustriesType.Farming,
                TransferManager.TransferReason.AnimalProducts => IndustriesType.Farming,
                TransferManager.TransferReason.Ore => IndustriesType.Ore,
                TransferManager.TransferReason.Metals => IndustriesType.Ore,
                TransferManager.TransferReason.Glass => IndustriesType.Ore,
                TransferManager.TransferReason.Oil => IndustriesType.Oil,
                TransferManager.TransferReason.Plastics => IndustriesType.Oil,
                TransferManager.TransferReason.Petroleum => IndustriesType.Oil,
                _ => IndustriesType.None,
            };

    }

    public enum IndustriesType {
        None,
        Forestry,
        Farming,
        Ore,
        Oil,
    }

    public class ConstantValueProfile : ProfileBase {
        public ConstantValueProfile(decimal costsFactor, WorkSpace newWorkSpaceBuffer) {
            CostsFactor = costsFactor;
            NewWorkSpaceBuffer = newWorkSpaceBuffer;
            Flag = TypeProfile.Constant;
        }
        public override int GetCost(int rawValue) => rawValue * 100 / 16;

        public override WorkSpace GetWorkSpace(WorkSpace rawWorkSpaceBuffer) {
            RawWorkSpaceBuffer = rawWorkSpaceBuffer;
            RawAllWorkers = RawWorkSpaceBuffer.Sum();
            if (Flag != TypeProfile.Constant) {
                ModLogger.ModLog("Flag error.");
                NewWorkSpaceBuffer = RawWorkSpaceBuffer;
                return NewWorkSpaceBuffer;
            }
            return NewWorkSpaceBuffer;
        }
    }


    public class RatioProfile : ProfileBase {
        public RatioProfile(decimal costsFactor, decimal workersFactor, WorkSpace workSpaceRatio) : base(costsFactor, workersFactor) {
            WorkSpaceRatio = workSpaceRatio;
            Flag = TypeProfile.Ratio;
        }
        public override WorkSpace GetWorkSpace(WorkSpace rawWorkSpaceBuffer) {
            RawWorkSpaceBuffer = rawWorkSpaceBuffer;
            RawAllWorkers = RawWorkSpaceBuffer.Sum();
            if (Flag != TypeProfile.Ratio) {
                ModLogger.ModLog("Flag error.");
                NewWorkSpaceBuffer = RawWorkSpaceBuffer;
                return NewWorkSpaceBuffer;
            }
            List<int> newWorkSpace = new();
            int newAllWorkers;
            if (WorkersFactor == 1) {
                newAllWorkers = RawAllWorkers;
            } else {
                newAllWorkers = (int)Math.Ceiling(RawAllWorkers * WorkersFactor);
            }
            foreach (int item in RawWorkSpaceBuffer) {
                newWorkSpace.Add((int)(GetRatio(item, WorkSpaceRatio.Sum()) * newAllWorkers));
            }
            NewWorkSpaceBuffer = new WorkSpace(newWorkSpace[0], newWorkSpace[1], newWorkSpace[2], newWorkSpace[3]);
            return NewWorkSpaceBuffer;
        }
    }

    public class ProfileBase : ITruck {
        public const decimal MinWarehouseTruckCountFactor = 0.5m;
        public const int MinWarehouseCapacity = 40000;
        public TransferManager.TransferReason OutputResource { get; set; }
        public int RawTruckCount { get; set; }
        public int NewTruckCount { get; set; }

        public TypeAI TypeAI { get; set; }
        public decimal CostsFactor { get; set; }
        public decimal WorkersFactor { get; set; }

        public WorkSpace RawWorkSpaceBuffer { get; protected set; }
        public WorkSpace WorkSpaceRatio { get; set; }
        public WorkSpace NewWorkSpaceBuffer { get; protected set; }

        public int RawAllWorkers { get; protected set; }
        public TypeProfile Flag { get; set; }

        public int WarehouseCapacity { get; set; }

        public ProfileBase() {
            Flag = TypeProfile.None;
        }
        public ProfileBase(decimal costsFactor, decimal workersFactor) {
            CostsFactor = costsFactor;
            WorkersFactor = workersFactor;
            Flag = TypeProfile.Base;
        }

        public virtual void InitializePrefab(TypeAI typeAI, TransferManager.TransferReason outputResource, int rawTruckCount) {
            TypeAI = typeAI;
            OutputResource = outputResource;
            RawTruckCount = rawTruckCount;
        }
        public virtual void SetProcessingFacilityBufferSize(ref int outRate) {
            var raw = GetBufferSize(outRate);
            var modified = EMath.Clamp(raw * 2, 20000, 60000);
            outRate = SetBufferSize(modified);
        }
        public virtual void SetProcessingFacilityBufferSize(ref int rate1, ref int rate2, ref int rate3, ref int rate4) {
            if (rate1 != 0) {
                var raw = GetBufferSize(rate1);
                var modified = EMath.Clamp(raw * 2, 20000, 60000);
                rate1 = SetBufferSize(modified);
            }
            if (rate2 != 0) {
                var raw = GetBufferSize(rate2);
                var modified = EMath.Clamp(raw * 2, 20000, 60000);
                rate2 = SetBufferSize(modified);
            }
            if (rate3 != 0) {
                var raw = GetBufferSize(rate3);
                var modified = EMath.Clamp(raw * 2, 20000, 60000);
                rate3 = SetBufferSize(modified);
            }
            if (rate4 != 0) {
                var raw = GetBufferSize(rate4);
                var modified = EMath.Clamp(raw * 2, 20000, 60000);
                rate4 = SetBufferSize(modified);
            }
        }
        private static int GetBufferSize(int rate) => ((rate * 32 + 8000) * 100 + 50) / 100 * 1000 / 1000;
        private static int SetBufferSize(int newBuffer) => ((newBuffer * 100 - 50) / 100 - 8000) / 32;

        public virtual int GetCost(int rawValue) => (int)Math.Ceiling(rawValue * CostsFactor);
        public virtual WorkSpace GetWorkSpace(WorkSpace rawWorkSpaceBuffer) {
            RawWorkSpaceBuffer = rawWorkSpaceBuffer;
            RawAllWorkers = RawWorkSpaceBuffer.Sum();
            if (Flag != TypeProfile.Base || Flag == TypeProfile.None) {
                ModLogger.ModLog("Flag error.");
                NewWorkSpaceBuffer = RawWorkSpaceBuffer;
                return NewWorkSpaceBuffer;
            }
            List<int> newWorkSpace = new();
            foreach (int item in RawWorkSpaceBuffer) {
                newWorkSpace.Add((int)Math.Ceiling(item * WorkersFactor));
            }
            NewWorkSpaceBuffer = new WorkSpace(newWorkSpace[0], newWorkSpace[1], newWorkSpace[2], newWorkSpace[3]);
            return NewWorkSpaceBuffer;
        }
        protected decimal GetRatio(decimal numerator, int denominator) => Math.Round(numerator / denominator, 2);

        public virtual int GetTruck() {
            if (RawTruckCount == 0) {
                NewTruckCount = default;
                return default;
            }
            var factor = GetTruckFactor(OutputResource);
            //if (factor == 0) {
            //    NewTruckCount = RawTruckCount;
            //    return NewTruckCount;
            //}
            if (TypeAI == TypeAI.WarehouseAI) {
                if (WarehouseCapacity == default) {
                    NewTruckCount = default;
                    return default;
                }
                if (IsRawWarehouse(OutputResource)) {
                    factor = (factor - 1) / 1.5m + 1;
                    NewTruckCount = (int)Math.Ceiling(RawTruckCount / factor);
                    if (WarehouseCapacity <= 40000) {
                        NewTruckCount = EMath.Max(1, NewTruckCount);
                    } else {
                        NewTruckCount = EMath.Max(2, NewTruckCount);
                    }
                    return NewTruckCount;
                } else {
                    NewTruckCount = (int)Math.Ceiling(RawTruckCount * MinWarehouseTruckCountFactor);
                    if (WarehouseCapacity <= MinWarehouseCapacity) {
                        NewTruckCount = EMath.Max(1, NewTruckCount);
                    } else {
                        NewTruckCount = EMath.Max(2, NewTruckCount);
                    }
                    return NewTruckCount;
                }
            } else {
                NewTruckCount = EMath.Clamp((int)Math.Ceiling(RawTruckCount / factor), 1, RawTruckCount);
                return NewTruckCount;
            }
        }

        protected static decimal GetTruckFactor(TransferManager.TransferReason material) =>
           material switch {
               TransferManager.TransferReason.Grain => 3m,
               TransferManager.TransferReason.AnimalProducts => 1.5m,
               TransferManager.TransferReason.Flours => 1.5m,
               TransferManager.TransferReason.Logs => 2m,
               TransferManager.TransferReason.PlanedTimber => 2m,
               TransferManager.TransferReason.Paper => 2m,
               TransferManager.TransferReason.Ore => 1.5m,
               TransferManager.TransferReason.Metals => 1.5m,
               TransferManager.TransferReason.Glass => 1.5m,
               TransferManager.TransferReason.Oil => 3m,
               TransferManager.TransferReason.Plastics => 1.5m,
               TransferManager.TransferReason.Petroleum => 2m,
               _ => 0m
           };
        protected static bool IsRawWarehouse(TransferManager.TransferReason material) => material switch {
            TransferManager.TransferReason.Grain or
            TransferManager.TransferReason.Logs or
            TransferManager.TransferReason.Ore or
            TransferManager.TransferReason.Oil => true,
            _ => false,
        };

    }

    public interface ITruck {
        TransferManager.TransferReason OutputResource { get; set; }
        int RawTruckCount { get; set; }
        int NewTruckCount { get; set; }
        int GetTruck();
    }

    public enum TypeProfile {
        Base,
        Ratio,
        Constant,
        None
    }

    public enum TypeAI {
        ExtractingFacilityAI,
        ProcessingFacilityAI,
        UniqueFactoryAI,
        WarehouseAI,
        None
    }

    public readonly struct WorkSpace : IEnumerable {
        public readonly int UneducatedWorkers;
        public readonly int EducatedWorkers;
        public readonly int WellEducatedWorkers;
        public readonly int HighlyEducatedWorkers;

        public WorkSpace(int uneducatedWorkers, int educatedWorkers, int wellEducatedWorkers, int highlyEducatedWorkers) {
            UneducatedWorkers = uneducatedWorkers;
            EducatedWorkers = educatedWorkers;
            WellEducatedWorkers = wellEducatedWorkers;
            HighlyEducatedWorkers = highlyEducatedWorkers;
        }

        public IEnumerator GetEnumerator() {
            yield return UneducatedWorkers;
            yield return EducatedWorkers;
            yield return WellEducatedWorkers;
            yield return HighlyEducatedWorkers;
        }

        public int Sum() => UneducatedWorkers + EducatedWorkers + WellEducatedWorkers + HighlyEducatedWorkers;
    }
}