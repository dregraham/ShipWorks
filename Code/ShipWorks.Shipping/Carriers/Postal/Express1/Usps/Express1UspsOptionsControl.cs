using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Usps
{
    /// <summary>
    /// Options control for Express1 Usps accounts
    /// </summary>
    [Component(RegistrationType.Self)]
    public class Express1UspsOptionsControl : UspsOptionsControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsOptionsControl()
        {
            ShipmentTypeCode = ShipmentTypeCode.Express1Usps;
        }
    }
}
