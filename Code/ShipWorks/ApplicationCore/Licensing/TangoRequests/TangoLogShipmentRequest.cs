using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Xml;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Log a shipment to Tango
    /// </summary>
    [Component]
    public class TangoLogShipmentRequest : ITangoLogShipmentRequest
    {
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly ILog log;
        private readonly ITangoWebRequestClient client;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoLogShipmentRequest(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient client,
            IShipmentTypeManager shipmentTypeManager,
            IStoreTypeManager storeTypeManager,
            ISqlAdapterFactory sqlAdapterFactory,
            Func<Type, ILog> createLog)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.storeTypeManager = storeTypeManager;
            this.shipmentTypeManager = shipmentTypeManager;
            this.client = client;
            this.requestSubmitterFactory = requestSubmitterFactory;
            log = createLog(GetType());
        }

        /// <summary>
        /// Log the given processed shipment to Tango.  isRetry is only for internal interapptive purposes to handle rare cases where shipments a customer
        /// insured did not make it up into tango, but the shipment did actually process.
        /// </summary>
        public Result LogShipment(DbConnection connection, StoreEntity store, ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            // Get the license from the store so we know how to log
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            // Create the request
            IHttpVariableRequestSubmitter postRequest = requestSubmitterFactory.GetHttpVariableRequestSubmitter();

            // Get the shipment and store types
            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            // Both methods use the license key
            postRequest.Variables.Add("license", license.Key);
            postRequest.ForcePreCallCertificateValidation = true;

            return ProcessRequest(shipment, license, postRequest, shipmentType, store)
                .Do(x =>
                {
                    log.InfoFormat("Shipment {0}  - Accounted", shipment.ShipmentID);

                    using (ISqlAdapter adapter = sqlAdapterFactory.Create(connection))
                    {
                        // So as to not deal with out of sync issues, only update the OnlineShipmentID
                        ShipmentEntity shipmentToUpdate = new ShipmentEntity { OnlineShipmentID = x };
                        adapter.UpdateEntitiesDirectly(shipmentToUpdate, new RelationPredicateBucket(new PredicateExpression(ShipmentFields.ShipmentID == shipment.ShipmentID)));

                        adapter.Commit();
                    }
                });
        }

        /// <summary>
        /// Process the request
        /// </summary>
        private GenericResult<string> ProcessRequest(ShipmentEntity shipment, ShipWorksLicense license, IHttpVariableRequestSubmitter postRequest, ShipmentType shipmentType, StoreEntity storeEntity)
        {
            if (license.IsTrial)
            {
                // Trial shipment logging
                PrepareLogTrialShipmentRequest(postRequest, shipmentType, storeEntity);
                return client.ProcessXmlRequest(postRequest, "LogTrialShipments", true)
                    .Map(_ => shipment.ShipmentID.ToString());
            }
            else
            {
                // Regular shipment logging
                PrepareLogShipmentRequest(shipment, shipmentType, postRequest);
                return client.ProcessXmlRequest(postRequest, "LogShipmentDetails", true)
                    .Bind(GetOnlineShipmentID);
            }
        }

        /// <summary>
        /// Get the OnlineShipmentID from Tango's response
        /// </summary>
        private GenericResult<string> GetOnlineShipmentID(XmlDocument xmlResponse)
        {
            // Check for error
            XmlNode errorNode = xmlResponse.SelectSingleNode("//Error");
            if (errorNode != null)
            {
                return new TangoException(errorNode.InnerText);
            }

            return xmlResponse.SelectSingleNode("//OnlineShipmentID")?.InnerText;
        }

        /// <summary>
        /// Prepare the log shipment request for a trial shipment
        /// </summary>
        private void PrepareLogTrialShipmentRequest(IHttpVariableRequestSubmitter postRequest,
                                                           ShipmentType shipmentType,
                                                           StoreEntity store)
        {
            StoreType storeType = storeTypeManager.GetType(store);

            postRequest.Variables.Add("action", "logtrialshipments");
            postRequest.Variables.Add("shipments", "1");
            postRequest.Variables.Add("service", shipmentType.ShipmentTypeName);
            postRequest.Variables.Add("storecode", storeType.TangoCode);
            postRequest.Variables.Add("identifier", storeType.LicenseIdentifier);

            // If isretry is true, Tango will check to see if the shipment exists, and if it does
            // it will not insert it into the database.  If it doesn't exist, it will do the 
            // insert.
            postRequest.Variables.Add("isretry", "1");
        }

        /// <summary>
        /// Prepare the log shipment request
        /// </summary>
        private static void PrepareLogShipmentRequest(ShipmentEntity shipment,
                                                      ShipmentType shipmentType,
                                                      IHttpVariableRequestSubmitter postRequest)
        {
            string tracking = shipment.TrackingNumber;

            // For the purposes of U-PIC logging, CustomsNumber cannot be counted as a true TrackingNumber
            if (PostalUtility.IsPostalShipmentType(shipmentType.ShipmentTypeCode) &&
                !shipment.ShipPerson.IsDomesticCountry())
            {
                tracking = "";
            }

            postRequest.Variables.Add("action", "logshipmentdetails");
            postRequest.Variables.Add("swshipmentid", shipment.ShipmentID.ToString());
            postRequest.Variables.Add("shipdate", shipment.ShipDate.ToString("yyyy-MM-dd HH:mm:ss"));
            postRequest.Variables.Add("labelFormat", shipment.ActualLabelFormat == null ? "9" : shipment.ActualLabelFormat.Value.ToString());
            postRequest.Variables.Add("returnShipment", shipment.ReturnShipment ? "1" : "0");
            postRequest.Variables.Add("carrierCost", shipment.ShipmentCost.ToString());

            // Send best rate usage data to Tango
            BestRateEventsDescription bestRateEventsDescription = new BestRateEventsDescription((BestRateEventTypes) shipment.BestRateEvents);
            postRequest.Variables.Add("bestrateevents", bestRateEventsDescription.ToString());

            ShipmentCommonDetail shipmentDetail = shipmentType.GetShipmentCommonDetail(shipment);
            AddOrderDetailsToLogShipmentRequest(shipment, postRequest);
            AddAddressDetailsToLogShipmentRequest(shipment, postRequest);
            AddInsuranceDetailsToLogShipmentRequest(shipment, postRequest, shipmentType);
            AddPackageDetailsToLogShipmentRequest(shipment, postRequest, shipmentType, shipmentDetail);
            AddCarrierDetailsToLogShipmentRequest(shipment, postRequest, shipmentType, shipmentDetail, tracking);
        }

        /// <summary>
        /// Add carrier and service details from the given shipment to the log shipment request
        /// </summary>
        private static void AddCarrierDetailsToLogShipmentRequest(ShipmentEntity shipment,
                                                                  IHttpVariableRequestSubmitter postRequest,
                                                                  ShipmentType shipmentType,
                                                                  ShipmentCommonDetail shipmentDetail,
                                                                  string tracking)
        {
            postRequest.Variables.Add("swtype", shipment.ShipmentType.ToString());
            postRequest.Variables.Add("swtypeOriginal", shipmentDetail.OriginalShipmentType != null ?
                                          ((int) shipmentDetail.OriginalShipmentType).ToString() :
                                          string.Empty);
            postRequest.Variables.Add("swServiceType", shipmentDetail.ServiceType.ToString());

            postRequest.Variables.Add("carrier", ShippingManager.GetCarrierName(shipmentType.ShipmentTypeCode));
            postRequest.Variables.Add("service", ShippingManager.GetActualServiceUsed(shipment));
            postRequest.Variables.Add("tracking", tracking);
            postRequest.Variables.Add("originAccount", shipmentDetail.OriginAccount);

        }

        /// <summary>
        /// Add package details from the given shipment to the log shipment request
        /// </summary>
        private static void AddPackageDetailsToLogShipmentRequest(ShipmentEntity shipment,
                                                                  IHttpVariableRequestSubmitter postRequest,
                                                                  ShipmentType shipmentType,
                                                                  ShipmentCommonDetail shipmentDetail)
        {
            postRequest.Variables.Add("packageCount", shipmentType.GetParcelCount(shipment).ToString());
            postRequest.Variables.Add("swPackagingType", shipmentDetail.PackagingType.ToString());

            postRequest.Variables.Add("weight", shipment.TotalWeight.ToString());
            postRequest.Variables.Add("packageLength", shipmentDetail.PackageLength.ToString());
            postRequest.Variables.Add("packageWidth", shipmentDetail.PackageWidth.ToString());
            postRequest.Variables.Add("packageHeight", shipmentDetail.PackageHeight.ToString());
        }

        /// <summary>
        /// Add address details from the given shipment to the log shipment request
        /// </summary>
        private static void AddAddressDetailsToLogShipmentRequest(ShipmentEntity shipment,
                                                                  IHttpVariableRequestSubmitter postRequest)
        {
            postRequest.Variables.Add("firstname", shipment.ShipFirstName);
            postRequest.Variables.Add("lastname", shipment.ShipLastName);
            postRequest.Variables.Add("country", shipment.ShipPerson.AdjustedCountryCode(ShipmentTypeCode.None));
            postRequest.Variables.Add("email", shipment.ShipEmail);
            postRequest.Variables.Add("recipientCompany", shipment.ShipCompany);
            postRequest.Variables.Add("recipientPhone", shipment.ShipPhone);
            postRequest.Variables.Add("recipientPostalCode", shipment.ShipPostalCode);

            postRequest.Variables.Add("originPostalCode", shipment.OriginPostalCode);
            postRequest.Variables.Add("originCountry", shipment.OriginCountryCode);
        }

        /// <summary>
        /// Add order details from the given shipment to the log shipment request
        /// </summary>
        private static void AddOrderDetailsToLogShipmentRequest(ShipmentEntity shipment,
                                                                IHttpVariableRequestSubmitter postRequest)
        {
            postRequest.Variables.Add("swordernumber", shipment.Order.OrderNumberComplete);
            postRequest.Variables.Add("orderSubTotal", OrderUtility.CalculateTotal(shipment.Order, false).ToString());
            postRequest.Variables.Add("orderTotal", shipment.Order.OrderTotal.ToString());
        }

        /// <summary>
        /// Add insurance details from the given shipment to the log shipment request
        /// </summary>
        private static void AddInsuranceDetailsToLogShipmentRequest(ShipmentEntity shipment,
                                                                    IHttpVariableRequestSubmitter postRequest,
                                                                    ShipmentType shipmentType)
        {
            bool shipWorksInsured = false;
            bool carrierInsured = false;
            bool pennyOne = false;
            decimal insuredValue = 0;

            List<IInsuranceChoice> insuredPackages = Enumerable.Range(0, shipmentType.GetParcelCount(shipment))
                .Select(parcelIndex => shipmentType.GetParcelDetail(shipment, parcelIndex).Insurance)
                .Where(choice => choice.Insured && choice.InsuranceProvider == InsuranceProvider.ShipWorks &&
                                 choice.InsuranceValue > 0)
                .ToList();

            if (insuredPackages.Count > 0)
            {
                IInsuranceChoice insuranceChoice = insuredPackages[0];

                shipWorksInsured = true;
                pennyOne = insuranceChoice.InsurancePennyOne ?? false;
                insuredValue = insuranceChoice.InsuranceValue;
            }
            else
            {
                carrierInsured = Enumerable.Range(0, shipmentType.GetParcelCount(shipment))
                    .Select(parcelIndex => shipmentType.GetParcelDetail(shipment, parcelIndex).Insurance)
                    .Any(choice => choice.Insured && choice.InsuranceProvider == InsuranceProvider.Carrier &&
                                   choice.InsuranceValue > 0);
            }

            postRequest.Variables.Add("declaredvalue", insuredValue.ToString());
            postRequest.Variables.Add("swinsurance", shipWorksInsured ? "1" : "0");

            if (shipment.InsurancePolicy == null)
            {
                postRequest.Variables.Add("insuredwith", EnumHelper.GetApiValue(InsuredWith.NotWithApi));
            }
            else
            {
                InsuredWith insuredWith = shipment.InsurancePolicy.CreatedWithApi ?
                    InsuredWith.SuccessfullyInsuredViaApi :
                    InsuredWith.FailedToInsureViaApi;
                postRequest.Variables.Add("insuredwith", EnumHelper.GetApiValue(insuredWith));
            }

            postRequest.Variables.Add("pennyone", pennyOne ? "1" : "0");
            postRequest.Variables.Add("carrierInsured", carrierInsured ? "1" : "0");
        }
    }
}
