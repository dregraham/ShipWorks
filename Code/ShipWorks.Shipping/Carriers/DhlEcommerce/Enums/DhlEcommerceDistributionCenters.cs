using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce.Enums
{
    /// <summary>
    /// DHL eCommerce distribution center codes
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public enum DhlEcommerceDistributionCenters
    {
        [ApiValue("USATL1")]
        [Description("USATL1 (Forest Park, GA)")]
        USATL1 = 0,

        [ApiValue("USBOS1")]
        [Description("USBOS1 (Franklin, MA)")]
        USBOS1 = 1,

        [ApiValue("USBWI1")]
        [Description("USBWI1 (Elkridge, MD)")]
        USBWI1 = 2,

        [ApiValue("USCAK1")]
        [Description("USCAK1 (Stow, OH)")]
        USCAK1 = 3,

        [ApiValue("USCVG1")]
        [Description("USCVG1 (Hebron, KY)")]
        USCVG1 = 4,

        [ApiValue("USDEN1")]
        [Description("USDEN1 (Denver, CO)")]
        USDEN1 = 5,

        [ApiValue("USDFW1")]
        [Description("USDFW1 (Grand Prairie, TX)")]
        USDFW1 = 6,

        [ApiValue("USEWR1")]
        [Description("USEWR1 (Secaucus, NJ)")]
        USEWR1 = 7,

        [ApiValue("USLAX1")]
        [Description("USLAX1 (Compton, CA)")]
        USLAX1 = 8,

        [ApiValue("USMCO1")]
        [Description("USMCO1 (Orlando, FL)")]
        USMCO1 = 9,

        [ApiValue("USMEM1")]
        [Description("USMEM1 (Memphis, TN)")]
        USMEM1 = 10,

        [ApiValue("USORD1")]
        [Description("USORD1 (Melrose Park, IL)")]
        USORD1 = 11,

        [ApiValue("USPHX1")]
        [Description("USPHX1 (Phoenix, AZ)")]
        USPHX1 = 12,

        [ApiValue("USRDU1")]
        [Description("USRDU1 (Raleigh, NC)")]
        USRDU1 = 13,

        [ApiValue("USSEA1")]
        [Description("USSEA1 (Auburn, WA)")]
        USSEA1 = 14,

        [ApiValue("USSFO1")]
        [Description("USSFO1 (Union City, CA)")]
        USSFO1 = 15,

        [ApiValue("USSLC1")]
        [Description("USSLC1 (Salt Lake City, UT)")]
        USSLC1 = 16,

        [ApiValue("USSTL1")]
        [Description("USSTL1 (St. Louis, MO)")]
        USSTL1 = 17,

        [ApiValue("USIAH1")]
        [Description("USIAH1 (Houston, TX)")]
        USIAH1 = 18,

        [ApiValue("USISP1")]
        [Description("USISP1 (Edgewood, NY)")]
        USISP1 = 19,

        [ApiValue("CNYYZ1")]
        [Description("CNYYZ1 (Mississauga, ON, CA)")]
        CNYYZ1 = 20,

        [ApiValue("GBLHR1")]
        [Description("GBLHR1 (Axis Park, Langley, GB)")]
        GBLHR1 = 21,
    }
}
