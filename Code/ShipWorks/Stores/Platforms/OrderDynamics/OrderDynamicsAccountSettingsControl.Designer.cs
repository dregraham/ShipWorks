namespace ShipWorks.Stores.Platforms.OrderDynamics
{
    partial class OrderDynamicsAccountSettingsControl
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.licenseTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the URL to the ShipWorks OrderDynamics Integration:";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(132, 27);
            this.fieldLengthProvider.SetMaxLengthSource(this.urlTextBox, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericModuleUrl);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(282, 21);
            this.urlTextBox.TabIndex = 0;
            // 
            // licenseTextBox
            // 
            this.licenseTextBox.Location = new System.Drawing.Point(132, 120);
            this.licenseTextBox.MaxLength = 50;
            this.fieldLengthProvider.SetMaxLengthSource(this.licenseTextBox, ShipWorks.Data.Utility.EntityFieldLengthSource.GenericStoreCode);
            this.licenseTextBox.Name = "licenseTextBox";
            this.licenseTextBox.Size = new System.Drawing.Size(282, 21);
            this.licenseTextBox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "&License ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Integration URL:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(386, 30);
            this.label3.TabIndex = 9;
            this.label3.Text = "Enter your OrderDynamics License Identifier, which can be found in your store\'s H" +
                "elp -> About page.";
            // 
            // OrderDynamicsAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.licenseTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "OrderDynamicsAccountSettingsControl";
            this.Size = new System.Drawing.Size(482, 283);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox licenseTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label label3;
    }
}
