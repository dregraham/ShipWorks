namespace ShipWorks.ApplicationCore.Settings.ResourceCleanup
{
    partial class ResourceCleanupDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceCleanupDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.deleteDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.statusPicture = new System.Windows.Forms.PictureBox();
            this.statusText = new System.Windows.Forms.Label();
            this.estimateLink = new ShipWorks.UI.Controls.LinkControl();
            ((System.ComponentModel.ISupportInitialize)(this.statusPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Standard (non-thermal) labels for shipments older than the date entered below wil" +
                "l be deleted.";
            // 
            // deleteDate
            // 
            this.deleteDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.deleteDate.Location = new System.Drawing.Point(158, 49);
            this.deleteDate.Name = "deleteDate";
            this.deleteDate.Size = new System.Drawing.Size(77, 20);
            this.deleteDate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Delete older than:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(109, 113);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&Delete";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OnDeleteClick);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(190, 113);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // statusPicture
            // 
            this.statusPicture.Image = ((System.Drawing.Image)(resources.GetObject("statusPicture.Image")));
            this.statusPicture.Location = new System.Drawing.Point(44, 71);
            this.statusPicture.Name = "statusPicture";
            this.statusPicture.Size = new System.Drawing.Size(16, 16);
            this.statusPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.statusPicture.TabIndex = 16;
            this.statusPicture.TabStop = false;
            this.statusPicture.Visible = false;
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.ForeColor = System.Drawing.Color.DimGray;
            this.statusText.Location = new System.Drawing.Point(61, 72);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(64, 13);
            this.statusText.TabIndex = 17;
            this.statusText.Text = "Estimating...";
            this.statusText.Visible = false;
            // 
            // estimateLink
            // 
            this.estimateLink.AutoSize = true;
            this.estimateLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.estimateLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.estimateLink.ForeColor = System.Drawing.Color.Blue;
            this.estimateLink.Location = new System.Drawing.Point(241, 52);
            this.estimateLink.Name = "estimateLink";
            this.estimateLink.Size = new System.Drawing.Size(80, 13);
            this.estimateLink.TabIndex = 18;
            this.estimateLink.Text = "estimate usage";
            this.estimateLink.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnEstimate);
            // 
            // DateSelectionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(374, 148);
            this.Controls.Add(this.estimateLink);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.statusPicture);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.deleteDate);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DateSelectionDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Purge Standard Labels";
            ((System.ComponentModel.ISupportInitialize)(this.statusPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker deleteDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.PictureBox statusPicture;
        private System.Windows.Forms.Label statusText;
        private UI.Controls.LinkControl estimateLink;
    }
}