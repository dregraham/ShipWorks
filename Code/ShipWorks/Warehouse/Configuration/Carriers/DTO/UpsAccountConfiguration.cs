using System.Reflection;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Warehouse.Configuration.Carriers.DTO
{
    /// <summary>
    /// DTO for importing UPS account data from the hub
    /// </summary>
    [Obfuscation]
    public class UpsAccountConfiguration
    {
        /// <summary>
        /// The UPS access token
        /// </summary>
        public string CustomerAccessNumber { get; set; }

        /// <summary>
        /// The UPS account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The UPS rate type
        /// </summary>
        public UpsRateType RateType { get; set; }

        /// <summary>
        /// Whether or not the customer entered invoice info
        /// </summary>
        public bool InvoiceAuth { get; set; }

        /// <summary>
        /// The customers generated username
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The customers generated password
        /// </summary>
        public string Password { get; set; }
    }
}
