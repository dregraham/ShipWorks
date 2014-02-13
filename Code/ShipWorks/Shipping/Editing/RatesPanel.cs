using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.Editing
{
    public partial class RatesPanel : UserControl, IDockingPanelContent
    {
        public RatesPanel()
        {
            InitializeComponent();
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
            OrderEntity order = DataProvider.GetEntity(selection.Keys.FirstOrDefault()) as OrderEntity;
            if (order != null)
            {
                rateControl.ClearRates(string.Empty);
                rateControl.ShowSpinner();

                ShipmentEntity shipment = ShippingManager.CreateShipment(order.OrderID);
                ShipmentType shipmentType = ShipmentTypeManager.GetType((ShipmentTypeCode)shipment.ShipmentType);
                
                RateGroup rateGroup = shipmentType.GetRates(shipment);

                rateControl.HideSpinner();
                rateControl.LoadRates(rateGroup);
            }
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row
        /// list with up-to-date displayed entity content.
        /// </summary>
        public void ReloadContent()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        public void UpdateContent()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Update the content to reflect changes to the loaded stores
        /// </summary>
        public void UpdateStoreDependentUI()
        {
            //throw new NotImplementedException();
        }
    }
}
