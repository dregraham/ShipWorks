using System.Reflection;

namespace ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount
{
    /// <summary>
    /// Request DTO for registering 
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class DhlEcommerceRegistrationRequest
    {
        public string Nickname { get; set; }

        public string ClientId { get; set; }

        public string ApiSecret { get; set; }

        public string PickupNumber { get; set; }

        public string DistributionCenter { get; set; }

        public string SoldTo { get; set; }

        public string AncillaryEndorsement { get; set; }
    }
}
