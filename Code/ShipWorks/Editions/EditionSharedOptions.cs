
namespace ShipWorks.Editions
{
    /// <summary>
    /// Encapsulates
    /// </summary>
    public class EditionSharedOptions
    {
        /// <summary>
        /// Indicates if DHL is enabled and allowed for the edition
        /// </summary>
        public bool EndiciaDhlEnabled { get; set; }

        /// <summary>
        /// Indicates if insurance is enabled and allowed for the edition
        /// </summary>
        public bool EndiciaInsuranceEnabled { get; set; }

        /// <summary>
        /// Indicates whether UPS SurePost is enabled and allowed for the edition
        /// </summary>
        /// <value><c>true</c> if UPS SurePost is enabled; otherwise, <c>false</c>.</value>
        public bool UpsSurePostEnabled { get; set; }

        /// <summary>
        /// Indicates whether consolidator support is enabled for the customer's endicia account
        /// </summary>
        public bool EndiciaConsolidatorEnabled { get; set; }

        /// <summary>
        /// Indicates whether scan based payment returns support is enabled for the customer's endicia account
        /// </summary>
        public bool EndiciaScanBasedReturnEnabled { get; set; }

        /// <summary>
        /// Indicates if insurance is enabled and allowed for the edition
        /// </summary>
        public bool StampsInsuranceEnabled { get; set; }
    }
}
