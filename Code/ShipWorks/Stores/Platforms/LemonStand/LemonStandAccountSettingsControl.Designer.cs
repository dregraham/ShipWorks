namespace ShipWorks.Stores.Platforms.LemonStand
{
    partial class LemonStandAccountSettingsControl
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
            this.accessTokenTextBox = new System.Windows.Forms.TextBox();
            this.apiKeyTextBox = new System.Windows.Forms.TextBox();
            this.storeURLTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // accessTokenTextbox
            // 
            this.accessTokenTextBox.Location = new System.Drawing.Point(116, 95);
            this.accessTokenTextBox.Name = "accessTokenTextbox";
            this.accessTokenTextBox.Size = new System.Drawing.Size(362, 21);
            this.accessTokenTextBox.TabIndex = 14;
            // 
            // apiKeyTextbox
            // 
            this.apiKeyTextBox.Location = new System.Drawing.Point(116, 69);
            this.apiKeyTextBox.Name = "apiKeyTextbox";
            this.apiKeyTextBox.Size = new System.Drawing.Size(362, 21);
            this.apiKeyTextBox.TabIndex = 13;
            // 
            // storeURLTextbox
            // 
            this.storeURLTextBox.Location = new System.Drawing.Point(116, 43);
            this.storeURLTextBox.Name = "storeURLTextbox";
            this.storeURLTextBox.Size = new System.Drawing.Size(362, 21);
            this.storeURLTextBox.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(34, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Access Token:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label3.Location = new System.Drawing.Point(61, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "API Key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label2.Location = new System.Drawing.Point(51, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Store URL:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Enter your LemonStand account information";
            // 
            // LemonStandAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accessTokenTextBox);
            this.Controls.Add(this.apiKeyTextBox);
            this.Controls.Add(this.storeURLTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "LemonStandAccountSettingsControl";
            this.Size = new System.Drawing.Size(500, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox accessTokenTextBox;
        private System.Windows.Forms.TextBox apiKeyTextBox;
        private System.Windows.Forms.TextBox storeURLTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
