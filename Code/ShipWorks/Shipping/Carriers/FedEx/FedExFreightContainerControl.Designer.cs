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
            this.fedExExpressFreightControl = new ShipWorks.Shipping.Carriers.FedEx.FedExExpressFreightControl();
            this.panelLtlFreight = new System.Windows.Forms.Panel();
            this.groupFreightPackages = new System.Windows.Forms.GroupBox();
            this.fedExPackageFreightDetailControl = new ShipWorks.Shipping.Carriers.FedEx.FedExPackageFreightDetailControl();
            this.fedExLtlFreightControl = new ShipWorks.Shipping.Carriers.FedEx.FedExLtlFreightControl();
            this.panelLtlFreight.SuspendLayout();
            this.groupFreightPackages.SuspendLayout();
            this.SuspendLayout();
            // 
            // fedExExpressFreightControl
            // 
            this.fedExExpressFreightControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fedExExpressFreightControl.BackColor = System.Drawing.SystemColors.Window;
            this.fedExExpressFreightControl.Location = new System.Drawing.Point(2, 2);
            this.fedExExpressFreightControl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.fedExExpressFreightControl.Name = "fedExExpressFreightControl";
            this.fedExExpressFreightControl.Size = new System.Drawing.Size(419, 50);
            this.fedExExpressFreightControl.TabIndex = 0;
            // 
            // panelLtlFreight
            // 
            this.panelLtlFreight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLtlFreight.Controls.Add(this.groupFreightPackages);
            this.panelLtlFreight.Controls.Add(this.fedExLtlFreightControl);
            this.panelLtlFreight.Location = new System.Drawing.Point(4, 66);
            this.panelLtlFreight.Name = "panelLtlFreight";
            this.panelLtlFreight.Size = new System.Drawing.Size(367, 494);
            this.panelLtlFreight.TabIndex = 2;
            // 
            // groupFreightPackages
            // 
            this.groupFreightPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupFreightPackages.Controls.Add(this.fedExPackageFreightDetailControl);
            this.groupFreightPackages.Location = new System.Drawing.Point(3, 323);
            this.groupFreightPackages.Name = "groupFreightPackages";
            this.groupFreightPackages.Size = new System.Drawing.Size(350, 166);
            this.groupFreightPackages.TabIndex = 3;
            this.groupFreightPackages.TabStop = false;
            this.groupFreightPackages.Text = "Freight Packages";
            // 
            // fedExPackageFreightDetailControl
            // 
            this.fedExPackageFreightDetailControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fedExPackageFreightDetailControl.BackColor = System.Drawing.Color.Transparent;
            this.fedExPackageFreightDetailControl.Location = new System.Drawing.Point(6, 19);
            this.fedExPackageFreightDetailControl.Name = "fedExPackageFreightDetailControl";
            this.fedExPackageFreightDetailControl.Size = new System.Drawing.Size(334, 128);
            this.fedExPackageFreightDetailControl.TabIndex = 0;
            // 
            // fedExLtlFreightControl
            // 
            this.fedExLtlFreightControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fedExLtlFreightControl.BackColor = System.Drawing.SystemColors.Window;
            this.fedExLtlFreightControl.Location = new System.Drawing.Point(0, 0);
            this.fedExLtlFreightControl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.fedExLtlFreightControl.Name = "fedExLtlFreightControl";
            this.fedExLtlFreightControl.Size = new System.Drawing.Size(353, 319);
            this.fedExLtlFreightControl.TabIndex = 2;
            // 
            // fedExFreightContainerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.panelLtlFreight);
            this.Controls.Add(this.fedExExpressFreightControl);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.Name = "fedExFreightContainerControl";
            this.Size = new System.Drawing.Size(424, 569);
            this.panelLtlFreight.ResumeLayout(false);
            this.groupFreightPackages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FedExExpressFreightControl fedExExpressFreightControl;
        private System.Windows.Forms.Panel panelLtlFreight;
        private System.Windows.Forms.GroupBox groupFreightPackages;
        private FedExPackageFreightDetailControl fedExPackageFreightDetailControl;
        private FedExLtlFreightControl fedExLtlFreightControl;
    }
}
