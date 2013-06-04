﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI.Utility;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// UserControl for editing ups packages for a shipment
    /// </summary>
    public partial class UpsPackageControl : UserControl
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

        /// <summary>
        /// The number of packages for the shipment has changed
        /// </summary>
        public event EventHandler PackageCountChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// One-time control initialization
        /// </summary>
        public void Initialize(ShipmentTypeCode shipmentTypeCode)
        {
            dimensionsControl.Initialize();

            List<KeyValuePair<string, int>> packageCountData = new List<KeyValuePair<string, int>>();

            for (int i = 1; i <= 25; i++)
            {
                packageCountData.Add(new KeyValuePair<string, int>(i.ToString(), i));
            }

            packagingType.DisplayMember = "Key";
            packagingType.ValueMember = "Value";

            packageCountCombo.Items.Clear();
            packageCountCombo.DisplayMember = "Key";
            packageCountCombo.ValueMember = "Value";
            packageCountCombo.DataSource = packageCountData;

            packageCountCombo.SelectedIndexChanged += this.OnChangePackageCount;

            // Get valid packaging types
            List<KeyValuePair<string, UpsPackagingType>> packaging = UpsUtility.GetValidPackagingTypes(shipmentTypeCode).Select(type => new KeyValuePair<string, UpsPackagingType>(EnumHelper.GetDescription(type), type)).ToList();
            if (shipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                // Worldship could possibly have some "invalid" values that need to be allowed in the combobox. This can happen when
                // the user enables Mail Innovations, sets a package type to a Mail Innovations-only one, then disables Mail Innovations.
                // Using a custom Binding Source to achieve this.
                List<KeyValuePair<string, UpsPackagingType>> specialPackages = UpsUtility.GetWorldShipConditionalPackagingType(null).Select(type => new KeyValuePair<string, UpsPackagingType>(EnumHelper.GetDescription(type), type)).ToList();

                // binding to a binding source that allows missing enum values in the UI
                EnumerationSubsetBindingSource bindingSource = EnumerationSubsetBindingSource.Create<UpsPackagingType>(packaging, specialPackages);
                packagingType.DataSource = bindingSource;
            }
            else
            {
                packagingType.DataSource = packaging;
            }
        }

        /// <summary>
        /// Load the shipments into the packaging control
        /// </summary>
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

            packageCountCombo.SelectedIndexChanged -= this.OnChangePackageCount;
            packagesGrid.SelectionChanged -= this.OnChangeSelectedPackages;

            // Clear previous rows
            packagesGrid.Rows.Clear();
            selectedRows.Clear();

            List<List<UpsPackageEntity>> packageBuckets = new List<List<UpsPackageEntity>>();

            // Load the shipment data
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    packageCountCombo.ApplyMultiValue(shipment.Ups.Packages.Count);

                    int index = 0;

                    // Sort the packages into the buckets.  Each bucket will correspond to a grid row
                    foreach (UpsPackageEntity package in shipment.Ups.Packages)
                    {
                        // We don't have a bucket for this index yet
                        if (packageBuckets.Count == index)
                        {
                            packageBuckets.Add(new List<UpsPackageEntity>());
                        }

                        // Add the package to the bucket
                        packageBuckets[index].Add(package);

                        // Next bucket next time
                        index++;
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

            List<UpsPackageEntity> packages = (List<UpsPackageEntity>) gridRow.Tag;
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
                UpsShipmentEntity ups = shipment.Ups;

                // Need more
                while (ups.Packages.Count < count)
                {
                    UpsPackageEntity package = UpsUtility.CreateDefaultPackage();
                    ups.Packages.Add(package);
                }

                // Need less
                while (ups.Packages.Count > count)
                {
                    UpsPackageEntity package = ups.Packages[ups.Packages.Count - 1];
                    ups.Packages.Remove(package);
                }
            }

            // Have to reload the UI
            LoadShipments(loadedShipments, packageCountCombo.Enabled);

            // Raise the rate critiera changed event
            RaiseRateCriteriaChanged();

            // Raise the package count changed event
            if (PackageCountChanged != null)
            {
                PackageCountChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Changing the selected package(s)
        /// </summary>
        private void OnChangeSelectedPackages(object sender, SelectionChangedEventArgs e)
        {
            suspendRateCriteriaEvent++;

            // Save the existing stuff before loading the new stuff
            SaveToEntities();
            selectedRows.Clear();

            List<DimensionsAdapter> dimensionsToLoad = new List<DimensionsAdapter>();
            List<InsuranceChoice> insuranceToLoad = new List<InsuranceChoice>();

            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools);

            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;

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
                        packagingType.ApplyMultiValue((UpsPackagingType) package.PackagingType);
                        weight.ApplyMultiWeight(package.Weight);

                        insuranceToLoad.Add(shipmentType.GetParcelInsuranceChoice(package.UpsShipment.Shipment, package.UpsShipment.Packages.IndexOf(package)));
                        dimensionsToLoad.Add(new DimensionsAdapter(package));
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
        }

        /// <summary>
        /// Save the current values to the selected entities
        /// </summary>
        public void SaveToEntities()
        {
            // Go through each selected row
            foreach (GridRow gridRow in selectedRows)
            {
                List<UpsPackageEntity> packages = (List<UpsPackageEntity>) gridRow.Tag;

                // Load the data from each selected package
                foreach (UpsPackageEntity package in packages)
                {
                    packagingType.ReadMultiValue(v => package.PackagingType = (int) v);
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
    }
}
