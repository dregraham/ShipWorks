using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Divelements.SandGrid.Specialized;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Control for combining orders from a buyer
    /// </summary>
    public partial class EbayCombineOrderControl : UserControl
    {
        EbayCombinedOrderType combineType;
        EbayCombinedOrderCandidate candidate;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayCombineOrderControl(EbayCombinedOrderType combineType, EbayCombinedOrderCandidate combinedOrder)
        {
            InitializeComponent();

            this.candidate = combinedOrder;
            this.combineType = combineType;

            if (combineType == EbayCombinedOrderType.Local)
            {
                // hide the ebay details panel
                eBayDetailsPanel.Visible = false;

                // move things up
                Height = Height - eBayDetailsPanel.Height;
            }
        }

        /// <summary>
        /// Indicates if the orders are being combined locally or on ebay
        /// </summary>
        public EbayCombinedOrderType CombinedOrderType
        {
            get { return combineType; }
        }

        /// <summary>
        /// The candidate combined order object
        /// </summary>
        public EbayCombinedOrderCandidate Candidate
        {
            get { return candidate; }
        }

        /// <summary>
        /// Indicates if the user has checked this candidate for combining
        /// </summary>
        public bool IsSelected
        {
            get { return enableCheckBox.Checked; }
        }

        /// <summary>
        /// Loading
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            buyerLabel.Text = candidate.BuyerID;

            sandGrid.Rows.Clear();

            // populate the grid
            foreach (EbayCombinedOrderComponent component in candidate.Components)
            {
                GridRow row = new GridRow(new GridCell[]
                    {
                        new GridCell(component.Order.OrderNumberComplete),
                        new GridDateTimeCell(component.Order.OrderDate),
                        new GridCell(GetItemNumber(component.Order)),
                        new GridCell(GetItemTitle(component.Order)),
                        new GridDecimalCell(component.Order.OrderTotal)
                    });

                row.Checked = component.Included;
                row.Tag = component;

                sandGrid.Rows.Add(row);
            }

            enableCheckBox.Checked = true;

            // populate the shipping and adjustments
            shippingTextBox.Amount = candidate.ShippingCost;
            taxTextBox.Text = "0.000";

            // load the state combo
            taxStateComboBox.Items.Add("No Sales Tax");
            taxStateComboBox.Items.AddRange(Geography.States.ToArray());
            taxStateComboBox.SelectedIndex = 0;

            // load the shipping methods
            shippingServiceComboBox.ValueMember = "Key";
            shippingServiceComboBox.DisplayMember = "Value";
            shippingServiceComboBox.DataSource = EbayUtility.ShippingMethods;

            EbayStoreEntity store = (EbayStoreEntity)StoreManager.GetStore(candidate.StoreID);
            if (IsDomestic())
            {
                shippingServiceComboBox.SelectedValue = store.DomesticShippingService;
            }
            else
            {
                shippingServiceComboBox.SelectedValue = store.InternationalShippingService;
            }
        }

        /// <summary>
        /// Determine if the buyer is a domestic one
        /// </summary>
        private bool IsDomestic()
        {
            // get the most recent, included order
            EbayOrderEntity orderTemplate = candidate.Components.Where(c => c.Included).OrderByDescending(c => c.Order.OnlineLastModified).First().Order;

            return orderTemplate.ShipCountryCode == "US";
        }

        /// <summary>
        /// Gets the title of the item for this order
        /// </summary>
        private string GetItemTitle(EbayOrderEntity ebayOrder)
        {
            if (ebayOrder.OrderItems == null)
            {
                return "";
            }

            return ebayOrder.OrderItems.First().Name;
        }

        /// <summary>
        /// Gets the item number of the item for this order
        /// </summary>
        private string GetItemNumber(EbayOrderEntity ebayOrder)
        {
            if (ebayOrder.OrderItems == null)
            {
                return "";
            }

            return ((EbayOrderItemEntity)ebayOrder.OrderItems.First()).EbayItemID.ToString();
        }

        /// <summary>
        /// Checked to include/exclude this combined payment
        /// </summary>
        private void OnEnableChecked(object sender, EventArgs e)
        {
            combinedPanel.Enabled = enableCheckBox.Checked;
        }

        /// <summary>
        /// User has toggled a row 
        /// </summary>
        private void OnAfterRowCheck(object sender, GridRowCheckEventArgs e)
        {
            bool rowChecked = e.Row.Checked;

            EbayCombinedOrderComponent component = e.Row.Tag as EbayCombinedOrderComponent;
            component.Included = rowChecked;
        }

        /// <summary>
        /// User changed the shipping value
        /// </summary>
        private void OnShippingChanged(object sender, EventArgs e)
        {
            candidate.ShippingCost = shippingTextBox.Amount;
        }

        /// <summary>
        /// User chnaged the tax value
        /// </summary>
        private void OnTaxChanged(object sender, EventArgs e)
        {
            try
            {
                candidate.TaxPercent = Convert.ToDecimal(taxTextBox.Text);
            }
            catch (FormatException)
            {
                candidate.TaxPercent = 0;
            }
        }

        /// <summary>
        /// A new state is selected
        /// </summary>
        private void OnTaxStateChanged(object sender, EventArgs e)
        {
            // get the text
            if (taxStateComboBox.SelectedIndex == 0)
            {
                taxTextBox.Enabled = false;
                taxShippingCheckBox.Enabled = false;
                candidate.TaxState = "";
            }
            else
            {
                taxTextBox.Enabled = true;
                taxShippingCheckBox.Enabled = true;
                candidate.TaxState = taxStateComboBox.Text;
            }
        }

        /// <summary>
        /// Toggle of taxing shipping and handling
        /// </summary>
        private void OnTaxShippingChanged(object sender, EventArgs e)
        {
            candidate.TaxShipping = taxShippingCheckBox.Checked;
        }

        /// <summary>
        /// User changed the shipping method
        /// </summary>
        private void OnShippingMethodChanged(object sender, EventArgs e)
        {
            object selectedValue = shippingServiceComboBox.SelectedValue;
            candidate.ShippingService = selectedValue == null ? "" : (string)selectedValue;
        }
    }
}
