﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Product DTO 
    /// </summary>
    [Obfuscation]
    public class Product
    {
        /// <summary>
        /// Create a product DTO from a product variant
        /// </summary>
        public static Product Create(IProductVariantEntity product) =>
            new Product(product);

        /// <summary>
        /// Constructor
        /// </summary>
        public Product(IProductVariantEntity product)
        {
            HubProductId = product.HubProductId?.ToString("D");
            HubVersion = product.HubVersion;
            Sku = product.DefaultSku;
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
            CreatedDate = product.CreatedDate;
            IsBundle = product.Product.IsBundle;

            Attributes = product.AttributeValues.EmptyIfNull().Select(ProductAttribute.Create);
            Aliases = product.Aliases.EmptyIfNull().Where(x => !x.IsDefault).Select(ProductAlias.Create);
            BundledProducts = product.Product.Bundles.EmptyIfNull().Select(BundledProduct.Create);
        }

        /// <summary>
        /// Id of the product in the hub, if available
        /// </summary>
        public string HubProductId { get; set; }

        /// <summary>
        /// Version of the product in the hub, if available
        /// </summary>
        public int? HubVersion { get; set; }

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
        public IEnumerable<BundledProduct> BundledProducts { get; private set; }
    }

    /// <summary>
    /// A product attribute for sending to the Hub
    /// </summary>
    [Obfuscation]
    public class ProductAttribute
    {
        /// <summary>
        /// Create a new product attribute
        /// </summary>
        public static ProductAttribute Create(IProductVariantAttributeValueEntity attribute) =>
            new ProductAttribute
            {
                Name = attribute.ProductAttribute.AttributeName,
                Value = attribute.AttributeValue
            };

        /// <summary>
        /// Name of the attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the attribute
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// A product alias for sending to the Hub
    /// </summary>
    [Obfuscation]
    public class ProductAlias
    {
        /// <summary>
        /// Create a product alias
        /// </summary>
        public static ProductAlias Create(IProductVariantAliasEntity alias) =>
            new ProductAlias
            {
                Name = alias.AliasName,
                Sku = alias.Sku
            };

        /// <summary>
        /// Name of the alias
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sku of the alias
        /// </summary>
        public string Sku { get; set; }
    }
}
