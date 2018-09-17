using System.Threading.Tasks;
using Interapptive.Shared.Business;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Interface for AddressValidationWebClient
    /// </summary>
    public interface IAddressValidationWebClient
    {
        /// <summary>
        /// Validates the address.
        /// </summary>
        Task<AddressValidationWebClientValidateAddressResult> ValidateAddressAsync(AddressAdapter addressAdapter);
    }
}