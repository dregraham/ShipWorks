using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Web client for product related interactions with the ShipWorks Warehouse app
    /// </summary>
    public interface IWarehouseProductClient
    {
        /// <summary>
        /// Add a product to the Hub
        /// </summary>
        Task<IProductChangeResult> AddProduct(IProductVariantEntity product);

        /// <summary>
        /// Change a product on the Hub
        /// </summary>
        Task<IProductChangeResult> ChangeProduct(IProductVariantEntity product);
    }
}
