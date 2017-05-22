namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsPackageControl
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
            this.panelPackage = new System.Windows.Forms.Panel();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.packagingType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.packagesGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumn = new Divelements.SandGrid.GridColumn();
            this.labelPackages = new System.Windows.Forms.Label();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.packageCountCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.panelPackage.SuspendLayout();
            this.SuspendLayout();
            //
            // panelPackage
            //
            this.panelPackage.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPackage.Controls.Add(this.insuranceControl);
            this.panelPackage.Controls.Add(this.packagingType);
            this.panelPackage.Controls.Add(this.labelPackaging);
            this.panelPackage.Controls.Add(this.weight);
            this.panelPackage.Controls.Add(this.labelWeight);
            this.panelPackage.Controls.Add(this.labelDimensions);
            this.panelPackage.Controls.Add(this.dimensionsControl);
            this.panelPackage.Location = new System.Drawing.Point(6, 88);
            this.panelPackage.Name = "panelPackage";
            this.panelPackage.Size = new System.Drawing.Size(357, 185);
            this.panelPackage.TabIndex = 3;
            //
            // insuranceControl
            //
            this.insuranceControl.BackColor = System.Drawing.Color.White;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(21, 137);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(416, 46);
            this.insuranceControl.TabIndex = 6;
            this.insuranceControl.InsuranceOptionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // packagingType
            //
            this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagingType.FormattingEnabled = true;
            this.packagingType.Location = new System.Drawing.Point(90, 5);
            this.packagingType.Name = "packagingType";
            this.packagingType.PromptText = "(Multiple Values)";
            this.packagingType.Size = new System.Drawing.Size(144, 21);
            this.packagingType.TabIndex = 1;
            this.packagingType.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // labelPackaging
            //
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(26, 8);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 0;
            this.labelPackaging.Text = "Packaging:";
            //
            // weight
            //
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(90, 33);
            this.weight.Name = "weight";
            this.weight.RangeMax = 1000D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(269, 21);
            this.weight.TabIndex = 3;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.weight.WeightChanged += OnShipSenseFieldChanged;
            this.weight.ShowShortcutInfo = true;
            //
            // labelWeight
            //
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(39, 36);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 2;
            this.labelWeight.Text = "Weight:";
            //
            // labelDimensions
            //
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(20, 66);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 4;
            this.labelDimensions.Text = "Dimensions:";
            //
            // dimensionsControl
            //
            this.dimensionsControl.BackColor = System.Drawing.Color.White;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dimensionsControl.Location = new System.Drawing.Point(87, 60);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 5;
            this.dimensionsControl.DimensionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.dimensionsControl.DimensionsChanged += OnShipSenseFieldChanged;
            //
            // packagesGrid
            //
            this.packagesGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.packagesGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn});
            this.packagesGrid.Location = new System.Drawing.Point(96, 27);
            this.packagesGrid.Name = "packagesGrid";
            this.packagesGrid.Renderer = windowsXPRenderer1;
            this.packagesGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Package 1")}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Package 2")})});
            this.packagesGrid.Size = new System.Drawing.Size(207, 60);
            this.packagesGrid.TabIndex = 2;
            this.packagesGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
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
            this.labelPackages.Location = new System.Drawing.Point(34, 7);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(56, 13);
            this.labelPackages.TabIndex = 0;
            this.labelPackages.Text = "Packages:";
            //
            // kryptonBorderEdge
            //
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(5, 13);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(26, 1);
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            this.kryptonBorderEdge.Visible = false;
            //
            // kryptonBorderEdge1
            //
            this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(5, 273);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(26, 1);
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Visible = false;
            //
            // kryptonBorderEdge3
            //
            this.kryptonBorderEdge3.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge3.AutoSize = false;
            this.kryptonBorderEdge3.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge3.Location = new System.Drawing.Point(5, 13);
            this.kryptonBorderEdge3.Name = "kryptonBorderEdge3";
            this.kryptonBorderEdge3.Size = new System.Drawing.Size(1, 260);
            this.kryptonBorderEdge3.Text = "kryptonBorderEdge1";
            this.kryptonBorderEdge3.Visible = false;
            //
            // packageCountCombo
            //
            this.packageCountCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packageCountCombo.FormattingEnabled = true;
            this.packageCountCombo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.packageCountCombo.Location = new System.Drawing.Point(96, 3);
            this.packageCountCombo.Name = "packageCountCombo";
            this.packageCountCombo.PromptText = "(Multiple Values)";
            this.packageCountCombo.Size = new System.Drawing.Size(106, 21);
            this.packageCountCombo.TabIndex = 1;
            //
            // UpsPackageControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.kryptonBorderEdge3);
            this.Controls.Add(this.kryptonBorderEdge1);
            this.Controls.Add(this.kryptonBorderEdge);
            this.Controls.Add(this.panelPackage);
            this.Controls.Add(this.packagesGrid);
            this.Controls.Add(this.packageCountCombo);
            this.Controls.Add(this.labelPackages);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "UpsPackageControl";
            this.Size = new System.Drawing.Size(363, 278);
            this.panelPackage.ResumeLayout(false);
            this.panelPackage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelPackage;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelDimensions;
        private ShipWorks.Shipping.Editing.DimensionsControl dimensionsControl;
        private Divelements.SandGrid.SandGrid packagesGrid;
        private Divelements.SandGrid.GridColumn gridColumn;
        private ShipWorks.UI.Controls.MultiValueComboBox packageCountCombo;
        private System.Windows.Forms.Label labelPackages;
        private ShipWorks.UI.Controls.MultiValueComboBox packagingType;
        private System.Windows.Forms.Label labelPackaging;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        private Insurance.InsuranceSelectionControl insuranceControl;
    }
}
