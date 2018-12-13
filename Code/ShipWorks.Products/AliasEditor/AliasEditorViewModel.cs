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
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.AliasEditor
{
    [Component]
    public class AliasEditorViewModel : ViewModelBase, IAliasEditorViewModel
    {
        private string aliasName;
        private string aliasSku;
        private ObservableCollection<ProductVariantAliasEntity> productAliases;
        private ProductVariantAliasEntity selectedProductAlias;
        private ProductVariantEntity productVariant;
        private readonly IMessageHelper messageHelper;
        private ProductVariantAliasEntity defaultAlias;

        /// <summary>
        /// Constructor
        /// </summary>
        public AliasEditorViewModel(IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;

            ProductAliases = new ObservableCollection<ProductVariantAliasEntity>();

            AddAliasCommand = new RelayCommand(AddAliasToProduct);
            RemoveAliasCommand = new RelayCommand(RemoveAliasFromProduct, () => SelectedProductAlias != null);
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
        /// Command for removing an alias from a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand RemoveAliasCommand { get; }

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
                messageHelper.ShowError("Please enter an alias sku.");
                return;
            }
            
            if (defaultAlias?.Sku == AliasSku)
            {
                messageHelper.ShowError($"\"{AliasSku}\" is already the default sku for this product.");
                return;
            }
            
            if (ProductAliases.Any(x => x.Sku == AliasSku))
            {
                messageHelper.ShowError($"This product already contains an alias with the sku \"{AliasSku}\".");
                return;
            }

            ProductAliases.Add(new ProductVariantAliasEntity
            {
                ProductVariantID = productVariant.ProductVariantID,
                AliasName = AliasName,
                Sku = AliasSku
            });

            AliasName = string.Empty;
            AliasSku = string.Empty;
        }

        /// <summary>
        /// Remove the selected alias from the product
        /// </summary>
        private void RemoveAliasFromProduct()
        {
            ProductAliases.Remove(SelectedProductAlias);
        }
    }
}