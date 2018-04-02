namespace ShipWorks.Shipping.Settings.WizardPages
{
    partial class ShippingWizardPagePrinting
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
            this.printOutputControl = new ShipWorks.Shipping.Settings.Printing.PrintOutputControl();
            this.SuspendLayout();
            // 
            // printOutputControl
            // 
            this.printOutputControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printOutputControl.AutoScroll = true;
            this.printOutputControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printOutputControl.Location = new System.Drawing.Point(16, 0);
            this.printOutputControl.Name = "printOutputControl";
            this.printOutputControl.Size = new System.Drawing.Size(584, 412);
            this.printOutputControl.TabIndex = 1;
            // 
            // ShippingWizardPagePrinting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.printOutputControl);
            this.Description = "Configure what to print when printing shipments.";
            this.Name = "ShippingWizardPagePrinting";
            this.Size = new System.Drawing.Size(600, 412);
            this.Title = "Printing Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnPageShown);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Shipping.Settings.Printing.PrintOutputControl printOutputControl;
    }
}
