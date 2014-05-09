namespace ShipWorks.Shipping.ShipSense.Settings
{
    partial class ShipSenseConfirmationDlg
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
            this.confirmationText = new System.Windows.Forms.Label();
            this.rebuildKnowledgebase = new System.Windows.Forms.CheckBox();
            this.noButton = new System.Windows.Forms.Button();
            this.yesButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // confirmationText
            // 
            this.confirmationText.Location = new System.Drawing.Point(53, 13);
            this.confirmationText.Name = "confirmationText";
            this.confirmationText.Size = new System.Drawing.Size(359, 15);
            this.confirmationText.TabIndex = 0;
            this.confirmationText.Text = "Confirmation text";
            // 
            // rebuildKnowledgebase
            // 
            this.rebuildKnowledgebase.AutoSize = true;
            this.rebuildKnowledgebase.Location = new System.Drawing.Point(56, 31);
            this.rebuildKnowledgebase.Name = "rebuildKnowledgebase";
            this.rebuildKnowledgebase.Size = new System.Drawing.Size(248, 17);
            this.rebuildKnowledgebase.TabIndex = 1;
            this.rebuildKnowledgebase.Text = "Reload recent shipment history into ShipSense";
            this.rebuildKnowledgebase.UseVisualStyleBackColor = true;
            // 
            // noButton
            // 
            this.noButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.noButton.Location = new System.Drawing.Point(326, 63);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(86, 23);
            this.noButton.TabIndex = 2;
            this.noButton.Text = "No";
            this.noButton.UseVisualStyleBackColor = true;
            // 
            // yesButton
            // 
            this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.yesButton.Location = new System.Drawing.Point(234, 63);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(86, 23);
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = "Yes";
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.warning32;
            this.pictureBox1.InitialImage = global::ShipWorks.Properties.Resources.warning32;
            this.pictureBox1.Location = new System.Drawing.Point(12, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 32);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // ShipSenseConfirmationDlg
            // 
            this.AcceptButton = this.noButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.noButton;
            this.ClientSize = new System.Drawing.Size(424, 98);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.rebuildKnowledgebase);
            this.Controls.Add(this.confirmationText);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShipSenseConfirmationDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modify ShipSense knowledge base";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label confirmationText;
        private System.Windows.Forms.CheckBox rebuildKnowledgebase;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}