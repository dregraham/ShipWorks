using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// View model for the FedEx From control
    /// </summary>
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.FedEx)]
    [WpfView(typeof(FedExFromControl))]
    public class FedExFromViewModel : GenericFromViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExFromViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
            AddressViewModel addressViewModel,
            ISchedulerProvider schedulerProvider,
			OrderLookupFromFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, carrierAccountRetrieverFactory, addressViewModel, schedulerProvider, fieldLayoutProvider)
        {
            // Don't give the user the option to have FedEx perform the address look up; the thought it that the shipper will know
            // what type of address they are shipping from, and it saves delays associated with a service call
            ResidentialDeterminations = EnumHelper.GetEnumList<ResidentialDeterminationType>(t => t != ResidentialDeterminationType.FedExAddressLookup && t != ResidentialDeterminationType.FromAddressValidation)
                .ToDictionary(x => (int) x.Value, x => x.Description);
        }

        /// <summary>
        /// Collection of applicable Residential Determinations
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> ResidentialDeterminations { get; }
    }
}