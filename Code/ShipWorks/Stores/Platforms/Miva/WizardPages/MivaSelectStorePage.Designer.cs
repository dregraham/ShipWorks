namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    partial class MivaSelectStorePage
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
            this.labelSelectStore = new System.Windows.Forms.Label();
            this.comboStores = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelSelectStore
            // 
            this.labelSelectStore.Location = new System.Drawing.Point(19, 12);
            this.labelSelectStore.Name = "labelSelectStore";
            this.labelSelectStore.Size = new System.Drawing.Size(417, 31);
            this.labelSelectStore.TabIndex = 0;
            this.labelSelectStore.Text = "The following stores were found in your Miva Merchant site.  Select the one you w" +
                "ant ShipWorks to connect to.";
            // 
            // comboStores
            // 
            this.comboStores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStores.FormattingEnabled = true;
            this.comboStores.Location = new System.Drawing.Point(48, 48);
            this.comboStores.MaxDropDownItems = 20;
            this.comboStores.Name = "comboStores";
            this.comboStores.Size = new System.Drawing.Size(264, 21);
            this.comboStores.TabIndex = 1;
            // 
            // MivaSelectStorePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboStores);
            this.Controls.Add(this.labelSelectStore);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MivaSelectStorePage";
            this.Size = new System.Drawing.Size(467, 214);
            this.Title = "Store Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSelectStore;
        private System.Windows.Forms.ComboBox comboStores;
    }
}
