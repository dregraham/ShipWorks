using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// For international to Canada only
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExPhysicalPackagingType
    {
        [Description("Bag")]
        Bag = 0,

        [Description("Barrel")]
        Barrel = 1,

        [Description("Basket or hamper")]
        BasketOrHamper = 2,

        [Description("Box")]
        Box = 3,

        [Description("Bucket")]
        Bucket = 4,

        [Description("Bundle")]
        Bundle = 5,

        [Description("Carton")]
        Carton = 6,

        [Description("Case")]
        Case = 7,

        [Description("Container")]
        Container = 8,

        [Description("Crate")]
        Crate = 9,

        [Description("Cylinder")]
        Cylinder = 10,

        [Description("Drum")]
        Drum = 11,

        [Description("Envelope")]
        Envelope = 12,

        [Description("Hamper")]
        Hamper = 13,

        [Description("Other")]
        Other = 14,

        [Description("Pail")]
        Pail = 15,

        [Description("Pallet")]
        Pallet = 16,

        [Description("Pieces")]
        Pieces = 17,

        [Description("Reel")]
        Reel = 18,

        [Description("Roll")]
        Roll = 19,

        [Description("Skid")]
        Skid = 20,

        [Description("Tank")]
        Tank = 21,

        [Description("Tube")]
        Tube = 22,
    }
}
