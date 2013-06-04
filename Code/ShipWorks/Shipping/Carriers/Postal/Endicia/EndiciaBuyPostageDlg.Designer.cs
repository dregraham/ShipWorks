namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaBuyPostageDlg
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
            this.cancel = new System.Windows.Forms.Button();
            this.purchase = new System.Windows.Forms.Button();
            this.postage = new ShipWorks.UI.Controls.MoneyTextBox();
            this.labelPostage = new System.Windows.Forms.Label();
            this.current = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(154, 65);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // purchase
            // 
            this.purchase.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.purchase.Location = new System.Drawing.Point(73, 65);
            this.purchase.Name = "purchase";
            this.purchase.Size = new System.Drawing.Size(75, 23);
            this.purchase.TabIndex = 4;
            this.purchase.Text = "Purchase";
            this.purchase.UseVisualStyleBackColor = true;
            this.purchase.Click += new System.EventHandler(this.OnPurchase);
            // 
            // postage
            // 
            this.postage.Amount = new decimal(new int[] {
            2500,
            0,
            0,
            131072});
            this.postage.Location = new System.Drawing.Point(103, 29);
            this.postage.Name = "postage";
            this.postage.Size = new System.Drawing.Size(75, 21);
            this.postage.TabIndex = 3;
            this.postage.Text = "$25.00";
            // 
            // labelPostage
            // 
            this.labelPostage.AutoSize = true;
            this.labelPostage.Location = new System.Drawing.Point(4, 32);
            this.labelPostage.Name = "labelPostage";
            this.labelPostage.Size = new System.Drawing.Size(94, 13);
            this.labelPostage.TabIndex = 2;
            this.labelPostage.Text = "Purchase amount:";
            // 
            // current
            // 
            this.current.AutoSize = true;
            this.current.Location = new System.Drawing.Point(100, 10);
            this.current.Name = "current";
            this.current.Size = new System.Drawing.Size(62, 13);
            this.current.TabIndex = 1;
            this.current.Text = "Checking...";
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Location = new System.Drawing.Point(12, 9);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(88, 13);
            this.labelCurrent.TabIndex = 0;
            this.labelCurrent.Text = "Current balance:";
            // 
            // EndiciaBuyPostageDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(241, 100);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.purchase);
            this.Controls.Add(this.postage);
            this.Controls.Add(this.labelPostage);
            this.Controls.Add(this.current);
            this.Controls.Add(this.labelCurrent);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EndiciaBuyPostageDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Buy Postage";
            this.Shown += new System.EventHandler(this.OnShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button purchase;
        private ShipWorks.UI.Controls.MoneyTextBox postage;
        private System.Windows.Forms.Label labelPostage;
        private System.Windows.Forms.Label current;
        private System.Windows.Forms.Label labelCurrent;
    }
}