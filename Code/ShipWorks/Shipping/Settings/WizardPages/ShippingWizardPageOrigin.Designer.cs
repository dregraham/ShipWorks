namespace ShipWorks.Shipping.Settings.WizardPages
{
    partial class ShippingWizardPageOrigin
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
            this.labelOriginInfo = new System.Windows.Forms.Label();
            this.originManagerControl = new ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl();
            this.SuspendLayout();
            // 
            // labelOriginInfo
            // 
            this.labelOriginInfo.Location = new System.Drawing.Point(19, 4);
            this.labelOriginInfo.Name = "labelOriginInfo";
            this.labelOriginInfo.Size = new System.Drawing.Size(473, 31);
            this.labelOriginInfo.TabIndex = 3;
            this.labelOriginInfo.Text = "You can setup common origin addresses for easy selection when shipping.  ShipWork" +
                "s also allows you to enter a manual address or use the address of your store, so" +
                " this step is optional.";
            // 
            // originManagerControl
            // 
            this.originManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originManagerControl.Location = new System.Drawing.Point(20, 42);
            this.originManagerControl.Name = "originManagerControl";
            this.originManagerControl.Size = new System.Drawing.Size(441, 150);
            this.originManagerControl.TabIndex = 2;
            this.originManagerControl.ShipperAdded += new System.EventHandler(this.OnOriginAdded);
            // 
            // ShippingWizardPageOrigin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelOriginInfo);
            this.Controls.Add(this.originManagerControl);
            this.Description = "Enter the origin addresses for your shipments.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShippingWizardPageOrigin";
            this.Size = new System.Drawing.Size(510, 206);
            this.Title = "Origin Address";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelOriginInfo;
        private ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl originManagerControl;
    }
}
