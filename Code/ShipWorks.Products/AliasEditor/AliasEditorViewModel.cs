using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.AliasEditor
{
    /// <summary>
    /// ViewModel for the AliasEditor
    /// </summary>
    [Component]
    public class AliasEditorViewModel : ViewModelBase, IAliasEditorViewModel
    {
        private string aliasName;
        private string aliasSku;
        private ObservableCollection<ProductVariantAliasEntity> productAliases;
        private ProductVariantAliasEntity selectedProductAlias;
        private ProductVariantEntity productVariant;
        private readonly IMessageHelper messageHelper;
        private readonly IProductCatalog productCatalog;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private ProductVariantAliasEntity defaultAlias;

        /// <summary>
        /// Constructor
        /// </summary>
        public AliasEditorViewModel(IMessageHelper messageHelper, IProductCatalog productCatalog, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.messageHelper = messageHelper;
            this.productCatalog = productCatalog;
            this.sqlAdapterFactory = sqlAdapterFactory;
            
            ResetFields();
            ProductAliases = new ObservableCollection<ProductVariantAliasEntity>();
            AddAliasCommand = new RelayCommand(AddAliasToProduct);
        }

        /// <summary>
        /// The alias name the user enters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AliasName
        {
            get => aliasName;
            set => Set(ref aliasName, value);
        }

        /// <summary>
        /// Alias sku the user enters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AliasSku
        {
            get => aliasSku;
            set => Set(ref aliasSku, value);
        }

        /// <summary>
        /// The list of aliases attached to the current product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<ProductVariantAliasEntity> ProductAliases
        {
            get => productAliases;
            set => Set(ref productAliases, value);
        }

        /// <summary>
        /// The product alias that the user has selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ProductVariantAliasEntity SelectedProductAlias
        {
            get => selectedProductAlias;
            set => Set(ref selectedProductAlias, value);
        }

        /// <summary>
        /// Command for adding an alias to a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddAliasCommand { get; }

        /// <summary>
        /// Load the view model with the given product
        /// </summary>
        public void Load(ProductVariantEntity productVariantEntity)
        {
            productVariant = productVariantEntity;

            // Save this to add it back in later
            defaultAlias = productVariant.Aliases.SingleOrDefault(x => x.IsDefault);
            
            // We don't want the user to be able to remove the default sku, so don't show it
            IEnumerable<ProductVariantAliasEntity> nonDefaultAliases = productVariant.Aliases.Where(x => !x.IsDefault);
            
            ProductAliases = new ObservableCollection<ProductVariantAliasEntity>(nonDefaultAliases);
        }

        /// <summary>
        /// Save the aliases to the product
        /// </summary>
        public void Save()
        {
            productVariant.Aliases.RemovedEntitiesTracker = new ProductVariantAliasCollection();
            productVariant.Aliases.Clear();

            // Add the default back in, then what the user has entered
            if (defaultAlias != null)
            {
                productVariant.Aliases.Add(defaultAlias);
            }
            
            foreach (ProductVariantAliasEntity alias in ProductAliases)
            {
                productVariant.Aliases.Add(alias);
            }
        }

        /// <summary>
        /// Add an alias to the product with the entered name and sku
        /// </summary>
        private void AddAliasToProduct()
        {
            if (string.IsNullOrWhiteSpace(AliasSku))
            {
                messageHelper.ShowError("Please enter an Alias SKU.");
                return;
            }

            if (defaultAlias?.Sku == AliasSku)
            {
                messageHelper.ShowError($"\"{AliasSku}\" is already the default SKU for this product.");
                return;
            }

            if (ProductAliases.Any(x => x.Sku == AliasSku))
            {
                messageHelper.ShowError($"This product already contains an alias with the SKU \"{AliasSku}\".");
                return;
            }

            if (IsAliasAlreadyInDatabase())
            {
                messageHelper.ShowError($"{AliasSku} already exists for another product in the database.");
                return;
            }

            ProductAliases.Add(new ProductVariantAliasEntity
            {
                ProductVariantID = productVariant.ProductVariantID,
                AliasName = AliasName,
                Sku = AliasSku
            });

            ResetFields();
        }

        /// <summary>
        /// Reset the input fields
        /// </summary>
        private void ResetFields()
        {
            AliasName = string.Empty;
            AliasSku = string.Empty;
        }

        /// <summary>
        /// Is alias already in database
        /// </summary>
        private bool IsAliasAlreadyInDatabase()
        {
            bool aliasExists = false;
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                if (productCatalog.FetchProductVariantEntity(adapter, AliasSku) != null)
                {
                    aliasExists = true;
                }
            }

            return aliasExists;
        }
    }
}