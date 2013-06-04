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

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Control for combining orders from a buyer
    /// </summary>
    public partial class EbayCombineOrderControl : UserControl
    {
        // type of combining being configured
        CombineType combineType;
        public CombineType CombineType
        {
            get { return combineType; }
        }

        // the combined order being configured
        CombinedOrder combinedOrder;
        public CombinedOrder CombinedOrder
        {
            get { return combinedOrder; }
        }

        public bool IsSelected
        {
            get { return enableCheckBox.Checked; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayCombineOrderControl(CombineType combineType, CombinedOrder combinedOrder)
        {
            InitializeComponent();

            this.combinedOrder = combinedOrder;
            this.combineType = combineType;

            if (combineType == CombineType.Local)
            {
                // hide the ebay details panel
                eBayDetailsPanel.Visible = false;

                // move things up
                Height = Height - eBayDetailsPanel.Height;
            }
        }

        /// <summary>
        /// Loading
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            buyerLabel.Text = combinedOrder.EbayBuyerID;

            sandGrid.Rows.Clear();

            // populate the grid
            foreach (CombinedOrderComponent component in combinedOrder.Components)
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
            shippingTextBox.Amount = combinedOrder.ShippingCost;
            taxTextBox.Text = "0.000";

            // load the state combo
            taxStateComboBox.Items.Add("No Sales Tax");
            taxStateComboBox.Items.AddRange(Geography.States.ToArray());
            taxStateComboBox.SelectedIndex = 0;

            // load the shipping methods
            shippingServiceComboBox.ValueMember = "Key";
            shippingServiceComboBox.DisplayMember = "Value";
            shippingServiceComboBox.DataSource = EbayUtility.ShippingMethods;

            EbayStoreEntity store = (EbayStoreEntity)StoreManager.GetStore(combinedOrder.StoreID);
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
            EbayOrderEntity orderTemplate = combinedOrder.Components.Where(c => c.Included).OrderByDescending(c => c.Order.OnlineLastModified).First().Order;

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

            CombinedOrderComponent component = e.Row.Tag as CombinedOrderComponent;
            component.Included = rowChecked;
        }

        /// <summary>
        /// User changed the shipping value
        /// </summary>
        private void OnShippingChanged(object sender, EventArgs e)
        {
            combinedOrder.ShippingCost = shippingTextBox.Amount;
        }

        /// <summary>
        /// User chnaged the tax value
        /// </summary>
        private void OnTaxChanged(object sender, EventArgs e)
        {
            try
            {
                combinedOrder.TaxPercent = Convert.ToDecimal(taxTextBox.Text);
            }
            catch (FormatException)
            {
                combinedOrder.TaxPercent = 0;
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
                combinedOrder.TaxState = "";
            }
            else
            {
                taxTextBox.Enabled = true;
                taxShippingCheckBox.Enabled = true;
                combinedOrder.TaxState = taxStateComboBox.Text;
            }
        }

        /// <summary>
        /// Toggle of taxing shipping and handling
        /// </summary>
        private void OnTaxShippingChanged(object sender, EventArgs e)
        {
            combinedOrder.TaxShipping = taxShippingCheckBox.Checked;
        }

        /// <summary>
        /// User changed the shipping method
        /// </summary>
        private void OnShippingMethodChanged(object sender, EventArgs e)
        {
            object selectedValue = shippingServiceComboBox.SelectedValue;
            combinedOrder.ShippingService = selectedValue == null ? "" : (string)selectedValue;
        }
    }
}
