using System;
using System.Web.Services.Protocols;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Class for getting UPS accounts registred for use in ShipWorks
    /// </summary>
    public static class UpsApiRegistration
    {
        /// <summary>
        /// The UPS URL to use - based on test server or not
        /// </summary>
        private static string ServiceUrl
        {
            get
            {
                if (UpsWebClient.UseTestServer)
                {
                    return "https://wwwcie.ups.com/webservices/Registration";
                }
                else
                {
                    return "https://onlinetools.ups.com/webservices/Registration";
                }
            }
        }

        /// <summary>
        /// Create the WebService client for UPS for the given action
        /// </summary>
        private static RegisterMgrAcctService CreateService(string action)
        {
            RegisterMgrAcctService client = new RegisterMgrAcctService(new ApiLogEntry(ApiLogSource.UPS, action));

            client.Url = ServiceUrl;

            return client;
        }

        /// <summary>
        /// Process the registration for the given UPS account
        /// </summary>
        /// <exception cref="UpsApiException"></exception>
        /// <exception cref="UpsWebServiceException"></exception>
        /// <exception cref="UpsException"></exception>
        public static void ProcessRegistration(UpsAccountEntity upsAccount, UpsOltInvoiceAuthorizationData invoiceAuth)
        {
            string userId = Guid.NewGuid().ToString("N").Substring(0, 16);
            string password = Guid.NewGuid().ToString("N").Substring(0, 8);

            RegisterRequest request = GenerateRegisterRequest(upsAccount, invoiceAuth, userId, password);

            try
            {
                using (RegisterMgrAcctService api = CreateService("RegisterAccount"))
                {
                    UPSSecurityServiceAccessToken upsAccessToken = new UPSSecurityServiceAccessToken
                    {
                        AccessLicenseNumber = "AB9B05D34FF5EAC0"
                    };

                    UPSSecurityUsernameToken upsUsernameToken = new UPSSecurityUsernameToken();
                    upsUsernameToken.Username = "shipworks_api";
                    upsUsernameToken.Password = "x98&$opiLhJK";

                    UPSSecurity upsSecurity = new UPSSecurity();
                    upsSecurity.ServiceAccessToken = upsAccessToken;
                    upsSecurity.UsernameToken = upsUsernameToken;

                    api.UPSSecurityValue = upsSecurity;

                    RegisterResponse response = api.ProcessRegister(request);
                    CodeDescriptionType responseStatus = response.Response.ResponseStatus;

                    if (responseStatus.Code == "1")
                    {
                        upsAccount.UserID = userId;
                        upsAccount.Password = password;
                        upsAccount.InvoiceAuth = true;
                    }
                    else
                    {
                        throw new UpsApiException(UpsApiResponseStatus.Hard, responseStatus.Code, responseStatus.Description);
                    }
                }
            }
            catch (SoapException ex)
            {
                throw new UpsWebServiceException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UpsException));
            }
        }

        /// <summary>
        /// Generates the register request.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static RegisterRequest GenerateRegisterRequest(UpsAccountEntity upsAccount, UpsOltInvoiceAuthorizationData invoiceAuth, string userId, string password)
        {
            RegisterRequest request = new RegisterRequest();
            request.Username = userId;
            request.Password = password;
            request.SuggestUsernameIndicator = "N";

            // Notification code to be sent if the account is about to expire.  '00' None, '01' Email. Given
            // our users wouldn't know what it means, we set to 00
            request.NotificationCode = "00";

            
            PersonAdapter accountAddress = new PersonAdapter(upsAccount, "");

            request.CompanyName = upsAccount.Company;
            request.CustomerName = new PersonName(accountAddress).FullName;

            AddressType address = new AddressType();
            request.Address = address;
            address.AddressLine = accountAddress.StreetLines;
            address.City = accountAddress.City;
            address.StateProvinceCode = accountAddress.StateProvCode;
            address.PostalCode = accountAddress.PostalCode;
            address.CountryCode = accountAddress.CountryCode;

            request.PhoneNumber = accountAddress.Phone10Digits;
            request.EmailAddress = accountAddress.Email;
           
            ShipperAccountType shipper = new ShipperAccountType();
            request.ShipperAccount = shipper;
            shipper.AccountName = "Interapptive";
            shipper.AccountNumber = upsAccount.AccountNumber;
            shipper.PostalCode = upsAccount.PostalCode;
            shipper.CountryCode = upsAccount.CountryCode;

            InvoiceInfoType invoiceInfo = new InvoiceInfoType();
            shipper.InvoiceInfo = invoiceInfo;
            invoiceInfo.CurrencyCode = UpsUtility.GetCurrency(upsAccount);
            invoiceInfo.InvoiceNumber = invoiceAuth.InvoiceNumber;
            invoiceInfo.InvoiceDate = invoiceAuth.InvoiceDate.ToString("yyyMMdd");
            invoiceInfo.InvoiceAmount = invoiceAuth.InvoiceAmount.ToString("0.00");
            invoiceInfo.ControlID = invoiceAuth.ControlID;
            
            return request;
        }
    }
}
