using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// The online tools supported by UPS
    /// </summary>
    public enum UpsOnLineToolType
    {
        LicenseAgreement,
        AccessKey,
        Register,
        Rate,
        TimeInTransit,
        ShipConfirm,
        ShipAccept,
        ShipVoid,
        Track,
        SurePostRate
    }
}
