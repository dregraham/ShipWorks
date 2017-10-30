using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Enum representing FedExFreightPhysicalPackaging
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExFreightPhysicalPackagingType
    {
        [Description("None")]
        [ApiValue("")]
        None = 0,

        [Description("Bag")]
        [ApiValue("BAG")]
        Bag = 2,

        [Description("Barrel")]
        [ApiValue("BARREL")]
        Barrel = 3,

        [Description("Basket")]
        [ApiValue("BASKET")]
        Basket = 4,

        [Description("Box")]
        [ApiValue("BOX")]
        Box = 5,

        [Description("Bucket")]
        [ApiValue("BUCKET")]
        Bucket = 6,

        [Description("Bundle")]
        [ApiValue("BUNDLE")]
        Bundle = 7,

        [Description("Carton")]
        [ApiValue("CARTON")]
        Carton = 8,

        [Description("Case")]
        [ApiValue("CASE")]
        Case = 9,

        [Description("Container")]
        [ApiValue("CONTAINER")]
        Container = 10,

        [Description("Create")]
        [ApiValue("CRATE")]
        Create = 11,

        [Description("Cylinder")]
        [ApiValue("CYLINDER")]
        Cylinder = 12,

        [Description("Drum")]
        [ApiValue("DRUM")]
        Drum = 13,

        [Description("Envelope")]
        [ApiValue("ENVELOPE")]
        Envelope = 14,

        [Description("Hamper")]
        [ApiValue("HAMPER")]
        Hamper = 15,

        [Description("Other")]
        [ApiValue("OTHER")]
        Other = 16,

        [Description("Pail")]
        [ApiValue("PAIL")]
        Pail = 17,

        [Description("Pallet")]
        [ApiValue("PALLET")]
        Pallet = 18,

        [Description("Piece")]
        [ApiValue("PIECE")]
        Piece = 19,

        [Description("Reel")]
        [ApiValue("REEL")]
        Reel = 20,

        [Description("Roll")]
        [ApiValue("ROLL")]
        Roll = 21,

        [Description("Skid")]
        [ApiValue("SKID")]
        Skid = 22,

        [Description("Tank")]
        [ApiValue("TANK")]
        Tank = 23,

        [Description("Tube")]
        [ApiValue("TUBE")]
        Tube = 24,
    }
}
