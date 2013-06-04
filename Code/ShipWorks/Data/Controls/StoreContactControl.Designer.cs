namespace ShipWorks.Data.Controls
{
    partial class StoreContactControl
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
            this.components = new System.ComponentModel.Container();
            this.labelContactInfo = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelWebsite = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelFax = new System.Windows.Forms.Label();
            this.labelPhone = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelPostalCode = new System.Windows.Forms.Label();
            this.labelStateProv = new System.Windows.Forms.Label();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelStreet = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelFullName = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.street = new ShipWorks.UI.Controls.StreetEditor();
            this.website = new System.Windows.Forms.TextBox();
            this.phone = new System.Windows.Forms.TextBox();
            this.fax = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.TextBox();
            this.country = new System.Windows.Forms.ComboBox();
            this.postalCode = new System.Windows.Forms.TextBox();
            this.state = new System.Windows.Forms.ComboBox();
            this.city = new System.Windows.Forms.TextBox();
            this.company = new System.Windows.Forms.TextBox();
            this.fullName = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelContactInfo
            // 
            this.labelContactInfo.AutoSize = true;
            this.labelContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelContactInfo.Location = new System.Drawing.Point(1, 270);
            this.labelContactInfo.Name = "labelContactInfo";
            this.labelContactInfo.Size = new System.Drawing.Size(122, 13);
            this.labelContactInfo.TabIndex = 73;
            this.labelContactInfo.Text = "Contact Information";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAddress.Location = new System.Drawing.Point(1, 83);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(53, 13);
            this.labelAddress.TabIndex = 72;
            this.labelAddress.Text = "Address";
            // 
            // labelWebsite
            // 
            this.labelWebsite.AutoSize = true;
            this.labelWebsite.Location = new System.Drawing.Point(20, 375);
            this.labelWebsite.Name = "labelWebsite";
            this.labelWebsite.Size = new System.Drawing.Size(50, 13);
            this.labelWebsite.TabIndex = 71;
            this.labelWebsite.Text = "Website:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(34, 294);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 70;
            this.labelEmail.Text = "Email:";
            // 
            // labelFax
            // 
            this.labelFax.AutoSize = true;
            this.labelFax.Location = new System.Drawing.Point(40, 321);
            this.labelFax.Name = "labelFax";
            this.labelFax.Size = new System.Drawing.Size(29, 13);
            this.labelFax.TabIndex = 69;
            this.labelFax.Text = "Fax:";
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(28, 348);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(41, 13);
            this.labelPhone.TabIndex = 68;
            this.labelPhone.Text = "Phone:";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(19, 240);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(50, 13);
            this.labelCountry.TabIndex = 67;
            this.labelCountry.Text = "Country:";
            // 
            // labelPostalCode
            // 
            this.labelPostalCode.AutoSize = true;
            this.labelPostalCode.Location = new System.Drawing.Point(1, 213);
            this.labelPostalCode.Name = "labelPostalCode";
            this.labelPostalCode.Size = new System.Drawing.Size(68, 13);
            this.labelPostalCode.TabIndex = 66;
            this.labelPostalCode.Text = "Postal Code:";
            // 
            // labelStateProv
            // 
            this.labelStateProv.AutoSize = true;
            this.labelStateProv.Location = new System.Drawing.Point(1, 186);
            this.labelStateProv.Name = "labelStateProv";
            this.labelStateProv.Size = new System.Drawing.Size(69, 13);
            this.labelStateProv.TabIndex = 65;
            this.labelStateProv.Text = "State \\ Prov:";
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(40, 159);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(30, 13);
            this.labelCity.TabIndex = 64;
            this.labelCity.Text = "City:";
            // 
            // labelStreet
            // 
            this.labelStreet.AutoSize = true;
            this.labelStreet.Location = new System.Drawing.Point(28, 106);
            this.labelStreet.Name = "labelStreet";
            this.labelStreet.Size = new System.Drawing.Size(41, 13);
            this.labelStreet.TabIndex = 63;
            this.labelStreet.Text = "Street:";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(13, 56);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(56, 13);
            this.labelCompany.TabIndex = 62;
            this.labelCompany.Text = "Company:";
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Location = new System.Drawing.Point(3, 29);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(67, 13);
            this.labelFullName.TabIndex = 61;
            this.labelFullName.Text = "Store Name:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelName.Location = new System.Drawing.Point(1, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(39, 13);
            this.labelName.TabIndex = 74;
            this.labelName.Text = "Name";
            // 
            // street
            // 
            this.street.AcceptsReturn = true;
            this.street.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.street.Line1 = "";
            this.street.Line2 = "";
            this.street.Line3 = "";
            this.street.Location = new System.Drawing.Point(75, 103);
            this.fieldLengthProvider.SetMaxLengthSource(this.street, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonStreetFull);
            this.street.Multiline = true;
            this.street.Name = "street";
            this.street.Size = new System.Drawing.Size(269, 47);
            this.street.TabIndex = 52;
            // 
            // website
            // 
            this.website.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.website.Location = new System.Drawing.Point(75, 372);
            this.fieldLengthProvider.SetMaxLengthSource(this.website, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonWebsite);
            this.website.Name = "website";
            this.website.Size = new System.Drawing.Size(269, 21);
            this.website.TabIndex = 60;
            // 
            // phone
            // 
            this.phone.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.phone.Location = new System.Drawing.Point(75, 345);
            this.fieldLengthProvider.SetMaxLengthSource(this.phone, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonPhone);
            this.phone.Name = "phone";
            this.phone.Size = new System.Drawing.Size(269, 21);
            this.phone.TabIndex = 59;
            // 
            // fax
            // 
            this.fax.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fax.Location = new System.Drawing.Point(75, 318);
            this.fieldLengthProvider.SetMaxLengthSource(this.fax, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonFax);
            this.fax.Name = "fax";
            this.fax.Size = new System.Drawing.Size(269, 21);
            this.fax.TabIndex = 58;
            // 
            // email
            // 
            this.email.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.email.Location = new System.Drawing.Point(75, 291);
            this.fieldLengthProvider.SetMaxLengthSource(this.email, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonEmail);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(269, 21);
            this.email.TabIndex = 57;
            // 
            // country
            // 
            this.country.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.country.FormattingEnabled = true;
            this.country.Location = new System.Drawing.Point(75, 237);
            this.country.MaxDropDownItems = 20;
            this.country.Name = "country";
            this.country.Size = new System.Drawing.Size(269, 21);
            this.country.TabIndex = 56;
            this.country.SelectedIndexChanged += new System.EventHandler(this.OnCountryChanged);
            // 
            // postalCode
            // 
            this.postalCode.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.postalCode.Location = new System.Drawing.Point(75, 210);
            this.fieldLengthProvider.SetMaxLengthSource(this.postalCode, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonPostal);
            this.postalCode.Name = "postalCode";
            this.postalCode.Size = new System.Drawing.Size(269, 21);
            this.postalCode.TabIndex = 55;
            // 
            // state
            // 
            this.state.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.state.FormattingEnabled = true;
            this.state.Location = new System.Drawing.Point(75, 183);
            this.state.MaxDropDownItems = 20;
            this.fieldLengthProvider.SetMaxLengthSource(this.state, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonState);
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(269, 21);
            this.state.TabIndex = 54;
            // 
            // city
            // 
            this.city.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.city.Location = new System.Drawing.Point(75, 156);
            this.fieldLengthProvider.SetMaxLengthSource(this.city, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonCity);
            this.city.Name = "city";
            this.city.Size = new System.Drawing.Size(269, 21);
            this.city.TabIndex = 53;
            // 
            // company
            // 
            this.company.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.company.Location = new System.Drawing.Point(75, 53);
            this.fieldLengthProvider.SetMaxLengthSource(this.company, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonCompany);
            this.company.Name = "company";
            this.company.Size = new System.Drawing.Size(269, 21);
            this.company.TabIndex = 51;
            // 
            // fullName
            // 
            this.fullName.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fullName.Location = new System.Drawing.Point(75, 26);
            this.fieldLengthProvider.SetMaxLengthSource(this.fullName, ShipWorks.Data.Utility.EntityFieldLengthSource.StoreName);
            this.fullName.Name = "fullName";
            this.fullName.Size = new System.Drawing.Size(269, 21);
            this.fullName.TabIndex = 50;
            // 
            // StoreContactControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.street);
            this.Controls.Add(this.labelContactInfo);
            this.Controls.Add(this.labelAddress);
            this.Controls.Add(this.website);
            this.Controls.Add(this.labelWebsite);
            this.Controls.Add(this.phone);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.fax);
            this.Controls.Add(this.labelFax);
            this.Controls.Add(this.email);
            this.Controls.Add(this.labelPhone);
            this.Controls.Add(this.country);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.postalCode);
            this.Controls.Add(this.labelPostalCode);
            this.Controls.Add(this.state);
            this.Controls.Add(this.labelStateProv);
            this.Controls.Add(this.city);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.labelStreet);
            this.Controls.Add(this.company);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.fullName);
            this.Controls.Add(this.labelFullName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "StoreContactControl";
            this.Size = new System.Drawing.Size(358, 410);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.StreetEditor street;
        protected System.Windows.Forms.Label labelContactInfo;
        protected System.Windows.Forms.Label labelAddress;
        protected System.Windows.Forms.TextBox website;
        protected System.Windows.Forms.Label labelWebsite;
        protected System.Windows.Forms.TextBox phone;
        protected System.Windows.Forms.Label labelEmail;
        protected System.Windows.Forms.TextBox fax;
        protected System.Windows.Forms.Label labelFax;
        protected System.Windows.Forms.TextBox email;
        protected System.Windows.Forms.Label labelPhone;
        protected System.Windows.Forms.ComboBox country;
        protected System.Windows.Forms.Label labelCountry;
        protected System.Windows.Forms.TextBox postalCode;
        protected System.Windows.Forms.Label labelPostalCode;
        protected System.Windows.Forms.ComboBox state;
        protected System.Windows.Forms.Label labelStateProv;
        protected System.Windows.Forms.TextBox city;
        protected System.Windows.Forms.Label labelCity;
        protected System.Windows.Forms.Label labelStreet;
        protected System.Windows.Forms.TextBox company;
        protected System.Windows.Forms.Label labelCompany;
        protected System.Windows.Forms.TextBox fullName;
        protected System.Windows.Forms.Label labelFullName;
        protected System.Windows.Forms.Label labelName;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}
