using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    /// <summary>
    /// Setup carrier configurations downloaded from the Hub
    /// </summary>
    public interface ICarrierSetup
    {
        /// <summary>
        /// Setup the carrier with the given config
        /// </summary>
        Task Setup(CarrierConfiguration config, UspsAccountEntity uspsOneBalanceAccount);
    }
}
