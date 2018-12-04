using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.BundleEditor
{
    /// <summary>
    /// Interface for the bundle editor view model
    /// </summary>
    public interface IBundleEditorViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Sku the user enters
        /// </summary>
        string Sku { get; set; }

        /// <summary>
        /// Quantity the user enters
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        /// The list of bundled skus displayed to the user.
        /// </summary>
        List<ProductBundleDisplayLineItem> BundleLineItems { get; set; }

        /// <summary>
        /// The bundle line item the user has selected
        /// </summary>
        ProductBundleDisplayLineItem SelectedBundleLineItem { get; set; }

        /// <summary>
        /// Command for adding a sku to the bundle
        /// </summary>
        ICommand AddSkuToBundleCommand { get; }

        /// <summary>
        /// Command for removing a sku from the bundle
        /// </summary>
        ICommand RemoveSkuFromBundleCommand { get; }

        /// <summary>
        /// Load the view model with the given base product
        /// </summary>
        void Load(ProductVariantEntity productVariant);

        /// <summary>
        /// Save the bundle to the product
        /// </summary>
        void Save();
    }
}