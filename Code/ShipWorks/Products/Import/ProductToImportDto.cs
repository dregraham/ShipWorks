using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// DTO for importing products
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ProductToImportDto
    {
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
        public string Active { get; set; }

        /// <summary>
        /// Get T value of the given string.
        /// </summary>
        public static T GetValue<T>(string text, string propertyName)
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

            throw new ProductImportException($"No value was provided for {propertyName}.");
        }
    }
}
