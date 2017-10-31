using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExPackageFreightDetailControl
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
            this.packagesGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumn = new Divelements.SandGrid.GridColumn();
            this.labelPackages = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelPackageSelector = new System.Windows.Forms.Panel();
            this.panelPackageDetails = new System.Windows.Forms.Panel();
            this.freightPieces = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelFreightPieces = new System.Windows.Forms.Label();
            this.freightPackaging = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelFreightPackaging = new System.Windows.Forms.Label();
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
            this.packagesGrid.Renderer = windowsXPRenderer2;
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
            this.mainPanel.Size = new System.Drawing.Size(331, 125);
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
            this.panelPackageDetails.Controls.Add(this.freightPackaging);
            this.panelPackageDetails.Controls.Add(this.labelFreightPackaging);
            this.panelPackageDetails.Controls.Add(this.freightPieces);
            this.panelPackageDetails.Controls.Add(this.labelFreightPieces);
            this.panelPackageDetails.Location = new System.Drawing.Point(0, 66);
            this.panelPackageDetails.Margin = new System.Windows.Forms.Padding(0);
            this.panelPackageDetails.Name = "panelPackageDetails";
            this.panelPackageDetails.Size = new System.Drawing.Size(230, 59);
            this.panelPackageDetails.TabIndex = 7;
            // 
            // freightPieces
            // 
            this.freightPieces.Location = new System.Drawing.Point(121, 36);
            this.freightPieces.Name = "freightPieces";
            this.freightPieces.Size = new System.Drawing.Size(100, 20);
            this.freightPieces.TabIndex = 24;
            // 
            // labelFreightPieces
            // 
            this.labelFreightPieces.AutoSize = true;
            this.labelFreightPieces.BackColor = System.Drawing.Color.Transparent;
            this.labelFreightPieces.Location = new System.Drawing.Point(75, 39);
            this.labelFreightPieces.Name = "labelFreightPieces";
            this.labelFreightPieces.Size = new System.Drawing.Size(42, 13);
            this.labelFreightPieces.TabIndex = 23;
            this.labelFreightPieces.Text = "Pieces:";
            // 
            // freightPackaging
            // 
            this.freightPackaging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.freightPackaging.FormattingEnabled = true;
            this.freightPackaging.Location = new System.Drawing.Point(121, 6);
            this.freightPackaging.Name = "freightPackaging";
            this.freightPackaging.PromptText = "(Multiple Values)";
            this.freightPackaging.Size = new System.Drawing.Size(106, 21);
            this.freightPackaging.TabIndex = 26;
            // 
            // labelFreightPackaging
            // 
            this.labelFreightPackaging.AutoSize = true;
            this.labelFreightPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelFreightPackaging.Location = new System.Drawing.Point(56, 9);
            this.labelFreightPackaging.Name = "labelFreightPackaging";
            this.labelFreightPackaging.Size = new System.Drawing.Size(61, 13);
            this.labelFreightPackaging.TabIndex = 25;
            this.labelFreightPackaging.Text = "Packaging:";
            // 
            // FedExPackageFreightDetailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.mainPanel);
            this.Name = "FedExPackageFreightDetailControl";
            this.Size = new System.Drawing.Size(334, 128);
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
        private UI.Controls.MultiValueTextBox freightPieces;
        private Label labelFreightPieces;
        private UI.Controls.MultiValueComboBox freightPackaging;
        private Label labelFreightPackaging;
    }
}
