namespace ShipWorks.Stores.Management
{
    partial class OnlineUpdateShipmentUpdateActionControl
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
            this.createTask.Checked = true;
            this.createTask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.createTask.Location = new System.Drawing.Point(3, 3);
            this.createTask.Name = "createTask";
            this.createTask.Size = new System.Drawing.Size(269, 17);
            this.createTask.TabIndex = 1;
            this.createTask.Text = "Update my online orders with the shipment details.";
            this.createTask.UseVisualStyleBackColor = true;
            // 
            // OnlineUpdateShipmentUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.createTask);
            this.Name = "OnlineUpdateShipmentUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox createTask;
    }
}
