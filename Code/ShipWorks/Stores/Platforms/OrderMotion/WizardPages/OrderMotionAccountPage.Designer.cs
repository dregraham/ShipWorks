﻿namespace ShipWorks.Stores.Platforms.OrderMotion.WizardPages
{
    partial class OrderMotionAccountPage
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
            this.bizIdTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the HTTP BizID for your OrderMotion account:";
            // 
            // bizIdTextBox
            // 
            this.bizIdTextBox.Location = new System.Drawing.Point(47, 43);
            this.bizIdTextBox.Multiline = true;
            this.bizIdTextBox.Name = "bizIdTextBox";
            this.bizIdTextBox.Size = new System.Drawing.Size(311, 117);
            this.bizIdTextBox.TabIndex = 2;
            // 
            // OrderMotionAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bizIdTextBox);
            this.Controls.Add(this.label1);
            this.Name = "OrderMotionAccountPage";
            this.Size = new System.Drawing.Size(464, 272);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox bizIdTextBox;
    }
}
