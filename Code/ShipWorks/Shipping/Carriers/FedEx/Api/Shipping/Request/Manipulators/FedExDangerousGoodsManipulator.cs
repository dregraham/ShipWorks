using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the DangerousGoodsDetail
    /// attributes within the FedEx API's IFedExNativeShipmentRequest object if the shipment
    /// has dangerous goods enabled.
    /// </summary>
    public class FedExDangerousGoodsManipulator : FedExShippingRequestManipulatorBase
    {
        private int currentPackageIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExDangerousGoodsManipulator" /> class.
        /// </summary>
        public FedExDangerousGoodsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExDangerousGoodsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExDangerousGoodsManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            if (request.ShipmentEntity.FedEx.Packages[currentPackageIndex].DangerousGoodsEnabled)
            {
                InitializeLineItem(nativeRequest);
                FedExPackageEntity package = request.ShipmentEntity.FedEx.Packages[currentPackageIndex];

                ConfigureShippingDocuments(nativeRequest);

                // Add the service option to the request
                ConfigureDangerousGoodsOption(nativeRequest);

                DangerousGoodsDetail dangerousGoods = new DangerousGoodsDetail();

                dangerousGoods.CargoAircraftOnly = package.DangerousGoodsCargoAircraftOnly;
                dangerousGoods.CargoAircraftOnlySpecified = true;

                dangerousGoods.EmergencyContactNumber = package.DangerousGoodsEmergencyContactPhone;
                dangerousGoods.Offeror = package.DangerousGoodsOfferor;

                SetupSignatory(package, dangerousGoods);

                if (package.DangerousGoodsType != (int) FedExDangerousGoodsMaterialType.NotApplicable)
                {
                    dangerousGoods.Options = new HazardousCommodityOptionType[] { GetApiHazardousCommodityType(package) };
                }

                if (package.DangerousGoodsType == (int)FedExDangerousGoodsMaterialType.HazardousMaterials)
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

                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail = dangerousGoods;
            }
        }

        /// <summary>
        /// Setups the signatory.
        /// </summary>
        private static void SetupSignatory(FedExPackageEntity package, DangerousGoodsDetail dangerousGoods)
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
        private static void ConfigureShippingDocuments(IFedExNativeShipmentRequest nativeRequest)
        {
            if (nativeRequest.RequestedShipment.ShippingDocumentSpecification == null)
            {
                nativeRequest.RequestedShipment.ShippingDocumentSpecification = new ShippingDocumentSpecification();
            }

            List<RequestedShippingDocumentType> requestedShippingDocumentTypes = new List<RequestedShippingDocumentType>();
            if (nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes != null)
            {
                requestedShippingDocumentTypes = nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes.ToList();
            }

            requestedShippingDocumentTypes.Add(RequestedShippingDocumentType.OP_900);

            nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = requestedShippingDocumentTypes.ToArray();
            
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

        /// <summary>
        /// Configures the hazardous materials of the DangerousGoodsDetail object..
        /// </summary>
        /// <param name="dangerousGoods">The dangerous goods.</param>
        /// <param name="package">The package.</param>
        private static void ConfigureHazardousMaterials(DangerousGoodsDetail dangerousGoods, FedExPackageEntity package)
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
        /// Adds the dangerous goods option to the request.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        private void ConfigureDangerousGoodsOption(IFedExNativeShipmentRequest nativeRequest)
        {
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested == null)
            {
                // Initialize the requested special services
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = new PackageSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes == null)
            {
                // Initialize the special service type array if needed
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[0];
            }

            // Resize the special service type array so we can add the dangerous goods service type
            PackageSpecialServiceType[] services = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes;
            Array.Resize(ref services, services.Length + 1);

            // Add the dangerous goods option and update the native request
            services[services.Length - 1] = PackageSpecialServiceType.DANGEROUS_GOODS;
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = services;
        }

        /// <summary>
        /// Gets the type of the API packing group for the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>The HazardousCommodityPackingGroupType value.</returns>
        /// <exception cref="System.InvalidOperationException">An unrecognized packing group was provided.</exception>
        private static HazardousCommodityPackingGroupType GetApiPackingGroup(FedExPackageEntity package)
        {
            FedExHazardousMaterialsPackingGroup packingGroup = (FedExHazardousMaterialsPackingGroup)package.HazardousMaterialPackingGroup;

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
        /// <exception cref="System.InvalidOperationException">Un unrecognized type of dangerous good was provided.</exception>
        private static HazardousCommodityOptionType GetApiHazardousCommodityType(FedExPackageEntity package)
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
        /// <exception cref="System.InvalidOperationException">An unrecognized dangerouse goods accessibility type was provided.</exception>
        private static DangerousGoodsAccessibilityType GetApiDangerousGoodsAccessibilityType(FedExPackageEntity package)
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
        /// Initializes the line item.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="lineItemIndex">Index of the line item.</param>
        private static void InitializeLineItem(IFedExNativeShipmentRequest nativeRequest)
        {
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems.Length == 0)
            {
                // We need to resize the line item array to accommodate the index
                RequestedPackageLineItem[] packageArray = nativeRequest.RequestedShipment.RequestedPackageLineItems;
                Array.Resize(ref packageArray, 1);

                nativeRequest.RequestedShipment.RequestedPackageLineItems = packageArray;
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[0] == null)
            {
                // We need to create a new package line item
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // We'll potentially be adding COD at the package level, so initialize the package index
            currentPackageIndex = request.SequenceNumber;

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            // Package initialization - make sure at least one package is on the request
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null || nativeRequest.RequestedShipment.RequestedPackageLineItems.Length == 0)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            }
        }
    }
}
