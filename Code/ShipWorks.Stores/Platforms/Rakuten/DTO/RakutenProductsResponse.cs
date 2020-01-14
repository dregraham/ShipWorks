using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// The GetProducts response from Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenProductsResponse : RakutenBaseResponse
    {
        /// <summary>
        /// The base product SKU
        /// </summary>
        [JsonProperty("baseSku")]
        public string BaseSKU { get; set; }

        /// <summary>
        /// Dictionary of attributes each variant has. 
        /// </summary>
        [JsonProperty("variantAttributeNames")]
        public Dictionary<string, Dictionary<string, string>> VariantAttributeNames { get; set; }

        /// <summary>
        /// Rakuten product variants
        /// </summary>
        [JsonProperty("variants")]
        public List<RakutenProductVariant> Variants { get; set; }

        /// <summary>
        /// The shop-specific details for this product
        /// </summary>
        [JsonProperty("productListedShops")]
        public List<RakutenProductDetails> ShopSpecificProductDetails { get; set; }
    }

    /// <summary>
    /// A Rakuten product variant
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenProductVariant
    {
        /// <summary>
        /// The variant-specific SKU
        /// </summary>
        [JsonProperty("sku")]
        public string SKU { get; set; }

        /// <summary>
        /// The variant-specific attributes
        /// </summary>
        [JsonProperty("variantAttributes")]
        public Dictionary<string, Dictionary<string, string>> VariantAttributes { get; set; }

        /// <summary>
        /// The manufacturer's part number of the variant
        /// </summary>
        [JsonProperty("manufacturerPartNumber")]
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// The variant-specific global trade items
        /// </summary>
        [JsonProperty("globalTradeItems")]
        public Dictionary<string, string> GlobalTradeItems { get; set; }

        /// <summary>
        /// Weight and dims of the product variant
        /// </summary>
        [JsonProperty("shippingPackage")]
        public RakutenProductShippingInfo ShippingInfo { get; set; }
    }

    /// <summary>
    /// The shop-specific details of a Rakuten product
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenProductDetails
    {
        /// <summary>
        /// The shop these details are specific to
        /// </summary>
        [JsonProperty("shopKey")]
        public RakutenShopKey ShopKey { get; set; }

        /// <summary>
        /// The langauge-specific product title
        /// </summary>
        [JsonProperty("title")]
        public Dictionary<string, string> Title { get; set; }

        /// <summary>
        /// The language-specific product description
        /// </summary>
        [JsonProperty("description")]
        public Dictionary<string, string> Description { get; set; }

        /// <summary>
        /// The brand of the product
        /// </summary>
        [JsonProperty("brand")]
        public string Brand { get; set; }

        /// <summary>
        /// The product images
        /// </summary>
        [JsonProperty("images")]
        public List<RakutenProductImage> Images { get; set; }

        /// <summary>
        /// The variant-specific info for this shop's product
        /// </summary>
        [JsonProperty("variantInfos")]
        public Dictionary<string, RakutenVariantInfo> VariantSpecificInfo { get; set; }

        /// <summary>
        /// Internal notes for this product
        /// </summary>
        [JsonProperty("internalNotes")]
        public Dictionary<string, string> InternalNotes { get; set; }
    }

    /// <summary>
    /// Weight and dims of a Rakuten product
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenProductShippingInfo
    {
        /// <summary>
        /// The weight
        /// </summary>
        [JsonProperty("weight")]
        public RakutenProductWeight Weight { get; set; }

        /// <summary>
        /// The dimensions
        /// </summary>
        [JsonProperty("dimensions")]
        public RakutenProductDimensions Dimensions { get; set; }
    }

    /// <summary>
    /// The Rakuten product variant weight
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenProductWeight
    {
        /// <summary>
        /// The unit used in measuring the weight
        /// </summary>
        [JsonProperty("unit")]
        public string Unit { get; set; }

        /// <summary>
        /// The weight
        /// </summary>
        [JsonProperty("value")]
        public double Value { get; set; }
    }

    /// <summary>
    /// The Rakuten product variant dimensions
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenProductDimensions
    {
        /// <summary>
        /// The unit of measurement
        /// </summary>
        [JsonProperty("unit")]
        public string Unit { get; set; }

        /// <summary>
        /// The depth
        /// </summary>
        [JsonProperty("depth")]
        public decimal Length { get; set; }

        /// <summary>
        /// The width
        /// </summary>
        [JsonProperty("width")]
        public decimal Width { get; set; }

        /// <summary>
        /// The height
        /// </summary>
        [JsonProperty("height")]
        public decimal Height { get; set; }
    }

    /// <summary>
    /// A Rakuten product image
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenProductImage
    {
        /// <summary>
        /// The image URL
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; set; }
    }

    /// <summary>
    /// The variant info specific to a product and shop
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenVariantInfo
    {
        /// <summary>
        /// This variant's images
        /// </summary>
        [JsonProperty("images")]
        public List<RakutenProductImage> Images { get; set; }
    }
}
