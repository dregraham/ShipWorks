using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public partial class UpsPackageDetailControl : UserControl
    {
        List<ShipmentEntity> loadedShipments;

        /// <summary>
        /// The user has edited\changed something
        /// </summary>
        public event EventHandler PackageDetailsChanged;

        // So we know when not to raise the changed event
        bool loading = false;

        // Keeps track of the selected rows, so when the selection changes, we know what to save
        List<GridRow> selectedRows = new List<GridRow>();

        public UpsPackageDetailControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the shipments into the packaging control
        /// </summary>
        [NDependIgnoreLongMethod]
        public void LoadShipments(List<ShipmentEntity> shipments, bool enableEditing)
        {
            loading = true;

            packagesGrid.SelectionChanged -= this.OnChangeSelectedPackages;

            loadedShipments = shipments;

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

            List<List<UpsPackageEntity>> packageBuckets = new List<List<UpsPackageEntity>>();

            // Load the shipment data
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    // Sort the packages into the buckets.  Each bucket will correspond to a grid row
                    // package[X] of each shipment will end up in packageBuckets[X]
                    for (int index = 0; index < shipment.Ups.Packages.Count; index++)
                    {
                        UpsPackageEntity package = shipment.Ups.Packages[index];

                        // We don't have a bucket for this index yet
                        if (packageBuckets.Count == index)
                        {
                            packageBuckets.Add(new List<UpsPackageEntity>());
                        }

                        // Add the package to the bucket
                        packageBuckets[index].Add(package);
                    }
                }
            }
            
            // Add in each bucket as a row in the grid
            foreach (List<UpsPackageEntity> bucket in packageBuckets)
            {
                GridRow gridRow = new GridRow("");
                gridRow.Tag = bucket;

                packagesGrid.Rows.Add(gridRow);

                UpdateRowText(gridRow);
            }

            packagesGrid.SelectionChanged += this.OnChangeSelectedPackages;

            // The only way there wouldnt be any packages is if there was no selection
            if (packagesGrid.Rows.Count > 0)
            {
                packagesGrid.Rows[0].Selected = true;
            }

            UpdateLayout();

            loading = false;

            OnPackageDetailsChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Update the display t ext of the given GridRow, which is dependant on how many packages it has
        /// </summary>
        private void UpdateRowText(GridRow gridRow)
        {
            string text = string.Format("Package {0}", gridRow.Index + 1);

            List<UpsPackageEntity> packages = (List<UpsPackageEntity>)gridRow.Tag;
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
                packageDetailPanel.Top = panelPackageSelector.Top;
            }
            else
            {
                int gridHeight = (Math.Min(rows, 5) + 1) * DetailViewSettings.SingleRowHeight + 6;

                packagesGrid.Visible = true;
                packagesGrid.Height = gridHeight;
                panelPackageSelector.Visible = true;
                packageDetailPanel.Top = packagesGrid.Bottom;
            }

            // Be just tall enough to hold the package content
            this.Height = packageDetailPanel.Bottom;
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

                    List<UpsPackageEntity> packages = (List<UpsPackageEntity>) gridRow.Tag;

                    // Load the data from each selected package
                    foreach (UpsPackageEntity package in packages)
                    {
                        additionalHandling.ApplyMultiCheck(package.AdditionalHandlingEnabled);

                        dryIceDetails.ApplyFrom(package);
                        verbalConfirmation.ApplyFrom(package);
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
                List<UpsPackageEntity> packages = (List<UpsPackageEntity>)gridRow.Tag;

                // Load the data from each selected package
                foreach (UpsPackageEntity package in packages)
                {
                    additionalHandling.ReadMultiCheck(x => package.AdditionalHandlingEnabled = x);

                    dryIceDetails.ReadInto(package);
                    verbalConfirmation.ReadInto(package);
                }
            }
        }

        public void PackageCountChanged(object sender, EventArgs e)
        {
            SaveToEntities();

            // package count wouldn't change if control weren't editable.
            LoadShipments(loadedShipments,true);
        }

        /// <summary>
        /// Indicates that the user has changed package details
        /// </summary>
        private void OnPackageDetailsChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                return;
            }

            if (PackageDetailsChanged != null)
            {
                PackageDetailsChanged(this, EventArgs.Empty);
            }
        }

    }
}
