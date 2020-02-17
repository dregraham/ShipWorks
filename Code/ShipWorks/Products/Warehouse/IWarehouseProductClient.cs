using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Web client for product related interactions with the ShipWorks Warehouse app
    /// </summary>
    public interface IWarehouseProductClient
    {
        /// <summary>
        /// Adds a product to the Hub
        /// </summary>
        Task<string> AddProduct(IProductVariantEntity product);
    }
}
