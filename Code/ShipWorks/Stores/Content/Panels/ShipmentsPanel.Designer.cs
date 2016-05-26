namespace ShipWorks.Stores.Content.Panels
{
    partial class ShipmentsPanel
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
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopyTracking = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInsuranceClaim = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ratesControl = new ShipWorks.Shipping.Editing.Rating.RatesPanel();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // addLink
            // 
            this.addLink.Location = new System.Drawing.Point(582, 42);
            this.addLink.Size = new System.Drawing.Size(73, 13);
            this.addLink.Text = "Add Shipment";
            this.addLink.Click += new System.EventHandler(this.OnAddShipment);
            // 
            // entityGrid
            // 
            this.entityGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.entityGrid.ContextMenuStrip = this.contextMenu;
            this.entityGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.entityGrid.EmptyText = "The order has no shipments.";
            this.entityGrid.EmptyTextForeColor = System.Drawing.SystemColors.GrayText;
            this.entityGrid.Size = new System.Drawing.Size(659, 37);
            this.entityGrid.RowLoadingComplete += new System.EventHandler(this.OnShipmentGridLoaded);
            this.entityGrid.GridCellLinkClicked += new ShipWorks.Data.Grid.GridHyperlinkClickEventHandler(this.OnGridCellLinkClicked);
            this.entityGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnShipmentSelectionChanged);
            // 
            // contextMenu
            // 
            this.contextMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuDelete,
            this.menuSep,
            this.menuTrack,
            this.menuCopyTracking,
            this.menuInsuranceClaim,
            this.menuSep2,
            this.menuCopy});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(199, 170);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuEdit
            // 
            this.menuEdit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(198, 22);
            this.menuEdit.Text = "Edit";
            this.menuEdit.Click += new System.EventHandler(this.OnEdit);
            // 
            // menuDelete
            // 
            this.menuDelete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(198, 22);
            this.menuDelete.Text = "Delete";
            this.menuDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // menuSep
            // 
            this.menuSep.Name = "menuSep";
            this.menuSep.Size = new System.Drawing.Size(195, 6);
            // 
            // menuTrack
            // 
            this.menuTrack.Image = global::ShipWorks.Properties.Resources.box_view16;
            this.menuTrack.Name = "menuTrack";
            this.menuTrack.Size = new System.Drawing.Size(198, 22);
            this.menuTrack.Text = "Track Shipment";
            this.menuTrack.Click += new System.EventHandler(this.OnTrackShipment);
            // 
            // menuCopyTracking
            // 
            this.menuCopyTracking.Name = "menuCopyTracking";
            this.menuCopyTracking.Size = new System.Drawing.Size(198, 22);
            this.menuCopyTracking.Text = "Copy Tracking Number";
            this.menuCopyTracking.Click += new System.EventHandler(this.OnCopyTracking);
            // 
            // menuInsuranceClaim
            // 
            this.menuInsuranceClaim.Image = global::ShipWorks.Properties.Resources.message;
            this.menuInsuranceClaim.Name = "menuInsuranceClaim";
            this.menuInsuranceClaim.Size = new System.Drawing.Size(198, 22);
            this.menuInsuranceClaim.Text = "Insurance Claim";
            this.menuInsuranceClaim.Click += new System.EventHandler(this.OnSubmitClaim);
            // 
            // menuSep2
            // 
            this.menuSep2.Name = "menuSep2";
            this.menuSep2.Size = new System.Drawing.Size(195, 6);
            // 
            // menuCopy
            // 
            this.menuCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(198, 22);
            this.menuCopy.Text = "Copy";
            // 
            // ratesControl
            // 
            this.ratesControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ratesControl.Location = new System.Drawing.Point(0, 64);
            this.ratesControl.Name = "ratesControl";
            this.ratesControl.Size = new System.Drawing.Size(659, 184);
            this.ratesControl.TabIndex = 5;
            // 
            // ShipmentsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.ratesControl);
            this.Name = "ShipmentsPanel";
            this.Size = new System.Drawing.Size(659, 327);
            this.Controls.SetChildIndex(this.addLink, 0);
            this.Controls.SetChildIndex(this.entityGrid, 0);
            this.Controls.SetChildIndex(this.ratesControl, 0);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripSeparator menuSep;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripMenuItem menuTrack;
        private System.Windows.Forms.ToolStripMenuItem menuCopyTracking;
        private System.Windows.Forms.ToolStripSeparator menuSep2;
        private Shipping.Editing.Rating.RatesPanel ratesControl;
        private System.Windows.Forms.ToolStripMenuItem menuInsuranceClaim;
    }
}
