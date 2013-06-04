using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using System;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Helper class for defining UPS restrictions on service/package types
    /// Also used to determine if a UPS shipment is valid based on defined restrictions
    /// </summary>
    public class UpsServicePackageTypeSetting
    {
        /// <summary>
        /// Static list of UpsServicePackageTypeSettings
        /// The list is defined in the ServicePackageValidationSettings getter
        /// </summary>
        static List<UpsServicePackageTypeSetting> servicePackageSettings;

        readonly UpsServiceType serviceType;
        readonly UpsPackagingType packageType;
        readonly WeightUnitOfMeasure weightUnitOfMeasure;
        readonly float minWeight;
        readonly float maxWeight;
        readonly int maxPackages;
        readonly bool declaredValueAllowed;

        const int legacyWorldShipVersion = 15;

        /// <summary>
        /// Constructor that defines the restriction for a service/package type
        /// </summary>
        /// <param name="serviceType">UpsServiceType</param>
        /// <param name="packageType">UpsPackagingType</param>
        /// <param name="weightUnitOfMeasure">Unit of measurement for the weight minimum/maximum</param>
        /// <param name="minWeight">The minimum weight allowed for the service type / package type</param>
        /// <param name="maxWeight">The maximum weight allowed for the service type / package type</param>
        /// <param name="maxPackages">The minimum number of packages in a single shipment allowed for the service type / package type</param>
        /// <param name="declaredValueAllowed">Bool that represents whether Declared Value is allowed to be set.  For example, MI does not allow it.</param>
        public UpsServicePackageTypeSetting(UpsServiceType serviceType, UpsPackagingType packageType, WeightUnitOfMeasure weightUnitOfMeasure, float minWeight, float maxWeight, int maxPackages, bool declaredValueAllowed)
        {
            this.serviceType = serviceType;
            this.packageType = packageType;
            this.weightUnitOfMeasure = weightUnitOfMeasure;
            this.minWeight = minWeight;
            this.maxWeight = maxWeight;
            this.maxPackages = maxPackages;
            this.declaredValueAllowed = declaredValueAllowed;
        }

        /// <summary>
        /// UpsServiceType for this UpsServicePackageTypeSetting
        /// </summary>
        public UpsServiceType ServiceType
        {
            get
            {
                return serviceType;
            }
        }

        /// <summary>
        /// UpsPackagingType for this UpsServicePackageTypeSetting
        /// </summary>
        public UpsPackagingType PackageType
        {
            get
            {
                return packageType;
            }
        }

        /// <summary>
        /// WeightUnitOfMeasure for this UpsServicePackageTypeSetting weights
        /// </summary>
        public WeightUnitOfMeasure WeightUnitOfMeasure
        {
            get
            {
                return weightUnitOfMeasure;
            }
        }

        /// <summary>
        /// Minimum weight allowed for this UpsServicePackageTypeSetting weights
        /// </summary>
        public float MinimumWeight
        {
            get
            {
                return minWeight;
            }
        }

        /// <summary>
        /// Maximum weight allowed for this UpsServicePackageTypeSetting weights
        /// </summary>
        public float MaximumWeight
        {
            get
            {
                return maxWeight;
            }
        }

        /// <summary>
        /// Minimum number of packages on a single shipment allowed for this UpsServicePackageTypeSetting weights
        /// </summary>
        public float MaximumNumberOfPackages
        {
            get
            {
                return maxPackages;
            }
        }

        /// <summary>
        /// Bool that represents whether Declared Value is allowed to be set.  For example, MI does not allow it.
        /// </summary>
        public bool DeclaredValueAllowed
        {
            get
            {
                return declaredValueAllowed;
            }
        }

        /// <summary>
        /// Method to validate a shipment against all defined UpsServicePackageTypeSetting entries
        /// </summary>
        /// <param name="shipment">The shipment to validate</param>
        /// <exception cref="ShippingException" />
        public static void Validate(ShipmentEntity shipment)
        {
            int numberOfPackages = shipment.Ups.Packages.Count();

            // Validate each package in the shipment
            foreach (var package in shipment.Ups.Packages)
            {
                Validate((UpsServiceType)shipment.Ups.Service, (UpsPackagingType)package.PackagingType, 
                    (float)UpsUtility.GetPackageTotalWeight(package), 
                    numberOfPackages);
            }
        }

        /// <summary>
        /// Method to validate a package against all defined UpsServicePackageTypeSetting entries
        /// </summary>
        /// <param name="upsServiceType"></param>
        /// <param name="upsPackageType"></param>
        /// <param name="weightInPounds"></param>
        /// <param name="numberOfPackages"></param>
        /// <exception cref="ShippingException" />
        private static void Validate(UpsServiceType upsServiceType, UpsPackagingType upsPackageType, float weightInPounds, int numberOfPackages)
        {
            // Find the setting entry for the given service type and package type
            UpsServicePackageTypeSetting setting = ServicePackageValidationSettings.FirstOrDefault(s => s.ServiceType == upsServiceType &&
                                                                                                        s.PackageType == upsPackageType);

            // If we didn't find a setting, it is not a valid combination, so throw.
            if (setting == null)
            {
                string allowedPackageTypes = string.Format("Allowed package types for {0} are:", EnumHelper.GetDescription(upsServiceType));
                foreach (UpsServicePackageTypeSetting upsServicePackageTypeSetting in ServicePackageValidationSettings.Where(s => s.ServiceType == upsServiceType))
                {
                    allowedPackageTypes = string.Format("{0}{1}{2}", 
                        allowedPackageTypes, 
                        Environment.NewLine,
                        EnumHelper.GetDescription(upsServicePackageTypeSetting.PackageType));
                }

                throw new ShippingException(string.Format("The Service type and package type combination you've selected is invalid. {0}{0}{1}", Environment.NewLine, allowedPackageTypes));
            }

            // If the setting doesn't allow the number of packages, throw
            if (numberOfPackages > setting.MaximumNumberOfPackages)
            {
                // If we are mail innovations, only 1 package is allowed, throw that message
                // otherwise throw a service/package type specific message
                if (upsServiceType == UpsServiceType.UpsMailInnovationsExpedited ||
                    upsServiceType == UpsServiceType.UpsMailInnovationsFirstClass ||
                    upsServiceType == UpsServiceType.UpsMailInnovationsIntEconomy ||
                    upsServiceType == UpsServiceType.UpsMailInnovationsIntPriority ||
                    upsServiceType == UpsServiceType.UpsMailInnovationsPriority)
                {
                    throw new ShippingException("Mail Innovations only allows 1 package per shipment.");
                }
                
                throw new ShippingException(string.Format("UPS only allows 1 package per shipment for {0} - {1}.",
                                                          EnumHelper.GetDescription(upsServiceType),
                                                          EnumHelper.GetDescription(upsPackageType)));
            }

            // Convert the weight to ounces if required
            float weight = weightInPounds;
            if (setting.WeightUnitOfMeasure == WeightUnitOfMeasure.Ounces)
            {
                weight = weight * 16;
            }

            // If the allowed weight falls outside bounds, throw.
            if (weight < setting.minWeight || weight > setting.maxWeight)
            {
                string msg = string.Format("The allowed weight for {0} - {1} is {2} to {3} {4}.",
                    EnumHelper.GetDescription(upsServiceType),
                    EnumHelper.GetDescription(upsPackageType),
                    setting.minWeight, setting.maxWeight,
                    EnumHelper.GetDescription(setting.WeightUnitOfMeasure).ToLowerInvariant());

                throw new ShippingException(msg);
            }
        }

        /// <summary>
        /// This is the list of allowed service/packge types, along with approved weight and packge counts
        /// </summary>
        public static List<UpsServicePackageTypeSetting> ServicePackageValidationSettings
        {
            get
            {
                if (servicePackageSettings != null && servicePackageSettings.Count > 0)
                {
                    return servicePackageSettings;
                }

                // In v16 (2013), Tube is no longer valid for SurePost.  So find out what version we are running, and don't add Tube
                // if we are greater than the legacy version.
                bool isWsVersionGreaterThanLegacyVersion = WorldShipUtility.WorldShipMajorVersion > legacyWorldShipVersion;

                servicePackageSettings = new List<UpsServicePackageTypeSetting>();

                // SurePost Less than 1 lb
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsSurePostLessThan1Lb, UpsPackagingType.Custom, WeightUnitOfMeasure.Ounces, 0.1f, 15.99f, 1, false));
                if (!isWsVersionGreaterThanLegacyVersion)
                {
                    servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                        UpsServiceType.UpsSurePostLessThan1Lb, UpsPackagingType.Tube, WeightUnitOfMeasure.Ounces, 0.1f, 15.99f, 1, false));
                }

                // SurePost 1 lb or Greater
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsSurePost1LbOrGreater, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 1f, 70.00f, 1, false));
                if (!isWsVersionGreaterThanLegacyVersion)
                {
                    servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                                                   UpsServiceType.UpsSurePost1LbOrGreater, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 1f, 70.00f, 1, false));
                }

                // SurePost Bound Printed Matter
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsSurePostBoundPrintedMatter, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 15.00f, 1, false));
                if (!isWsVersionGreaterThanLegacyVersion)
                {
                    servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                                                   UpsServiceType.UpsSurePostBoundPrintedMatter, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 15.00f, 1, false));
                }

                // SurePost Media
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsSurePostMedia, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 1f, 70.00f, 1, false));
                if (!isWsVersionGreaterThanLegacyVersion)
                {
                    servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                                                   UpsServiceType.UpsSurePostMedia, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 1f, 70.00f, 1, false));
                }

                // Mail Innovations Expedited
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsExpedited, UpsPackagingType.BPMFlats, WeightUnitOfMeasure.Pounds, 0.1f, 15.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsExpedited, UpsPackagingType.BPMParcels, WeightUnitOfMeasure.Pounds, 0.1f, 15.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsExpedited, UpsPackagingType.Irregulars, WeightUnitOfMeasure.Ounces, 1.0f, 15.99f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsExpedited, UpsPackagingType.Machinables, WeightUnitOfMeasure.Ounces, 6.0f, 15.99f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsExpedited, UpsPackagingType.MediaMail, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsExpedited, UpsPackagingType.ParcelPost, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsExpedited, UpsPackagingType.StandardFlats, WeightUnitOfMeasure.Ounces, 0.1f, 15.99f, 1, false));

                // Mail Innovations Priority
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsPriority, UpsPackagingType.PriorityMail, WeightUnitOfMeasure.Pounds, 0.9f, 70.0f, 1, false));

                // Mail Innovations First Class
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsFirstClass, UpsPackagingType.FirstClassMail, WeightUnitOfMeasure.Ounces, 0.1f, 13.0f, 1, false));

                // International Mail Innovations Economy
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsIntEconomy, UpsPackagingType.BPM, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsIntEconomy, UpsPackagingType.Flats, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsIntEconomy, UpsPackagingType.Parcels, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));

                // International Mail Innovations Priority
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsIntPriority, UpsPackagingType.BPM, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsIntPriority, UpsPackagingType.Flats, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsMailInnovationsIntPriority, UpsPackagingType.Parcels, WeightUnitOfMeasure.Pounds, 0.1f, 70.0f, 1, false));

                // International Worldwide Express Plus
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.Box10Kg, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.Box25Kg, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpressPlus, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // International Worldwide Express
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.Box10Kg, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.Box25Kg, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpress, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // International Worldwide Saver
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.Box10Kg, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.Box25Kg, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideSaver, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // International Worldwide Expedited
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpedited, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpedited, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpedited, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpedited, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpedited, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.WorldwideExpedited, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // Next Day Air Early AM
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirAM, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirAM, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // Next Day Air 
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAir, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAir, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAir, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAir, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAir, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAir, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAir, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // Next Day Air Saver
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsNextDayAirSaver, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // 2nd Day Air AM
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAirAM, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAirAM, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAirAM, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAirAM, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAirAM, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAirAM, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAirAM, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // 2nd Day Air 
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAir, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressLarge, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressMedium, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAir, UpsPackagingType.BoxExpressSmall, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAir, UpsPackagingType.Letter, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAir, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups2DayAir, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // 3 day select
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups3DaySelect, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // 3 day select from Canada
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.Ups3DaySelectFromCanada, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // Ground
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsGround, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // UPS Standard
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsStandard, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // UpsExpress
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpress, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpress, UpsPackagingType.BoxExpress, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpress, UpsPackagingType.ExpressEnvelope, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpress, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpress, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // UpsExpressEarlyAM
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressEarlyAm, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressEarlyAm, UpsPackagingType.BoxExpress, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressEarlyAm, UpsPackagingType.ExpressEnvelope, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressEarlyAm, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressEarlyAm, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // UpsExpressSaver
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressSaver, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressSaver, UpsPackagingType.BoxExpress, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressSaver, UpsPackagingType.ExpressEnvelope, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressSaver, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpressSaver, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));

                // UpsExpedited
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpedited, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpedited, UpsPackagingType.BoxExpress, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpedited, UpsPackagingType.ExpressEnvelope, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpedited, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsExpedited, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));


                // UpsCAWorldWideExpressSaver
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressSaver, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressSaver, UpsPackagingType.BoxExpress, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressSaver, UpsPackagingType.ExpressEnvelope, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressSaver, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressSaver, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));


                // UpsCaWorldWideExpressPlus
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressPlus, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressPlus, UpsPackagingType.BoxExpress, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressPlus, UpsPackagingType.ExpressEnvelope, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressPlus, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpressPlus, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));


                // UpsCaWorldWideExpress
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpress, UpsPackagingType.Custom, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpress, UpsPackagingType.BoxExpress, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpress, UpsPackagingType.ExpressEnvelope, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 1, false));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpress, UpsPackagingType.Pak, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));
                servicePackageSettings.Add(new UpsServicePackageTypeSetting(
                    UpsServiceType.UpsCaWorldWideExpress, UpsPackagingType.Tube, WeightUnitOfMeasure.Pounds, 0.1f, 150.0f, 9999, true));


                return servicePackageSettings;
            }
        }
    }
}
