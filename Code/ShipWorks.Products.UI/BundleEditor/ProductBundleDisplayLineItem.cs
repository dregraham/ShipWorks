using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.UI.BundleEditor
{
    /// <summary>
    /// DTO for displaying product bundle line items
    /// </summary>
    public class ProductBundleDisplayLineItem
    {
        private readonly string sku;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductBundleDisplayLineItem(ProductBundleEntity bundledProduct, string sku)
        {
            BundledProduct = bundledProduct;
            this.sku = sku;
        }
        
        /// <summary>
        /// The product that is part of the bundle
        /// </summary>
        public ProductBundleEntity BundledProduct { get; }
        
        /// <summary>
        /// Text to display for the bundle line item
        /// </summary>
        public string DisplayText => $"{BundledProduct.Quantity}x {sku}";
    }
}