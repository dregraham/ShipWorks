namespace ShipWorks.Stores.Platforms.Groupon.WizardPages
{
    partial class GrouponStoreAccountPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.supplierIDTextbox = new System.Windows.Forms.TextBox();
            this.tokenTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter your Groupon account information :";
            // 
            // supplierIDTextbox
            // 
            this.supplierIDTextbox.Location = new System.Drawing.Point(100, 41);
            this.supplierIDTextbox.Name = "supplierIDTextbox";
            this.supplierIDTextbox.Size = new System.Drawing.Size(315, 21);
            this.supplierIDTextbox.TabIndex = 3;
            // 
            // tokenTextBox
            // 
            this.tokenTextBox.Location = new System.Drawing.Point(100, 68);
            this.tokenTextBox.Name = "tokenTextBox";
            this.tokenTextBox.Size = new System.Drawing.Size(315, 21);
            this.tokenTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Supplier ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Token:";
            // 
            // helpLink
            // 
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(245, 92);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(55, 18);
            this.helpLink.TabIndex = 12;
            this.helpLink.TabStop = true;
            this.helpLink.Text = "click here.";
            this.helpLink.Url = "http://www.interapptive.com/shipworks/help";
            this.helpLink.UseCompatibleTextRendering = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(97, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(223, 29);
            this.label5.TabIndex = 13;
            this.label5.Text = "For help with adding Groupon";
            // 
            // GrouponStoreAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tokenTextBox);
            this.Controls.Add(this.supplierIDTextbox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "GrouponStoreAccountPage";
            this.Size = new System.Drawing.Size(544, 377);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox supplierIDTextbox;
        private System.Windows.Forms.TextBox tokenTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ApplicationCore.Interaction.HelpLink helpLink;
        private System.Windows.Forms.Label label5;
    }
}
