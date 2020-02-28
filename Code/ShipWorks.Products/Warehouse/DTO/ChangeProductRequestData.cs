using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Request data for changing a product on the Hub
    /// </summary>
    [Obfuscation]
    public class ChangeProductRequestData : IWarehouseProductRequestData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeProductRequestData(IProductVariantEntity product, string warehouseId)
        {
            Product = new ProductChange(product);
            Version = product.HubVersion.GetValueOrDefault(0);
            WarehouseId = warehouseId;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeProductRequestData(IProductVariantEntity product)
        {
            Product = new ProductChange(product);
            Version = product.HubVersion.GetValueOrDefault(0);
        }

        /// <summary>
        /// Known version of the product on the Hub
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Id of the warehouse which is changing the product
        /// </summary>
        [JsonProperty("warehouseId")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// Product that should be changed
        /// </summary>
        public ProductChange Product { get; set; }
    }

    /// <summary>
    /// Product DTO 
    /// </summary>
    [Obfuscation]
    public class ProductChange
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductChange(IProductVariantEntity product)
        {
            Name = product.Name;
            Upc = product.UPC;
            Asin = product.ASIN;
            Isbn = product.ISBN;
            Weight = product.Weight;
            Length = product.Length;
            Width = product.Width;
            Height = product.Height;
            ImageUrl = product.ImageUrl;
            BinLocation = product.BinLocation;
            HarmonizedCode = product.HarmonizedCode;
            DeclaredValue = product.DeclaredValue;
            CountryOfOrigin = product.CountryOfOrigin;
            Fnsku = product.FNSku;
            Ean = product.EAN;
            Enabled = product.IsActive;
            IsBundle = product.Product.IsBundle;

            Attributes = product.AttributeValues.EmptyIfNull().Select(ProductAttribute.Create);
            Aliases = product.Aliases.EmptyIfNull().Where(x => !x.IsDefault).Select(ProductAlias.Create);
            BundledProducts = product.Product.Bundles.EmptyIfNull().Select(BundledProduct.Create);
        }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// UPC of the product
        /// </summary>
        public string Upc { get; set; }

        /// <summary>
        /// ASIN of the product
        /// </summary>
        public string Asin { get; set; }

        /// <summary>
        /// ASIN of the product
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// Weight of the product
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Length of the product
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// Width of the product
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// Height of the product
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Url of the image for the product
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Bin location of the product
        /// </summary>
        public string BinLocation { get; set; }

        /// <summary>
        /// Harmonized code of the product
        /// </summary>
        public string HarmonizedCode { get; set; }

        /// <summary>
        /// Declared value of the product
        /// </summary>
        public decimal? DeclaredValue { get; set; }

        /// <summary>
        /// Country of origin of the product
        /// </summary>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// FNSKU of the product
        /// </summary>
        public string Fnsku { get; set; }

        /// <summary>
        /// EAN of the product
        /// </summary>
        public string Ean { get; set; }

        /// <summary>
        /// Whether or not the product is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Whether or not the product is a bundle
        /// </summary>
        public bool IsBundle { get; set; }

        /// <summary>
        /// Attributes of the product
        /// </summary>
        public IEnumerable<ProductAttribute> Attributes { get; set; }

        /// <summary>
        /// Aliases for this product
        /// </summary>
        public IEnumerable<ProductAlias> Aliases { get; set; }

        /// <summary>
        /// Products that are bundled in this one
        /// </summary>
        public IEnumerable<BundledProduct> BundledProducts { get; private set; }
    }
}
