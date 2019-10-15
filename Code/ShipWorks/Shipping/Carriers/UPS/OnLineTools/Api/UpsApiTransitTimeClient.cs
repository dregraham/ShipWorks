﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// API for getting time in transit for a shipment
    /// </summary>
    public class UpsApiTransitTimeClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(UpsApiTransitTimeClient));
        
        private const string TwoDayAirSaturdayTransitCode = "2DAS";
        private const string NextDayAirSaturdayTransitCode = "1DAS";
        private const string NextDayAirAMSaturdayTransitCode = "1DMS";
              
        // These codes are found on page 52 of this doc 
        // V:/Partners/Shipping/UPS/Time in Transit/Time_In_Transit2018/TimeInTransit/Developers Guide/Time In Transit XML Developer Guide.pdf
        private static readonly List<string> saturdayTransitCodes = new List<string>
        {
            TwoDayAirSaturdayTransitCode,
            NextDayAirSaturdayTransitCode,
            NextDayAirAMSaturdayTransitCode
        };
        
        /// <summary>
        /// Get transit times for the given shipment
        /// Uses counter rates if specified
        /// </summary>
        public IEnumerable<UpsTransitTime> GetTransitTimes(ShipmentEntity shipment, bool useCounterRates)
        {
            ICarrierSettingsRepository settingsRepository = null;
            ICertificateInspector certificateInspector = null;
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository = null;

            // Create the appropriate settings, certificate inspector
            if (useCounterRates)
            {
                settingsRepository = new UpsCounterRateSettingsRepository(TangoCredentialStore.Instance);
                certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.UpsCertificateVerificationData);
                accountRepository = new UpsCounterRateAccountRepository(TangoCredentialStore.Instance);
            }
            else
            {

                settingsRepository = new UpsSettingsRepository();
                certificateInspector = new TrustingCertificateInspector();
                accountRepository = new UpsAccountRepository();
            }

            return GetTransitTimes(shipment, accountRepository, settingsRepository, certificateInspector);
        }

        /// <summary>
        /// Get transit times for the given shipment
        /// </summary>
        public IEnumerable<UpsTransitTime> GetTransitTimes(ShipmentEntity shipment,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector)
        {
            List<UpsTransitTime> upsTransitTimes = new List<UpsTransitTime>();

            using (XmlTextWriter xmlWriter = PrepareTransitRequest(shipment, accountRepository, settingsRepository))
            {
                try
                {
                    XmlDocument xmlDocument = UpsWebClient.ProcessRequest(xmlWriter, LogActionType.GetRates, certificateInspector).XmlDocument;

                    // Process the request
                    upsTransitTimes = ProcessApiResponse(xmlDocument, shipment);
                }
                catch (UpsApiException ex)
                {
                    // This error might mean only Surepost is a valid shipping option for the address.
                    if (ex.ErrorCode == "270032")
                    {
                        log.Warn("Invalid Address error for TimeInTransit call in UpsShipmentType", ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return upsTransitTimes;
        }

        /// <summary>
        /// Prepares the transit request.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static XmlTextWriter PrepareTransitRequest(ShipmentEntity shipment,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            ICarrierSettingsRepository settingsRepository)
        {
            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.TimeInTransit, UpsApiCore.GetUpsAccount(shipment, accountRepository), settingsRepository);

            UpsShipmentEntity ups = shipment.Ups;

            // Transit From
            xmlWriter.WriteStartElement("TransitFrom");
            xmlWriter.WriteStartElement("AddressArtifactFormat");
            xmlWriter.WriteElementString("PoliticalDivision2", shipment.OriginCity);
            xmlWriter.WriteElementString("PoliticalDivision1", UpsApiCore.AdjustUpsStateProvinceCode(shipment.OriginCountryCode, shipment.OriginStateProvCode));
            xmlWriter.WriteElementString("CountryCode", shipment.AdjustedOriginCountryCode());
            xmlWriter.WriteElementString("PostcodePrimaryLow", shipment.OriginPostalCode);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            // Transit To
            xmlWriter.WriteStartElement("TransitTo");
            xmlWriter.WriteStartElement("AddressArtifactFormat");
            xmlWriter.WriteElementString("PoliticalDivision2", shipment.ShipCity);
            xmlWriter.WriteElementString("PoliticalDivision1", UpsApiCore.AdjustUpsStateProvinceCode(shipment.ShipCountryCode, shipment.ShipStateProvCode));
            xmlWriter.WriteElementString("CountryCode", shipment.AdjustedShipCountryCode());
            xmlWriter.WriteElementString("PostcodePrimaryLow", shipment.ShipPostalCode);

            if (ResidentialDeterminationService.DetermineResidentialAddress(shipment))
            {
                xmlWriter.WriteElementString("ResidentialAddressIndicator", null);
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            // Pickup Date (assume today)
            xmlWriter.WriteElementString("PickupDate", DateTime.Today.ToString("yyyyMMdd"));

            // Shipment Weight. UPS currently does not allow this to be over 150, even though thats wrong, since
            // a shipment can be - its the packages the can't.  We limit the weight to 150 to get around this, it
            // does not affect the transit times.
            xmlWriter.WriteStartElement("ShipmentWeight");
            xmlWriter.WriteStartElement("UnitOfMeasurement");
            xmlWriter.WriteElementString("Code", "LBS");
            xmlWriter.WriteEndElement();
            double weight = shipment.TotalWeight <= 0.00 ? 0.1 : shipment.TotalWeight;
            xmlWriter.WriteElementString("Weight", Math.Min(weight, 150).ToString("##0.##"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("TotalPackagesInShipment", ups.Packages.Count.ToString(CultureInfo.InvariantCulture));

            if (!ShipmentTypeManager.GetType(shipment).IsDomestic(shipment))
            {
                xmlWriter.WriteStartElement("InvoiceLineTotal");

                IUpsAccountEntity account = accountRepository.GetAccountReadOnly(shipment);
                xmlWriter.WriteElementString("CurrencyCode", UpsUtility.GetCurrency(account));
                xmlWriter.WriteElementString("MonetaryValue", shipment.CustomsValue.ToString("0.00"));
                xmlWriter.WriteEndElement();
            }

            // Documents only
            if (UpsUtility.IsDocumentsOnlyRequired(shipment.AdjustedOriginCountryCode(), shipment.AdjustedShipCountryCode()) && ups.CustomsDocumentsOnly)
            {
                xmlWriter.WriteElementString("DocumentsOnlyIndicator", null);
            }
            return xmlWriter;
        }

        /// <summary>
        /// Process the given API response in the XML document.
        /// </summary>
        private static List<UpsTransitTime> ProcessApiResponse(XmlDocument xmlDocument, ShipmentEntity shipment)
        {
            List<UpsTransitTime> transitTimes = new List<UpsTransitTime>();

            // Create the XPath engine
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            // Get all the service summary nodes
            XPathNodeIterator summaryNodes = xpath.Select("//ServiceSummary");
            while (summaryNodes.MoveNext())
            {
                // Get the navigator for this service summary
                XPathNavigator summaryNode = summaryNodes.Current.Clone();

                // Extract the data
                string transitCode = XPathUtility.Evaluate(summaryNode, "Service/Code", "");
                string businessDays = XPathUtility.Evaluate(summaryNode, "EstimatedArrival/BusinessTransitDays", "");
                string date = XPathUtility.Evaluate(summaryNode, "EstimatedArrival/Date", "");
                string localTime = XPathUtility.Evaluate(summaryNode, "EstimatedArrival/Time", "");

                DateTime arrivalDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);
                arrivalDate = arrivalDate.Add(DateTime.Parse(localTime).TimeOfDay);

                try
                {
                    // Use the service manager to try to identify the service by the transit code
                    IUpsServiceManagerFactory serviceManagerFactory = new UpsServiceManagerFactory(shipment);
                    IUpsServiceManager serviceManager = serviceManagerFactory.Create(shipment);
                    
                    // UPS has different transit service codes for air services with saturday delivery. 
                    // When the shipment is marked for saturday delivery, replace the saturday code with the regular one.
                    // The api returns the results in order of quickest to slowest service and we use the first transit
                    // time we find for a given transit code. This way, we use the correct delivery date/time, but still
                    // treat the service the same everywhere else.
                    if (shipment.Ups.SaturdayDelivery &&
                        UpsUtility.CanDeliverOnSaturday((UpsServiceType) shipment.Ups.Service, shipment.ShipDate) &&
                        saturdayTransitCodes.Contains(transitCode))
                    {
                        transitCode = transitCode.Remove(transitCode.Length - 1);
                    }

                    UpsServiceType service = serviceManager.GetServiceByTransitCode(transitCode, shipment.AdjustedShipCountryCode()).UpsServiceType;

                    if (transitTimes.All(t => t.Service != service))
                    {
                        // Only add the service if we haven't seen it before
                        transitTimes.Add(new UpsTransitTime(service, Convert.ToInt32(businessDays), arrivalDate.ToUniversalTime()));
                    }
                }
                catch (UpsException)
                {
                    // There are some codes we don't account for (i.e. codes for freight services), so just log
                    // these and continue
                    log.WarnFormat("Could not lookup service for TNT code {0}", transitCode);
                }
            }

            return transitTimes;
        }
    }
}
