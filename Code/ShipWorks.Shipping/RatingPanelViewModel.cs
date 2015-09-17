//using System;
//using ShipWorks.Core.UI;
//using ShipWorks.Data.Model.EntityClasses;
//using ShipWorks.Shipping.UI;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Net;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;
//using Interapptive.Shared.Messaging;
//using log4net;
//using ShipWorks.Data;
//using ShipWorks.Shipping.Editing.Rating;
//using ShipWorks.Shipping.Policies;

//namespace ShipWorks.Shipping
//{
//    /// <summary>
//    /// Main view model for the shipment panel
//    /// </summary>
//    public class RatingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging
//    {
//        // Logger
//        static readonly ILog log = LogManager.GetLogger(typeof(RatingPanelViewModel));

//        private ILoader<ShippingPanelLoadedShipment> shipmentLoader;

//        private PropertyChangedHandler handler;

//        public event PropertyChangedEventHandler PropertyChanged;
//        public event PropertyChangingEventHandler PropertyChanging;

//        private RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
//        private ShipmentEntity shipment;
//        private IMessenger messenger;
//        private bool showSpinner;
//        private string clearRates;
//        private string errorMessage;
//        private ShipmentTypeCode shipmentType;

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public RatingPanelViewModel(ILoader<ShippingPanelLoadedShipment> shipmentLoader, IMessenger messenger)
//        {
//            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

//            this.shipmentLoader = shipmentLoader;
//            this.messenger = messenger;

//            messenger.Handle<ShipmentChangedMessage>(this, HandleShipmentChangedMessage);

//            //ClearRates = string.Empty;
//        }

//        /// <summary>
//        /// Handle any filter node changed messages
//        /// </summary>
//        private async void HandleShipmentChangedMessage(ShipmentChangedMessage message)
//        {
//            this.shipment = message.Shipment;
//            RateGroup = await FetchRatesAsync();
//        }

//        [Obfuscation(Exclude = true)]
//        public bool ShowSpinner
//        {
//            get { return showSpinner; }
//            private set { handler.Set(nameof(ShowSpinner), ref showSpinner, value); }
//        }

//        [Obfuscation(Exclude = true)]
//        public RateGroup RateGroup
//        {
//            get { return rateGroup; }
//            private set { handler.Set(nameof(RateGroup), ref rateGroup, value); }
//        }

//        [Obfuscation(Exclude = true)]
//        public string ClearRates
//        {
//            get { return clearRates; }
//            private set { handler.Set(nameof(ClearRates), ref clearRates, value); }
//        }

//        [Obfuscation(Exclude = true)]
//        public ShipmentTypeCode ShipmentType
//        {
//            get { return shipmentType; }
//            private set { handler.Set(nameof(ShipmentType), ref shipmentType, value); }
//        }

//        [Obfuscation(Exclude = true)]
//        public string ErrorMessage
//        {
//            get { return errorMessage; }
//            private set { handler.Set(nameof(ErrorMessage), ref errorMessage, value); }
//        }

//        /// <summary>
//        /// Load the rates for an order's shipment
//        /// </summary>
//        public async Task LoadRates(long orderID)
//        {
//            //RateGroup = null;
//            //ClearRates = string.Empty;

//            ShippingPanelLoadedShipment loadedShipment = await shipmentLoader.LoadAsync(orderID);

//            if (loadedShipment.Shipment == null)
//            {
//                // No shipment was created.  Show a message and return.
//                switch (loadedShipment.Result)
//                {
//                    case ShippingPanelLoadedShipmentResult.Multiple:
//                    case ShippingPanelLoadedShipmentResult.NotCreated:
//                        //ErrorMessage = "No unprocessed shipments exist to rate.";
//                        break;
//                    case ShippingPanelLoadedShipmentResult.Error:
//                        //ErrorMessage = "An error occurred while getting rates.";
//                        break;
//                }

//                return;
//            }

//            shipment = loadedShipment.Shipment;

//            // Update the current shipment type
//            if (loadedShipment.Shipment.ShipmentType != shipment.ShipmentType)
//            {
//                ShipmentType = (ShipmentTypeCode)loadedShipment.Shipment.ShipmentType;
//            }

//            RateGroup newRateGroup = await FetchRatesAsync();

//            LoadRateGroup(newRateGroup);
//        }

//        /// <summary>
//        /// Loads the rates.
//        /// </summary>
//        /// <param name="newRateGroup">The rate group.</param>
//        private void LoadRateGroup(RateGroup newRateGroup)
//        {
//            if (newRateGroup == null)
//            {
//                if (shipment.Processed)
//                {
//                    //ErrorMessage = "The shipment has already been processed.";
//                }
//                else
//                {
//                    //ClearRates = string.Empty;
//                }
//            }
//            else if (newRateGroup.Rates.Count == 0)
//            {
//                if (!newRateGroup.FootnoteFactories.Any())
//                {
//                    //ErrorMessage = "No rates are available for the shipment.";
//                }
//                else
//                {
//                    //ClearRates = string.Empty;
//                }
//            }

//            RateGroup = newRateGroup;
//        }

//        /// <summary>
//        /// Actually get the rates when the debounce timer has elapsed
//        /// </summary>
//        private async Task<RateGroup> FetchRatesAsync()
//        {
//            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);

//            ShipmentType uiShipmentType = ShipmentTypeManager.GetType(clonedShipment);

//            if (!uiShipmentType.SupportsGetRates || clonedShipment.Processed)
//            {
//                return new RateGroup(Enumerable.Empty<RateResult>());
//            }

//            ShowSpinner = true;

//            try
//            {
//                RateGroup newRateGroup = await TaskEx.Run(() => ShippingManager.GetRates(clonedShipment));

//                return newRateGroup;
//            }
//            catch (InvalidRateGroupShippingException ex)
//            {
//                log.Error("Shipping exception encountered while getting rates", ex);
//                return ex.InvalidRates;
//            }
//            catch (ShippingException ex)
//            {
//                log.Error("Shipping exception encountered while getting rates", ex);
//                return null;
//            }
//            finally
//            {
//                ShowSpinner = false;
//            }
//        }
//    }
//}
