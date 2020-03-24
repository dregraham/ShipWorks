using System;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Entity class which represents the entity 'Product'.
    /// </summary>
    public partial class ProductEntity
    {
        /// <summary>
        /// Create a new default Product
        /// </summary>
        public static ProductEntity Create()
        {
            return new ProductEntity()
            {
                    IsActive = true,
                    IsBundle = false,
                    CreatedDate = DateTime.UtcNow,
            };
        }
    }
}