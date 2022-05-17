using System;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    /// <summary>
    /// DTO for migrating an MWS store to an SP store
    /// </summary>
    public class MigrateMwsToSpRequest
    {
        public string CountryCode { get; set; }

        public string SellingPartnerId { get; set; }

        public string MwsAuthToken { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
