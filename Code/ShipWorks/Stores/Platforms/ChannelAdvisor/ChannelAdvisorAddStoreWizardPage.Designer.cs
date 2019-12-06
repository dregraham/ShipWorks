namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorAddStoreWizardPage
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
            this.excludeFba = new ShipWorks.Stores.Platforms.ChannelAdvisor.ChannelAdvisorExcludeFbaControl();
            this.SuspendLayout();
            // 
            // excludeFba
            // 
            this.excludeFba.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludeFba.Location = new System.Drawing.Point(0, 0);
            this.excludeFba.Name = "excludeFba";
            this.excludeFba.Size = new System.Drawing.Size(580, 60);
            this.excludeFba.TabIndex = 0;
            // 
            // ChannelAdvisorAddStoreWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.excludeFba);
            this.Name = "ChannelAdvisorAddStoreWizardPage";
            this.Size = new System.Drawing.Size(585, 150);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private ChannelAdvisorExcludeFbaControl excludeFba;
    }
}
