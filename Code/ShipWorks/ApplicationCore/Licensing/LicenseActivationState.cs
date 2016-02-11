using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// The various states of activation a license can be in
    /// </summary>
    public enum LicenseActivationState
    {
        Active,
        ActiveElsewhere,
        ActiveNowhere,
        Deactivated,
        Canceled,
        Invalid,
        OverChannelLimit
    }
}
