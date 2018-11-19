using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataVirtualization;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Products
{
    /// <summary>
    /// Provide product data
    /// </summary>
    public class ProductsProvider : IItemsProvider<IShipmentEntity>
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsProvider(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Fetch the count of products
        /// </summary>
        public int FetchCount()
        {
            var queryFactory = new QueryFactory();
            var query = queryFactory.Shipment.Select(() => ShipmentFields.ShipmentID.ToValue<long>());

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {

                return sqlAdapter.FetchScalar<int>(query);
            }
        }

        /// <summary>
        /// Fetch a range of products
        /// </summary>
        public Task<IList<IShipmentEntity>> FetchRange(int startIndex, int pageCount)
        {
            throw new NotImplementedException();
        }
    }
}
