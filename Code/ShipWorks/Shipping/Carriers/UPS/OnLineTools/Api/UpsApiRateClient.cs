using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;
using ShipWorks.Editions;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Data;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using log4net;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Wrapper for accessing UPS rates
    /// </summary>
    public class UpsApiRateClient
    {
        private ICarrierAccountRepository<UpsAccountEntity> accountRepository;
        private readonly ILog log;
        private ICarrierSettingsRepository settingsRepository;
        private ICertificateInspector certificateInspector;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsApiRateClient"/> class.
        /// </summary>
        public UpsApiRateClient()
            : this(new UpsAccountRepository(), LogManager.GetLogger(typeof(UpsApiRateClient)), new UpsSettingsRepository(), new TrustingCertificateInspector())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsApiRateClient"/> class.
        /// </summary>
        public UpsApiRateClient(ICarrierAccountRepository<UpsAccountEntity> accountRepository, ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector)
            : this(accountRepository, LogManager.GetLogger(typeof(UpsApiRateClient)), settingsRepository, certificateInspector)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsApiRateClient" /> class.
        /// </summary>
        private UpsApiRateClient(ICarrierAccountRepository<UpsAccountEntity> accountRepository, ILog log, ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector)
        {
            this.accountRepository = accountRepository;
            this.log = log;
            this.settingsRepository = settingsRepository;
            this.certificateInspector = certificateInspector;
        }

        /// <summary>
        /// Get the rates for the given shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A list of service rates from UPS.</returns>
        public List<UpsServiceRate> GetRates(ShipmentEntity shipment)
        {
            List<UpsServiceRate> rates = new List<UpsServiceRate>();
            UpsAccountEntity account = UpsApiCore.GetUpsAccount(shipment, accountRepository);

            // To track the first exception that was thrown (if any)
            UpsException firstExceptionEncountered = null;

            try
            {
                // Get non-SurePost rates using the "standard" element writers
                XmlTextWriter xmlTextWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.Rate, account, settingsRepository);
                List<UpsServiceRate> nonSurePostRates = GetRates(shipment, account, xmlTextWriter, new UpsRateServiceElementWriter(xmlTextWriter), new UpsRatePackageWeightElementWriter(xmlTextWriter), new UpsRatePackageServiceOptionsElementWriter(xmlTextWriter));

                rates.AddRange(nonSurePostRates);
            }
            catch (UpsException e)
            {
                // Log and eat the exception for now, to allow a chance to check for SurePost rates
                log.WarnFormat("An error was received trying to get the UPS rates: {0}", e.Message);

                // Make a note of the exception for possibly future use if there aren't any rates returned
                // from SurePost either
                firstExceptionEncountered = e;
            }
            

            if (EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.UpsSurePost).Level == EditionRestrictionLevel.None)
            {
                UpsServiceManagerFactory serviceManagerFactory = new UpsServiceManagerFactory(shipment);
                IUpsServiceManager upsServiceManager = serviceManagerFactory.Create(shipment);
                IEnumerable<UpsServiceType> surePostServiceTypes = upsServiceManager.GetServices(shipment).Where(s => s.IsSurePost).Select(s => s.UpsServiceType);

                foreach (UpsServiceType serviceType in surePostServiceTypes)
                {
                    try
                    {
                        rates.AddRange(GetSurePostRate(shipment, account, serviceType));
                    }
                    catch (UpsException e)
                    {
                        // Log and eat the exception, so that some rates are returned in the event there's an error obtaining SurePost rates
                        // for one or all of the SurePost service types
                        log.WarnFormat("An error was received trying to get the SurePost rates: {0}", e.Message);
                        
                        if (firstExceptionEncountered == null)
                        {
                            firstExceptionEncountered = e;
                        }
                    }
                }
            }

            if (!rates.Any() && firstExceptionEncountered != null)
            {
                // There weren't any rates found for the given shipment, but there was an exception 
                // encountered. Throw the exception to give the user feedback as to why there aren't
                // any rates
                throw firstExceptionEncountered;
            }

            return rates;
        }

        public List<UpsServiceRate> GetRates(ShipmentEntity shipment, bool useCounterRates)
        {
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

            return GetRates(shipment);
        }

        /// <summary>
        /// Gets the SurePost rates given shipment and service type.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="account">The account.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The rates from UPS.</returns>
        private List<UpsServiceRate> GetSurePostRate(ShipmentEntity shipment, UpsAccountEntity account, UpsServiceType serviceType)
        {
            using (XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.SurePostRate, account, settingsRepository))
            {
                // Ge the rates for the given SurePost service using the SurePost specific element writers
                return GetRates(shipment, account, xmlWriter, new UpsSurePostRateServiceElementWriter(xmlWriter, serviceType, shipment),
                                new UpsRateSurePostPackageWeightWriter(xmlWriter, serviceType), new UpsSurePostPackageServiceOptionsElementWriter(xmlWriter));
            }
        }

        /// <summary>
        /// Get the rates for the given shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreTooManyParams]
        private List<UpsServiceRate> GetRates(ShipmentEntity shipment, UpsAccountEntity account, XmlTextWriter xmlWriter, UpsRateServiceElementWriter serviceElementWriter,
            UpsPackageWeightElementWriter weightElementWriter, UpsPackageServiceOptionsElementWriter serviceOptionsElementWriter)
        {
            UpsShipmentEntity ups = shipment.Ups;
            UpsRateType accountRateType = (UpsRateType)account.RateType;
            UpsServiceType serviceType = (UpsServiceType)ups.Service;

            if(UpsUtility.IsUpsMiService(serviceType))
            {
                return new List<UpsServiceRate>();
            }

            // PickupType
            xmlWriter.WriteStartElement("PickupType");
            xmlWriter.WriteElementString("Code", UpsApiCore.GetPickupTypeCode(accountRateType));
            xmlWriter.WriteEndElement();

            // Customer Classification
            xmlWriter.WriteStartElement("CustomerClassification");
            xmlWriter.WriteElementString("Code", UpsApiCore.GetCustomerClassificationCode(accountRateType));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Shipment");

            // Shipper
            xmlWriter.WriteStartElement("Shipper");
            xmlWriter.WriteElementString("ShipperNumber", account.AccountNumber);
            UpsApiCore.WriteAddressXml(xmlWriter, new PersonAdapter(shipment, "Origin"));
            xmlWriter.WriteEndElement();

            var shipTo = new PersonAdapter(shipment, "Ship");
            var shipFrom = new PersonAdapter(shipment, "Origin");

            if (shipment.ReturnShipment)
            {
                PersonAdapter temp = shipTo;
                shipTo = shipFrom;
                shipFrom = temp;
            }

            // ShipTo
            xmlWriter.WriteStartElement("ShipTo");
            UpsApiCore.WriteAddressXml(xmlWriter, shipTo, ResidentialDeterminationService.DetermineResidentialAddress(shipment) ? "ResidentialAddressIndicator" : (string)null);
            xmlWriter.WriteEndElement();

            // ShipFrom
            xmlWriter.WriteStartElement("ShipFrom");
            UpsApiCore.WriteAddressXml(xmlWriter, shipFrom);
            xmlWriter.WriteEndElement();

            // Service Options
            xmlWriter.WriteStartElement("ShipmentServiceOptions");

            // If they want saturday delivery, and it could be delivered on a saturday, set that flag.
            // Note: we don't usually use the selected service for figuring what the rates are - but we do here, since we only want
            // to use the saturday flag if the user can acutally see the saturday checkbox.
            if (ups.SaturdayDelivery && UpsUtility.CanDeliverOnSaturday((UpsServiceType)ups.Service, shipment.ShipDate))
            {
                xmlWriter.WriteElementString("SaturdayDelivery", "");
            }

            if (shipment.ShipDate.DayOfWeek == DayOfWeek.Saturday)
            {
                xmlWriter.WriteElementString("SaturdayPickup", "");
            }

            // Close element
            xmlWriter.WriteEndElement();
           
            UpsApiCore.WritePackagesXml(ups, xmlWriter, false, weightElementWriter, serviceOptionsElementWriter);

            // Write the element containing any specific UPS service codes that may be needed
            serviceElementWriter.WriteServiceElement();

            // Request Negotiated Rates?
            if (accountRateType == UpsRateType.Negotiated)
            {
                // Rate Information
                xmlWriter.WriteStartElement("RateInformation");

                // Requesting Negotiated Rates 
                xmlWriter.WriteElementString("NegotiatedRatesIndicator", "");

                // Close element
                xmlWriter.WriteEndElement();
            }

            // Process the request
            XmlDocument xmlDocument = UpsWebClient.ProcessRequest(xmlWriter, LogActionType.GetRates, certificateInspector);

            return ProcessApiResponse(shipment, xmlDocument, (UpsRateType)account.RateType);
        }

        /// <summary>
        /// Process the response document returned by UPS
        /// </summary>
        private List<UpsServiceRate> ProcessApiResponse(ShipmentEntity shipment, XmlDocument xmlDocument, UpsRateType rateType)
        {
            List<UpsServiceRate> rates = new List<UpsServiceRate>();

            // Create the XPath engine
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            // Get all the shipment nodes
            XPathNodeIterator shipmentNodes = xpath.Select("//RatedShipment");
            while (shipmentNodes.MoveNext())
            {
                // Get the navigator for this shipment
                XPathNavigator shipmentNode = shipmentNodes.Current.Clone();

                // Extract the data
                string serviceCode = XPathUtility.Evaluate(shipmentNode, "Service/Code", "");
                string warning = XPathUtility.Evaluate(shipmentNode, "RatedShipmentWarning", "");
                decimal totalCharge = XPathUtility.Evaluate(shipmentNode, "TotalCharges/MonetaryValue", (decimal) 0.0);
                decimal negotiatedTotal = XPathUtility.Evaluate(shipmentNode, "NegotiatedRates/NetSummaryCharges/GrandTotal/MonetaryValue", (decimal) -1.0);
                
                int? guaranteedDaysToDelivery = XPathUtility.Evaluate(shipmentNode, "GuaranteedDaysToDelivery", -1);
                if (guaranteedDaysToDelivery == -1)
                {
                    guaranteedDaysToDelivery = null;
                }


                if (!string.IsNullOrEmpty(warning))
                {
                    log.WarnFormat("UPS returned a warning for rate {0}: {1}", serviceCode, warning);
                }

                UpsServiceType? serviceType = null;

                try
                {
                    serviceType = GetServiceTypeByRateServiceCode(shipment, serviceCode);
                }
                catch (UpsException ex)
                {
                    log.Error("UPSError in ProcessApiResponse", ex);
                }

 
                // If an unknown service type is returned from GetRates, don't display that rate.
                if (!serviceType.HasValue)
                {
                    continue;
                }

                bool negotiated = rateType == UpsRateType.Negotiated && negotiatedTotal >= 0;
                decimal amount = negotiated ? negotiatedTotal : totalCharge;

                if (rates.All(r => r.Service != serviceType))
                {
                    rates.Add(new UpsServiceRate(serviceType.Value, amount, negotiated, guaranteedDaysToDelivery));
                }
            }

            return rates;
        }

        /// <summary>
        /// Get the service type that corresponds to the given rate service code
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="rateServiceCode">The rate service code.</param>
        private UpsServiceType GetServiceTypeByRateServiceCode(ShipmentEntity shipment, string rateServiceCode)
        {
            UpsServiceManagerFactory serviceManagerFactory = new UpsServiceManagerFactory(shipment);

            // Defer to the service manager to get the service type for this shipment
            IUpsServiceManager serviceManager = serviceManagerFactory.Create(shipment);
            return serviceManager.GetServicesByRateCode(rateServiceCode, shipment.AdjustedShipCountryCode()).UpsServiceType;
        }
    }
}
