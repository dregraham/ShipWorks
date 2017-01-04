using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExPackageDetailControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.packagesGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumn = new Divelements.SandGrid.GridColumn();
            this.labelPackages = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelPackageSelector = new System.Windows.Forms.Panel();
            this.panelPackageDetails = new System.Windows.Forms.Panel();
            this.labelDangerousGoods = new System.Windows.Forms.Label();
            this.containsAlcohol = new System.Windows.Forms.CheckBox();
            this.labelAlcohol = new System.Windows.Forms.Label();
            this.dryIceWeight = new ShipWorks.UI.Controls.WeightControl();
            this.dangerousGoodsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExDangerousGoodsControl();
            this.priorityAlertControl = new ShipWorks.Shipping.Carriers.FedEx.FedExPackagePriorityAlertsControl();
            this.labelDryIceWeight = new System.Windows.Forms.Label();
            this.skidPieces = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelSkidPieces = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.panelPackageSelector.SuspendLayout();
            this.panelPackageDetails.SuspendLayout();
            this.SuspendLayout();
            //
            // packagesGrid
            //
            this.packagesGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.packagesGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn});
            this.packagesGrid.Location = new System.Drawing.Point(121, 3);
            this.packagesGrid.Name = "packagesGrid";
            this.packagesGrid.Renderer = windowsXPRenderer1;
            this.packagesGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Package 1")}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Package 2")})});
            this.packagesGrid.Size = new System.Drawing.Size(207, 60);
            this.packagesGrid.TabIndex = 3;
            this.packagesGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.packagesGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnChangeSelectedPackages);
            //
            // gridColumn
            //
            this.gridColumn.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumn.Clickable = false;
            this.gridColumn.HeaderText = "Packages (Select to Edit)";
            this.gridColumn.Width = 203;
            //
            // labelPackages
            //
            this.labelPackages.AutoSize = true;
            this.labelPackages.BackColor = System.Drawing.Color.Transparent;
            this.labelPackages.Location = new System.Drawing.Point(59, 7);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(58, 13);
            this.labelPackages.TabIndex = 4;
            this.labelPackages.Text = "Packages:";
            //
            // mainPanel
            //
            this.mainPanel.AutoSize = true;
            this.mainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainPanel.Controls.Add(this.panelPackageSelector);
            this.mainPanel.Controls.Add(this.panelPackageDetails);
            this.mainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(343, 297);
            this.mainPanel.TabIndex = 5;
            //
            // panelPackageSelector
            //
            this.panelPackageSelector.AutoSize = true;
            this.panelPackageSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelPackageSelector.Controls.Add(this.labelPackages);
            this.panelPackageSelector.Controls.Add(this.packagesGrid);
            this.panelPackageSelector.Location = new System.Drawing.Point(0, 0);
            this.panelPackageSelector.Margin = new System.Windows.Forms.Padding(0);
            this.panelPackageSelector.Name = "panelPackageSelector";
            this.panelPackageSelector.Size = new System.Drawing.Size(331, 66);
            this.panelPackageSelector.TabIndex = 6;
            //
            // panelPackageDetails
            //
            this.panelPackageDetails.AutoSize = true;
            this.panelPackageDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelPackageDetails.BackColor = System.Drawing.Color.Transparent;
            this.panelPackageDetails.Controls.Add(this.labelDangerousGoods);
            this.panelPackageDetails.Controls.Add(this.containsAlcohol);
            this.panelPackageDetails.Controls.Add(this.labelAlcohol);
            this.panelPackageDetails.Controls.Add(this.dryIceWeight);
            this.panelPackageDetails.Controls.Add(this.dangerousGoodsControl);
            this.panelPackageDetails.Controls.Add(this.priorityAlertControl);
            this.panelPackageDetails.Controls.Add(this.labelDryIceWeight);
            this.panelPackageDetails.Controls.Add(this.skidPieces);
            this.panelPackageDetails.Controls.Add(this.labelSkidPieces);
            this.panelPackageDetails.Location = new System.Drawing.Point(0, 66);
            this.panelPackageDetails.Margin = new System.Windows.Forms.Padding(0);
            this.panelPackageDetails.Name = "panelPackageDetails";
            this.panelPackageDetails.Size = new System.Drawing.Size(343, 231);
            this.panelPackageDetails.TabIndex = 7;
            //
            // labelDangerousGoods
            //
            this.labelDangerousGoods.AutoSize = true;
            this.labelDangerousGoods.Location = new System.Drawing.Point(21, 212);
            this.labelDangerousGoods.Name = "labelDangerousGoods";
            this.labelDangerousGoods.Size = new System.Drawing.Size(96, 13);
            this.labelDangerousGoods.TabIndex = 19;
            this.labelDangerousGoods.Text = "Dangerous Goods:";
            this.labelDangerousGoods.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // containsAlcohol
            //
            this.containsAlcohol.AutoSize = true;
            this.containsAlcohol.Location = new System.Drawing.Point(121, 135);
            this.containsAlcohol.Name = "containsAlcohol";
            this.containsAlcohol.Size = new System.Drawing.Size(171, 17);
            this.containsAlcohol.TabIndex = 22;
            this.containsAlcohol.Text = "This package contains alcohol";
            this.containsAlcohol.UseVisualStyleBackColor = true;
            this.containsAlcohol.CheckStateChanged += new System.EventHandler(this.OnPackageDetailsChanged);
            //
            // labelAlcohol
            //
            this.labelAlcohol.AutoSize = true;
            this.labelAlcohol.BackColor = System.Drawing.Color.Transparent;
            this.labelAlcohol.Location = new System.Drawing.Point(72, 135);
            this.labelAlcohol.Name = "labelAlcohol";
            this.labelAlcohol.Size = new System.Drawing.Size(45, 13);
            this.labelAlcohol.TabIndex = 20;
            this.labelAlcohol.Text = "Alcohol:";
            //
            // dryIceWeight
            //
            this.dryIceWeight.BackColor = System.Drawing.Color.Transparent;
            this.dryIceWeight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dryIceWeight.Location = new System.Drawing.Point(121, 158);
            this.dryIceWeight.Name = "dryIceWeight";
            this.dryIceWeight.RangeMax = 2000D;
            this.dryIceWeight.RangeMin = 0D;
            this.dryIceWeight.Size = new System.Drawing.Size(218, 21);
            this.dryIceWeight.TabIndex = 18;
            this.dryIceWeight.Weight = 0D;
            this.dryIceWeight.WeightChanged += new System.EventHandler(this.OnPackageDetailsChanged);
            //
            // dangerousGoodsControl
            //
            this.dangerousGoodsControl.AutoSize = true;
            this.dangerousGoodsControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dangerousGoodsControl.BackColor = System.Drawing.Color.Transparent;
            this.dangerousGoodsControl.Location = new System.Drawing.Point(121, 211);
            this.dangerousGoodsControl.Name = "dangerousGoodsControl";
            this.dangerousGoodsControl.Size = new System.Drawing.Size(219, 17);
            this.dangerousGoodsControl.TabIndex = 21;
            this.dangerousGoodsControl.DangerousGoodsChanged += new EventHandler(OnDangerousGoodsChecked);
            //
            // priorityAlertControl
            //
            this.priorityAlertControl.Location = new System.Drawing.Point(7, 3);
            this.priorityAlertControl.Name = "priorityAlertControl";
            this.priorityAlertControl.Size = new System.Drawing.Size(332, 138);
            this.priorityAlertControl.TabIndex = 16;
            //
            // labelDryIceWeight
            //
            this.labelDryIceWeight.AutoSize = true;
            this.labelDryIceWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelDryIceWeight.Location = new System.Drawing.Point(36, 161);
            this.labelDryIceWeight.Name = "labelDryIceWeight";
            this.labelDryIceWeight.Size = new System.Drawing.Size(81, 13);
            this.labelDryIceWeight.TabIndex = 17;
            this.labelDryIceWeight.Text = "Dry Ice Weight:";
            //
            // skidPieces
            //
            this.skidPieces.Location = new System.Drawing.Point(121, 185);
            this.skidPieces.Name = "skidPieces";
            this.skidPieces.Size = new System.Drawing.Size(100, 20);
            this.skidPieces.TabIndex = 24;
            //
            // labelSkidPieces
            //
            this.labelSkidPieces.AutoSize = true;
            this.labelSkidPieces.BackColor = System.Drawing.Color.Transparent;
            this.labelSkidPieces.Location = new System.Drawing.Point(38, 188);
            this.labelSkidPieces.Name = "labelSkidPieces";
            this.labelSkidPieces.Size = new System.Drawing.Size(79, 13);
            this.labelSkidPieces.TabIndex = 23;
            this.labelSkidPieces.Text = "Pieces on skid:";
            //
            // FedExPackageDetailControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.mainPanel);
            this.Name = "FedExPackageDetailControl";
            this.Size = new System.Drawing.Size(346, 300);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.panelPackageSelector.ResumeLayout(false);
            this.panelPackageSelector.PerformLayout();
            this.panelPackageDetails.ResumeLayout(false);
            this.panelPackageDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Divelements.SandGrid.SandGrid packagesGrid;
        private Divelements.SandGrid.GridColumn gridColumn;
        private System.Windows.Forms.Label labelPackages;
        private System.Windows.Forms.FlowLayoutPanel mainPanel;
        private System.Windows.Forms.Panel panelPackageSelector;
        private Panel panelPackageDetails;
        private Label labelDangerousGoods;
        private CheckBox containsAlcohol;
        private Label labelAlcohol;
        private UI.Controls.WeightControl dryIceWeight;
        private FedExDangerousGoodsControl dangerousGoodsControl;
        private FedExPackagePriorityAlertsControl priorityAlertControl;
        private Label labelDryIceWeight;
        private UI.Controls.MultiValueTextBox skidPieces;
        private Label labelSkidPieces;
    }
}
