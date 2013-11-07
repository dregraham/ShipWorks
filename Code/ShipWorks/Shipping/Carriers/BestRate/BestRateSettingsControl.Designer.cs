using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class BestRateSettingsControl
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
            this.panelProviders = new ShippingTypeCheckBoxesControl();
            this.labelProvidersInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelProviders
            // 
            this.panelProviders.Location = new System.Drawing.Point(6, 17);
            this.panelProviders.Name = "panelProviders";
            this.panelProviders.Size = new System.Drawing.Size(329, 100);
            this.panelProviders.TabIndex = 3;
            // 
            // labelProvidersInfo
            // 
            this.labelProvidersInfo.AutoSize = true;
            this.labelProvidersInfo.Location = new System.Drawing.Point(3, 0);
            this.labelProvidersInfo.Name = "labelProvidersInfo";
            this.labelProvidersInfo.Size = new System.Drawing.Size(310, 13);
            this.labelProvidersInfo.TabIndex = 2;
            this.labelProvidersInfo.Text = "The following providers will be available in the shipping window:";
            // 
            // BestRateSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelProviders);
            this.Controls.Add(this.labelProvidersInfo);
            this.Name = "BestRateSettingsControl";
            this.Size = new System.Drawing.Size(338, 389);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShippingTypeCheckBoxesControl panelProviders;
        private System.Windows.Forms.Label labelProvidersInfo;
    }
}
