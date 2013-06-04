namespace ShipWorks.Stores.Platforms.ProStores
{
    partial class ProStoresAccountSettingsTokenControl
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
            this.mangeTokenControl = new ShipWorks.Stores.Platforms.ProStores.ProStoresTokenManageControl();
            this.SuspendLayout();
            // 
            // mangeTokenControl
            // 
            this.mangeTokenControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.mangeTokenControl.Location = new System.Drawing.Point(3, 3);
            this.mangeTokenControl.Name = "mangeTokenControl";
            this.mangeTokenControl.Size = new System.Drawing.Size(524, 109);
            this.mangeTokenControl.TabIndex = 0;
            // 
            // ProStoresAccountSettingsTokenControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mangeTokenControl);
            this.Name = "ProStoresAccountSettingsTokenControl";
            this.Size = new System.Drawing.Size(565, 122);
            this.ResumeLayout(false);

        }

        #endregion

        private ProStoresTokenManageControl mangeTokenControl;
    }
}
