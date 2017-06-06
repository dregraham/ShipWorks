using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Endicia
{
    /// <summary>
    /// Options control for Express1 Endicia accounts
    /// </summary>
    [Component(RegistrationType.Self)]
    public class Express1EndiciaOptionsControl : EndiciaOptionsControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaOptionsControl() : base(EndiciaReseller.Express1)
        {
        }
    }
}
