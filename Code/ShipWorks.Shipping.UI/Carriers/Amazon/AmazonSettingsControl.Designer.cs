using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    partial class AmazonSettingsControl
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
            this.amazonOptionsControl1 = new ShipWorks.Shipping.Carriers.Amazon.AmazonOptionsControl();
            this.SuspendLayout();
            // 
            // amazonOptionsControl1
            // 
            this.amazonOptionsControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amazonOptionsControl1.Location = new System.Drawing.Point(2, 0);
            this.amazonOptionsControl1.Name = "amazonOptionsControl1";
            this.amazonOptionsControl1.Size = new System.Drawing.Size(377, 46);
            this.amazonOptionsControl1.TabIndex = 25;
            // 
            // AmazonSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.amazonOptionsControl1);
            this.Name = "AmazonSettingsControl";
            this.Size = new System.Drawing.Size(445, 119);
            this.ResumeLayout(false);

        }

        #endregion
        private AmazonOptionsControl amazonOptionsControl1;
    }
}
