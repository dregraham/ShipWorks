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
    /// An implementation of IDockingPanelContent interface that will fetch and 
    /// show rates for an order that has been selected. If an order doesn't have 
    /// any shipments, a shipment will be created; for orders that have multiple 
    /// shipments, the first unprocessed shipment is used for rating. Rates are 
    /// not retrieved for orders that only have processed shipments.
    /// </summary>
    public partial class RatesPanel : UserControl, IDockingPanelContent
    {
        private long selectedOrderID;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatesPanel"/> class.
        /// </summary>
        public RatesPanel()
        {
            InitializeComponent();

            selectedOrderID = 0;

            // We want to show the configure link for all rates, so we
            // can open the shipping dialog
            rateControl.ShowConfigureLink = true;
            rateControl.ShowAllRates = false;

            rateControl.ConfigureRateClicked += OnConfigureRateClicked;

            // Force the rates to be refreshed when the rate control tells us
            rateControl.ReloadRatesRequired += (sender, args) => RefreshRates();

            rateControl.Initialize(new FootnoteParameters(RefreshRates, GetStoreForCurrentShipment));
        }

        /// <summary>
        /// Gets the store for the current order, if only one is selected
        /// </summary>
        private StoreEntity GetStoreForCurrentShipment()
        {
            OrderEntity order = DataProvider.GetEntity(selectedOrderID) as OrderEntity;

            if (order != null)
            {
                return StoreManager.GetStore(order.StoreID);
            }

            return null;
        }

        /// <summary>
        /// The supported filter targets that the panel can display for.
        /// </summary>
        public FilterTarget[] SupportedTargets
        {
            // Need to know when the selected order has been changed
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
            // The rates panel is interested in shipment entities - when an entity has
            // been added, removed, or modified, the panel needs to be refreshed
            get { return EntityType.ShipmentEntity; }
        }

        /// <summary>
        /// Change the content of the panel based on the given keys.
        /// </summary>
        /// <param name="selection"></param>
        public void ChangeContent(Data.Grid.IGridSelection selection)
        {
            if (selection.Count == 1)
            {
                // Make note of the selected order ID - we'll use this later to determine if the
                // user has clicked off of the order. We don't want to load the rates grid for
                // a shipment that is not for the currently selected order
                selectedOrderID = selection.Keys.FirstOrDefault();

                // Refresh the rates in the panel; using cached rates is fine here since nothing
                // about the shipment has changed, so don't force a re-fetch
                RefreshRates();
            }
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row
        /// list with up-to-date displayed entity content.
        /// </summary>
        public void ReloadContent()
        {
            // A row has been added/removed, so force the rates to be refreshed to reflect the change
            RefreshRates();
        }

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        public void UpdateContent()
        {
            // Something about the shipment has changed, so we need to refresh the rates            
            RefreshRates();
        }

        /// <summary>
        /// Forces rates to be refreshed by re-fetching the rates from the shipping provider.
        /// </summary>
        private void RefreshRates()
        {
            // This will be 0 when ShipWorks is first started and an order 
            // has not been selected yet
            if (selectedOrderID > 0)
            {
                OrderEntity order = DataProvider.GetEntity(selectedOrderID) as OrderEntity;

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
                        ShipmentType shipmentType = ShipmentTypeManager.GetType(shipmentForRating);

                        if (!shipmentType.SupportsGetRates)
                        {
                            rateControl.HideSpinner();
                            rateControl.ClearRates(string.Format("The provider \"{0}\" does not support retrieving rates.",
                                EnumHelper.GetDescription(shipmentType.ShipmentTypeCode)));
                        }
                        else
                        {
                            // We need to fetch the rates from the provider
                            //rateControl.ClearRates(string.Empty);
                            FetchRates(shipmentForRating);
                        }
                    }
                    else
                    {
                        rateControl.HideSpinner();
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
            using (BackgroundWorker ratesWorker = new BackgroundWorker())
            {
                ratesWorker.WorkerReportsProgress = false;
                ratesWorker.WorkerSupportsCancellation = true;

                // We're going to be going over the network to get rates from the provider, so show the spinner
                // while rates are being fetched to give the user some indication that we're working
                ShipmentRateGroup panelRateGroup = new ShipmentRateGroup(new RateGroup(new List<RateResult>()), shipment);
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
                        panelRateGroup = new ShipmentRateGroup(ShippingManager.GetRates(shipment), shipment);
                    }
                    catch (ShippingException ex)
                    {
                        // The invalid rate group should be cached, so use the shipping manager to get the rate
                        // so we can have access to the exception footer.
                        panelRateGroup = new ShipmentRateGroup(ShippingManager.GetRates(shipment), shipment);

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
                        if (ratedShipment != null && ratedShipment.OrderID == selectedOrderID)
                        {
                            // Only update the rate control if the shipment is for the currently selected 
                            // order to avoid the appearance of lag when a user is quickly clicking around
                            // the rate grid
                            LoadRates(panelRateGroup);
                        }
                    }
                };

                // Execute the work to get the rates
                ratesWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Update the content to reflect changes to the loaded stores
        /// </summary>
        public void UpdateStoreDependentUI()
        {
            // Do nothing
        }
        
        /// <summary>
        /// A helper method for loading the rates in the rate control.
        /// </summary>
        /// <param name="rateGroup">The rate group.</param>
        private void LoadRates(ShipmentRateGroup rateGroup)
        {
            // Only show the More link for the best rate shipment type
            rateControl.ShowAllRates = rateGroup.Carrier != ShipmentTypeCode.BestRate;

            rateControl.HideSpinner();
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
