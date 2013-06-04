using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration to map to the FedEx International Controlled Export values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExInternationalControlledExportType
    {
        [Description("None")]
        None = 0,

        [Description("DEA 036")]
        Dea036 = 1,

        [Description("DEA 236")]
        Dea236 = 2,

        [Description("DEA 486")]
        Dea486 = 3,

        [Description("DSP 05")]
        Dsp05 = 4,

        [Description("DSP 61")]
        Dsp61 = 5,

        [Description("DSP 73")]
        Dsp73 = 6,

        [Description("DSP 85")]
        Dsp85 = 7,

        [Description("DSP 94")]
        Dsp94 = 8,

        [Description("DSP License Agreement")]
        DspLicenseAgreement = 9,

        [Description("From Foreign Trade Zone")]
        FromForeignTradeZone = 10,

        [Description("Warehouse Withdrawal")]
        WarehouseWithdrawal = 11
    }
}
