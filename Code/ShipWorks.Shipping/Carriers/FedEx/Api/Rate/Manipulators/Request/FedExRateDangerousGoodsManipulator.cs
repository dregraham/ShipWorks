using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.FedEx;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the DangerousGoodsDetail
    /// attributes within the FedEx API's IFedExNativeShipmentRequest object if the shipment 
    /// has dangerous goods enabled.
    /// </summary>
    public class FedExRateDangerousGoodsManipulator : IFedExRateRequestManipulator
    {
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
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) =>
            shipment.FedEx.Packages.ElementAt(0).DangerousGoodsEnabled;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <remarks>This method helps make testing easier</remarks>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            var lineItem = request.RequestedShipment.RequestedPackageLineItems[0];
            var specialServices = lineItem.SpecialServicesRequested;

            specialServices.SpecialServiceTypes =
                specialServices.SpecialServiceTypes.Append(PackageSpecialServiceType.DANGEROUS_GOODS).ToArray();

            specialServices.DangerousGoodsDetail = BuildDangerousGoods(shipment, specialServices);

            return request;
        }

        /// <summary>
        /// Build the dangerous goods element
        /// </summary>
        private DangerousGoodsDetail BuildDangerousGoods(IShipmentEntity shipment, PackageSpecialServicesRequested specialServices)
        {
            IFedExPackageEntity package = shipment.FedEx.Packages.ElementAt(0);
            DangerousGoodsDetail dangerousGoods = new DangerousGoodsDetail
            {
                CargoAircraftOnly = package.DangerousGoodsCargoAircraftOnly,
                CargoAircraftOnlySpecified = true,
                EmergencyContactNumber = package.DangerousGoodsEmergencyContactPhone,
                Offeror = package.DangerousGoodsOfferor,
                Signatory = BuildSignatory(package),
            };

            if (package.DangerousGoodsType != (int) FedExDangerousGoodsMaterialType.NotApplicable)
            {
                dangerousGoods.Options = new[] { GetApiHazardousCommodityType(package) };
            }

            if (package.DangerousGoodsType == (int) FedExDangerousGoodsMaterialType.HazardousMaterials)
            {
                ConfigureHazardousMaterials(dangerousGoods, package);
            }
            else
            {
                // Accessibility options do not apply to hazardous materials
                if (package.DangerousGoodsAccessibilityType != (int) FedExDangerousGoodsAccessibilityType.NotApplicable)
                {
                    dangerousGoods.Accessibility = GetApiDangerousGoodsAccessibilityType(package);
                    dangerousGoods.AccessibilitySpecified = true;
                }
            }

            if (package.DangerousGoodsType == (int) FedExDangerousGoodsMaterialType.Batteries)
            {
                specialServices.BatteryDetails = ConfigureBatteryMaterials(package);
            }

            return dangerousGoods;
        }

        /// <summary>
        /// Setups the signatory.
        /// </summary>
        private static DangerousGoodsSignatory BuildSignatory(IFedExPackageEntity package)
        {
            if (string.IsNullOrEmpty(package.SignatoryContactName) &&
                string.IsNullOrEmpty(package.SignatoryPlace) &&
                string.IsNullOrEmpty(package.SignatoryTitle))
            {
                return null;
            }

            return new DangerousGoodsSignatory
            {
                ContactName = package.SignatoryContactName.NullIfEmpty(),
                Title = package.SignatoryTitle.NullIfEmpty(),
                Place = package.SignatoryPlace.NullIfEmpty(),
            };
        }

        /// <summary>
        /// Configures the hazardous materials of the DangerousGoodsDetail object..
        /// </summary>
        /// <param name="dangerousGoods">The dangerous goods.</param>
        /// <param name="package">The package.</param>
        private static void ConfigureHazardousMaterials(DangerousGoodsDetail dangerousGoods, IFedExPackageEntity package)
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
                                TechnicalName = package.HazardousMaterialTechnicalName
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

            if (package.HazardousMaterialPackingGroup != (int) FedExHazardousMaterialsPackingGroup.NotApplicable)
            {
                dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup = GetApiPackingGroup(package);
                dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroupSpecified = true;
            }

            dangerousGoods.Packaging = new HazardousCommodityPackagingDetail
            {
                Count = package.DangerousGoodsPackagingCount.ToString(),
                Units = EnumHelper.GetDescription((FedExHazardousMaterialsQuantityUnits) package.HazardousMaterialQuanityUnits)
            };
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
        private bool SetBatteryDetail<T, K>(T value, IDictionary<T, K> lookup, Action<K> setProperty)
        {
            if (lookup.ContainsKey(value))
            {
                setProperty(lookup[value]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the type of the API packing group for the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>The HazardousCommodityPackingGroupType value.</returns>
        /// <exception cref="System.InvalidOperationException">An unrecognized packing group was provided.</exception>
        private static HazardousCommodityPackingGroupType GetApiPackingGroup(IFedExPackageEntity package)
        {
            FedExHazardousMaterialsPackingGroup packingGroup = (FedExHazardousMaterialsPackingGroup) package.HazardousMaterialPackingGroup;

            switch (packingGroup)
            {
                case FedExHazardousMaterialsPackingGroup.Default: return HazardousCommodityPackingGroupType.DEFAULT;
                case FedExHazardousMaterialsPackingGroup.I: return HazardousCommodityPackingGroupType.I;
                case FedExHazardousMaterialsPackingGroup.II: return HazardousCommodityPackingGroupType.II;
                case FedExHazardousMaterialsPackingGroup.III: return HazardousCommodityPackingGroupType.III;
            }

            throw new InvalidOperationException("An unrecognized packing group was provided.");
        }

        /// <summary>
        /// Gets the type of the FedEx API hazardous commodity for the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>The HazardousCommodityOptionType value.</returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized type of dangerous good was provided.</exception>
        private static HazardousCommodityOptionType GetApiHazardousCommodityType(IFedExPackageEntity package)
        {
            FedExDangerousGoodsMaterialType materialType = (FedExDangerousGoodsMaterialType) package.DangerousGoodsType;

            switch (materialType)
            {
                case FedExDangerousGoodsMaterialType.Batteries: return HazardousCommodityOptionType.BATTERY;
                case FedExDangerousGoodsMaterialType.HazardousMaterials: return HazardousCommodityOptionType.HAZARDOUS_MATERIALS;
                case FedExDangerousGoodsMaterialType.OrmD: return HazardousCommodityOptionType.ORM_D;
            }

            throw new InvalidOperationException("An unrecognized type of dangerous good was provided.");
        }

        /// <summary>
        /// Gets the type of the FedEx API dangerous goods accessibility for the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>The DangerousGoodsAccessibilityType value.</returns>
        /// <exception cref="System.InvalidOperationException">An unrecognized dangerous goods accessibility type was provided.</exception>
        private static DangerousGoodsAccessibilityType GetApiDangerousGoodsAccessibilityType(IFedExPackageEntity package)
        {
            FedExDangerousGoodsAccessibilityType accessibilityType = (FedExDangerousGoodsAccessibilityType) package.DangerousGoodsAccessibilityType;

            switch (accessibilityType)
            {
                case FedExDangerousGoodsAccessibilityType.Accessible: return DangerousGoodsAccessibilityType.ACCESSIBLE;
                case FedExDangerousGoodsAccessibilityType.Inaccessible: return DangerousGoodsAccessibilityType.INACCESSIBLE;
            }

            throw new InvalidOperationException("An unrecognized dangerous goods accessibility type was provided.");
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems)
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);
        }
    }
}
