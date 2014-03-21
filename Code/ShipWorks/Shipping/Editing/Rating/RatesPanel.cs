using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.BestRate;
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
        private long? selectedShipmentID = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatesPanel"/> class.
        /// </summary>
        public RatesPanel()
        {
            InitializeComponent();

            // We want to show the configure link for all rates, so we
            // can open the shipping dialog
            rateControl.ActionLinkVisible = true;
            rateControl.ShowAllRates = false;

            rateControl.ActionLinkClicked += OnConfigureRateClicked;

            // Force the rates to be refreshed when the rate control tells us
            rateControl.ReloadRatesRequired += (sender, args) => RefreshRates(true);

            rateControl.Initialize(new FootnoteParameters(() => RefreshRates(false), GetStoreForCurrentShipment));
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
            selectedShipmentID = shipmentID;

            // Refresh the rates in the panel; using cached rates is fine here since nothing
            // about the shipment has changed, so don't force a re-fetch
            RefreshRates(false);
        }


        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row
        /// list with up-to-date displayed entity content.
        /// </summary>
        public void ReloadRates()
        {
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
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

                    if (!shipmentType.SupportsGetRates)
                    {
                        rateControl.ClearRates(string.Format("The provider \"{0}\" does not support retrieving rates.",
                                                                EnumHelper.GetDescription(shipmentType.ShipmentTypeCode)));
                    }
                    else
                    {
                        // We need to fetch the rates from the provider
                        FetchRates(shipment, ignoreCache);
                    }
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
        /// Fetches the rates from the shipment type and 
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="ignoreCache">Should the cached rates be ignored?</param>
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
                        panelRateGroup = new ShipmentRateGroup(ShippingManager.GetRates(shipment), shipment);
                    }
                    catch (ShippingException ex)
                    {
                        InvalidRateGroupShippingException invalidRateGroupException = ex as InvalidRateGroupShippingException;
                        if (invalidRateGroupException != null)
                        {
                            panelRateGroup = new ShipmentRateGroup(invalidRateGroupException.InvalidRates, shipment);
                        }
                        else
                        {
                            // The invalid rate group should be cached, so use the shipping manager to get the rate
                            // so we can have access to the exception footer.
                            panelRateGroup = new ShipmentRateGroup(ShippingManager.GetRates(shipment), shipment);
                        }

                        // Add the shipment ID to the exception data, so we can determine whether
                        // to update the rate control
                        ex.Data.Add("shipmentID", shipment.ShipmentID);
                        args.Result = ex;
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
                        ShipmentEntity ratedShipment = (ShipmentEntity)args.Result;
                        if (ratedShipment != null && ratedShipment.ShipmentID == selectedShipmentID)
                        {
                            // Only update the rate control if the shipment is for the currently selected 
                            // order to avoid the appearance of lag when a user is quickly clicking around
                            // the rate grid
                            LoadRates(panelRateGroup);
                        }
                    }
                };

                // Execute the work to get the rates
                rateControl.ShowSpinner = true;
                ratesWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// A helper method for loading the rates in the rate control.
        /// </summary>
        /// <param name="rateGroup">The rate group.</param>
        private void LoadRates(ShipmentRateGroup rateGroup)
        {
            // Only show the More link for the best rate shipment type
            rateControl.ShowAllRates = rateGroup.Carrier != ShipmentTypeCode.BestRate;

            rateControl.LoadRates(rateGroup);
        }

        /// <summary>
        /// Called when a [rate is selected].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="rateSelectedEventArgs">The <see cref="RateSelectedEventArgs"/> instance containing the event data.</param>
        private void OnConfigureRateClicked(object sender, RateSelectedEventArgs rateSelectedEventArgs)
        {
            ShipmentRateGroup rateGroup = (ShipmentRateGroup)rateControl.RateGroup;
            ShipmentEntity shipment = rateGroup.Shipment;

            BestRateResultTag resultTag = rateSelectedEventArgs.Rate.Tag as BestRateResultTag;
            if (resultTag != null && !resultTag.IsRealRate)
            {
                resultTag.RateSelectionDelegate(shipment);
            }
            else
            {
                using (ShippingDlg dialog = new ShippingDlg(shipment, rateSelectedEventArgs))
                {
                    dialog.ShowDialog(this);
                }
            }
        }
    }
}
