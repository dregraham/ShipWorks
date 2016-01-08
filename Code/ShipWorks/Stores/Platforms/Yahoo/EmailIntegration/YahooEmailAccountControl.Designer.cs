namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    partial class YahooEmailAccountControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.changeAccount = new System.Windows.Forms.Button();
            this.editAccount = new System.Windows.Forms.Button();
            this.accountDescription = new System.Windows.Forms.TextBox();
            this.labelAlreadySetup = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // changeAccount
            // 
            this.changeAccount.Location = new System.Drawing.Point(227, 46);
            this.changeAccount.Name = "changeAccount";
            this.changeAccount.Size = new System.Drawing.Size(75, 23);
            this.changeAccount.TabIndex = 7;
            this.changeAccount.Text = "Change...";
            this.changeAccount.UseVisualStyleBackColor = true;
            this.changeAccount.Click += new System.EventHandler(this.OnChange);
            // 
            // editAccount
            // 
            this.editAccount.Location = new System.Drawing.Point(146, 46);
            this.editAccount.Name = "editAccount";
            this.editAccount.Size = new System.Drawing.Size(75, 23);
            this.editAccount.TabIndex = 6;
            this.editAccount.Text = "Edit...";
            this.editAccount.UseVisualStyleBackColor = true;
            this.editAccount.Click += new System.EventHandler(this.OnEdit);
            // 
            // accountDescription
            // 
            this.accountDescription.Location = new System.Drawing.Point(6, 21);
            this.accountDescription.Name = "accountDescription";
            this.accountDescription.ReadOnly = true;
            this.accountDescription.Size = new System.Drawing.Size(296, 21);
            this.accountDescription.TabIndex = 5;
            // 
            // labelAlreadySetup
            // 
            this.labelAlreadySetup.AutoSize = true;
            this.labelAlreadySetup.Location = new System.Drawing.Point(3, 5);
            this.labelAlreadySetup.Name = "labelAlreadySetup";
            this.labelAlreadySetup.Size = new System.Drawing.Size(304, 13);
            this.labelAlreadySetup.TabIndex = 4;
            this.labelAlreadySetup.Text = "The following email account will be checked for Yahoo! orders:";
            // 
            // YahooEmailAccountControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.changeAccount);
            this.Controls.Add(this.editAccount);
            this.Controls.Add(this.accountDescription);
            this.Controls.Add(this.labelAlreadySetup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "YahooEmailAccountControl";
            this.Size = new System.Drawing.Size(309, 76);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button changeAccount;
        private System.Windows.Forms.Button editAccount;
        private System.Windows.Forms.TextBox accountDescription;
        private System.Windows.Forms.Label labelAlreadySetup;
    }
}
