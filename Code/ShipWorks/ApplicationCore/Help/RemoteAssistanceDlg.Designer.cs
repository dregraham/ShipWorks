namespace ShipWorks.ApplicationCore.Help
{
    partial class RemoteAssistanceDlg
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelPin = new System.Windows.Forms.Label();
            this.connect = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.pinBox = new ShipWorks.UI.Controls.NumericTextBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.remote_assist32;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(32, 32);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(64, 12);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(336, 32);
            this.labelInfo.TabIndex = 1;
            this.labelInfo.Text = "Please enter the 6-digit PIN code provided by your Interapptive support represent" +
                "ative to start a remote screen sharing session.";
            // 
            // labelPin
            // 
            this.labelPin.AutoSize = true;
            this.labelPin.Location = new System.Drawing.Point(76, 51);
            this.labelPin.Name = "labelPin";
            this.labelPin.Size = new System.Drawing.Size(87, 13);
            this.labelPin.TabIndex = 2;
            this.labelPin.Text = "6-digit PIN code:";
            // 
            // connect
            // 
            this.connect.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.connect.Location = new System.Drawing.Point(224, 88);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 4;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.OnConnect);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(305, 88);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // pinBox
            // 
            this.pinBox.Location = new System.Drawing.Point(169, 48);
            this.pinBox.MaxLength = 6;
            this.pinBox.Name = "pinBox";
            this.pinBox.Size = new System.Drawing.Size(100, 21);
            this.pinBox.TabIndex = 6;
            // 
            // RemoteAssistanceDlg
            // 
            this.AcceptButton = this.connect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(392, 123);
            this.Controls.Add(this.pinBox);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.labelPin);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.pictureBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RemoteAssistanceDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remote Assistance";
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelPin;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button cancel;
        private ShipWorks.UI.Controls.NumericTextBox pinBox;
    }
}