using Interapptive.Shared.Business;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Address data as returned from the call to the validation service
    /// </summary>
    public class AddressValidationResult
    {
        /// <summary>
        /// Creates a new AddressValidationResult
        /// </summary>
        public AddressValidationResult()
        {
            // Default all the fields to empty strings instead of nulls to
            // reduce the amount of null checks before copying
            Street1 = string.Empty;
            Street2 = string.Empty;
            Street3 = string.Empty;
            City = string.Empty;
            StateProvCode = string.Empty;
            PostalCode = string.Empty;
            CountryCode = string.Empty;
        }

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

        /// <summary>
        /// Defines whether an address is valid and usable
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Checks whether this result is equal to the address in the adapter
        /// </summary>
        public bool IsEqualTo(PersonAdapter adapter)
        {
            if (adapter == null)
            {
                return false;
            }

            return
                Street1 == adapter.Street1 &&
                Street2 == adapter.Street2 &&
                Street3 == adapter.Street3 &&
                City == adapter.City &&
                StateProvCode == adapter.StateProvCode &&
                PostalCode == adapter.PostalCode &&
                CountryCode == adapter.CountryCode;
        }

        /// <summary>
        /// Copies the address of this result into the specified adapter
        /// </summary>
        public void CopyTo(PersonAdapter adapter)
        {
            adapter.Street1 = Street1;
            adapter.Street2 = Street2;
            adapter.Street3 = Street3;
            adapter.City = City;
            adapter.StateProvCode = StateProvCode;
            adapter.PostalCode = PostalCode;
            adapter.CountryCode = CountryCode;
        }
    }
}     