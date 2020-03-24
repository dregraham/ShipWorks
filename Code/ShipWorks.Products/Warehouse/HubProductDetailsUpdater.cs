using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Update details about a product
    /// </summary>
    public class HubProductDetailsUpdater : IHubProductItemUpdater
    {
        /// <summary>
        /// Update the product variant with the data from the hub
        /// </summary>
        public void UpdateProductVariant(ProductVariantEntity productVariant, WarehouseProduct warehouseProduct)
        {
            productVariant.HubVersion = warehouseProduct.Version;
            productVariant.HubSequence = warehouseProduct.Sequence;
            productVariant.HubProductId = Guid.Parse(warehouseProduct.ProductId);
            productVariant.Name = warehouseProduct.Name;
            productVariant.UPC = warehouseProduct.Upc;
            productVariant.ASIN = warehouseProduct.Asin;
            productVariant.ISBN = warehouseProduct.Isbn;
            productVariant.Weight = warehouseProduct.Weight;
            productVariant.Length = warehouseProduct.Length;
            productVariant.Width = warehouseProduct.Width;
            productVariant.Height = warehouseProduct.Height;
            productVariant.ImageUrl = warehouseProduct.ImageUrl;
            productVariant.HarmonizedCode = warehouseProduct.HarmonizedCode;
            productVariant.DeclaredValue = warehouseProduct.DeclaredValue;
            productVariant.CountryOfOrigin = warehouseProduct.CountryOfOrigin;
            productVariant.FNSku = warehouseProduct.Fnsku;
            productVariant.EAN = warehouseProduct.Ean;
            productVariant.Product.IsActive = warehouseProduct.Enabled;
            productVariant.Product.IsBundle = warehouseProduct.IsBundle;
            productVariant.CreatedDate = warehouseProduct.CreatedDate.ToSqlSafeDateTime();
            
            productVariant.IsActive = warehouseProduct.Enabled;
        }
    }
}
