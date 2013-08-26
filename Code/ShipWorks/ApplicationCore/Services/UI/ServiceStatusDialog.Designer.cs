namespace ShipWorks.ApplicationCore.Services.UI
{
    partial class ServiceStatusDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button closeButton;
            System.Windows.Forms.Label labelHeader;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceStatusDialog));
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer2 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.startingServicePanel = new System.Windows.Forms.Panel();
            this.startingServiceImage = new System.Windows.Forms.PictureBox();
            this.startingServiceLabel = new System.Windows.Forms.Label();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            closeButton = new System.Windows.Forms.Button();
            labelHeader = new System.Windows.Forms.Label();
            this.startingServicePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startingServiceImage)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            closeButton.Location = new System.Drawing.Point(490, 227);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(76, 23);
            closeButton.TabIndex = 6;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.FlatStyle = System.Windows.Forms.FlatStyle.System;
            labelHeader.Location = new System.Drawing.Point(12, 9);
            labelHeader.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(687, 52);
            labelHeader.TabIndex = 8;
            labelHeader.Text = resources.GetString("labelHeader.Text");
            // 
            // startingServicePanel
            // 
            this.startingServicePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startingServicePanel.Controls.Add(this.startingServiceImage);
            this.startingServicePanel.Controls.Add(this.startingServiceLabel);
            this.startingServicePanel.Location = new System.Drawing.Point(9, 227);
            this.startingServicePanel.Name = "startingServicePanel";
            this.startingServicePanel.Size = new System.Drawing.Size(221, 23);
            this.startingServicePanel.TabIndex = 11;
            // 
            // startingServiceImage
            // 
            this.startingServiceImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startingServiceImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.startingServiceImage.Image = global::ShipWorks.Properties.Resources.arrows_greengray;
            this.startingServiceImage.Location = new System.Drawing.Point(3, 3);
            this.startingServiceImage.Name = "startingServiceImage";
            this.startingServiceImage.Size = new System.Drawing.Size(16, 16);
            this.startingServiceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.startingServiceImage.TabIndex = 6;
            this.startingServiceImage.TabStop = false;
            // 
            // startingServiceLabel
            // 
            this.startingServiceLabel.AutoSize = true;
            this.startingServiceLabel.Location = new System.Drawing.Point(25, 5);
            this.startingServiceLabel.Name = "startingServiceLabel";
            this.startingServiceLabel.Size = new System.Drawing.Size(155, 13);
            this.startingServiceLabel.TabIndex = 0;
            this.startingServiceLabel.Text = "Starting service, please wait...";
            // 
            // entityGrid
            // 
            this.entityGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entityGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(12, 52);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.Renderer = windowsXPRenderer2;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(554, 169);
            this.entityGrid.StretchPrimaryGrid = false;
            this.entityGrid.TabIndex = 0;
            // 
            // ServiceStatusDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = closeButton;
            this.ClientSize = new System.Drawing.Size(584, 262);
            this.Controls.Add(this.entityGrid);
            this.Controls.Add(this.startingServicePanel);
            this.Controls.Add(labelHeader);
            this.Controls.Add(closeButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 200);
            this.Name = "ServiceStatusDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Action Scheduler - Service Status Monitor";
            this.startingServicePanel.ResumeLayout(false);
            this.startingServicePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startingServiceImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel startingServicePanel;
        private System.Windows.Forms.Label startingServiceLabel;
        private System.Windows.Forms.PictureBox startingServiceImage;
        private Data.Grid.Paging.PagedEntityGrid entityGrid;
    }
}