namespace ShipWorks.Stores.Platforms.OrderMotion
{
    partial class OrderMotionAccountSettingsControl
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
            this.emailAccountControl = new ShipWorks.Stores.Platforms.OrderMotion.OrderMotionEmailAccountControl();
            this.bizIdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // emailAccountControl
            // 
            this.emailAccountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailAccountControl.Location = new System.Drawing.Point(8, 10);
            this.emailAccountControl.Name = "emailAccountControl";
            this.emailAccountControl.Size = new System.Drawing.Size(343, 76);
            this.emailAccountControl.TabIndex = 17;
            // 
            // bizIdTextBox
            // 
            this.bizIdTextBox.Location = new System.Drawing.Point(15, 120);
            this.bizIdTextBox.Multiline = true;
            this.bizIdTextBox.Name = "bizIdTextBox";
            this.bizIdTextBox.Size = new System.Drawing.Size(336, 117);
            this.bizIdTextBox.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Enter the HTTP BizID for your OrderMotion account:";
            // 
            // OrderMotionAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bizIdTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.emailAccountControl);
            this.Name = "OrderMotionAccountSettingsControl";
            this.Size = new System.Drawing.Size(452, 321);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Stores.Platforms.OrderMotion.OrderMotionEmailAccountControl emailAccountControl;
        private System.Windows.Forms.TextBox bizIdTextBox;
        private System.Windows.Forms.Label label2;
    }
}
