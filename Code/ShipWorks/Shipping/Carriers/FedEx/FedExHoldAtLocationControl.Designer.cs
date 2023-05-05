namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExHoldAtLocationControl
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
            this.holdAtLocation = new System.Windows.Forms.CheckBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.searchLink = new System.Windows.Forms.LinkLabel();
            this.locationDetails = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.SuspendLayout();
            // 
            // holdAtLocation
            // 
            this.holdAtLocation.AutoSize = true;
            this.holdAtLocation.BackColor = System.Drawing.Color.White;
            this.holdAtLocation.Location = new System.Drawing.Point(4, 4);
            this.holdAtLocation.Name = "holdAtLocation";
            this.holdAtLocation.Size = new System.Drawing.Size(178, 17);
            this.holdAtLocation.TabIndex = 0;
            this.holdAtLocation.Text = "Hold shipment at location";
            this.holdAtLocation.UseVisualStyleBackColor = false;
            this.holdAtLocation.CheckedChanged += new System.EventHandler(this.OnHoldAtLocationChanged);
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.BackColor = System.Drawing.Color.White;
            this.locationLabel.Enabled = false;
            this.locationLabel.Location = new System.Drawing.Point(25, 36);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(51, 13);
            this.locationLabel.TabIndex = 2;
            this.locationLabel.Text = "Location:";
            // 
            // searchLink
            // 
            this.searchLink.AutoSize = true;
            this.searchLink.BackColor = System.Drawing.Color.White;
            this.searchLink.Enabled = false;
            this.searchLink.Location = new System.Drawing.Point(188, 5);
            this.searchLink.Name = "searchLink";
            this.searchLink.Size = new System.Drawing.Size(95, 13);
            this.searchLink.TabIndex = 3;
            this.searchLink.TabStop = true;
            this.searchLink.Text = "Select a location...";
            this.searchLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnSelectLocationClicked);
            // 
            // locationDetails
            // 
            this.locationDetails.Enabled = false;
            this.locationDetails.Location = new System.Drawing.Point(82, 33);
            this.locationDetails.Multiline = true;
            this.locationDetails.Name = "locationDetails";
            this.locationDetails.ReadOnly = true;
            this.locationDetails.Size = new System.Drawing.Size(236, 78);
            this.locationDetails.TabIndex = 4;
            // 
            // FedExHoldAtLocationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.locationDetails);
            this.Controls.Add(this.searchLink);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.holdAtLocation);
            this.Name = "FedExHoldAtLocationControl";
            this.Size = new System.Drawing.Size(326, 121);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox holdAtLocation;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.LinkLabel searchLink;
        private UI.Controls.MultiValueTextBox locationDetails;

    }
}
