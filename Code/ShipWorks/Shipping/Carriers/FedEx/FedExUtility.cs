using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.OpenShip;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Utility functions for working with FedEx
    /// </summary>
    public static class FedExUtility
    {
        /// <summary>
        /// Gets the One Rate service types.
        /// </summary>
        public static List<FedExServiceType> OneRateServiceTypes
        {
            get
            {
                return new List<FedExServiceType>
                {
                    FedExServiceType.OneRate2Day,
                    FedExServiceType.OneRate2DayAM,
                    FedExServiceType.OneRateExpressSaver,
                    FedExServiceType.OneRateFirstOvernight,
                    FedExServiceType.OneRatePriorityOvernight,
                    FedExServiceType.OneRateStandardOvernight
                };
            }
        }

        /// <summary>
        /// Gets the valid service types.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A List of FedExServiceType objects.</returns>
        public static List<FedExServiceType> GetValidServiceTypes(ShipmentEntity shipment)
        {
            return GetValidServiceTypes(new List<ShipmentEntity> { shipment });
        }

        /// <summary>
        /// Gets the valid service types.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <returns>A List of FedExServiceType objects.</returns>
        public static List<FedExServiceType> GetValidServiceTypes(List<ShipmentEntity> shipments)
        {
            if (shipments.All(s => s.OriginCountryCode == "GB" || s.OriginCountryCode == "UK") &&
                shipments.All(s => s.ShipCountryCode == "GB" || s.ShipCountryCode == "UK"))
            {
                return GetUKServiceTypes();
            }

            FedExShipmentType shipmentType = new FedExShipmentType();
            List<FedExServiceType> serviceTypes = new List<FedExServiceType>();

            if (shipments.All(shipmentType.IsDomestic))
            {
                // All shipments are domestic
                serviceTypes = GetDomesticServiceTypes(shipments);
            }
            else if (shipments.All(s => !shipmentType.IsDomestic(s)))
            {
                // All the shipments are international shipments
                serviceTypes = GetInternationalServiceTypes(shipments);
            }

            return serviceTypes;
        }

        /// <summary>
        /// Gets the domestic service types.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <returns>A List of FedExServiceType objects.</returns>
        private static List<FedExServiceType> GetDomesticServiceTypes(List<ShipmentEntity> shipments)
        {
            // Common service types between US/Canada for domestic shipments (based on certification tests)
            List<FedExServiceType> serviceTypes = new List<FedExServiceType>
            {
                FedExServiceType.FedExGround,
                FedExServiceType.StandardOvernight,
                FedExServiceType.FirstOvernight,
                FedExServiceType.PriorityOvernight,
                FedExServiceType.FedEx2Day,
                FedExServiceType.FedEx1DayFreight,
                FedExServiceType.FedEx2DayAM,
                FedExServiceType.FedExFreightEconomy,
                FedExServiceType.FedExFreightPriority
            };

            // Since all shipments are going to the same country, just pick out the first one
            if (shipments.Count > 0 && shipments.First().AdjustedShipCountryCode() == "US")
            {
                serviceTypes.Add(FedExServiceType.FedExExpressSaver);

                // Add additional service types for US domestic
                serviceTypes.Add(FedExServiceType.GroundHomeDelivery);
                serviceTypes.Add(FedExServiceType.SmartPost);
                serviceTypes.Add(FedExServiceType.FirstFreight);
                serviceTypes.Add(FedExServiceType.FedEx2DayFreight);
                serviceTypes.Add(FedExServiceType.FedEx3DayFreight);

                // One Rate is only available for US domestic shipments (according to online ShipManager)
                serviceTypes.Add(FedExServiceType.OneRateStandardOvernight);
                serviceTypes.Add(FedExServiceType.OneRateFirstOvernight);
                serviceTypes.Add(FedExServiceType.OneRatePriorityOvernight);
                serviceTypes.Add(FedExServiceType.OneRateExpressSaver);
                serviceTypes.Add(FedExServiceType.OneRate2Day);
                serviceTypes.Add(FedExServiceType.OneRate2DayAM);
            }
            else if (shipments.Count > 0 && shipments.First().AdjustedShipCountryCode() == "CA")
            {
                serviceTypes.Add(FedExServiceType.FedExEconomyCanada);
            }

            return serviceTypes;
        }

        /// <summary>
        /// Gets the international service types.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <returns>A List of FedExServiceType objects.</returns>
        private static List<FedExServiceType> GetInternationalServiceTypes(List<ShipmentEntity> shipments)
        {
            List<FedExServiceType> serviceTypes = new List<FedExServiceType>
            {
                FedExServiceType.InternationalFirst,
                FedExServiceType.InternationalPriority,
                FedExServiceType.InternationalPriorityExpress,
                FedExServiceType.InternationalEconomy,
                FedExServiceType.InternationalPriorityFreight,
                FedExServiceType.InternationalEconomyFreight
            };

            if (shipments.All(s => (s.AdjustedOriginCountryCode() == "US" && s.AdjustedShipCountryCode() == "CA") ||
                (s.AdjustedOriginCountryCode() == "CA" && s.AdjustedShipCountryCode() == "US")))
            {
                // Ground service is allowed between US and CA
                serviceTypes.Add(FedExServiceType.FedExInternationalGround);
            }

            // Add FIMS if enabled
            if (ShippingSettings.Fetch().FedExFimsEnabled)
            {
                serviceTypes.AddRange(EnumHelper.GetEnumList<FedExServiceType>().Where(s => IsFimsService(s.Value)).Select(s => s.Value));
            }

            if (shipments.All(s => IsSmartPostEnabled(s) && s.ShipPerson.IsUSInternationalTerritory()))
            {
                // SmartPost service is allowed between US and US Territories
                serviceTypes.Add(FedExServiceType.SmartPost);
            }

            return serviceTypes;
        }

        /// <summary>
        /// FedEx Services types only available in the UK
        /// </summary>
        public static List<FedExServiceType> GetUKServiceTypes()
        {
            return new List<FedExServiceType>
            {
                FedExServiceType.FedExNextDayAfternoon,
                FedExServiceType.FedExNextDayEarlyMorning,
                FedExServiceType.FedExNextDayMidMorning,
                FedExServiceType.FedExNextDayEndOfDay,
                FedExServiceType.FedExDistanceDeferred,
                FedExServiceType.FedExNextDayFreight
            };
        }

        /// <summary>
        /// Get the valid packaging types for the specified service
        /// </summary>
        public static List<FedExPackagingType> GetValidPackagingTypes(FedExServiceType service)
        {
            List<FedExPackagingType> types = new List<FedExPackagingType>();

            switch (service)
            {
                case FedExServiceType.PriorityOvernight:
                case FedExServiceType.StandardOvernight:
                case FedExServiceType.FirstOvernight:
                case FedExServiceType.FedEx2Day:
                case FedExServiceType.FedExExpressSaver:
                case FedExServiceType.FedExEconomyCanada:
                case FedExServiceType.InternationalPriority:
                case FedExServiceType.InternationalPriorityExpress:
                case FedExServiceType.InternationalEconomy:
                case FedExServiceType.InternationalFirst:
                case FedExServiceType.FedEx2DayAM:
                    {
                        types.Add(FedExPackagingType.Envelope);
                        types.Add(FedExPackagingType.Pak);
                        types.Add(FedExPackagingType.Box);
                        types.Add(FedExPackagingType.Tube);

                        break;
                    }
            }

            switch (service)
            {
                case FedExServiceType.InternationalPriority:
                case FedExServiceType.InternationalPriorityExpress:
                    {
                        types.Add(FedExPackagingType.Box10Kg);
                        types.Add(FedExPackagingType.Box25Kg);

                        break;
                    }
            }

            if (OneRateServiceTypes.Any(s => s == service))
            {
                types.Add(FedExPackagingType.Envelope);
                types.Add(FedExPackagingType.Pak);
                types.Add(FedExPackagingType.Tube);
            }

            if (OneRateServiceTypes.All(s => s != service))
            {
                // Custom packaging is not available for the OneRate service type
                types.Add(FedExPackagingType.Custom);
            }

            if (service == FedExServiceType.FedExFreightEconomy || 
                service == FedExServiceType.FedExFreightPriority)
            {
                types.Add(FedExPackagingType.Custom);
            }
            else
            {
                // These are available for all service types
                types.Add(FedExPackagingType.SmallBox);
                types.Add(FedExPackagingType.MediumBox);
                types.Add(FedExPackagingType.LargeBox);
                types.Add(FedExPackagingType.ExtraLargeBox);
            }

            return types;
        }

        /// <summary>
        /// Create a new package entity that has default values
        /// </summary>
        public static FedExPackageEntity CreateDefaultPackage()
        {
            FedExPackageEntity package = new FedExPackageEntity();

            ApplyDimensionDefaults(package);

            package.SkidPieces = 1;

            package.DryIceWeight = 0;

            package.ContainsAlcohol = false;
            package.AlcoholRecipientType = (int) AlcoholRecipientType.CONSUMER;

            package.PriorityAlert = false;
            package.PriorityAlertDetailContent = string.Empty;
            package.PriorityAlertEnhancementType = (int) FedExPriorityAlertEnhancementType.None;

            package.Insurance = false;
            package.InsuranceValue = 0;
            package.InsurancePennyOne = false;
            package.DeclaredValue = 0;

            package.TrackingNumber = "";

            package.SignatoryContactName = string.Empty;
            package.SignatoryTitle = string.Empty;
            package.SignatoryPlace = string.Empty;

            package.ContainerType = string.Empty;
            package.NumberOfContainers = 0;
            package.PackingDetailsCargoAircraftOnly = false;
            package.PackingDetailsPackingInstructions = string.Empty;
            package.FreightPackaging = FedExFreightPhysicalPackagingType.None;
            package.FreightPieces = 0;

            ApplyDangerousGoodsDefaults(package);
            ApplyHazardousMaterialsDefaults(package);
            ApplyBatteryDetailsDefaults(package);

            return package;
        }

        /// <summary>
        /// Apply dimension defaults
        /// </summary>
        private static void ApplyDimensionDefaults(FedExPackageEntity package)
        {
            package.Weight = 0;

            package.DimsProfileID = 0;
            package.DimsLength = 0;
            package.DimsWidth = 0;
            package.DimsHeight = 0;
            package.DimsWeight = 0;
            package.DimsAddWeight = true;
        }

        /// <summary>
        /// Apply battery defaults
        /// </summary>
        private static void ApplyBatteryDetailsDefaults(FedExPackageEntity package)
        {
            package.BatteryMaterial = FedExBatteryMaterialType.NotSpecified;
            package.BatteryPacking = FedExBatteryPackingType.NotSpecified;
            package.BatteryRegulatorySubtype = FedExBatteryRegulatorySubType.NotSpecified;
        }

        /// <summary>
        /// Apply dangerous goods defaults
        /// </summary>
        private static void ApplyDangerousGoodsDefaults(FedExPackageEntity package)
        {
            package.DangerousGoodsEnabled = false;
            package.DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries;
            package.DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible;
            package.DangerousGoodsCargoAircraftOnly = false;
            package.DangerousGoodsEmergencyContactPhone = string.Empty;
            package.DangerousGoodsOfferor = string.Empty;
            package.DangerousGoodsPackagingCount = 0;
        }

        /// <summary>
        /// Apply hazardous materials defaults
        /// </summary>
        private static void ApplyHazardousMaterialsDefaults(FedExPackageEntity package)
        {
            package.HazardousMaterialNumber = string.Empty;
            package.HazardousMaterialClass = string.Empty;
            package.HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.Default;
            package.HazardousMaterialProperName = string.Empty;
            package.HazardousMaterialTechnicalName = string.Empty;
            package.HazardousMaterialQuanityUnits = (int) FedExHazardousMaterialsQuantityUnits.Kilogram;
            package.HazardousMaterialQuantityValue = 0;
        }

        /// <summary>
        /// Determines if the shipment is a FIMS shipment.
        /// </summary>
        public static bool IsFimsService(FedExServiceType service)
        {
            List<FedExServiceType> fimsServices = new List<FedExServiceType>
            {
                FedExServiceType.FedExFimsMailView,
                FedExServiceType.FedExFimsMailViewLite,
                FedExServiceType.FedExFimsPremium,
                FedExServiceType.FedExFimsStandard
            };

            return fimsServices.Contains(service);
        }

        /// <summary>
        /// Indicates if the given service is a freight service
        /// </summary>
        public static bool IsFreightService(FedExServiceType serviceType)
        {
            switch (serviceType)
            {
                case FedExServiceType.FirstFreight:
                case FedExServiceType.FedEx1DayFreight:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.InternationalEconomyFreight:
                case FedExServiceType.InternationalPriorityFreight:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if COD is available for the given service
        /// </summary>
        public static bool IsCodAvailable(FedExServiceType serviceType)
        {
            switch (serviceType)
            {
                case FedExServiceType.PriorityOvernight:
                case FedExServiceType.StandardOvernight:
                case FedExServiceType.FedEx2Day:
                case FedExServiceType.FedExExpressSaver:
                case FedExServiceType.FedExEconomyCanada:
                case FedExServiceType.FirstFreight:
                case FedExServiceType.FedEx1DayFreight:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.FedExGround:
                case FedExServiceType.FedExInternationalGround:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if Saturday delivery is available for the given service and ship date
        /// </summary>
        public static bool CanDeliverOnSaturday(FedExServiceType serviceType, DateTime shipDate)
        {
            if ((serviceType == FedExServiceType.PriorityOvernight || serviceType == FedExServiceType.FedEx1DayFreight || serviceType == FedExServiceType.FirstFreight)
                && shipDate.DayOfWeek == DayOfWeek.Friday)
            {
                return true;
            }

            if ((serviceType == FedExServiceType.FedEx2Day || serviceType == FedExServiceType.FedEx2DayAM || serviceType == FedExServiceType.FedEx2DayFreight)
                && shipDate.DayOfWeek == DayOfWeek.Thursday)
            {
                return true;
            }

            if (serviceType == FedExServiceType.InternationalPriority &&
                (shipDate.DayOfWeek == DayOfWeek.Wednesday || shipDate.DayOfWeek == DayOfWeek.Thursday || shipDate.DayOfWeek == DayOfWeek.Friday))
            {
                return true;
            }

            if (serviceType == FedExServiceType.FedExNextDayAfternoon ||
                serviceType == FedExServiceType.FedExNextDayEarlyMorning ||
                serviceType == FedExServiceType.FedExNextDayMidMorning ||
                serviceType == FedExServiceType.FedExNextDayEndOfDay ||
                serviceType == FedExServiceType.FedExNextDayFreight && shipDate.DayOfWeek == DayOfWeek.Friday)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the total weight of the package including the dimensional weight
        /// </summary>
        public static decimal GetPackageTotalWeight(FedExPackageEntity package)
        {
            double weight = package.Weight;

            if (package.DimsAddWeight)
            {
                weight += package.DimsWeight;
            }

            return (decimal) weight;
        }

        /// <summary>
        /// Get all SmartPost Hub IDs across all FedEx accounts
        /// </summary>
        public static List<string> GetSmartPostHubs()
        {
            return FedExAccountManager.Accounts
                .Select(a => XElement.Parse(a.SmartPostHubList))
                .SelectMany(x => x.Descendants("HubID").Select(n => (string) n))
                .Distinct()
                .OrderBy(s => s).ToList();
        }

        /// <summary>
        /// Load the ComboBox with the selectable list of SmartPost hubs
        /// </summary>
        public static void LoadSmartPostComboBox(ComboBox comboBox)
        {
            comboBox.DataSource = null;
            comboBox.DisplayMember = "Key";
            comboBox.ValueMember = "Value";

            List<string> hubList = GetSmartPostHubs();
            List<KeyValuePair<string, string>> bindingList = new List<KeyValuePair<string, string>>();

            if (hubList.Count == 0)
            {
                bindingList.Add(new KeyValuePair<string, string>("(No hubs configured)", "0"));
            }
            else
            {
                bindingList.Add(new KeyValuePair<string, string>("(Account Default)", "0"));

                foreach (string hubID in hubList)
                {
                    bindingList.Add(new KeyValuePair<string, string>(hubID, hubID));
                }
            }

            comboBox.DataSource = bindingList;
        }

        /// <summary>
        /// If a valid HubID is given it is returned.  If the special HubID of zero is given, then
        /// if the account has a configured default HubID it is returned.  Otherwise empty is returned.
        /// </summary>
        public static string GetSmartPostHub(string hubID, FedExAccountEntity account)
        {
            if (string.IsNullOrEmpty(hubID))
            {
                return "";
            }

            if (hubID != "0")
            {
                return hubID;
            }

            // Get the list of hubs for the account
            try
            {
                XElement defaultHub = XElement.Parse(account.SmartPostHubList).Descendants("HubID").FirstOrDefault();
                if (defaultHub != null)
                {
                    return (string) defaultHub;
                }
            }
            catch (XmlException)
            {
                // This is most likely the result of the hub ID being zero
                // Do nothing and let flow fall through to return an empty string
            }

            return "";
        }

        /// <summary>
        /// Helper method to save request and response files to a common folder
        /// </summary>
        /// <param name="uniqueId">The unique id.</param>
        /// <param name="action">The action.</param>
        /// <param name="rawSoap">The raw SOAP.</param>
        public static void SaveCertificationRequestAndResponseFiles(string uniqueId, string action, WebServiceRawSoap rawSoap)
        {
            // This will write out the request and response data to single files (FedEx is expecting the certification
            // artifacts in this format)
            string requestFilename = GetCertificationFileName(uniqueId, action, "Request", "xml", false);
            File.AppendAllText(requestFilename, rawSoap.RequestXml);

            string responseFilename = GetCertificationFileName(uniqueId, action, "Response", "xml", false);
            File.AppendAllText(responseFilename, rawSoap.ResponseXml);

            // Write the request and response to a file that will be unique for each transaction for debugging purposes
            string debugRequestFilename = GetCertificationFileName(uniqueId, action, "Request", "xml", true);
            File.AppendAllText(debugRequestFilename, rawSoap.RequestXml);

            string debugResponseFilename = GetCertificationFileName(uniqueId, action, "Response", "xml", true);
            File.AppendAllText(debugResponseFilename, rawSoap.ResponseXml);
        }

        /// <summary>
        /// Builds a file name for saving a certification file. If the isForDebugging is true, the unique ID will be
        /// used to build the file name for easier troubleshooting purposes
        /// </summary>
        /// <param name="uniqueId">The unique id.</param>
        /// <param name="action">The action.</param>
        /// <param name="postfix">The postfix.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="isForDebugging">if set to <c>true</c> [is for debugging].</param>
        public static string GetCertificationFileName(string uniqueId, string action, string postfix, string extension, bool isForDebugging)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                // If one wasn't supplied, create one.
                action = Guid.NewGuid().ToString();
            }

            string outputFolder = string.Format(@"{0}\FedExCertification{1}\", LogSession.LogFolder, isForDebugging ? "Debug\\" : string.Empty);
            Directory.CreateDirectory(outputFolder);

            // return a file name in the format of {output directory}\[uniqueId_]action_postfix.extension; unique ID
            return string.Format(@"{0}{1}{2}_{3}.{4}", outputFolder, isForDebugging ? uniqueId + "_" : string.Empty, action, postfix, extension);
        }

        /// <summary>
        /// Determines whether Smart Post is enabled.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns><c>true</c> if [smart post is enabled]; otherwise, <c>false</c>.</returns>
        public static bool IsSmartPostEnabled(IShipmentEntity shipment)
        {
            bool isEnabled = false;

            if (shipment != null && shipment.FedEx != null)
            {
                isEnabled = !String.IsNullOrEmpty(shipment.FedEx.SmartPostHubID);
            }

            return isEnabled;
        }

        /// <summary>
        /// Builds a valid tracking number from the passed in number and any relevant shipment data (like the USPS Application Id)
        /// </summary>
        public static string BuildTrackingNumber(string trackingNumber, FedExShipmentEntity fedexShipment)
        {
            return fedexShipment != null && (FedExServiceType) fedexShipment.Service == FedExServiceType.SmartPost ?
                fedexShipment.SmartPostUspsApplicationId + trackingNumber :
                trackingNumber;
        }

        /// <summary>
        /// Get the tracking number for use by the FedEx api
        /// </summary>
        /// <remarks>For most shipments, this will simply return the tracking number as-is.  For SmartPost shipments,
        /// this will remove the application id from the tracking number first.</remarks>
        public static string GetTrackingNumberForApi(string trackingNumber, FedExShipmentEntity fedexShipment)
        {
            if (fedexShipment != null &&
                (FedExServiceType) fedexShipment.Service == FedExServiceType.SmartPost &&
                trackingNumber.StartsWith(fedexShipment.SmartPostUspsApplicationId, StringComparison.Ordinal))
            {
                return trackingNumber.Substring(fedexShipment.SmartPostUspsApplicationId.Length);
            }

            return trackingNumber;
        }

        /// <summary>
        /// Determines whether shipment service is a ground service
        /// </summary>
        public static bool IsGroundService(FedExServiceType service)
        {
            return
                (new List<FedExServiceType>
                {
                    FedExServiceType.FedExGround,
                    FedExServiceType.GroundHomeDelivery,
                    FedExServiceType.FedExInternationalGround
                }).Contains(service);
        }
    }
}
