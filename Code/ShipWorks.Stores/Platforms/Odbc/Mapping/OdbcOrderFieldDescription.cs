using System.ComponentModel;
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
        ItemAttributeName
    }
}
