using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// User control that will fetch and
    /// show rates for an order that has been selected. If an order doesn't have
    /// any shipments, a shipment will be created; for orders that have multiple
    /// shipments, the first unprocessed shipment is used for rating. Rates are
    /// not retrieved for orders that only have processed shipments.
    /// </summary>
    public partial class RatesPanel : UserControl
    {
        private long? selectedShipmentID;
        private bool resetCollapsibleStateRequired;
        private readonly IDisposable uspsAccountConvertedToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatesPanel"/> class.
        /// </summary>
        public RatesPanel()
        {
            ConsolidatePostalRates = false;
            InitializeComponent();

            resetCollapsibleStateRequired = true;

            // We want to show the configure link for all rates, so we
            // can open the shipping dialog
            rateControl.ActionLinkVisible = true;
            rateControl.ShowAllRates = false;

            rateControl.ActionLinkClicked += OnConfigureRateClicked;

            // Force the rates to be refreshed when the rate control tells us
            rateControl.ReloadRatesRequired += (sender, args) => RefreshRates(true);

            rateControl.Initialize(new FootnoteParameters(() => RefreshRates(false), GetStoreForCurrentShipment));

            uspsAccountConvertedToken = Messenger.Current.OfType<UspsAutomaticExpeditedChangedMessage>().Subscribe(OnStampsUspsAutomaticExpeditedChanged);
        }

        /// <summary>
        /// Called when the shipping settings for using USPS has changed. We need to refresh the
        /// shipment data/rates being displayed to accurately reflect that the shipment type has changed to USPS.
        /// </summary>
        private void OnStampsUspsAutomaticExpeditedChanged(UspsAutomaticExpeditedChangedMessage message)
        {
            if (!selectedShipmentID.HasValue)
            {
                return;
            }

            // Refresh the shipment data and then the rates
            ShipmentEntity shipment = ShippingManager.GetShipment(selectedShipmentID.Value);
            ShippingManager.RefreshShipment(shipment);

            FetchRates(shipment, false);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [in shipments panel].
        /// </summary>
        public bool ConsolidatePostalRates
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the store for the current order, if only one is selected
        /// </summary>
        private StoreEntity GetStoreForCurrentShipment()
        {
            if (selectedShipmentID != null)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(selectedShipmentID.Value);

                if (shipment != null)
                {
                    return StoreManager.GetStore(shipment.Order.StoreID);
                }
            }

            return null;
        }

        /// <summary>
        /// Change the content of the control to be the given shipment
        /// </summary>
        public void ChangeShipment(long? shipmentID)
        {
            // This method can get triggered when the shipment dialog closes but the
            // shipment ID did not actually change. We only want to reset the collapsible
            // state when the shipment ID actually changes.
            resetCollapsibleStateRequired = selectedShipmentID != shipmentID;
            selectedShipmentID = shipmentID;


            // Refresh the rates in the panel; using cached rates is fine here since nothing
            // about the shipment has changed, so don't force a re-fetch
            RefreshRates(false);
        }

        /// <summary>
        /// When the size of the rate control changes, we have to update our size to match. This is what makes the auto-scrolling in the containing panel work
        /// </summary>
        private void OnRateControlSizeChanged(object sender, EventArgs e)
        {
            Height = rateControl.Bottom;
        }

        /// <summary>
        /// Forces rates to be refreshed by re-fetching the rates from the shipping provider.
        /// </summary>
        /// <param name="ignoreCache">Should the cached rates be ignored?</param>
        private void RefreshRates(bool ignoreCache)
        {
            ShipmentEntity shipment = null;

            if (selectedShipmentID != null)
            {
                shipment = ShippingManager.GetShipment(selectedShipmentID.Value);
            }

            if (shipment != null)
            {
                if (!shipment.Processed)
                {
                    // We need to fetch the rates from the provider
                    FetchRates(shipment, ignoreCache);
                }
                else
                {
                    rateControl.ClearRates("The shipment has already been processed.");
                }
            }
            else
            {
                rateControl.ClearRates("No shipments are selected.");
            }
        }

        /// <summary>
        /// Clears the rates.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ClearRates(string message)
        {
            selectedShipmentID = null;
            rateControl.ClearRates(message);
        }

        /// <summary>
        /// Fetches the rates from the shipment type and
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="ignoreCache">Should the cached rates be ignored?</param>
        [NDependIgnoreLongMethod]
        private void FetchRates(ShipmentEntity shipment, bool ignoreCache)
        {
            using (BackgroundWorker ratesWorker = new BackgroundWorker())
            {
                ratesWorker.WorkerReportsProgress = false;
                ratesWorker.WorkerSupportsCancellation = true;

                // We're going to be going over the network to get rates from the provider, so show the spinner
                // while rates are being fetched to give the user some indication that we're working
                ShipmentRateGroup panelRateGroup = new ShipmentRateGroup(new RateGroup(new List<RateResult>()), shipment);
                rateControl.ClearRates(string.Empty);

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
                        ShippingManager.EnsureShipmentLoaded(shipment);
                        args.Result = shipment;

                        // We want to ignore the cache primarily when changes come from the rate control, since only the promotion
                        // footer raises the event and we want to include Express1 rates in that case
                        if (ignoreCache)
                        {
                            ShippingManager.RemoveShipmentFromRatesCache(shipment);
                        }

                        // Fetch the rates and add them to the cache
                        shipmentType = PrepareShipmentAndGetShipmentType(shipment);

                        rates = ShippingManager.GetRates(shipment, shipmentType);
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
                        panelRateGroup = new ShipmentRateGroup(new InvalidRateGroup(ShipmentTypeCode.FedEx, ex),
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
                            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                            {
                                ShipmentTypeCode code = shipmentType?.ShipmentTypeCode ?? ShipmentTypeCode.None;
                                rates.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(code, ex));
                            }
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
                        if (exception.Data.Contains("shipmentID") && (long) exception.Data["shipmentID"] == selectedShipmentID)
                        {
                            if (panelRateGroup.FootnoteFactories.OfType<ExceptionsRateFootnoteFactory>().Any())
                            {
                                rateControl.LoadRates(panelRateGroup);
                            }
                            else
                            {
                                rateControl.ClearRates(exception.Message);
                            }
                        }
                    }
                    else
                    {
                        ShipmentEntity ratedShipment = (ShipmentEntity) args.Result;
                        if (ratedShipment != null && ratedShipment.ShipmentID == selectedShipmentID)
                        {
                            if (ShipmentTypeManager.GetType(shipment).SupportsGetRates)
                            {
                                // Only update the rate control if the shipment is for the currently selected
                                // order to avoid the appearance of lag when a user is quickly clicking around
                                // the rate grid
                                LoadRates(panelRateGroup);
                            }
                            else
                            {
                                // The carrier does not support get rates so we display a message to the user
                                rateControl.ClearRates($"The carrier \"{EnumHelper.GetDescription(panelRateGroup.Carrier)}\" does not support retrieving rates.", panelRateGroup);
                            }
                        }
                    }
                };

                // Execute the work to get the rates
                rateControl.ShowSpinner = true;
                ratesWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Prepares the shipment for get rates and gets shipment type.
        /// This handles consolidating of postal rates if required.
        /// </summary>
        private ShipmentType PrepareShipmentAndGetShipmentType(ShipmentEntity shipment)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ShipmentType shipmentType;
                ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) shipment.ShipmentType;

                // Only change this to best rate for non-USPS postal types
                if (ConsolidatePostalRates &&
                    PostalUtility.IsPostalShipmentType(shipmentTypeCode) &&
                !PostalUtility.IsPostalSetup() &&
                    shipmentTypeCode != ShipmentTypeCode.Usps &&
                    shipmentTypeCode != ShipmentTypeCode.Express1Endicia &&
                    shipmentTypeCode != ShipmentTypeCode.Express1Usps)
                {
                    // Func to create a BestRateShippingBroker factory that takes BestRateConsolidatePostalRates
                    // and returns a BestRateShippingBrokerFactory with the correct BrokerFilters
                    Func<BestRateConsolidatePostalRates, IBestRateShippingBrokerFactory> brokerFactoryFactory =
                        lifetimeScope.Resolve<Func<BestRateConsolidatePostalRates, IBestRateShippingBrokerFactory>>();

                    // Resolve the BestRateShipmentType and pass in IBestRateShippingBrokerFactory with
                    // BrokerFilters needed to BestRateConsolidatePostalRates
                    shipmentType =
                        lifetimeScope.Resolve<BestRateShipmentType>(
                            new TypedParameter(typeof(IBestRateShippingBrokerFactory),
                                brokerFactoryFactory(BestRateConsolidatePostalRates.Yes)));


                    shipment.ShipmentType = (int) ShipmentTypeCode.BestRate;
                    ShippingManager.EnsureShipmentLoaded(shipment);

                    shipment.BestRate.DimsProfileID = shipment.Postal.DimsProfileID;
                    shipment.BestRate.DimsLength = shipment.Postal.DimsLength;
                    shipment.BestRate.DimsWidth = shipment.Postal.DimsWidth;
                    shipment.BestRate.DimsHeight = shipment.Postal.DimsHeight;
                    shipment.BestRate.DimsWeight = shipment.Postal.DimsWeight;
                    shipment.BestRate.DimsAddWeight = shipment.Postal.DimsAddWeight;
                    shipment.BestRate.ServiceLevel = (int) ServiceLevelType.Anytime;
                    shipment.BestRate.InsuranceValue = shipment.Postal.InsuranceValue;
                }
                else
                {
                    shipmentType = ShipmentTypeManager.GetType(shipment);
                }

                return shipmentType;
            }
        }

        /// <summary>
        /// A helper method for loading the rates in the rate control.
        /// </summary>
        /// <param name="rateGroup">The rate group.</param>
        private void LoadRates(ShipmentRateGroup rateGroup)
        {
            // Reset the rate control to show all rates and let the policy change the
            // behavior if it's necessary
            rateControl.ShowAllRates = true;
            rateControl.ShowSingleRate = false;

            if (resetCollapsibleStateRequired)
            {
                rateControl.ResetCollapsibleState();
            }

            // Apply any applicable policies to the rate control prior to loading the rates
            ShippingPolicies.Current.Apply((ShipmentTypeCode) rateGroup.Shipment.ShipmentType, rateControl);
            rateControl.LoadRates(rateGroup);
        }

        /// <summary>
        /// Called when a [rate is selected].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="rateSelectedEventArgs">The <see cref="RateSelectedEventArgs"/> instance containing the event data.</param>
        private void OnConfigureRateClicked(object sender, RateSelectedEventArgs rateSelectedEventArgs)
        {
            ShipmentRateGroup rateGroup = (ShipmentRateGroup) rateControl.RateGroup;
            ShipmentEntity shipment = rateGroup.Shipment;

            BestRateResultTag resultTag = rateSelectedEventArgs.Rate.Tag as BestRateResultTag;
            if (resultTag != null && !resultTag.IsRealRate)
            {
                resultTag.RateSelectionDelegate(shipment);
            }
            else
            {
                Messenger.Current.Send(new OpenShippingDialogMessage(this, new[] { shipment }, rateSelectedEventArgs));
            }
        }
    }
}
