using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPackagingType
    {
        [Description("Your Packaging")]
        Custom = 0,

        [Description("UPS Letter")]
        Letter = 1,

        [Description("UPS Tube")]
        Tube = 2,

        [Description("UPS Express® Pak")]
        Pak = 3,

        [Description("UPS Express® Box - Small")]
        BoxExpressSmall = 4,

        [Description("UPS Express® Box - Medium")]
        BoxExpressMedium = 5,

        [Description("UPS Express® Box - Large")]
        BoxExpressLarge = 6,

        [Description("UPS 25 KG Box®")]
        Box25Kg = 7,

        [Description("UPS 10 KG Box®")]
        Box10Kg = 8,

        // The following (9-20) are for Mail Innovations
        [Description("First Class")]
        FirstClassMail = 9,

        [Description("Priority")]
        PriorityMail = 10,

        [Description("BPM Flats")]
        BPMFlats = 11,

        [Description("BPM Parcels")]
        BPMParcels = 12,

        [Description("Irregulars")]
        Irregulars = 13,

        [Description("Machinables")]
        Machinables = 14, 

        [Description("Media Mail")]
        MediaMail = 15,

        [Description("Parcel Post")]
        ParcelPost = 16,

        [Description("Standard Flats")]
        StandardFlats = 17,

        [Description("Flats")]
        Flats = 18,

        [Description("BPM")]
        BPM = 19,

        [Description("Parcels")]
        Parcels = 20,

        // Canada package types
        [Description("UPS Express Box")]
        BoxExpress = 21,

        [Description("UPS Express Envelope")]
        ExpressEnvelope = 22
    }
}
