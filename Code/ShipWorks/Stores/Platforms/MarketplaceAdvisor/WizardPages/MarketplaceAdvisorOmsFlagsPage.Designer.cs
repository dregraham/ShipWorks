namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    partial class MarketplaceAdvisorOmsFlagsPage
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
            this.flagsControl = new ShipWorks.Stores.Platforms.MarketplaceAdvisor.MarketplaceAdvisorOmsFlagsControl();
            this.SuspendLayout();
            // 
            // flagsControl
            // 
            this.flagsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.flagsControl.Location = new System.Drawing.Point(19, 12);
            this.flagsControl.Name = "flagsControl";
            this.flagsControl.Size = new System.Drawing.Size(486, 218);
            this.flagsControl.TabIndex = 0;
            // 
            // MarketplaceAdvisorOmsFlagsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flagsControl);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MarketplaceAdvisorOmsFlagsPage";
            this.Size = new System.Drawing.Size(515, 237);
            this.Title = "Store Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnPageShown);
            this.ResumeLayout(false);

        }

        #endregion

        private MarketplaceAdvisorOmsFlagsControl flagsControl;

    }
}
