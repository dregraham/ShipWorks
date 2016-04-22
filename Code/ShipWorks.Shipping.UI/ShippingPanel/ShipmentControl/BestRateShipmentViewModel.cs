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
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by BestRateShipmentControl
    /// </summary>
    public partial class BestRateShipmentViewModel : ShipmentViewModelBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BestRateShipmentViewModel));
        private readonly IDisposable subscriptions;

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        protected BestRateShipmentViewModel()
        {
            maxPackages = 1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateShipmentViewModel(IMessenger messenger,
            IDimensionsManager dimensionsManager,
            IShippingViewModelFactory shippingViewModelFactory,
            ICustomsManager customsManager) : base(null, null, messenger, dimensionsManager, shippingViewModelFactory, customsManager)
        {
            maxPackages = 1;

            serviceLevels.Clear();
            EnumHelper.GetEnumList<ServiceLevelType>().Select(x => x.Value).ToList().ForEach(slt => serviceLevels.Add((int) slt, EnumHelper.GetDescription(slt)));

            subscriptions = new CompositeDisposable(
                SubscribeToRatesRetrieval());
        }

        /// <summary>
        /// Subscribe to rates retrieving and retrieved messages so we can update the
        /// rate text.
        /// </summary>
        public IDisposable SubscribeToRatesRetrieval()
        {
            return new CompositeDisposable(
                messenger.OfType<RatesRetrievingMessage>()
                    .Subscribe(_ => RatesLoaded = false),
                messenger.OfType<RatesRetrievingMessage>()
                    .Select(GetMatchingRatesRetrievedMessage)
                    .Switch()
                    .Subscribe(_ => RatesLoaded = true));
        }

        /// <summary>
        /// Get rates retrieved messages that match the rates retrieving message
        /// </summary>
        private IObservable<RatesRetrievedMessage> GetMatchingRatesRetrievedMessage(RatesRetrievingMessage rateRetrievingMsg)
        {
            return messenger.OfType<RatesRetrievedMessage>()
               .Where(rateRetrivedMsg => rateRetrievingMsg.RatingHash == rateRetrivedMsg.RatingHash);
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public override void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
			base.Load(newShipmentAdapter);
			
            ServiceLevel = shipmentAdapter.Shipment.BestRate.ServiceLevel;
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public override void RefreshServiceTypes()
        {
            // BestRate has no service types, just return.
        }

        /// <summary>
        /// Refreshes the package types.
        /// </summary>
        public override void RefreshPackageTypes()
        {
            // BestRate has no package types, just return.
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public override void Save()
        {
			base.Save();

            shipmentAdapter.Shipment.BestRate.ServiceLevel = ServiceLevel;
        }

        /// <summary>
        /// Select the given rate
        /// </summary>
        public override void SelectRate(RateResult rateResult)
        {
            SelectedRate = rateResult;
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

                return InputValidation<BestRateShipmentViewModel>.Validate(this, columnName);
            }
        }

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public override ICollection<string> AllErrors()
        {
            return InputValidation<BestRateShipmentViewModel>.Validate(this);
        }

        #endregion

        /// <summary>
        /// Dispose resources
        /// </summary>
        public override void Dispose() 
        {
            base.Dispose();
            subscriptions?.Dispose();
        }
    }
}