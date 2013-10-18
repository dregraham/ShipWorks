using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.EquaShip.WebServices;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using log4net;
using System.Reflection;
using ShipWorks.Data;
using System.IO;
using ShipWorks.UI;
using System.Drawing;
using System.Drawing.Imaging;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.EquaShip.Enums;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Insurance;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Templates.Tokens;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Settings;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Client for EquaShip connectivity
    /// </summary>
    public static class EquaShipClient
    {
        // logging
        static readonly ILog log = LogManager.GetLogger(typeof(EquaShipClient));

        /// <summary>
        /// Determine live/test server to use
        /// </summary>
        public static bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("EquaShipTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("EquaShipTestServer", value);
            }
        }


        /// <summary>
        /// Gets the web servie Url for equaship
        /// </summary>
        public static string ServiceUrl
        {
            get
            {
                if (UseTestServer)
                {
                    return "https://integrate.equaship.com/apidev/equashipapi.asmx";
                }
                else
                {
                    return "https://api.equaship.com/api/EquashipAPI.asmx";
                }
            }
        }

        /// <summary>
        /// Gets the Interapptive Activation Key for Equaship
        /// </summary>
        public static string ActivationKey
        {
            get
            {
                if (UseTestServer)
                {
                    return "3gN9/Gl+f0YD0A7rW9mfpAvsvcmVxJwOXrqs8qA2VHwS+QMQL+4ZOXC+YO5ZNZkz";
                }
                else
                {
                    return "2/YQOcFJDDD8BDZIKPcmlhNXeaaGFqNK7XoRG8kKUD7FrdOfe2J7Ew==";
                }
            }
        }

        /// <summary>
        /// Service Codes
        /// </summary>
        static Tuple<string, EquaShipServiceType>[] servicesCodes = new Tuple<string, EquaShipServiceType>[]
        {
            new Tuple<string, EquaShipServiceType>("2725", EquaShipServiceType.Ground),
            new Tuple<string, EquaShipServiceType>("2734", EquaShipServiceType.International),

            /* Documenting the other services here, which ShipWorks won't be supporting
            new Tuple<string, EquaShipServiceType>("2726", EquaShipServiceType.ExpressMail),
            new Tuple<string, EquaShipServiceType>("2727", EquaShipServiceType.ExpressFlatRateEnvelope),
            new Tuple<string, EquaShipServiceType>("2728", EquaShipServiceType.Priority),
            new Tuple<string, EquaShipServiceType>("2729", EquaShipServiceType.ExpressPriorityFlatRateEnvelope),
            new Tuple<string, EquaShipServiceType>("2730", EquaShipServiceType.ExpressPriorityPaddedFlatRateEnvelope),
            new Tuple<string, EquaShipServiceType>("2731", EquaShipServiceType.ExpressPrioritySmallFlatRateBox),
            new Tuple<string, EquaShipServiceType>("2732", EquaShipServiceType.ExpressPriorityMediumFlatRateBox),
            new Tuple<string, EquaShipServiceType>("2733", EquaShipServiceType.ExpressPriorityLargeFlatRateBox),
             */
        };

        /// <summary>
        /// Tests connectivity
        /// </summary>
        public static void TestConnection(EquaShipAccountEntity account)
        {
            using (EquashipAPI api = CreateService("TestConnection_Void"))
            {
                try
                {
                    VoidShipmentRequest request = new VoidShipmentRequest();
                    request.Authentication = CreateAuthentication(account);

                    // dummy tracking number
                    request.TrackingNumber = "123track456";

                    ResponseToVoidShipmentRequest response = api.VoidShipment(request);
                    ValidateResponse(response, ErrorResponseLocation.Status);
                }
                catch (EquaShipException ex)
                {
                    // if we get the Login failed error, we know the credentials were bad
                    if (String.Compare(ex.Message, "Login failed", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the web proxy for EquaShip communication
        /// </summary>
        private static EquashipAPI CreateService(string action)
        {
            EquashipAPI client = new EquashipAPI(new ApiLogEntry(ApiLogSource.EquaShip, action));

            client.Url = ServiceUrl;

            return client;
        }

        /// <summary>
        /// Voids the provided shipment
        /// </summary>
        public static void VoidShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            EquaShipAccountEntity account = GetAccount(shipment);

            try
            {
                using (EquashipAPI api = CreateService("VoidShipment"))
                {
                    VoidShipmentRequest request = new VoidShipmentRequest();
                    request.Authentication = CreateAuthentication(account);

                    request.TrackingNumber = shipment.TrackingNumber;

                    ResponseToVoidShipmentRequest response = api.VoidShipment(request);
                    ValidateResponse(response, ErrorResponseLocation.Status);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EquaShipException));
            }
        }

        /// <summary>
        /// Track a shipment
        /// </summary>
        public static TrackingResult TrackShipment(string trackingNumber)
        {
            EquaShipAccountEntity account = EquaShipAccountManager.Accounts.FirstOrDefault();
            if (account == null)
            {
                throw new EquaShipException("ShipWorks cannot track a UPS shipment without a configured EquaShip account.");
            }

            try
            {
                using (EquashipAPI service = CreateService("TrackShipment"))
                {
                    TrackShipmentRequest request = new TrackShipmentRequest();
                    request.Authentication = CreateAuthentication(account);
                    request.TrackingNumber = trackingNumber;

                    ResponseToTrackShipmentRequest response = service.TrackShipment(request);
                    ValidateResponse(response, ErrorResponseLocation.Status);

                    TrackingResult result = new TrackingResult();

                    if (response.Events != null && response.Events.Event != null)
                    {
                        result.Details.AddRange(response.Events.Event.Select(t => new TrackingResultDetail()
                        {
                            Activity = t.EventDescription,
                            Date = t.EventDateTime.Date.ToString(),
                            Time = t.EventDateTime.TimeOfDay.ToString(),
                            Location = t.Location
                        }));
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EquaShipException));
            }
        }

        /// <summary>
        /// Processes a shipment using EquaShip
        /// </summary>
        public static void ProcessShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // don't allow International shipments to go since Equaship doesn't support them yet
            if (shipment.EquaShip.Service == (int)EquaShipServiceType.International)
            {
                throw new ShippingException("International shipping is unavailable at this time.");
            }

            EquaShipAccountEntity account = GetAccount(shipment);

            try
            {
                using (EquashipAPI service = CreateService("ProcessShipment"))
                {
                    ShipmentRequest request = new ShipmentRequest();
                    request.Authentication = CreateAuthentication(account);
                    request.Shipment = CreateShipment(shipment);

                    ResponseToShipmentRequest response = service.ProcessShipment(request);
                    ValidateResponse(response, ErrorResponseLocation.Errors);

                    // save the labels
                    SaveLabelImages(shipment, response);

                    // extract the result
                    shipment.TrackingNumber = response.TrackingNumber;
                    shipment.ShipmentCost = Convert.ToDecimal(response.TotalCharges);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EquaShipException));
            }
        }

        /// <summary>
        /// Save shipment label data
        /// </summary>
        private static void SaveLabelImages(ShipmentEntity shipment, ResponseToShipmentRequest response)
        {
            if (response.Label == null)
            {
                throw new EquaShipException("No labels present in the EquaShip response.");
            }

            // if we had saved an image for this shipment previously, clear it
            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

            // read each label
            int part = 0;
            foreach (ShipmentLabelType label in response.Label)
            {
                SaveLabelImage(shipment, part == 0 ? "LabelPrimary" : String.Format("LabelPart{0}", part), label.LabelBase64String, false);
                part++;
            }

            // customs?
            if (response.Document != null)
            {
                part = 0;
                foreach (ShipmentDocumentType doc in response.Document)
                {
                    part++;
                    SaveLabelImage(shipment,String.Format("Customs{0}", part), doc.DocumentBase64String, false);
                }
            }
        }

        /// <summary>
        /// Save a label image
        /// </summary>
        private static void SaveLabelImage(ShipmentEntity shipment, string name, string base64, bool crop)
        {
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64)))
            {
                // If not cropping, or if it is thermal, just save it as-is
                if (!crop || shipment.ThermalType != null)
                {
                    DataResourceManager.CreateFromBytes(stream.ToArray(), shipment.ShipmentID, name);
                }
                else
                {
                    using (Image imageOriginal = Image.FromStream(stream))
                    {
                        using (Image imageLabelCrop = DisplayHelper.CropImage(imageOriginal, 0, 0, imageOriginal.Width, Math.Min(imageOriginal.Height, 1580)))
                        {
                            using (MemoryStream imageStream = new MemoryStream())
                            {
                                imageLabelCrop.Save(imageStream, ImageFormat.Png);

                                DataResourceManager.CreateFromBytes(imageStream.ToArray(), shipment.ShipmentID, name);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks a response 
        /// </summary>
        private static void ValidateResponse(object response, ErrorResponseLocation errorLocation)
        {
            // validate response
            if ((object)response == null)
            {
                throw new ArgumentNullException("response");
            }

            Type responseType = response.GetType();

            // pick out the errors
            if (errorLocation == ErrorResponseLocation.Status)
            {
                // reflect to get the value of status property
                PropertyInfo statusProperty = responseType.GetProperty("Status");
                if (statusProperty == null)
                {
                    throw new InvalidOperationException("Unable to retrieve the Status property on type " + responseType.ToString());
                }

                ErrorType status = statusProperty.GetValue(response, null) as ErrorType;
                if (status != null)
                {
                    if (status.Code != 0)
                    {
                        throw new EquaShipException(status.Message);
                    }
                }
            }
            else if (errorLocation == ErrorResponseLocation.Errors)
            {
                PropertyInfo errorsProperty = responseType.GetProperty("Errors");
                if (errorsProperty == null)
                {
                    throw new InvalidOperationException("Unable to retrieve the Errors property on type " + responseType.ToString());
                }

                ErrorListType errorList = errorsProperty.GetValue(response, null) as ErrorListType;
                if (errorList != null && errorList.Error != null)
                {
                    string userMessage = "";
                    string logMessage = "";
                    foreach (ErrorType error in errorList.Error)
                    {
                        if (userMessage.Length > 0)
                        {
                            userMessage += ", ";
                        }

                        userMessage += error.Message;
                        logMessage += String.Format("{0}, code={1}", error.Message, error.Code);
                    }

                    if (userMessage.Length > 0)
                    {
                        log.Error(logMessage);
                        throw new EquaShipException(userMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Create a shipment request to process the provided shipment
        /// </summary>
        private static WebServices.ShipmentType CreateShipment(ShipmentEntity shipment)
        {
            WebServices.ShipmentType request = new WebServices.ShipmentType();
            EquaShipShipmentEntity eqShip = shipment.EquaShip;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            // address information
            request.ShipFromAddress = new AddressType();
            request.ShipToAddress = new AddressType();
            FillAddress(request.ShipFromAddress, new PersonAdapter(shipment, "Origin"));
            FillAddress(request.ShipToAddress, new PersonAdapter(shipment, "Ship"));

            request.ServiceCode = GetServiceCode((EquaShipServiceType)eqShip.Service);
            request.ShipDate = shipment.ShipDate;
            request.ResidentialFlag = shipment.ResidentialResult;
            request.SaturdayDeliveryFlag = eqShip.SaturdayDelivery;
            request.SignatureConfirmationFlag = eqShip.Confirmation == (int)EquaShipConfirmationType.Signature;
            request.DeliveryConfirmationFlag = eqShip.Confirmation == (int)EquaShipConfirmationType.Delivery;

            request.EmailNotificationFlag = eqShip.EmailNotification;

            // process the tokenized text fields
            request.ShippingNotes = TemplateTokenProcessor.ProcessTokens(eqShip.ShippingNotes, shipment.ShipmentID);
            request.ReferenceNumber = TemplateTokenProcessor.ProcessTokens(eqShip.ReferenceNumber, shipment.ShipmentID);
            request.Description = TemplateTokenProcessor.ProcessTokens(eqShip.Description, shipment.ShipmentID);

            // Not using equaship insurance
            request.InsurancePayType = InsurancePayTypeEnum.WAIVED;

            // package information
            request.PackagingType = GetPackagingType((EquaShipPackageType)eqShip.PackageType);
            request.Height = (int)eqShip.DimsHeight;
            request.Length = (int)eqShip.DimsLength;
            request.Width = (int)eqShip.DimsWidth;
            request.Weight = shipment.TotalWeight;

            // locate and set the desired thermal type
            ThermalLanguage? thermalType = settings.EquaShipThermal ? (ThermalLanguage) settings.EquaShipThermalType : (ThermalLanguage?) null;

            // adjust thermal type based on service used?

            // update the shipment
            shipment.ThermalType = (int?)thermalType;

            // label 
            if (shipment.ThermalType.HasValue)
            {
                // 1 for epl, 3 for zebra
                request.LabelType = shipment.ThermalType.Value == (int)ThermalLanguage.EPL ? 1 : 3;
            }
            else
            {
                // request PNG
                request.LabelType = 6;
            }

            // undocumented, required?
            //request.LabelSize = 0;
            
            // customs items
            ApplyInternational(request, shipment);

            return request;
        }

        /// <summary>
        /// Apply international-only data
        /// </summary>
        private static void ApplyInternational(WebServices.ShipmentType request, ShipmentEntity shipment)
        {
            if (shipment.ShipCountryCode == "US")
            {
                return;
            }

            request.LineItems = new LineItemListType();

            // line items
            List<LineItemType> lineItems = new List<LineItemType>();
            foreach (ShipmentCustomsItemEntity customs in shipment.CustomsItems)
            {
                LineItemType lineItem = new LineItemType();
                lineItem.ScheduleB = customs.HarmonizedCode;
                lineItem.Price = Convert.ToDouble(customs.UnitValue);
                lineItem.Description = customs.Description;
                lineItem.Quantity = (int)customs.Quantity;
                lineItem.QuantityUnit = "ea";
            }

            request.LineItems.LineItem = lineItems.ToArray();
        }

        /// <summary>
        /// Gets the API value to send for the package type
        /// </summary>
        private static int GetPackagingType(EquaShipPackageType packageType )
        {
            switch (packageType)
            {
                case EquaShipPackageType.Box:
                    return 103;
                case EquaShipPackageType.Tube:
                    return 104;
                case EquaShipPackageType.CustomerSupplied:
                    return 105;
                case EquaShipPackageType.LetterEnvelope:
                    return 106;
                case EquaShipPackageType.NonstandardContainer:
                    return 107;
                default:
                    // unknown
                    return 101;
            }
        }

        /// <summary>
        /// Gets a service type from its EquaShip code
        /// </summary>
        private static EquaShipServiceType GetServiceFromCode(string code)
        {
            Tuple<string, EquaShipServiceType> codeTuple = servicesCodes.FirstOrDefault(t => String.Compare(t.Item1, code, StringComparison.OrdinalIgnoreCase) == 0);
            if (codeTuple == null)
            {
                throw new InvalidOperationException(String.Format("Unhandled EquaShip Service Code: {0}", code));
            }

            return codeTuple.Item2;
        }

        /// <summary>
        /// Gets the API value for the service type
        /// </summary>
        private static string GetServiceCode(EquaShipServiceType serviceType)
        {
            Tuple<string, EquaShipServiceType> codeTuple = servicesCodes.FirstOrDefault(t => t.Item2 == serviceType);
            if (codeTuple == null)
            {
                throw new InvalidOperationException(String.Format("Unhandled EquashipServiceType: {0}", serviceType));
            }

            return codeTuple.Item1;
        }

        /// <summary>
        /// Populates an AddressType from a person adapter
        /// </summary>
        private static void FillAddress(AddressType address, PersonAdapter person)
        {
            address.DepartmentOrName = new PersonName(person).FullName;
            address.Company = person.Company;
            address.AddressLine1 = person.Street1;
            address.AddressLine2 = person.Street2;
            address.City = person.City;
            address.State = person.StateProvCode;
            address.Zip = person.PostalCode5;
            address.Zip4 = person.PostalCode4;
            address.ISO2CountryCode = person.CountryCode;
            address.Phone = person.Phone;
            address.Fax = person.Fax;
            address.Email = person.Email;
        }

        /// <summary>
        /// Creates the authentication object needed for EquaShip communication
        /// </summary>
        private static AuthenticationType CreateAuthentication(EquaShipAccountEntity account)
        {
            AuthenticationType auth = new AuthenticationType();
            auth.ActivationKey = SecureText.Decrypt(ActivationKey, "interapptive");
            auth.LoginName = account.Username;
            auth.Password = SecureText.Decrypt(account.Password, account.Username);

            return auth;
        }

        /// <summary>
        /// Gets the account used for the shipment
        /// </summary>
        private static EquaShipAccountEntity GetAccount(ShipmentEntity shipment)
        {
            EquaShipAccountEntity account = EquaShipAccountManager.GetAccount(shipment.EquaShip.EquaShipAccountID);

            if (account == null)
            {
                throw new EquaShipException("No EquaShip account is selected for the shipment.");
            }

            return account;
        }

        /// <summary>
        /// Get shipping rates for a shipment
        /// </summary>
        public static List<EquaShipServiceRate> GetRates(ShipmentEntity shipment)
        {
            // find the associated account
            EquaShipAccountEntity account = GetAccount(shipment);

            // resulting rates
            List<EquaShipServiceRate> rates = new List<EquaShipServiceRate>();

            // Equaship returns rates one at a time.  At this time, only handling Ground shipments with Equaship
            EquaShipServiceType[] rateService = new EquaShipServiceType[] { EquaShipServiceType.Ground };

            try
            {
                using (EquashipAPI api = CreateService("GetRates"))
                {                    
                    foreach (EquaShipServiceType equashipService in rateService)
                    {
                        RatesAndTransitTimesRequest request = new RatesAndTransitTimesRequest();
                        request.Authentication = CreateAuthentication(account);
                        request.Shipment = CreateShipment(shipment);

                        // fixup the request 
                        request.Shipment.ServiceCode = GetServiceCode(equashipService);

                        // if ship-to is blank, the request will fail
                        if (request.Shipment.ShipToAddress.Email.Trim().Length == 0)
                        {
                            request.Shipment.ShipToAddress.Email = "nobody@interapptive.com";
                        }

                        ResponseToRatesAndTransitTimesRequest response = api.GetRatesAndTransitTimes(request);
                        ValidateResponse(response, ErrorResponseLocation.Status);

                        // go through each result, turning it into one of our rate objects
                        rates.AddRange(response.Services.Service.Select(s =>
                                new EquaShipServiceRate()
                                {
                                    Service = GetServiceFromCode(s.Code),
                                    Rate = s.Rate,
                                    EstimatedDelivery = s.EstimatedDeliveryDate
                                }
                        ));
                    }
                }

                return rates;
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EquaShipException));
            }
        }

        /// <summary>
        /// Retrieves dropoff locations
        /// </summary>
        internal static void GetDropOffLocations(EquaShipAccountEntity account)
        {
            try
            {
                using (EquashipAPI api = CreateService("GetDropoffLocations"))
                {
                    DropOffPointRequest request = new DropOffPointRequest();
                    request.Authentication = CreateAuthentication(account);
                    request.ZipCode = account.PostalCode;

                    api.LocateDropOffPoints(request);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EquaShipException));
            }
        }
    }
}