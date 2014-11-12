namespace ShipWorks.Stores.Platforms.Miva
{
    partial class MivaOrderStatusControl
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
            this.sebenzaOrderStatusEmail = new System.Windows.Forms.CheckBox();
            this.updateStrategy = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sebenzaOrderStatusEmail
            // 
            this.sebenzaOrderStatusEmail.AutoSize = true;
            this.sebenzaOrderStatusEmail.Location = new System.Drawing.Point(102, 55);
            this.sebenzaOrderStatusEmail.Name = "sebenzaOrderStatusEmail";
            this.sebenzaOrderStatusEmail.Size = new System.Drawing.Size(287, 17);
            this.sebenzaOrderStatusEmail.TabIndex = 30;
            this.sebenzaOrderStatusEmail.Text = "Sebenza module should send emails on status changes";
            this.sebenzaOrderStatusEmail.UseVisualStyleBackColor = true;
            // 
            // updateStrategy
            // 
            this.updateStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.updateStrategy.FormattingEnabled = true;
            this.updateStrategy.Location = new System.Drawing.Point(102, 28);
            this.updateStrategy.Name = "updateStrategy";
            this.updateStrategy.Size = new System.Drawing.Size(236, 21);
            this.updateStrategy.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Update using:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(380, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "ShipWorks can update online order statuses, depending on system capabilities.";
            // 
            // MivaOrderStatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sebenzaOrderStatusEmail);
            this.Controls.Add(this.updateStrategy);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "MivaOrderStatusControl";
            this.Size = new System.Drawing.Size(404, 78);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox sebenzaOrderStatusEmail;
        private System.Windows.Forms.ComboBox updateStrategy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}
