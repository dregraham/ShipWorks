using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.BundleEditor;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// View Model for the Product Editor
    /// </summary>
    [Component]
    public class ProductEditorViewModel : IProductEditorViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IDialog dialog;
        private readonly IMessageHelper messageHelper;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IProductCatalog productCatalog;
        private ProductVariantEntity productVariant;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductEditorViewModel(
            IProductEditorDialogFactory dialogFactory, 
            IMessageHelper messageHelper, 
            IBundleEditorViewModel bundleEditorViewModel, 
            ISqlAdapterFactory sqlAdapterFactory,
            IProductCatalog productCatalog)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            dialog = dialogFactory.Create();
            this.messageHelper = messageHelper;
            BundleEditorViewModel = bundleEditorViewModel;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.productCatalog = productCatalog;
            Save = new RelayCommand(async () => await SaveProduct());
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
        public bool IsActive
        {
            get => isActive;
            set => handler.Set(nameof(IsActive), ref isActive, value);
        }

        [Obfuscation(Exclude = true)]
        public bool IsBundle
        {
            get => isBundle;
            set => handler.Set(nameof(IsBundle), ref isBundle, value);
        }

        [Obfuscation(Exclude = true)]
        public DateTime CreatedDate
        {
            get => createdDate;
            set => handler.Set(nameof(CreatedDate), ref createdDate, value);
        }

        [Obfuscation(Exclude = true)]
        public bool IsNew
        {
            get => isNew;
            set => handler.Set(nameof(IsNew), ref isNew, value);
        }

        [Obfuscation(Exclude = true)]
        public string SKU
        {
            get => sku;
            set => handler.Set(nameof(SKU), ref sku, value);
        }

        [Obfuscation(Exclude = true)]
        public string Name
        {
            get => name;
            set => handler.Set(nameof(Name), ref name, value);
        }

        [Obfuscation(Exclude = true)]
        public string UPC
        {
            get => upc;
            set => handler.Set(nameof(UPC), ref upc, value);
        }

        [Obfuscation(Exclude = true)]
        public string ASIN
        {
            get => asin;
            set => handler.Set(nameof(ASIN), ref asin, value);
        }

        [Obfuscation(Exclude = true)]
        public string ISBN
        {
            get => isbn;
            set => handler.Set(nameof(ISBN), ref isbn, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Weight
        {
            get => weight;
            set => handler.Set(nameof(Weight), ref weight, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Length
        {
            get => length;
            set => handler.Set(nameof(Length), ref length, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Width
        {
            get => width;
            set => handler.Set(nameof(Width), ref width, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal Height
        {
            get => height;
            set => handler.Set(nameof(Height), ref height, value);
        }

        [Obfuscation(Exclude = true)]
        public string ImageUrl
        {
            get => imageUrl;
            set => handler.Set(nameof(ImageUrl), ref imageUrl, value);
        }

        [Obfuscation(Exclude = true)]
        public string BinLocation
        {
            get => binLocation;
            set => handler.Set(nameof(BinLocation), ref binLocation, value);
        }

        [Obfuscation(Exclude = true)]
        public string HarmonizedCode
        {
            get => harmonizedCode;
            set => handler.Set(nameof(HarmonizedCode), ref harmonizedCode, value);
        }

        [Obfuscation(Exclude = true)]
        public decimal DeclaredValue
        {
            get => declaredValue;
            set => handler.Set(nameof(DeclaredValue), ref declaredValue, value);
        }

        [Obfuscation(Exclude = true)]
        public string CountryOfOrigin
        {
            get => countryOfOrigin;
            set => handler.Set(nameof(CountryOfOrigin), ref countryOfOrigin, value);
        }

        /// <summary>
        /// Bundle editor
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IBundleEditorViewModel BundleEditorViewModel { get; }

        /// <summary>
        /// Show the product editor
        /// </summary>
        public bool? ShowProductEditor(ProductVariantEntity productVariant)
        {
            this.productVariant = productVariant;
            IsNew = productVariant.IsNew;

            if (!IsNew)
            {
                CreatedDate = DateTime.SpecifyKind(productVariant.CreatedDate, DateTimeKind.Utc)
                    .ToLocalTime();
            }

            BundleEditorViewModel.Load(productVariant);

            SKU = productVariant.Aliases.First(a => a.IsDefault).Sku ?? string.Empty;
            IsActive = productVariant.IsNew || productVariant.IsActive;
            IsBundle = !productVariant.IsNew && productVariant.Product.IsBundle;
            Name = productVariant.Name ?? string.Empty;
            UPC = productVariant.UPC ?? string.Empty;
            ASIN = productVariant.ASIN ?? string.Empty;
            ISBN = productVariant.ISBN ?? string.Empty;
            Weight = productVariant.Weight ?? 0;
            Length = productVariant.Length ?? 0;
            Width = productVariant.Width ?? 0;
            Height = productVariant.Height ?? 0;
            ImageUrl = productVariant.ImageUrl ?? string.Empty;
            BinLocation = productVariant.BinLocation ?? string.Empty;
            HarmonizedCode = productVariant.HarmonizedCode ?? string.Empty;
            DeclaredValue = productVariant.DeclaredValue ?? 0;
            CountryOfOrigin = productVariant.CountryOfOrigin ?? string.Empty;

            dialog.DataContext = this;
            return messageHelper.ShowDialog(dialog);
        }

        /// <summary>
        /// Save the product
        /// </summary>
        private async Task SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(SKU))
            {
                messageHelper.ShowError($"The following field is required: {Environment.NewLine}SKU");
                return;
            }

            if (IsBundle)
            {
                int inHowManyBundles = await productCatalog.InBundleCount(productVariant.ProductVariantID).ConfigureAwait(true);
                if (inHowManyBundles > 0)
                {
                    string plural = inHowManyBundles > 1 ? "s" : "";
                    string question = $"A bundle cannot be in other bundles.\r\n\r\nThis bundle is already in {inHowManyBundles} existing bundle{plural}.\r\n\r\nShould ShipWorks remove this bundle from the existing bundles{plural}? ";

                    DialogResult answer = messageHelper.ShowQuestion(question);
                    if (answer != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            productVariant.Product.IsBundle = IsBundle;

            productVariant.Aliases.First(a => a.IsDefault).Sku = SKU.Trim();
            productVariant.IsActive = IsActive;
            productVariant.Name = Name.Trim();
            productVariant.UPC = UPC.Trim();
            productVariant.ASIN = ASIN.Trim();
            productVariant.ISBN = ISBN.Trim();
            productVariant.Weight = Weight;
            productVariant.Length = Length;
            productVariant.Width = Width;
            productVariant.Height = Height;
            productVariant.ImageUrl = ImageUrl.Trim();
            productVariant.BinLocation = BinLocation.Trim();
            productVariant.HarmonizedCode = HarmonizedCode.Trim();
            productVariant.DeclaredValue = DeclaredValue;
            productVariant.CountryOfOrigin = CountryOfOrigin.Trim();

            Result saveResult;

            using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
            {
                await BundleEditorViewModel.Save(adapter).ConfigureAwait(true);
                saveResult = productCatalog.Save(adapter, productVariant.Product);
                if (saveResult.Success)
                {
                    adapter.Commit();
                }
                else
                {
                    adapter.Rollback();
                }
            }

            if (saveResult.Success)
            {
                dialog.DialogResult = true;
                dialog.Close();
            }
            else
            {
                if ((saveResult.Exception.GetBaseException() as SqlException)?.Number == 2601)
                {
                    string sku = productVariant.Aliases.First(a => a.IsDefault).Sku;
                    messageHelper.ShowError($"The SKU \"{sku}\" already exists. Please enter a unique value for the Product SKU.", saveResult.Exception);
                }
                else
                {
                    messageHelper.ShowError(saveResult.Message);
                }
            }

        }
    }
}
