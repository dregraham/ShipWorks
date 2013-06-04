namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    partial class GenericStoreXmlSetupPage
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
            this.xmlFormatControl = new ShipWorks.Stores.Platforms.GenericFile.Formats.Xml.GenericFileXmlSetupControl();
            this.SuspendLayout();
            // 
            // xmlFormatControl
            // 
            this.xmlFormatControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmlFormatControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.xmlFormatControl.IsVerified = false;
            this.xmlFormatControl.Location = new System.Drawing.Point(0, 0);
            this.xmlFormatControl.Name = "xmlFormatControl";
            this.xmlFormatControl.Size = new System.Drawing.Size(525, 314);
            this.xmlFormatControl.TabIndex = 0;
            // 
            // GenericStoreXmlSetupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xmlFormatControl);
            this.Name = "GenericStoreXmlSetupPage";
            this.Size = new System.Drawing.Size(525, 314);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);

        }

        #endregion

        private Formats.Xml.GenericFileXmlSetupControl xmlFormatControl;
    }
}
