using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Endicia test servers
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EndiciaTestServer
    {
        [Description("www.envmgr.com")]
        [ApiValue("https://www.envmgr.com/LabelService/EwsLabelService.asmx")]
        Envmgr = 0,

        [Description("elstestserver.endicia.com - Sandbox")]
        [ApiValue("https://elstestserver.endicia.com/LabelService/EwsLabelService.asmx")]
        ElsTestServer = 1
    }
}
