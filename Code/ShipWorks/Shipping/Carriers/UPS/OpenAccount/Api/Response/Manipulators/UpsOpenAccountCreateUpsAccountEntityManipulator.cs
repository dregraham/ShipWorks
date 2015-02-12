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

        public void Manipulate(ICarrierResponse carrierResponse)
        {
            Validate(carrierResponse);

            UpsOpenAccountResponse upsOpenAccountResponse = carrierResponse as UpsOpenAccountResponse;
            OpenAccountRequest openAccountRequest = upsOpenAccountResponse.Request.NativeRequest as OpenAccountRequest;
            openAccountResponse = upsOpenAccountResponse.NativeResponse as OpenAccountResponse;

            UpsAccountEntity upsAccount = upsOpenAccountResponse.Request.CarrierAccountEntity as UpsAccountEntity;

            upsAccount.AccountNumber = openAccountResponse.ShipperNumber;
            upsAccount.RateType = GetRateType(openAccountRequest.PickupInformation.PickupOption.Code);
            
            upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
            upsAccount.InitializeNullsToDefault();
        }

        /// <summary>
        /// Given the pickup option return the RateType.
        /// </summary>
        private static int GetRateType(string pickupOptionCode)
        {
            UpsPickupOption upsPickupOption = EnumHelper.GetEnumByApiValue<UpsPickupOption>(pickupOptionCode);
            UpsRateType rateType = UpsRateType.Occasional;

            if (upsPickupOption == UpsPickupOption.DailyOnRoute || upsPickupOption == UpsPickupOption.RegularDailyPickup)
            {
                rateType = UpsRateType.DailyPickup;
            }

            return (int)rateType;
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

            if (openAccountResponse.BillingAddressCandidate != null)
            {
                throw new UpsOpenAccountBusinessAddressException(openAccountResponse.BillingAddressCandidate);
            }

            if (openAccountResponse.PickupAddressCandidate != null)
            {
                throw new UpsOpenAccountPickupAddressException(openAccountResponse.PickupAddressCandidate);
            }
        }
    }
}
