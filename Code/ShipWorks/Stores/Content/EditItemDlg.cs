using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content.Panels;
using System.Globalization;
using ShipWorks.Data.Connection;
using ShipWorks.Templates.Tokens;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using Divelements.SandGrid;
using Divelements.SandGrid.Specialized;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Form for editing a single item of an order
    /// </summary>
    public partial class EditItemDlg : Form
    {
        OrderItemEntity item;
        long storeID;
        PanelDataMode dataMode;

        List<OrderItemAttributeEntity> allAttributes = new List<OrderItemAttributeEntity>();
        List<OrderItemAttributeEntity> addedAttributes = new List<OrderItemAttributeEntity>();
        List<OrderItemAttributeEntity> deletedAttributes = new List<OrderItemAttributeEntity>();

        /// <summary>
        /// Constructor.  StoreID is necessary b\c there is no way to know the store when dataMode is local.
        /// </summary>
        public EditItemDlg(OrderItemEntity item, long storeID, PanelDataMode dataMode)
        {
            InitializeComponent();

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.item = item;
            this.storeID = storeID;
            this.dataMode = dataMode;

            base.Text = item.IsNew ? "Add Order Item" : "Edit Order Item";
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            name.Text = item.Name;
            code.Text = item.Code;
            quantity.Text = item.Quantity.ToString();
            price.Amount = item.UnitPrice;
            UpdateStatusDisplay(item.LocalStatus);

            sku.Text = item.SKU;
            upc.Text = item.UPC;
            isbn.Text = item.ISBN;
            location.Text = item.Location;
            weight.Weight = item.Weight;
            cost.Amount = item.UnitCost;
            description.Text = item.Description;
            thumbnailUrl.Text = item.Thumbnail;
            imageUrl.Text = item.Image;

            LoadAttributes();

            gridLinkEdit.ButtonClicked += OnEditAttribute;
            gridLinkDelete.ButtonClicked += OnDeleteAttribute;
        }

        /// <summary>
        /// Load the attribute grid
        /// </summary>
        private void LoadAttributes()
        {
            IEnumerable<OrderItemAttributeEntity> attributes = new List<OrderItemAttributeEntity>();

            if (dataMode == PanelDataMode.LocalPending)
            {
                attributes = item.OrderItemAttributes.Select(EntityUtility.CloneEntity);
            }
            else if (!item.IsNew)
            {
                attributes = DataProvider.GetRelatedEntities(item.OrderItemID, EntityType.OrderItemAttributeEntity).Cast<OrderItemAttributeEntity>();
            }

            foreach (OrderItemAttributeEntity attribute in attributes)
            {
                attributeGrid.Rows.Add(CreateGridRow(attribute));

                allAttributes.Add(attribute);
            }
        }

        /// <summary>
        /// Create and return a new grid row for the given attribute
        /// </summary>
        private static GridRow CreateGridRow(OrderItemAttributeEntity attribute)
        {
            GridRow row = new GridRow(new GridCell[]
                    {
                        new GridCell(),
                        new GridCell(),
                        new GridHyperlinkCell("Edit"),
                        new GridHyperlinkCell("Delete")
                    });

            row.Tag = attribute;

            UpdateGridRow(row);

            return row;
        }

        /// <summary>
        /// Update the display state of the given grid row based on the attribute
        /// </summary>
        private static void UpdateGridRow(GridRow row)
        {
            OrderItemAttributeEntity attribute = (OrderItemAttributeEntity) row.Tag;

            string text = attribute.Name;
            if (text.Length > 0 && attribute.Description.Length > 0)
            {
                text += ": ";
            }

            text += attribute.Description;

            row.Cells[0].Text = text;
            row.Cells[1].Text = attribute.UnitPrice == 0 ? "" : attribute.UnitPrice.ToString("c");
        }

        /// <summary>
        /// Add a new attribute
        /// </summary>
        private void OnAddAttribute(object sender, EventArgs e)
        {
            OrderItemAttributeEntity attribute = new OrderItemAttributeEntity();
            attribute.IsManual = true;

            attribute.OrderItemID = item.OrderItemID;
            attribute.InitializeNullsToDefault();

            // For local ones, they have to have a fakeID until they are ready to save
            if (dataMode == PanelDataMode.LocalPending)
            {
                attribute.OrderItemAttributeID = allAttributes.Count == 0 ?
                    -EntityUtility.GetEntitySeed(EntityType.OrderItemAttributeEntity) :
                    allAttributes.Min(a => a.OrderItemAttributeID) - 1000;
            }

            using (EditAttributeDlg dlg = new EditAttributeDlg(attribute))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    attributeGrid.Rows.Add(CreateGridRow(attribute));

                    allAttributes.Add(attribute);
                    addedAttributes.Add(attribute);
                }
            }
        }

        /// <summary>
        /// Edit the selected attribute
        /// </summary>
        void OnEditAttribute(object sender, GridRowColumnEventArgs e)
        {
            EditAttribute(e.Row);
        }

        /// <summary>
        /// User has double-clicked a row
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            EditAttribute(e.Row);
        }

        /// <summary>
        /// Initiate editing of the attribute attached to the given row
        /// </summary>
        private void EditAttribute(GridRow gridRow)
        {
            using (EditAttributeDlg dlg = new EditAttributeDlg((OrderItemAttributeEntity) gridRow.Tag))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateGridRow(gridRow);
                }
            }
        }

        /// <summary>
        /// Delete the selected attribute
        /// </summary>
        void OnDeleteAttribute(object sender, GridRowColumnEventArgs e)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected attribute?");

            if (result == DialogResult.OK)
            {
                OrderItemAttributeEntity attribute = (OrderItemAttributeEntity) e.Row.Tag;

                if (addedAttributes.Contains(attribute))
                {
                    addedAttributes.Remove(attribute);
                }
                else
                {
                    deletedAttributes.Add(attribute);
                }

                attributeGrid.Rows.Remove(e.Row);
            }
        }

        /// <summary>
        /// Update the status display for the given value
        /// </summary>
        private void UpdateStatusDisplay(string value)
        {
            status.Text = value.Trim().Length == 0 ? "(none)" : value;
            status.Tag = value;
        }

        /// <summary>
        /// Open the local status menu so the user can select
        /// </summary>
        private void OnLinkLocalStatus(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            contextMenu.Items.AddRange(MenuCommandConverter.ToToolStripItems(
                StatusPresetCommands.CreateMenuCommands(StatusPresetTarget.OrderItem, new List<long> { storeID }),
                OnSetLocalStatus));

            contextMenu.Show(status.Parent.PointToScreen(new Point(status.Left, status.Bottom)));
        }

        /// <summary>
        /// Execute an invoked menu command
        /// </summary>
        void OnSetLocalStatus(object sender, EventArgs e)
        {
            MenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

            StatusPresetEntity preset = (StatusPresetEntity) command.Tag;
            UpdateStatusDisplay(preset.StatusText);            

            if (TemplateTokenProcessor.HasTokens(preset.StatusText))
            {
                MessageHelper.ShowInformation(this, "ShipWorks will process the tokens in the status text when the item is saved.");
            }
        }

        /// <summary>
        /// Commit the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            double quantityValue;
            if (!double.TryParse(quantity.Text, NumberStyles.Any, null, out quantityValue))
            {
                MessageHelper.ShowMessage(this, "Enter a valid quantity for the item.");
                return;
            }

            item.Name = name.Text;
            item.Code = code.Text;
            item.Quantity = quantityValue;
            item.UnitPrice = price.Amount;
            item.LocalStatus = status.Tag.ToString();

            item.SKU = sku.Text;
            item.ISBN = isbn.Text;
            item.UPC = upc.Text;

            item.Location = location.Text;
            item.Weight = weight.Weight;
            item.UnitCost = cost.Amount;
            item.Description = description.Text;
            item.Thumbnail = thumbnailUrl.Text;
            item.Image = imageUrl.Text;

            try
            {
                if (dataMode == PanelDataMode.LiveDatabase)
                {
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        adapter.SaveAndRefetch(item);

                        foreach (OrderItemAttributeEntity deleted in deletedAttributes)
                        {
                            adapter.DeleteEntity(deleted);
                        }

                        foreach (GridRow row in attributeGrid.Rows)
                        {
                            OrderItemAttributeEntity attribute = (OrderItemAttributeEntity) row.Tag;

                            // If the item was new, then we have just gotten the item ID
                            attribute.OrderItemID = item.OrderItemID;

                            adapter.SaveEntity(attribute);
                        }

                        OrderUtility.UpdateOrderTotal(item.OrderID);

                        if (TemplateTokenProcessor.HasTokens(item.LocalStatus))
                        {
                            item.LocalStatus = TemplateTokenProcessor.ProcessTokens(item.LocalStatus, item.OrderItemID);
                        }


                        // Everything has been set on the order, so calculate the hash key
                        OrderEntity order = (OrderEntity) DataProvider.GetEntity(item.OrderID);
                        OrderUtility.PopulateOrderDetails(order, adapter);
                        
                        OrderUtility.UpdateShipSenseHashKey(order);
                        adapter.SaveAndRefetch(order);

                        adapter.Commit();
                    }
                }
                else
                {
                    foreach (OrderItemAttributeEntity deleted in deletedAttributes)
                    {
                        item.OrderItemAttributes.Remove(item.OrderItemAttributes.Single(a => a.OrderItemAttributeID == deleted.OrderItemAttributeID));
                    }

                    foreach (OrderItemAttributeEntity existing in item.OrderItemAttributes)
                    {
                        OrderItemAttributeEntity working = (OrderItemAttributeEntity) attributeGrid.Rows.Cast<GridRow>()
                            .Select(r => (OrderItemAttributeEntity) r.Tag).Single(a => a.OrderItemAttributeID == existing.OrderItemAttributeID);

                        existing.Name = working.Name;
                        existing.Description = working.Description;
                        existing.UnitPrice = working.UnitPrice;
                    }

                    foreach (OrderItemAttributeEntity added in addedAttributes)
                    {
                        item.OrderItemAttributes.Add(added);
                    }
                }

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this,
                    "The item has been edited or deleted by another user, and your changes could not be saved.");

                DialogResult = DialogResult.Abort;
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowError(this,
                    "The order has been deleted, and the item cannot be saved.");

                DialogResult = DialogResult.Abort;
            }
        }
    }
}
