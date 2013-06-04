namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    partial class MivaModuleQuestionPage
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
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.labelIntsalled = new System.Windows.Forms.Label();
            this.radioInstalled = new System.Windows.Forms.RadioButton();
            this.radioNotInstalled = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(19, 12);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(440, 34);
            this.labelInfo1.TabIndex = 2;
            this.labelInfo1.Text = "ShipWorks connects to Miva Merchant using the ShipWorks Miva Module, which must b" +
                "e installed on your Miva Merchant website.";
            // 
            // labelIntsalled
            // 
            this.labelIntsalled.AutoSize = true;
            this.labelIntsalled.Location = new System.Drawing.Point(40, 48);
            this.labelIntsalled.Name = "labelIntsalled";
            this.labelIntsalled.Size = new System.Drawing.Size(273, 13);
            this.labelIntsalled.TabIndex = 3;
            this.labelIntsalled.Text = "Have you already installed the ShipWorks Miva Module?";
            // 
            // radioInstalled
            // 
            this.radioInstalled.AutoSize = true;
            this.radioInstalled.Location = new System.Drawing.Point(60, 92);
            this.radioInstalled.Name = "radioInstalled";
            this.radioInstalled.Size = new System.Drawing.Size(111, 17);
            this.radioInstalled.TabIndex = 1;
            this.radioInstalled.TabStop = true;
            this.radioInstalled.Text = "Yes, it is installed.";
            this.radioInstalled.UseVisualStyleBackColor = true;
            // 
            // radioNotInstalled
            // 
            this.radioNotInstalled.AutoSize = true;
            this.radioNotInstalled.Location = new System.Drawing.Point(60, 69);
            this.radioNotInstalled.Name = "radioNotInstalled";
            this.radioNotInstalled.Size = new System.Drawing.Size(121, 17);
            this.radioNotInstalled.TabIndex = 0;
            this.radioNotInstalled.TabStop = true;
            this.radioNotInstalled.Text = "No, or I don\'t know.";
            this.radioNotInstalled.UseVisualStyleBackColor = true;
            // 
            // MivaModuleQuestionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioNotInstalled);
            this.Controls.Add(this.radioInstalled);
            this.Controls.Add(this.labelIntsalled);
            this.Controls.Add(this.labelInfo1);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MivaModuleQuestionPage";
            this.Size = new System.Drawing.Size(473, 203);
            this.Title = "Store Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfo1;
        private System.Windows.Forms.Label labelIntsalled;
        private System.Windows.Forms.RadioButton radioInstalled;
        private System.Windows.Forms.RadioButton radioNotInstalled;
    }
}
