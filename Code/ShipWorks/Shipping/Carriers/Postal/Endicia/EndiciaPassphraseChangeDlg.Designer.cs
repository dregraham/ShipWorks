namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaPassphraseChangeDlg
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
            this.labelCurrent = new System.Windows.Forms.Label();
            this.current = new System.Windows.Forms.TextBox();
            this.newPassphrase = new System.Windows.Forms.TextBox();
            this.labelNew = new System.Windows.Forms.Label();
            this.confirmation = new System.Windows.Forms.TextBox();
            this.labelConfirm = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Location = new System.Drawing.Point(12, 14);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(106, 13);
            this.labelCurrent.TabIndex = 0;
            this.labelCurrent.Text = "Current Passphrase:";
            // 
            // current
            // 
            this.current.Location = new System.Drawing.Point(124, 11);
            this.current.Name = "current";
            this.current.Size = new System.Drawing.Size(174, 21);
            this.current.TabIndex = 1;
            // 
            // newPassphrase
            // 
            this.newPassphrase.Location = new System.Drawing.Point(124, 56);
            this.newPassphrase.Name = "newPassphrase";
            this.newPassphrase.Size = new System.Drawing.Size(174, 21);
            this.newPassphrase.TabIndex = 3;
            // 
            // labelNew
            // 
            this.labelNew.AutoSize = true;
            this.labelNew.Location = new System.Drawing.Point(28, 59);
            this.labelNew.Name = "labelNew";
            this.labelNew.Size = new System.Drawing.Size(90, 13);
            this.labelNew.TabIndex = 2;
            this.labelNew.Text = "New Passphrase:";
            // 
            // confirmation
            // 
            this.confirmation.Location = new System.Drawing.Point(124, 83);
            this.confirmation.Name = "confirmation";
            this.confirmation.Size = new System.Drawing.Size(174, 21);
            this.confirmation.TabIndex = 5;
            // 
            // labelConfirm
            // 
            this.labelConfirm.AutoSize = true;
            this.labelConfirm.Location = new System.Drawing.Point(70, 86);
            this.labelConfirm.Name = "labelConfirm";
            this.labelConfirm.Size = new System.Drawing.Size(48, 13);
            this.labelConfirm.TabIndex = 4;
            this.labelConfirm.Text = "Confirm:";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(143, 117);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 6;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.Location = new System.Drawing.Point(224, 117);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // EndiciaPassphraseChangeDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(311, 152);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.confirmation);
            this.Controls.Add(this.labelConfirm);
            this.Controls.Add(this.newPassphrase);
            this.Controls.Add(this.labelNew);
            this.Controls.Add(this.current);
            this.Controls.Add(this.labelCurrent);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EndiciaPassphraseChangeDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Endicia Passphrase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.TextBox current;
        private System.Windows.Forms.TextBox newPassphrase;
        private System.Windows.Forms.Label labelNew;
        private System.Windows.Forms.TextBox confirmation;
        private System.Windows.Forms.Label labelConfirm;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
    }
}