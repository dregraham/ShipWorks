using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac.Core;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel : ShipmentViewModelBase
    {
        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        public OtherShipmentViewModel()
        {
            maxPackages = 1;
        }

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        public OtherShipmentViewModel(IMessenger messenger, IDimensionsManager dimensionsManager, IShippingViewModelFactory shippingViewModelFactory, ICustomsManager customsManager) : 
            base(null, null, messenger, dimensionsManager, shippingViewModelFactory, customsManager)
        {
            maxPackages = 1;
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public override void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
            base.Load(newShipmentAdapter);
            TotalWeight = shipmentAdapter.TotalWeight;
            ShipmentContentWeight = shipmentAdapter.ContentWeight;

            OtherShipmentEntity otherShipment = shipmentAdapter.Shipment.Other;

            Debug.Assert(otherShipment != null);

            CarrierName = otherShipment.Carrier;
            Service = otherShipment.Service;
            Cost = shipmentAdapter.Shipment.ShipmentCost;
            TrackingNumber = shipmentAdapter.Shipment.TrackingNumber;
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public override void Save()
        {
            base.Save();

            shipmentAdapter.Shipment.ShipmentCost = Cost;
            shipmentAdapter.Shipment.TrackingNumber = TrackingNumber;
            shipmentAdapter.Shipment.TotalWeight = TotalWeight;
            shipmentAdapter.ContentWeight = ShipmentContentWeight;

            OtherShipmentEntity otherShipment = shipmentAdapter.Shipment.Other;
            Debug.Assert(otherShipment != null);

            otherShipment.Carrier = CarrierName;
            otherShipment.Service = Service;
        }

        #region IDataErrorInfo
        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public override string this[string columnName]
        {
            get
            {
                // If the shipment is null or processed, don't validate anything.
                if (shipmentAdapter?.Shipment == null || shipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

                return InputValidation<OtherShipmentViewModel>.Validate(this, columnName);
            }
        }

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public override ICollection<string> AllErrors()
        {
            return InputValidation<OtherShipmentViewModel>.Validate(this);
        }

        #endregion

        /// <summary>
        /// Updates the services
        /// </summary>

        public override void RefreshServiceTypes()
        {
            // Other shipment type does not support services
        }

        /// <summary>
        /// Updates the packages
        /// </summary>

        public override void RefreshPackageTypes()
        {
            // Other shipment type does not support packages
        }

        /// <summary>
        /// Select the given rate
        /// </summary>

        
        public override void SelectRate(RateResult selectedRate)
        {
            // Other shipment type does not support service types
        }
    }
}