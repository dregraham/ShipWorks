namespace ShipWorks.Shipping.Carriers.iParcel
{
    partial class iParcelPackageControl
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer2 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.labelPackages = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.packagesGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumn = new Divelements.SandGrid.GridColumn();
            this.panelPackage = new System.Windows.Forms.Panel();
            this.skuAndQuantity = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelSkuAndQuantity = new System.Windows.Forms.Label();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.packageCountCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.panelPackage.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(36, 38);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 3;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // labelPackages
            // 
            this.labelPackages.AutoSize = true;
            this.labelPackages.BackColor = System.Drawing.Color.Transparent;
            this.labelPackages.Location = new System.Drawing.Point(46, 7);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(56, 13);
            this.labelPackages.TabIndex = 0;
            this.labelPackages.Text = "Packages:";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(55, 9);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 0;
            this.labelWeight.Text = "Weight:";
            // 
            // packagesGrid
            // 
            this.packagesGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.packagesGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn});
            this.packagesGrid.Location = new System.Drawing.Point(108, 27);
            this.packagesGrid.Name = "packagesGrid";
            this.packagesGrid.Renderer = windowsXPRenderer2;
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
            // panelPackage
            // 
            this.panelPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPackage.BackColor = System.Drawing.Color.White;
            this.panelPackage.Controls.Add(this.skuAndQuantity);
            this.panelPackage.Controls.Add(this.labelSkuAndQuantity);
            this.panelPackage.Controls.Add(this.insuranceControl);
            this.panelPackage.Controls.Add(this.weight);
            this.panelPackage.Controls.Add(this.labelWeight);
            this.panelPackage.Controls.Add(this.labelDimensions);
            this.panelPackage.Controls.Add(this.dimensionsControl);
            this.panelPackage.Location = new System.Drawing.Point(0, 88);
            this.panelPackage.Name = "panelPackage";
            this.panelPackage.Size = new System.Drawing.Size(591, 212);
            this.panelPackage.TabIndex = 3;
            // 
            // skuAndQuantity
            // 
            this.skuAndQuantity.Location = new System.Drawing.Point(106, 170);
            this.skuAndQuantity.MaxLength = 32767;
            this.skuAndQuantity.Name = "skuAndQuantity";
            this.skuAndQuantity.Size = new System.Drawing.Size(222, 21);
            this.skuAndQuantity.TabIndex = 78;
            this.skuAndQuantity.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            this.skuAndQuantity.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.ShippingReference;
            this.skuAndQuantity.TextChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // labelSkuAndQuantity
            // 
            this.labelSkuAndQuantity.AutoSize = true;
            this.labelSkuAndQuantity.BackColor = System.Drawing.Color.Transparent;
            this.labelSkuAndQuantity.Location = new System.Drawing.Point(1, 174);
            this.labelSkuAndQuantity.Name = "labelSkuAndQuantity";
            this.labelSkuAndQuantity.Size = new System.Drawing.Size(96, 13);
            this.labelSkuAndQuantity.TabIndex = 79;
            this.labelSkuAndQuantity.Text = "SKU and Quantity:";
            // 
            // insuranceControl
            // 
            this.insuranceControl.BackColor = System.Drawing.Color.White;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(38, 111);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(416, 46);
            this.insuranceControl.TabIndex = 8;
            this.insuranceControl.InsuranceOptionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // weight
            // 
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.ConfigureTelemetryEntityCounts = null;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(106, 6);
            this.weight.Name = "weight";
            this.weight.RangeMax = 2000D;
            this.weight.RangeMin = 0D;
            this.weight.ShowShortcutInfo = true;
            this.weight.Size = new System.Drawing.Size(269, 24);
            this.weight.TabIndex = 2;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.White;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(103, 32);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 4;
            this.dimensionsControl.DimensionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
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
            this.packageCountCombo.Location = new System.Drawing.Point(108, 3);
            this.packageCountCombo.Name = "packageCountCombo";
            this.packageCountCombo.PromptText = "(Multiple Values)";
            this.packageCountCombo.Size = new System.Drawing.Size(106, 21);
            this.packageCountCombo.TabIndex = 1;
            // 
            // iParcelPackageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelPackage);
            this.Controls.Add(this.packagesGrid);
            this.Controls.Add(this.packageCountCombo);
            this.Controls.Add(this.labelPackages);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "iParcelPackageControl";
            this.Size = new System.Drawing.Size(591, 299);
            this.panelPackage.ResumeLayout(false);
            this.panelPackage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Shipping.Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label labelDimensions;
        private ShipWorks.UI.Controls.MultiValueComboBox packageCountCombo;
        private System.Windows.Forms.Label labelPackages;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private Divelements.SandGrid.SandGrid packagesGrid;
        private Divelements.SandGrid.GridColumn gridColumn;
        private System.Windows.Forms.Panel panelPackage;
        private Insurance.InsuranceSelectionControl insuranceControl;
        private Templates.Tokens.TemplateTokenTextBox skuAndQuantity;
        private System.Windows.Forms.Label labelSkuAndQuantity;
    }
}
