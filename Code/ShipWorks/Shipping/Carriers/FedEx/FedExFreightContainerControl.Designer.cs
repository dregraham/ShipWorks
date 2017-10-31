namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExFreightContainerControl
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
            this.fedExPackageFreightDetailControl = new ShipWorks.Shipping.Carriers.FedEx.FedExPackageFreightDetailControl();
            this.fedExExpressFreightControl = new ShipWorks.Shipping.Carriers.FedEx.FedExExpressFreightControl();
            this.SuspendLayout();
            // 
            // fedExPackageFreightDetailControl
            // 
            this.fedExPackageFreightDetailControl.AutoSize = true;
            this.fedExPackageFreightDetailControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fedExPackageFreightDetailControl.BackColor = System.Drawing.Color.Transparent;
            this.fedExPackageFreightDetailControl.Location = new System.Drawing.Point(2, 2);
            this.fedExPackageFreightDetailControl.Name = "fedExPackageFreightDetailControl";
            this.fedExPackageFreightDetailControl.Size = new System.Drawing.Size(334, 128);
            this.fedExPackageFreightDetailControl.TabIndex = 1;
            // 
            // fedExExpressFreightControl
            // 
            this.fedExExpressFreightControl.BackColor = System.Drawing.SystemColors.Window;
            this.fedExExpressFreightControl.Location = new System.Drawing.Point(2, 136);
            this.fedExExpressFreightControl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.fedExExpressFreightControl.Name = "fedExExpressFreightControl";
            this.fedExExpressFreightControl.Size = new System.Drawing.Size(487, 50);
            this.fedExExpressFreightControl.TabIndex = 0;
            // 
            // FedExFreightContainerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.fedExPackageFreightDetailControl);
            this.Controls.Add(this.fedExExpressFreightControl);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.Name = "FedExFreightContainerControl";
            this.Size = new System.Drawing.Size(602, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FedExExpressFreightControl fedExExpressFreightControl;
        private FedExPackageFreightDetailControl fedExPackageFreightDetailControl;
    }
}
