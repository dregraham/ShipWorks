using System;
using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Product DTO 
    /// </summary>
    [Obfuscation]
    class WarehouseProduct
    {
        /// <summary>
        /// Id of the product in the hub
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Version of the product in the hub
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Sequence of the product in the hub
        /// </summary>
        public long Sequence { get; set; }

        /// <summary>
        /// SKU of the product
        /// </summary>
        public string Sku { get; set; }

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
        /// When the product was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

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
        public IEnumerable<BundledProduct> BundledProducts { get; set; }

        /// <summary>
        /// Warehouse associations
        /// </summary>
        public IDictionary<string, WarehouseProductWarehouse> Warehouses { get; set; }
    }
}
