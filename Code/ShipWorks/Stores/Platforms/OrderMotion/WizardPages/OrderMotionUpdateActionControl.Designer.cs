namespace ShipWorks.Stores.Platforms.OrderMotion.WizardPages
{
    partial class OrderMotionOnlineUpdateActionControl
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
            this.SuspendLayout();
            // 
            // createTask
            // 
            this.createTask.AutoSize = true;
            this.createTask.Location = new System.Drawing.Point(4, 4);
            this.createTask.Name = "createTask";
            this.createTask.Size = new System.Drawing.Size(208, 17);
            this.createTask.TabIndex = 0;
            this.createTask.Text = "Upload the shipment tracking number.";
            this.createTask.UseVisualStyleBackColor = true;
            // 
            // OrderMotionOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.createTask);
            this.Name = "OrderMotionOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(324, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox createTask;
    }
}
