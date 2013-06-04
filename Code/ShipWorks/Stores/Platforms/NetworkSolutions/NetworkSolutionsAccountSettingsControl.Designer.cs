namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    partial class NetworkSolutionsAccountSettingsControl
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
            this.manageTokenControl = new ShipWorks.Stores.Platforms.NetworkSolutions.NetworkSolutionsManageTokenControl();
            this.SuspendLayout();
            // 
            // manageTokenControl
            // 
            this.manageTokenControl.Location = new System.Drawing.Point(3, 3);
            this.manageTokenControl.Name = "manageTokenControl";
            this.manageTokenControl.Size = new System.Drawing.Size(524, 106);
            this.manageTokenControl.TabIndex = 0;
            // 
            // NetworkSolutionsAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.manageTokenControl);
            this.Name = "NetworkSolutionsAccountSettingsControl";
            this.Size = new System.Drawing.Size(553, 130);
            this.ResumeLayout(false);

        }

        #endregion

        private NetworkSolutionsManageTokenControl manageTokenControl;

    }
}
