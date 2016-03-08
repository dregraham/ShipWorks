namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    partial class SimpleAddressControl
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
            this.street = new ShipWorks.UI.Controls.StreetEditor();
            this.postalCode = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelPostalCode = new System.Windows.Forms.Label();
            this.state = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelStateProv = new System.Windows.Forms.Label();
            this.city = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelStreet = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // street
            // 
            this.street.AcceptsReturn = true;
            this.street.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.street.Line1 = "";
            this.street.Line2 = "";
            this.street.Line3 = "";
            this.street.Location = new System.Drawing.Point(75, 3);
            this.street.Multiline = true;
            this.street.Name = "street";
            this.street.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.street.Size = new System.Drawing.Size(266, 47);
            this.street.TabIndex = 45;
            // 
            // postalCode
            // 
            this.postalCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.postalCode.Location = new System.Drawing.Point(75, 110);
            this.postalCode.Name = "postalCode";
            this.postalCode.Size = new System.Drawing.Size(266, 20);
            this.postalCode.TabIndex = 48;
            // 
            // labelPostalCode
            // 
            this.labelPostalCode.AutoSize = true;
            this.labelPostalCode.Location = new System.Drawing.Point(1, 113);
            this.labelPostalCode.Name = "labelPostalCode";
            this.labelPostalCode.Size = new System.Drawing.Size(67, 13);
            this.labelPostalCode.TabIndex = 52;
            this.labelPostalCode.Text = "Postal Code:";
            // 
            // state
            // 
            this.state.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.state.FormattingEnabled = true;
            this.state.Location = new System.Drawing.Point(75, 83);
            this.state.MaxDropDownItems = 20;
            this.state.Name = "state";
            this.state.PromptText = "(Multiple Values)";
            this.state.Size = new System.Drawing.Size(266, 21);
            this.state.TabIndex = 47;
            // 
            // labelStateProv
            // 
            this.labelStateProv.AutoSize = true;
            this.labelStateProv.Location = new System.Drawing.Point(0, 86);
            this.labelStateProv.Name = "labelStateProv";
            this.labelStateProv.Size = new System.Drawing.Size(68, 13);
            this.labelStateProv.TabIndex = 51;
            this.labelStateProv.Text = "State \\ Prov:";
            // 
            // city
            // 
            this.city.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.city.Location = new System.Drawing.Point(75, 56);
            this.city.Name = "city";
            this.city.Size = new System.Drawing.Size(266, 20);
            this.city.TabIndex = 46;
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(41, 59);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(27, 13);
            this.labelCity.TabIndex = 50;
            this.labelCity.Text = "City:";
            // 
            // labelStreet
            // 
            this.labelStreet.AutoSize = true;
            this.labelStreet.Location = new System.Drawing.Point(30, 6);
            this.labelStreet.Name = "labelStreet";
            this.labelStreet.Size = new System.Drawing.Size(38, 13);
            this.labelStreet.TabIndex = 49;
            this.labelStreet.Text = "Street:";
            // 
            // SimpleAddressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.street);
            this.Controls.Add(this.postalCode);
            this.Controls.Add(this.labelPostalCode);
            this.Controls.Add(this.state);
            this.Controls.Add(this.labelStateProv);
            this.Controls.Add(this.city);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.labelStreet);
            this.Name = "SimpleAddressControl";
            this.Size = new System.Drawing.Size(344, 134);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.StreetEditor street;
        private UI.Controls.MultiValueTextBox postalCode;
        private System.Windows.Forms.Label labelPostalCode;
        private UI.Controls.MultiValueComboBox state;
        private System.Windows.Forms.Label labelStateProv;
        private UI.Controls.MultiValueTextBox city;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.Label labelStreet;
    }
}
