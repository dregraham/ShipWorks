namespace ShipWorks.Shipping.Carriers.Amazon
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
            this.accountManagerControl = new ShipWorks.Shipping.Carriers.Amazon.AmazonAccountManagerControl();
            this.SuspendLayout();
            // 
            // accountManager
            // 
            this.accountManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountManagerControl.Location = new System.Drawing.Point(29, 182);
            this.accountManagerControl.Name = "accountManager";
            this.accountManagerControl.Size = new System.Drawing.Size(400, 168);
            this.accountManagerControl.TabIndex = 0;
            // 
            // AmazonSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountManagerControl);
            this.Name = "AmazonSettingsControl";
            this.Size = new System.Drawing.Size(493, 665);
            this.ResumeLayout(false);

        }

        #endregion

        private AmazonAccountManagerControl accountManagerControl;
    }
}
