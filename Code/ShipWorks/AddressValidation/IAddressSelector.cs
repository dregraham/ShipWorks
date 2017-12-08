using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading.Tasks;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Interface for suggested addresses
    /// </summary>
    public interface IAddressSelector
    {
        /// <summary>
        /// Select the specified address into an existing address
        /// </summary>
        Task<AddressAdapter> SelectAddress(AddressAdapter addressToUpdate, ValidatedAddressEntity selectedAddress, StoreEntity store);
        
        /// <summary>
        /// Format the address for display in the menu
        /// </summary>
        string FormatAddress(ValidatedAddressEntity x);

        /// <summary>
        /// Text to display as the ValidationSuggestion
        /// </summary>
        string DisplayValidationSuggestionLabel(object arg);
    }
}