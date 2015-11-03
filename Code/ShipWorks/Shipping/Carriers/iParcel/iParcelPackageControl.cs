using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.UI.Controls;
using Divelements.SandGrid;
using Interapptive.Shared;
using ShipWorks.Data.Model;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// User control for editing i-Parcel packages
    /// </summary>
    public partial class iParcelPackageControl : UserControl
    {
        List<ShipmentEntity> loadedShipments;

        // Keeps track of the selected rows, so when the selection changes, we know what to save
        List<GridRow> selectedRows = new List<GridRow>();

        // Indiciates if the event shouldn't currently be raised
        int suspendRateCriteriaEvent = 0;

        // Indiciates if the event shouldn't currently be raised
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
        public iParcelPackageControl()
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

            List<List<IParcelPackageEntity>> packageBuckets = new List<List<IParcelPackageEntity>>();

            // Load the shipment data
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    packageCountCombo.ApplyMultiValue(shipment.IParcel.Packages.Count);

                    int index = 0;

                    // Sort the packages into the buckets.  Each bucket will correspond to a grid row
                    foreach (IParcelPackageEntity package in shipment.IParcel.Packages)
                    {
                        // We don't have a bucket for this index yet
                        if (packageBuckets.Count == index)
                        {
                            packageBuckets.Add(new List<IParcelPackageEntity>());
                        }

                        // Add the package to the bucket
                        packageBuckets[index].Add(package);

                        // Next bucket next time
                        index++;
                    }
                }
            }

            // Add in each bucket as a row in the grid
            foreach (List<IParcelPackageEntity> bucket in packageBuckets)
            {
                GridRow gridRow = new GridRow("");
                gridRow.Tag = bucket;

                packagesGrid.Rows.Add(gridRow);

                UpdateRowText(gridRow);
            }

            // Use the i-parcel specific factory for populating the suggested tokens
            skuAndQuantity.TokenSuggestionFactory = new iParcelTokenSuggestionFactory(shipments);
            
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
        /// Update the display text of the given GridRow, which is dependant on how many packages it has
        /// </summary>
        private void UpdateRowText(GridRow gridRow)
        {
            string text = string.Format("Package {0}", gridRow.Index + 1);

            List<IParcelPackageEntity> packages = (List<IParcelPackageEntity>) gridRow.Tag;
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
                IParcelShipmentEntity iParcelShipmentEntity = shipment.IParcel;

                // Need more
                while (iParcelShipmentEntity.Packages.Count < count)
                {
                    IParcelPackageEntity package = iParcelShipmentType.CreateDefaultPackage();
                    iParcelShipmentEntity.Packages.Add(package);
                }

                // Need less
                while (iParcelShipmentEntity.Packages.Count > count)
                {
                    IParcelPackageEntity package = iParcelShipmentEntity.Packages[iParcelShipmentEntity.Packages.Count - 1];
                    iParcelShipmentEntity.Packages.Remove(package);
                }
            }

            // Have to reload the UI
            LoadShipments(loadedShipments, packageCountCombo.Enabled);

            // Raise the rate critiera changed event
            RaiseRateCriteriaChanged();

            // Raise the ShipSense critiera changed event
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
            List<InsuranceChoice> insuranceToLoad = new List<InsuranceChoice>();

            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.iParcel);

            using (MultiValueScope scope = new MultiValueScope())
            {
                // Go through each selected row
                foreach (GridRow gridRow in packagesGrid.SelectedElements)
                {
                    selectedRows.Add(gridRow);

                    List<IParcelPackageEntity> packages = (List<IParcelPackageEntity>) gridRow.Tag;

                    // Load the data from each selected package
                    foreach (IParcelPackageEntity package in packages)
                    {
                        weight.ApplyMultiWeight(package.Weight);

                        insuranceToLoad.Add(shipmentType.GetParcelDetail(package.IParcelShipment.Shipment, package.IParcelShipment.Packages.IndexOf(package)).Insurance);
                        dimensionsToLoad.Add(new DimensionsAdapter(package));

                        skuAndQuantity.ApplyMultiText(package.SkuAndQuantities);
                    }
                }
            }

            // Load the insurance
            insuranceControl.LoadInsuranceChoices(insuranceToLoad);

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
                List<IParcelPackageEntity> packages = (List<IParcelPackageEntity>) gridRow.Tag;

                // Load the data from each selected package
                foreach (IParcelPackageEntity package in packages)
                {
                    weight.ReadMultiWeight(w => package.Weight = w);
                    skuAndQuantity.ReadMultiText(t => package.SkuAndQuantities = t);
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
            OnChangeSelectedPackages(null, null);
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
        /// Something affecting ShipSense critiera has changed
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
