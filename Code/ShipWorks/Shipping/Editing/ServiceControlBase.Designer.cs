﻿using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Editing
{
    partial class ServiceControlBase
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sectionShipment = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.sectionLabelOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.labelFormat = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelFormatLabel = new System.Windows.Forms.Label();
            this.sectionReturns = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.returnsPanel = new System.Windows.Forms.Panel();
            this.includeReturn = new System.Windows.Forms.CheckBox();
            this.applyReturnProfile = new System.Windows.Forms.CheckBox();
            this.returnProfileID = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.returnProfileIDLabel = new System.Windows.Forms.Label();
            this.labelReturnWarning = new System.Windows.Forms.Label();
            this.pictureBoxReturnWarning = new System.Windows.Forms.PictureBox();
            this.linkManageProfiles = new ShipWorks.UI.Controls.LinkControl();
            this.returnShipment = new System.Windows.Forms.CheckBox();
            this.sectionRecipient = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.residentialDetermination = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelResidentialCommercial = new System.Windows.Forms.Label();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.help = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions.ContentPanel)).BeginInit();
            this.sectionLabelOptions.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns.ContentPanel)).BeginInit();
            this.sectionReturns.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.help)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionShipment
            // 
            this.sectionShipment.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionShipment.ExtraText = "";
            this.sectionShipment.Location = new System.Drawing.Point(3, 33);
            this.sectionShipment.Name = "sectionShipment";
            this.sectionShipment.SectionName = "Shipment Details";
            this.sectionShipment.SettingsKey = "{b1ef9b57-045b-4881-b290-4dbf6f070eff}";
            this.sectionShipment.Size = new System.Drawing.Size(385, 75);
            this.sectionShipment.TabIndex = 3;
            // 
            // sectionLabelOptions
            // 
            this.sectionLabelOptions.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionLabelOptions.Collapsed = true;
            // 
            // sectionLabelOptions.ContentPanel
            // 
            this.sectionLabelOptions.ContentPanel.Controls.Add(this.help);
            this.sectionLabelOptions.ContentPanel.Controls.Add(this.labelFormat);
            this.sectionLabelOptions.ContentPanel.Controls.Add(this.labelFormatLabel);
            this.sectionLabelOptions.ExpandedHeight = 70;
            this.sectionLabelOptions.ExtraText = "";
            this.sectionLabelOptions.Location = new System.Drawing.Point(3, 144);
            this.sectionLabelOptions.Name = "sectionLabelOptions";
            this.sectionLabelOptions.SectionName = "Label Options";
            this.sectionLabelOptions.SettingsKey = "{d20eb555-afcd-4050-9c9e-bd982dbc60c9}";
            this.sectionLabelOptions.Size = new System.Drawing.Size(385, 24);
            this.sectionLabelOptions.TabIndex = 4;
            // 
            // labelFormat
            // 
            this.labelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.labelFormat.FormattingEnabled = true;
            this.labelFormat.Location = new System.Drawing.Point(145, 8);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.PromptText = "(Multiple Values)";
            this.labelFormat.Size = new System.Drawing.Size(90, 21);
            this.labelFormat.TabIndex = 7;
            // 
            // labelFormatLabel
            // 
            this.labelFormatLabel.AutoSize = true;
            this.labelFormatLabel.BackColor = System.Drawing.Color.White;
            this.labelFormatLabel.Location = new System.Drawing.Point(10, 11);
            this.labelFormatLabel.Name = "labelFormatLabel";
            this.labelFormatLabel.Size = new System.Drawing.Size(128, 13);
            this.labelFormatLabel.TabIndex = 6;
            this.labelFormatLabel.Text = "Requested Label Format:";
            // 
            // sectionReturns
            // 
            this.sectionReturns.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionReturns.Collapsed = true;
            // 
            // sectionReturns.ContentPanel
            // 
            this.sectionReturns.ContentPanel.Controls.Add(this.returnsPanel);
            this.sectionReturns.ContentPanel.Controls.Add(this.includeReturn);
            this.sectionReturns.ContentPanel.Controls.Add(this.applyReturnProfile);
            this.sectionReturns.ContentPanel.Controls.Add(this.returnProfileID);
            this.sectionReturns.ContentPanel.Controls.Add(this.returnProfileIDLabel);
            this.sectionReturns.ContentPanel.Controls.Add(this.linkManageProfiles);
            this.sectionReturns.ContentPanel.Controls.Add(this.returnShipment);
            this.sectionReturns.ContentPanel.Controls.Add(this.pictureBoxReturnWarning);
            this.sectionReturns.ContentPanel.Controls.Add(this.labelReturnWarning);
            this.sectionReturns.ExpandedHeight = 205;
            this.sectionReturns.ExtraText = "";
            this.sectionReturns.Location = new System.Drawing.Point(3, 114);
            this.sectionReturns.Name = "sectionReturns";
            this.sectionReturns.SectionName = "Return Shipment";
            this.sectionReturns.SettingsKey = "{CCEDFAA6-0D40-4786-8F24-4F89E357825D}";
            this.sectionReturns.Size = new System.Drawing.Size(385, 24);
            this.sectionReturns.TabIndex = 2;
            // 
            // returnsPanel
            // 
            this.returnsPanel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.returnsPanel.AutoSize = true;
            this.returnsPanel.BackColor = System.Drawing.Color.White;
            this.returnsPanel.Location = new System.Drawing.Point(37, 94);
            this.returnsPanel.Name = "returnsPanel";
            this.returnsPanel.Size = new System.Drawing.Size(365, 134);
            this.returnsPanel.TabIndex = 79;
            // 
            // includeReturn
            // 
            this.includeReturn.AutoSize = true;
            this.includeReturn.BackColor = System.Drawing.Color.White;
            this.includeReturn.Location = new System.Drawing.Point(13, 12);
            this.includeReturn.Name = "includeReturn";
            this.includeReturn.Size = new System.Drawing.Size(143, 17);
            this.includeReturn.TabIndex = 75;
            this.includeReturn.Text = "Automatically include a return label";
            this.includeReturn.UseVisualStyleBackColor = false;
            this.includeReturn.CheckedChanged += new System.EventHandler(this.OnIncludeReturnChanged);
            // 
            // applyReturnProfile
            // 
            this.applyReturnProfile.AutoSize = true;
            this.applyReturnProfile.BackColor = System.Drawing.Color.White;
            this.applyReturnProfile.Location = new System.Drawing.Point(37, 33);
            this.applyReturnProfile.Name = "applyReturnProfile";
            this.applyReturnProfile.Size = new System.Drawing.Size(200, 17);
            this.applyReturnProfile.TabIndex = 76;
            this.applyReturnProfile.Text = "Apply a profile to the return shipment";
            this.applyReturnProfile.UseVisualStyleBackColor = false;
            this.applyReturnProfile.CheckedChanged += new System.EventHandler(this.OnApplyReturnChanged);
            // 
            // returnProfileID
            // 
            this.returnProfileID.AutoSize = true;
            this.returnProfileID.BackColor = System.Drawing.Color.White;
            this.returnProfileID.Location = new System.Drawing.Point(120, 52);
            this.returnProfileID.Name = "returnProfileID";
            this.returnProfileID.Size = new System.Drawing.Size(150, 13);
            this.returnProfileID.TabIndex = 77;
            this.returnProfileID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.returnProfileID.FormattingEnabled = true;
            this.returnProfileID.DropDown += new System.EventHandler(this.OnReturnProfileIDOpened);
            this.returnProfileID.SelectionChangeCommitted += new System.EventHandler(this.OnReturnProfileSelected);
            this.returnProfileID.EnabledChanged += new System.EventHandler(this.OnReturnProfileIDEnabledChanged);
            // 
            // returnProfileIDLabel
            // 
            this.returnProfileIDLabel.AutoSize = true;
            this.returnProfileIDLabel.BackColor = System.Drawing.Color.White;
            this.returnProfileIDLabel.Location = new System.Drawing.Point(37, 55);
            this.returnProfileIDLabel.Name = "returnProfileIDLabel";
            this.returnProfileIDLabel.Size = new System.Drawing.Size(50, 13);
            this.returnProfileIDLabel.TabIndex = 77;
            this.returnProfileIDLabel.Text = "Return Profile:";
            // 
            // linkManageProfiles
            // 
            this.linkManageProfiles.AutoSize = true;
            this.linkManageProfiles.BackColor = System.Drawing.Color.Transparent;
            this.linkManageProfiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkManageProfiles.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkManageProfiles.ForeColor = System.Drawing.Color.Blue;
            this.linkManageProfiles.Location = new System.Drawing.Point(275, 55);
            this.linkManageProfiles.Name = "linkManageProfiles";
            this.linkManageProfiles.Size = new System.Drawing.Size(57, 13);
            this.linkManageProfiles.TabIndex = 1;
            this.linkManageProfiles.Text = "Manage Profiles";
            this.linkManageProfiles.Click += new System.EventHandler(this.OnManageProfiles);
            // 
            // returnShipment
            // 
            this.returnShipment.AutoSize = true;
            this.returnShipment.BackColor = System.Drawing.Color.White;
            this.returnShipment.Location = new System.Drawing.Point(13, 76);
            this.returnShipment.Name = "returnShipment";
            this.returnShipment.Size = new System.Drawing.Size(143, 17);
            this.returnShipment.TabIndex = 78;
            this.returnShipment.Text = "Make this a return shipment";
            this.returnShipment.UseVisualStyleBackColor = false;
            this.returnShipment.CheckedChanged += new System.EventHandler(this.OnReturnShipmentChanged);
            // 
            // labelReturnWarning
            // 
            this.labelReturnWarning.AutoSize = true;
            this.labelReturnWarning.BackColor = System.Drawing.Color.White;
            this.labelReturnWarning.ForeColor = System.Drawing.Color.DimGray;
            this.labelReturnWarning.Location = new System.Drawing.Point(225, 13);
            this.labelReturnWarning.Name = "labelReturnWarning";
            this.labelReturnWarning.Size = new System.Drawing.Size(328, 13);
            this.labelReturnWarning.TabIndex = 82;
            this.labelReturnWarning.Text = "Shipment rates do not include any return label charges.";
            // 
            // pictureBoxReturnWarning
            // 
            this.pictureBoxReturnWarning.BackColor = System.Drawing.Color.White;
            this.pictureBoxReturnWarning.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBoxReturnWarning.Location = new System.Drawing.Point(208, 12);
            this.pictureBoxReturnWarning.Name = "pictureBoxReturnWarning";
            this.pictureBoxReturnWarning.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxReturnWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxReturnWarning.TabIndex = 81;
            this.pictureBoxReturnWarning.TabStop = false;
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionRecipient.Collapsed = true;
            // 
            // sectionRecipient.ContentPanel
            // 
            this.sectionRecipient.ContentPanel.Controls.Add(this.residentialDetermination);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelAddress);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelResidentialCommercial);
            this.sectionRecipient.ContentPanel.Controls.Add(this.personControl);
            this.sectionRecipient.ExpandedHeight = 414;
            this.sectionRecipient.ExtraText = "";
            this.sectionRecipient.Location = new System.Drawing.Point(3, 3);
            this.sectionRecipient.Name = "sectionRecipient";
            this.sectionRecipient.SectionName = "To";
            this.sectionRecipient.SettingsKey = "{0ed19c0f-fcec-4c51-8fb8-9b01f4ad9937}";
            this.sectionRecipient.Size = new System.Drawing.Size(385, 24);
            this.sectionRecipient.TabIndex = 0;
            // 
            // residentialDetermination
            // 
            this.residentialDetermination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.residentialDetermination.FormattingEnabled = true;
            this.residentialDetermination.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.residentialDetermination.Location = new System.Drawing.Point(79, 355);
            this.residentialDetermination.Name = "residentialDetermination";
            this.residentialDetermination.PromptText = "(Multiple Values)";
            this.residentialDetermination.Size = new System.Drawing.Size(190, 21);
            this.residentialDetermination.TabIndex = 71;
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.BackColor = System.Drawing.Color.White;
            this.labelAddress.Location = new System.Drawing.Point(23, 358);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(50, 13);
            this.labelAddress.TabIndex = 2;
            this.labelAddress.Text = "Address:";
            // 
            // labelResidentialCommercial
            // 
            this.labelResidentialCommercial.AutoSize = true;
            this.labelResidentialCommercial.BackColor = System.Drawing.Color.White;
            this.labelResidentialCommercial.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelResidentialCommercial.Location = new System.Drawing.Point(10, 335);
            this.labelResidentialCommercial.Name = "labelResidentialCommercial";
            this.labelResidentialCommercial.Size = new System.Drawing.Size(149, 13);
            this.labelResidentialCommercial.TabIndex = 1;
            this.labelResidentialCommercial.Text = "Residential \\ Commercial";
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields) ((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company)
            | ShipWorks.Data.Controls.PersonFields.Street)
            | ShipWorks.Data.Controls.PersonFields.City)
            | ShipWorks.Data.Controls.PersonFields.State)
            | ShipWorks.Data.Controls.PersonFields.Postal)
            | ShipWorks.Data.Controls.PersonFields.Country)
            | ShipWorks.Data.Controls.PersonFields.Residential)
            | ShipWorks.Data.Controls.PersonFields.Email)
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.personControl.BackColor = System.Drawing.Color.Transparent;
            this.personControl.EnableValidationControls = false;
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.personControl.Location = new System.Drawing.Point(3, 2);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(273, 330);
            this.personControl.TabIndex = 0;
            this.personControl.DestinationChanged += new System.EventHandler(this.OnRecipientDestinationChanged);
            // 
            // help
            // 
            this.help.BackColor = System.Drawing.Color.Transparent;
            this.help.Cursor = System.Windows.Forms.Cursors.Hand;
            this.help.Image = global::ShipWorks.Properties.Resources.help2_16;
            this.help.Location = new System.Drawing.Point(238, 11);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(16, 16);
            this.help.TabIndex = 8;
            this.help.TabStop = false;
            this.help.Click += new System.EventHandler(this.OnHelpClick);
            // 
            // ServiceControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMargin = new System.Drawing.Size(0, 5);
            this.Controls.Add(this.sectionShipment);
            this.Controls.Add(this.sectionLabelOptions);
            this.Controls.Add(this.sectionReturns);
            this.Controls.Add(this.sectionRecipient);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ServiceControlBase";
            this.Size = new System.Drawing.Size(391, 241);
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions.ContentPanel)).EndInit();
            this.sectionLabelOptions.ContentPanel.ResumeLayout(false);
            this.sectionLabelOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns.ContentPanel)).EndInit();
            this.sectionReturns.ContentPanel.ResumeLayout(false);
            this.sectionReturns.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.help)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected ShipWorks.UI.Controls.CollapsibleGroupControl sectionRecipient;
        protected ShipWorks.Data.Controls.PersonControl personControl;
        protected System.Windows.Forms.Label labelResidentialCommercial;
        protected System.Windows.Forms.Label labelAddress;
        protected UI.Controls.MultiValueComboBox residentialDetermination;
        private System.Windows.Forms.CheckBox returnShipment;
        private System.Windows.Forms.Panel returnsPanel;
        protected UI.Controls.CollapsibleGroupControl sectionReturns;
        protected UI.Controls.CollapsibleGroupControl sectionShipment;
        protected UI.Controls.CollapsibleGroupControl sectionLabelOptions;
        private System.Windows.Forms.Label labelFormatLabel;
        protected MultiValueComboBox labelFormat;
        private System.Windows.Forms.PictureBox help;
        private System.Windows.Forms.CheckBox includeReturn;
        private System.Windows.Forms.CheckBox applyReturnProfile;
        private ShipWorks.UI.Controls.MultiValueComboBox returnProfileID;
        private ShipWorks.UI.Controls.LinkControl linkManageProfiles;
        private System.Windows.Forms.Label returnProfileIDLabel;
        private System.Windows.Forms.Label labelReturnWarning;
        private System.Windows.Forms.PictureBox pictureBoxReturnWarning;
    }
}
