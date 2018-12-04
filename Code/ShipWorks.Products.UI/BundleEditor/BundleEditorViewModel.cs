﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.BundleEditor;

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
        private ObservableCollection<ProductBundleDisplayLineItem> bundleLineItems;
        private ProductBundleDisplayLineItem selectedBundleLineItem;
        private ProductVariantEntity baseProduct;

        /// <summary>
        /// Constructor
        /// </summary>
        public BundleEditorViewModel(IProductCatalog productCatalog, IMessageHelper messageHelper, ISqlAdapterFactory sqlAdapterFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            this.productCatalog = productCatalog;
            this.messageHelper = messageHelper;
            this.sqlAdapterFactory = sqlAdapterFactory;

            BundleLineItems = new ObservableCollection<ProductBundleDisplayLineItem>();
            AddSkuToBundleCommand = new RelayCommand(AddProductToBundle);
            RemoveSkuFromBundleCommand = new RelayCommand(RemoveProductFromBundle, () => SelectedBundleLineItem != null);
            Quantity = 1;
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
        public ObservableCollection<ProductBundleDisplayLineItem> BundleLineItems
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
        /// Load the view model with the given base product
        /// </summary>
        public void Load(ProductVariantEntity baseProductVariant)
        {
            baseProduct = baseProductVariant;
            baseProduct.Product.Bundles.RemovedEntitiesTracker = new ProductBundleCollection();

            if (baseProduct.Product.IsBundle)
            {
                // Loop through each bundled product to get their skus
                foreach (ProductBundleEntity bundledProduct in baseProduct.Product.Bundles)
                {
                    ProductVariantAliasEntity bundledProductVariantAlias =
                        bundledProduct.ChildVariant?.Aliases.FirstOrDefault(a => a.IsDefault);

                    if (bundledProductVariantAlias != null)
                    {
                        BundleLineItems.Add(
                            new ProductBundleDisplayLineItem(bundledProduct, bundledProductVariantAlias.Sku));
                    }

                    SelectedBundleLineItem = BundleLineItems?.FirstOrDefault();
                }
            }
            else
            {
                BundleLineItems.Clear();
            }
        }

        /// <summary>
        /// Save the currently configured bundle
        /// </summary>
        public void Save()
        {
            // If the base product is not a bundle, remove all of its bundle items
            if (!baseProduct.Product.IsBundle)
            {
                baseProduct.Product.Bundles.RemovedEntitiesTracker.AddRange(baseProduct.Product.Bundles);
            }

            // Delete the removed items
            if (baseProduct.Product.Bundles.RemovedEntitiesTracker.Count > 0)
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.DeleteEntityCollection(baseProduct.Product.Bundles.RemovedEntitiesTracker);
                }
            }

            // Add in any bundle items
            if (baseProduct.Product.IsBundle)
            {
                foreach (ProductBundleDisplayLineItem bundleLineItem in BundleLineItems)
                {
                    baseProduct.Product.Bundles.Add(bundleLineItem.BundledProduct);
                }
            }
        }

        /// <summary>
        /// Add the product with the entered sku to the bundle
        /// </summary>
        private void AddProductToBundle()
        {
            if (string.IsNullOrWhiteSpace(Sku))
            {
                messageHelper.ShowError("Please enter a SKU to add to the bundle");
                return;
            }

            if (Quantity <= 0)
            {
                messageHelper.ShowError("Please enter a quantity greater than 0");
                return;
            }

            if (baseProduct.Aliases.Any(a=>a.Sku == sku))
            {
                messageHelper.ShowError("A bundle cannot contain itself");
                return;
            }

            ProductVariantEntity productVariant;

            // Fetch Alias from sku
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                productVariant = productCatalog.FetchProductVariantEntity(adapter, Sku);
            }

            if (productVariant != null)
            {
                if (BundleLineItems.Any(i => i.BundledProduct.ChildProductVariantID == productVariant.ProductVariantID))
                {
                    messageHelper.ShowError("SKU already exists in bundle.");
                    return;
                }

                if (productVariant.Product.IsBundle)
                {
                    messageHelper.ShowError("The SKU refers to a bundle. A bundle cannot contain another bundle.");
                    return;
                }

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

            Sku = string.Empty;
            Quantity = 1;
        }

        /// <summary>
        /// Remove selected product from bundle
        /// </summary>
        private void RemoveProductFromBundle()
        {
            ProductBundleEntity bundleProductToDelete = baseProduct.Product.Bundles.FirstOrDefault(b => b.ProductID == SelectedBundleLineItem.BundledProduct.ProductID);

            if (bundleProductToDelete != null)
            {
                baseProduct.Product.Bundles.Remove(bundleProductToDelete);
            }

            BundleLineItems.Remove(SelectedBundleLineItem);
        }
    }
}
