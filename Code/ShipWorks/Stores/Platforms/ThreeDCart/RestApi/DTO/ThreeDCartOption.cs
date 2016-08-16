using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartOption
    {
        public int OptionID { get; set; }

        public string OptionName { get; set; }

        public bool OptionSelected { get; set; }

        public bool OptionHide { get; set; }

        public decimal OptionValue { get; set; }

        public string OptionPartNumber { get; set; }

        public int OptionSorting { get; set; }

        public string OptionImagePath { get; set; }

        public int OptionBundleCatalogId { get; set; }

        public int OptionBundleQuantity { get; set; }
    }
}