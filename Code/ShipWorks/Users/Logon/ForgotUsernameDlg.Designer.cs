namespace ShipWorks.Users.Logon
{
    partial class ForgotUsernameDlg
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
            this.labelEmail = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.send = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelEmail.Location = new System.Drawing.Point(66, 70);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(77, 13);
            this.labelEmail.TabIndex = 1;
            this.labelEmail.Text = "Email Address:";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(68, 87);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(391, 21);
            this.textBox.TabIndex = 2;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.dude3_question_48;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(48, 48);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(270, 122);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(108, 23);
            this.send.TabIndex = 4;
            this.send.Text = "Send Username";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.OnSendUsername);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(384, 122);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelTitle.Location = new System.Drawing.Point(66, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(105, 13);
            this.labelTitle.TabIndex = 6;
            this.labelTitle.Text = "Forgot Username";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(66, 33);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(404, 34);
            this.labelInfo.TabIndex = 8;
            this.labelInfo.Text = "Enter the email address associated with your ShipWorks logon and your username wi" +
                "ll be sent to you.";
            // 
            // ForgotUsernameDlg
            // 
            this.AcceptButton = this.send;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(482, 157);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.send);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.labelEmail);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ForgotUsernameDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Forgot Username";
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelInfo;
    }
}