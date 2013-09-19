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

            //TODO: This will be replaced with a call to the facade
            SetAccessors(s => s.StampsAutomaticExpress1, (s, b) => s.StampsAutomaticExpress1 = b,
                s => s.StampsAutomaticExpress1Account, (s, a) => s.StampsAutomaticExpress1Account = a);
        }

        /// <summary>
        /// TODO: Replace with the facade
        /// </summary>
        /// <returns></returns>
        protected override ICollection<KeyValuePair<string, long>> GetAccountList()
        {
            return StampsAccountManager.GetAccounts(true)
                                       .Select(a => new KeyValuePair<string, long>(a.Description, a.StampsAccountID))
                                       .ToList();
        }
    }
}
