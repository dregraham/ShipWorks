﻿namespace ShipWorks.Shipping.ShipEngine.DTOs.Registration
{
    /// <summary>
    /// Request DTO for registering 
    /// </summary>
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
