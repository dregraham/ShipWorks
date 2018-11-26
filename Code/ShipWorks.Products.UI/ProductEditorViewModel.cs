using System;
using System.ComponentModel;
using System.Reflection;
using ShipWorks.Core.UI;

namespace ShipWorks.Products.UI
{
    public class ProductEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        private bool isActive;
        private DateTime createdDate;
        private string sku;
        private string name;
        private string upc;
        private string asin;
        private string isbn;
        private double weight;
        private double length;
        private double width;
        private double height;
        private string imageUrl;
        private string binLocation;
        private string harmonizedCode;
        private decimal declaredValue;
        private string countryOfOrigin;

        public ProductEditorViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
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
        public double Weight
        {
            get => weight;
            set => handler.Set(nameof(Weight), ref weight, value);
        }

        [Obfuscation(Exclude = true)]
        public double Length
        {
            get => length;
            set => handler.Set(nameof(Length), ref length, value);
        }

        [Obfuscation(Exclude = true)]
        public double Width
        {
            get => width;
            set => handler.Set(nameof(Width), ref width, value);
        }

        [Obfuscation(Exclude = true)]
        public double Height
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
    }
}
