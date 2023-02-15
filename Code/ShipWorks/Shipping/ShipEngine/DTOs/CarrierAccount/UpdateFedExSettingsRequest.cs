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
            this.Nickname = account.Email;
            this.SmartPostHub = EnumHelper.GetApiValue((FedExSmartPostHub) account.SmartPostHub);
            this.InvoiceSignatureImage = account.Signature;
            this.InvoiceLetterheadImage = account.Letterhead;

            //The update request requires the endorsment to be set.
            //However we are sending the endorsement with each request instead of using the account setting.
            //So just set it to the default
            this.SmartPostEndorsement = EnumHelper.GetApiValue(FedExSmartPostEndorsement.AddressCorrection);
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
