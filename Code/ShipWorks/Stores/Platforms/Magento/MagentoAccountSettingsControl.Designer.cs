﻿namespace ShipWorks.Stores.Platforms.Magento
{
    partial class MagentoAccountSettingsControl
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
            this.label6 = new System.Windows.Forms.Label();
            this.storeCodeTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.radioModuleDirect = new System.Windows.Forms.RadioButton();
            this.radioMagentoConnect = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // helpLink
            // 
            this.helpLink.Location = new System.Drawing.Point(47, 215);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(42, 241);
            // 
            // moduleUrl
            // 
            this.moduleUrl.Location = new System.Drawing.Point(117, 238);
            this.moduleUrl.Size = new System.Drawing.Size(362, 21);
            this.moduleUrl.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(20, 202);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 100);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(50, 128);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(52, 154);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(117, 125);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(117, 151);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 301);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Store Code:";
            // 
            // storeCodeTextBox
            // 
            this.storeCodeTextBox.Location = new System.Drawing.Point(117, 298);
            this.storeCodeTextBox.Name = "storeCodeTextBox";
            this.storeCodeTextBox.Size = new System.Drawing.Size(107, 21);
            this.storeCodeTextBox.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(42, 279);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(249, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Only required on multi-store Magento installations:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Magento Connection";
            // 
            // radioModuleDirect
            // 
            this.radioModuleDirect.AutoSize = true;
            this.radioModuleDirect.Checked = true;
            this.radioModuleDirect.Location = new System.Drawing.Point(23, 22);
            this.radioModuleDirect.Name = "radioModuleDirect";
            this.radioModuleDirect.Size = new System.Drawing.Size(250, 17);
            this.radioModuleDirect.TabIndex = 15;
            this.radioModuleDirect.TabStop = true;
            this.radioModuleDirect.Text = "I use Magento Community or Enterprise Edition";
            this.radioModuleDirect.UseVisualStyleBackColor = true;
            // 
            // radioMagentoConnect
            // 
            this.radioMagentoConnect.AutoSize = true;
            this.radioMagentoConnect.Location = new System.Drawing.Point(23, 45);
            this.radioMagentoConnect.Name = "radioMagentoConnect";
            this.radioMagentoConnect.Size = new System.Drawing.Size(110, 17);
            this.radioMagentoConnect.TabIndex = 16;
            this.radioMagentoConnect.Text = "I use Magento Go";
            this.radioMagentoConnect.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Account Credentials";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 182);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Connection URL";
            // 
            // MagentoAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.radioMagentoConnect);
            this.Controls.Add(this.radioModuleDirect);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.storeCodeTextBox);
            this.Controls.Add(this.label7);
            this.Name = "MagentoAccountSettingsControl";
            this.Size = new System.Drawing.Size(482, 337);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.username, 0);
            this.Controls.SetChildIndex(this.password, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.storeCodeTextBox, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.helpLink, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.moduleUrl, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.radioModuleDirect, 0);
            this.Controls.SetChildIndex(this.radioMagentoConnect, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox storeCodeTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioModuleDirect;
        private System.Windows.Forms.RadioButton radioMagentoConnect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}
