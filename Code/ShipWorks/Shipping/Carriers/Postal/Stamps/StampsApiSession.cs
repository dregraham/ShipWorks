using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using Interapptive.Shared.Utility;
using System.Web.Services.Protocols;
using System.Globalization;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;
using Interapptive.Shared.Net;
using System.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using System.Diagnostics;
using ShipWorks.Data.Connection;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ShipWorks.UI;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Business;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using log4net;
using ShipWorks.Shipping.Settings;
using ShipWorks.Templates.Tokens;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Central point where API stuff goes through for stamps.com
    /// </summary>
    public static class StampsApiSession
    {
        static readonly ILog log = LogManager.GetLogger(typeof(StampsApiSession));

        static string productionUrl = "https://swsim.stamps.com/swsim/SwsimV26.asmx";
        static Guid integrationID = new Guid("F784C8BC-9CAD-4DAF-B320-6F9F86090032");

        // Maps stamps.com usernames to their latest authenticator tokens
        static Dictionary<string, string> usernameAuthenticatorMap = new Dictionary<string, string>();

        // Maps stamps.com usernames to the object lock used to make sure only one thread is trying to authenticate at a time
        static Dictionary<string, object> authenticationLockMap = new Dictionary<string, object>();

        // Cleansed address map so we don't do common addresses over and over again
        static Dictionary<PersonAdapter, Address> cleansedAddressMap = new Dictionary<PersonAdapter, Address>();

        /// <summary>
        /// Indicates if the test server should be used instead of hte live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("StampsTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("StampsTestServer", value); }
        }

        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static SwsimV26 CreateWebService(string logName)
        {
            SwsimV26 webService = new SwsimV26(new ApiLogEntry(ApiLogSource.UspsStamps, logName));
            webService.Url = productionUrl;

            return webService;
        }

        /// <summary>
        /// Authenticate the given user with Stamps.com.  If 
        /// </summary>
        public static void AuthenticateUser(string username, string password)
        {
            try
            {
                // Output parameters from stamps.com
                DateTime lastLoginTime = new DateTime();
                bool clearCredential = false;
                string bannerText = string.Empty;
                bool passwordExpired = false;

                using (SwsimV26 webService = CreateWebService("Authenticate"))
                {
                    string auth = webService.AuthenticateUser(new Credentials
                    {
                        IntegrationID = integrationID,
                        Username = username,
                        Password = password
                    }, out lastLoginTime, out clearCredential, out bannerText, out passwordExpired);

                    usernameAuthenticatorMap[username] = auth;
                }
            }
            catch (SoapException ex)
            {
                throw new StampsApiException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(StampsException));
            }
        }

        /// <summary>
        /// Get the account info for the given Stamps.com username
        /// </summary>
        public static AccountInfo GetAccountInfo(StampsAccountEntity account)
        {
            return AuthenticationWrapper(() => { return GetAccountInfoInternal(account); }, account);
        }

        /// <summary>
        /// The internal GetAccountInfo implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private static AccountInfo GetAccountInfoInternal(StampsAccountEntity account)
        {
            AccountInfo accountInfo;
            Address address;
            string email;

            using (SwsimV26 webService = CreateWebService("GetAccountInfo"))
            {
                string auth = webService.GetAccountInfo(GetAuthenticator(account), out accountInfo, out address, out email);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            return accountInfo;
        }

        /// <summary>
        /// Get the Stamps.com URL of the given urlType
        /// </summary>
        public static string GetUrl(StampsAccountEntity account, UrlType urlType)
        {
            return AuthenticationWrapper(() => { return GetUrlInternal(account, urlType); }, account);
        }

        /// <summary>
        /// The internal GetUrl implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private static string GetUrlInternal(StampsAccountEntity account, UrlType urlType)
        {
            string url;

            using (SwsimV26 webService = CreateWebService("GetURL"))
            {
                string auth = webService.GetURL(GetAuthenticator(account), urlType, string.Empty, out url);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            return url;
        }

        /// <summary>
        /// Purchase postage for the given account for the specified amount.  ControlTotal is the ControlTotal value last retrieved from GetAccountInfo.
        /// </summary>
        public static void PurchasePostage(StampsAccountEntity account, decimal amount, decimal controlTotal)
        {
            AuthenticationWrapper(() => { PurchasePostageInternal(account, amount, controlTotal); return true; }, account);
        }

        /// <summary>
        /// The internal PurchasePostageInternal implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private static void PurchasePostageInternal(StampsAccountEntity account, decimal amount, decimal controlTotal)
        {
            PurchaseStatus purchaseStatus;
            int transactionID;
            PostageBalance postageBalance;
            string rejectionReason;

            bool miRequired_Unused;

            using (SwsimV26 webService = CreateWebService("PurchasePostage"))
            {
                string auth = webService.PurchasePostage(GetAuthenticator(account), amount, controlTotal, null, null, out purchaseStatus, out transactionID, out postageBalance, out rejectionReason, out miRequired_Unused);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            if (purchaseStatus == PurchaseStatus.Rejected)
            {
                throw new StampsException(rejectionReason);
            }
        }

        /// <summary>
        /// Get the rates for the given shipment based on its settings
        /// </summary>
        public static List<RateV10> GetRates(ShipmentEntity shipment)
        {
            StampsAccountEntity account = StampsAccountManager.GetAccount(shipment.Postal.Stamps.StampsAccountID);
            if (account == null)
            {
                throw new StampsException("No Stamps.com account is selected for the shipment.");
            }

            return AuthenticationWrapper(() => { return GetRatesInternal(shipment, account); }, account);
        }

        /// <summary>
        /// The internal GetRates implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private static List<RateV10> GetRatesInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            RateV10 rate = CreateRateForRating(shipment, account);

            List<RateV10> rateResults = new List<RateV10>();

            using (SwsimV26 webService = CreateWebService("GetRates"))
            {
                RateV10[] ratesArray;

                string auth = webService.GetRates(GetAuthenticator(account), rate, out ratesArray);
                usernameAuthenticatorMap[account.Username] = auth;

                rateResults = ratesArray.ToList();
            }

            List<RateV10> noConfirmationServiceRates = new List<RateV10>();

            // If its a "Flat" then FirstClass and Priority can't have a confirmation
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;
            if (packagingType == PostalPackagingType.Envelope || packagingType == PostalPackagingType.LargeEnvelope)
            {
                noConfirmationServiceRates.AddRange(rateResults.Where(r => r.ServiceType == ServiceType.USFC || r.ServiceType == ServiceType.USPM));
            }

            // Remove the Delivery and Signature add ons from all those that shouldnt support it
            foreach (RateV10 noConfirmationServiceRate in noConfirmationServiceRates)
            {
                if (noConfirmationServiceRate != null && noConfirmationServiceRate.AddOns != null)
                {
                    noConfirmationServiceRate.AddOns = noConfirmationServiceRate.AddOns.Where(a => a.AddOnType != AddOnTypeV3.USASC && a.AddOnType != AddOnTypeV3.USADC).ToArray();
                }
            }

            return rateResults;
        }

        /// <summary>
        /// Cleans the address of the given person using the specified stamps account
        /// </summary>
        public static Address CleanseAddress(StampsAccountEntity account, PersonAdapter person, bool requireFullMatch)
        {
            return AuthenticationWrapper(() => { return CleanseAddressInternal(person, account, requireFullMatch); }, account);
        }

        /// <summary>
        /// Internal CleanseAddress implementation intended to be warpped by the auth wrapper
        /// </summary>
        private static Address CleanseAddressInternal(PersonAdapter person, StampsAccountEntity account, bool requireFullMatch)
        {
            Address address;
            if (cleansedAddressMap.TryGetValue(person, out address))
            {
                return address;
            }

            address = CreateAddress(person);

            bool addressMatch;
            bool cityStateZipOK;
            ResidentialDeliveryIndicatorType residentialIndicator;
            bool? isPoBox;
            bool isPoBoxSpecified;
            Address[] candidates;
            StatusCodes statusCodes;

            using (SwsimV26 webService = CreateWebService("CleanseAddress"))
            {
                string auth = webService.CleanseAddress(GetAuthenticator(account), ref address, out addressMatch, out cityStateZipOK, out residentialIndicator, out isPoBox, out isPoBoxSpecified, out candidates, out statusCodes);
                usernameAuthenticatorMap[account.Username] = auth;

                if (!addressMatch)
                {
                    if (!cityStateZipOK)
                    {
                        throw new StampsException(string.Format("The address for '{0}' is not a valid address.", new PersonName(person).FullName));
                    }
                    else if (requireFullMatch)
                    {
                        throw new StampsException(string.Format("The city, state, and postal code for '{0}' is valid, but the full address is not.", new PersonName(person).FullName));
                    }
                }
            }

            cleansedAddressMap[person] = address;

            return address;
        }

        /// <summary>
        /// Registers a new account with Stamps.com.
        /// </summary>
        public static StampsRegistrationResult RegisterAccount(StampsRegistration registration)
        {
            // Output parameters supplied to the request
            string suggestedUserName = string.Empty;
            int userId = 0;
            string promoUrl = string.Empty;

            try
            {
                RegistrationStatus registrationStatus = RegistrationStatus.Fail;

                using (SwsimV26 webService = CreateWebService("RegisterAccount"))
                {
                    // Note: API docs say the address must be cleansed prior to registering the account, but the API 
                    // for cleansing an address assumes there are existing credentials. Question is out to Stamps.com 
                    // on this, but haven't heard anything back as of 11/8/2012.
                    registrationStatus = webService.RegisterAccount
                        (
                            integrationID,
                            registration.UserName,
                            registration.Password,
                            registration.FirstCodewordType,
                            registration.FirstCodewordValue,
                            registration.SecondCodewordType,
                            registration.SecondCodewordValue,
                            registration.PhysicalAddress,
                            null,
                            registration.MachineInfo,
                            registration.Email,
                            registration.AccountType,
                            registration.PromoCode,
                            (object) registration.CreditCard ?? registration.AchAccount,
                            out suggestedUserName,
                            out userId,
                            out promoUrl
                        );
                }

                return new StampsRegistrationResult(registrationStatus, suggestedUserName, promoUrl);
            }
            catch (Exception ex)
            {
                throw new StampsRegistrationException("Stamps.com encountered an error trying to register your account:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates the scan form.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="stampsAccountEntity">The stamps account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to 
        /// identify results from Stamps.com</returns>
        public static XDocument CreateScanForm(IEnumerable<StampsShipmentEntity> shipments, StampsAccountEntity stampsAccountEntity)
        {
            if (stampsAccountEntity == null)
            {
                throw new StampsException("No Stamps.com account is selected for the SCAN form.");
            }

            XDocument result = new XDocument();
            AuthenticationWrapper(() => { result = CreateScanFormInternal(shipments, stampsAccountEntity); return true; }, stampsAccountEntity);

            return result;
        }

        /// <summary>
        /// Creates the scan form via the Stamps.com API and intended to be wrapped by the authentication wrapper.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="stampsAccountEntity">The stamps account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to 
        /// identify results from Stamps.com</returns>
        private static XDocument CreateScanFormInternal(IEnumerable<StampsShipmentEntity> shipments, StampsAccountEntity stampsAccountEntity)
        {
            List<Guid> stampsTransactions = shipments.Select(s => s.StampsTransactionID).ToList();
            PersonAdapter person = new PersonAdapter(stampsAccountEntity, string.Empty);

            string scanFormStampsId = string.Empty;
            string scanFormUrl = string.Empty;

            using (SwsimV26 webService = CreateWebService("ScanForm"))
            {
                webService.CreateScanForm
                (
                    GetAuthenticator(stampsAccountEntity),
                    stampsTransactions.ToArray(),
                    CreateScanFormAddress(person),
                    ImageType.Png,
                    false, // Don't print instructions
                    null,
                    false,
                    out scanFormStampsId,
                    out scanFormUrl
                );
            }

            if (scanFormUrl.Contains(" "))
            {
                // According to the docs, there is a chance that there could be multiple URLs; the first
                // URL contains the actual SCAN form though
                scanFormUrl = scanFormUrl.Split(new char[] { ' ' })[0];
            }

            string responseXml = string.Format("<ScanForm><TransactionId>{0}</TransactionId><Url>{1}</Url></ScanForm>", scanFormStampsId, scanFormUrl);
            XDocument response = XDocument.Parse(responseXml);

            return response;
        }

        /// <summary>
        /// Void the given already processed shipment
        /// </summary>
        public static void VoidShipment(ShipmentEntity shipment)
        {
            StampsAccountEntity account = StampsAccountManager.GetAccount(shipment.Postal.Stamps.StampsAccountID);
            if (account == null)
            {
                throw new StampsException("No Stamps.com account is selected for the shipment.");
            }

            AuthenticationWrapper(() => { VoidShipmentInternal(shipment, account); return true; }, account);
        }

        /// <summary>
        /// The internal VoidShipment implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private static void VoidShipmentInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            using (SwsimV26 webService = CreateWebService("Void"))
            {
                string auth = webService.CancelIndicium(GetAuthenticator(account), shipment.Postal.Stamps.StampsTransactionID);
                usernameAuthenticatorMap[account.Username] = auth;
            }
        }

        /// <summary>
        /// Process the given shipment, downloading label images and tracking information
        /// </summary>
        public static void ProcessShipment(ShipmentEntity shipment)
        {
            StampsAccountEntity account = StampsAccountManager.GetAccount(shipment.Postal.Stamps.StampsAccountID);
            if (account == null)
            {
                throw new StampsException("No Stamps.com account is selected for the shipment.");
            }

            AuthenticationWrapper(() => { ProcessShipmentInternal(shipment, account); return true; }, account);
        }

        /// <summary>
        /// The internal ProcessShipment implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private static void ProcessShipmentInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            Guid stampsGuid;
            string tracking = string.Empty;
            string labelUrl;

            Address fromAddress = CleanseAddress(account, new PersonAdapter(shipment, "Origin"), false);
            Address toAddress = CleanseAddress(account, new PersonAdapter(shipment, "Ship"), shipment.Postal.Stamps.RequireFullAddressValidation);

            RateV10 rate = CreateRateForProcessing(shipment, account);
            Customs customs = CreateCustoms(shipment);
            PostageBalance postageBalance;
            string memo = StringUtility.Truncate(TemplateTokenProcessor.ProcessTokens(shipment.Postal.Stamps.Memo, shipment.ShipmentID), 200);

            // Stamps requires that the address in the Rate match that of the request.  Makes sense - but could be different if they auto-cleansed the address.
            rate.ToState = toAddress.State;
            rate.ToZIPCode = toAddress.ZIPCode;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            ThermalLabelType? thermalType = settings.StampsThermal ? (ThermalLabelType)settings.StampsThermalType : (ThermalLabelType?)null;

            // However, Stamps.com doesn't currently support thermal internationals
            if (!PostalUtility.IsDomesticCountry(shipment.ShipCountryCode) || PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
            {
                thermalType = null;
            }

            // Each request needs to get a new requestID.  If Stamps.com sees a duplicate, it thinks its the same request.  
            // So if you had an error (like weight was too much) and then changed the weight and resubmitted, it would still 
            // be in error if you used the same ID again.
            shipment.Postal.Stamps.IntegratorTransactionID = Guid.NewGuid();
            string integratorGuid = shipment.Postal.Stamps.IntegratorTransactionID.ToString();

            string mac_Unused;
            string postageHash;

            if (shipment.Postal.PackagingType == (int)PostalPackagingType.Envelope)
            {
                // Envelopes don't support thermal
                thermalType = null;

                // A separate service call is used for processing envelope according to Stamps.com as of v. 22
                using (SwsimV26 webService = CreateWebService("Process"))
                {
                    // Always use the personal envelope layout to generate the envelope label
                    rate.PrintLayout = "EnvelopePersonal";

                    string envelopeAuth = webService.CreateEnvelopeIndicium(GetAuthenticator(account), ref integratorGuid,
                        ref rate,
                        fromAddress,
                        toAddress,
                        null,
                        UseTestServer ? CreateIndiciumModeV1.Sample : CreateIndiciumModeV1.Normal,
                        ImageType.Png,
                        0, // cost code ID
                        false, // do not hide the facing identification mark (FIM) 
                        out tracking,
                        out stampsGuid,
                        out labelUrl,
                        out postageBalance,
                        out mac_Unused,
                        out postageHash);

                    usernameAuthenticatorMap[account.Username] = envelopeAuth;
                }
            }
            else
            {
                // Labels for all other package types other than envelope get created via the CreateIndicium method
                using (SwsimV26 webService = CreateWebService("Process"))
                {
                    string auth = webService.CreateIndicium(GetAuthenticator(account), ref integratorGuid,
                        ref tracking,
                        ref rate,
                        fromAddress,
                        toAddress,
                        null,
                        customs,
                        UseTestServer,
                        thermalType == null ? ImageType.Png : ((thermalType == ThermalLabelType.EPL) ? ImageType.Epl : ImageType.Zpl),
                        EltronPrinterDPIType.Default,
                        memo, // Memo
                        0, // Cost Code
                        "", // recipient email
                        false, // delivery notify
                        false, // shipment notify
                        false, // shipment notify cc to main
                        false, // shipment notify from company
                        false, // shipment notify company in subject
                        0, // Rotation
                        null, false, // horizontal offset
                        null, false, // vertical offset
                        null, false, // print density
                        null, false, // print memo 
                        null, false, // print instructions
                        false, // request postage hash
                        NonDeliveryOption.Return, // return to sender
                        null, // OriginalPostageHash 
                        out stampsGuid,
                        out labelUrl,
                        out postageBalance,
                        out mac_Unused,
                        out postageHash);

                    usernameAuthenticatorMap[account.Username] = auth;
                }
            }

            shipment.TrackingNumber = tracking;
            shipment.ShipmentCost = rate.Amount + (rate.AddOns != null ? rate.AddOns.Sum(a => a.Amount) : 0);
            shipment.Postal.Stamps.StampsTransactionID = stampsGuid;

            // Set the thermal type for the shipment
            shipment.ThermalType = (int?)thermalType;

            // Interapptive users have an Unprocess button.  If we are reprocessing we need to clear the old images
            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

            string[] labelUrls = labelUrl.Split(' ');

            SaveLabelImages(shipment, labelUrls);
        }

        /// <summary>
        /// Save the label images for each url
        /// </summary>
        private static void SaveLabelImages(ShipmentEntity shipment, string[] labelUrls)
        {
            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;

            // Domestic
            if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
            {
                // For APO/FPO, the customs docs come in the next two images
                if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
                {
                    // They come down different depending on form type
                    if (PostalUtility.GetCustomsForm(shipment) == PostalCustomsForm.CN72)
                    {
                        SaveLabelImage(shipment, labelUrls[0], "LabelPrimary", new Rectangle(49, 48, 1597, 1005));
                        SaveLabelImage(shipment, labelUrls[0], "LabelPart2", new Rectangle(49, 1078, 1597, 1005));
                        SaveLabelImage(shipment, labelUrls[1], "LabelPart3", new Rectangle(49, 48, 1597, 1005));
                        SaveLabelImage(shipment, labelUrls[1], "LabelPart4", new Rectangle(49, 1078, 1597, 1005));
                    }
                    else
                    {
                        SaveLabelImage(shipment, labelUrls[0], "LabelPrimary", new Rectangle(198, 123, 1202, 772));
                    }
                }
                else
                {
                    // First one is always the primary
                    SaveLabelImage(shipment, labelUrls[0], "LabelPrimary", Rectangle.Empty);
                }
            }
            // International servics require some trickdickery
            else
            {
                // As of now Stamps.com doesnt handle international thermals - so we arent setup to handle it. Safeguard for if we ever add it..
                if (shipment.ThermalType != null)
                {
                    throw new NotImplementedException();
                }

                // First-class labels are always a single label
                if (serviceType == PostalServiceType.InternationalFirst ||

                    // Internatioanl priority flat rate can be the same size as a first-class.  We can tell when this happens
                    // b\c we get only 2 urls (instead of 4).  the 2nd is a duplicate of the first in the cases ive seen, and we dont need it
                    (serviceType == PostalServiceType.InternationalPriority && labelUrls.Length <= 2))
                {
                    SaveLabelImage(shipment, labelUrls[0], "LabelPrimary", new Rectangle(198, 123, 1202, 772));
                }
                else
                {
                    // typical situation not including continuation pages
                    if (labelUrls.Length < 4)
                    {
                        // The first 2 images represent 4 labels that need cropped out. The 3rd url will be the instructions, that we don't need
                        SaveLabelImage(shipment, labelUrls[0], "LabelPrimary", new Rectangle(0, 0, 1600, 1010));
                        SaveLabelImage(shipment, labelUrls[0], "LabelPart2", new Rectangle(0, 1075, 1600, 1010));
                        SaveLabelImage(shipment, labelUrls[1], "LabelPart3", new Rectangle(0, 0, 1600, 1010));
                        SaveLabelImage(shipment, labelUrls[1], "LabelPart4", new Rectangle(0, 1075, 1600, 1010));
                    }
                    else
                    {
                        // there are Continuation forms to deal with.  We are going to assume there's a single continuation page
                        // last url is for instructions that aren't needed

                        // primary + continuation
                        SaveLabelImage(shipment, labelUrls[0], "LabelPrimary", new Rectangle(0, 0, 1600, 1010));
                        SaveLabelImage(shipment, labelUrls[0], "LabelPart2", new Rectangle(0, 1075, 1600, 1010));

                        // secondary + continuation
                        SaveLabelImage(shipment, labelUrls[1], "LabelPart3", new Rectangle(0, 0, 1600, 1010));
                        SaveLabelImage(shipment, labelUrls[1], "LabelPart4", new Rectangle(0, 1075, 1600, 1010));

                        // tertiary (Dispatch Note)
                        SaveLabelImage(shipment, labelUrls[2], "LabelPart5", new Rectangle(0, 0, 1600, 1010));

                        // create a blank png so that the sender's copy and continuation page are separate from the Dispatch Note
                        if (shipment.ThermalType == null)
                        {
                            using (Image blankImage = CreateBlankImage(new Rectangle(0, 0, 1600, 1010)))
                            {
                                SaveLabelImage(shipment, "LabelPartBlank", blankImage);
                            }
                        }

                        // Sender's Copy + continuation
                        SaveLabelImage(shipment, labelUrls[3], "LabelPart6", new Rectangle(0, 0, 1600, 1010));
                        SaveLabelImage(shipment, labelUrls[3], "LabelPart7", new Rectangle(0, 1075, 1600, 1010));
                    }
                }
            }
        }

        /// <summary>
        /// Creates a blank label of the size specified by the rectangle
        /// </summary>
        private static Image CreateBlankImage(Rectangle rectangle)
        {
            // Cate an image and fill it white
            Image image = new Bitmap(rectangle.Width, rectangle.Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.FillRectangle(Brushes.White, rectangle);
            }

            return image;
        }

        /// <summary>
        /// Saves a label Image to the database
        /// </summary>
        private static void SaveLabelImage(ShipmentEntity shipment, string name, Image labelImage)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                Debug.Assert(adapter.InSystemTransaction);

                using (MemoryStream imageStream = new MemoryStream())
                {
                    labelImage.Save(imageStream, ImageFormat.Png);

                    DataResourceManager.CreateFromBytes(imageStream.ToArray(), shipment.ShipmentID, name);
                }
            }
        }

        /// <summary>
        /// Save the primary shipping label image for the given shipment
        /// </summary>
        private static void SaveLabelImage(ShipmentEntity shipment, string url, string name, Rectangle crop)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                Debug.Assert(adapter.InSystemTransaction);

                // Standard images
                if (shipment.ThermalType == null)
                {
                    using (Image imageOriginal = DownloadLabelImage(url))
                    {
                        Image imageCropped;

                        // If not cropping save it as-is
                        if (crop == Rectangle.Empty)
                        {
                            imageCropped = imageOriginal;
                        }
                        else
                        {
                            // For endicia we are just cropping off the "Cut here along line", and its at the same spot on every label that needs it
                            imageCropped = DisplayHelper.CropImage(imageOriginal, crop.X, crop.Y, crop.Width, crop.Height);
                        }

                        using (MemoryStream imageStream = new MemoryStream())
                        {
                            imageCropped.Save(imageStream, ImageFormat.Png);

                            DataResourceManager.CreateFromBytes(imageStream.ToArray(), shipment.ShipmentID, name);
                        }

                        // Make sure cropped get's disposed in case we created it.  If it's the same as original it's ok, b\c Dispose is allowed to be called
                        // multiple times.
                        imageCropped.Dispose();
                    }
                }
                // Thermal image
                else
                {
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] thermalData = webClient.DownloadData(url);

                        DataResourceManager.CreateFromBytes(thermalData, shipment.ShipmentID, name);
                    }
                }
            }
        }

        /// <summary>
        /// Download the stamps.com label image from the given URL
        /// </summary>
        private static Image DownloadLabelImage(string url)
        {
            Image imageLabel;

            try
            {
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        imageLabel = Image.FromStream(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Failed processing stamps image at URL '{0}'", url), ex);

                throw;
            }

            return imageLabel;
        }

        /// <summary>
        /// Creates a scan form address (which is entirely different that the address object the rest of the API uses).
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns>A from_address_v1 object.</returns>
        private static from_address_v1 CreateScanFormAddress(PersonAdapter person)
        {
            from_address_v1 fromAddress = new from_address_v1();

            PersonName originName = new PersonName(person);
            fromAddress.name_ = originName.FullName;
            fromAddress.company_ = person.Company;

            fromAddress.address_1_ = person.Street1;
            fromAddress.address_2_ = person.Street2;

            fromAddress.city_ = person.City;

            fromAddress.state_ = person.StateProvCode;
            fromAddress.zip_code_ = person.PostalCode5;
            fromAddress.zip_code_add_on_ = person.PostalCode4;

            fromAddress.phone_number_ = person.Phone;

            return fromAddress;
        }

        /// <summary>
        /// Create a Stamps.com Address API object based on the given person address
        /// </summary>
        private static Address CreateAddress(PersonAdapter person)
        {
            Address address = new Address();

            PersonName shippingName = new PersonName(person);

            address.FullName = shippingName.FullName;
            address.Company = person.Company;

            address.Address1 = person.Street1;
            address.Address2 = person.Street2;
            address.Address3 = person.Street3;

            address.City = person.City;

            if (PostalUtility.IsDomesticCountry(person.CountryCode))
            {
                address.State = person.StateProvCode;
                address.ZIPCode = person.PostalCode5;
                address.ZIPCodeAddOn = person.PostalCode4;
            }
            else
            {
                address.Province = person.StateProvCode;
                address.PostalCode = person.PostalCode;
            }

            address.Country = AdjustCountryCode(person.CountryCode);

            if (person.CountryCode == "US")
            {
                address.PhoneNumber = person.Phone10Digits;
            }
            else
            {
                address.PhoneNumber = person.Phone;
            }

            return address;
        }

        /// <summary>
        /// Create a Rate object used as the rate info for the GetRates method
        /// </summary>
        private static RateV10 CreateRateForRating(ShipmentEntity shipment, StampsAccountEntity account)
        {
            RateV10 rate = new RateV10();

            if (!string.IsNullOrEmpty(account.MailingPostalCode))
            {
                // We have a mailing postal code defined, so we'll use this to get the rate
                rate.FromZIPCode = account.MailingPostalCode;
            }
            else
            {
                // There's no mailing postal code defined, so use the origin postal code
                rate.FromZIPCode = shipment.OriginPostalCode;
            }


            rate.ToZIPCode = shipment.ShipPostalCode;
            rate.ToCountry = AdjustCountryCode(shipment.ShipCountryCode);

            WeightValue weightValue = new WeightValue(shipment.TotalWeight);
            rate.WeightLb = weightValue.PoundsOnly;
            rate.WeightOz = weightValue.OuncesOnly;

            rate.PackageType = StampsUtility.GetApiPackageType((PostalPackagingType)shipment.Postal.PackagingType, new DimensionsAdapter(shipment.Postal));
            rate.NonMachinable = shipment.Postal.NonMachinable;

            rate.Length = shipment.Postal.DimsLength;
            rate.Width = shipment.Postal.DimsWidth;
            rate.Height = shipment.Postal.DimsHeight;

            rate.ShipDate = shipment.ShipDate;
            rate.DeclaredValue = shipment.CustomsValue;

            return rate;
        }

        /// <summary>
        /// Adjust the country code for stamps.com processing
        /// </summary>
        private static string AdjustCountryCode(string code)
        {
            // Stamps.com does not like UK.. only GB
            if (code == "UK")
            {
                code = "GB";
            }

            return code;
        }

        /// <summary>
        /// Create the rate object for the given shipment
        /// </summary>
        private static RateV10 CreateRateForProcessing(ShipmentEntity shipment, StampsAccountEntity account)
        {
            PostalServiceType serviceType = (PostalServiceType) shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType) shipment.Postal.PackagingType;

            RateV10 rate = CreateRateForRating(shipment, account);
            rate.ServiceType = StampsUtility.GetApiServiceType(serviceType);

            List<AddOnV3> addOns = new List<AddOnV3>();

            // For domestic, add in Delivery\Signature confirmation
            if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
            {
                PostalConfirmationType confirmation = (PostalConfirmationType)shipment.Postal.Confirmation;

                // If the service type is Parcel Select, Force DC, otherwise stamps throws an error
                if (confirmation == PostalConfirmationType.Delivery)
                {
                    addOns.Add(new AddOnV3 { AddOnType = AddOnTypeV3.USADC });
                }

                if (confirmation == PostalConfirmationType.Signature)
                {
                    addOns.Add(new AddOnV3 { AddOnType = AddOnTypeV3.USASC });
                }

                // Add in the hidden postage option (but not supported for envelopes)
                if (shipment.Postal.Stamps.HidePostage && shipment.Postal.PackagingType != (int) PostalPackagingType.Envelope)
                {
                    addOns.Add(new AddOnV3 { AddOnType = AddOnTypeV3.SCAHP });
                }
            }

            // Check for the new (as of 01/27/13) international delivery service.  In that case, we have to explicitly turn on DC
            else if (PostalUtility.IsFreeInternationalDeliveryConfirmation(shipment.ShipCountryCode, serviceType, packagingType))
            {
                addOns.Add(new AddOnV3 { AddOnType = AddOnTypeV3.USADC });
            }

            // For express, apply the signature waiver if necessary
            if (serviceType == PostalServiceType.ExpressMail)
            {
                if (!shipment.Postal.ExpressSignatureWaiver)
                {
                    addOns.Add(new AddOnV3 { AddOnType = AddOnTypeV3.USASR });
                }
            }

            if (addOns.Count > 0)
            {
                rate.AddOns = addOns.ToArray();
            }

            // For APO/FPO, we have to specifically ask for customs docs
            if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
            {
                rate.PrintLayout = (PostalUtility.GetCustomsForm(shipment) == PostalCustomsForm.CN72) ? "NormalCP72" : "NormalCN22";
            }

            return rate;
        }

        /// <summary>
        /// Create the customs information for the given shipment
        /// </summary>
        private static Customs CreateCustoms(ShipmentEntity shipment)
        {
            if (!CustomsManager.IsCustomsRequired(shipment))
            {
                return null;
            }

            Customs customs = new Customs();

            // Content type
            customs.ContentType = StampsUtility.GetApiContentType((PostalCustomsContentType)shipment.Postal.CustomsContentType);
            if (customs.ContentType == ContentType.Other)
            {
                if (shipment.Postal.CustomsContentType == (int)PostalCustomsContentType.Merchandise)
                {
                    customs.OtherDescribe = "Merchandise";
                }
                else
                {
                    customs.OtherDescribe = shipment.Postal.CustomsContentDescription;
                }
            }

            List<CustomsLine> lines = new List<CustomsLine>();

            // Go through each of the customs contents to create  the stamps.com custom line entity
            foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
            {
                WeightValue weightValue = new WeightValue(customsItem.Weight);

                CustomsLine line = new CustomsLine();
                line.Description = customsItem.Description;
                line.Quantity = customsItem.Quantity;
                line.Value = customsItem.UnitValue;

                line.WeightLb = weightValue.PoundsOnly;
                line.WeightOz = weightValue.OuncesOnly;

                line.HSTariffNumber = customsItem.HarmonizedCode;
                line.CountryOfOrigin = customsItem.CountryOfOrigin;

                lines.Add(line);
            }

            customs.CustomsLines = lines.ToArray();

            return customs;
        }

        /// <summary>
        /// Wraps the given executor in methods that ensure the appropriate authentication for the account
        /// </summary>
        private static T AuthenticationWrapper<T>(Func<T> executor, StampsAccountEntity account)
        {
            object authenticationLock;

            lock (authenticationLockMap)
            {
                if (!authenticationLockMap.TryGetValue(account.Username, out authenticationLock))
                {
                    authenticationLock = new object();
                    authenticationLockMap[account.Username] = authenticationLock;
                }
            }

            // We have to lockout authentication of this account to make sure only one thread is trying to authenticate at a time,
            // otherwise there will be race conditions try to get the latest authenticator.
            lock (authenticationLock)
            {
                int triesLeft = 5;

                while (true)
                {
                    triesLeft--;

                    try
                    {
                        return executor();
                    }
                    catch (SoapException ex)
                    {
                        log.ErrorFormat("Failed connecting to Stamps.com: {0}, {1}", StampsApiException.GetErrorCode(ex), ex.Message);

                        if (triesLeft > 0 && IsStaleAuthenticator(ex))
                        {
                            AuthenticateUser(account.Username, SecureText.Decrypt(account.Password, account.Username));
                        }
                        else
                        {
                            throw new StampsApiException(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw WebHelper.TranslateWebException(ex, typeof(StampsException));
                    }
                }
            }
        }

        /// <summary>
        /// Get the authenticator for the given account
        /// </summary>
        private static string GetAuthenticator(StampsAccountEntity account)
        {
            string auth;
            if (!usernameAuthenticatorMap.TryGetValue(account.Username, out auth))
            {
                AuthenticateUser(account.Username, SecureText.Decrypt(account.Password, account.Username));

                auth = usernameAuthenticatorMap[account.Username];
            }

            return auth;
        }

        /// <summary>
        /// Indicates if the exception represents an authenticator that has gone stale
        /// </summary>
        private static bool IsStaleAuthenticator(SoapException ex)
        {
            long code = StampsApiException.GetErrorCode(ex);

            switch (code)
            {
                case 0x002b0201: // Invalid
                case 0x002b0202: // Expired
                case 0x004C0105: // Expired
                case 0x00500102: // Expired
                case 0x8004E112: // Expired
                case 0x002b0203: // Invalid
                case 0x002b0204: // Out of sync
                    return true;
            }

            return false;
        }
    }
}
