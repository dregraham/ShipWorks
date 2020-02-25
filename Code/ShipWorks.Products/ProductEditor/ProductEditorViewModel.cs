using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.AliasEditor;
using ShipWorks.Products.AttributeEditor;
using ShipWorks.Products.BundleEditor;

namespace ShipWorks.Products.ProductEditor
{
    /// <summary>
    /// View Model for the Product Editor
    /// </summary>
    [Component]
    public class ProductEditorViewModel : ViewModelBase, IProductEditorViewModel
    {
        private readonly IDialog dialog;
        private readonly IMessageHelper messageHelper;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IProductCatalog productCatalog;
        private ProductVariantEntity productVariant;

        private bool canEdit = true;
        private bool isActive;
        private DateTime createdDate;
        private string sku;
        private string name;
        private string upc;
        private string asin;
        private string isbn;
        private decimal weight;
        private decimal length;
        private decimal width;
        private decimal height;
        private string imageUrl;
        private string binLocation;
        private string harmonizedCode;
        private decimal declaredValue;
        private string countryOfOrigin;
        private bool isNew;
        private bool isBundle;
        private string fnsku;
        private string ean;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductEditorViewModel(
            IProductEditorDialogFactory dialogFactory,
            IMessageHelper messageHelper,
            IBundleEditorViewModel bundleEditorViewModel,
            IAttributeEditorViewModel attributeEditorViewModel,
            IAliasEditorViewModel aliasEditorViewModel,
            ISqlAdapterFactory sqlAdapterFactory,
            IProductCatalog productCatalog)
        {
            dialog = dialogFactory.Create();
            this.messageHelper = messageHelper;
            BundleEditorViewModel = bundleEditorViewModel;
            AttributeEditorViewModel = attributeEditorViewModel;
            AliasEditorViewModel = aliasEditorViewModel;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.productCatalog = productCatalog;
            Save = new RelayCommand(async () => await SaveProduct().ConfigureAwait(true));
            Cancel = new RelayCommand(dialog.Close);
        }

        /// <summary>
        /// Save
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Save { get; }

        /// <summary>
        /// Cancel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Cancel { get; }

        [Obfuscation(Exclude = true)]
        public bool CanEdit
        {
            get => canEdit;
            set => Set(ref canEdit, value);
        }

        [Obfuscation(Exclude = true)]
        public bool IsActive
        {
            get => isActive;
            set => Set(ref isActive, value);
        }

        [Obfuscation(Exclude = true)]
        public bool IsBundle
        {
            get => isBundle;
            set => Set(ref isBundle, value);
        }

        [Obfuscation(Exclude = true)]
        public DateTime CreatedDate
        {
            get => createdDate;
            set => Set(ref createdDate, value);
        }

        [Obfuscation(Exclude = true)]
        public bool IsNew
        {
            get => isNew;
            set => Set(ref isNew, value);
        }

        [Obfuscation(Exclude = true)]
        public string SKU
        {
            get => sku;
            set => Set(ref sku, value);
        }

        [Obfuscation(Exclude = true)]
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        [Obfuscation(Exclude = true)]
        public string UPC
        {
            get => upc;
            set => Set(ref upc, value);
        }

        [Obfuscation(Exclude = true)]
        public string ASIN
        {
            get => asin;
            set => Set(ref asin, value);
        }

        [Obfuscation(Exclude = true)]
        public string ISBN
        {
            get => isbn;
            set => Set(ref isbn, value);
        }
        
        [Obfuscation(Exclude = true)]
        public string FNSKU
        {
            get => fnsku;
            set => Set(ref fnsku, value);
        }
        
        [Obfuscation(Exclude = true)]
        public string EAN
        {
            get => ean;
            set => Set(ref ean, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Weight
        {
            get => weight;
            set => Set(ref weight, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Length
        {
            get => length;
            set => Set(ref length, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Width
        {
            get => width;
            set => Set(ref width, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Height
        {
            get => height;
            set => Set(ref height, value);
        }

        [Obfuscation(Exclude = true)]
        public string ImageUrl
        {
            get => imageUrl;
            set => Set(ref imageUrl, value);
        }

        [Obfuscation(Exclude = true)]
        public string BinLocation
        {
            get => binLocation;
            set => Set(ref binLocation, value);
        }

        [Obfuscation(Exclude = true)]
        public string HarmonizedCode
        {
            get => harmonizedCode;
            set => Set(ref harmonizedCode, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal DeclaredValue
        {
            get => declaredValue;
            set => Set(ref declaredValue, value);
        }

        [Obfuscation(Exclude = true)]
        public string CountryOfOrigin
        {
            get => countryOfOrigin;
            set => Set(ref countryOfOrigin, value);
        }

        /// <summary>
        /// Bundle editor
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IBundleEditorViewModel BundleEditorViewModel { get; }

        /// <summary>
        /// Attribute Editor
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IAttributeEditorViewModel AttributeEditorViewModel { get; }

        /// <summary>
        /// Attribute Editor
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DialogTitle { get; private set; }

        /// <summary>
        /// View model for the alias editor
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IAliasEditorViewModel AliasEditorViewModel { get; }

        /// <summary>
        /// Show the product editor
        /// </summary>
        public async Task<bool?> ShowProductEditor(ProductVariantEntity productToLoad, string dialogTitle)
        {
            DialogTitle = dialogTitle;
            productVariant = productToLoad;
            IsNew = productToLoad.IsNew;

            if (!IsNew)
            {
                CreatedDate = DateTime.SpecifyKind(productToLoad.CreatedDate, DateTimeKind.Utc)
                    .ToLocalTime();
            }

            BundleEditorViewModel.Load(productToLoad);
            await AttributeEditorViewModel.Load(productToLoad).ConfigureAwait(true);
            AliasEditorViewModel.Load(productToLoad);

            SKU = productToLoad.DefaultSku ?? string.Empty;
            IsActive = productToLoad.IsNew || productToLoad.IsActive;
            IsBundle = !productToLoad.IsNew && productToLoad.Product.IsBundle;
            Name = productToLoad.Name ?? string.Empty;
            UPC = productToLoad.UPC ?? string.Empty;
            ASIN = productToLoad.ASIN ?? string.Empty;
            ISBN = productToLoad.ISBN ?? string.Empty;
            FNSKU = productToLoad.FNSku ?? string.Empty;
            EAN = productToLoad.EAN ?? string.Empty;
            Weight = productToLoad.Weight ?? 0;
            Length = productToLoad.Length ?? 0;
            Width = productToLoad.Width ?? 0;
            Height = productToLoad.Height ?? 0;
            ImageUrl = productToLoad.ImageUrl ?? string.Empty;
            BinLocation = productToLoad.BinLocation ?? string.Empty;
            HarmonizedCode = productToLoad.HarmonizedCode ?? string.Empty;
            DeclaredValue = productToLoad.DeclaredValue ?? 0;
            CountryOfOrigin = productToLoad.CountryOfOrigin ?? string.Empty;

            dialog.DataContext = this;
            return messageHelper.ShowDialog(dialog);
        }

        /// <summary>
        /// Save the product
        /// </summary>
        private async Task SaveProduct()
        {
            CanEdit = false;
            try
            {
                var counts = new ProductTelemetryCounts("InlineUI");

                UpdateProductEntityValues();

                Result saveResult = await productCatalog.Save(productVariant, sqlAdapterFactory).ConfigureAwait(true);

                if (saveResult.Success)
                {
                    counts.AddSuccess(IsNew);

                    dialog.DialogResult = true;
                    dialog.Close();
                }
                else
                {
                    counts.AddFailure(IsNew);

                    if ((saveResult.Exception?.GetBaseException() as SqlException)?.Number == 2601)
                    {
                        messageHelper.ShowError("A specified SKU or Alias SKU already exists. Please enter a unique value for all SKUs.", saveResult.Exception);
                    }
                    else if (!string.IsNullOrWhiteSpace(saveResult.Message))
                    {
                        messageHelper.ShowError(saveResult.Message);
                    }
                }

                counts.SendTelemetry();
            }
            finally
            {
                CanEdit = true;
            }
        }

        /// <summary>
        /// Update the product entities values with the current values in the view model fields
        /// </summary>
        private void UpdateProductEntityValues()
        {
            productVariant.Product.IsBundle = IsBundle;

            productVariant.Aliases.First(a => a.IsDefault).Sku = SKU.Trim();
            productVariant.IsActive = IsActive;
            productVariant.Name = Name.Trim();
            productVariant.UPC = UPC.Trim();
            productVariant.ASIN = ASIN.Trim();
            productVariant.ISBN = ISBN.Trim();
            productVariant.FNSku = FNSKU.Trim();
            productVariant.EAN = EAN.Trim();
            productVariant.Weight = Weight;
            productVariant.Length = Length;
            productVariant.Width = Width;
            productVariant.Height = Height;
            productVariant.ImageUrl = ImageUrl.Trim();
            productVariant.BinLocation = BinLocation.Trim();
            productVariant.HarmonizedCode = HarmonizedCode.Trim();
            productVariant.DeclaredValue = DeclaredValue;
            productVariant.CountryOfOrigin = CountryOfOrigin.Trim();

            BundleEditorViewModel.Save();
            AttributeEditorViewModel.Save();
            AliasEditorViewModel.Save();
        }
    }
}
