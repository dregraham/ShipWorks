namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    partial class GenericStoreCsvSetupPage
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
            this.csvMapChooser = new ShipWorks.Data.Import.Spreadsheet.Types.Csv.OrderSchema.GenericCsvOrderMapChooserControl();
            this.SuspendLayout();
            // 
            // csvMapChooser
            // 
            this.csvMapChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csvMapChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvMapChooser.Location = new System.Drawing.Point(0, 0);
            this.csvMapChooser.Map = null;
            this.csvMapChooser.Name = "csvMapChooser";
            this.csvMapChooser.Size = new System.Drawing.Size(517, 203);
            this.csvMapChooser.TabIndex = 0;
            // 
            // GenericStoreCsvSetupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.csvMapChooser);
            this.Name = "GenericStoreCsvSetupPage";
            this.Size = new System.Drawing.Size(517, 203);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Data.Import.Spreadsheet.Types.Csv.OrderSchema.GenericCsvOrderMapChooserControl csvMapChooser;
    }
}
