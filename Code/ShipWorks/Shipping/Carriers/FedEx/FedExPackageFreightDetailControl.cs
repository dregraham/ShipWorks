﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.FedEx;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Control for managing LTL freight package level properties
    /// </summary>
    public partial class FedExPackageFreightDetailControl : UserControl
    {
        List<ShipmentEntity> loadedShipments;

        // So we know when not to raise the changed event
        bool loading = false;

        // Keeps track of the selected rows, so when the selection changes, we know what to save
        List<GridRow> selectedRows = new List<GridRow>();

        /// <summary>
        /// Some part of the packaging has changed the rate criteria
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExPackageFreightDetailControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<FedExFreightPhysicalPackagingType>(freightPackaging);

            freightPackaging.SelectedValueChanged += OnRateCriteriaChanged;
            freightPieces.TextChanged += OnRateCriteriaChanged;
        }

        /// <summary>
        /// Load the shipments into the packaging control
        /// </summary>
        [NDependIgnoreLongMethod]
        public void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing)
        {
            loading = true;

            packagesGrid.SelectionChanged -= this.OnChangeSelectedPackages;

            loadedShipments = shipments.ToList();

            // Enable all the controls (except the grid, which you can always select from) based on the enabled state
            foreach (Control control in Controls)
            {
                if (control != packagesGrid)
                {
                    control.Enabled = enableEditing;
                }
            }

            // Clear previous rows
            packagesGrid.Rows.Clear();
            selectedRows.Clear();

            List<List<FedExPackageEntity>> packageBuckets = new List<List<FedExPackageEntity>>();

            // Load the shipment data
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    // Sort the packages into the buckets.  Each bucket will correspond to a grid row
                    // package[X] of each shipment will end up in packageBuckets[X]
                    for (int index = 0; index < shipment.FedEx.Packages.Count; index++)
                    {
                        FedExPackageEntity package = shipment.FedEx.Packages[index];

                        // We don't have a bucket for this index yet
                        if (packageBuckets.Count == index)
                        {
                            packageBuckets.Add(new List<FedExPackageEntity>());
                        }

                        // Add the package to the bucket
                        packageBuckets[index].Add(package);
                    }
                }
            }

            // Add in each bucket as a row in the grid
            foreach (List<FedExPackageEntity> bucket in packageBuckets)
            {
                GridRow gridRow = new GridRow("");
                gridRow.Tag = bucket;

                packagesGrid.Rows.Add(gridRow);

                UpdateRowText(gridRow);
            }

            packagesGrid.SelectionChanged += this.OnChangeSelectedPackages;

            // The only way there wouldn't be any packages is if there was no selection
            if (packagesGrid.Rows.Count > 0)
            {
                packagesGrid.Rows[0].Selected = true;
            }

            UpdateLayout();

            loading = false;
        }

        /// <summary>
        /// Update the display text of the given GridRow, which is Dependant on how many packages it has
        /// </summary>
        private void UpdateRowText(GridRow gridRow)
        {
            string text = string.Format("Package {0}", gridRow.Index + 1);

            List<FedExPackageEntity> packages = (List<FedExPackageEntity>)gridRow.Tag;
            if (packages.Count < loadedShipments.Count)
            {
                text += string.Format(" ({0} shipment{1})", packages.Count, packages.Count > 1 ? "s" : "");
            }

            gridRow.Cells[0].Text = text;
        }

        /// <summary>
        /// Update the layout and size of the control based on the package count
        /// </summary>
        private void UpdateLayout()
        {
            int rows = packagesGrid.Rows.Count;

            // Update the grid UI sizing
            if (rows <= 1)
            {
                panelPackageSelector.Visible = false;
            }
            else
            {
                int gridHeight = (Math.Min(rows, 5) + 1) * DetailViewSettings.SingleRowHeight + 6;

                packagesGrid.Visible = true;
                packagesGrid.Height = gridHeight;
                panelPackageSelector.Visible = true;
            }
        }

        /// <summary>
        /// Called when [change selected packages].
        /// </summary>
        private void OnChangeSelectedPackages(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
        {
            SaveToEntities();

            selectedRows.Clear();

            using (MultiValueScope scope = new MultiValueScope())
            {
                // Go through each selected row
                foreach (GridRow gridRow in packagesGrid.SelectedElements)
                {
                    selectedRows.Add(gridRow);

                    List<FedExPackageEntity> packages = (List<FedExPackageEntity>) gridRow.Tag;

                    // Load the data from each selected package
                    foreach (FedExPackageEntity package in packages)
                    {
                        freightPieces.ApplyMultiText(package.FreightPieces.ToString());
                        freightPackaging.ApplyMultiValue(package.FreightPackaging);
                    }
                }
            }
        }

        /// <summary>
        /// Save the current values to the selected entities
        /// </summary>
        public void SaveToEntities()
        {
            // Go through each selected row
            foreach (GridRow gridRow in selectedRows)
            {
                List<FedExPackageEntity> packages = (List<FedExPackageEntity>)gridRow.Tag;

                // Load the data from each selected package
                foreach (FedExPackageEntity package in packages)
                {
                    freightPieces.ReadMultiText(t =>
                    {
                        int pieces;
                        if (int.TryParse(freightPieces.Text, out pieces))
                        {
                            package.FreightPieces = pieces;
                        }
                    });

                    freightPackaging.ReadMultiValue(p => package.FreightPackaging = (FedExFreightPhysicalPackagingType) p);
                }
            }
        }

        /// <summary>
        /// The package count changed so reload.
        /// </summary>
        public void PackageCountChanged(int packageCount)
        {
            SaveToEntities();

            // package count wouldn't change if control weren't editable.
            LoadShipments(loadedShipments,true);
        }

        /// <summary>
        /// Rate criteria changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            // Raise the rate criteria changed event
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Raises the RateCriteriaChanged event
        /// </summary>
        private void RaiseRateCriteriaChanged()
        {
            if (!loading)
            {
                RateCriteriaChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                freightPackaging.SelectedValueChanged -= OnRateCriteriaChanged;
                freightPieces.TextChanged -= OnRateCriteriaChanged;
            }

            base.Dispose(disposing);
        }
    }
}
