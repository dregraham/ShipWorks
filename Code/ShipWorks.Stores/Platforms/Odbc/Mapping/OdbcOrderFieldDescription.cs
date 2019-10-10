﻿using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcOrderFieldDescription
    {
        [Description("Order Number")]
        Number,

        [Description("Order Date & Time")]
        DateAndTime,

        [Description("Last Modified Date & Time")]
        LastModifiedDateAndTime,

        [Description("Order Date")]
        Date,

        [Description("Order Time")]
        Time,

        [Description("Local Status")]
        LocalStatus,

        [Description("Online Status")]
        OnlineStatus,

        [Description("Requested Shipping")]
        RequestedShipping,

        [Description("Ship By Date")]
        ShipByDate,

        [Description("Custom Field 1")]
        Custom1,

        [Description("Custom Field 2")]
        Custom2,

        [Description("Custom Field 3")]
        Custom3,

        [Description("Custom Field 4")]
        Custom4,

        [Description("Custom Field 5")]
        Custom5,

        [Description("Custom Field 6")]
        Custom6,

        [Description("Custom Field 7")]
        Custom7,

        [Description("Custom Field 8")]
        Custom8,

        [Description("Custom Field 9")]
        Custom9,

        [Description("Custom Field 10")]
        Custom10,

        [Description("Customer Number")]
        CustomerID,

        [Description("Note (Internal)")]
        NoteInternal,

        [Description("Note (Public)")]
        NotePublic,

        [Description("Shipping Amount")]
        ChargeShipping,

        [Description("Handling Amount")]
        ChargeHandling,

        [Description("Discount Amount")]
        ChargeDiscount,

        [Description("Insurance Amount")]
        ChargeInsurance,

        [Description("Other Amount")]
        ChargeOther,

        [Description("Tax Amount")]
        ChargeTax,

        [Description("Payment Method")]
        PaymentMethod,

        [Description("Payment Reference")]
        PaymentReference,

        [Description("Credit Card Type")]
        PaymentCCType,

        [Description("Credit Card Number")]
        PaymentCCNumber,

        [Description("Credit Card Expiration")]
        PaymentCCExpiration,

        [Description("Credit Card Name")]
        PaymentCCName,

        [Description("Name")]
        ItemName,

        [Description("Code")]
        ItemCode,

        [Description("SKU")]
        ItemSKU,

        [Description("Quantity")]
        ItemQuantity,

        [Description("Unit Price")]
        ItemUnitPrice,

        [Description("Total Price")]
        ItemTotalPrice,

        [Description("Unit Weight")]
        ItemUnitWeight,

        [Description("Total Weight")]
        ItemTotalWeight,

        [Description("Status")]
        ItemLocalStatus,

        [Description("Description")]
        ItemDescription,

        [Description("Location")]
        ItemLocation,

        [Description("Unit Cost")]
        ItemUnitCost,

        [Description("Total Cost")]
        ItemTotalCost,

        [Description("Image URL")]
        ItemImage,

        [Description("Thumbnail URL")]
        ItemThumbnail,

        [Description("UPC")]
        ItemUPC,

        [Description("ISBN")]
        ItemISBN,

        [Description("Attribute Name")]
        ItemAttributeName,
        
        [Description("Length")]
        ItemLength,

        [Description("Width")]
        ItemWidth,

        [Description("Height")]
        ItemHeight,

        [Description("Brand")]
        ItemBrand,

        [Description("MPN")]
        ItemMPN,

        [Description("Custom Field 1")]
        ItemCustom1,

        [Description("Custom Field 2")]
        ItemCustom2,

        [Description("Custom Field 3")]
        ItemCustom3,

        [Description("Custom Field 4")]
        ItemCustom4,

        [Description("Custom Field 5")]
        ItemCustom5
    }
}
