namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Address data as returned from the call to the validation service
    /// </summary>
    public class AddressValidationResult
    {
        /// <summary>
        /// Street 1 of the address
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Street 2 of the address
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Street 3 of the address
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// City of the address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State or province code of the address
        /// </summary>
        public string StateProvCode { get; set; }

        /// <summary>
        /// Postal code of the address
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country code of the address
        /// </summary>
        public string CountryCode { get; set; }
    }
}
