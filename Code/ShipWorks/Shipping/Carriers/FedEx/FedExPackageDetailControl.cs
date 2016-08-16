using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public partial class FedExPackageDetailControl : UserControl
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

        public FedExPackageDetailControl()
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

            // The only way there wouldnt be any packages is if there was no selection
            if (packagesGrid.Rows.Count > 0)
            {
                packagesGrid.Rows[0].Selected = true;
            }

            UpdateLayout();
            UpdateFreightUI();

            loading = false;
        }

        /// <summary>
        /// Update the display t ext of the given GridRow, which is dependant on how many packages it has
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
        /// Update the dispaly of freight information based on the loaded shipment's service types
        /// </summary>
        private void UpdateFreightUI()
        {
            bool allFreight = loadedShipments.Count > 0;

            foreach (ShipmentEntity shipment in loadedShipments)
            {
                if (!FedExUtility.IsFreightService((FedExServiceType)shipment.FedEx.Service))
                {
                    allFreight = false;
                    break;
                }
            }

            UpdateFreightUI(allFreight);
        }

        /// <summary>
        /// Update the freight ui to show as freight service selection or not
        /// </summary>
        public void UpdateFreightUI(bool showAsFreight)
        {
            if (showAsFreight)
            {
                labelPackages.Text = "      Skids:";
                gridColumn.HeaderText = "Skids (Select to Edit)";

                labelSkidPieces.Visible = true;
                skidPieces.Visible = true;

                labelDangerousGoods.Location = new Point(labelDangerousGoods.Location.X,
                    labelSkidPieces.Bottom + 11);
                dangerousGoodsControl.Location = new Point(dangerousGoodsControl.Location.X,
                    skidPieces.Bottom + 6);

            }
            else
            {
                labelPackages.Text = "Packages:";
                gridColumn.HeaderText = "Packages (Select to Edit)";

                labelSkidPieces.Visible = false;
                skidPieces.Visible = false;

                labelDangerousGoods.Location = new Point(labelDangerousGoods.Location.X, 
                    labelSkidPieces.Location.Y);
                dangerousGoodsControl.Location = new Point(dangerousGoodsControl.Location.X, 
                    skidPieces.Location.Y + 3);
            }

            UpdateLayout();
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
                        skidPieces.ApplyMultiText(package.SkidPieces.ToString());
                        dryIceWeight.ApplyMultiWeight(package.DryIceWeight);

                        containsAlcohol.ApplyMultiCheck(package.ContainsAlcohol);

                        // Load the priority alerts and dangerous goods
                        priorityAlertControl.LoadPriorityAlertData(package);
                        dangerousGoodsControl.LoadDangerousGoodsData(package);
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
                    skidPieces.ReadMultiText(t =>
                    {
                        int pieces;
                        if (int.TryParse(skidPieces.Text, out pieces))
                        {
                            package.SkidPieces = pieces;
                        }
                    });

                    dryIceWeight.ReadMultiWeight(w => package.DryIceWeight = w);

                    containsAlcohol.ReadMultiCheck(w => package.ContainsAlcohol = w);
                }

                // Save the priority alerts and dangerous goods
                priorityAlertControl.SavePriorityAlertToPackage(packages);
                dangerousGoodsControl.SaveToPackage(packages);
            }
        }

        public void PackageCountChanged(int packageCount)
        {
            SaveToEntities();

            // package count wouldn't change if control weren't editable.
            LoadShipments(loadedShipments,true);
        }

        /// <summary>
        /// Indicates that the user has changed values 
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
