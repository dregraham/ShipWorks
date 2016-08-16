using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages
{
    partial class YahooProductWeightsPage
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
            this.labelProductWeights = new System.Windows.Forms.Label();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.radioUseWeights = new System.Windows.Forms.RadioButton();
            this.radioNoWeights = new System.Windows.Forms.RadioButton();
            this.importProductsControl = new YahooEmailImportProductsControl();
            this.SuspendLayout();
            // 
            // labelProductWeights
            // 
            this.labelProductWeights.AutoSize = true;
            this.labelProductWeights.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelProductWeights.Location = new System.Drawing.Point(25, 11);
            this.labelProductWeights.Name = "labelProductWeights";
            this.labelProductWeights.Size = new System.Drawing.Size(100, 13);
            this.labelProductWeights.TabIndex = 0;
            this.labelProductWeights.Text = "Product Weights";
            // 
            // labelInfo1
            // 
            this.labelInfo1.CausesValidation = false;
            this.labelInfo1.Location = new System.Drawing.Point(28, 31);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(510, 18);
            this.labelInfo1.TabIndex = 1;
            this.labelInfo1.Text = "To retrieve product weights ShipWorks needs to download the Product Catalog for y" +
                "our store.";
            // 
            // radioUseWeights
            // 
            this.radioUseWeights.AutoSize = true;
            this.radioUseWeights.Checked = true;
            this.radioUseWeights.Location = new System.Drawing.Point(30, 51);
            this.radioUseWeights.Name = "radioUseWeights";
            this.radioUseWeights.Size = new System.Drawing.Size(348, 17);
            this.radioUseWeights.TabIndex = 4;
            this.radioUseWeights.TabStop = true;
            this.radioUseWeights.Text = "I want ShipWorks to use the product weights from my Yahoo! store";
            this.radioUseWeights.UseVisualStyleBackColor = true;
            this.radioUseWeights.CheckedChanged += new System.EventHandler(this.OnRadioSelectionChanged);
            // 
            // radioNoWeights
            // 
            this.radioNoWeights.AutoSize = true;
            this.radioNoWeights.Location = new System.Drawing.Point(31, 180);
            this.radioNoWeights.Name = "radioNoWeights";
            this.radioNoWeights.Size = new System.Drawing.Size(270, 17);
            this.radioNoWeights.TabIndex = 6;
            this.radioNoWeights.Text = "I don\'t need product weights from my Yahoo! store";
            this.radioNoWeights.UseVisualStyleBackColor = true;
            this.radioNoWeights.CheckedChanged += new System.EventHandler(this.OnRadioSelectionChanged);
            // 
            // importProductsControl
            // 
            this.importProductsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.importProductsControl.Location = new System.Drawing.Point(44, 73);
            this.importProductsControl.Name = "importProductsControl";
            this.importProductsControl.Size = new System.Drawing.Size(513, 105);
            this.importProductsControl.TabIndex = 7;
            // 
            // YahooProductWeightsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importProductsControl);
            this.Controls.Add(this.radioNoWeights);
            this.Controls.Add(this.radioUseWeights);
            this.Controls.Add(this.labelInfo1);
            this.Controls.Add(this.labelProductWeights);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "YahooProductWeightsPage";
            this.Size = new System.Drawing.Size(562, 262);
            this.Title = "Store Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProductWeights;
        private System.Windows.Forms.Label labelInfo1;
        private System.Windows.Forms.RadioButton radioUseWeights;
        private System.Windows.Forms.RadioButton radioNoWeights;
        private YahooEmailImportProductsControl importProductsControl;
    }
}
