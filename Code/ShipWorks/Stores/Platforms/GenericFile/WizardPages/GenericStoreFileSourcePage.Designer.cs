namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    partial class GenericStoreFileSourcePage
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
            this.fileSourceMasterControl = new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceMasterControl();
            this.SuspendLayout();
            // 
            // fileSourceMasterControl
            // 
            this.fileSourceMasterControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileSourceMasterControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.fileSourceMasterControl.Location = new System.Drawing.Point(0, 0);
            this.fileSourceMasterControl.Name = "fileSourceMasterControl";
            this.fileSourceMasterControl.ShowChooseOption = true;
            this.fileSourceMasterControl.Size = new System.Drawing.Size(491, 415);
            this.fileSourceMasterControl.TabIndex = 0;
            // 
            // GenericStoreFileSourcePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fileSourceMasterControl);
            this.Name = "GenericStoreFileSourcePage";
            this.Size = new System.Drawing.Size(491, 415);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceMasterControl fileSourceMasterControl;
    }
}
