namespace ShipWorks.Stores.UI.Platforms.LemonStand
{
    partial class LemonStandOnlineUpdateActionControl
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
            this.setStatus = new System.Windows.Forms.CheckBox();
            this.status = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // setStatus
            // 
            this.setStatus.AutoSize = true;
            this.setStatus.Checked = true;
            this.setStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setStatus.Location = new System.Drawing.Point(13, 3);
            this.setStatus.Name = "setStatus";
            this.setStatus.Size = new System.Drawing.Size(170, 17);
            this.setStatus.TabIndex = 0;
            this.setStatus.Text = "Set the online order status to ";
            this.setStatus.UseVisualStyleBackColor = true;
            this.setStatus.CheckedChanged += new System.EventHandler(this.OnChangeSetOrderStatus);
            // 
            // status
            // 
            this.status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.status.FormattingEnabled = true;
            this.status.Location = new System.Drawing.Point(180, 1);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(200, 21);
            this.status.TabIndex = 1;
            // 
            // LemonStandOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.status);
            this.Controls.Add(this.setStatus);
            this.Name = "LemonStandOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox setStatus;
        private System.Windows.Forms.ComboBox status;
    }
}
