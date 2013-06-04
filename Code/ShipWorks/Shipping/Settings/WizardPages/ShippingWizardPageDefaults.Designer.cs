namespace ShipWorks.Shipping.Settings.WizardPages
{
    partial class ShippingWizardPageDefaults
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
            this.defaultsControl = new ShipWorks.Shipping.Settings.Defaults.ShippingDefaultsControl();
            this.SuspendLayout();
            // 
            // defaultsControl
            // 
            this.defaultsControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultsControl.AutoScroll = true;
            this.defaultsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.defaultsControl.Location = new System.Drawing.Point(23, 12);
            this.defaultsControl.Name = "defaultsControl";
            this.defaultsControl.Size = new System.Drawing.Size(570, 499);
            this.defaultsControl.TabIndex = 1;
            // 
            // ShippingWizardPageDefaults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.defaultsControl);
            this.Description = "Configure default settings for shipments.";
            this.Name = "ShippingWizardPageDefaults";
            this.Size = new System.Drawing.Size(599, 525);
            this.Title = "Shipment Defaults";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Shipping.Settings.Defaults.ShippingDefaultsControl defaultsControl;
    }
}
