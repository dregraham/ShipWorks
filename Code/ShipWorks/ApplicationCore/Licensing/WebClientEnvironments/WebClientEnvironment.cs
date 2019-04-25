namespace ShipWorks.ApplicationCore.Licensing.WebClientEnvironments
{
    /// <summary>
    /// ShipWorks web client environment
    /// </summary>
    public class WebClientEnvironment
    {
        /// <summary>
        /// Environment name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL for Tango
        /// </summary>
        public string TangoUrl { get; set; }

        /// <summary>
        /// URL for Warehouse
        /// </summary>
        public string WarehouseUrl { get; set; }

        /// <summary>
        /// URL for Tango Activation
        /// </summary>
        public string ActivationUrl { get; set; }

        /// <summary>
        /// ShipWorks username that is put in the header
        /// </summary>
        public string HeaderShipWorksUsername { get; set; }

        /// <summary>
        /// ShipWorks password that is put in the header
        /// </summary>
        public string HeaderShipWorksPassword { get; set; }

        /// <summary>
        /// SoapAction passed to Tango
        /// </summary>
        public string SoapAction { get; set; }

        /// <summary>
        /// Force pre call certification validation
        /// </summary>
        public bool ForcePreCallCertificationValidation { get; set; }

        /// <summary>
        /// Tango security validator to use
        /// </summary>
        public ITangoSecurityValidator TangoSecurityValidator { get; set; }
    }
}