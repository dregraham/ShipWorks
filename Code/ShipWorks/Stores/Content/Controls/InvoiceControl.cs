using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.Data;

namespace ShipWorks.Stores.Content.Controls
{
    /// <summary>
    /// UserControl that is an item\charge editor that has an invoice layout
    /// </summary>
    public partial class InvoiceControl : UserControl
    {
        PanelDataMode dataMode;

        long currentOrderID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public InvoiceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control and load the column state
        /// </summary>
        public void Initialize(Guid itemsLayoutID, Guid chargesLayoutID)
        {
            Initialize(itemsLayoutID, chargesLayoutID, PanelDataMode.LiveDatabase);
        }

        /// <summary>
        /// Initialize the control and load the column state
        /// </summary>
        public void Initialize(Guid itemsLayoutID, Guid chargesLayoutID, PanelDataMode dataMode)
        {
            this.dataMode = dataMode;

            itemsControl.Initialize(itemsLayoutID, GridColumnDefinitionSet.OrderItems, dataMode, (GridColumnLayout layout) =>
                {
                    // Move the status before the price so that Price from Items aligns with Amount from Charges
                    layout.AllColumns[OrderItemFields.UnitPrice].Position = layout.AllColumns[OrderItemFields.LocalStatus].Position;

                    // Hide all the store specific columns
                    foreach (GridColumnPosition column in layout.AllColumns.Where(c => c.Definition.StoreTypeCode != null))
                    {
                        column.Visible = false;
                    }
                });

            chargesControl.Initialize(chargesLayoutID, GridColumnDefinitionSet.Charges, dataMode, null);

            itemsControl.LoadState();
            chargesControl.LoadState();
        }

        /// <summary>
        /// The PanelDataMode the panel is in
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelDataMode DataMode
        {
            get { return dataMode; }
        }

        /// <summary>
        /// The local items that has been added.  Only valid if the data mode is local
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OrderItemEntity> LocalItems
        {
            get { return itemsControl.LocalItems; }
        }

        /// <summary>
        /// The local charges that have been added.  Only valid if the data mode is local.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OrderChargeEntity> LocalCharges
        {
            get { return chargesControl.LocalCharges; }
        }

        /// <summary>
        /// The store ID that the local set of items goes with.  Only used of data mode is local.  Use to populate the item status menu.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long? LocalStoreID
        {
            get { return itemsControl.LocalStoreID; }
            set { itemsControl.LocalStoreID = value; }
        }

        /// <summary>
        /// Update the content for the given order
        /// </summary>
        public void UpdateContent(long orderID)
        {
            currentOrderID = orderID;

            itemsControl.ChangeContent(orderID );
            chargesControl.ChangeContent(orderID);

            decimal total = 0;
            if (dataMode == PanelDataMode.LiveDatabase)
            {
                OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);
                if (order != null)
                {
                    total = order.OrderTotal;
                }
            }
            else
            {
                total = OrderUtility.CalculateTotal(LocalItems, LocalCharges);
            }

            int orderTotalRight = orderTotal.Right;
            orderTotal.Text = total.ToString("c");
            orderTotal.Left = orderTotalRight - orderTotal.Width;
        }

        /// <summary>
        /// Save the column state
        /// </summary>
        public void SaveState()
        {
            itemsControl.SaveState();
            chargesControl.SaveState();
        }

        /// <summary>
        /// Resizing the control
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            UpdateOrderDetailLayout();
        }

        /// <summary>
        /// The ideal size of one of the order detail grid panels has changed
        /// </summary>
        private void OnOrderDetailIdealSizeChanged(object sender, EventArgs e)
        {
            UpdateOrderDetailLayout();
        }

        /// <summary>
        /// Update layout and positioning of the order detail content
        /// </summary>
        private void UpdateOrderDetailLayout()
        {
            // Determine how much height we would ideally have
            int height = itemsControl.IdealSize.Height + chargesControl.IdealSize.Height + panelOrderTotal.Height;

            // If we fit, set the height of everyone to what it wants
            if (height < this.Height)
            {
                itemsControl.Height = itemsControl.IdealSize.Height;
                chargesControl.Height = chargesControl.IdealSize.Height;
            }
            else
            {
                // Divide the space among the two controls
                int spaceForTop = (int) Math.Floor((this.Height - panelOrderTotal.Height) / 2.0);
                int spaceForBottom = spaceForTop;

                if (itemsControl.IdealSize.Height < spaceForTop)
                {
                    spaceForBottom += (spaceForTop - itemsControl.IdealSize.Height);
                    spaceForTop = itemsControl.IdealSize.Height;
                }
                else if (chargesControl.IdealSize.Height < spaceForBottom)
                {
                    spaceForTop += (spaceForBottom - chargesControl.IdealSize.Height);
                    spaceForBottom = chargesControl.IdealSize.Height;
                }

                itemsControl.Height = spaceForTop;
                chargesControl.Height = spaceForBottom;
            }

            // Layout the positioning
            chargesControl.Top = itemsControl.Bottom + 1;
            panelOrderTotal.Top = chargesControl.Bottom + 1;

            int chargesRight = Math.Min(this.Width, itemsControl.IdealSize.Width);

            // Try to align the charges section with the  items section
            chargesControl.Width = Math.Min(this.Width, chargesControl.IdealSize.Width);
            chargesControl.Left = Math.Max(0, chargesRight - chargesControl.Width);

            // Try to align the totals control
            panelOrderTotal.Left = chargesControl.Right - panelOrderTotal.Width;
        }

        /// <summary>
        /// A charge has been added, edited, or deleted
        /// </summary>
        private void OnChargesChanged(object sender, EventArgs e)
        {
            UpdateContent(currentOrderID);
        }

        /// <summary>
        /// An item has been added, edited, or deleted
        /// </summary>
        private void OnItemsChanged(object sender, EventArgs e)
        {
            UpdateContent(currentOrderID);
        }
    }
}
