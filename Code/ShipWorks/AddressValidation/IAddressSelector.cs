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
        Task<AddressAdapter> SelectAddress(AddressAdapter addressToUpdate, ValidatedAddressEntity selectedAddress);
        
        /// <summary>
        /// Format the address for display in the menu
        /// </summary>
        string FormatAddress(ValidatedAddressEntity x);
    }
}