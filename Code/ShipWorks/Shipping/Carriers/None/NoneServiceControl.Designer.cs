namespace ShipWorks.Shipping.Carriers.None
{
    partial class NoneServiceControl
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
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 114);
            this.sectionRecipient.Size = new System.Drawing.Size(398, 24);
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(391, 330);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 85);
            this.sectionReturns.Size = new System.Drawing.Size(398, 24);
            this.sectionReturns.Visible = false;

            this.sectionLabelOptions.Size = new System.Drawing.Size(398, 24);
            this.sectionLabelOptions.Visible = false;
            // 
            // sectionShipment
            // 
            this.sectionShipment.Location = new System.Drawing.Point(3, 5);
            this.sectionShipment.Size = new System.Drawing.Size(398, 75);
            this.sectionShipment.TabIndex = 1;
            this.sectionShipment.Visible = false;
            // 
            // NoneServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NoneServiceControl";
            this.Size = new System.Drawing.Size(404, 354);
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
