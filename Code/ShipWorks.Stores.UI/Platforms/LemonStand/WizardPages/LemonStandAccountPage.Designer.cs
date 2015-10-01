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
            this.storeURLTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // accessTokenTextbox
            // 
            this.accessTokenTextbox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accessTokenTextbox.Location = new System.Drawing.Point(102, 62);
            this.accessTokenTextbox.Name = "accessTokenTextbox";
            this.accessTokenTextbox.Size = new System.Drawing.Size(339, 21);
            this.accessTokenTextbox.TabIndex = 19;
            // 
            // storeURLTextbox
            // 
            this.storeURLTextbox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeURLTextbox.Location = new System.Drawing.Point(102, 35);
            this.storeURLTextbox.Name = "storeURLTextbox";
            this.storeURLTextbox.Size = new System.Drawing.Size(339, 21);
            this.storeURLTextbox.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Access Token:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Store URL:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Enter the URL to your store and and your access token.";
            // 
            // helpLink
            // 
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(295, 86);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(55, 18);
            this.helpLink.TabIndex = 20;
            this.helpLink.TabStop = true;
            this.helpLink.Text = "click here.";
            this.helpLink.Url = "http://support.shipworks.com/support/solutions/articles/4000062623";
            this.helpLink.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(99, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "For help adding your LemonStand store,";
            // 
            // LemonStandAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.accessTokenTextbox);
            this.Controls.Add(this.storeURLTextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "LemonStandAccountPage";
            this.Size = new System.Drawing.Size(487, 209);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox accessTokenTextbox;
        private System.Windows.Forms.TextBox storeURLTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ApplicationCore.Interaction.HelpLink helpLink;
        private System.Windows.Forms.Label label3;
    }
}
