namespace ShipWorks.Stores.Content
{
    partial class AddCustomerDlg
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
            this.imageUser = new System.Windows.Forms.PictureBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.shipBillControl = new ShipWorks.Stores.Content.Controls.ShipBillAddressControl();
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).BeginInit();
            this.SuspendLayout();
            // 
            // imageUser
            // 
            this.imageUser.Image = global::ShipWorks.Properties.Resources.user1;
            this.imageUser.Location = new System.Drawing.Point(9, 9);
            this.imageUser.Name = "imageUser";
            this.imageUser.Size = new System.Drawing.Size(48, 48);
            this.imageUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageUser.TabIndex = 12;
            this.imageUser.TabStop = false;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(559, 465);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 14;
            this.ok.Text = "Create";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(640, 465);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 15;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // shipBillControl
            // 
            this.shipBillControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.shipBillControl.Location = new System.Drawing.Point(65, 9);
            this.shipBillControl.Name = "shipBillControl";
            this.shipBillControl.Size = new System.Drawing.Size(650, 450);
            this.shipBillControl.TabIndex = 13;
            // 
            // AddCustomerDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(727, 496);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.shipBillControl);
            this.Controls.Add(this.imageUser);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCustomerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Customer";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imageUser;
        private ShipWorks.Stores.Content.Controls.ShipBillAddressControl shipBillControl;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
    }
}