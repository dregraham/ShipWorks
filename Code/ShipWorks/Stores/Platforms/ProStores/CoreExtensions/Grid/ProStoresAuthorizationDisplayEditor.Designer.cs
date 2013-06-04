namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Grid
{
    partial class ProStoresAuthorizationDisplayEditor
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
            this.showIcon = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // showIcon
            // 
            this.showIcon.AutoSize = true;
            this.showIcon.Location = new System.Drawing.Point(6, 273);
            this.showIcon.Name = "showIcon";
            this.showIcon.Size = new System.Drawing.Size(128, 17);
            this.showIcon.TabIndex = 8;
            this.showIcon.Text = "Show authorized icon";
            this.showIcon.UseVisualStyleBackColor = true;
            // 
            // ProStoresAuthorizationDisplayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.showIcon);
            this.Name = "ProStoresAuthorizationDisplayEditor";
            this.Size = new System.Drawing.Size(220, 304);
            this.Controls.SetChildIndex(this.showIcon, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showIcon;
    }
}
