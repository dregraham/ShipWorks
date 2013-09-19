using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Stamps version of the Automatic Express1 control
    /// </summary>
    /// <remarks>
    /// TODO: Can this class be removed, since we can base the facade on the shipment type?
    /// </remarks>
    public partial class StampsAutomaticExpress1Control : AutomaticExpress1ControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAutomaticExpress1Control()
        {
            InitializeComponent();
            
        }

    }
}
