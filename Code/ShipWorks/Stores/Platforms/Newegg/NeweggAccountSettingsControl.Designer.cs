namespace ShipWorks.Stores.Platforms.Newegg
{
    partial class NeweggAccountSettingsControl
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
            this.storeCredentialsControl = new ShipWorks.Stores.Platforms.Newegg.NeweggStoreCredentialsControl();
            this.SuspendLayout();
            // 
            // storeSettingsControl
            // 
            this.storeCredentialsControl.Location = new System.Drawing.Point(4, 4);
            this.storeCredentialsControl.Name = "storeSettingsControl";
            this.storeCredentialsControl.SecretKey = "";
            this.storeCredentialsControl.SellerId = "";
            this.storeCredentialsControl.Size = new System.Drawing.Size(378, 154);
            this.storeCredentialsControl.TabIndex = 0;
            // 
            // NeweggAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storeCredentialsControl);
            this.Name = "NeweggAccountSettingsControl";
            this.Size = new System.Drawing.Size(393, 167);
            this.ResumeLayout(false);

        }

        #endregion

        private NeweggStoreCredentialsControl storeCredentialsControl;




    }
}
