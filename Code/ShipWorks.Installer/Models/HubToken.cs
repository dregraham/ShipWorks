using System;

namespace ShipWorks.Installer.Models
{
    /// <summary>
    /// Token for logging into the Hub
    /// </summary>
    public class HubToken
    {
        /// <summary>
        /// The authorization token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The refresh token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Customer license key
        /// </summary>
        public string CustomerLicenseKey { get; set; }

        /// <summary>
        /// The end date of the customer's trial
        /// </summary>
        public DateTime RecurlyTrialEndDate { get; set; }
    }
}
