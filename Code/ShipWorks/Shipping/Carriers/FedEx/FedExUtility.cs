using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using System.Xml.Linq;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Insurance;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Utility functions for working with FedEx
    /// </summary>
    public static class FedExUtility
    {
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
            List<FedExServiceType> serviceTypes = new List<FedExServiceType>();

            if (shipments.All(s => ShipmentType.IsDomestic(s)))
            {
                // All shipments are domestic
                serviceTypes = GetDomesticServiceTypes(shipments);
            }
            else if (shipments.All(s => !ShipmentType.IsDomestic(s)))
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
                FedExServiceType.FirstOvernight,
                FedExServiceType.PriorityOvernight,
                FedExServiceType.FedEx2Day,
                FedExServiceType.FedExExpressSaver,
                FedExServiceType.FedEx1DayFreight,
                FedExServiceType.FedEx2DayAM
            };

            // Since all shipments are going to the same country, just pick out the first one
            if (shipments.Count > 0 && shipments.First().ShipCountryCode == "US")
            {
                // Add additional service types for US domestic
                serviceTypes.Add(FedExServiceType.GroundHomeDelivery);
                serviceTypes.Add(FedExServiceType.StandardOvernight);
                serviceTypes.Add(FedExServiceType.SmartPost);
                serviceTypes.Add(FedExServiceType.FirstFreight);
                serviceTypes.Add(FedExServiceType.FedEx2DayFreight);
                serviceTypes.Add(FedExServiceType.FedEx3DayFreight);
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
                FedExServiceType.InternationalEconomy,
                FedExServiceType.InternationalPriorityFreight,
                FedExServiceType.InternationalEconomyFreight
            };

            if (shipments.All(s => (s.OriginCountryCode == "US" && s.ShipCountryCode == "CA") || (s.OriginCountryCode == "CA" && s.ShipCountryCode == "US")))
            {
                // Ground service is allowed between US and CA
                serviceTypes.Add(FedExServiceType.FedExGround);
            }

            return serviceTypes;
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
                case FedExServiceType.InternationalPriority:
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
                {
                    types.Add(FedExPackagingType.Box10Kg);
                    types.Add(FedExPackagingType.Box25Kg);

                    break;
                }
            }

            types.Add(FedExPackagingType.Custom);

            return types;
        }

        /// <summary>
        /// Create a new package entity that has default values
        /// </summary>
        public static FedExPackageEntity CreateDefaultPackage()
        {
            FedExPackageEntity package = new FedExPackageEntity();

            package.Weight = 0;

            package.DimsProfileID = 0;
            package.DimsLength = 0;
            package.DimsWidth = 0;
            package.DimsHeight = 0;
            package.DimsWeight = 0;
            package.DimsAddWeight = true;

            package.SkidPieces = 1;

            package.DryIceWeight = 0;

            package.ContainsAlcohol = false;

            package.PriorityAlert = false;
            package.PriorityAlertDetailContent = string.Empty;
            package.PriorityAlertEnhancementType = (int) FedExPriorityAlertEnhancementType.None;

            package.Insurance = false;
            package.InsuranceValue = 0;
            package.InsurancePennyOne = false;
            package.DeclaredValue = 0;

            package.TrackingNumber = "";

            package.DangerousGoodsEnabled = false;
            package.DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.LithiumBatteries;            
            package.DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible;
            package.DangerousGoodsCargoAircraftOnly = false;
            package.DangerousGoodsEmergencyContactPhone = string.Empty;
            package.DangerousGoodsOfferor = string.Empty;
            package.DangerousGoodsPackagingCount = 0;
            package.HazardousMaterialNumber = string.Empty;
            package.HazardousMaterialClass = string.Empty;
            package.HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.Default;
            package.HazardousMaterialProperName = string.Empty;
            package.HazardousMaterialQuanityUnits = (int) FedExHazardousMaterialsQuantityUnits.Kilogram;
            package.HazardousMaterialQuantityValue = 0;

            return package;
        }

        /// <summary>
        /// Indicates if the given service is a freight servce
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
                case FedExServiceType.FirstFreight:
                case FedExServiceType.FedEx1DayFreight:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.FedExGround:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if saturday delivery is available for the given service and ship date
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
        /// Get all SmartPost Hub IDs accross all FedEx accounts
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
        /// Load the ComboBox with the seletable list of SmartPost hubs
        /// </summary>
        public static void LoadSmartPostComboBox(ComboBox comboBox)
        {
            comboBox.DataSource = null;
            comboBox.DisplayMember = "Key";
            comboBox.ValueMember = "Value";

            List<string> hubList = GetSmartPostHubs();
            List<KeyValuePair<string, string>> bindingList = new List<KeyValuePair<string,string>>();

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
            XElement defaultHub = XElement.Parse(account.SmartPostHubList).Descendants("HubID").FirstOrDefault();
            if (defaultHub != null)
            {
                return (string) defaultHub;
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
        /// Builds a filename for saving a certification file. If the isForDebugging is true, the unique ID will be 
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
    }
}
