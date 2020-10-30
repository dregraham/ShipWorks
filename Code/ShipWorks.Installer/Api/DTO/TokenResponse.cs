using System;
using System.Xml.Serialization;

namespace ShipWorks.Installer.Api.DTO
{
    /// <summary>
    /// Class representing token response
    /// </summary>
    [XmlType(TypeName = "TokenResponse")]
    public class TokenResponse
    {
        /// <summary>
        /// The auth token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// A refresh token
        /// </summary>
        public string refreshToken { get; set; }

        /// <summary>
        /// A redirect token
        /// </summary>
        public string redirectToken { get; set; }

        /// <summary>
        /// Customer license key
        /// </summary>
        public string CustomerLicenseKey { get; set; }

        /// <summary>
        /// The end date of the customer's trial
        /// </summary>
        public DateTime RecurlyTrialEndDate { get; set; }

        /// <summary>
        /// An error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The result
        /// </summary>
        public int Result { get; set; }
    }
}
