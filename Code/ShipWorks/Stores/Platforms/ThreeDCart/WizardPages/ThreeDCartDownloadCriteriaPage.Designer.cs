namespace ShipWorks.Stores.Platforms.ThreeDCart.WizardPages
{
    partial class ThreeDCartDownloadCriteriaPage
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
            this.threeDCartDownloadCriteriaControl = new ShipWorks.Stores.Platforms.ThreeDCart.ThreeDCartDownloadCriteriaControl();
            this.SuspendLayout();
            // 
            // threeDCartDownloadCriteriaControl
            // 
            this.threeDCartDownloadCriteriaControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.threeDCartDownloadCriteriaControl.Location = new System.Drawing.Point(8, 8);
            this.threeDCartDownloadCriteriaControl.Name = "threeDCartDownloadCriteriaControl";
            this.threeDCartDownloadCriteriaControl.Size = new System.Drawing.Size(508, 200);
            this.threeDCartDownloadCriteriaControl.TabIndex = 2;
            // 
            // ThreeDCartDownloadCriteriaPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.threeDCartDownloadCriteriaControl);
            this.Name = "ThreeDCartDownloadCriteriaPage";
            this.Size = new System.Drawing.Size(544, 112);
            this.Title = "ThreeDCart Download Criteria";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);

        }

        #endregion

        private ThreeDCartDownloadCriteriaControl threeDCartDownloadCriteriaControl;
    }
}
