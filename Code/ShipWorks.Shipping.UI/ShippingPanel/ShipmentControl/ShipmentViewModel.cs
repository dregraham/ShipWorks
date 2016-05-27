using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public class ShipmentViewModel : ShipmentViewModelBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentViewModel));

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        protected ShipmentViewModel()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentViewModel(IShipmentServicesBuilderFactory shipmentServicesBuilderFactory,
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory, IMessenger messenger,
            IDimensionsManager dimensionsManager,
            IShippingViewModelFactory shippingViewModelFactory,
            ICustomsManager customsManager) : base(shipmentServicesBuilderFactory, shipmentPackageTypesBuilderFactory, messenger, dimensionsManager, shippingViewModelFactory, customsManager)
        {
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

                return InputValidation<ShipmentViewModel>.Validate(this, columnName);
            }
        }

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public override ICollection<string> AllErrors()
        {
            return InputValidation<ShipmentViewModel>.Validate(this);
        }

        #endregion
    }
}