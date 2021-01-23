using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;

namespace ShipWorks.Warehouse.Configuration.Carriers
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
