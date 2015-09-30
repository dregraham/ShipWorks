namespace ShipWorks.Shipping.UI.ShippingPanel
{
    partial class ShippingPanel
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
            this.shipmentPanelelementHost = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.shipmentPanelelementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shipmentPanelelementHost.Location = new System.Drawing.Point(0, 0);
            this.shipmentPanelelementHost.Name = "elementHost1";
            this.shipmentPanelelementHost.Size = new System.Drawing.Size(381, 420);
            this.shipmentPanelelementHost.TabIndex = 0;
            this.shipmentPanelelementHost.Text = "elementHost1";
            this.shipmentPanelelementHost.Child = null;
            // 
            // ShipmentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.shipmentPanelelementHost);
            this.Name = "ShipmentPanel";
            this.Size = new System.Drawing.Size(381, 420);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost shipmentPanelelementHost;
    }
}
