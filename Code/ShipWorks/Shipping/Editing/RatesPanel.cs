using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Editing
{
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

            cachedRates = new LruCache<long, RateGroup>(100);
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
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Save the state of the panel.
        /// </summary>
        public void SaveState()
        {
            //throw new NotImplementedException();
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
                    selectedOrderID = order.OrderID;

                    List<ShipmentEntity> shipments = ShippingManager.GetShipments(order.OrderID, true);
                    if (shipments.Any(s => !s.Processed))
                    {
                        ShipmentEntity shipmentForRating = shipments.FirstOrDefault(s => !s.Processed);

                        if (cachedRates.Contains(shipmentForRating.ShipmentID))
                        {
                            RateGroup rateGroup = cachedRates[shipmentForRating.ShipmentID];
                            
                            rateControl.HideSpinner();
                            rateControl.LoadRates(rateGroup);
                            
                        }
                        else
                        {
                            FetchRates(shipmentForRating);
                        }
                    }
                    else
                    {
                        rateControl.ClearRates("All shipments for this order have processed.");
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

            RateGroup rateGroup = new RateGroup(new List<RateResult>());
            rateControl.ClearRates(string.Empty);
            rateControl.ShowSpinner();

            ratesWorker.DoWork += (sender, args) =>
            {
                ShippingManager.EnsureShipmentLoaded(shipment);

                ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);
                args.Result = clonedShipment;

                //ShipmentType shipmentType = ShipmentTypeManager.GetType((ShipmentTypeCode)clonedShipment.ShipmentType);
                //rateGroup = shipmentType.GetRates(clonedShipment);
                rateGroup = ShippingManager.GetRates(clonedShipment);

                cachedRates[clonedShipment.ShipmentID] = rateGroup;
            };

            ratesWorker.RunWorkerCompleted += (sender, args) =>
            {
                ShipmentEntity ratedShipment = (ShipmentEntity)args.Result;
                if (ratedShipment.OrderID == selectedOrderID)
                {
                    rateControl.HideSpinner();
                    rateControl.LoadRates(rateGroup);
                }
            };

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
            MessageHelper.ShowMessage(this, "A rate was selected");
        }
    }
}
