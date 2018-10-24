﻿using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// Viewmodel for From panel in the OrderLookup view
    /// </summary>
    [KeyedComponent(typeof(IToViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(GenericToWithResidentialDeterminationControl))]
    public class UpsToViewModel : GenericToWithResidentialDeterminationViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsToViewModel(IOrderLookupShipmentModel shipmentModel, AddressViewModel addressViewModel)
            : base(shipmentModel, addressViewModel)
        {
            ResidentialDeterminations = EnumHelper.GetEnumList<ResidentialDeterminationType>(r => r != ResidentialDeterminationType.FedExAddressLookup)
                .ToDictionary(r => (int) r.Value, r => r.Description);
        }
    }
}
