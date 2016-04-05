namespace ShipWorks.Stores.Management
{
    partial class StoreConfirmDeleteDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.delete = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.pictureBoxStore = new System.Windows.Forms.PictureBox();
            this.labelStore = new System.Windows.Forms.Label();
            this.checkBoxConfirm = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStore)).BeginInit();
            this.SuspendLayout();
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Enabled = false;
            this.delete.Location = new System.Drawing.Point(301, 78);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(75, 23);
            this.delete.TabIndex = 0;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(382, 78);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // pictureBoxStore
            // 
            this.pictureBoxStore.Image = global::ShipWorks.Properties.Resources.school_delete32;
            this.pictureBoxStore.Location = new System.Drawing.Point(12, 11);
            this.pictureBoxStore.Name = "pictureBoxStore";
            this.pictureBoxStore.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxStore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxStore.TabIndex = 2;
            this.pictureBoxStore.TabStop = false;
            // 
            // labelStore
            // 
            this.labelStore.AutoSize = true;
            this.labelStore.Location = new System.Drawing.Point(57, 10);
            this.labelStore.Name = "labelStore";
            this.labelStore.Size = new System.Drawing.Size(195, 13);
            this.labelStore.TabIndex = 3;
            this.labelStore.Text = "Delete store \'{0}\' and all of its content?";
            // 
            // checkBoxConfirm
            // 
            this.checkBoxConfirm.Location = new System.Drawing.Point(58, 29);
            this.checkBoxConfirm.Name = "checkBoxConfirm";
            this.checkBoxConfirm.Size = new System.Drawing.Size(401, 36);
            this.checkBoxConfirm.TabIndex = 4;
            this.checkBoxConfirm.Text = "I understand this permanently deletes all data for the store, including customers" +
    ", orders, and shipments.";
            this.checkBoxConfirm.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.checkBoxConfirm.UseVisualStyleBackColor = true;
            this.checkBoxConfirm.CheckedChanged += new System.EventHandler(this.OnConfirm);
            // 
            // StoreConfirmDeleteDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(469, 113);
            this.Controls.Add(this.checkBoxConfirm);
            this.Controls.Add(this.labelStore);
            this.Controls.Add(this.pictureBoxStore);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.delete);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StoreConfirmDeleteDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete Store";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStore)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.PictureBox pictureBoxStore;
        private System.Windows.Forms.Label labelStore;
        private System.Windows.Forms.CheckBox checkBoxConfirm;
    }
}