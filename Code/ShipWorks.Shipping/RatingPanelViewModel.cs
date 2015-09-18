using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Policies;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// View model for getting rates
    /// </summary>
    public class RatingPanelViewModel : IDisposable, INotifyPropertyChanged
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(RatingPanelViewModel));

        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly MessengerToken uspsAccountConvertedToken;
        private readonly MessengerToken shipmentChangedMessageToken;
        private readonly IMessenger messenger;

        private readonly ILoader<ShippingPanelLoadedShipment> shipmentLoader;
        private readonly IShippingManager shippingManager;
        private readonly IShipmentTypeFactory shipmentTypeFactory;

        private long? selectedShipmentID;
        private readonly bool consolidatePostalRates;
        private bool actionLinkVisible;
        private bool showAllRates;
        private bool showSpinner;
        public StoreEntity store;
        private RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
        private string errorMessage = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messenger"></param>
        public RatingPanelViewModel(ILoader<ShippingPanelLoadedShipment> shipmentLoader, IMessenger messenger, IShippingManager shippingManager, IShipmentTypeFactory shipmentTypeFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            this.shipmentLoader = shipmentLoader;
            this.messenger = messenger;
            this.shippingManager = shippingManager;
            this.shipmentTypeFactory = shipmentTypeFactory;

            consolidatePostalRates = true;
            ActionLinkVisible = false;
            ShowAllRates = true;

            uspsAccountConvertedToken = messenger.Handle<UspsAutomaticExpeditedChangedMessage>(this, HandleUspsAutomaticExpeditedChangedMessage);

            shipmentChangedMessageToken = messenger.Handle<ShipmentChangedMessage>(this, HandleShipmentChangedMessage);
        }

        /// <summary>
        /// Gets/Sets whether to show the action link
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ActionLinkVisible
        {
            get { return actionLinkVisible; }
            set
            {
                handler.Set(nameof(ActionLinkVisible), ref actionLinkVisible, value);
            }
        }

        /// <summary>
        /// Gets/Sets whether to show all rates
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowAllRates
        {
            get { return showAllRates; }
            set
            {
                handler.Set(nameof(ShowAllRates), ref showAllRates, value);
            }
        }

        /// <summary>
        /// Gets/Sets the rate group 
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RateGroup RateGroup
        {
            get { return rateGroup; }
            set
            {
                handler.Set(nameof(RateGroup), ref rateGroup, value); 
            }
        }

        /// <summary>
        /// Gets/Sets any error message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                handler.Set(nameof(ErrorMessage), ref errorMessage, value);
            }
        }

        /// <summary>
        /// Gets/Sets whether to show the spinner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowSpinner
        {
            get { return showSpinner; }
            set
            {
                handler.Set(nameof(ShowSpinner), ref showSpinner, value);
            }
        }

        /// <summary>
        /// Store for the shipment
        /// </summary>
        public StoreEntity Store { get; set; }

        /// <summary>
        /// Change the content of the control to be the given shipment
        /// </summary>
        public void ChangeShipment(ShipmentEntity shipment)
        {
            selectedShipmentID = shipment.ShipmentID;

            // Refresh the rates in the panel; using cached rates is fine here since nothing
            // about the shipment has changed, so don't force a re-fetch
            RefreshRates(false);
        }

        /// <summary>
        /// Forces rates to be refreshed by re-fetching the rates from the shipping provider.
        /// </summary>
        /// <param name="ignoreCache">Should the cached rates be ignored?</param>
        public void RefreshRates(bool ignoreCache)
        {
            ShipmentEntity shipment = null;

            if (selectedShipmentID != null)
            {
                shipment = shippingManager.GetShipment(selectedShipmentID.Value);
            }

            if (shipment != null)
            {
                if (!shipment.Processed)
                {
                    ShipmentType shipmentType = shipmentTypeFactory.Get(shipment.ShipmentTypeCode);

                    if (!shipmentType.SupportsGetRates)
                    {
                        RateGroup rateGroupWithNoRatesFooter = new RateGroup(new List<RateResult>());

                        rateGroupWithNoRatesFooter.AddFootnoteFactory(new InformationFootnoteFactory("Select another provider to get rates."));

                        RateGroup = rateGroupWithNoRatesFooter;
                    }
                    else
                    {
                        // We need to fetch the rates from the provider
                        FetchRates(shipment, ignoreCache);
                    }
                }
                else
                {
                    ErrorMessage = "The shipment has already been processed.";
                }
            }
            else
            {
                ErrorMessage = "No shipments are selected.";
            }
        }

        /// <summary>
        /// Refreshes the selected shipments - Updates the rate control
        /// </summary>
        public async Task RefreshSelectedShipments(long orderID)
        {
            ShippingPanelLoadedShipment loadedShipment = await shipmentLoader.LoadAsync(orderID);
            
            if (loadedShipment.Result == ShippingPanelLoadedShipmentResult.Success)
            {
                ChangeShipment(loadedShipment.Shipment);
            }
            else if (loadedShipment.Result == ShippingPanelLoadedShipmentResult.Multiple)
            {
                ErrorMessage = "Multiple shipments selected.";
            }
            else if (loadedShipment.Result == ShippingPanelLoadedShipmentResult.Error)
            {
                ErrorMessage = "An error occurred while retrieving rates.";
            }
            else
            {
                ErrorMessage = "No shipments are selected.";
            }
        }

        /// <summary>
        /// Called when the shipment values have changed. We need to refresh the
        /// shipment data/rates being displayed to accurately.
        /// </summary>
        private void HandleShipmentChangedMessage(ShipmentChangedMessage message)
        {
            // Refresh the shipment data and then the rates
            ShipmentEntity shipment = message.Shipment;

            FetchRates(shipment, false);
        }

        /// <summary>
        /// Called when the shipping settings for using USPS has changed. We need to refresh the
        /// shipment data/rates being displayed to accurately reflect that the shipment type has changed to USPS.
        /// </summary>
        private void HandleUspsAutomaticExpeditedChangedMessage(UspsAutomaticExpeditedChangedMessage message)
        {
            if (!selectedShipmentID.HasValue)
            {
                return;
            }

            // Refresh the shipment data and then the rates
            ShipmentEntity shipment = shippingManager.GetShipment(selectedShipmentID.Value);
            shippingManager.RefreshShipment(shipment);

            FetchRates(shipment, false);
        }

        /// <summary>
        /// Fetches the rates from the shipment type and 
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="ignoreCache">Should the cached rates be ignored?</param>
        private void FetchRates(ShipmentEntity shipment, bool ignoreCache)
        {
            ShowSpinner = true;

            using (BackgroundWorker ratesWorker = new BackgroundWorker())
            {
                ratesWorker.WorkerReportsProgress = false;
                ratesWorker.WorkerSupportsCancellation = true;

                // We're going to be going over the network to get rates from the provider, so show the spinner
                // while rates are being fetched to give the user some indication that we're working
                ShipmentRateGroup panelRateGroup = new ShipmentRateGroup(new RateGroup(new List<RateResult>()), shipment);

                // Setup the worker with the work to perform asynchronously
                ratesWorker.DoWork += (sender, args) =>
                {
                    RateGroup rates = null;
                    ShipmentType shipmentType = null;

                    try
                    {
                        // Load all the child shipment data otherwise we'll get null reference
                        // errors when getting rates; we're going to use the shipment in the
                        // completed event handler to see if we should load the grid or not
                        shippingManager.EnsureShipmentLoaded(shipment);
                        args.Result = shipment;

                        // We want to ignore the cache primarily when changes come from the rate control, since only the promotion
                        // footer raises the event and we want to include Express1 rates in that case
                        if (ignoreCache)
                        {
                            shippingManager.RemoveShipmentFromRatesCache(shipment);
                        }

                        // Fetch the rates and add them to the cache
                        shipmentType = PrepareShipmentAndGetShipmentType(shipment);

                        rates = shippingManager.GetRates(shipment, shipmentType);
                        panelRateGroup = new ShipmentRateGroup(rates, shipment);
                    }
                    catch (InvalidRateGroupShippingException ex)
                    {
                        panelRateGroup = new ShipmentRateGroup(ex.InvalidRates, shipment);

                        // Add the shipment ID to the exception data, so we can determine whether
                        // to update the rate control
                        ex.Data.Add("shipmentID", shipment.ShipmentID);
                        args.Result = ex;
                    }
                    catch (FedExAddressValidationException ex)
                    {
                        panelRateGroup = new ShipmentRateGroup(new InvalidRateGroup(new FedExShipmentType(), ex),
                            shipment);

                        // Add the shipment ID to the exception data, so we can determine whether
                        // to update the rate control
                        ex.Data.Add("shipmentID", shipment.ShipmentID);
                        args.Result = ex;

                    }
                    catch (ShippingException ex)
                    {
                        // While the rate group should be cached, there was a situation where getting credentials from Tango caused an exception,
                        // which happened before exception handling kicked in.  So calling GetRates again and relying on cache was causing a crash.
                        if (rates == null)
                        {
                            rates = new RateGroup(new List<RateResult>());
                            ShipmentType exceptionShipmentType = shipmentType ?? shipmentTypeFactory.Get(ShipmentTypeCode.None);
                            rates.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(exceptionShipmentType, ex));
                        }

                        panelRateGroup = new ShipmentRateGroup(rates, shipment);
                    }
                };

                // What to run when the work has been completed
                ratesWorker.RunWorkerCompleted += (sender, args) =>
                {
                    ShippingException exception = args.Result as ShippingException;
                    if (exception != null)
                    {
                        if (exception.Data.Contains("shipmentID") && (long)exception.Data["shipmentID"] == selectedShipmentID)
                        {
                            if (panelRateGroup.FootnoteFactories.OfType<ExceptionsRateFootnoteFactory>().Any())
                            {
                                RateGroup = panelRateGroup;
                            }
                            else
                            {
                                ErrorMessage = exception.Message;
                            }
                        }
                    }
                    else
                    {
                        ShipmentEntity ratedShipment = (ShipmentEntity)args.Result;
                        if (ratedShipment != null && ratedShipment.ShipmentID == selectedShipmentID)
                        {
                            // Only update the rate control if the shipment is for the currently selected 
                            // order to avoid the appearance of lag when a user is quickly clicking around
                            // the rate grid
                            RateGroup = panelRateGroup;
                        }
                    }

                    ShowSpinner = false;
                };

                // Execute the work to get the rates
                ratesWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Prepares the shipment for get rates and gets shipment type.
        /// This handles consolidating of postal rates if required.
        /// </summary>
        private ShipmentType PrepareShipmentAndGetShipmentType(ShipmentEntity shipment)
        {
            ShipmentType shipmentType;
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)shipment.ShipmentType;

            // Only change this to best rate for non-USPS postal types
            if (consolidatePostalRates &&
                PostalUtility.IsPostalShipmentType(shipmentTypeCode) &&
                !PostalUtility.IsPostalSetup() &&
                shipmentTypeCode != ShipmentTypeCode.Usps &&
                shipmentTypeCode != ShipmentTypeCode.Express1Endicia &&
                shipmentTypeCode != ShipmentTypeCode.Express1Usps)
            {
                shipmentType = new BestRateShipmentType(new BestRateShippingBrokerFactory(new List<IShippingBrokerFilter> { new PostalCounterBrokerFilter(), new PostalOnlyBrokerFilter() }));

                shipment.ShipmentType = (int)ShipmentTypeCode.BestRate;
                shippingManager.EnsureShipmentLoaded(shipment);

                shipment.BestRate.DimsProfileID = shipment.Postal.DimsProfileID;
                shipment.BestRate.DimsLength = shipment.Postal.DimsLength;
                shipment.BestRate.DimsWidth = shipment.Postal.DimsWidth;
                shipment.BestRate.DimsHeight = shipment.Postal.DimsHeight;
                shipment.BestRate.DimsWeight = shipment.Postal.DimsWeight;
                shipment.BestRate.DimsAddWeight = shipment.Postal.DimsAddWeight;
                shipment.BestRate.ServiceLevel = (int)ServiceLevelType.Anytime;
                shipment.BestRate.InsuranceValue = shipment.Postal.InsuranceValue;
            }
            else
            {
                shipmentType = shipmentTypeFactory.Get(shipment.ShipmentTypeCode);
            }

            return shipmentType;
        }
        
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                messenger.Remove(uspsAccountConvertedToken);
                messenger.Remove(shipmentChangedMessageToken);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}
