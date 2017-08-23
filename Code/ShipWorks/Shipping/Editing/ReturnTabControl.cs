using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// UserControl used for editing return item information
    /// </summary>
    public partial class ReturnTabControl : UserControl
    {
        // The shipments that were called LoadShipments
        ShipmentEntity loadedShipment;

        // Keeps track of the selected row, if any
        List<GridRow> selectedRows = new List<GridRow>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnTabControl()
        {
            InitializeComponent();
        }
      
        public void LoadShipments(List<ShipmentEntity> shipments, bool createIfEmpty)
        {
            if (!shipments.IsCountEqualTo(1))
            {
                sectionContents.Visible = false;
                return;
            }

            // Event handler suspended to prevent UI flash
            itemsGrid.SelectionChanged -= OnItemsGridChangeSelectedRow;
            itemsGrid.Rows.Clear();
            itemsGrid.SelectionChanged += OnItemsGridChangeSelectedRow;

            loadedShipment = shipments.Single();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                lifetimeScope.Resolve<IReturnItemRepository>().LoadReturnData(loadedShipment, createIfEmpty);
            }

            foreach (ShipmentReturnItemEntity item in loadedShipment.ShipmentReturnItem)
            {
                GridRow row = new GridRow(item.Name) {Tag = item};
                itemsGrid.Rows.Add(row);
            }
        }

        /// <summary>
        /// Selected return row has changed
        /// </summary>
        private void OnItemsGridChangeSelectedRow(object sender, SelectionChangedEventArgs e)
        {
            SaveValuesToSelectedEntities();
            
            if (itemsGrid.SelectedElements.Count == 0)
            {
                ClearValues();
            }
            else
            {
                name.TextChanged -= OnDescriptionChanged;
                selectedRows = itemsGrid.SelectedElements.Cast<GridRow>().ToList();

                using (MultiValueScope scope = new MultiValueScope())
                {
                    foreach (ShipmentReturnItemEntity returnItem in ReturnItemsFromRows(selectedRows))
                    {
                        LoadFormData(returnItem);
                    }
                }

                name.TextChanged += OnDescriptionChanged;
            }
            
        }

        /// <summary>
        /// Loads the form data.
        /// </summary>
        /// <param name="returnItem">The return item.</param>
        protected virtual void LoadFormData(ShipmentReturnItemEntity returnItem)
        {
            name.ApplyMultiText(returnItem.Name);
            sku.ApplyMultiText(returnItem.SKU);
            code.ApplyMultiText(returnItem.Code);
            quantity.ApplyMultiText(returnItem.Quantity.ToString(CultureInfo.InvariantCulture));
            weight.ApplyMultiWeight(returnItem.Weight);
            notes.ApplyMultiText(returnItem.Notes);
        }

        /// <summary>
        /// Handler for when the text in the description box changes
        /// </summary>
        private void OnDescriptionChanged(object sender, EventArgs e)
        {
            foreach (GridRow row in itemsGrid.SelectedElements)
            {
                row.Cells[0].Text = name.Text;
            }

            SaveValuesToSelectedEntities();
        }
        
        /// <summary>
        /// Clear the value data out of the entry controls
        /// </summary>
        private void ClearValues()
        {
            selectedRows.Clear();

            itemsGrid.SelectedElements.Clear();
                
            name.TextChanged -= OnDescriptionChanged;

            name.Text = string.Empty;
            sku.Text = string.Empty;
            code.Text = string.Empty;
            quantity.Text = string.Empty;
            weight.Weight = 0;
            notes.Text = string.Empty;

            name.TextChanged += OnDescriptionChanged;
        }

        /// <summary>
        /// Save data currently in the last selected row
        /// </summary>
        private void SaveValuesToSelectedEntities()
        {
            foreach (ShipmentReturnItemEntity returnItem in ReturnItemsFromRows(selectedRows))
            {
                SaveReturnItem(returnItem);
            }

            loadedShipment.ContentWeight = loadedShipment.ShipmentReturnItem.Sum(c => c.Quantity * c.Weight);
        }

        /// <summary>
        /// Saves the return item.
        /// </summary>
        /// <param name="returnItem">The return item.</param>
        protected virtual void SaveReturnItem(ShipmentReturnItemEntity returnItem)
        {
            quantity.ReadMultiText(s =>
            {
                double quantityValue;
                if (double.TryParse(s, NumberStyles.Any, null, out quantityValue))
                {
                    if (Math.Abs(quantityValue - returnItem.Quantity) > .01)
                    {
                        returnItem.Quantity = quantityValue;
                    }
                }
            });

            weight.ReadMultiWeight(newWeight =>
            {
                if (Math.Abs(returnItem.Weight - newWeight) > .01)
                {
                    returnItem.Weight = newWeight;
                }
            });
               
            name.ReadMultiText(s => returnItem.Name = s);
            sku.ReadMultiText(s => returnItem.SKU = s);
            code.ReadMultiText(s => returnItem.Code = s);
            notes.ReadMultiText(s => returnItem.Notes = s);
        }

        /// <summary>
        /// Get the return items from all the selected rows
        /// </summary>
        private static IEnumerable<ShipmentReturnItemEntity> ReturnItemsFromRows(IEnumerable<GridRow> shipmentRows)
        {
            return shipmentRows.Select(row => row.Tag as ShipmentReturnItemEntity).ToList();
        }
          
        /// <summary>
        /// Delete selected return item
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete all selected return items?");
            if (result != DialogResult.OK)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            
            // Capture the selected rows that will be removed
            List<GridRow> rowsToRemove = itemsGrid.SelectedElements.Cast<GridRow>().ToList();

            // Capture return items that are being removed (and eventually deleted)
            IEnumerable<ShipmentReturnItemEntity> returnItems = ReturnItemsFromRows(rowsToRemove);

            // Clear the selection and remove each of the rows
            itemsGrid.SelectedElements.Clear();
            rowsToRemove.ForEach(r => itemsGrid.Rows.Remove(r));

            // Remove the return items from each of their shipments
            foreach (ShipmentReturnItemEntity item in returnItems)
            {
                loadedShipment.ShipmentReturnItem.Remove(item);
            }

            loadedShipment.ContentWeight = loadedShipment.ShipmentReturnItem.Sum(c => c.Quantity * c.Weight);
        }

        /// <summary>
        /// Add a return item 
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            itemsGrid.SelectedElements.Clear();

            ShipmentReturnItemEntity newItem = new ShipmentReturnItemEntity()
            {
                ShipmentID = loadedShipment.ShipmentID,
                Name = string.Empty,
                SKU = string.Empty,
                Code = string.Empty,
                Quantity = 0,
                Weight = 0,
                Notes = string.Empty
            };
            
            GridRow row = new GridRow(newItem.Name) {Tag = newItem};

            itemsGrid.Rows.Add(row);
            loadedShipment.ShipmentReturnItem.Add(newItem);

            SelectGridRowByTag(newItem);
            selectedRows = itemsGrid.SelectedElements.Cast<GridRow>().ToList();
        }

        /// <summary>
        /// Given a tag, select the first grid row that equals that tag.
        /// </summary>
        private void SelectGridRowByTag(ShipmentReturnItemEntity tag)
        {
            GridRow matchedRow = itemsGrid.Rows.Cast<GridRow>()
                .FirstOrDefault(r => Equals((ShipmentReturnItemEntity) r.Tag, tag));

            if (matchedRow != null)
            {
                matchedRow.Selected = true;
            }
        }
    }
}

