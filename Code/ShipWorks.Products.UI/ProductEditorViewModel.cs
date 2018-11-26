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
        private readonly IProductEditorDialog dialog;
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
        public ProductEditorViewModel(IProductEditorDialog dialog, IMessageHelper messageHelper, ISqlAdapterFactory adapterFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.dialog = dialog;
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
            this.product = product;
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
                adapter.SaveEntity(product.ProductVariant);
                adapter.SaveEntity(product);
            }
        }
    }
}
