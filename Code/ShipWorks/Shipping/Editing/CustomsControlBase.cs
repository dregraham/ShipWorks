﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// UserControl used for editing customs information
    /// </summary>
    public partial class CustomsControlBase : UserControl
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(CustomsControlBase));

        // If enableEditing was specified in LoadShipments
        protected bool enableEditing;

        // The shipments that were called LoadShipments
        List<ShipmentEntity> loadedShipments;

        // Keeps track of the selected row, if any
        List<GridRow> selectedRows = new List<GridRow>();

        // Indicates if the event shouldn't currently be raised
        int suspendShipSenseFieldEvent = 0;

        /// <summary>
        /// Some part of the packaging has changed the ShipSense criteria
        /// </summary>
        public event EventHandler ShipSenseFieldChanged;

        class RowBucket
        {
            public string Description { get; set; }
            public List<ShipmentCustomsItemEntity> CustomsItems { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomsControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Provided for derived classes to do any one-time initialization
        /// </summary>
        public virtual void Initialize()
        {
            
        }

        /// <summary>
        /// The shipments last past to LoadShipments
        /// </summary>
        protected List<ShipmentEntity> LoadedShipments => loadedShipments;

        /// <summary>
        /// The enable editing value last past to LoadShipments
        /// </summary>
        protected bool EnableEditing => enableEditing;

        /// <summary>
        /// Remove the weight option
        /// </summary>
        protected void RemoveWeight()
        {
            this.weight.Visible = false;
            this.labelWeight.Visible = false;
            AdjustPosition(value, 0, -25);
            AdjustPosition(labelValue, 0, -25);
            AdjustPosition(harmonizedCode, 0, -25);
            AdjustPosition(labelHarmonized, 0, -25);
            AdjustPosition(countryOfOrigin, 0, -25);
            AdjustPosition(labelCountryOfOrigin, 0, -25);
        }

        /// <summary>
        /// Adjust the position of a control by the given offsets
        /// </summary>
        /// <param name="control"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        private void AdjustPosition(Control control, int xOffset, int yOffset)
        {
            control.Location = new Point(control.Location.X + xOffset, control.Location.Y + yOffset);
        }

        /// <summary>
        /// Load the given shipments customs information into the control and resets the selection
        /// to the first item in the list (if there are any).
        /// </summary>
        public virtual void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing)
        {
            LoadShipments(shipments, enableEditing, true);
        }

        /// <summary>
        /// Load the given shipments customs information into the control
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool allowEditing, bool resetSelection)
        {
            SuspendShipSenseFieldChangeEvent();

            enableEditing = allowEditing;

            // Enable\disable the ContentPanels... not the groups themselves, so the groups can still be open\closed
            // Don't do the commodities panel, it gets its individual controls disabled
            foreach (CollapsibleGroupControl group in Controls.OfType<CollapsibleGroupControl>().Where(x => x != sectionContents))
            {
                group.ContentPanel.Enabled = allowEditing;
            }

            // If the country hasn't been loaded yet do that now
            if (countryOfOrigin.DataSource == null)
            {
                countryOfOrigin.DataSource = Geography.Countries;
            }

            // Event handler suspended to prevent UI flash
            itemsGrid.SelectionChanged -= this.OnItemsGridChangeSelectedRow;
            itemsGrid.Rows.Clear();
            itemsGrid.SelectionChanged += this.OnItemsGridChangeSelectedRow;

            // Each bucket represents a row that we will created.  if Description for a bucket is not null, it means EVERY shipment
            // so far has at least one content item with that description.  Number of buckets = MAX(CustomContents.Count) across all shipments.
            List<RowBucket> buckets = new List<RowBucket>();
            loadedShipments = shipments.Where(s => CustomsManager.IsCustomsRequired(s)).ToList();

            // Go through each shipment, ignoring domestic ones (which may be in this list due to multiple selection)
            foreach (ShipmentEntity shipment in loadedShipments)
            {
                Debug.Assert(shipment.CustomsItemsLoaded, "Customs contents must already be loaded!");

                // First one is a special case
                if (buckets.Count == 0)
                {
                    buckets.AddRange(shipment.CustomsItems.Select(c => new RowBucket
                    {
                        Description = c.Description,
                        CustomsItems = new List<ShipmentCustomsItemEntity> { c }
                    }));
                }
                else
                {
                    // Make a copy of the buckets that already exist that we have available to match up with
                    List<RowBucket> bucketsToUse = buckets.ToList();

                    // For the second pass, the items that we didn't find an existing bucket for
                    List<ShipmentCustomsItemEntity> unmatchedItems = new List<ShipmentCustomsItemEntity>();

                    // Go through each custom's line
                    foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
                    {
                        // Find the first bucket, if any, that already has an item placed by this description
                        RowBucket bucket = bucketsToUse.FirstOrDefault(b => b.CustomsItems.Any(c => c.Description == customsItem.Description));

                        // If we found it, add this content to its contents, and remove it from our list to use
                        if (bucket != null)
                        {
                            bucket.CustomsItems.Add(customsItem);
                            bucketsToUse.Remove(bucket);
                        }
                        // No matching bucket found.
                        else
                        {
                            unmatchedItems.Add(customsItem);
                        }
                    }

                    // Go through all the unmatched items and either just pick a remaining bucket, or create a new one if we have to
                    foreach (ShipmentCustomsItemEntity customsItem in unmatchedItems)
                    {
                        // If there are any buckets left to use, just pick one and use it
                        if (bucketsToUse.Count > 0)
                        {
                            RowBucket bucket = bucketsToUse[0];

                            // Its now a multi
                            bucket.Description = null;

                            // Remove from the list of buckets we can use, and add ourselves to its list
                            bucket.CustomsItems.Add(customsItem);
                            bucketsToUse.Remove(bucket);
                        }
                        else
                        {
                            // No more buckets left to use, have to create a new one
                            buckets.Add(new RowBucket
                            {
                                Description = customsItem.Description,
                                CustomsItems = new List<ShipmentCustomsItemEntity> { customsItem }
                            });
                        }
                    }
                }
            }

            // Load the shipment level values
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    customsValue.ApplyMultiAmount(shipment.CustomsValue);
                }
            }

            // Go through and add a row foreach bucket
            foreach (RowBucket bucket in buckets)
            {
                GridRow row = new GridRow("");
                row.Tag = bucket.CustomsItems;

                UpdateRowDescription(row, bucket.Description ?? "(Multiple)");

                itemsGrid.Rows.Add(row);
            }

            if (itemsGrid.Rows.Count > 0)
            {
                if (resetSelection)
                {
                    itemsGrid.Rows[0].Selected = true;
                }
            }
            else
            {
                ClearUI();
            }

            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Refreshes the grid contents using the shipments already loaded in the control. Any items
        /// that were previously selected are retained.
        /// </summary>
        public void RefreshItems()
        {
            // Helps to retain selections when a shipment is synced with ShipSense; this retains
            // any rows that were selected prior to the sync.

            // Make note of any rows that are selected in the grid, so we can re-select them
            // after loading the customs items
            List<object> selectedtags = selectedRows.Select(r => r.Tag).ToList();
            LoadShipments(loadedShipments, loadedShipments.All(s => !s.Processed), false);

            // Select the rows that were originally selected. Even though the rows should not have
            // changed, we got an IndexOutOfRangeException after this was released. So we'll ensure
            // that the selected index still exists before trying to select it.
            selectedtags.ForEach(t =>
            {
                itemsGrid.SelectionChanged -= OnItemsGridChangeSelectedRow;
                SelectGridRowByTag((List<ShipmentCustomsItemEntity>) t);
                itemsGrid.SelectionChanged += OnItemsGridChangeSelectedRow;
            });
        }

        /// <summary>
        /// Get the description of the bucket as it should show up in the grid
        /// </summary>
        private void UpdateRowDescription(GridRow row, string rowDescription)
        {
            List<ShipmentCustomsItemEntity> customsItems = (List<ShipmentCustomsItemEntity>) row.Tag;

            if (customsItems.Count < loadedShipments.Count)
            {
                rowDescription += string.Format(" ({0} shipment{1})", customsItems.Count, customsItems.Count > 1 ? "s" : "");
            }

            row.Cells[0].Text = rowDescription;
        }

        /// <summary>
        /// Clears the grid, all the value controls, and updates the UI state
        /// </summary>
        private void ClearUI()
        {
            itemsGrid.Rows.Clear();

            ClearValues();
            UpdateEnabledUI();
        }

        /// <summary>
        /// Clear the value data out of the entry controls
        /// </summary>
        private void ClearValues()
        {
            selectedRows.Clear();

            SuspendShipSenseFieldChangeEvent();
            description.TextChanged -= this.OnDescriptionChanged;

            quantity.Text = "";
            description.Text = "";
            weight.Weight = 0;
            value.Amount = 0;
            harmonizedCode.Text = "";
            countryOfOrigin.SelectedIndex = -1;

            description.TextChanged += this.OnDescriptionChanged;

            ResumeShipSenseFieldChangeEvent();
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Update the editable state of the control
        /// </summary>
        protected virtual void UpdateEnabledUI()
        {
            add.Enabled = enableEditing && loadedShipments.Count > 0;
            delete.Enabled = enableEditing && itemsGrid.SelectedElements.Count > 0;

            quantity.Enabled = enableEditing && itemsGrid.SelectedElements.Count > 0;
            description.Enabled = enableEditing && itemsGrid.SelectedElements.Count > 0;
            weight.Enabled = enableEditing && itemsGrid.SelectedElements.Count > 0;
            value.Enabled = enableEditing && itemsGrid.SelectedElements.Count > 0;
            harmonizedCode.Enabled = enableEditing && itemsGrid.SelectedElements.Count > 0;
            countryOfOrigin.Enabled = enableEditing && itemsGrid.SelectedElements.Count > 0;
        }

        /// <summary>
        /// Selected customs row has changed
        /// </summary>
        private void OnItemsGridChangeSelectedRow(object sender, SelectionChangedEventArgs e)
        {
            SuspendShipSenseFieldChangeEvent();

            SaveValuesToSelectedEntities();

            UpdateEnabledUI();

            if (itemsGrid.SelectedElements.Count == 0)
            {
                ClearValues();
            }
            else
            {
                description.TextChanged -= this.OnDescriptionChanged;

                selectedRows = itemsGrid.SelectedElements.Cast<GridRow>().ToList();

                using (MultiValueScope scope = new MultiValueScope())
                {
                    foreach (ShipmentCustomsItemEntity customsItem in CustomsItemsFromRows(selectedRows))
                    {
                        LoadFormData(customsItem);
                    }
                }

                description.TextChanged += this.OnDescriptionChanged;
            }

            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Loads the form data.
        /// </summary>
        /// <param name="customsItem">The customs item.</param>
        protected virtual void LoadFormData(ShipmentCustomsItemEntity customsItem)
        {
            description.ApplyMultiText(customsItem.Description);
            quantity.ApplyMultiText(customsItem.Quantity.ToString());
            weight.ApplyMultiWeight(customsItem.Weight);
            value.ApplyMultiText(customsItem.UnitValue.ToString());
            harmonizedCode.ApplyMultiText(customsItem.HarmonizedCode);
            countryOfOrigin.ApplyMultiText(Geography.GetCountryName(customsItem.CountryOfOrigin));
        }

        /// <summary>
        /// Save data currently in the last selected row
        /// </summary>
        private void SaveValuesToSelectedEntities()
        {
            // Keeps a list for all shipments whose content weights or values have changed
            List<long> changedWeights = new List<long>();
            List<long> changedValues = new List<long>();

            foreach (ShipmentCustomsItemEntity customsItem in CustomsItemsFromRows(selectedRows))
            {
                SaveCustomsItem(customsItem, changedWeights, changedValues);
            }

            // Update the content weights and values for all affected shipments
            UpdateContentWeight(loadedShipments.Where(s => changedWeights.Contains(s.ShipmentID)));
            UpdateCustomsValue(loadedShipments.Where(s => changedValues.Contains(s.ShipmentID)));
        }

        /// <summary>
        /// Get the customs items from all the selected rows
        /// </summary>
        private static IEnumerable<ShipmentCustomsItemEntity> CustomsItemsFromRows(IEnumerable<GridRow> shipmentRows) =>
            shipmentRows.Select(x => x.Tag).Cast<List<ShipmentCustomsItemEntity>>().SelectMany(x => x);

        /// <summary>
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        public virtual void FlushChanges() => weight.FlushChanges();

        /// <summary>
        /// Saves the customs item.
        /// </summary>
        /// <param name="customsItem">The customs item.</param>
        /// <param name="changedWeights">The changed weights.</param>
        /// <param name="changedValues">The changed values.</param>
        protected virtual void SaveCustomsItem(ShipmentCustomsItemEntity customsItem, List<long> changedWeights, List<long> changedValues)
        {
            description.ReadMultiText(s => customsItem.Description = s);

            try
            {
                quantity.ReadMultiText(s =>
                {
                    double quantityValue;
                    if (double.TryParse(s, NumberStyles.Any, null, out quantityValue))
                    {
                        if (quantityValue != customsItem.Quantity)
                        {
                            customsItem.Quantity = quantityValue;
                            changedWeights.Add(customsItem.ShipmentID);
                            changedValues.Add(customsItem.ShipmentID);
                        }
                    }
                });
                weight.ReadMultiWeight(newWeight =>
                {
                    if (customsItem.Weight != newWeight)
                    {
                        customsItem.Weight = newWeight;
                        changedWeights.Add(customsItem.ShipmentID);
                    }
                });
                value.ReadMultiText(s =>
                {
                    decimal unitValue;
                    if (decimal.TryParse(s, NumberStyles.Any, null, out unitValue))
                    {
                        if (unitValue != customsItem.UnitValue)
                        {
                            customsItem.UnitValue = unitValue;
                            changedValues.Add(customsItem.ShipmentID);
                        }
                    }
                });
            }
            catch (ORMEntityIsDeletedException ex)
            {
                // ShipSense sync might delete the original customs item i think.
                log.Error(string.Format("Error saving customs item: {0}", ex.Message));
            }
            harmonizedCode.ReadMultiText(s => customsItem.HarmonizedCode = s);
            countryOfOrigin.ReadMultiText(s => customsItem.CountryOfOrigin = Geography.GetCountryCode(s));
        }

        /// <summary>
        /// Update the ContentWeight for each of the given shipments
        /// </summary>
        private void UpdateContentWeight(IEnumerable<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                shipment.ContentWeight = shipment.CustomsItems.Sum(c => c.Quantity * c.Weight);
            }
        }

        /// <summary>
        /// The focus is leaving a control that affects the customs value
        /// </summary>
        private void OnLeaveValueAffectingControl(object sender, EventArgs e)
        {
            SaveValuesToSelectedEntities();
        }

        /// <summary>
        /// Update the customs value for each shipment in the list
        /// </summary>
        private void UpdateCustomsValue(IEnumerable<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                shipment.CustomsValue = CalculateCustomsValue(shipment);
            }

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    customsValue.ApplyMultiAmount(shipment.CustomsValue);
                }
            }
        }

        /// <summary>
        /// Calculate the total customs value for the given shipment
        /// </summary>
        /// <remarks>
        /// This method will clamp the value it returns to the max value that can be stored in a field
        /// </remarks>
        private decimal CalculateCustomsValue(IShipmentEntity shipment)
        {
            decimal decimalValue = shipment.CustomsItems.Sum(c => ((decimal) c.Quantity * c.UnitValue));

            return Math.Min(decimalValue, SqlUtility.MoneyMaxValue);
        }

        /// <summary>
        /// Handler for when the text in the description box changes
        /// </summary>
        private void OnDescriptionChanged(object sender, EventArgs e)
        {
            foreach (GridRow row in itemsGrid.SelectedElements)
            {
                UpdateRowDescription(row, description.Text);
            }

            SaveValuesToSelectedEntities();

            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Delete the customs contents represented by all the selected rows
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete all selected customs content?");
            if (result != DialogResult.OK)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Keeps a list for all shipments whose content weights have changed
            Dictionary<ShipmentEntity, bool> affectedShipments = new Dictionary<ShipmentEntity, bool>();

            // Capture the selected rows that will be removed
            List<GridRow> rowsToRemove = itemsGrid.SelectedElements.Cast<GridRow>().ToList();

            // Capture customs items that are being removed (and eventually deleted)
            var customsItems = rowsToRemove.SelectMany(r => (List<ShipmentCustomsItemEntity>) r.Tag).ToList();

            // Clear the selection and remove each of the rows
            itemsGrid.SelectedElements.Clear();
            rowsToRemove.ForEach(r => itemsGrid.Rows.Remove(r));

            // Remove the customs items from each of their shipments
            foreach (var item in customsItems)
            {
                ShipmentEntity shipment = item.Shipment;
                shipment.CustomsItems.Remove(item);

                affectedShipments[shipment] = true;
            }

            // Update the content weight values for all affected shipments
            UpdateContentWeight(affectedShipments.Select(p => p.Key));
            UpdateCustomsValue(affectedShipments.Select(p => p.Key));

            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Add a shipment commodity to every loaded shipment
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            itemsGrid.SelectedElements.Clear();

            List<ShipmentCustomsItemEntity> createdList = loadedShipments.Select(CustomsManager.CreateCustomsItem).ToList();

            // We only need one row to represent all the items we just created
            if (createdList.Count > 0)
            {
                GridRow row = new GridRow(createdList[0].Description);
                row.Tag = createdList;

                itemsGrid.Rows.Add(row);

                RaiseShipSenseFieldChanged();

                SelectGridRowByTag(createdList);
                selectedRows = itemsGrid.SelectedElements.Cast<GridRow>().ToList();
            }
            else
            {
                RaiseShipSenseFieldChanged();
            }
        }

        /// <summary>
        /// Given a tag, select the first grid row that equals that tag.
        /// </summary>
        private void SelectGridRowByTag(List<ShipmentCustomsItemEntity> tag)
        {
            GridRow matchedRow = itemsGrid.Rows.Cast<GridRow>().FirstOrDefault(r =>
            {
                List<ShipmentCustomsItemEntity> tagInGrid = ((List<ShipmentCustomsItemEntity>) r.Tag);
                return tagInGrid.All(customsItemFromGrid => tag.Any(customsItem => customsItem == customsItemFromGrid));
            });

            if (matchedRow != null)
            {
                matchedRow.Selected = true;
            }
        }

        /// <summary>
        /// Save the UI state to the given shipment list
        /// </summary>
        public virtual void SaveToShipments()
        {
            SaveValuesToSelectedEntities();

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                customsValue.ReadMultiAmount(amount => shipment.CustomsValue = amount);
            }
        }

        /// <summary>
        /// Laying out controls
        /// </summary>
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            // They are all collapsible controls
            if (Controls.Count == Controls.OfType<CollapsibleGroupControl>().Count())
            {
                int location = AutoScrollPosition.Y + 5;

                foreach (CollapsibleGroupControl group in Controls.OfType<CollapsibleGroupControl>().Where(x => x.Visible))
                {
                    group.Location = new Point(group.Location.X, location);
                    location = group.Bottom + 5;
                }
            }
        }

        /// <summary>
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Raises the ShipSenseFieldChanged event
        /// </summary>
        private void RaiseShipSenseFieldChanged()
        {
            if (suspendShipSenseFieldEvent > 0)
            {
                return;
            }

            ShipSenseFieldChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Suspend raising the event that ShipSense criteria has changed
        /// </summary>
        protected void SuspendShipSenseFieldChangeEvent()
        {
            suspendShipSenseFieldEvent++;
        }

        /// <summary>
        /// Resume raising the event that the rate ShipSense has changed.  This function does not raise the event
        /// </summary>
        protected void ResumeShipSenseFieldChangeEvent()
        {
            suspendShipSenseFieldEvent--;
        }

        /// <summary>
        /// Unload shipments
        /// </summary>
        internal void UnloadShipments()
        {
            loadedShipments.Clear();
        }
    }
}
