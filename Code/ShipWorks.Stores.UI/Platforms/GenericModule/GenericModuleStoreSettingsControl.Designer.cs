using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Stores.UI.Platforms.GenericModule
{
    partial class GenericModuleStoreSettingsControl
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
            this.amazon = new ShipWorks.Shipping.Carriers.Amazon.AmazonShippingSettingsControl();
            this.SuspendLayout();
            // 
            // amazon
            // 
            this.amazon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.amazon.Location = new System.Drawing.Point(0, 13);
            this.amazon.Name = "amazon";
            this.amazon.Size = new System.Drawing.Size(617, 129);
            this.amazon.TabIndex = 2;
            // 
            // GenericModuleStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.amazon);
            this.Name = "GenericModuleStoreSettingsControl";
            this.Size = new System.Drawing.Size(617, 142);
            this.ResumeLayout(false);

        }

        #endregion
        
        private AmazonShippingSettingsControl amazon;
    }
}
