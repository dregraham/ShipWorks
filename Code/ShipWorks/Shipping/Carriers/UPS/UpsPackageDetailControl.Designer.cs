namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsPackageDetailControl
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
            this.panelPackageSelector = new System.Windows.Forms.Panel();
            this.labelPackages = new System.Windows.Forms.Label();
            this.packagesGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumn = new Divelements.SandGrid.GridColumn();
            this.additionalHandling = new System.Windows.Forms.CheckBox();
            this.labelAdditionalHandling = new System.Windows.Forms.Label();
            this.packageDetailPanel = new System.Windows.Forms.Panel();
            this.verbalConfirmation = new ShipWorks.Shipping.Carriers.UPS.UpsContactInfoControl();
            this.dryIceDetails = new ShipWorks.Shipping.Carriers.UPS.UpsDryIceControl();
            this.panelPackageSelector.SuspendLayout();
            this.packageDetailPanel.SuspendLayout();
            this.SuspendLayout();
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
            this.panelPackageSelector.TabIndex = 7;
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
            // 
            // gridColumn
            // 
            this.gridColumn.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumn.Clickable = false;
            this.gridColumn.HeaderText = "Packages (Select to Edit)";
            this.gridColumn.Width = 203;
            // 
            // additionalHandling
            // 
            this.additionalHandling.AutoSize = true;
            this.additionalHandling.Location = new System.Drawing.Point(118, 2);
            this.additionalHandling.Name = "additionalHandling";
            this.additionalHandling.Size = new System.Drawing.Size(222, 17);
            this.additionalHandling.TabIndex = 0;
            this.additionalHandling.Text = "This package requires additional handling";
            this.additionalHandling.UseVisualStyleBackColor = true;
            this.additionalHandling.CheckStateChanged += new System.EventHandler(this.OnPackageDetailsChanged);
            // 
            // labelAdditionalHandling
            // 
            this.labelAdditionalHandling.AutoSize = true;
            this.labelAdditionalHandling.BackColor = System.Drawing.Color.Transparent;
            this.labelAdditionalHandling.Location = new System.Drawing.Point(11, 3);
            this.labelAdditionalHandling.Name = "labelAdditionalHandling";
            this.labelAdditionalHandling.Size = new System.Drawing.Size(101, 13);
            this.labelAdditionalHandling.TabIndex = 23;
            this.labelAdditionalHandling.Text = "Additional Handling:";
            // 
            // packageDetailPanel
            // 
            this.packageDetailPanel.Controls.Add(this.verbalConfirmation);
            this.packageDetailPanel.Controls.Add(this.dryIceDetails);
            this.packageDetailPanel.Controls.Add(this.additionalHandling);
            this.packageDetailPanel.Controls.Add(this.labelAdditionalHandling);
            this.packageDetailPanel.Location = new System.Drawing.Point(3, 69);
            this.packageDetailPanel.Margin = new System.Windows.Forms.Padding(0);
            this.packageDetailPanel.Name = "packageDetailPanel";
            this.packageDetailPanel.Size = new System.Drawing.Size(419, 185);
            this.packageDetailPanel.TabIndex = 8;
            // 
            // verbalConfirmation
            // 
            this.verbalConfirmation.ContactName = "";
            this.verbalConfirmation.Location = new System.Drawing.Point(8, 106);
            this.verbalConfirmation.Name = "verbalConfirmation";
            this.verbalConfirmation.PhoneExtension = "";
            this.verbalConfirmation.PhoneNumber = "";
            this.verbalConfirmation.Size = new System.Drawing.Size(377, 80);
            this.verbalConfirmation.State = false;
            this.verbalConfirmation.TabIndex = 2;
            this.verbalConfirmation.ContactInfoChanged += new System.EventHandler(this.OnPackageDetailsChanged);
            // 
            // dryIceDetails
            // 
            this.dryIceDetails.Location = new System.Drawing.Point(65, 25);
            this.dryIceDetails.Name = "dryIceDetails";
            this.dryIceDetails.Size = new System.Drawing.Size(351, 75);
            this.dryIceDetails.State = false;
            this.dryIceDetails.TabIndex = 1;
            this.dryIceDetails.RateCriteriaChanged += new System.EventHandler(this.OnPackageDetailsChanged);
            // 
            // UpsPackageDetailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.packageDetailPanel);
            this.Controls.Add(this.panelPackageSelector);
            this.Name = "UpsPackageDetailControl";
            this.Size = new System.Drawing.Size(423, 254);
            this.panelPackageSelector.ResumeLayout(false);
            this.panelPackageSelector.PerformLayout();
            this.packageDetailPanel.ResumeLayout(false);
            this.packageDetailPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelPackageSelector;
        private System.Windows.Forms.Label labelPackages;
        private Divelements.SandGrid.SandGrid packagesGrid;
        private Divelements.SandGrid.GridColumn gridColumn;
        private UpsDryIceControl dryIceDetails;
        private System.Windows.Forms.CheckBox additionalHandling;
        private System.Windows.Forms.Label labelAdditionalHandling;
        private System.Windows.Forms.Panel packageDetailPanel;
        private UpsContactInfoControl verbalConfirmation;
    }
}
