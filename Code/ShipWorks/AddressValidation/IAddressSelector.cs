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
        /// Select the specified address into 
        /// </summary>
        /// <param name="addressToUpdate"></param>
        /// <param name="selectedAddress"></param>
        /// <returns></returns>
        Task SelectAddress(AddressAdapter addressToUpdate, ValidatedAddressEntity selectedAddress);
    }
}