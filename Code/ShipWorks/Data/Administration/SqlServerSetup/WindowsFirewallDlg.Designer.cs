namespace ShipWorks.Data.Administration.SqlServerSetup
{
    partial class WindowsFirewallDlg
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
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.close = new System.Windows.Forms.Button();
            this.updateWindowsFirewall = new ShipWorks.UI.Controls.ShieldButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.SuspendLayout();
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(62, 45);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(420, 13);
            this.label27.TabIndex = 13;
            this.label27.Text = "Only the minimum changes required for running ShipWorks will be made to the firew" +
    "all.";
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(62, 10);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(394, 32);
            this.label28.TabIndex = 11;
            this.label28.Text = "Windows Firewall needs to be configured to allow ShipWorks to work on multiple co" +
    "mputers.";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::ShipWorks.Properties.Resources.firewall;
            this.pictureBox6.Location = new System.Drawing.Point(6, 10);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(48, 48);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 10;
            this.pictureBox6.TabStop = false;
            // 
            // close
            // 
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(407, 145);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 14;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // updateWindowsFirewall
            // 
            this.updateWindowsFirewall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.updateWindowsFirewall.Location = new System.Drawing.Point(64, 82);
            this.updateWindowsFirewall.Name = "updateWindowsFirewall";
            this.updateWindowsFirewall.Size = new System.Drawing.Size(168, 23);
            this.updateWindowsFirewall.TabIndex = 12;
            this.updateWindowsFirewall.Text = "Update Windows Firewall";
            this.updateWindowsFirewall.UseVisualStyleBackColor = true;
            this.updateWindowsFirewall.Click += new System.EventHandler(this.OnUpdateWindowsFirewall);
            // 
            // WindowsFirewallDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(502, 180);
            this.Controls.Add(this.close);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.updateWindowsFirewall);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.pictureBox6);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WindowsFirewallDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windows Firewall";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label27;
        private ShipWorks.UI.Controls.ShieldButton updateWindowsFirewall;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Button close;
    }
}