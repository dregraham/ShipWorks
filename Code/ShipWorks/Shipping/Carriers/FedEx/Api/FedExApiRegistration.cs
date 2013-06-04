using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;
using ShipWorks.Data;
using ShipWorks.ApplicationCore.Logging;
using System.Web.Services.Protocols;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using System.Net;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// FedEx registration API functionality
    /// </summary>
    public static class FedExApiRegistration
    {
        static string cspSolutionID = "086";

        static bool hasDoneVersionCapture = false;

        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static RegistrationService CreateRegistrationService(string logName)
        {
            RegistrationService webService = new RegistrationService(new ApiLogEntry(ApiLogSource.FedEx, logName));
            webService.Url = FedExApiCore.ServerUrl;

            return webService;
        }

        /// <summary>
        /// Subscribe a new FedEx shipper for use with shipworks
        /// </summary>
        public static void SubscribeShipper(FedExAccountEntity account)
        {
            // If we haven't done a Register yet, that has to come first
            if (ShippingSettings.Fetch().FedExUsername == null)
            {
                RegisterCspUser(account);
            }

            SubscriptionRequest request = new SubscriptionRequest();

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client 
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "fcas",
                Major = 2,
                Intermediate = 1,
                Minor = 0
            };

            // CSP 
            request.CspSolutionId = cspSolutionID;
            request.CspType = CspType.CERTIFIED_SOLUTION_PROVIDER;
            request.CspTypeSpecified = true;

            PersonAdapter person = new PersonAdapter(account, "");

            // Subscriber info
            request.Subscriber = new Party();
            request.Subscriber.AccountNumber = account.AccountNumber;
            request.Subscriber.Address = FedExApiCore.CreateAddress<Address>(person);
            request.Subscriber.Contact = FedExApiCore.CreateContact<Contact>(person);
            request.AccountShippingAddress = FedExApiCore.CreateAddress<Address>(person);

            try
            {
                using (RegistrationService webService = CreateRegistrationService("Subscribe"))
                {
                    SubscriptionReply reply = webService.subscription(request);

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    account.MeterNumber = reply.MeterNumber;
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
        /// Do one-time registration of the CSP user
        /// </summary>
        private static void RegisterCspUser(FedExAccountEntity account)
        {
            RegisterWebCspUserRequest request = new RegisterWebCspUserRequest();

            // Authentication
            request.WebAuthenticationDetail = new WebAuthenticationDetail
            {
                CspCredential = FedExApiCore.GetCspCredentials<WebAuthenticationCredential>()
            };

            // Client
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "fcas",
                Major = 2,
                Intermediate = 1,
                Minor = 0
            };

            PersonAdapter person = new PersonAdapter(account, "");

            // Contact
            request.BillingAddress = FedExApiCore.CreateAddress<Address>(person);
            request.UserContactAndAddress = new ParsedContactAndAddress();
            request.UserContactAndAddress.Address = FedExApiCore.CreateAddress<Address>(person);
            request.UserContactAndAddress.Contact = FedExApiCore.CreateParsedContact<ParsedContact>(person);

            try
            {
                using (RegistrationService webService = CreateRegistrationService("RegisterCSPUser"))
                {
                    RegisterWebCspUserReply reply = webService.registerWebCSPUser(request);

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    ShippingSettingsEntity settings = ShippingSettings.Fetch();
                    settings.FedExUsername = reply.Credential.Key;
                    settings.FedExPassword = SecureText.Encrypt(reply.Credential.Password, "FedEx");

                    ShippingSettings.Save(settings);
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
        /// Do the version capture calls for FedEx.  Will only actually do something once per run of ShipWorks.
        /// </summary>
        public static void VersionCapture()
        {
            if (hasDoneVersionCapture)
            {
                return;
            }

            // Do version capture for each account
            foreach (FedExAccountEntity account in FedExAccountManager.Accounts.Where(a => !a.Is2xMigrationPending))
            {
                VersionCaptureRequest request = new VersionCaptureRequest();

                // Authentication
                request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

                // Client
                request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

                // Version
                request.Version = new VersionId
                {
                    ServiceId = "fcas",
                    Major = 2,
                    Intermediate = 1,
                    Minor = 0
                };

                request.OriginLocationId = FedExApiPackageMovement.GetLocationID(account);
                request.VendorProductPlatform = "WINDOWS";

                try
                {
                    using (RegistrationService webService = CreateRegistrationService("VersionCapture"))
                    {
                        VersionCaptureReply reply = webService.versionCapture(request);

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

            hasDoneVersionCapture = true;
        }
    }
}
