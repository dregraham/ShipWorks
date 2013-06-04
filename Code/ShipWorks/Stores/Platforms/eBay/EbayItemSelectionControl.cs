using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Control to allow the user to select individual order items from a provided order, 
    /// or to summarize the total number of order items to be impacted if multiple
    /// orders are provided.
    /// </summary>
    public partial class EbayItemSelectionControl : UserControl
    {
        /// <summary>
        /// Container for holding combobox items
        /// </summary>
        class ItemComboEntry
        {
            // The text to display in the combobox
            string text;
            public string Text
            {
                get
                {
                    if (OrderItem == null)
                    {
                        return text;
                    }
                    else
                    {
                        return String.Format("{0} - {1}", OrderItem.Code, OrderItem.Name);
                    }
                }
                set
                {
                    text = value;
                }
            }

            // the item attached to this
            public EbayOrderItemEntity OrderItem { get; set; }

            /// <summary>
            /// return the display string
            /// </summary>
            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        /// Returns the selected item id
        /// </summary>
        public long SelectedKey
        {
            get
            {
                ItemComboEntry entry = itemComboBox.SelectedItem as ItemComboEntry;
                if (entry != null)
                {
                    if (entry.OrderItem != null)
                    {
                        return entry.OrderItem.OrderItemID;
                    }
                }

                // 0 indicates the "all" option was selected
                return 0;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayItemSelectionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the orders to have order items displayed
        /// </summary>
        public void LoadOrders(List<long> orderIds)
        {
            if (orderIds.Count == 1)
            {
                // get the order
                EbayOrderEntity order = DataProvider.GetEntity(orderIds[0]) as EbayOrderEntity;
                if (order == null)
                {
                    itemComboBox.Items.Add(new ItemComboEntry { Text = "The selected order was deleted." });
                }
                else
                {
                    // get all of the related Order Items
                    int counter = 0;
                    foreach (EbayOrderItemEntity orderItem in DataProvider.GetRelatedEntities(order.OrderID, EntityType.EbayOrderItemEntity))
                    {
                        counter++;

                        itemComboBox.Items.Add(new ItemComboEntry { OrderItem = orderItem });
                    }

                    if (counter > 1)
                    {
                        itemComboBox.Items.Insert(0, new ItemComboEntry { Text = String.Format("All {0} items", counter) });
                    }
                }

                // select the first item
                itemComboBox.SelectedIndex = 0;
            }
            else
            {
                // count how many orders and order items there are
                List<EbayOrderEntity> ebayOrders = new List<EbayOrderEntity>();
                orderIds.ForEach(orderId => ebayOrders.Add(DataProvider.GetEntity(orderId) as EbayOrderEntity));

                // prune any manual orders
                ebayOrders.RemoveAll(o => o == null || o.IsManual);

                // now count how many order items are involved
                int itemCount = GetItemCount(ebayOrders);

                // multiple orders selected, just allow leaving for all at once
                itemComboBox.Items.Add(new ItemComboEntry { Text = String.Format("{0} orders, {1} items", ebayOrders.Count, itemCount) });

                // select the first item
                itemComboBox.SelectedIndex = 0;

                // disable
                itemComboBox.Enabled = false;
            }
        }

        /// <summary>
        /// Returns the number of order items associated with the specified orders
        /// </summary>
        private int GetItemCount(List<EbayOrderEntity> ebayOrders)
        {
            int count = 0;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ebayOrders.ForEach(o =>
                {
                    object result = adapter.GetScalar(OrderItemFields.OrderItemID, null, AggregateFunction.Count, OrderItemFields.OrderID == o.OrderID & OrderItemFields.IsManual == false);

                    count += (result is DBNull) ? 0 : Convert.ToInt32(result);
                });
            }

            return count;
        }
    }
}
