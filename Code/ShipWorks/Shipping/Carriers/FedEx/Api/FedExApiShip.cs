using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using System.Web.Services.Protocols;
using System.Net;
using ShipWorks.Shipping.Settings;
using System.Drawing;
using System.IO;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Templates.Tokens;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore;
using log4net;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Utilized the FedEx ship API
    /// </summary>
    public static class FedExApiShip
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FedExApiShip));

        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static ShipService CreateShipService(string logName)
        {
            ShipService webService = new ShipService(new ApiLogEntry(ApiLogSource.FedEx, logName));
            webService.Url = FedExApiCore.ServerUrl;

            return webService;
        }


        /// <summary>
        /// Void the given FedEx shipment
        /// </summary>
        public static void VoidShipment(ShipmentEntity shipment)
        {
            FedExShipmentEntity fedex = shipment.FedEx;

            FedExAccountEntity account = FedExAccountManager.GetAccount(fedex.FedExAccountID);
            if (account == null)
            {
                throw new FedExException("No FedEx account is selected for the shipment.");
            }

            DeleteShipmentRequest request = new DeleteShipmentRequest();

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client 
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "ship",
                Major = 7,
                Intermediate = 0,
                Minor = 0
            };

            TrackingIdType trackingIdType;

            FedExServiceType serviceType = (FedExServiceType) shipment.FedEx.Service;
            if (serviceType == FedExServiceType.FedExGround || 
                serviceType == FedExServiceType.GroundHomeDelivery)
            {
                trackingIdType = TrackingIdType.GROUND;
            }
            else if (serviceType == FedExServiceType.SmartPost)
            {
                trackingIdType = TrackingIdType.USPS;
            }
            else
            {
                trackingIdType = TrackingIdType.EXPRESS;
            }

            request.TrackingId = new TrackingId 
            { 
                TrackingNumber = shipment.TrackingNumber,
                TrackingIdType = trackingIdType,
                TrackingIdTypeSpecified = true
            };

            request.DeletionControl = DeletionControlType.DELETE_ALL_PACKAGES;

            try
            {
                using (ShipService webService = CreateShipService("Void"))
                {
                    ShipmentReply reply = webService.deleteShipment(request);

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }
                }
            }
            catch (SoapException ex)
            {
                throw new FedExSoapException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Process the given FedEx shipment
        /// </summary>
        public static void ProcessShipment(ShipmentEntity shipment)
        {
            FedExShipmentEntity fedex = shipment.FedEx;

            FedExAccountEntity account = FedExAccountManager.GetAccount(fedex.FedExAccountID);
            if (account == null)
            {
                throw new FedExException("No FedEx account is selected for the shipment.");
            }
            else if (account.Is2xMigrationPending)
            {
                throw new FedExException("The FedEx account selected for the shipment was migrated from ShipWorks 2, but has not yet been configured for ShipWorks 3.");
            }

            // Ensure we have done version capture
            FedExApiRegistration.VersionCapture();

            // Build the list of packages
            foreach (FedExPackageEntity package in shipment.FedEx.Packages)
            {
                ProcessPackage(account, shipment, package);
            }
        }

        /// <summary>
        /// Process the specific package of the given shipment
        /// </summary>
        private static void ProcessPackage(FedExAccountEntity account, ShipmentEntity shipment, FedExPackageEntity package)
        {
            FedExShipmentEntity fedex = shipment.FedEx;
            int packageIndex = fedex.Packages.IndexOf(package);

            ProcessShipmentRequest shipRequest = new ProcessShipmentRequest();

            // Authentication
            shipRequest.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client 
            shipRequest.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            shipRequest.Version = new VersionId
            {
                ServiceId = "ship",
                Major = 7,
                Intermediate = 0,
                Minor = 0
            };

            // Set the detail object
            RequestedShipment shipDetail = new RequestedShipment();
            shipRequest.RequestedShipment = shipDetail;

            // For MPS, have to propagate the master tracking info
            if (packageIndex > 0)
            {
                shipDetail.MasterTrackingId = new TrackingId();
                shipDetail.MasterTrackingId.TrackingNumber = shipment.TrackingNumber;
                shipDetail.MasterTrackingId.FormId = fedex.MasterFormID;
            }

            // Set up the special services container for anyone who needs 
            shipDetail.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            List<ShipmentSpecialServiceType> specialServiceTypes = new List<ShipmentSpecialServiceType>();

            // Where it's coming from
            shipDetail.Shipper = new Party();
            shipDetail.Shipper.Contact = FedExApiCore.CreateContact<Contact>(new PersonAdapter(shipment, "Origin"));
            shipDetail.Shipper.Address = FedExApiCore.CreateAddress<Address>(new PersonAdapter(shipment, "Origin"));

            // Where it's going
            shipDetail.Recipient = new Party();
            shipDetail.Recipient.Contact = FedExApiCore.CreateContact<Contact>(new PersonAdapter(shipment, "Ship"));
            shipDetail.Recipient.Address = FedExApiCore.CreateAddress<Address>(new PersonAdapter(shipment, "Ship"));

            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipment))
            {
                shipDetail.Recipient.Address.Residential = shipment.ResidentialResult;
                shipDetail.Recipient.Address.ResidentialSpecified = true;
            }

            // This is just for certification gayness
            if (InterapptiveOnly.IsInterapptiveUser && !string.IsNullOrEmpty(fedex.ReferencePO))
            {
                shipRequest.TransactionDetail = new TransactionDetail();
                shipRequest.TransactionDetail.CustomerTransactionId = TemplateTokenProcessor.ProcessTokens(fedex.ReferencePO, shipment.ShipmentID);
            }

            // We just want the actual account rates
            shipDetail.RateRequestTypes = new RateRequestType[] { FedExApiCore.UseListRates ? RateRequestType.LIST : RateRequestType.ACCOUNT };

            // When its giong out and how
            shipDetail.ShipTimestamp = new DateTime(shipment.ShipDate.Ticks, DateTimeKind.Local);
            shipDetail.DropoffType = DropoffType.REGULAR_PICKUP;

            // If its future, set that flag
            if (shipDetail.ShipTimestamp.Date != DateTime.Today)
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT);
            }

            // If its a saturday, set the pickup flag
            if (shipDetail.ShipTimestamp.DayOfWeek == DayOfWeek.Saturday)
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_PICKUP);
            }

            // If they want saturday delivery, and it could be delivered on a saturday, set that flag
            if (shipment.FedEx.SaturdayDelivery && FedExUtility.CanDeliverOnSaturday((FedExServiceType) fedex.Service, shipDetail.ShipTimestamp))
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_DELIVERY);
            }

            // Service and packaging
            shipDetail.ServiceType = GetApiServiceType((FedExServiceType) fedex.Service);
            shipDetail.PackagingType = GetApiPackagingType((FedExPackagingType) fedex.PackagingType);

            // Weight and declared value
            shipDetail.TotalWeight = new Weight { Units = WeightUnits.LB, Value = (decimal) shipment.TotalWeight };
            shipDetail.TotalInsuredValue = new Money { Currency = "USD", Amount = shipment.FedEx.Packages.Sum(p => p.DeclaredValue) };

            // Payment.  For this to work correctly, CountryCode needs to be specified (as opposed to Duties)
            shipDetail.ShippingChargesPayment = GetPaymentDetail(fedex.PayorTransportType, fedex.PayorTransportAccount, "US", account);

            RequestedPackageLineItem packageRequest = new RequestedPackageLineItem();
            packageRequest.SequenceNumber = (packageIndex + 1).ToString();

            // Prepare for possible special services
            PackageSpecialServicesRequested packageServices = new PackageSpecialServicesRequested();
            packageRequest.SpecialServicesRequested = packageServices;

            // Set the packages
            shipDetail.PackageCount = fedex.Packages.Count.ToString();
            shipDetail.PackageDetail = RequestedPackageDetailType.INDIVIDUAL_PACKAGES;
            shipDetail.PackageDetailSpecified = true;
            shipDetail.RequestedPackageLineItems = new RequestedPackageLineItem[] { packageRequest };

            // Home delivery info
            ApplyHomeDeliveryOptions(shipDetail, specialServiceTypes, fedex);

            // Freight
            ApplyFreightOptions(shipDetail, specialServiceTypes, fedex);

            // Email
            ApplyEmailOptions(shipDetail, specialServiceTypes, fedex);

            // Apply COD options
            ApplyCodOptions(shipDetail, specialServiceTypes, fedex, packageIndex);

            // Apply international values
            ApplyInternational(shipDetail, specialServiceTypes, shipment, account);

            // In any special services were added, set them
            shipDetail.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();

            // Package weight and value
            packageRequest.Weight = new Weight { Units = WeightUnits.LB, Value = FedExUtility.GetPackageTotalWeight(package) };
            packageRequest.InsuredValue = new Money { Amount = package.DeclaredValue, Currency = "USD" };

            // Apply smart post options
            ApplySmartPost(shipDetail, shipment, account);

            if (fedex.PackagingType == (int) FedExPackagingType.Custom)
            {
                packageRequest.Dimensions = new Dimensions();
                packageRequest.Dimensions.Units = LinearUnits.IN;
                packageRequest.Dimensions.Length = ((int) package.DimsLength).ToString();
                packageRequest.Dimensions.Height = ((int) package.DimsHeight).ToString();
                packageRequest.Dimensions.Width = ((int) package.DimsWidth).ToString();
            }

            List<PackageSpecialServiceType> specialServices = new List<PackageSpecialServiceType>();

            // Signature
            FedExSignatureType signature = (FedExSignatureType) fedex.Signature;
            if (signature != FedExSignatureType.ServiceDefault)
            {
                packageServices.SignatureOptionDetail = new SignatureOptionDetail();
                packageServices.SignatureOptionDetail.OptionType = GetApiSignatureType(signature);
                packageServices.SignatureOptionDetail.SignatureReleaseNumber = account.SignatureRelease;

                specialServices.Add(PackageSpecialServiceType.SIGNATURE_OPTION);
            }

            // Non-standard container (only applies to Ground services)
            if (fedex.NonStandardContainer && (shipDetail.ServiceType == ServiceType.GROUND_HOME_DELIVERY || shipDetail.ServiceType == ServiceType.FEDEX_GROUND))
            {
                specialServices.Add(PackageSpecialServiceType.NON_STANDARD_CONTAINER);
            }

            List<CustomerReference> references = new List<CustomerReference>();
            fedex.ReferenceCustomer = AddCustomerReference(fedex, references, fedex.ReferenceCustomer, CustomerReferenceType.CUSTOMER_REFERENCE);
            fedex.ReferenceInvoice = AddCustomerReference(fedex, references, fedex.ReferenceInvoice, CustomerReferenceType.INVOICE_NUMBER);

            // For internal users (for fedex cert) this get's used as the transid
            if (!InterapptiveOnly.IsInterapptiveUser)
            {
                fedex.ReferencePO = AddCustomerReference(fedex, references, fedex.ReferencePO, CustomerReferenceType.P_O_NUMBER);
            }

            // Set the references
            if (references.Count > 0)
            {
                packageRequest.CustomerReferences = references.ToArray();
            }

            // Set the special service type flags
            packageServices.SpecialServiceTypes = specialServices.ToArray();

            // Label specification
            shipDetail.LabelSpecification = GetLabelSpecification(fedex);

            try
            {
                using (ShipService webService = CreateShipService("Process"))
                {
                    ProcessShipmentReply reply = webService.processShipment(shipRequest);

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    // This should never happen, but our users will let us know if it does
                    if (reply.CompletedShipmentDetail.CompletedPackageDetails.Length != 1)
                    {
                        throw new ShippingException("Invalid number of package details returned for a single package request.");
                    }

                    // If we had saved an image for this shipment previously, but the shipment errored out later (like for an MPS), then clear before
                    // we start.
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        ObjectReferenceManager.ClearReferences(shipment.ShipmentID);
                        ObjectReferenceManager.ClearReferences(package.FedExPackageID);
                    }

                    // Documents
                    if (reply.CompletedShipmentDetail.ShipmentDocuments != null)
                    {
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            foreach (ShippingDocument document in reply.CompletedShipmentDetail.ShipmentDocuments
                                .Where(d => d.Type != ReturnedShippingDocumentType.TERMS_AND_CONDITIONS))
                            {
                                SaveLabel("Document" + GetLabelName(document.Type), document, shipment.ShipmentID);
                            }
                        }
                    }

                    CompletedPackageDetail packageReply = reply.CompletedShipmentDetail.CompletedPackageDetails[0];

                    // Save the package tracking information
                    package.TrackingNumber = packageReply.TrackingIds[0].TrackingNumber;

                    // If this is the first one, save the tracking stuff
                    if (packageIndex == 0)
                    {
                        // For MPS there is an actual master tracking number
                        if (reply.CompletedShipmentDetail.MasterTrackingId != null)
                        {
                            shipment.TrackingNumber = reply.CompletedShipmentDetail.MasterTrackingId.TrackingNumber;
                            shipment.FedEx.MasterFormID = reply.CompletedShipmentDetail.MasterTrackingId.FormId;
                        }
                        // For single, just use the package level number
                        else
                        {
                            shipment.TrackingNumber = package.TrackingNumber;
                            shipment.FedEx.MasterFormID = "";
                        }

                        // For COD we have to save off the cod tracking info.  Null for ground
                        if (fedex.CodEnabled && reply.CompletedShipmentDetail.CodReturnDetail != null 
                                             && reply.CompletedShipmentDetail.CodReturnDetail.CodRoutingDetail != null
                                             && reply.CompletedShipmentDetail.CodReturnDetail.CodRoutingDetail.AstraDetails != null)
                        {
                            TrackingId codTrackingID = reply.CompletedShipmentDetail.CodReturnDetail.CodRoutingDetail.AstraDetails[0].TrackingId;
                            fedex.CodTrackingNumber = codTrackingID.TrackingNumber;
                            fedex.CodTrackingFormID = codTrackingID.FormId;
                        }
                    }

                    // If it's the last one, then we save the rating
                    if (packageIndex == fedex.Packages.Count - 1)
                    {
                        ShipmentRating ratingInfo = reply.CompletedShipmentDetail.ShipmentRating;

                        if (ratingInfo != null)
                        {
                            // By default use the first one
                            ShipmentRateDetail rateDetail = ratingInfo.ShipmentRateDetails[0];

                            // If there is more than one, try to use the actual one
                            if (ratingInfo.ShipmentRateDetails.Length > 1 && ratingInfo.ActualRateTypeSpecified)
                            {
                                ShipmentRateDetail actualDetail = ratingInfo.ShipmentRateDetails.Where(d => d.RateType == ratingInfo.ActualRateType).FirstOrDefault();
                                if (actualDetail != null)
                                {
                                    rateDetail = actualDetail;
                                }
                            }

                            // Save the rate used
                            shipment.ShipmentCost = rateDetail.TotalNetCharge.Amount;
                        }
                        else
                        {
                            shipment.ShipmentCost = 0;

                            log.WarnFormat("FedEx did not return rating details for shipment {0}", shipment.ShipmentID);
                        }

                        // For COD we have to save off the cod label
                        if (fedex.CodEnabled && reply.CompletedShipmentDetail.CodReturnDetail.Label != null)
                        {
                            // Save the label image
                            using (SqlAdapter adapter = new SqlAdapter())
                            {
                                SaveLabel("COD", reply.CompletedShipmentDetail.CodReturnDetail.Label, shipment.ShipmentID);
                            }
                        }
                    }

                    // Save the label iamges
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        // Save the primary label image
                        SaveLabel("LabelImage", packageReply.Label, package.FedExPackageID);

                        // Package level COD
                        if (fedex.CodEnabled && packageReply.CodReturnDetail != null && packageReply.CodReturnDetail.Label != null)
                        {
                            SaveLabel("COD", packageReply.CodReturnDetail.Label, package.FedExPackageID);
                        }

                        // Save all the additional labels
                        if (packageReply.PackageDocuments != null)
                        {
                            foreach (ShippingDocument document in packageReply.PackageDocuments
                                .Where(d => d.Type != ReturnedShippingDocumentType.TERMS_AND_CONDITIONS))
                            {
                                SaveLabel("Document" + GetLabelName(document.Type), document, package.FedExPackageID);
                            }
                        }
                    }
                }
            }
            catch (SoapException ex)
            {
                throw new FedExSoapException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Save a label of the given name ot the database from the specified label document
        /// </summary>
        private static void SaveLabel(string name, ShippingDocument labelDocument, long ownerID)
        {
            // We need to know if this ever happens
            if (labelDocument.Parts.Length != 1)
            {
                throw new ShippingException("Multiple parts returned for label. " + labelDocument);
            }

            // Convert the string into an image stream
            using (MemoryStream imageStream = new MemoryStream(labelDocument.Parts[0].Image))
            {
                // Save the label image
                DataResourceManager.CreateFromBytes(imageStream.ToArray(), ownerID, name);

                // File.WriteAllBytes(string.Format(@"D:\Vista Folders\Desktop\Random\{0}.png", name), labelDocument.Parts[0].Image);
            }
        }

        /// <summary>
        /// Get the name of the label to be used for the specified document type
        /// </summary>
        private static string GetLabelName(ReturnedShippingDocumentType documentType)
        {
            switch (documentType)
            {
                case ReturnedShippingDocumentType.AUXILIARY_LABEL: return "AuxiliaryLabel";
                case ReturnedShippingDocumentType.TERMS_AND_CONDITIONS: return "TermsAndConditions";
            }

            throw new InvalidOperationException("Unhandled label document type: " + documentType);
        }

        /// <summary>
        /// Apply SmartPost options to the shipment
        /// </summary>
        private static void ApplySmartPost(RequestedShipment shipDetail, ShipmentEntity shipment, FedExAccountEntity account)
        {
            if (shipment.FedEx.Service != (int) FedExServiceType.SmartPost)
            {
                return;
            }

            FedExShipmentEntity fedex = shipment.FedEx;

            SmartPostShipmentDetail smartPost = new SmartPostShipmentDetail();
            shipDetail.SmartPostDetail = smartPost;

            fedex.SmartPostHubID = FedExUtility.GetSmartPostHub(fedex.SmartPostHubID, account);
            smartPost.HubId = fedex.SmartPostHubID;

            smartPost.Indicia = GetSmartPostIndiciaType((FedExSmartPostIndicia) fedex.SmartPostIndicia);
            smartPost.IndiciaSpecified = true;

            var endorsement = GetSmartPostEndorsementType((FedExSmartPostEndorsement) fedex.SmartPostEndorsement);
            if (endorsement != null)
            {
                smartPost.AncillaryEndorsement = endorsement.Value;
                smartPost.AncillaryEndorsementSpecified = true;
            }

            if (smartPost.Indicia == SmartPostIndiciaType.PARCEL_SELECT)
            {
                fedex.SmartPostConfirmation = true;
            }

            if (fedex.SmartPostConfirmation)
            {
                smartPost.SpecialServices = new SmartPostShipmentSpecialServiceType[] { SmartPostShipmentSpecialServiceType.USPS_DELIVERY_CONFIRMATION };
            }

            fedex.SmartPostCustomerManifest = TemplateTokenProcessor.ProcessTokens(fedex.SmartPostCustomerManifest, shipment.ShipmentID);
            smartPost.CustomerManifestId = fedex.SmartPostCustomerManifest;

            // For smartpost this is always zero
            shipDetail.TotalInsuredValue.Amount = 0;
            shipDetail.RequestedPackageLineItems[0].InsuredValue.Amount = 0;
        }

        /// <summary>
        /// Apply international options to the shipment
        /// </summary>
        private static void ApplyInternational(RequestedShipment shipDetail, List<ShipmentSpecialServiceType> specialServiceTypes, ShipmentEntity shipment, FedExAccountEntity account)
        {
            // Nothing to do for domestic
            if (shipment.ShipCountryCode == "US")
            {
                return;
            }

            // Create the intl content holder
            InternationalDetail intl = new InternationalDetail();
            shipDetail.InternationalDetail = intl;

            FedExShipmentEntity fedex = shipment.FedEx;

            // Overall customs value
            intl.CustomsValue = new Money { Amount = shipment.CustomsValue, Currency = "USD" };

            // Documents only
            intl.DocumentContent = fedex.CustomsDocumentsOnly ? InternationalDocumentContentType.DOCUMENTS_ONLY : InternationalDocumentContentType.NON_DOCUMENTS;

            // Commodities
            if (!fedex.CustomsDocumentsOnly)
            {
                intl.Commodities = GetInternationalCommodities(shipment).ToArray();
            }
            else
            {
                intl.Commodities = new Commodity[] { 
                    new Commodity
                    {
                        Description = fedex.CustomsDocumentsDescription,
                        Quantity = "1",
                        QuantityUnits = "EA",
                        NumberOfPieces = "1",
                        Weight = new Weight { Value = 0, Units = WeightUnits.LB },
                        UnitPrice = new Money { Amount = 0, Currency = "USD" },
                        CountryOfManufacture = "US",
                        HarmonizedCode = ""
                    }
                };
            }

            // Duties payor.  For Duties to work correctly CountryCode should be blank.
            intl.DutiesPayment = GetPaymentDetail(fedex.PayorDutiesType, fedex.PayorDutiesAccount, "", account);

            // Broker
            ApplyBrokerOptions(shipDetail, specialServiceTypes, fedex);

            // Admissibility for canada only 
            if (shipment.ShipCountryCode == "CA")
            {
                shipDetail.RequestedPackageLineItems[0].PhysicalPackaging = GetApiAdmissibilityPackagingType((FedExPhysicalPackagingType) fedex.CustomsAdmissibilityPackaging);
                shipDetail.RequestedPackageLineItems[0].PhysicalPackagingSpecified = true;
            }

            // Recipient TIN
            shipDetail.Recipient.Tin = new TaxpayerIdentification
                {
                    Number = fedex.CustomsRecipientTIN,
                    TinType = TinType.EIN
                };

            // Commercial Invoice
            ApplyCommercialInvoiceOptions(shipDetail, fedex);
        }

        /// <summary>
        /// Generate the list of international commodities based on the commoedities for the given shipment
        /// </summary>
        private static List<Commodity> GetInternationalCommodities(ShipmentEntity shipment)
        {
            List<Commodity> commodities = new List<Commodity>();

            foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
            {
                Commodity commodity = new Commodity();
                commodities.Add(commodity);

                commodity.Description = customsItem.Description;
                commodity.Quantity = Math.Ceiling(customsItem.Quantity).ToString();
                commodity.QuantityUnits = "EA";
                commodity.NumberOfPieces = commodity.Quantity;
                commodity.Weight = new Weight { Value = (decimal) customsItem.Weight, Units = WeightUnits.LB };
                commodity.UnitPrice = new Money { Amount = customsItem.UnitValue, Currency = "USD" };
                commodity.CountryOfManufacture = customsItem.CountryOfOrigin;
                commodity.HarmonizedCode = customsItem.HarmonizedCode;
            }

            return commodities;
        }
        
        /// <summary>
        /// Add a reference to the reference list of the reference is non-blank
        /// </summary>
        private static string AddCustomerReference(FedExShipmentEntity fedex, List<CustomerReference> references, string referenceToken, CustomerReferenceType referenceType)
        {
            string referenceValue = TemplateTokenProcessor.ProcessTokens(referenceToken, fedex.ShipmentID);

            if (!string.IsNullOrEmpty(referenceValue))
            {
                references.Add(
                        new CustomerReference
                        {
                            CustomerReferenceType = referenceType,
                            Value = referenceValue
                        }
                    );
            }

            return referenceValue;
        }

        /// <summary>
        /// Add COD stuff to the request
        /// </summary>
        private static void ApplyCodOptions(RequestedShipment shipDetail, List<ShipmentSpecialServiceType> specialServices, FedExShipmentEntity fedex, int packageIndex)
        {
            if (!fedex.CodEnabled)
            {
                return;
            }

            specialServices.Add(ShipmentSpecialServiceType.COD);

            FedExServiceType serviceType = (FedExServiceType) fedex.Service;

            bool isGround = serviceType == FedExServiceType.GroundHomeDelivery || serviceType == FedExServiceType.FedExGround;

            // Holds the COD details
            CodDetail codDetail = new CodDetail();
            shipDetail.SpecialServicesRequested.CodDetail = codDetail;

            // The CAD is per test case 502244
            string currency = fedex.Shipment.ShipCountryCode == "CA" ? "CAD" : "USD";

            // Amount.  Per-package for ground, per-shipment for express
            if (isGround)
            {
                decimal amount = fedex.CodAmount / fedex.Packages.Count;

                shipDetail.RequestedPackageLineItems[0].SpecialServicesRequested.CodCollectionAmount = new Money 
                { 
                    Amount = amount, 
                    Currency = currency
                };
            }
            else
            {
                shipDetail.SpecialServicesRequested.CodCollectionAmount = new Money { Amount = fedex.CodAmount, Currency = currency };
            }

            // Secured\unsecured
            codDetail.CollectionType = GetApiCodCollectionType((FedExCodPaymentType) fedex.CodPaymentType);

            // Add Freight
            if (fedex.CodAddFreight)
            {
                codDetail.AddTransportationCharges = CodAddTransportationChargesType.ADD_ACCOUNT_NET_FREIGHT;
                codDetail.AddTransportationChargesSpecified = true;
            }

            // Recipient
            codDetail.CodRecipient = new Party();
            codDetail.CodRecipient.Address = FedExApiCore.CreateAddress<Address>(new PersonAdapter(fedex, "Cod"));
            codDetail.CodRecipient.Address.CountryCode = "US";
            codDetail.CodRecipient.Contact = FedExApiCore.CreateContact<Contact>(new PersonAdapter(fedex, "Cod"));

            // If this is the last shipment of an MPS, we need to put the return tracking id, and it exists (in Ground it doesnt)
            if (fedex.Packages.Count > 1 && packageIndex == fedex.Packages.Count - 1 && !string.IsNullOrEmpty(fedex.CodTrackingNumber))
            {
                shipDetail.CodReturnTrackingId = new TrackingId();
                shipDetail.CodReturnTrackingId.TrackingNumber = fedex.CodTrackingNumber;
                shipDetail.CodReturnTrackingId.FormId = fedex.CodTrackingFormID;
            }
        }

        /// <summary>
        /// Home delivery detail information
        /// </summary>
        private static void ApplyHomeDeliveryOptions(RequestedShipment detail, List<ShipmentSpecialServiceType> specialServices, FedExShipmentEntity fedex)
        {
            if (fedex.Service != (int) FedExServiceType.GroundHomeDelivery)
            {
                return;
            }

            FedExHomeDeliveryType type = (FedExHomeDeliveryType) fedex.HomeDeliveryType;
            if (type == FedExHomeDeliveryType.None)
            {
                return;
            }

            // Add home deliv premium
            specialServices.Add(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM);

            // Insructions
            detail.DeliveryInstructions = fedex.HomeDeliveryInstructions;

            // Figure out the FedEx API premium type value
            HomeDeliveryPremiumType premiumType;

            if (type == FedExHomeDeliveryType.DateCertain)
            {
                premiumType = HomeDeliveryPremiumType.DATE_CERTAIN;
            }
            else if (type == FedExHomeDeliveryType.Evening)
            {
                premiumType = HomeDeliveryPremiumType.EVENING;
            }
            else
            {
                premiumType = HomeDeliveryPremiumType.APPOINTMENT;
            }

            HomeDeliveryPremiumDetail home = new HomeDeliveryPremiumDetail();
            home.HomeDeliveryPremiumType = premiumType;
            home.PhoneNumber = fedex.HomeDeliveryPhone;

            if (type == FedExHomeDeliveryType.DateCertain)
            {
                home.Date = fedex.HomeDeliveryDate;
                home.DateSpecified = true;
            }

            detail.SpecialServicesRequested.HomeDeliveryPremiumDetail = home;
        }

        /// <summary>
        /// Apply email notification options
        /// </summary>
        private static void ApplyEmailOptions(RequestedShipment detail, List<ShipmentSpecialServiceType> specialServices, FedExShipmentEntity fedex)
        {
            bool notifySender = fedex.EmailNotifySender != 0 && fedex.Shipment.OriginEmail.Length > 0;
            bool notifyRecipient = fedex.EmailNotifyRecipient != 0 && fedex.Shipment.ShipEmail.Length > 0;
            bool notifyOther = fedex.EmailNotifyOther != 0 && fedex.EmailNotifyOtherAddress.Length > 0;

            // Check if there are any at all up front
            if (!(notifySender || notifyRecipient || notifyOther))
            {
                return;
            }

            specialServices.Add(ShipmentSpecialServiceType.EMAIL_NOTIFICATION);

            EMailNotificationDetail emailDetail = new EMailNotificationDetail();
            detail.SpecialServicesRequested.EMailNotificationDetail = emailDetail;

            // Set the special message
            if (!string.IsNullOrEmpty(fedex.EmailNotifyMessage))
            {
                emailDetail.PersonalMessage = fedex.EmailNotifyMessage;
            }

            List<EMailNotificationRecipient> recipients = new List<EMailNotificationRecipient>();

            // See if any are being sent to the sender
            if (notifySender)
            {
                EMailNotificationRecipient recipient = new EMailNotificationRecipient();
                recipients.Add(recipient);

                recipient.Format = EMailNotificationFormatType.HTML;
                recipient.Localization = new Localization { LanguageCode = "EN" };

                recipient.EMailNotificationRecipientType = EMailNotificationRecipientType.SHIPPER;
                recipient.EMailAddress = fedex.Shipment.OriginEmail;

                ApplyEmailRecipientOptions(recipient, fedex.EmailNotifySender);
            }

            // See if any are being sent to the recipient
            if (notifyRecipient)
            {
                EMailNotificationRecipient recipient = new EMailNotificationRecipient();
                recipients.Add(recipient);

                recipient.Format = EMailNotificationFormatType.HTML;
                recipient.Localization = new Localization { LanguageCode = "EN" };

                recipient.EMailNotificationRecipientType = EMailNotificationRecipientType.RECIPIENT;
                recipient.EMailAddress = fedex.Shipment.ShipEmail;

                ApplyEmailRecipientOptions(recipient, fedex.EmailNotifyRecipient);
            }

            // See if any are being sent to other
            if (notifyOther)
            {
                EMailNotificationRecipient recipient = new EMailNotificationRecipient();
                recipients.Add(recipient);

                recipient.Format = EMailNotificationFormatType.HTML;
                recipient.Localization = new Localization { LanguageCode = "EN" };

                recipient.EMailNotificationRecipientType = EMailNotificationRecipientType.OTHER;
                recipient.EMailAddress = fedex.EmailNotifyOtherAddress;

                ApplyEmailRecipientOptions(recipient, fedex.EmailNotifyOther);
            }

            emailDetail.Recipients = recipients.ToArray();
        }

        /// <summary>
        /// Apply the given notification types to the recipient
        /// </summary>
        private static void ApplyEmailRecipientOptions(EMailNotificationRecipient recipient, int notifcationTypes)
        {
            recipient.NotifyOnShipment = (notifcationTypes & (int) FedExEmailNotificationType.Ship) != 0;
            recipient.NotifyOnShipmentSpecified = true;

            recipient.NotifyOnException = (notifcationTypes & (int) FedExEmailNotificationType.Exception) != 0;
            recipient.NotifyOnExceptionSpecified = true;

            recipient.NotifyOnDelivery = (notifcationTypes & (int) FedExEmailNotificationType.Deliver) != 0;
            recipient.NotifyOnDeliverySpecified = true;
        }

        /// <summary>
        /// Apply any freight options to the shipment
        /// </summary>
        private static void ApplyFreightOptions(RequestedShipment detail, List<ShipmentSpecialServiceType> specialServiceTypes, FedExShipmentEntity fedex)
        {
            if (!FedExUtility.IsFreightService((FedExServiceType) fedex.Service))
            {
                return;
            }

            detail.ExpressFreightDetail = new ExpressFreightDetail();
            detail.ExpressFreightDetail.BookingConfirmationNumber = fedex.FreightBookingNumber;

            if (fedex.Shipment.ShipCountryCode == "US")
            {
                if (fedex.FreightInsideDelivery)
                {
                    specialServiceTypes.Add(ShipmentSpecialServiceType.INSIDE_DELIVERY);
                }

                if (fedex.FreightInsidePickup)
                {
                    specialServiceTypes.Add(ShipmentSpecialServiceType.INSIDE_PICKUP);
                }
            }
            else
            {
                detail.ExpressFreightDetail.ShippersLoadAndCount = fedex.FreightLoadAndCount.ToString();
            }
        }

        /// <summary>
        /// Apply the international broker options
        /// </summary>
        private static void ApplyBrokerOptions(RequestedShipment shipDetail, List<ShipmentSpecialServiceType> specialServices, FedExShipmentEntity fedex)
        {
            if (!fedex.BrokerEnabled)
            {
                return;
            }

            specialServices.Add(ShipmentSpecialServiceType.BROKER_SELECT_OPTION);

            PersonAdapter person = new PersonAdapter(fedex, "Broker");

            shipDetail.InternationalDetail.Broker = new Party();
            shipDetail.InternationalDetail.Broker.AccountNumber = fedex.BrokerAccount;
            shipDetail.InternationalDetail.Broker.Address = FedExApiCore.CreateAddress<Address>(person);
            shipDetail.InternationalDetail.Broker.Contact = FedExApiCore.CreateContact<Contact>(person);
        }

        /// <summary>
        /// Apply the international commercial invoice options
        /// </summary>
        private static void ApplyCommercialInvoiceOptions(RequestedShipment shipDetail, FedExShipmentEntity fedex)
        {
            if (!fedex.CommercialInvoice)
            {
                return;
            }

            CommercialInvoice ci = new CommercialInvoice();
            shipDetail.InternationalDetail.CommercialInvoice = ci;

            ci.TermsOfSale = GetApiTermsOfSale((FedExTermsOfSale) fedex.CommercialInvoiceTermsOfSale);
            ci.TermsOfSaleSpecified = true;

            ci.Purpose = GetApiCommercialInvoicePurpose((FedExCommercialInvoicePurpose) fedex.CommercialInvoicePurpose);
            ci.PurposeSpecified = true;

            ci.Comments = new string[] { fedex.CommercialInvoiceComments };

            ci.FreightCharge = new Money { Amount = fedex.CommercialInvoiceFreight, Currency = "USD" };
            ci.TaxesOrMiscellaneousCharge = new Money { Amount = fedex.CommercialInvoiceOther, Currency = "USD" };

            shipDetail.InternationalDetail.InsuranceCharges =  new Money { Amount = fedex.CommercialInvoiceInsurance, Currency = "USD" };

            if (fedex.ImporterOfRecord)
            {
                Party ior = new Party();
                shipDetail.InternationalDetail.ImporterOfRecord = ior;

                PersonAdapter importerPerson = new PersonAdapter(fedex, "Importer");

                ior.Address = FedExApiCore.CreateAddress<Address>(importerPerson);
                ior.Contact = FedExApiCore.CreateContact<Contact>(importerPerson);
                ior.AccountNumber = fedex.ImporterAccount;
                ior.Tin = new TaxpayerIdentification { Number = fedex.ImporterTIN, TinType = TinType.EIN };
            }
        }

        /// <summary>
        /// Get the label specification info for the given shipment
        /// </summary>
        private static LabelSpecification GetLabelSpecification(FedExShipmentEntity fedex)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            LabelSpecification label = new LabelSpecification();
            label.LabelFormatType = LabelFormatType.COMMON2D;

            if (settings.FedExMaskAccount)
            {
                CustomerSpecifiedLabelDetail detail = new CustomerSpecifiedLabelDetail();
                detail.MaskedData = new LabelMaskableDataType[] { LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER };

                label.CustomerSpecifiedDetail = detail;
            }

            // Have to record if we downloaded a standard or thermal
            if (settings.FedExThermal)
            {
                fedex.Shipment.ThermalType = settings.FedExThermalType;
            }
            else
            {
                fedex.Shipment.ThermalType = null;
            }

            if (settings.FedExThermal)
            {
                // Thermal type
                label.ImageType = GetApiThermalType((ThermalLanguage) settings.FedExThermalType);

                // Has a doc-tab
                if (settings.FedExThermalDocTab)
                {
                    if (settings.FedExThermalDocTabType == (int) ThermalDocTabType.Leading)
                    {
                        label.LabelStockType = LabelStockType.STOCK_4X675_LEADING_DOC_TAB;
                    }
                    else
                    {
                        label.LabelStockType = LabelStockType.STOCK_4X675_TRAILING_DOC_TAB;
                    }
                }
                else
                {
                    label.LabelStockType = LabelStockType.STOCK_4X6;
                }
            }
            else
            {
                label.ImageType = ShippingDocumentImageType.PNG;
                label.LabelStockType = LabelStockType.PAPER_4X6;
            }

            label.ImageTypeSpecified = true;
            label.LabelStockTypeSpecified = true;

            return label;
        }

        /// <summary>
        /// Get the payment detail info for the given shipment
        /// </summary>
        private static Payment GetPaymentDetail(int payorType, string payorAcccount, string payorCountryCode, FedExAccountEntity account)
        {
            Payment payment = new Payment();

            payment.PaymentType = GetApiPaymentType((FedExPayorType) payorType);

            if (payment.PaymentType != PaymentType.COLLECT)
            {
                payment.Payor = new Payor();
                if (payment.PaymentType == PaymentType.SENDER)
                {
                    payment.Payor.AccountNumber = account.AccountNumber;
                    payment.Payor.CountryCode = account.CountryCode;
                }
                else
                {
                    payment.Payor.AccountNumber = payorAcccount;
                    payment.Payor.CountryCode = payorCountryCode;
                }
            }

            return payment;
        }

        /// <summary>
        /// Get the FedEx API value for our internal thermal type
        /// </summary>
        private static ShippingDocumentImageType GetApiThermalType(ThermalLanguage thermalType)
        {
            switch (thermalType)
            {
                case ThermalLanguage.EPL: return ShippingDocumentImageType.EPL2;
                case ThermalLanguage.ZPL: return ShippingDocumentImageType.ZPLII;
            }

            throw new InvalidOperationException("Invalid FedEx thermal type " + thermalType);
        }

        /// <summary>
        /// Get the FedEx API value for the given payor type
        /// </summary>
        private static PaymentType GetApiPaymentType(FedExPayorType payorType)
        {
            switch (payorType)
            {
                case FedExPayorType.Sender: return PaymentType.SENDER;
                case FedExPayorType.Recipient: return PaymentType.RECIPIENT;
                case FedExPayorType.ThirdParty: return PaymentType.THIRD_PARTY;
                case FedExPayorType.Collect: return PaymentType.COLLECT;
            }

            throw new InvalidOperationException("Invalid FedEx payor type " + payorType);
        }

        /// <summary>
        /// Get the API value for our internal signature type
        /// </summary>
        private static SignatureOptionType GetApiSignatureType(FedExSignatureType signature)
        {
            switch (signature)
            {
                case FedExSignatureType.Indirect: return SignatureOptionType.INDIRECT;
                case FedExSignatureType.Direct: return SignatureOptionType.DIRECT;
                case FedExSignatureType.Adult: return SignatureOptionType.ADULT;
                case FedExSignatureType.NoSignature: return SignatureOptionType.NO_SIGNATURE_REQUIRED;
            }

            return SignatureOptionType.SERVICE_DEFAULT;
        }

        /// <summary>
        /// Get the API service type based on our internal value
        /// </summary>
        private static ServiceType GetApiServiceType(FedExServiceType serviceType)
        {
            switch (serviceType)
            {
                case FedExServiceType.PriorityOvernight: return ServiceType.PRIORITY_OVERNIGHT;
                case FedExServiceType.StandardOvernight: return ServiceType.STANDARD_OVERNIGHT;
                case FedExServiceType.FirstOvernight: return ServiceType.FIRST_OVERNIGHT;
                case FedExServiceType.FedEx2Day: return ServiceType.FEDEX_2_DAY;
                case FedExServiceType.FedExExpressSaver: return ServiceType.FEDEX_EXPRESS_SAVER;
                case FedExServiceType.InternationalPriority: return ServiceType.INTERNATIONAL_PRIORITY;
                case FedExServiceType.InternationalEconomy: return ServiceType.INTERNATIONAL_ECONOMY;
                case FedExServiceType.InternationalFirst: return ServiceType.INTERNATIONAL_FIRST;
                case FedExServiceType.FedEx1DayFreight: return ServiceType.FEDEX_1_DAY_FREIGHT;
                case FedExServiceType.FedEx2DayFreight: return ServiceType.FEDEX_2_DAY_FREIGHT;
                case FedExServiceType.FedEx3DayFreight: return ServiceType.FEDEX_3_DAY_FREIGHT;
                case FedExServiceType.FedExGround: return ServiceType.FEDEX_GROUND;
                case FedExServiceType.GroundHomeDelivery: return ServiceType.GROUND_HOME_DELIVERY;
                case FedExServiceType.InternationalPriorityFreight: return ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                case FedExServiceType.InternationalEconomyFreight: return ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                case FedExServiceType.SmartPost: return ServiceType.SMART_POST;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + serviceType);
        }

        /// <summary>
        /// Determine the ship service packaging type
        /// </summary>
        private static PackagingType GetApiPackagingType(FedExPackagingType packagingType)
        {
            switch (packagingType)
            {
                case FedExPackagingType.Box: return PackagingType.FEDEX_BOX;
                case FedExPackagingType.Box10Kg: return PackagingType.FEDEX_10KG_BOX;
                case FedExPackagingType.Box25Kg: return PackagingType.FEDEX_25KG_BOX;
                case FedExPackagingType.Custom: return PackagingType.YOUR_PACKAGING;
                case FedExPackagingType.Envelope: return PackagingType.FEDEX_ENVELOPE;
                case FedExPackagingType.Pak: return PackagingType.FEDEX_PAK;
                case FedExPackagingType.Tube: return PackagingType.FEDEX_TUBE;
            }

            throw new InvalidOperationException("Invalid FedEx Packaging Type");
        }

        /// <summary>
        /// Determine the API value to use for our COD payment type
        /// </summary>
        private static CodCollectionType GetApiCodCollectionType(FedExCodPaymentType paymentType)
        {
            switch (paymentType)
            {
                case FedExCodPaymentType.Any: return CodCollectionType.ANY;
                case FedExCodPaymentType.Secured: return CodCollectionType.GUARANTEED_FUNDS;
                case FedExCodPaymentType.Unsecured: return CodCollectionType.CASH;
            }

            throw new InvalidOperationException("Invalid FedEx payment type: " + paymentType);
        }

        /// <summary>
        /// Determine the API value corresponding to our internal type
        /// </summary>
        private static PhysicalPackagingType GetApiAdmissibilityPackagingType(FedExPhysicalPackagingType type)
        {
            switch (type)
            {
                case FedExPhysicalPackagingType.Bag: return PhysicalPackagingType.BAG;
                case FedExPhysicalPackagingType.Barrel: return PhysicalPackagingType.BARREL;
                case FedExPhysicalPackagingType.BasketOrHamper: return PhysicalPackagingType.BASKET;
                case FedExPhysicalPackagingType.Box: return PhysicalPackagingType.BOX;
                case FedExPhysicalPackagingType.Bucket: return PhysicalPackagingType.BUCKET;
                case FedExPhysicalPackagingType.Bundle: return PhysicalPackagingType.BUNDLE;
                case FedExPhysicalPackagingType.Carton: return PhysicalPackagingType.CARTON;
                case FedExPhysicalPackagingType.Case: return PhysicalPackagingType.CASE;
                case FedExPhysicalPackagingType.Container: return PhysicalPackagingType.CONTAINER;
                case FedExPhysicalPackagingType.Crate: return PhysicalPackagingType.CRATE;
                case FedExPhysicalPackagingType.Cylinder: return PhysicalPackagingType.CYLINDER;
                case FedExPhysicalPackagingType.Drum: return PhysicalPackagingType.DRUM;
                case FedExPhysicalPackagingType.Envelope: return PhysicalPackagingType.ENVELOPE;
                case FedExPhysicalPackagingType.Pail: return PhysicalPackagingType.PAIL;
                case FedExPhysicalPackagingType.Pallet: return PhysicalPackagingType.PALLET;
                case FedExPhysicalPackagingType.Pieces: return PhysicalPackagingType.PIECE;
                case FedExPhysicalPackagingType.Reel: return PhysicalPackagingType.REEL;
                case FedExPhysicalPackagingType.Roll: return PhysicalPackagingType.ROLL;
                case FedExPhysicalPackagingType.Skid: return PhysicalPackagingType.SKID;
                case FedExPhysicalPackagingType.Tank: return PhysicalPackagingType.TANK;
                case FedExPhysicalPackagingType.Tube: return PhysicalPackagingType.TUBE;
            }

            throw new InvalidOperationException("Invalid FedEx Admissibility Type: " + type);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private static PurposeOfShipmentType GetApiCommercialInvoicePurpose(FedExCommercialInvoicePurpose purpose)
        {
            switch (purpose)
            {
                case FedExCommercialInvoicePurpose.Sold: return PurposeOfShipmentType.SOLD;
                case FedExCommercialInvoicePurpose.NotSold: return PurposeOfShipmentType.NOT_SOLD;
                case FedExCommercialInvoicePurpose.Gift: return PurposeOfShipmentType.GIFT;
                case FedExCommercialInvoicePurpose.Sample: return PurposeOfShipmentType.SAMPLE;
                case FedExCommercialInvoicePurpose.Personal: return PurposeOfShipmentType.PERSONAL_EFFECTS;
                case FedExCommercialInvoicePurpose.Repair: return PurposeOfShipmentType.REPAIR_AND_RETURN;
            }

            throw new InvalidOperationException("Invalid FedEx Commercial Invoice Purpose: " + purpose);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private static TermsOfSaleType GetApiTermsOfSale(FedExTermsOfSale termsOfSale)
        {
            switch (termsOfSale)
            {
                case FedExTermsOfSale.FOB_or_FCA: return TermsOfSaleType.FOB_OR_FCA;
                case FedExTermsOfSale.CFR_or_CPT: return TermsOfSaleType.CFR_OR_CPT;
                case FedExTermsOfSale.CIF_or_CIP: return TermsOfSaleType.CIF_OR_CIP;
                case FedExTermsOfSale.EXW: return TermsOfSaleType.EXW;
                case FedExTermsOfSale.DDP: return TermsOfSaleType.DDP;
                case FedExTermsOfSale.DDU: return TermsOfSaleType.DDU;
            }

            throw new InvalidOperationException("Invalid FedEx TermsOfSale: " + termsOfSale);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal vlaue
        /// </summary>
        private static SmartPostAncillaryEndorsementType? GetSmartPostEndorsementType(FedExSmartPostEndorsement endorsement)
        {
            switch (endorsement)
            {
                case FedExSmartPostEndorsement.AddressCorrection: return SmartPostAncillaryEndorsementType.ADDRESS_CORRECTION;
                case FedExSmartPostEndorsement.ChangeService: return SmartPostAncillaryEndorsementType.CHANGE_SERVICE;
                case FedExSmartPostEndorsement.ForwardingService: return SmartPostAncillaryEndorsementType.FORWARDING_SERVICE;
                case FedExSmartPostEndorsement.LeaveIfNoResponse: return SmartPostAncillaryEndorsementType.CARRIER_LEAVE_IF_NO_RESPONSE;
                case FedExSmartPostEndorsement.ReturnService: return SmartPostAncillaryEndorsementType.RETURN_SERVICE;
                case FedExSmartPostEndorsement.None: return null;
            }

            throw new InvalidOperationException("Invalid endorsement value: " + endorsement);
        }

        /// <summary>
        /// Get the FedEx API value that correspons to our internal value
        /// </summary>
        private static SmartPostIndiciaType GetSmartPostIndiciaType(FedExSmartPostIndicia indicia)
        {
            switch (indicia)
            {
                case FedExSmartPostIndicia.ParcelSelect: return SmartPostIndiciaType.PARCEL_SELECT;
                case FedExSmartPostIndicia.MediaMail: return SmartPostIndiciaType.MEDIA_MAIL;
                case FedExSmartPostIndicia.BoundPrintedMatter: return SmartPostIndiciaType.PRESORTED_BOUND_PRINTED_MATTER;
                case FedExSmartPostIndicia.PresortedStandard: return SmartPostIndiciaType.PRESORTED_STANDARD;
            }

            throw new InvalidOperationException("Invalid indicia type: " + indicia);
        }
    }
}
