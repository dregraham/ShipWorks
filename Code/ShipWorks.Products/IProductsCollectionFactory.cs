using DataVirtualization;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products
{
    /// <summary>
    /// Factory for creating product collections
    /// </summary>
    public interface IProductsCollectionFactory
    {
        /// <summary>
        /// Create a collection of products
        /// </summary>
        DataWrapper<IVirtualizingCollection<IProductListItemEntity>> Create(bool includeInactiveProducts, IBasicSortDefinition sortDefinition);
    }
}