using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.UI.BundleEditor
{
    /// <summary>
    /// View model for the BundleEditorControl
    /// </summary>
    [Component]
    public class BundleEditorViewModel : IBundleEditorViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IProductCatalog productCatalog;
        private readonly IMessageHelper messageHelper;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private string sku;
        private int quantity;
        private List<ProductBundleDisplayLineItem> bundleLineItems;
        private ProductBundleDisplayLineItem selectedBundleLineItem;
        private ProductVariantEntity productVariant;

        /// <summary>
        /// Constructor
        /// </summary>
        public BundleEditorViewModel(IProductCatalog productCatalog, IMessageHelper messageHelper, ISqlAdapterFactory sqlAdapterFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            this.productCatalog = productCatalog;
            this.messageHelper = messageHelper;
            this.sqlAdapterFactory = sqlAdapterFactory;
            AddSkuToBundleCommand = new RelayCommand(AddProductToBundle);
            RemoveSkuFromBundleCommand = new RelayCommand(RemoveProductFromBundle, () => SelectedBundleLineItem != null);
        }

        /// <summary>
        /// Sku the user enters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Sku
        {
            get => sku;
            set => handler.Set(nameof(Sku), ref sku, value);
        }

        /// <summary>
        /// Quantity the user enters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Quantity
        {
            get => quantity;
            set => handler.Set(nameof(Quantity), ref quantity, value);
        }

        /// <summary>
        /// The list of bundled skus displayed to the user.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<ProductBundleDisplayLineItem> BundleLineItems
        {
            get => bundleLineItems;
            set => handler.Set(nameof(BundleLineItems), ref bundleLineItems, value);
        }

        /// <summary>
        /// The bundle line item the user has selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ProductBundleDisplayLineItem SelectedBundleLineItem
        {
            get => selectedBundleLineItem;
            set => handler.Set(nameof(SelectedBundleLineItem), ref selectedBundleLineItem, value);
        }

        /// <summary>
        /// Command for adding a sku to the bundle
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddSkuToBundleCommand { get; }

        /// <summary>
        /// Command for removing a sku from the bundle
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand RemoveSkuFromBundleCommand { get; }

        /// <summary>
        /// Load the view model with the given base product and sql adapter
        /// </summary>
        public void Load(ProductVariantEntity baseProductVariant)
        {
            productVariant = baseProductVariant;

            if (productVariant.Product.IsBundle)
            {
                foreach (ProductBundleEntity bundledProduct in productVariant.Product.Bundles)
                {
                    ProductVariantEntity bundledProductVariant = bundledProduct.Product.Variants.SingleOrDefault(
                        x => x.ProductVariantID == bundledProduct.ChildProductVariantID);

                    ProductVariantAliasEntity bundledProductVariantAlias = bundledProductVariant?.Aliases.FirstOrDefault();

                    if (bundledProductVariantAlias != null)
                    {
                        BundleLineItems.Add(new ProductBundleDisplayLineItem(bundledProduct, bundledProductVariantAlias.Sku));
                    }
                }
            }
            else
            {
                BundleLineItems = new List<ProductBundleDisplayLineItem>();
            }
        }

        /// <summary>
        /// Save the currently configured bundle
        /// </summary>
        public void Save()
        {
            productVariant.Product.Bundles.Clear();

            foreach (ProductBundleDisplayLineItem bundleLineItem in BundleLineItems)
            {
                productVariant.Product.Bundles.Add(bundleLineItem.BundledProduct);
            }
        }

        /// <summary>
        /// Add the product with the entered sku to the bundle
        /// </summary>
        private void AddProductToBundle()
        {
            ProductVariantEntity productVariant;

            // Fetch Alias from sku
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                productVariant = productCatalog.FetchProductVariantEntity(adapter, Sku);
            }

            if (productVariant != null)
            {
                ProductBundleEntity bundleEntity = new ProductBundleEntity
                {
                    ProductID = productVariant.ProductID,
                    ChildProductVariantID = productVariant.ProductVariantID,
                    Quantity = Quantity
                };

                // Add to bundled skus
                BundleLineItems.Add(new ProductBundleDisplayLineItem(bundleEntity, Sku));
            }
            else
            {
                // Could not find entered sku
                messageHelper.ShowError($"SKU {Sku} not found");
            }
        }

        /// <summary>
        /// Remove selected product from bundle
        /// </summary>
        private void RemoveProductFromBundle()
        {
            BundleLineItems.Remove(SelectedBundleLineItem);
        }
    }
}
