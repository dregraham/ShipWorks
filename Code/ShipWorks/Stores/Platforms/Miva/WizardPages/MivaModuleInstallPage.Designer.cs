namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    partial class MivaModuleInstallPage
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
            this.linkInstructions = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.labelProceed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linkInstructions
            // 
            this.linkInstructions.AutoSize = true;
            this.linkInstructions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkInstructions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkInstructions.ForeColor = System.Drawing.Color.Blue;
            this.linkInstructions.Location = new System.Drawing.Point(109, 26);
            this.linkInstructions.Name = "linkInstructions";
            this.linkInstructions.Size = new System.Drawing.Size(96, 13);
            this.linkInstructions.TabIndex = 5;
            this.linkInstructions.Text = "these instructions.";
            this.linkInstructions.Url = "http://www.interapptive.com/shipworks/help";
            // 
            // labelProceed
            // 
            this.labelProceed.AutoSize = true;
            this.labelProceed.Location = new System.Drawing.Point(19, 68);
            this.labelProceed.Name = "labelProceed";
            this.labelProceed.Size = new System.Drawing.Size(258, 13);
            this.labelProceed.TabIndex = 6;
            this.labelProceed.Text = "Proceed to the next step after installing the module.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(453, 27);
            this.label1.TabIndex = 7;
            this.label1.Text = "Before using ShipWorks with Miva Merchant you must download and install the ShipW" +
    "orks Miva Module using";
            // 
            // MivaModuleInstallPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkInstructions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelProceed);
            this.Description = "Enter the following information about your online store.";
            this.Name = "MivaModuleInstallPage";
            this.Size = new System.Drawing.Size(475, 102);
            this.Title = "Store Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkInstructions;
        private System.Windows.Forms.Label labelProceed;
        private System.Windows.Forms.Label label1;
    }
}
