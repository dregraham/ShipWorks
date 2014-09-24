namespace ShipWorks.Shipping.Carriers
{
    partial class MultiSelectServiceControl
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
            this.labelFromMultiple = new System.Windows.Forms.Label();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.ExpandedHeight = 446;
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(418, 24);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(384, 330);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 164);
            this.sectionReturns.Size = new System.Drawing.Size(418, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(418, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(418, 96);
            // 
            // sectionFrom
            // 
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            // 
            // sectionFrom.ContentPanel
            // 
            this.sectionFrom.ContentPanel.Controls.Add(this.labelFromMultiple);
            this.sectionFrom.ContentPanel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ExpandedHeight = 56;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(418, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // labelFromMultiple
            // 
            this.labelFromMultiple.AutoSize = true;
            this.labelFromMultiple.BackColor = System.Drawing.Color.Transparent;
            this.labelFromMultiple.ForeColor = System.Drawing.Color.DimGray;
            this.labelFromMultiple.Location = new System.Drawing.Point(6, 8);
            this.labelFromMultiple.Name = "labelFromMultiple";
            this.labelFromMultiple.Size = new System.Drawing.Size(51, 13);
            this.labelFromMultiple.TabIndex = 0;
            this.labelFromMultiple.Text = "(Multiple)";
            // 
            // insuranceControl
            // 
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(11, 9);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(501, 48);
            this.insuranceControl.TabIndex = 13;
            // 
            // MultiSelectServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "MultiSelectServiceControl";
            this.Size = new System.Drawing.Size(424, 265);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            this.sectionFrom.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private System.Windows.Forms.Label labelFromMultiple;
        private Insurance.InsuranceSelectionControl insuranceControl;

    }
}
