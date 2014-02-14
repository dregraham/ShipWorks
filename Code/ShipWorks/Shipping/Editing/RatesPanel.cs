using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// An implementation of IDockingPanelContent interface that will fetch and 
    /// show rates for an order that has been selected. Rates for the last 1000
    /// shipments get cached. If an order doesn't have any shipments, a shipment
    /// will be created; for orders that have multiple shipments, the first 
    /// unprocessed shipment is used for rating. Rates are not retrieved for
    /// orders that only have processed shipments.
    /// </summary>
    public partial class RatesPanel : UserControl, IDockingPanelContent
    {
        private BackgroundWorker ratesWorker;
        private readonly LruCache<long, RateGroup> cachedRates;
        private long selectedOrderID;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatesPanel"/> class.
        /// </summary>
        public RatesPanel()
        {
            InitializeComponent();

            cachedRates = new LruCache<long, RateGroup>(1000);
            selectedOrderID = 0;

            rateControl.RateSelected += OnRateSelected;
        }

        /// <summary>
        /// The supported filter targets that the panel can display for.
        /// </summary>
        public Filters.FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders }; }
        }

        /// <summary>
        /// Indicates if the panel can handle multiple selected items at one time.
        /// </summary>
        public bool SupportsMultiSelect
        {
            get { return false; }
        }

        /// <summary>
        /// Load the state of the panel.
        /// </summary>
        public void LoadState()
        {
            // Do nothing
        }

        /// <summary>
        /// Save the state of the panel.
        /// </summary>
        public void SaveState()
        {
            // Do nothing
        }

        /// <summary>
        /// The EntityType displayed by the panel grid
        /// </summary>
        public EntityType EntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// Change the content of the panel based on the given keys.
        /// </summary>
        /// <param name="selection"></param>
        public void ChangeContent(Data.Grid.IGridSelection selection)
        {
            if (selection.Count == 1)
            {
                OrderEntity order = DataProvider.GetEntity(selection.Keys.FirstOrDefault()) as OrderEntity;

                if (order != null)
                {
                    // Make note of the selected order ID - we'll use this later to determine if the
                    // user has clicked off of the order. We don't want to load the rates grid for
                    // a shipment that is not for the currently selected order
                    selectedOrderID = order.OrderID;

                    List<ShipmentEntity> shipments = ShippingManager.GetShipments(order.OrderID, true);
                    if (shipments.Any(s => !s.Processed))
                    {
                        // Grab the first unprocessed shipment
                        ShipmentEntity shipmentForRating = shipments.FirstOrDefault(s => !s.Processed);

                        if (cachedRates.Contains(shipmentForRating.ShipmentID))
                        {
                            // Rates for this shipment have already been cached
                            RateGroup rateGroup = cachedRates[shipmentForRating.ShipmentID];
                            
                            rateControl.HideSpinner();
                            rateControl.LoadRates(rateGroup);
                            
                        }
                        else
                        {
                            // We need to fetch the rates from the provider
                            rateControl.ClearRates(string.Empty);
                            FetchRates(shipmentForRating);
                        }
                    }
                    else
                    {
                        rateControl.ClearRates("All shipments for this order have been processed.");
                    }
                }
            }
        }

        /// <summary>
        /// Fetches the rates from the shipment type and 
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void FetchRates(ShipmentEntity shipment)
        {
            if (ratesWorker != null && ratesWorker.IsBusy && !ratesWorker.CancellationPending)
            {
                ratesWorker.CancelAsync();
                ratesWorker.Dispose();
            }

            ratesWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = true
            };

            // We're going to be going over the network to get rates from the provider, so show the spinner
            // while rates are being fetched to give the user some indication that we're working
            RateGroup rateGroup = new RateGroup(new List<RateResult>());
            rateControl.ShowSpinner();

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

                    // Fetch the rates and add them to the cache
                    rateGroup = ShippingManager.GetRates(shipment);
                    cachedRates[shipment.ShipmentID] = rateGroup;
                }
                catch (ShippingException ex)
                {
                    // Add the order ID to the exception data, so we can determine whether
                    // to update the rate control
                    ex.Data.Add("orderID", shipment.OrderID);
                    args.Result = ex;
                }
            };

            // What to run when the work has been completed
            ratesWorker.RunWorkerCompleted += (sender, args) =>
            {
                ShippingException exception = args.Result as ShippingException;
                if (exception != null)
                {
                    if (exception.Data.Contains("orderID") && (long)exception.Data["orderID"] == selectedOrderID)
                    {
                        // Update the rate control if the selected order is the one that 
                        // produced the error
                        rateControl.HideSpinner();
                        rateControl.ClearRates(exception.Message);
                    }
                }
                else
                {
                    ShipmentEntity ratedShipment = (ShipmentEntity)args.Result;
                    if (ratedShipment != null && ratedShipment.OrderID == selectedOrderID)
                    {
                        // Only update the rate control if the shipment is for the currently selected 
                        // order to avoid the appearance of lag when a user is quickly clicking around
                        // the rate grid
                        rateControl.HideSpinner();
                        rateControl.LoadRates(rateGroup);
                    }
                }
            };

            // Execute the work to get the rates
            ratesWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row
        /// list with up-to-date displayed entity content.
        /// </summary>
        public void ReloadContent()
        {
            // Do nothing
        }

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        public void UpdateContent()
        {
            // Do nothing
        }

        /// <summary>
        /// Update the content to reflect changes to the loaded stores
        /// </summary>
        public void UpdateStoreDependentUI()
        {
            // Do nothing
        }


        /// <summary>
        /// Called when [rate selected].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="rateSelectedEventArgs">The <see cref="RateSelectedEventArgs"/> instance containing the event data.</param>
        private void OnRateSelected(object sender, RateSelectedEventArgs rateSelectedEventArgs)
        {
            MessageHelper.ShowMessage(this, "A rate was selected. This still needs to be implemented");
        }
    }
}
