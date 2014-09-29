namespace ShipWorks.Data.Controls
{
    partial class PersonControl
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
            this.addressValidationPanel = new System.Windows.Forms.Panel();
            this.validateAddress = new System.Windows.Forms.LinkLabel();
            this.addressValidationSuggestionLink = new System.Windows.Forms.LinkLabel();
            this.addressValidationStatusText = new System.Windows.Forms.Label();
            this.addressValidationStatusIcon = new System.Windows.Forms.PictureBox();
            this.addressLabelPanel = new System.Windows.Forms.Panel();
            this.street = new ShipWorks.UI.Controls.StreetEditor();
            this.website = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.phone = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.fax = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.email = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.country = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.postalCode = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.state = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.city = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.company = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.fullName = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.addressValidationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.addressValidationStatusIcon)).BeginInit();
            this.addressLabelPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelContactInfo
            // 
            this.labelContactInfo.AutoSize = true;
            this.labelContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContactInfo.Location = new System.Drawing.Point(2, 265);
            this.labelContactInfo.Name = "labelContactInfo";
            this.labelContactInfo.Size = new System.Drawing.Size(122, 13);
            this.labelContactInfo.TabIndex = 49;
            this.labelContactInfo.Text = "Contact Information";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAddress.Location = new System.Drawing.Point(2, 1);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(53, 13);
            this.labelAddress.TabIndex = 48;
            this.labelAddress.Text = "Address";
            // 
            // labelWebsite
            // 
            this.labelWebsite.AutoSize = true;
            this.labelWebsite.Location = new System.Drawing.Point(21, 370);
            this.labelWebsite.Name = "labelWebsite";
            this.labelWebsite.Size = new System.Drawing.Size(50, 13);
            this.labelWebsite.TabIndex = 46;
            this.labelWebsite.Text = "Website:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(35, 289);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 44;
            this.labelEmail.Text = "Email:";
            // 
            // labelFax
            // 
            this.labelFax.AutoSize = true;
            this.labelFax.Location = new System.Drawing.Point(41, 343);
            this.labelFax.Name = "labelFax";
            this.labelFax.Size = new System.Drawing.Size(29, 13);
            this.labelFax.TabIndex = 42;
            this.labelFax.Text = "Fax:";
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(29, 316);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(41, 13);
            this.labelPhone.TabIndex = 40;
            this.labelPhone.Text = "Phone:";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(20, 238);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(50, 13);
            this.labelCountry.TabIndex = 38;
            this.labelCountry.Text = "Country:";
            // 
            // labelPostalCode
            // 
            this.labelPostalCode.AutoSize = true;
            this.labelPostalCode.Location = new System.Drawing.Point(2, 211);
            this.labelPostalCode.Name = "labelPostalCode";
            this.labelPostalCode.Size = new System.Drawing.Size(68, 13);
            this.labelPostalCode.TabIndex = 36;
            this.labelPostalCode.Text = "Postal Code:";
            // 
            // labelStateProv
            // 
            this.labelStateProv.AutoSize = true;
            this.labelStateProv.Location = new System.Drawing.Point(2, 184);
            this.labelStateProv.Name = "labelStateProv";
            this.labelStateProv.Size = new System.Drawing.Size(69, 13);
            this.labelStateProv.TabIndex = 34;
            this.labelStateProv.Text = "State \\ Prov:";
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(41, 157);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(30, 13);
            this.labelCity.TabIndex = 32;
            this.labelCity.Text = "City:";
            // 
            // labelStreet
            // 
            this.labelStreet.AutoSize = true;
            this.labelStreet.Location = new System.Drawing.Point(29, 104);
            this.labelStreet.Name = "labelStreet";
            this.labelStreet.Size = new System.Drawing.Size(41, 13);
            this.labelStreet.TabIndex = 30;
            this.labelStreet.Text = "Street:";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(14, 55);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(56, 13);
            this.labelCompany.TabIndex = 28;
            this.labelCompany.Text = "Company:";
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Location = new System.Drawing.Point(13, 28);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(57, 13);
            this.labelFullName.TabIndex = 26;
            this.labelFullName.Text = "Full Name:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(2, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(39, 13);
            this.labelName.TabIndex = 50;
            this.labelName.Text = "Name";
            // 
            // addressValidationPanel
            // 
            this.addressValidationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressValidationPanel.Controls.Add(this.validateAddress);
            this.addressValidationPanel.Controls.Add(this.addressValidationSuggestionLink);
            this.addressValidationPanel.Controls.Add(this.addressValidationStatusText);
            this.addressValidationPanel.Controls.Add(this.addressValidationStatusIcon);
            this.addressValidationPanel.Location = new System.Drawing.Point(61, -2);
            this.addressValidationPanel.Name = "addressValidationPanel";
            this.addressValidationPanel.Size = new System.Drawing.Size(291, 19);
            this.addressValidationPanel.TabIndex = 51;
            // 
            // validateAddress
            // 
            this.validateAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.validateAddress.AutoSize = true;
            this.validateAddress.Location = new System.Drawing.Point(236, 3);
            this.validateAddress.Name = "validateAddress";
            this.validateAddress.Size = new System.Drawing.Size(45, 13);
            this.validateAddress.TabIndex = 29;
            this.validateAddress.TabStop = true;
            this.validateAddress.Text = "Validate";
            this.validateAddress.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnValidateAddressLinkClicked);
            // 
            // addressValidationSuggestionLink
            // 
            this.addressValidationSuggestionLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addressValidationSuggestionLink.AutoSize = true;
            this.addressValidationSuggestionLink.Location = new System.Drawing.Point(207, 3);
            this.addressValidationSuggestionLink.Name = "addressValidationSuggestionLink";
            this.addressValidationSuggestionLink.Size = new System.Drawing.Size(74, 13);
            this.addressValidationSuggestionLink.TabIndex = 28;
            this.addressValidationSuggestionLink.TabStop = true;
            this.addressValidationSuggestionLink.Text = "3 Suggestions";
            this.addressValidationSuggestionLink.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.addressValidationSuggestionLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnAddressValidationSuggestionLinkClicked);
            // 
            // addressValidationStatusText
            // 
            this.addressValidationStatusText.AutoEllipsis = true;
            this.addressValidationStatusText.Location = new System.Drawing.Point(37, 3);
            this.addressValidationStatusText.Name = "addressValidationStatusText";
            this.addressValidationStatusText.Size = new System.Drawing.Size(63, 13);
            this.addressValidationStatusText.TabIndex = 27;
            this.addressValidationStatusText.Text = "Valid";
            // 
            // addressValidationStatusIcon
            // 
            this.addressValidationStatusIcon.Location = new System.Drawing.Point(15, 2);
            this.addressValidationStatusIcon.Name = "addressValidationStatusIcon";
            this.addressValidationStatusIcon.Size = new System.Drawing.Size(16, 16);
            this.addressValidationStatusIcon.TabIndex = 0;
            this.addressValidationStatusIcon.TabStop = false;
            // 
            // addressLabelPanel
            // 
            this.addressLabelPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressLabelPanel.Controls.Add(this.labelAddress);
            this.addressLabelPanel.Controls.Add(this.addressValidationPanel);
            this.addressLabelPanel.Location = new System.Drawing.Point(0, 76);
            this.addressLabelPanel.Name = "addressLabelPanel";
            this.addressLabelPanel.Size = new System.Drawing.Size(355, 15);
            this.addressLabelPanel.TabIndex = 52;
            // 
            // street
            // 
            this.street.AcceptsReturn = true;
            this.street.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.street.Line1 = "";
            this.street.Line2 = "";
            this.street.Line3 = "";
            this.street.Location = new System.Drawing.Point(76, 101);
            this.fieldLengthProvider.SetMaxLengthSource(this.street, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonStreetFull);
            this.street.Multiline = true;
            this.street.Name = "street";
            this.street.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.street.Size = new System.Drawing.Size(266, 47);
            this.street.TabIndex = 2;
            this.street.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // website
            // 
            this.website.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.website.Location = new System.Drawing.Point(76, 367);
            this.fieldLengthProvider.SetMaxLengthSource(this.website, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonWebsite);
            this.website.Name = "website";
            this.website.Size = new System.Drawing.Size(266, 21);
            this.website.TabIndex = 11;
            this.website.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // phone
            // 
            this.phone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.phone.Location = new System.Drawing.Point(76, 313);
            this.fieldLengthProvider.SetMaxLengthSource(this.phone, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonPhone);
            this.phone.Name = "phone";
            this.phone.Size = new System.Drawing.Size(266, 21);
            this.phone.TabIndex = 9;
            this.phone.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // fax
            // 
            this.fax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fax.Location = new System.Drawing.Point(76, 340);
            this.fieldLengthProvider.SetMaxLengthSource(this.fax, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonFax);
            this.fax.Name = "fax";
            this.fax.Size = new System.Drawing.Size(266, 21);
            this.fax.TabIndex = 10;
            this.fax.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // email
            // 
            this.email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.email.Location = new System.Drawing.Point(76, 286);
            this.fieldLengthProvider.SetMaxLengthSource(this.email, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonEmail);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(266, 21);
            this.email.TabIndex = 8;
            this.email.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // country
            // 
            this.country.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.country.FormattingEnabled = true;
            this.country.Location = new System.Drawing.Point(76, 235);
            this.country.MaxDropDownItems = 20;
            this.country.Name = "country";
            this.country.PromptText = "(Multiple Values)";
            this.country.Size = new System.Drawing.Size(266, 21);
            this.country.TabIndex = 6;
            this.country.SelectedIndexChanged += new System.EventHandler(this.OnDestinationChanged);
            this.country.SelectedValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // postalCode
            // 
            this.postalCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.postalCode.Location = new System.Drawing.Point(76, 208);
            this.fieldLengthProvider.SetMaxLengthSource(this.postalCode, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonPostal);
            this.postalCode.Name = "postalCode";
            this.postalCode.Size = new System.Drawing.Size(266, 21);
            this.postalCode.TabIndex = 5;
            this.postalCode.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // state
            // 
            this.state.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.state.FormattingEnabled = true;
            this.state.Location = new System.Drawing.Point(76, 181);
            this.state.MaxDropDownItems = 20;
            this.fieldLengthProvider.SetMaxLengthSource(this.state, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonState);
            this.state.Name = "state";
            this.state.PromptText = "(Multiple Values)";
            this.state.Size = new System.Drawing.Size(266, 21);
            this.state.TabIndex = 4;
            this.state.DropDown += new System.EventHandler(this.OnStateDropDown);
            this.state.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // city
            // 
            this.city.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.city.Location = new System.Drawing.Point(76, 154);
            this.fieldLengthProvider.SetMaxLengthSource(this.city, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonCity);
            this.city.Name = "city";
            this.city.Size = new System.Drawing.Size(266, 21);
            this.city.TabIndex = 3;
            this.city.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // company
            // 
            this.company.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.company.Location = new System.Drawing.Point(76, 52);
            this.fieldLengthProvider.SetMaxLengthSource(this.company, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonCompany);
            this.company.Name = "company";
            this.company.Size = new System.Drawing.Size(266, 21);
            this.company.TabIndex = 1;
            this.company.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // fullName
            // 
            this.fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fullName.Location = new System.Drawing.Point(76, 25);
            this.fieldLengthProvider.SetMaxLengthSource(this.fullName, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonNameFull);
            this.fullName.Name = "fullName";
            this.fullName.Size = new System.Drawing.Size(266, 21);
            this.fullName.TabIndex = 0;
            this.fullName.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // PersonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addressLabelPanel);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.street);
            this.Controls.Add(this.labelContactInfo);
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
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PersonControl";
            this.Size = new System.Drawing.Size(355, 402);
            this.Load += new System.EventHandler(this.OnLoad);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.addressValidationPanel.ResumeLayout(false);
            this.addressValidationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.addressValidationStatusIcon)).EndInit();
            this.addressLabelPanel.ResumeLayout(false);
            this.addressLabelPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.StreetEditor street;
        private ShipWorks.UI.Controls.MultiValueTextBox fullName;
        private System.Windows.Forms.Label labelContactInfo;
        private System.Windows.Forms.Label labelAddress;
        private ShipWorks.UI.Controls.MultiValueTextBox website;
        private System.Windows.Forms.Label labelWebsite;
        private ShipWorks.UI.Controls.MultiValueTextBox phone;
        private System.Windows.Forms.Label labelEmail;
        private ShipWorks.UI.Controls.MultiValueTextBox fax;
        private System.Windows.Forms.Label labelFax;
        private ShipWorks.UI.Controls.MultiValueTextBox email;
        private System.Windows.Forms.Label labelPhone;
        private ShipWorks.UI.Controls.MultiValueComboBox country;
        private System.Windows.Forms.Label labelCountry;
        private ShipWorks.UI.Controls.MultiValueTextBox postalCode;
        private System.Windows.Forms.Label labelPostalCode;
        private ShipWorks.UI.Controls.MultiValueComboBox state;
        private System.Windows.Forms.Label labelStateProv;
        private ShipWorks.UI.Controls.MultiValueTextBox city;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.Label labelStreet;
        private ShipWorks.UI.Controls.MultiValueTextBox company;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelFullName;
        private System.Windows.Forms.Label labelName;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Panel addressValidationPanel;
        private System.Windows.Forms.Label addressValidationStatusText;
        private System.Windows.Forms.PictureBox addressValidationStatusIcon;
        private System.Windows.Forms.LinkLabel addressValidationSuggestionLink;
        private System.Windows.Forms.LinkLabel validateAddress;
        private System.Windows.Forms.Panel addressLabelPanel;

    }
}
