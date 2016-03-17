using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    public class ThreeDCartProduct
    {

        [JsonProperty("MFGID")]
        public string MFGID { get; set; }

        [JsonProperty("ShortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("ManufacturerID")]
        public int ManufacturerID { get; set; }

        [JsonProperty("LastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("UserID")]
        public string UserID { get; set; }

        [JsonProperty("GTIN")]
        public string GTIN { get; set; }

        [JsonProperty("NonTaxable")]
        public bool NonTaxable { get; set; }

        [JsonProperty("NotForSale")]
        public bool NotForSale { get; set; }

        [JsonProperty("Hide")]
        public bool Hide { get; set; }

        [JsonProperty("GiftCertificate")]
        public bool GiftCertificate { get; set; }

        [JsonProperty("HomeSpecial")]
        public bool HomeSpecial { get; set; }

        [JsonProperty("CategorySpecial")]
        public bool CategorySpecial { get; set; }

        [JsonProperty("NonSearchable")]
        public bool NonSearchable { get; set; }

        [JsonProperty("GiftWrapItem")]
        public bool GiftWrapItem { get; set; }

        [JsonProperty("ShipCost")]
        public double ShipCost { get; set; }

        [JsonProperty("Weight")]
        public double Weight { get; set; }

        [JsonProperty("Height")]
        public double Height { get; set; }

        [JsonProperty("Width")]
        public double Width { get; set; }

        [JsonProperty("Depth")]
        public double Depth { get; set; }

        [JsonProperty("SelfShip")]
        public bool SelfShip { get; set; }

        [JsonProperty("FreeShipping")]
        public bool FreeShipping { get; set; }

        [JsonProperty("RewardPoints")]
        public int RewardPoints { get; set; }

        [JsonProperty("RedeemPoints")]
        public int RedeemPoints { get; set; }

        [JsonProperty("DisableRewards")]
        public bool DisableRewards { get; set; }

        [JsonProperty("StockAlert")]
        public int StockAlert { get; set; }

        [JsonProperty("ReorderQuantity")]
        public int ReorderQuantity { get; set; }

        [JsonProperty("InStockMessage")]
        public string InStockMessage { get; set; }

        [JsonProperty("OutOfStockMessage")]
        public string OutOfStockMessage { get; set; }

        [JsonProperty("BackOrderMessage")]
        public string BackOrderMessage { get; set; }

        [JsonProperty("WarehouseLocation")]
        public string WarehouseLocation { get; set; }

        [JsonProperty("WarehouseBin")]
        public string WarehouseBin { get; set; }

        [JsonProperty("WarehouseAisle")]
        public string WarehouseAisle { get; set; }

        [JsonProperty("WarehouseCustom")]
        public string WarehouseCustom { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Keywords")]
        public string Keywords { get; set; }

        [JsonProperty("ExtraField1")]
        public string ExtraField1 { get; set; }

        [JsonProperty("ExtraField2")]
        public string ExtraField2 { get; set; }

        [JsonProperty("ExtraField3")]
        public string ExtraField3 { get; set; }

        [JsonProperty("ExtraField4")]
        public string ExtraField4 { get; set; }

        [JsonProperty("ExtraField5")]
        public string ExtraField5 { get; set; }

        [JsonProperty("ExtraField6")]
        public string ExtraField6 { get; set; }

        [JsonProperty("ExtraField7")]
        public string ExtraField7 { get; set; }

        [JsonProperty("ExtraField8")]
        public string ExtraField8 { get; set; }

        [JsonProperty("ExtraField9")]
        public string ExtraField9 { get; set; }

        [JsonProperty("ExtraField10")]
        public string ExtraField10 { get; set; }

        [JsonProperty("ExtraField11")]
        public string ExtraField11 { get; set; }

        [JsonProperty("ExtraField12")]
        public string ExtraField12 { get; set; }

        [JsonProperty("ExtraField13")]
        public string ExtraField13 { get; set; }

        [JsonProperty("MainImageFile")]
        public string MainImageFile { get; set; }

        [JsonProperty("MainImageCaption")]
        public string MainImageCaption { get; set; }

        [JsonProperty("ThumbnailFile")]
        public string ThumbnailFile { get; set; }

        [JsonProperty("MediaFile")]
        public string MediaFile { get; set; }

        [JsonProperty("AdditionalImageFile2")]
        public string AdditionalImageFile2 { get; set; }

        [JsonProperty("AdditionalImageCaption2")]
        public string AdditionalImageCaption2 { get; set; }

        [JsonProperty("AdditionalImageFile3")]
        public string AdditionalImageFile3 { get; set; }

        [JsonProperty("AdditionalImageCaption3")]
        public string AdditionalImageCaption3 { get; set; }

        [JsonProperty("AdditionalImageFile4")]
        public string AdditionalImageFile4 { get; set; }

        [JsonProperty("AdditionalImageCaption4")]
        public string AdditionalImageCaption4 { get; set; }

        [JsonProperty("ImageGalleryList")]
        public IEnumerable<ThreeDCartImageGallery> ImageGalleryList { get; set; }

        [JsonProperty("OptionSetList")]
        public IEnumerable<ThreeDCartOptionSet> OptionSetList { get; set; }

        [JsonProperty("DoNotUseCategoryOptions")]
        public bool DoNotUseCategoryOptions { get; set; }

        [JsonProperty("DateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("ListingTemplateID")]
        public int ListingTemplateID { get; set; }

        [JsonProperty("ListingTemplateName")]
        public string ListingTemplateName { get; set; }

        [JsonProperty("LoginRequiredOptionID")]
        public int LoginRequiredOptionID { get; set; }

        [JsonProperty("LoginRequiredOptionName")]
        public string LoginRequiredOptionName { get; set; }

        [JsonProperty("LoginRequiredOptionRedirectTo")]
        public string LoginRequiredOptionRedirectTo { get; set; }

        [JsonProperty("AllowAccessCustomerGroupID")]
        public int AllowAccessCustomerGroupID { get; set; }

        [JsonProperty("AllowAccessCustomerGroupName")]
        public string AllowAccessCustomerGroupName { get; set; }

        [JsonProperty("RMAMaxPeriod")]
        public string RMAMaxPeriod { get; set; }

        [JsonProperty("TaxCode")]
        public string TaxCode { get; set; }

        [JsonProperty("DisplayText")]
        public string DisplayText { get; set; }

        [JsonProperty("MinimumQuantity")]
        public double MinimumQuantity { get; set; }

        [JsonProperty("MaximumQuantity")]
        public double MaximumQuantity { get; set; }

        [JsonProperty("AllowOnlyMultiples")]
        public bool AllowOnlyMultiples { get; set; }

        [JsonProperty("AllowFractionalQuantity")]
        public bool AllowFractionalQuantity { get; set; }

        [JsonProperty("QuantityOptions")]
        public string QuantityOptions { get; set; }

        [JsonProperty("GroupOptionsForQuantityPricing")]
        public bool GroupOptionsForQuantityPricing { get; set; }

        [JsonProperty("ApplyQuantityDiscountToOptions")]
        public bool ApplyQuantityDiscountToOptions { get; set; }

        [JsonProperty("EnableMakeAnOfferFeature")]
        public bool EnableMakeAnOfferFeature { get; set; }

        [JsonProperty("MinimumAcceptableOffer")]
        public string MinimumAcceptableOffer { get; set; }

        [JsonProperty("PriceLevel1")]
        public double PriceLevel1 { get; set; }

        [JsonProperty("PriceLevel1Hide")]
        public bool PriceLevel1Hide { get; set; }

        [JsonProperty("PriceLevel2")]
        public double PriceLevel2 { get; set; }

        [JsonProperty("PriceLevel2Hide")]
        public bool PriceLevel2Hide { get; set; }

        [JsonProperty("PriceLevel3")]
        public double PriceLevel3 { get; set; }

        [JsonProperty("PriceLevel3Hide")]
        public bool PriceLevel3Hide { get; set; }

        [JsonProperty("PriceLevel4")]
        public double PriceLevel4 { get; set; }

        [JsonProperty("PriceLevel4Hide")]
        public bool PriceLevel4Hide { get; set; }

        [JsonProperty("PriceLevel5")]
        public double PriceLevel5 { get; set; }

        [JsonProperty("PriceLevel5Hide")]
        public bool PriceLevel5Hide { get; set; }

        [JsonProperty("PriceLevel6")]
        public double PriceLevel6 { get; set; }

        [JsonProperty("PriceLevel6Hide")]
        public bool PriceLevel6Hide { get; set; }

        [JsonProperty("PriceLevel7")]
        public double PriceLevel7 { get; set; }

        [JsonProperty("PriceLevel7Hide")]
        public bool PriceLevel7Hide { get; set; }

        [JsonProperty("PriceLevel8")]
        public double PriceLevel8 { get; set; }

        [JsonProperty("PriceLevel8Hide")]
        public bool PriceLevel8Hide { get; set; }

        [JsonProperty("PriceLevel9")]
        public double PriceLevel9 { get; set; }

        [JsonProperty("PriceLevel9Hide")]
        public bool PriceLevel9Hide { get; set; }

        [JsonProperty("PriceLevel10")]
        public double PriceLevel10 { get; set; }

        [JsonProperty("PriceLevel10Hide")]
        public bool PriceLevel10Hide { get; set; }

        [JsonProperty("BuyButtonLink")]
        public string BuyButtonLink { get; set; }

        [JsonProperty("ProductLink")]
        public string ProductLink { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("CustomFileName")]
        public string CustomFileName { get; set; }

        [JsonProperty("RedirectLink")]
        public string RedirectLink { get; set; }

        [JsonProperty("MetaTags")]
        public string MetaTags { get; set; }

        [JsonProperty("SpecialInstructions")]
        public string SpecialInstructions { get; set; }

        [JsonProperty("AssignKey")]
        public bool AssignKey { get; set; }

        [JsonProperty("ReUseKeys")]
        public bool ReUseKeys { get; set; }
    }
}