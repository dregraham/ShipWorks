namespace ShipWorks.Stores.Platforms.LemonStand.WizardPages
{
    partial class LemonStandAccountPage
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
            this.accessTokenTextbox = new System.Windows.Forms.TextBox();
            this.apiKeyTextbox = new System.Windows.Forms.TextBox();
            this.storeURLTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // accessTokenTextbox
            // 
            this.accessTokenTextbox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accessTokenTextbox.Location = new System.Drawing.Point(116, 95);
            this.accessTokenTextbox.Name = "accessTokenTextbox";
            this.accessTokenTextbox.Size = new System.Drawing.Size(362, 21);
            this.accessTokenTextbox.TabIndex = 21;
            this.accessTokenTextbox.Text = "mR5xLW3j1lChB6QPOm1UN5lAT6tq6zIUZUZtgQwr";
            // 
            // apiKeyTextbox
            // 
            this.apiKeyTextbox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apiKeyTextbox.Location = new System.Drawing.Point(116, 69);
            this.apiKeyTextbox.Name = "apiKeyTextbox";
            this.apiKeyTextbox.Size = new System.Drawing.Size(362, 21);
            this.apiKeyTextbox.TabIndex = 20;
            this.apiKeyTextbox.Text = "7hMk9yBK4sRdV9SGC2m0KGQYytlWjirWRDN2E6jH";
            // 
            // storeURLTextbox
            // 
            this.storeURLTextbox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeURLTextbox.Location = new System.Drawing.Point(116, 43);
            this.storeURLTextbox.Name = "storeURLTextbox";
            this.storeURLTextbox.Size = new System.Drawing.Size(362, 21);
            this.storeURLTextbox.TabIndex = 19;
            this.storeURLTextbox.Text = "https://shipworks.lemonstand.com/";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(34, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Access Token:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(61, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "API Key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Store URL:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Enter your LemonStand account information";
            // 
            // LemonStandAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accessTokenTextbox);
            this.Controls.Add(this.apiKeyTextbox);
            this.Controls.Add(this.storeURLTextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "LemonStandAccountPage";
            this.Size = new System.Drawing.Size(500, 200);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox accessTokenTextbox;
        private System.Windows.Forms.TextBox apiKeyTextbox;
        private System.Windows.Forms.TextBox storeURLTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
