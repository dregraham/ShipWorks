namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class BestRateServiceControl
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
            this.sectionRates = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.rateControl = new ShipWorks.Shipping.Editing.RateControl();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRates.ContentPanel)).BeginInit();
            this.sectionRates.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 143);
            // 
            // sectionReturns
            // 
            // 
            // sectionShipment
            // 
            this.sectionShipment.Location = new System.Drawing.Point(3, 34);
            // 
            // sectionRates
            // 
            this.sectionRates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionRates.Collapsed = true;
            // 
            // sectionRates.ContentPanel
            // 
            this.sectionRates.ContentPanel.Controls.Add(this.rateControl);
            this.sectionRates.ExpandedHeight = 90;
            this.sectionRates.ExtraText = "";
            this.sectionRates.Location = new System.Drawing.Point(3, 5);
            this.sectionRates.Name = "sectionRates";
            this.sectionRates.SectionName = "Rates";
            this.sectionRates.SettingsKey = "{4b96a784-c2c9-4e5e-9f58-28adec07349f}";
            this.sectionRates.Size = new System.Drawing.Size(385, 24);
            this.sectionRates.TabIndex = 2;
            // 
            // rateControl
            // 
            this.rateControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rateControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rateControl.Location = new System.Drawing.Point(0, 0);
            this.rateControl.Name = "rateControl";
            this.rateControl.Size = new System.Drawing.Size(396, 0);
            this.rateControl.TabIndex = 3;
            this.rateControl.RateSelected += new ShipWorks.Shipping.Editing.RateSelectedEventHandler(this.OnRateSelected);
            // 
            // BestRateServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionRates);
            this.Name = "BestRateServiceControl";
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRates, 0);
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRates.ContentPanel)).EndInit();
            this.sectionRates.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionRates)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionRates;
        private ShipWorks.Shipping.Editing.RateControl rateControl;
    }
}
