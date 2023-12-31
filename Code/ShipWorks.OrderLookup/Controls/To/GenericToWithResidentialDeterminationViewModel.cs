using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// ViewModel for To panel in the OrderLookup view
    /// </summary>
    [KeyedComponent(typeof(IToViewModel), ShipmentTypeCode.FedEx)]
    [WpfView(typeof(GenericToWithResidentialDeterminationControl))]
    public class GenericToWithResidentialDeterminationViewModel : GenericToViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericToWithResidentialDeterminationViewModel(
                IOrderLookupShipmentModel shipmentModel,
                AddressViewModel addressViewModel,
                ISchedulerProvider schedulerProvider,
                OrderLookupFieldLayoutProvider fieldLayoutProvider) :
            base(shipmentModel, addressViewModel, schedulerProvider, fieldLayoutProvider)
        {
            ResidentialDeterminations = EnumHelper.GetEnumList<ResidentialDeterminationType>()
                                                  .ToDictionary(r => (int) r.Value, r => r.Description);
        }

        /// <summary>
        /// Residential Determination choices
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> ResidentialDeterminations { get; set; }
    }
}