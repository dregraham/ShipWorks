namespace ShipWorks.Stores.Management
{
    partial class StoreAddressControl
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
            this.labelAddress = new System.Windows.Forms.Label();
            this.country = new System.Windows.Forms.ComboBox();
            this.labelCountry = new System.Windows.Forms.Label();
            this.postalCode = new System.Windows.Forms.TextBox();
            this.labelPostalCode = new System.Windows.Forms.Label();
            this.state = new System.Windows.Forms.ComboBox();
            this.labelStateProv = new System.Windows.Forms.Label();
            this.city = new System.Windows.Forms.TextBox();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelStreet = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.fullName = new System.Windows.Forms.TextBox();
            this.labelFullName = new System.Windows.Forms.Label();
            this.company = new System.Windows.Forms.TextBox();
            this.labelCompany = new System.Windows.Forms.Label();
            this.street = new ShipWorks.UI.Controls.StreetEditor();
            this.SuspendLayout();
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAddress.Location = new System.Drawing.Point(0, 74);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(53, 13);
            this.labelAddress.TabIndex = 83;
            this.labelAddress.Text = "Address";
            // 
            // country
            // 
            this.country.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.country.FormattingEnabled = true;
            this.country.Location = new System.Drawing.Point(74, 256);
            this.country.MaxDropDownItems = 20;
            this.country.Name = "country";
            this.country.Size = new System.Drawing.Size(274, 21);
            this.country.TabIndex = 77;
            this.country.SelectedIndexChanged += new System.EventHandler(this.OnCountryChanged);
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(18, 259);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(50, 13);
            this.labelCountry.TabIndex = 82;
            this.labelCountry.Text = "Country:";
            // 
            // postalCode
            // 
            this.postalCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.postalCode.Location = new System.Drawing.Point(74, 229);
            this.postalCode.Name = "postalCode";
            this.postalCode.Size = new System.Drawing.Size(274, 21);
            this.postalCode.TabIndex = 76;
            // 
            // labelPostalCode
            // 
            this.labelPostalCode.AutoSize = true;
            this.labelPostalCode.Location = new System.Drawing.Point(0, 232);
            this.labelPostalCode.Name = "labelPostalCode";
            this.labelPostalCode.Size = new System.Drawing.Size(68, 13);
            this.labelPostalCode.TabIndex = 81;
            this.labelPostalCode.Text = "Postal Code:";
            // 
            // state
            // 
            this.state.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.state.FormattingEnabled = true;
            this.state.Location = new System.Drawing.Point(74, 202);
            this.state.MaxDropDownItems = 20;
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(274, 21);
            this.state.TabIndex = 75;
            // 
            // labelStateProv
            // 
            this.labelStateProv.AutoSize = true;
            this.labelStateProv.Location = new System.Drawing.Point(0, 205);
            this.labelStateProv.Name = "labelStateProv";
            this.labelStateProv.Size = new System.Drawing.Size(69, 13);
            this.labelStateProv.TabIndex = 80;
            this.labelStateProv.Text = "State \\ Prov:";
            // 
            // city
            // 
            this.city.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.city.Location = new System.Drawing.Point(74, 175);
            this.city.Name = "city";
            this.city.Size = new System.Drawing.Size(274, 21);
            this.city.TabIndex = 74;
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(39, 178);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(30, 13);
            this.labelCity.TabIndex = 79;
            this.labelCity.Text = "City:";
            // 
            // labelStreet
            // 
            this.labelStreet.AutoSize = true;
            this.labelStreet.Location = new System.Drawing.Point(27, 125);
            this.labelStreet.Name = "labelStreet";
            this.labelStreet.Size = new System.Drawing.Size(41, 13);
            this.labelStreet.TabIndex = 78;
            this.labelStreet.Text = "Street:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(71, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 13);
            this.label1.TabIndex = 87;
            this.label1.Text = "(This is just how your store will display in ShipWorks)";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(0, 3);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(39, 13);
            this.labelName.TabIndex = 86;
            this.labelName.Text = "Name";
            // 
            // fullName
            // 
            this.fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fullName.Location = new System.Drawing.Point(74, 23);
            this.fullName.Name = "fullName";
            this.fullName.Size = new System.Drawing.Size(274, 21);
            this.fullName.TabIndex = 84;
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Location = new System.Drawing.Point(2, 26);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(66, 13);
            this.labelFullName.TabIndex = 85;
            this.labelFullName.Text = "Store name:";
            // 
            // company
            // 
            this.company.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.company.Location = new System.Drawing.Point(74, 95);
            this.company.Name = "company";
            this.company.Size = new System.Drawing.Size(274, 21);
            this.company.TabIndex = 88;
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(12, 98);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(56, 13);
            this.labelCompany.TabIndex = 89;
            this.labelCompany.Text = "Company:";
            // 
            // street
            // 
            this.street.AcceptsReturn = true;
            this.street.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.street.Line1 = "";
            this.street.Line2 = "";
            this.street.Line3 = "";
            this.street.Location = new System.Drawing.Point(74, 122);
            this.street.Multiline = true;
            this.street.Name = "street";
            this.street.Size = new System.Drawing.Size(274, 47);
            this.street.TabIndex = 73;
            // 
            // StoreAddressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.company);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.fullName);
            this.Controls.Add(this.labelFullName);
            this.Controls.Add(this.street);
            this.Controls.Add(this.labelAddress);
            this.Controls.Add(this.country);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.postalCode);
            this.Controls.Add(this.labelPostalCode);
            this.Controls.Add(this.state);
            this.Controls.Add(this.labelStateProv);
            this.Controls.Add(this.city);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.labelStreet);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StoreAddressControl";
            this.Size = new System.Drawing.Size(360, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.StreetEditor street;
        protected System.Windows.Forms.Label labelAddress;
        protected System.Windows.Forms.ComboBox country;
        protected System.Windows.Forms.Label labelCountry;
        protected System.Windows.Forms.TextBox postalCode;
        protected System.Windows.Forms.Label labelPostalCode;
        protected System.Windows.Forms.ComboBox state;
        protected System.Windows.Forms.Label labelStateProv;
        protected System.Windows.Forms.TextBox city;
        protected System.Windows.Forms.Label labelCity;
        protected System.Windows.Forms.Label labelStreet;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label labelName;
        protected System.Windows.Forms.TextBox fullName;
        protected System.Windows.Forms.Label labelFullName;
        protected System.Windows.Forms.TextBox company;
        protected System.Windows.Forms.Label labelCompany;
    }
}
