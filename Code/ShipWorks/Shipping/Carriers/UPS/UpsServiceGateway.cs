using System;
using System.Web.Services.Protocols;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using RegistrationAPI = ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using OpenAccountAPI = ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Implementation of the IUpsServiceGateway interface that is responsible for network communication with UPS.
    /// </summary>
    public class UpsServiceGateway : IUpsServiceGateway
    {
        private readonly UpsSettings settings;

        private const string AccessLicenseNumber = "AB9B05D34FF5EAC0";

        private const string UserName = "shipworks3";
        private const string Password = "Z59$sBXsTcYk";


        /// <summary>
        /// Initializes a new instance of the <see cref="UpsServiceGateway" /> class.
        /// </summary>
        /// <param name="settingsRepositoryIndex">The settings repository.</param>
        public UpsServiceGateway(IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepositoryIndex)
        {
            // Tell the UpsOpenAccount settings which data source to use 
            settings = new UpsSettings(settingsRepositoryIndex[ShipmentTypeCode.UpsOnLineTools]);
        }

        /// <summary>
        /// Intended to interact with the UPS OpenAccount API for opening a new account with UPS.
        /// </summary>
        /// <param name="openAccountRequest">The open account request.</param>
        /// <returns>The OpenAccountResponse received from UPS.</returns>
        /// <exception cref="UpsOpenAccountSoapException"></exception>
        public OpenAccountAPI.OpenAccountResponse OpenAccount(OpenAccountAPI.OpenAccountRequest openAccountRequest)
        {
            try
            {
                // This is where we actually communicate with UpsOpenAccount, so it's okay to explicitly create the 
                // OpenAccountService object here (i.e. no more abstractions can be made)
                using (OpenAccountAPI.OpenAccountService service = new OpenAccountAPI.OpenAccountService(new ApiLogEntry(ApiLogSource.UPS, "OpenAccount")))
                {
                    // Point the service to the correct endpoint
                    service.Url = $"{settings.EndpointUrl}/webservices/OpenAccount";

                    service.UPSSecurityValue = new OpenAccountAPI.UPSSecurity()
                    {
                        ServiceAccessToken = new OpenAccountAPI.UPSSecurityServiceAccessToken()
                        {
                            AccessLicenseNumber = AccessLicenseNumber
                        },
                        UsernameToken = new OpenAccountAPI.UPSSecurityUsernameToken()
                        {
                            Username = UserName,
                            Password = Password
                        }
                    };

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to UpsOpenAccount
                    return service.ProcessOpenAccount(openAccountRequest);
                }
            }
            catch (SoapException ex)
            {
                throw new UpsOpenAccountSoapException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof (UpsOpenAccountException));
            }
        }

        /// <summary>
        /// Intended to interact with the UPS registration API when adding an account to ShipWorks.
        /// </summary>
        public RegistrationAPI.RegisterResponse RegisterAccount(RegistrationAPI.RegisterRequest request) =>
           RegisterManageHelper(request, "RegisterAccount", (accountRequest, service) => service.ProcessRegister(accountRequest));

        /// <summary>
        /// Intended to interact with the UPS registration API when associating a UPS account with a profile.
        /// </summary>
        public RegistrationAPI.ManageAccountResponse ManageAccount(RegistrationAPI.ManageAccountRequest request) => 
            RegisterManageHelper(request, "ManageAccount", (accountRequest, service) => service.ProcessManageAccount(accountRequest));


        /// <summary>
        /// Registers the manage helper.
        /// </summary>
        private TResponseType RegisterManageHelper<TRequestType, TResponseType>(
            TRequestType request,
            string logName,
            Func<TRequestType, RegistrationAPI.RegisterMgrAcctService, TResponseType> serviceCall)
        {
            try
            {
                // Explicit call to Ups. 
                using (RegistrationAPI.RegisterMgrAcctService service = new RegistrationAPI.RegisterMgrAcctService(new ApiLogEntry(ApiLogSource.UPS, logName)))
                {
                    service.Url = $"{settings.EndpointUrl}/webservices/Registration";

                    service.UPSSecurityValue = new RegistrationAPI.UPSSecurity()
                    {
                        ServiceAccessToken = new RegistrationAPI.UPSSecurityServiceAccessToken()
                        {
                            AccessLicenseNumber = AccessLicenseNumber
                        },
                        UsernameToken = new RegistrationAPI.UPSSecurityUsernameToken()
                        {
                            Username = UserName,
                            Password = Password
                        }
                    };

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to UpsOpenAccount
                    return serviceCall(request, service);
                }
            }
            catch (SoapException ex)
            {
                throw new UpsWebServiceException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(CarrierException));
            }
        }
    }
}