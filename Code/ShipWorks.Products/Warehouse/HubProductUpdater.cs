using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Update product a product variant with data from the Hub
    /// </summary>
    [Component]
    public class HubProductUpdater : IHubProductUpdater
    {
        private readonly IEnumerable<IHubProductItemUpdater> itemUpdaters;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubProductUpdater(WarehouseProduct productData, IEnumerable<IHubProductItemUpdater> itemUpdaters)
        {
            ProductData = productData;
            this.itemUpdaters = itemUpdaters;
        }

        /// <summary>
        /// Product data from the Hub
        /// </summary>
        public WarehouseProduct ProductData { get; }

        /// <summary>
        /// Product variant in ShipWorks
        /// </summary>
        public ProductVariantEntity ProductVariant { get; set; }

        /// <summary>
        /// Update the product variant with the data from the hub
        /// </summary>
        public void UpdateProductVariant()
        {
            foreach (var updater in itemUpdaters)
            {
                updater.UpdateProductVariant(ProductVariant, ProductData);
            }
        }
    }
}
