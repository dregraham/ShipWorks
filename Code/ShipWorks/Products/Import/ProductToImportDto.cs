using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// DTO for importing products
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ProductToImportDto
    {
        private static readonly string[] ActiveValues = new[] { "ACTIVE", "YES", "TRUE", "1" };
        private static readonly string SkuSeparatorRegex = @"(?<!($|[^\\])(\\\\)*?\\)\|";
        private static readonly string SkuQuantitySeparatorRegex = @"(?<!($|[^\\])(\\\\)*?\\):";
        private static List<string> propertyNames = new List<string>();

        [DisplayName("SKU")]
        public string Sku { get; set; }

        [DisplayName("Alias SKUs")]
        public string AliasSkus { get; set; }

        [DisplayName("Bundled SKUs")]
        public string BundleSkus { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("UPC")]
        public string Upc { get; set; }

        [DisplayName("ASIN")]
        public string Asin { get; set; }

        [DisplayName("ISBN")]
        public string Isbn { get; set; }

        [DisplayName("Weight")]
        public string Weight { get; set; }

        [DisplayName("Length")]
        public string Length { get; set; }

        [DisplayName("Width")]
        public string Width { get; set; }

        [DisplayName("Height")]
        public string Height { get; set; }

        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }

        [DisplayName("Warehouse-Bin Location")]
        public string WarehouseBin { get; set; }

        [DisplayName("Declared Value")]
        public string DeclaredValue { get; set; }

        [DisplayName("Country of Origin")]
        public string CountryOfOrigin { get; set; }

        [DisplayName("Harmonized Code")]
        public string HarmonizedCode { get; set; }

        [DisplayName("Active")]
        public string Active { private get; set; }

        public bool IsActive => ActiveValues.Contains(Active?.ToUpperInvariant());

        public IEnumerable<string> AliasSkuList => AliasSkus.IsNullOrWhiteSpace() ? 
            Enumerable.Empty<string>() :
            Regex.Split(AliasSkus, SkuSeparatorRegex, RegexOptions.IgnoreCase)
                .Where(s => !s.Equals(Sku.Trim(), StringComparison.InvariantCultureIgnoreCase))
                .Distinct();

        public IEnumerable<(string Sku, int Quantity)> BundleSkuList
        {
            get
            {
                if (BundleSkus.IsNullOrWhiteSpace())
                {
                    return Enumerable.Empty<(string, int)>();
                }

                return Regex.Split(BundleSkus, SkuSeparatorRegex, RegexOptions.IgnoreCase)
                    .Select(skuAndQty => Regex.Split(skuAndQty, SkuQuantitySeparatorRegex, RegexOptions.IgnoreCase))
                    .Where(values => !(values.Length == 1 && values[0].IsNullOrWhiteSpace())) // Ignore extra beginning/ending delimiters
                    .Select(values =>
                    {
                        if (values.Length != 2)
                        {
                            throw new ProductImportException($"Quantity is required, but wasn't supplied for bundled item SKU {values[0]}");
                        }

                        string testSku = Regex.Unescape(values[0]);
                        string testQty = Regex.Unescape(values[1]);
                        if (testSku.Equals(Sku, StringComparison.InvariantCultureIgnoreCase) || 
                                            AliasSkuList.Any(a => a.Equals(testSku, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            throw new ProductImportException($"Bundles may not be composed of its SKU or any of its alias SKUs.  Problem SKU: {testSku}");
                        }

                        int quantity = GetValue(testQty, "Bundle Quantity", 0);
                        if (quantity <= 0)
                        {
                            throw new ProductImportException($"Quantity must be greater than 0 for bundled item SKU {values[0]}");
                        }

                        return (testSku, quantity);
                    });
            }
        }

        /// <summary>
        /// Get T value of the given string.
        /// </summary>
        public static T GetValue<T>(string text, string propertyName, T defaultValue)
        {
            if (!text.IsNullOrWhiteSpace())
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T) converter.ConvertFromString(text);
                    }
                }
                catch 
                {
                }

                throw new ProductImportException($"Unable to convert '{propertyName}' with value {text.Trim()} to a number.");
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the list of column names based on each property's DisplayName attribute
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public static List<string> PropertyNames
        {
            get
            {
                if (propertyNames.None())
                {
                    propertyNames = typeof(ProductToImportDto).GetProperties().Where(p => p.Name != nameof(PropertyNames))
                        .Select(p => p.GetCustomAttributes(typeof(DisplayNameAttribute), true).Select(a => ((DisplayNameAttribute) a).DisplayName).FirstOrDefault())
                        .ToList();
                }

                return propertyNames;
            }
        }
    }
}
