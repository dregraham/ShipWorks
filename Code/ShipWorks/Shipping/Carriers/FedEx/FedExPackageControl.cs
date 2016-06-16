using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// User control for editing fedex packages
    /// </summary>
    public partial class FedExPackageControl : UserControl
    {
        List<ShipmentEntity> loadedShipments;

        // Keeps track of the selected rows, so when the selection changes, we know what to save
        List<GridRow> selectedRows = new List<GridRow>();

        // Indiciates if the event shouldnt currently be raised
        int suspendRateCriteriaEvent = 0;

        /// <summary>
        /// Some part of the packaging has changed the rate criteria
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        // Indiciates if the event shouldnt currently be raised
        int suspendShipSenseFieldEvent = 0;

        /// <summary>
        /// Some part of the packaging has changed the ShipSense criteria
        /// </summary>
        public event EventHandler ShipSenseFieldChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExPackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The number of packages for the shipment has changed
        /// </summary>
        public Action<int> PackageCountChanged
        {
            get;
            set;
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
            suspendShipSenseFieldEvent++;

            packageCountCombo.SelectedIndexChanged -= this.OnChangePackageCount;
            packagesGrid.SelectionChanged -= this.OnChangeSelectedPackages;

            // Clear previous rows
            packagesGrid.Rows.Clear();
            selectedRows.Clear();

            List<List<FedExPackageEntity>> packageBuckets = new List<List<FedExPackageEntity>>();

            // Load the shipment data
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    packageCountCombo.ApplyMultiValue(shipment.FedEx.Packages.Count);

                    int index = 0;

                    // Sort the packages into the buckets.  Each bucket will correspond to a grid row
                    foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                    {
                        // We don't have a bucket for this index yet
                        if (packageBuckets.Count == index)
                        {
                            packageBuckets.Add(new List<FedExPackageEntity>());
                        }

                        // Add the package to the bucket
                        packageBuckets[index].Add(package);

                        // Next bucket next time
                        index++;
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

            // Start listening again
            packageCountCombo.SelectedIndexChanged += this.OnChangePackageCount;
            packagesGrid.SelectionChanged += this.OnChangeSelectedPackages;

            // The only way there wouldnt be any packages is if there was no selection
            if (packagesGrid.Rows.Count > 0)
            {
                packagesGrid.Rows[0].Selected = true;
            }

            UpdateLayout();

            suspendRateCriteriaEvent--;
            suspendShipSenseFieldEvent--;
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
        /// Update the display t ext of the given GridRow, which is dependant on how many packages it has
        /// </summary>
        private void UpdateRowText(GridRow gridRow)
        {
            string text = string.Format("Package {0}", gridRow.Index + 1);

            List<FedExPackageEntity> packages = (List<FedExPackageEntity>) gridRow.Tag;
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
                FedExShipmentEntity fedex = shipment.FedEx;

                // Need more
                while (fedex.Packages.Count < count)
                {
                    FedExPackageEntity package = FedExUtility.CreateDefaultPackage();
                    fedex.Packages.Add(package);
                }

                // Need less
                while (fedex.Packages.Count > count)
                {
                    FedExPackageEntity package = fedex.Packages[fedex.Packages.Count - 1];
                    fedex.Packages.Remove(package);
                }
            }

            // Have to reload the UI
            LoadShipments(loadedShipments, packageCountCombo.Enabled);

            // Raise the rate critiera changed event
            RaiseRateCriteriaChanged();

            // Raise the ShipSense changed event
            RaiseShipSenseFieldChanged();

            // Raise the package count changed event
            if (PackageCountChanged != null)
            {
                PackageCountChanged(count);
            }
        }

        /// <summary>
        /// Changing the selected package(s)
        /// </summary>
        private void OnChangeSelectedPackages(object sender, SelectionChangedEventArgs e)
        {
            suspendRateCriteriaEvent++;
            suspendShipSenseFieldEvent++;

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

                    List<FedExPackageEntity> packages = (List<FedExPackageEntity>) gridRow.Tag;

                    // Load the data from each selected package
                    foreach (FedExPackageEntity package in packages)
                    {
                        weight.ApplyMultiWeight(package.Weight);

                        dimensionsToLoad.Add(new DimensionsAdapter(package));
                    }
                }
            }

            // Load the dimensions
            dimensionsControl.LoadDimensions(dimensionsToLoad);

            // Start the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;

            // Load the insurance
            UpdateInsuranceDisplay();

            suspendRateCriteriaEvent--;
            suspendShipSenseFieldEvent--;
        }

        /// <summary>
        /// Save the current values to the selected entities
        /// </summary>
        public void SaveToEntities()
        {
            // Go through each selected row
            foreach (GridRow gridRow in selectedRows)
            {
                List<FedExPackageEntity> packages = (List<FedExPackageEntity>) gridRow.Tag;

                // Load the data from each selected package
                foreach (FedExPackageEntity package in packages)
                {
                    weight.ReadMultiWeight(w => package.Weight = w);
                }
            }

            // Save dimensions
            dimensionsControl.SaveToEntities();

            // Save insurance
            insuranceControl.SaveToInsuranceChoices();
        }

        /// <summary>
        /// Update the insurance display information
        /// </summary>
        public void UpdateInsuranceDisplay()
        {
            suspendRateCriteriaEvent++;

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx);

            IEnumerable<IInsuranceChoice> insuranceToLoad = packagesGrid.SelectedElements.OfType<GridRow>()
                .Select(x => x.Tag).OfType<List<FedExPackageEntity>>()
                .SelectMany(x => x).Select(x => shipmentType.GetParcelDetail(x.FedExShipment.Shipment, x.FedExShipment.Packages.IndexOf(x)).Insurance);

            // Load the insurance
            insuranceControl.LoadInsuranceChoices(insuranceToLoad);

            suspendRateCriteriaEvent--;
        }

        /// <summary>
        /// Something affecting rate critiera has changed
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

            if (ShipSenseFieldChanged != null)
            {
                ShipSenseFieldChanged(this, EventArgs.Empty);
            }
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
    }
}
