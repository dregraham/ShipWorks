using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.OwnedInstances;
using Divelements.SandGrid;
using Interapptive.Shared;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// User control for editing DHL Express packages
    /// </summary>
    public partial class DhlExpressPackageControl : UserControl
    {
        List<ShipmentEntity> loadedShipments;

        // Keeps track of the selected rows, so when the selection changes, we know what to save
        List<GridRow> selectedRows = new List<GridRow>();

        // Indicates if the event shouldn't currently be raised
        int suspendRateCriteriaEvent = 0;

        // Indicates if the event shouldn't currently be raised
        int suspendShipSenseFieldChangedEvent = 0;

        /// <summary>
        /// Some part of the packaging has changed the rate criteria
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Some part of the packaging has changed the ShipSense criteria
        /// </summary>
        public event EventHandler ShipSenseFieldChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressPackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// One-time control initialization
        /// </summary>
        public void Initialize()
        {
            dimensionsControl.Initialize();

            List<KeyValuePair<string, int>> packageCountData = new List<KeyValuePair<string, int>>();

            for (int i = 1; i <= 25; i++)
            {
                packageCountData.Add(new KeyValuePair<string, int>(i.ToString(), i));
            }

            packageCountCombo.Items.Clear();
            packageCountCombo.DisplayMember = "Key";
            packageCountCombo.ValueMember = "Value";
            packageCountCombo.DataSource = packageCountData;

            packageCountCombo.SelectedIndexChanged += this.OnChangePackageCount;

            weight.ConfigureTelemetryEntityCounts = telemetryEvent =>
            {
                telemetryEvent.AddMetric(WeightControl.ShipmentQuantityTelemetryKey,
                    loadedShipments?.Count ?? 0);
                telemetryEvent.AddMetric(WeightControl.PackageQuantityTelemetryKey,
                    selectedRows.Select(x => x.Tag).OfType<List<DhlExpressPackageEntity>>().SelectMany(x => x).Count());
            };
        }

        /// <summary>
        /// Load the shipments into the packaging control
        /// </summary>
        [NDependIgnoreLongMethod]
        public void LoadShipments(List<ShipmentEntity> shipments, bool enableEditing)
        {
            this.loadedShipments = shipments;

            // Enable all the controls (except the grid, which you can always select from) based on the enabled state
            foreach (Control control in Controls)
            {
                if (control != packagesGrid)
                {
                    control.Enabled = enableEditing;
                }
            }

            suspendRateCriteriaEvent++;
            suspendShipSenseFieldChangedEvent++;

            packageCountCombo.SelectedIndexChanged -= this.OnChangePackageCount;
            packagesGrid.SelectionChanged -= this.OnChangeSelectedPackages;

            // Clear previous rows
            packagesGrid.Rows.Clear();
            selectedRows.Clear();
            dimensionsControl.LoadDimensions(
                shipments.SelectMany(s => s.DhlExpress.Packages).Select(p => new DimensionsAdapter(p)));

            List<List<DhlExpressPackageEntity>> packageBuckets = new List<List<DhlExpressPackageEntity>>();

            // Load the shipment data
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    packageCountCombo.ApplyMultiValue(shipment.DhlExpress.Packages.Count);

                    int index = 0;

                    // Sort the packages into the buckets.  Each bucket will correspond to a grid row
                    foreach (DhlExpressPackageEntity package in shipment.DhlExpress.Packages)
                    {
                        // We don't have a bucket for this index yet
                        if (packageBuckets.Count == index)
                        {
                            packageBuckets.Add(new List<DhlExpressPackageEntity>());
                        }

                        // Add the package to the bucket
                        packageBuckets[index].Add(package);

                        // Next bucket next time
                        index++;
                    }
                }
            }

            // Add in each bucket as a row in the grid
            foreach (List<DhlExpressPackageEntity> bucket in packageBuckets)
            {
                GridRow gridRow = new GridRow("");
                gridRow.Tag = bucket;

                packagesGrid.Rows.Add(gridRow);

                UpdateRowText(gridRow);
            }

            UpdateInsuranceDisplay();

            // Start listening again
            packageCountCombo.SelectedIndexChanged += this.OnChangePackageCount;
            packagesGrid.SelectionChanged += this.OnChangeSelectedPackages;

            // The only way there wouldn't be any packages is if there was no selection
            if (packagesGrid.Rows.Count > 0)
            {
                packagesGrid.Rows[0].Selected = true;
            }

            UpdateLayout();

            suspendRateCriteriaEvent--;
            suspendShipSenseFieldChangedEvent--;
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
                packagesGrid.Visible = false;
                panelPackage.Top = packagesGrid.Top;
            }
            else
            {
                int gridHeight = (Math.Min(rows, 5) + 1) * DetailViewSettings.SingleRowHeight + 6;

                packagesGrid.Visible = true;
                packagesGrid.Height = gridHeight;
                panelPackage.Top = packagesGrid.Bottom;
            }

            // Be just tall enough to hold the package content
            this.Height = panelPackage.Bottom;
        }

        /// <summary>
        /// Update the display text of the given GridRow, which is dependent on how many packages it has
        /// </summary>
        private void UpdateRowText(GridRow gridRow)
        {
            string text = string.Format("Package {0}", gridRow.Index + 1);

            List<DhlExpressPackageEntity> packages = (List<DhlExpressPackageEntity>) gridRow.Tag;
            if (packages.Count < loadedShipments.Count)
            {
                text += string.Format(" ({0} shipment{1})", packages.Count, packages.Count > 1 ? "s" : "");
            }

            gridRow.Cells[0].Text = text;
        }

        /// <summary>
        /// Change how many packages there are in the selected shipments
        /// </summary>
        private void OnChangePackageCount(object sender, EventArgs e)
        {
            // Changing the count will cause a UI reload.  Have to save before we do that.
            SaveToEntities();

            int count = (int) packageCountCombo.SelectedValue;

            // Have to go through each shipment and setup the correct count
            foreach (ShipmentEntity shipment in loadedShipments)
            {
                DhlExpressShipmentEntity DhlExpressShipmentEntity = shipment.DhlExpress;

                // Need more
                while (DhlExpressShipmentEntity.Packages.Count < count)
                {
                    DhlExpressPackageEntity package = DhlExpressShipmentType.CreateDefaultPackage();
                    DhlExpressShipmentEntity.Packages.Add(package);
                }

                // Need less
                while (DhlExpressShipmentEntity.Packages.Count > count)
                {
                    DhlExpressPackageEntity package = DhlExpressShipmentEntity.Packages[DhlExpressShipmentEntity.Packages.Count - 1];
                    DhlExpressShipmentEntity.Packages.Remove(package);
                }
            }

            // Have to reload the UI
            LoadShipments(loadedShipments, packageCountCombo.Enabled);

            // Raise the rate criteria changed event
            RaiseRateCriteriaChanged();

            // Raise the ShipSense criteria changed event
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Changing the selected package(s)
        /// </summary>
        private void OnChangeSelectedPackages(object sender, SelectionChangedEventArgs e)
        {
            suspendRateCriteriaEvent++;
            suspendShipSenseFieldChangedEvent++;

            // Save the existing stuff before loading the new stuff
            SaveToEntities();
            selectedRows.Clear();

            List<DimensionsAdapter> dimensionsToLoad = new List<DimensionsAdapter>();

            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;
            
            using (MultiValueScope scope = new MultiValueScope())
            {
                // Go through each selected row
                foreach (GridRow gridRow in packagesGrid.SelectedElements)
                {
                    selectedRows.Add(gridRow);

                    List<DhlExpressPackageEntity> packages = (List<DhlExpressPackageEntity>) gridRow.Tag;

                    // Load the data from each selected package
                    foreach (DhlExpressPackageEntity package in packages)
                    {
                        weight.ApplyMultiWeight(package.Weight);                        
                        dimensionsToLoad.Add(new DimensionsAdapter(package));
                    }
                }
            }

            UpdateInsuranceDisplay();

            // Load the dimensions
            dimensionsControl.LoadDimensions(dimensionsToLoad);

            // Start the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;

            suspendRateCriteriaEvent--;
            suspendShipSenseFieldChangedEvent--;
        }

        /// <summary>
        /// Save the current values to the selected entities
        /// </summary>
        public void SaveToEntities()
        {
            // Go through each selected row
            foreach (GridRow gridRow in selectedRows)
            {
                List<DhlExpressPackageEntity> packages = (List<DhlExpressPackageEntity>) gridRow.Tag;

                // Load the data from each selected package
                foreach (DhlExpressPackageEntity package in packages)
                {
                    weight.ReadMultiWeight(w => package.Weight = w);
                }
            }

            // Save insurance
            insuranceControl.SaveToInsuranceChoices();

            // Save dimensions
            dimensionsControl.SaveToEntities();
        }

        /// <summary>
        /// Update the insurance display information
        /// </summary>
        public void UpdateInsuranceDisplay()
        {
            List<IInsuranceChoice> insuranceToLoad = new List<IInsuranceChoice>();

            using (MultiValueScope scope = new MultiValueScope())
            {            
                // Go through each selected row
                foreach (GridRow gridRow in packagesGrid.SelectedElements)
                {
                    List<DhlExpressPackageEntity> packages = (List<DhlExpressPackageEntity>)gridRow.Tag;
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.DhlExpress);
                
                    // Load the data from each selected package
                    foreach (DhlExpressPackageEntity package in packages)
                    {
                        insuranceToLoad.Add(shipmentType.GetParcelDetail(package.DhlExpressShipment.Shipment, package.DhlExpressShipment.Packages.IndexOf(package)).Insurance);
                    }
                }
            }
            
            // Load the insurance
            insuranceControl.LoadInsuranceChoices(insuranceToLoad);

        }

        /// <summary>
        /// Something affecting rate criteria has changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Raises the RateCriteriaChanged event
        /// </summary>
        private void RaiseRateCriteriaChanged()
        {
            if (suspendRateCriteriaEvent > 0)
            {
                return;
            }

            if (RateCriteriaChanged != null)
            {
                RateCriteriaChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Something affecting ShipSense criteria has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        internal void FlushChanges()
        {
            dimensionsControl.FlushChanges();
            weight.FlushChanges();
        }

        /// <summary>
        /// Raises the ShipSenseFieldChanged event
        /// </summary>
        private void RaiseShipSenseFieldChanged()
        {
            if (suspendShipSenseFieldChangedEvent > 0)
            {
                return;
            }

            if (ShipSenseFieldChanged != null)
            {
                ShipSenseFieldChanged(this, EventArgs.Empty);
            }
        }
    }
}
