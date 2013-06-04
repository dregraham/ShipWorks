namespace ShipWorks.Templates.Distribution
{
    partial class InternalTestVersionDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelInstalled = new System.Windows.Forms.Label();
            this.installedVersion = new System.Windows.Forms.TextBox();
            this.swVersion = new System.Windows.Forms.TextBox();
            this.labelShipWorks = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(107, 78);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(188, 78);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelInstalled
            // 
            this.labelInstalled.AutoSize = true;
            this.labelInstalled.Location = new System.Drawing.Point(12, 13);
            this.labelInstalled.Name = "labelInstalled";
            this.labelInstalled.Size = new System.Drawing.Size(140, 13);
            this.labelInstalled.TabIndex = 2;
            this.labelInstalled.Text = "Installed templates version:";
            // 
            // installedVersion
            // 
            this.installedVersion.Location = new System.Drawing.Point(158, 10);
            this.installedVersion.Name = "installedVersion";
            this.installedVersion.Size = new System.Drawing.Size(105, 21);
            this.installedVersion.TabIndex = 4;
            // 
            // swVersion
            // 
            this.swVersion.Location = new System.Drawing.Point(158, 37);
            this.swVersion.Name = "swVersion";
            this.swVersion.Size = new System.Drawing.Size(105, 21);
            this.swVersion.TabIndex = 5;
            // 
            // labelShipWorks
            // 
            this.labelShipWorks.AutoSize = true;
            this.labelShipWorks.Location = new System.Drawing.Point(12, 41);
            this.labelShipWorks.Name = "labelShipWorks";
            this.labelShipWorks.Size = new System.Drawing.Size(144, 13);
            this.labelShipWorks.TabIndex = 6;
            this.labelShipWorks.Text = "Override ShipWorks Version:";
            // 
            // InternalTestVersionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(275, 113);
            this.Controls.Add(this.labelShipWorks);
            this.Controls.Add(this.swVersion);
            this.Controls.Add(this.installedVersion);
            this.Controls.Add(this.labelInstalled);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InternalTestVersionDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template Distribution Testing";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelInstalled;
        private System.Windows.Forms.TextBox installedVersion;
        private System.Windows.Forms.TextBox swVersion;
        private System.Windows.Forms.Label labelShipWorks;
    }
}