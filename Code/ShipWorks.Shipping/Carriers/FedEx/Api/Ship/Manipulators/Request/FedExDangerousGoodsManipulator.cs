using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.FedEx;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the DangerousGoodsDetail
    /// attributes within the FedEx API's IFedExNativeShipmentRequest object if the shipment
    /// has dangerous goods enabled.
    /// </summary>
    public class FedExDangerousGoodsManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        private readonly IDictionary<FedExBatteryMaterialType, BatteryMaterialType> batteryMaterialLookup =
            new Dictionary<FedExBatteryMaterialType, BatteryMaterialType>
            {
                { FedExBatteryMaterialType.LithiumIon, BatteryMaterialType.LITHIUM_ION },
                { FedExBatteryMaterialType.LithiumMetal, BatteryMaterialType.LITHIUM_METAL }
            };

        private readonly IDictionary<FedExBatteryPackingType, BatteryPackingType> batteryPackingLookup =
            new Dictionary<FedExBatteryPackingType, BatteryPackingType>
            {
                { FedExBatteryPackingType.ContainsInEquipement, BatteryPackingType.CONTAINED_IN_EQUIPMENT },
                { FedExBatteryPackingType.PackedWithEquipment, BatteryPackingType.PACKED_WITH_EQUIPMENT }
            };

        private readonly IDictionary<FedExBatteryRegulatorySubType, BatteryRegulatorySubType> batteryRegulatorySubtypeLookup =
            new Dictionary<FedExBatteryRegulatorySubType, BatteryRegulatorySubType>
            {
                { FedExBatteryRegulatorySubType.IATASectionII, BatteryRegulatorySubType.IATA_SECTION_II }
            };

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExDangerousGoodsManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            shipment.FedEx.Packages.ElementAt(sequenceNumber).DangerousGoodsEnabled;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            IFedExPackageEntity package = shipment.FedEx.Packages.ElementAt(sequenceNumber);

            return ValidateRequest(shipment, request)
                .Map(x => BuildDangerousGoods(shipment, x, package))
                .Bind(x => ApplyOptions(x, package))
                .Bind(x => ApplyHazardousMaterials(x, package))
                .Map(x => request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail = x)
                .Map(_ => ApplyBatteryDetails(request, package));
        }

        /// <summary>
        /// Apply the battery details
        /// </summary>
        private ProcessShipmentRequest ApplyBatteryDetails(ProcessShipmentRequest request, IFedExPackageEntity package)
        {
            if (package.DangerousGoodsType == (int) FedExDangerousGoodsMaterialType.Batteries)
            {
                request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails =
                    ConfigureBatteryMaterials(package);
            }

            return request;
        }

        /// <summary>
        /// Apply hazardous materials
        /// </summary>
        private static GenericResult<DangerousGoodsDetail> ApplyHazardousMaterials(DangerousGoodsDetail dangerousGoods, IFedExPackageEntity package)
        {
            if (package.DangerousGoodsType == (int) FedExDangerousGoodsMaterialType.HazardousMaterials)
            {
                return ConfigureHazardousMaterials(dangerousGoods, package);
            }

            // Accessibility options do not apply to hazardous materials
            if (package.DangerousGoodsAccessibilityType == (int) FedExDangerousGoodsAccessibilityType.NotApplicable)
            {
                return dangerousGoods;
            }

            return GetApiDangerousGoodsAccessibilityType(package)
                .Do(x =>
                {
                    dangerousGoods.Accessibility = x;
                    dangerousGoods.AccessibilitySpecified = true;
                })
                .Map(x => dangerousGoods);
        }

        /// <summary>
        /// Apply options
        /// </summary>
        private static GenericResult<DangerousGoodsDetail> ApplyOptions(DangerousGoodsDetail dangerousGoods, IFedExPackageEntity package)
        {
            if (package.DangerousGoodsType == (int) FedExDangerousGoodsMaterialType.NotApplicable)
            {
                return dangerousGoods;
            }

            return GetApiHazardousCommodityType(package)
                .Do(x =>
                {
                    dangerousGoods.Options = new HazardousCommodityOptionType[] { x };
                })
                .Map(x => dangerousGoods);
        }

        /// <summary>
        /// Build the dangerous goods element
        /// </summary>
        private DangerousGoodsDetail BuildDangerousGoods(IShipmentEntity shipment, ProcessShipmentRequest request, IFedExPackageEntity package)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            ConfigureShippingDocuments(request, shipment.FedEx.Packages.Count());

            // Add the service option to the request
            ConfigureDangerousGoodsOption(request, package);

            DangerousGoodsDetail dangerousGoods = new DangerousGoodsDetail();

            dangerousGoods.CargoAircraftOnly = package.DangerousGoodsCargoAircraftOnly;
            dangerousGoods.CargoAircraftOnlySpecified = true;

            dangerousGoods.EmergencyContactNumber = package.DangerousGoodsEmergencyContactPhone;
            dangerousGoods.Offeror = package.DangerousGoodsOfferor;

            SetupSignatory(package, dangerousGoods);
            return dangerousGoods;
        }

        /// <summary>
        /// Validate the requests
        /// </summary>
        private GenericResult<ProcessShipmentRequest> ValidateRequest(IShipmentEntity shipment, ProcessShipmentRequest request)
        {
            if (shipment.FedEx.RequestedLabelFormat != (int) ThermalLanguage.None &&
                shipment.FedEx.Packages.Any(package => package.DangerousGoodsEnabled))
            {
                return new FedExException("Cannot create thermal dangerous goods label.");
            }

            return request;
        }

        /// <summary>
        /// Setups the signatory.
        /// </summary>
        private static void SetupSignatory(IFedExPackageEntity package, DangerousGoodsDetail dangerousGoods)
        {
            if (!string.IsNullOrEmpty(package.SignatoryContactName) || !string.IsNullOrEmpty(package.SignatoryPlace) ||
                !string.IsNullOrEmpty(package.SignatoryTitle))
            {
                dangerousGoods.Signatory = new DangerousGoodsSignatory();
            }
            if (!string.IsNullOrEmpty(package.SignatoryContactName))
            {
                dangerousGoods.Signatory.ContactName = package.SignatoryContactName;
            }
            if (!string.IsNullOrEmpty(package.SignatoryTitle))
            {
                dangerousGoods.Signatory.Title = package.SignatoryTitle;
            }
            if (!string.IsNullOrEmpty(package.SignatoryPlace))
            {
                dangerousGoods.Signatory.Place = package.SignatoryPlace;
            }
        }

        /// <summary>
        /// Configures the shipping documents for OP_900
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="labelFormat">The format to print shipping documents</param>
        private static void ConfigureShippingDocuments(IFedExNativeShipmentRequest nativeRequest, int packageCount)
        {
            var documentTypes = nativeRequest.RequestedShipment
                .Ensure(x => x.ShippingDocumentSpecification)
                .Ensure(x => x.ShippingDocumentTypes)
                .AsEnumerable();

            if (packageCount == 1)
            {
                documentTypes = documentTypes.Append(RequestedShippingDocumentType.OP_900);
                nativeRequest.RequestedShipment.ShippingDocumentSpecification.Op900Detail = new Op900Detail
                {
                    Format = new ShippingDocumentFormat
                    {
                        ImageType = ShippingDocumentImageType.PDF,
                        ImageTypeSpecified = true,
                        StockType = ShippingDocumentStockType.OP_900_LL_B,
                        StockTypeSpecified = true
                    }
                };
            }

            documentTypes = documentTypes.Append(RequestedShippingDocumentType.DANGEROUS_GOODS_SHIPPERS_DECLARATION);
            nativeRequest.RequestedShipment.ShippingDocumentSpecification.DangerousGoodsShippersDeclarationDetail = new DangerousGoodsShippersDeclarationDetail
            {
                Format = new ShippingDocumentFormat
                {
                    ImageType = ShippingDocumentImageType.PDF,
                    ImageTypeSpecified = true,
                    StockType = ShippingDocumentStockType.PAPER_4X6,
                    StockTypeSpecified = true
                }
            };

            nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = documentTypes.ToArray();
        }

        /// <summary>
        /// Configures the hazardous materials of the DangerousGoodsDetail object..
        /// </summary>
        /// <param name="dangerousGoods">The dangerous goods.</param>
        /// <param name="package">The package.</param>
        private static GenericResult<DangerousGoodsDetail> ConfigureHazardousMaterials(DangerousGoodsDetail dangerousGoods, IFedExPackageEntity package)
        {
            // We  need to supply a description of the hazardous commodity when shipment contains hazardous materials
            dangerousGoods.Containers = new[]
            {
                new DangerousGoodsContainer
                {
                    ContainerType = package.ContainerType,
                    NumberOfContainers = package.NumberOfContainers.ToString(),
                    HazardousCommodities = new[]
                    {
                        new HazardousCommodityContent
                        {
                            Description = new HazardousCommodityDescription
                            {
                                Id = package.HazardousMaterialNumber,
                                HazardClass = package.HazardousMaterialClass,
                                PackingDetails = new HazardousCommodityPackingDetail
                                {
                                    CargoAircraftOnlySpecified = true,
                                    CargoAircraftOnly = package.PackingDetailsCargoAircraftOnly,
                                    PackingInstructions = package.PackingDetailsPackingInstructions
                                },
                                ProperShippingName = package.HazardousMaterialProperName,
                                TechnicalName = package.HazardousMaterialTechnicalName,
                            },
                            Quantity = new HazardousCommodityQuantityDetail
                            {
                                Amount = (decimal) package.HazardousMaterialQuantityValue,
                                AmountSpecified = true,
                                Units = EnumHelper.GetDescription((FedExHazardousMaterialsQuantityUnits) package.HazardousMaterialQuanityUnits)
                            }
                        }
                    }
                }
            };

            return SpecifyPackingGroup(package, dangerousGoods)
                .Do(x =>
                {
                    x.Packaging = new HazardousCommodityPackagingDetail
                    {
                        Count = package.DangerousGoodsPackagingCount.ToString(),
                        Units = EnumHelper.GetDescription((FedExHazardousMaterialsQuantityUnits) package.HazardousMaterialQuanityUnits)
                    };
                });
        }

        /// <summary>
        /// Specify packing group
        /// </summary>
        private static GenericResult<DangerousGoodsDetail> SpecifyPackingGroup(IFedExPackageEntity package, DangerousGoodsDetail dangerousGoods)
        {
            if (package.HazardousMaterialPackingGroup == (int) FedExHazardousMaterialsPackingGroup.NotApplicable)
            {
                return dangerousGoods;
            }

            return GetApiPackingGroup(package)
                .Map(x =>
                {
                    dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup = x;
                    dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroupSpecified = true;
                    return dangerousGoods;
                });
        }

        /// <summary>
        /// Configure battery materials
        /// </summary>
        private BatteryClassificationDetail[] ConfigureBatteryMaterials(IFedExPackageEntity package)
        {
            var details = new BatteryClassificationDetail();

            details.MaterialSpecified = SetBatteryDetail(package.BatteryMaterial, batteryMaterialLookup, x => details.Material = x);
            details.PackingSpecified = SetBatteryDetail(package.BatteryPacking, batteryPackingLookup, x => details.Packing = x);
            details.RegulatorySubTypeSpecified = SetBatteryDetail(package.BatteryRegulatorySubtype, batteryRegulatorySubtypeLookup, x => details.RegulatorySubType = x);

            return new[] { details };
        }

        /// <summary>
        /// Set a specific battery detail
        /// </summary>
        private bool SetBatteryDetail<TKey, TValue>(TKey value, IDictionary<TKey, TValue> lookup, Action<TValue> setProperty)
        {
            if (lookup.ContainsKey(value))
            {
                setProperty(lookup[value]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds the dangerous goods option to the request.
        /// </summary>
        private void ConfigureDangerousGoodsOption(IFedExNativeShipmentRequest nativeRequest, IFedExPackageEntity package)
        {
            var servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0]
                .Ensure(x => x.SpecialServicesRequested);
            servicesRequested.SpecialServiceTypes = servicesRequested.Ensure(x => x.SpecialServiceTypes)
                .Append(PackageSpecialServiceType.DANGEROUS_GOODS)
                .ToArray();

            if (package.DangerousGoodsType == (int) FedExDangerousGoodsMaterialType.Batteries)
            {
                servicesRequested.SpecialServiceTypes = servicesRequested.Ensure(x => x.SpecialServiceTypes)
                    .Append(PackageSpecialServiceType.BATTERY)
                    .ToArray();
            }
        }

        /// <summary>
        /// Gets the type of the API packing group for the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>The HazardousCommodityPackingGroupType value.</returns>
        private static GenericResult<HazardousCommodityPackingGroupType> GetApiPackingGroup(IFedExPackageEntity package)
        {
            FedExHazardousMaterialsPackingGroup packingGroup = (FedExHazardousMaterialsPackingGroup) package.HazardousMaterialPackingGroup;

            switch (packingGroup)
            {
                case FedExHazardousMaterialsPackingGroup.Default: return HazardousCommodityPackingGroupType.DEFAULT;
                case FedExHazardousMaterialsPackingGroup.I: return HazardousCommodityPackingGroupType.I;
                case FedExHazardousMaterialsPackingGroup.II: return HazardousCommodityPackingGroupType.II;
                case FedExHazardousMaterialsPackingGroup.III: return HazardousCommodityPackingGroupType.III;
            }

            return new InvalidOperationException("An unrecognized packing group was provided.");
        }

        /// <summary>
        /// Gets the type of the FedEx API hazardous commodity for the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>The HazardousCommodityOptionType value.</returns>
        private static GenericResult<HazardousCommodityOptionType> GetApiHazardousCommodityType(IFedExPackageEntity package)
        {
            FedExDangerousGoodsMaterialType materialType = (FedExDangerousGoodsMaterialType) package.DangerousGoodsType;

            switch (materialType)
            {
                case FedExDangerousGoodsMaterialType.Batteries: return HazardousCommodityOptionType.BATTERY;
                case FedExDangerousGoodsMaterialType.HazardousMaterials: return HazardousCommodityOptionType.HAZARDOUS_MATERIALS;
                case FedExDangerousGoodsMaterialType.OrmD: return HazardousCommodityOptionType.ORM_D;
            }

            return new InvalidOperationException("An unrecognized type of dangerous good was provided.");
        }

        /// <summary>
        /// Gets the type of the FedEx API dangerous goods accessibility for the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>The DangerousGoodsAccessibilityType value.</returns>
        private static GenericResult<DangerousGoodsAccessibilityType> GetApiDangerousGoodsAccessibilityType(IFedExPackageEntity package)
        {
            FedExDangerousGoodsAccessibilityType accessibilityType = (FedExDangerousGoodsAccessibilityType) package.DangerousGoodsAccessibilityType;

            switch (accessibilityType)
            {
                case FedExDangerousGoodsAccessibilityType.Accessible: return DangerousGoodsAccessibilityType.ACCESSIBLE;
                case FedExDangerousGoodsAccessibilityType.Inaccessible: return DangerousGoodsAccessibilityType.INACCESSIBLE;
            }

            return new InvalidOperationException("An unrecognized dangerous goods accessibility type was provided.");
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        private void InitializeRequest(IShipmentEntity shipment, ProcessShipmentRequest request) =>
            request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems);
    }
}
