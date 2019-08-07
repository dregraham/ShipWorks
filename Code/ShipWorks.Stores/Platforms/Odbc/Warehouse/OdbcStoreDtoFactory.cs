using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Stores.Platforms.Odbc.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Odbc)]
    public class OdbcStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="helpers"></param>
        public OdbcStoreDtoFactory(IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public Task<Store> Create(StoreEntity baseStoreEntity)
        {
            return Task.FromResult(helpers.PopulateCommonData(baseStoreEntity, new Store()));
        }
    }
}
