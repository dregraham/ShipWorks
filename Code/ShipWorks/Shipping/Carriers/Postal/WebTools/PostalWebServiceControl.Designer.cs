namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    partial class PostalWebServiceControl
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
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            ((System.ComponentModel.ISupportInitialize) (this.sectionExpress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // insuranceControl
            // 
            this.insuranceControl.Size = new System.Drawing.Size(469, 50);
            // 
            // sectionExpress
            // 
            this.sectionExpress.Location = new System.Drawing.Point(3, 479);
            this.sectionExpress.Size = new System.Drawing.Size(415, 24);
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(415, 24);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Size = new System.Drawing.Size(407, 330);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 508);
            this.sectionReturns.Size = new System.Drawing.Size(415, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(415, 24);
            this.sectionLabelOptions.Visible = false;
            // 
            // sectionShipment
            // 
            this.sectionShipment.Location = new System.Drawing.Point(3, 161);
            this.sectionShipment.Size = new System.Drawing.Size(415, 313);
            // 
            // sectionFrom
            // 
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            // 
            // sectionFrom.ContentPanel
            // 
            this.sectionFrom.ContentPanel.Controls.Add(this.originControl);
            this.sectionFrom.ContentPanel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ExpandedHeight = 490;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(415, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originControl.Location = new System.Drawing.Point(0, 5);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(411, 0);
            this.originControl.TabIndex = 0;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // PostalWebServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "PostalWebServiceControl";
            this.Size = new System.Drawing.Size(421, 617);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionExpress, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize) (this.sectionExpress)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
    }
}
