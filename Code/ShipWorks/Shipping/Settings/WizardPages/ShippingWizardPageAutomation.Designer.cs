namespace ShipWorks.Shipping.Settings.WizardPages
{
    partial class ShippingWizardPageAutomation
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                lifetimeScope.Dispose();
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
            this.automationControl = new ShipWorks.Shipping.Settings.ShipmentAutomationControl();
            this.SuspendLayout();
            //
            // automationControl
            //
            this.automationControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.automationControl.Location = new System.Drawing.Point(21, 11);
            this.automationControl.Name = "automationControl";
            this.automationControl.Size = new System.Drawing.Size(345, 267);
            this.automationControl.TabIndex = 1;
            //
            // ShippingWizardPageAutomation
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.automationControl);
            this.Description = "Configure automated processing tasks.";
            this.Name = "ShippingWizardPageAutomation";
            this.Size = new System.Drawing.Size(431, 282);
            this.Title = "Processing Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipmentAutomationControl automationControl;
    }
}
