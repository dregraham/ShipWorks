using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.Contract;
using log4net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// This is hitting v39 of the Stamps.com API to change the plan. At the time this class was created
    /// the StampsApiSession was using v29 of the API. In order to not have to retest both Stamps.com and
    /// Express1 to use the functionality to change plan, this class was created that gets called by the 
    /// StampsApiSession.
    /// </summary>
    public class StampsContractWebClient
    {
        // This value came from Stamps.com (the "standard" account value is 88)
        private const int ExpeditedPlanID = 236;

        private readonly Guid integrationID = new Guid("F784C8BC-9CAD-4DAF-B320-6F9F86090032");
        private readonly ILog log;

        // Maps stamps.com usernames to their latest authenticator tokens
        static Dictionary<string, string> usernameAuthenticatorMap = new Dictionary<string, string>();

        // Maps stamps.com usernames to the object lock used to make sure only one thread is trying to authenticate at a time
        static Dictionary<string, object> authenticationLockMap = new Dictionary<string, object>();

        private readonly bool useTestServer;
        private readonly ICertificateInspector certificateInspector;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsContractWebClient" /> class.
        /// </summary>
        /// <param name="useTestServer">if set to <c>true</c> [use test server].</param>
        /// <param name="certificateInspector">The certificate inspector.</param>
        public StampsContractWebClient(bool useTestServer, ICertificateInspector certificateInspector)
        {
            this.useTestServer = useTestServer;
            this.certificateInspector = certificateInspector;

            log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Gets the service URL.
        /// </summary>
        private string ServiceUrl
        {
            get { return useTestServer ? "https://swsim.testing.stamps.com/swsim/SwsimV39.asmx" : "https://swsim.stamps.com/swsim/swsimv39.asmx"; }
        }

        /// <summary>
        /// Makes request to Stamps.com API to change plan associated with the account referenced by the authenticator to be 
        /// an Expedited plan. This requires an authentication call to the Stamps.com API prior to changing the plan.
        /// </summary>
        public void ChangeToExpeditedPlan(StampsAccountEntity account, string promoCode)
        {
            AuthenticationWrapper(() =>
            {
                InternalChangeToExpeditedPlan(GetAuthenticator(account), promoCode);
                return true;
            }, account);
        }

        /// <summary>
        /// Makes request to Stamps.com API to change plan associated with the account referenced by the authenticator to be 
        /// an Expedited plan. This requires an authentication call to the Stamps.com API prior to changing the plan.
        /// </summary>
        private void InternalChangeToExpeditedPlan(string authenticator, string promoCode)
        {
            // Output parameters for web service call
            int transactionID;
            PurchaseStatus purchaseStatus;
            string rejectionReason = string.Empty;
            
            try
            {
                using (SwsimV39 webService = CreateWebService("ChangePlan"))
                {
                    webService.Url = ServiceUrl;
                    webService.ChangePlan(authenticator, ExpeditedPlanID, promoCode, out purchaseStatus, out transactionID, out rejectionReason);
                }
            }
            catch (StampsException exception)
            {
                log.ErrorFormat("ShipWorks was unable to change the Stamps.com plan. {0}. {1}", rejectionReason ?? string.Empty, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Checks with Stamps.com to get the contract type of the account.
        /// </summary>
        public StampsAccountContractType GetContractType(StampsAccountEntity account)
        {
            return AuthenticationWrapper(() => InternalGetContractType(account), account);
        }

        /// <summary>
        /// The internal GetContractType implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private StampsAccountContractType InternalGetContractType(StampsAccountEntity account)
        {
            StampsAccountContractType contract = StampsAccountContractType.Unknown;
            AccountInfo accountInfo;
            
            // There's a chance that this will be called when checking counter rates, so check the
            // certificate before transmitting our credentials
            CheckCertificate();

            using (SwsimV39 webService = CreateWebService("GetContractType"))
            {
                // Address and CustomerEmail are not returned by Express1, so do not use them.
                Address address;
                string email;

                string auth = webService.GetAccountInfo(GetAuthenticator(account), out accountInfo, out address, out email);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            RatesetType? rateset = accountInfo.RatesetType;
            if (rateset.HasValue)
            {
                switch (rateset)
                {
                    case RatesetType.CBP:
                    case RatesetType.Retail:
                        contract = StampsAccountContractType.Commercial;
                        break;

                    case RatesetType.CPP:
                    case RatesetType.NSA:
                        contract = StampsAccountContractType.CommercialPlus;
                        break;

                    case RatesetType.Reseller:
                        contract = StampsAccountContractType.Reseller;
                        break;

                    default:
                        contract = StampsAccountContractType.Unknown;
                        break;
                }
            }

            return contract;
        }
        
        private void CheckCertificate()
        {
            CertificateRequest certificateRequest = new CertificateRequest(new Uri(ServiceUrl), certificateInspector);

            CertificateSecurityLevel certificateSecurityLevel = certificateRequest.Submit();
            if (certificateSecurityLevel != CertificateSecurityLevel.Trusted)
            {
                string description = EnumHelper.GetDescription(ShipmentTypeCode.Stamps);
                throw new StampsException(string.Format("ShipWorks is unable to make a secure connection to {0}.", description));
            }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV39 CreateWebService(string logName)
        {
            SwsimV39 webService = new SwsimV39(new LogEntryFactory().GetLogEntry(ApiLogSource.UspsStamps, logName, LogActionType.Other))
            {
                Url = ServiceUrl
            };

            return webService;
        }


        /// <summary>
        /// Authenticate the given user with Stamps.com. 
        /// </summary>
        private void AuthenticateUser(string username, string password)
        {
            try
            {
                // Output parameters from stamps.com
                DateTime lastLoginTime = new DateTime();
                bool clearCredential = false;
                bool codeWordsSet = false;

                string bannerText = string.Empty;
                bool passwordExpired = false;

                using (SwsimV39 webService = CreateWebService("Authenticate"))
                {
                    string auth = webService.AuthenticateUser(new Credentials
                    {
                        IntegrationID = integrationID,
                        Username = username,
                        Password = password
                    }, out lastLoginTime, out clearCredential, out bannerText, out passwordExpired, out codeWordsSet);

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
        /// Wraps the given executor in methods that ensure the appropriate authentication for the account
        /// </summary>
        private T AuthenticationWrapper<T>(Func<T> executor, StampsAccountEntity account)
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

                        if (triesLeft > 0 && IsStaleAuthenticator(ex, account.StampsReseller == (int)StampsResellerType.Express1))
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
        private string GetAuthenticator(StampsAccountEntity account)
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
        private static bool IsStaleAuthenticator(SoapException ex, bool isExpress1)
        {
            if (isExpress1)
            {
                // Express1 does not return error codes...
                switch (ex.Message)
                {
                    case "Invalid authentication info":
                    case "Unable to authenticate user.":
                        return true;
                }

                return false;
            }
            else
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
}
