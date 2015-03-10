namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    partial class VolusionOnlineUpdateActionControl
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
            this.createTask = new System.Windows.Forms.CheckBox();
            this.sendEmail = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // createTask
            // 
            this.createTask.AutoSize = true;
            this.createTask.Location = new System.Drawing.Point(4, 4);
            this.createTask.Name = "createTask";
            this.createTask.Size = new System.Drawing.Size(247, 17);
            this.createTask.TabIndex = 0;
            this.createTask.Text = "Update the online order with shipment details.";
            this.createTask.UseVisualStyleBackColor = true;
            this.createTask.CheckedChanged += new System.EventHandler(this.OnCreateTaskChecked);
            // 
            // sendEmail
            // 
            this.sendEmail.AutoSize = true;
            this.sendEmail.Enabled = false;
            this.sendEmail.Location = new System.Drawing.Point(40, 27);
            this.sendEmail.Name = "sendEmail";
            this.sendEmail.Size = new System.Drawing.Size(275, 17);
            this.sendEmail.TabIndex = 1;
            this.sendEmail.Text = "Have Volusion send Shipped emails to the customer.";
            this.sendEmail.UseVisualStyleBackColor = true;
            // 
            // VolusionOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sendEmail);
            this.Controls.Add(this.createTask);
            this.Name = "VolusionOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(324, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox createTask;
        private System.Windows.Forms.CheckBox sendEmail;
    }
}
