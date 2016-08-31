using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Imports UPS packages from WorldShip
    /// </summary>
    public class WorldShipPackageImporter
    {
        private readonly IShippingManager shippingManager;
        private readonly Func<ShipmentEntity, IUpsServiceManagerFactory> upsServiceManagerFactoryProvider;
        private readonly ILog log;

        /// <summary>
        /// Package type names that come back from WorldShip
        /// </summary>
        private readonly Dictionary<string, UpsPackagingType> upsPackageTypeNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldShipPackageImporter"/> class.
        /// </summary>
        public WorldShipPackageImporter(Func<Type, ILog> logFactory, IShippingManager shippingManager, Func<ShipmentEntity, IUpsServiceManagerFactory> upsServiceManagerFactoryProvider)
        {
            this.shippingManager = shippingManager;
            this.upsServiceManagerFactoryProvider = upsServiceManagerFactoryProvider;
            log = logFactory(typeof(WorldShipPackageImporter));

            // WS returns the names of the packages differently, so create a mapping of WS package type names
            // to our UpsPackagingType
            upsPackageTypeNames = new Dictionary<string, UpsPackagingType>();
            upsPackageTypeNames["BPM FLATS"] = UpsPackagingType.BPMFlats;
            upsPackageTypeNames["BPM PARCEL"] = UpsPackagingType.BPMParcels;
            upsPackageTypeNames["BPM"] = UpsPackagingType.BPM;
            upsPackageTypeNames["FIRST CLASS"] = UpsPackagingType.FirstClassMail;
            upsPackageTypeNames["FLATS"] = UpsPackagingType.Flats;
            upsPackageTypeNames["IRREGULARS"] = UpsPackagingType.Irregulars;
            upsPackageTypeNames["MACHINABLES"] = UpsPackagingType.Machinables;
            upsPackageTypeNames["MEDIA MAIL"] = UpsPackagingType.MediaMail;
            upsPackageTypeNames["PACKAGE"] = UpsPackagingType.Custom;
            upsPackageTypeNames["PARCEL POST"] = UpsPackagingType.ParcelPost;
            upsPackageTypeNames["PARCELS"] = UpsPackagingType.Parcels;
            upsPackageTypeNames["PRIORITY"] = UpsPackagingType.PriorityMail;
            upsPackageTypeNames["STANDARD FLATS"] = UpsPackagingType.StandardFlats;
            upsPackageTypeNames["UPS 10 KG BOX"] = UpsPackagingType.Box10Kg;
            upsPackageTypeNames["UPS 25 KG BOX"] = UpsPackagingType.Box25Kg;
            upsPackageTypeNames["UPS EXPRESS BOX LARGE"] = UpsPackagingType.BoxExpressLarge;
            upsPackageTypeNames["UPS EXPRESS BOX MEDIUM"] = UpsPackagingType.BoxExpressMedium;
            upsPackageTypeNames["UPS EXPRESS BOX SMALL"] = UpsPackagingType.BoxExpressSmall;
            upsPackageTypeNames["UPS LETTER"] = UpsPackagingType.Letter;
            upsPackageTypeNames["UPS PAK"] = UpsPackagingType.Pak;
            upsPackageTypeNames["UPS TUBE"] = UpsPackagingType.Tube;

            // Canada package types
            upsPackageTypeNames["UPS EXPRESS"] = UpsPackagingType.BoxExpress;
            upsPackageTypeNames["UPS EXPRESS PAK"] = UpsPackagingType.Pak;
            upsPackageTypeNames["UPS EXPRESS TUBE"] = UpsPackagingType.Tube;
            upsPackageTypeNames["UPS EXPRESS ENVELOPE"] = UpsPackagingType.ExpressEnvelope;
        }

        /// <summary>
        /// Imports the package.
        /// </summary>
        public void ImportPackageToShipment(ShipmentEntity shipment, WorldShipProcessedEntity import)
        {
            shippingManager.EnsureShipmentLoaded(shipment);

            UpsShipmentEntity upsShipment = shipment.Ups;

            // Try to find the package by import.UpsPackageID
            // Will not find a package if import.UpsPackageID is blank
            UpsPackageEntity upsPackage =
                upsShipment.Packages.FirstOrDefault(
                    p =>
                        !string.IsNullOrWhiteSpace(import.UpsPackageID) &&
                        p.UpsPackageID.ToString() == import.UpsPackageID);

            if (upsPackage == null)
            {
                // In case after the upgrade, WS still had entries with no UpsPackageId, we need to support them
                // See if we can find a package that does not yet have a tracking number

                upsPackage =
                    upsShipment.Packages
                        .FirstOrDefault(p =>
                            string.IsNullOrEmpty(SafeTrim(p.TrackingNumber)) ^
                            string.IsNullOrEmpty(SafeTrim(import.TrackingNumber)));
            }

            // This is the case where the user created a new package in WS, so create a new one and add to the shipment
            if (upsPackage == null)
            {
                upsPackage = UpsUtility.CreateDefaultPackage();
                upsShipment.Packages.Add(upsPackage);
            }

            UpdatePackage(shipment, upsPackage, import);

            // Save the published charges
            upsShipment.PublishedCharges = (decimal) import.PublishedCharges;

            // Figure out if there was a negotiated rate
            if (import.NegotiatedCharges > 0)
            {
                upsShipment.NegotiatedRate = true;

                shipment.ShipmentCost = (decimal) import.NegotiatedCharges;
            }
            else
            {
                upsShipment.NegotiatedRate = false;
                shipment.ShipmentCost = upsShipment.PublishedCharges;
            }
        }

        /// <summary>
        /// Updates the package.
        /// </summary>
        private void UpdatePackage(ShipmentEntity shipment, UpsPackageEntity upsPackage, WorldShipProcessedEntity import)
        {
            UpsShipmentEntity upsShipment = shipment.Ups;
            UpdateServiceType(import, upsShipment);

            UpdatePackageType(import, upsPackage);

            // To support old mappings, make sure we have a non null DeclaredValueOption
            if (import.DeclaredValueOption != null)
            {
                // Update the declared value
                if (import.DeclaredValueAmount.HasValue && SafeTrim(import.DeclaredValueOption).ToUpperInvariant() == "Y")
                {
                    upsPackage.DeclaredValue = (decimal) import.DeclaredValueAmount.Value;
                }
            }

            // Update the tracking numbers (Do this AFTER we update the service/package types)
            SetTrackingNumbers(shipment, upsShipment, upsPackage, import);
        }

        /// <summary>
        /// Updates the type of the package.
        /// </summary>
        private void UpdatePackageType(WorldShipProcessedEntity import, UpsPackageEntity upsPackage)
        {
            // To support old mappings, make sure we have a non null PackageType
            if (import.PackageType != null)
            {
                // Update the package type sent from WS
                string worldShipPackageType = SafeTrim(import.PackageType).ToUpperInvariant();
                if (upsPackageTypeNames.ContainsKey(worldShipPackageType))
                {
                    UpsPackagingType packageType = upsPackageTypeNames[worldShipPackageType];
                    upsPackage.PackagingType = (int) packageType;
                }
                else
                {
                    // WorldShip sent us a package type that doesn't match what we expected...log it and move on
                    log.WarnFormat("WorldShip exported an unknown Package Type, '{0}', for ShipmentID '{1}'.",
                        worldShipPackageType, import.ShipmentID);
                }
            }
        }

        /// <summary>
        /// Updates the type of the service.
        /// </summary>
        private void UpdateServiceType(WorldShipProcessedEntity import, UpsShipmentEntity upsShipment)
        {
            // To support old mappings, make sure we have a non null ServiceType
            if (import.ServiceType != null)
            {
                // The user may have changed the service type and package type in WS, update locally so we are in sync.
                string worldShipServiceType = SafeTrim(import.ServiceType).ToUpperInvariant();

                IUpsServiceManagerFactory upsServiceManagerFactory = upsServiceManagerFactoryProvider(upsShipment.Shipment);
                IUpsServiceManager upsServiceManager = upsServiceManagerFactory.Create(upsShipment.Shipment);
                IUpsServiceMapping upsServiceMapping = null;
                try
                {
                    upsServiceMapping = upsServiceManager.GetServicesByWorldShipDescription(worldShipServiceType,
                        upsShipment.Shipment.AdjustedShipCountryCode());
                }
                catch (UpsException)
                {
                    // The description WorldShip returned isn't one we know about.  We don't want to crash SW, so we'll just leave the service type
                    // as is.  Just log and continue on.
                    log.ErrorFormat("WorldShip returned an unknown description: '{0}'", worldShipServiceType);
                }

                if (upsServiceMapping != null)
                {
                    upsShipment.Service = (int) upsServiceMapping.UpsServiceType;
                }
            }
        }

        /// <summary>
        /// Determines tracking numbers for the shipment package
        /// </summary>
        /// <param name="shipment">The shipment entity</param>
        /// <param name="upsShipment">The UPS Shipment entity</param>
        /// <param name="upsPackage">The UPS Package upon which to set tracking number</param>
        /// <param name="import">The WorldShipProcessed row with tracking information for this UPS Package</param>
        private static void SetTrackingNumbers(ShipmentEntity shipment, UpsShipmentEntity upsShipment, UpsPackageEntity upsPackage, WorldShipProcessedEntity import)
        {
            string uspsTrackingNumber;
            string trackingNumber;
            string leadTrackingNumber;

            // If we are mail innovations, set the tracking number to what WS set UspsTrackingNumber to.
            // But if that is blank, we use ReferenceNumber (1)
            // WS does not provide a LeadTrackingNumber for MI, so we'll use the UspsTrackingNumber
            if (UpsUtility.IsUpsMiService((UpsServiceType)upsShipment.Service))
            {
                uspsTrackingNumber = !string.IsNullOrWhiteSpace(import.UspsTrackingNumber) ? import.UspsTrackingNumber : (upsShipment.ReferenceNumber ?? string.Empty);
                trackingNumber = string.Empty;
                leadTrackingNumber = uspsTrackingNumber;
            }
            else if (UpsUtility.IsUpsSurePostService((UpsServiceType)upsShipment.Service))
            {
                // SurePost provides both UPS and USPS tracking numbers, so we'll use the UPS tracking numbers
                // so that tracking info is available as soon as possible for customers
                // Also save the USPS tracking number so that it is available via templates
                uspsTrackingNumber = import.UspsTrackingNumber ?? string.Empty;
                trackingNumber = import.TrackingNumber ?? uspsTrackingNumber;
                leadTrackingNumber = import.LeadTrackingNumber ?? trackingNumber;
            }
            else
            {
                // We aren't mail innovations or sure post, so just get the regular tracking number
                uspsTrackingNumber = string.Empty;
                trackingNumber = import.TrackingNumber ?? string.Empty;
                leadTrackingNumber = import.LeadTrackingNumber ?? trackingNumber;
            }

            upsPackage.UspsTrackingNumber = uspsTrackingNumber;
            upsShipment.UspsTrackingNumber = uspsTrackingNumber;
            upsPackage.TrackingNumber = trackingNumber;
            shipment.TrackingNumber = leadTrackingNumber;
        }

        /// <summary>
        /// Helper method to safely trim a possibly null string
        /// </summary>
        private static string SafeTrim(string stringToTrim)
        {
            if (!string.IsNullOrWhiteSpace(stringToTrim))
            {
                return stringToTrim.Trim();
            }

            return string.Empty;
        }
    }
}