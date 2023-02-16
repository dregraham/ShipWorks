using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount
{
    public class UpdateFedExSettingsRequest
    {
        public UpdateFedExSettingsRequest(IFedExAccountEntity account)
        {
            Nickname = account.Email;

            InvoiceSignatureImage = account.Signature;
            InvoiceLetterheadImage = account.Letterhead;

            if (account.SmartPostHub != (int) FedExSmartPostHub.None)
            {
                SmartPostHub = EnumHelper.GetApiValue((FedExSmartPostHub) account.SmartPostHub);

                //The update request requires the endorsment to be set.
                //However we are sending the endorsement with each request instead of using the account setting.
                //So just set it to the default
                SmartPostEndorsement = EnumHelper.GetApiValue(FedExSmartPostEndorsement.AddressCorrection);
            }
        }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("smart_post_hub")]
        public string SmartPostHub { get; set; }

        [JsonProperty("smart_post_endorsement")]
        public string SmartPostEndorsement { get; set; }

        [JsonProperty("signature_image")]
        public string InvoiceSignatureImage { get; set; }

        [JsonProperty("letterhead_image")]
        public string InvoiceLetterheadImage { get; set; }
    }
}
