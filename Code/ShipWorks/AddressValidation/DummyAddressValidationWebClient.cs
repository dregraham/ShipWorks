using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.AddressValidation
{
    public class DummyAddressValidationWebClient : IAddressValidationWebClient
    {
        public List<AddressValidationResult> ValidateAddress(string street1, string street2, string city, string state, string zip)
        {
            return new List<AddressValidationResult>
            {
                new AddressValidationResult
                {
                    Street1 = "1 Memorial Dr.",
                    Street2 = "Suite 2000",
                    City = "St. Louis",
                    StateProvCode = "MO",
                    CountryCode = "US",
                    PostalCode = "63123",
                    IsValid = true
                }
            };
        }
    }
}
