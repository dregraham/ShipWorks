using System;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

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
        private readonly Func<IProductEditorDialog> createDialog;
        private readonly IMessageHelper messageHelper;
        private readonly ISqlAdapterFactory adapterFactory;
        private ProductVariantAliasEntity product;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductEditorViewModel(Func<IProductEditorDialog> createDialog, IMessageHelper messageHelper, ISqlAdapterFactory adapterFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;
            this.adapterFactory = adapterFactory;
        }

        [Obfuscation(Exclude = true)]
        public bool IsActive
        {
            get => isActive;
            set => handler.Set(nameof(IsActive), ref isActive, value);
        }

        [Obfuscation(Exclude = true)]
        public DateTime CreatedDate
        {
            get => createdDate;
            set => handler.Set(nameof(CreatedDate), ref createdDate, value);
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
        /// Show the product editor
        /// </summary>
        public void ShowProductEditor(ProductVariantAliasEntity product)
        {
            // Ensure the product has a product variant
            // new ProductVariantAliasEntity does not have a ProductVarientEntity
            if (product.ProductVariant == null)
            {
                product.ProductVariant = new ProductVariantEntity();
            }

            // Ensure the product variant has a product
            // new ProductVarientEntity does not have a ProductEntity
            if (product.ProductVariant.Product == null)
            {
                product.ProductVariant.Product = new ProductEntity();
            }

            this.product = product;

            SKU = product.Sku;
            IsActive = product.ProductVariant.IsActive;
            Name = product.ProductVariant.Name;
            UPC = product.ProductVariant.UPC;
            ASIN = product.ProductVariant.ASIN;
            ISBN = product.ProductVariant.ISBN;
            Weight = product.ProductVariant.Weight ?? 0;
            Length = product.ProductVariant.Length ?? 0;
            Width = product.ProductVariant.Width ?? 0;
            Height = product.ProductVariant.Height ?? 0;
            ImageUrl = product.ProductVariant.ImageUrl;
            BinLocation = product.ProductVariant.BinLocation;
            HarmonizedCode = product.ProductVariant.HarmonizedCode;
            DeclaredValue = product.ProductVariant.DeclaredValue ?? 0;
            CountryOfOrigin = product.ProductVariant.CountryOfOrigin;

            IProductEditorDialog dialog = createDialog();
            dialog.DataContext = this;
            messageHelper.ShowDialog(dialog);
        }

        /// <summary>
        /// Save the product
        /// </summary>
        private void SaveProduct()
        {
            product.Sku = SKU;
            product.ProductVariant.IsActive = IsActive;
            product.ProductVariant.Name = Name;
            product.ProductVariant.UPC = UPC;
            product.ProductVariant.ASIN = ASIN;
            product.ProductVariant.ISBN = ISBN;
            product.ProductVariant.Weight = Weight;
            product.ProductVariant.Length = Length;
            product.ProductVariant.Width = Width;
            product.ProductVariant.Height = Height;
            product.ProductVariant.ImageUrl = ImageUrl;
            product.ProductVariant.BinLocation = BinLocation;
            product.ProductVariant.HarmonizedCode = HarmonizedCode;
            product.ProductVariant.DeclaredValue = DeclaredValue;
            product.ProductVariant.CountryOfOrigin = CountryOfOrigin;

            using (ISqlAdapter adapter = adapterFactory.Create())
            {
                try
                {
                    adapter.SaveEntity(product.ProductVariant.Product);
                }
                catch (Exception ex)
                {
                    messageHelper.ShowError("An error occurred while saving the product", ex);
                }
            }
        }
    }
}
