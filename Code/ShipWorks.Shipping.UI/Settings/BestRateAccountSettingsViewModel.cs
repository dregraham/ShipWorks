using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.UI.Settings
{
    [Component(RegistrationType.Self)]
    public class BestRateAccountSettingsViewModel
    {
        private static readonly List<ShipmentTypeCode> excludedShipmentTypes = new List<ShipmentTypeCode>
        {
            ShipmentTypeCode.None,
            ShipmentTypeCode.BestRate,
            ShipmentTypeCode.Other,
            ShipmentTypeCode.PostalWebTools,
            ShipmentTypeCode.Express1Endicia,
            ShipmentTypeCode.Express1Usps,
            ShipmentTypeCode.UpsWorldShip,
            ShipmentTypeCode.AmazonSFP,
            ShipmentTypeCode.iParcel
        };

        public BestRateAccountSettingsViewModel()
        {
            Carriers = new ObservableCollection<BestRateCarrier>();
        }

        public ObservableCollection<BestRateCarrier> Carriers { get; set; }
    }
}
