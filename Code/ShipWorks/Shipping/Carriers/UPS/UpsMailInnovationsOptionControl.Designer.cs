namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    partial class UpsMailInnovationsOptionsControl
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
            this.mailInnovations = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // mailInnovations
            // 
            this.mailInnovations.AutoSize = true;
            this.mailInnovations.Location = new System.Drawing.Point(3, 3);
            this.mailInnovations.Name = "mailInnovations";
            this.mailInnovations.Size = new System.Drawing.Size(128, 17);
            this.mailInnovations.TabIndex = 0;
            this.mailInnovations.Text = "UPS Mail Innovations";
            this.mailInnovations.UseVisualStyleBackColor = true;
            // 
            // WorldShipContractServicesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mailInnovations);
            this.Name = "WorldShipContractServicesControl";
            this.Size = new System.Drawing.Size(283, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox mailInnovations;
    }
}
