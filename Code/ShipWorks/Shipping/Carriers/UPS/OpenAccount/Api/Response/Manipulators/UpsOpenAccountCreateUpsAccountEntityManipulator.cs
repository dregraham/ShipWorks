using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response.Manipulators
{
    public class UpsOpenAccountCreateUpsAccountEntityManipulator : ICarrierResponseManipulator
    {
        private OpenAccountResponse openAccountResponse;

        private UpsAccountEntity upsAccountEntity;

        public void Manipulate(ICarrierResponse carrierResponse)
        {
            Validate(carrierResponse);

            UpsOpenAccountResponse upsOpenAccountResponse = carrierResponse as UpsOpenAccountResponse;
            OpenAccountRequest openAccountRequest = upsOpenAccountResponse.Request.NativeRequest as OpenAccountRequest;
            openAccountResponse = upsOpenAccountResponse.NativeResponse as OpenAccountResponse;

            string userId = Guid.NewGuid().ToString("N").Substring(0, 16);
            string password = Guid.NewGuid().ToString("N").Substring(0, 8);

            // TODO: Get the real rate type here!
            upsAccountEntity = new UpsAccountEntity()
                {
                    AccountNumber = openAccountResponse.ShipperNumber,
                    City = openAccountResponse.BillingAddressCandidate.City,
                    Company = openAccountRequest.BillingAddress.CompanyName,
                    CountryCode = openAccountResponse.BillingAddressCandidate.CountryCode,
                    Description = string.Empty,
                    Email = openAccountRequest.BillingAddress.EmailAddress,
                    FirstName = openAccountRequest.BillingAddress.ContactName,
                    LastName = openAccountRequest.BillingAddress.ContactName,
                    MiddleName = openAccountRequest.BillingAddress.ContactName,
                    UserID = userId,
                    Password = password,
                    Phone = openAccountRequest.BillingAddress.Phone.Number,
                    PostalCode = openAccountResponse.BillingAddressCandidate.PostalCode,
                    StateProvCode = openAccountResponse.BillingAddressCandidate.State,
                    Street1 = openAccountResponse.BillingAddressCandidate.StreetAddress,
                    Website = "DO WE STORE THIS???",
                    RateType = (int)UpsRateType.Occasional
                };

            upsAccountEntity.Description = UpsAccountManager.GetDefaultDescription(upsAccountEntity);
            upsAccountEntity.InitializeNullsToDefault();

            // TODO: See if we can get the UpsAccessKey from the response.  

            UpsOpenAccountRepository upsAccountRepository = new UpsOpenAccountRepository();
            upsAccountRepository.Save(upsAccountEntity);

            // TODO: Hoping that we don't have to send this...we'll see.
            string upsLicense = "";
            UpsUtility.FetchAndSaveUpsAccessKey(upsAccountEntity, upsLicense);
        }

        /// <summary>
        /// Checks a response for valid properties
        /// </summary>
        private void Validate(ICarrierResponse carrierResponse)
        {
            UpsOpenAccountResponse upsOpenAccountResponse = carrierResponse as UpsOpenAccountResponse;

            if (upsOpenAccountResponse == null || upsOpenAccountResponse.NativeResponse == null)
            {
                throw new UpsOpenAccountException("No response was received from UPS.");
            }

            openAccountResponse = upsOpenAccountResponse.NativeResponse as OpenAccountResponse;

            if (openAccountResponse.Response.ResponseStatus.Code == EnumHelper.GetApiValue(UpsOpenAccountResponseStatusCode.Failed))
            {
                throw new UpsOpenAccountException(openAccountResponse.Response.Alert);
            }
        }
    }
}
