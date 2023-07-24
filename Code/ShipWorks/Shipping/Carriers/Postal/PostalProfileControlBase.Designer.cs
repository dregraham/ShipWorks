namespace ShipWorks.Shipping.Carriers.Postal
{
    partial class PostalProfileControlBase
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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage = new System.Windows.Forms.TabPage();
			this.groupExpressMail = new System.Windows.Forms.GroupBox();
			this.expressSignatureRequirement = new System.Windows.Forms.CheckBox();
			this.expressSignatureRequirementState = new System.Windows.Forms.CheckBox();
			this.kryptonBorderEdge5 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
			this.groupReturns = new System.Windows.Forms.GroupBox();
			this.includeReturn = new System.Windows.Forms.CheckBox();
			this.applyReturnProfileState = new System.Windows.Forms.CheckBox();
			this.includeReturnState = new System.Windows.Forms.CheckBox();
			this.pictureBoxReturnWarning = new System.Windows.Forms.PictureBox();
			this.labelReturnWarning = new System.Windows.Forms.Label();
			this.applyReturnProfile = new System.Windows.Forms.CheckBox();
			this.returnProfileID = new ShipWorks.UI.Controls.MultiValueComboBox();
			this.returnProfileIDLabel = new System.Windows.Forms.Label();
			this.returnShipment = new System.Windows.Forms.CheckBox();
			this.returnState = new System.Windows.Forms.CheckBox();
			this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
			this.groupInsurance = new System.Windows.Forms.GroupBox();
			this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
			this.insuranceState = new System.Windows.Forms.CheckBox();
			this.kryptonBorderEdge4 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
			this.groupBoxCustoms = new System.Windows.Forms.GroupBox();
			this.contentDescription = new ShipWorks.UI.Controls.MultiValueTextBox();
			this.contentType = new ShipWorks.UI.Controls.MultiValueComboBox();
			this.labelContent = new System.Windows.Forms.Label();
			this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
			this.customsContentState = new System.Windows.Forms.CheckBox();
			this.customsRecipientTinState = new System.Windows.Forms.CheckBox();
			this.labelCustomsRecipientTin = new System.Windows.Forms.Label();
			this.customsRecipientTin = new ShipWorks.UI.Controls.MultiValueTextBox();
			this.groupBoxFrom = new System.Windows.Forms.GroupBox();
			this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
			this.labelSender = new System.Windows.Forms.Label();
			this.originCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
			this.senderState = new System.Windows.Forms.CheckBox();
			this.groupShipment = new System.Windows.Forms.GroupBox();
			this.nonRectangular = new System.Windows.Forms.CheckBox();
			this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
			this.dimensionsState = new System.Windows.Forms.CheckBox();
			this.machinableState = new System.Windows.Forms.CheckBox();
			this.packagingState = new System.Windows.Forms.CheckBox();
			this.weightState = new System.Windows.Forms.CheckBox();
			this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
			this.nonMachinable = new System.Windows.Forms.CheckBox();
			this.labelDimensions = new System.Windows.Forms.Label();
			this.packagingType = new ShipWorks.UI.Controls.MultiValueComboBox();
			this.labelPackaging = new System.Windows.Forms.Label();
			this.weight = new ShipWorks.UI.Controls.WeightControl();
			this.labelWeight = new System.Windows.Forms.Label();
			this.labelService = new System.Windows.Forms.Label();
			this.labelConfirmation = new System.Windows.Forms.Label();
			this.confirmation = new ShipWorks.UI.Controls.MultiValueComboBox();
			this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
			this.confirmationState = new System.Windows.Forms.CheckBox();
			this.serviceState = new System.Windows.Forms.CheckBox();
			this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
			this.customsITNstate = new System.Windows.Forms.CheckBox();
			this.labelCustomsITN = new System.Windows.Forms.Label();
			this.customsITN = new ShipWorks.UI.Controls.MultiValueTextBox();
			this.tabControl.SuspendLayout();
			this.tabPage.SuspendLayout();
			this.groupExpressMail.SuspendLayout();
			this.groupReturns.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxReturnWarning)).BeginInit();
			this.groupInsurance.SuspendLayout();
			this.groupBoxCustoms.SuspendLayout();
			this.groupBoxFrom.SuspendLayout();
			this.groupShipment.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPage);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(439, 659);
			this.tabControl.TabIndex = 0;
			// 
			// tabPage
			// 
			this.tabPage.AutoScroll = true;
			this.tabPage.Controls.Add(this.groupExpressMail);
			this.tabPage.Controls.Add(this.groupReturns);
			this.tabPage.Controls.Add(this.groupInsurance);
			this.tabPage.Controls.Add(this.groupBoxCustoms);
			this.tabPage.Controls.Add(this.groupBoxFrom);
			this.tabPage.Controls.Add(this.groupShipment);
			this.tabPage.Location = new System.Drawing.Point(4, 22);
			this.tabPage.Name = "tabPage";
			this.tabPage.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage.Size = new System.Drawing.Size(431, 633);
			this.tabPage.TabIndex = 0;
			this.tabPage.Text = "Settings";
			this.tabPage.UseVisualStyleBackColor = true;
			// 
			// groupExpressMail
			// 
			this.groupExpressMail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupExpressMail.Controls.Add(this.expressSignatureRequirement);
			this.groupExpressMail.Controls.Add(this.expressSignatureRequirementState);
			this.groupExpressMail.Controls.Add(this.kryptonBorderEdge5);
			this.groupExpressMail.Location = new System.Drawing.Point(8, 492);
			this.groupExpressMail.Name = "groupExpressMail";
			this.groupExpressMail.Size = new System.Drawing.Size(400, 53);
			this.groupExpressMail.TabIndex = 4;
			this.groupExpressMail.TabStop = false;
			this.groupExpressMail.Text = "Priority Mail Express";
			// 
			// expressSignatureRequirement
			// 
			this.expressSignatureRequirement.AutoSize = true;
			this.expressSignatureRequirement.Location = new System.Drawing.Point(47, 22);
			this.expressSignatureRequirement.Name = "expressSignatureRequirement";
			this.expressSignatureRequirement.Size = new System.Drawing.Size(281, 17);
			this.expressSignatureRequirement.TabIndex = 1;
			this.expressSignatureRequirement.Text = "Waive signature requirement for Priority Mail Express";
			this.expressSignatureRequirement.UseVisualStyleBackColor = true;
			// 
			// expressSignatureRequirementState
			// 
			this.expressSignatureRequirementState.AutoSize = true;
			this.expressSignatureRequirementState.Location = new System.Drawing.Point(9, 23);
			this.expressSignatureRequirementState.Name = "expressSignatureRequirementState";
			this.expressSignatureRequirementState.Size = new System.Drawing.Size(15, 14);
			this.expressSignatureRequirementState.TabIndex = 0;
			this.expressSignatureRequirementState.UseVisualStyleBackColor = true;
			// 
			// kryptonBorderEdge5
			// 
			this.kryptonBorderEdge5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.kryptonBorderEdge5.AutoSize = false;
			this.kryptonBorderEdge5.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
			this.kryptonBorderEdge5.Location = new System.Drawing.Point(29, 20);
			this.kryptonBorderEdge5.Name = "kryptonBorderEdge5";
			this.kryptonBorderEdge5.Size = new System.Drawing.Size(1, 23);
			this.kryptonBorderEdge5.Text = "kryptonBorderEdge1";
			// 
			// groupReturns
			// 
			this.groupReturns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupReturns.Controls.Add(this.includeReturn);
			this.groupReturns.Controls.Add(this.applyReturnProfileState);
			this.groupReturns.Controls.Add(this.includeReturnState);
			this.groupReturns.Controls.Add(this.pictureBoxReturnWarning);
			this.groupReturns.Controls.Add(this.labelReturnWarning);
			this.groupReturns.Controls.Add(this.applyReturnProfile);
			this.groupReturns.Controls.Add(this.returnProfileID);
			this.groupReturns.Controls.Add(this.returnProfileIDLabel);
			this.groupReturns.Controls.Add(this.returnShipment);
			this.groupReturns.Controls.Add(this.returnState);
			this.groupReturns.Controls.Add(this.kryptonBorderEdge3);
			this.groupReturns.Location = new System.Drawing.Point(8, 552);
			this.groupReturns.Name = "groupReturns";
			this.groupReturns.Size = new System.Drawing.Size(400, 117);
			this.groupReturns.TabIndex = 5;
			this.groupReturns.TabStop = false;
			this.groupReturns.Text = "Return Shipment";
			// 
			// includeReturn
			// 
			this.includeReturn.AutoSize = true;
			this.includeReturn.BackColor = System.Drawing.Color.White;
			this.includeReturn.Location = new System.Drawing.Point(47, 22);
			this.includeReturn.Name = "includeReturn";
			this.includeReturn.Size = new System.Drawing.Size(193, 17);
			this.includeReturn.TabIndex = 1;
			this.includeReturn.Text = "Automatically include a return label";
			this.includeReturn.UseVisualStyleBackColor = false;
			this.includeReturn.CheckedChanged += new System.EventHandler(this.OnIncludeReturnChanged);
			// 
			// applyReturnProfileState
			// 
			this.applyReturnProfileState.AutoSize = true;
			this.applyReturnProfileState.Location = new System.Drawing.Point(9, 44);
			this.applyReturnProfileState.Name = "applyReturnProfileState";
			this.applyReturnProfileState.Size = new System.Drawing.Size(15, 14);
			this.applyReturnProfileState.TabIndex = 2;
			this.applyReturnProfileState.UseVisualStyleBackColor = true;
			// 
			// includeReturnState
			// 
			this.includeReturnState.AutoSize = true;
			this.includeReturnState.Location = new System.Drawing.Point(9, 23);
			this.includeReturnState.Name = "includeReturnState";
			this.includeReturnState.Size = new System.Drawing.Size(15, 14);
			this.includeReturnState.TabIndex = 0;
			this.includeReturnState.UseVisualStyleBackColor = true;
			// 
			// pictureBoxReturnWarning
			// 
			this.pictureBoxReturnWarning.BackColor = System.Drawing.Color.White;
			this.pictureBoxReturnWarning.Image = global::ShipWorks.Properties.Resources.exclamation16;
			this.pictureBoxReturnWarning.Location = new System.Drawing.Point(242, 22);
			this.pictureBoxReturnWarning.Name = "pictureBoxReturnWarning";
			this.pictureBoxReturnWarning.Size = new System.Drawing.Size(16, 16);
			this.pictureBoxReturnWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBoxReturnWarning.TabIndex = 81;
			this.pictureBoxReturnWarning.TabStop = false;
			// 
			// labelReturnWarning
			// 
			this.labelReturnWarning.AutoSize = true;
			this.labelReturnWarning.BackColor = System.Drawing.Color.White;
			this.labelReturnWarning.ForeColor = System.Drawing.Color.DimGray;
			this.labelReturnWarning.Location = new System.Drawing.Point(259, 23);
			this.labelReturnWarning.Name = "labelReturnWarning";
			this.labelReturnWarning.Size = new System.Drawing.Size(208, 13);
			this.labelReturnWarning.TabIndex = 82;
			this.labelReturnWarning.Text = "Rates do not include return label charges.";
			// 
			// applyReturnProfile
			// 
			this.applyReturnProfile.AutoSize = true;
			this.applyReturnProfile.BackColor = System.Drawing.Color.White;
			this.applyReturnProfile.Location = new System.Drawing.Point(71, 43);
			this.applyReturnProfile.Name = "applyReturnProfile";
			this.applyReturnProfile.Size = new System.Drawing.Size(206, 17);
			this.applyReturnProfile.TabIndex = 3;
			this.applyReturnProfile.Text = "Apply a profile to the return shipment";
			this.applyReturnProfile.UseVisualStyleBackColor = false;
			// 
			// returnProfileID
			// 
			this.returnProfileID.BackColor = System.Drawing.Color.White;
			this.returnProfileID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.returnProfileID.FormattingEnabled = true;
			this.returnProfileID.Location = new System.Drawing.Point(154, 62);
			this.returnProfileID.Name = "returnProfileID";
			this.returnProfileID.PromptText = "(Multiple Values)";
			this.returnProfileID.Size = new System.Drawing.Size(150, 21);
			this.returnProfileID.TabIndex = 4;
			this.returnProfileID.DropDown += new System.EventHandler(this.OnReturnProfileIDOpened);
			this.returnProfileID.EnabledChanged += new System.EventHandler(this.OnReturnProfileIDEnabledChanged);
			// 
			// returnProfileIDLabel
			// 
			this.returnProfileIDLabel.AutoSize = true;
			this.returnProfileIDLabel.BackColor = System.Drawing.Color.White;
			this.returnProfileIDLabel.Location = new System.Drawing.Point(71, 65);
			this.returnProfileIDLabel.Name = "returnProfileIDLabel";
			this.returnProfileIDLabel.Size = new System.Drawing.Size(77, 13);
			this.returnProfileIDLabel.TabIndex = 77;
			this.returnProfileIDLabel.Text = "Return Profile:";
			// 
			// returnShipment
			// 
			this.returnShipment.AutoSize = true;
			this.returnShipment.Location = new System.Drawing.Point(47, 86);
			this.returnShipment.Name = "returnShipment";
			this.returnShipment.Size = new System.Drawing.Size(159, 17);
			this.returnShipment.TabIndex = 6;
			this.returnShipment.Text = "Make this a return shipment";
			this.returnShipment.UseVisualStyleBackColor = true;
			this.returnShipment.CheckedChanged += new System.EventHandler(this.OnReturnShipmentChanged);
			// 
			// returnState
			// 
			this.returnState.AutoSize = true;
			this.returnState.Location = new System.Drawing.Point(9, 87);
			this.returnState.Name = "returnState";
			this.returnState.Size = new System.Drawing.Size(15, 14);
			this.returnState.TabIndex = 5;
			this.returnState.UseVisualStyleBackColor = true;
			// 
			// kryptonBorderEdge3
			// 
			this.kryptonBorderEdge3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.kryptonBorderEdge3.AutoSize = false;
			this.kryptonBorderEdge3.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
			this.kryptonBorderEdge3.Location = new System.Drawing.Point(29, 20);
			this.kryptonBorderEdge3.Name = "kryptonBorderEdge3";
			this.kryptonBorderEdge3.Size = new System.Drawing.Size(1, 87);
			this.kryptonBorderEdge3.Text = "kryptonBorderEdge1";
			// 
			// groupInsurance
			// 
			this.groupInsurance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupInsurance.Controls.Add(this.insuranceControl);
			this.groupInsurance.Controls.Add(this.insuranceState);
			this.groupInsurance.Controls.Add(this.kryptonBorderEdge4);
			this.groupInsurance.Location = new System.Drawing.Point(8, 292);
			this.groupInsurance.Name = "groupInsurance";
			this.groupInsurance.Size = new System.Drawing.Size(400, 82);
			this.groupInsurance.TabIndex = 2;
			this.groupInsurance.TabStop = false;
			this.groupInsurance.Text = "Insurance";
			// 
			// insuranceControl
			// 
			this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.insuranceControl.Location = new System.Drawing.Point(44, 21);
			this.insuranceControl.Name = "insuranceControl";
			this.insuranceControl.Size = new System.Drawing.Size(316, 52);
			this.insuranceControl.TabIndex = 1;
			// 
			// insuranceState
			// 
			this.insuranceState.AutoSize = true;
			this.insuranceState.Location = new System.Drawing.Point(9, 25);
			this.insuranceState.Name = "insuranceState";
			this.insuranceState.Size = new System.Drawing.Size(15, 14);
			this.insuranceState.TabIndex = 0;
			this.insuranceState.UseVisualStyleBackColor = true;
			// 
			// kryptonBorderEdge4
			// 
			this.kryptonBorderEdge4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.kryptonBorderEdge4.AutoSize = false;
			this.kryptonBorderEdge4.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
			this.kryptonBorderEdge4.Location = new System.Drawing.Point(29, 20);
			this.kryptonBorderEdge4.Name = "kryptonBorderEdge4";
			this.kryptonBorderEdge4.Size = new System.Drawing.Size(1, 52);
			this.kryptonBorderEdge4.Text = "kryptonBorderEdge1";
			// 
			// groupBoxCustoms
			// 
			this.groupBoxCustoms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxCustoms.Controls.Add(this.customsITNstate);
			this.groupBoxCustoms.Controls.Add(this.labelCustomsITN);
			this.groupBoxCustoms.Controls.Add(this.customsITN);
			this.groupBoxCustoms.Controls.Add(this.contentDescription);
			this.groupBoxCustoms.Controls.Add(this.contentType);
			this.groupBoxCustoms.Controls.Add(this.labelContent);
			this.groupBoxCustoms.Controls.Add(this.kryptonBorderEdge2);
			this.groupBoxCustoms.Controls.Add(this.customsContentState);
			this.groupBoxCustoms.Controls.Add(this.customsRecipientTinState);
			this.groupBoxCustoms.Controls.Add(this.labelCustomsRecipientTin);
			this.groupBoxCustoms.Controls.Add(this.customsRecipientTin);
			this.groupBoxCustoms.Location = new System.Drawing.Point(8, 380);
			this.groupBoxCustoms.Name = "groupBoxCustoms";
			this.groupBoxCustoms.Size = new System.Drawing.Size(400, 107);
			this.groupBoxCustoms.TabIndex = 3;
			this.groupBoxCustoms.TabStop = false;
			this.groupBoxCustoms.Text = "Customs";
			// 
			// contentDescription
			// 
			this.contentDescription.Location = new System.Drawing.Point(252, 18);
			this.contentDescription.MaxLength = 15;
			this.fieldLengthProvider.SetMaxLengthSource(this.contentDescription, ShipWorks.Data.Utility.EntityFieldLengthSource.PostalCustomsDescription);
			this.contentDescription.Name = "contentDescription";
			this.contentDescription.Size = new System.Drawing.Size(154, 21);
			this.contentDescription.TabIndex = 3;
			this.contentDescription.Visible = false;
			// 
			// contentType
			// 
			this.contentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.contentType.FormattingEnabled = true;
			this.contentType.Location = new System.Drawing.Point(147, 17);
			this.contentType.Name = "contentType";
			this.contentType.PromptText = "(Multiple Values)";
			this.contentType.Size = new System.Drawing.Size(136, 21);
			this.contentType.TabIndex = 1;
			this.contentType.SelectedIndexChanged += new System.EventHandler(this.OnContentTypeChanged);
			// 
			// labelContent
			// 
			this.labelContent.AutoSize = true;
			this.labelContent.BackColor = System.Drawing.Color.Transparent;
			this.labelContent.Location = new System.Drawing.Point(90, 20);
			this.labelContent.Name = "labelContent";
			this.labelContent.Size = new System.Drawing.Size(50, 13);
			this.labelContent.TabIndex = 17;
			this.labelContent.Text = "Content:";
			// 
			// kryptonBorderEdge2
			// 
			this.kryptonBorderEdge2.AutoSize = false;
			this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
			this.kryptonBorderEdge2.Location = new System.Drawing.Point(29, 17);
			this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
			this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 80);
			this.kryptonBorderEdge2.Text = "kryptonBorderEdge2";
			// 
			// customsContentState
			// 
			this.customsContentState.AutoSize = true;
			this.customsContentState.Checked = true;
			this.customsContentState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.customsContentState.Location = new System.Drawing.Point(9, 20);
			this.customsContentState.Name = "customsContentState";
			this.customsContentState.Size = new System.Drawing.Size(15, 14);
			this.customsContentState.TabIndex = 0;
			this.customsContentState.Tag = "";
			this.customsContentState.UseVisualStyleBackColor = true;
			// 
			// customsRecipientTinState
			// 
			this.customsRecipientTinState.AutoSize = true;
			this.customsRecipientTinState.Location = new System.Drawing.Point(9, 48);
			this.customsRecipientTinState.Name = "customsRecipientTinState";
			this.customsRecipientTinState.Size = new System.Drawing.Size(15, 14);
			this.customsRecipientTinState.TabIndex = 2;
			this.customsRecipientTinState.Tag = "";
			this.customsRecipientTinState.UseVisualStyleBackColor = true;
			// 
			// labelCustomsRecipientTin
			// 
			this.labelCustomsRecipientTin.AutoSize = true;
			this.labelCustomsRecipientTin.BackColor = System.Drawing.Color.Transparent;
			this.labelCustomsRecipientTin.Location = new System.Drawing.Point(52, 48);
			this.labelCustomsRecipientTin.Name = "labelCustomsRecipientTin";
			this.labelCustomsRecipientTin.Size = new System.Drawing.Size(90, 13);
			this.labelCustomsRecipientTin.TabIndex = 95;
			this.labelCustomsRecipientTin.Text = "Recipient Tax ID:";
			// 
			// customsRecipientTin
			// 
			this.customsRecipientTin.Location = new System.Drawing.Point(147, 45);
			this.customsRecipientTin.Name = "customsRecipientTin";
			this.customsRecipientTin.Size = new System.Drawing.Size(136, 21);
			this.customsRecipientTin.TabIndex = 95;
			// 
			// groupBoxFrom
			// 
			this.groupBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxFrom.Controls.Add(this.kryptonBorderEdge1);
			this.groupBoxFrom.Controls.Add(this.labelSender);
			this.groupBoxFrom.Controls.Add(this.originCombo);
			this.groupBoxFrom.Controls.Add(this.senderState);
			this.groupBoxFrom.Location = new System.Drawing.Point(8, 6);
			this.groupBoxFrom.Name = "groupBoxFrom";
			this.groupBoxFrom.Size = new System.Drawing.Size(400, 52);
			this.groupBoxFrom.TabIndex = 0;
			this.groupBoxFrom.TabStop = false;
			this.groupBoxFrom.Text = "From";
			// 
			// kryptonBorderEdge1
			// 
			this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.kryptonBorderEdge1.AutoSize = false;
			this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
			this.kryptonBorderEdge1.Location = new System.Drawing.Point(29, 17);
			this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
			this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 24);
			this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
			// 
			// labelSender
			// 
			this.labelSender.AutoSize = true;
			this.labelSender.Location = new System.Drawing.Point(65, 20);
			this.labelSender.Name = "labelSender";
			this.labelSender.Size = new System.Drawing.Size(39, 13);
			this.labelSender.TabIndex = 12;
			this.labelSender.Text = "Origin:";
			// 
			// originCombo
			// 
			this.originCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.originCombo.FormattingEnabled = true;
			this.originCombo.Location = new System.Drawing.Point(110, 17);
			this.originCombo.Name = "originCombo";
			this.originCombo.PromptText = "(Multiple Values)";
			this.originCombo.Size = new System.Drawing.Size(206, 21);
			this.originCombo.TabIndex = 1;
			// 
			// senderState
			// 
			this.senderState.AutoSize = true;
			this.senderState.Checked = true;
			this.senderState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.senderState.Location = new System.Drawing.Point(9, 20);
			this.senderState.Name = "senderState";
			this.senderState.Size = new System.Drawing.Size(15, 14);
			this.senderState.TabIndex = 0;
			this.senderState.Tag = "";
			this.senderState.UseVisualStyleBackColor = true;
			// 
			// groupShipment
			// 
			this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupShipment.Controls.Add(this.nonRectangular);
			this.groupShipment.Controls.Add(this.kryptonBorderEdge);
			this.groupShipment.Controls.Add(this.dimensionsState);
			this.groupShipment.Controls.Add(this.machinableState);
			this.groupShipment.Controls.Add(this.packagingState);
			this.groupShipment.Controls.Add(this.weightState);
			this.groupShipment.Controls.Add(this.dimensionsControl);
			this.groupShipment.Controls.Add(this.nonMachinable);
			this.groupShipment.Controls.Add(this.labelDimensions);
			this.groupShipment.Controls.Add(this.packagingType);
			this.groupShipment.Controls.Add(this.labelPackaging);
			this.groupShipment.Controls.Add(this.weight);
			this.groupShipment.Controls.Add(this.labelWeight);
			this.groupShipment.Controls.Add(this.labelService);
			this.groupShipment.Controls.Add(this.labelConfirmation);
			this.groupShipment.Controls.Add(this.confirmation);
			this.groupShipment.Controls.Add(this.service);
			this.groupShipment.Controls.Add(this.confirmationState);
			this.groupShipment.Controls.Add(this.serviceState);
			this.groupShipment.Location = new System.Drawing.Point(8, 64);
			this.groupShipment.Name = "groupShipment";
			this.groupShipment.Size = new System.Drawing.Size(400, 223);
			this.groupShipment.TabIndex = 1;
			this.groupShipment.TabStop = false;
			this.groupShipment.Text = "Shipment";
			// 
			// nonRectangular
			// 
			this.nonRectangular.AutoSize = true;
			this.nonRectangular.BackColor = System.Drawing.Color.Transparent;
			this.nonRectangular.Location = new System.Drawing.Point(212, 120);
			this.nonRectangular.Name = "nonRectangular";
			this.nonRectangular.Size = new System.Drawing.Size(107, 17);
			this.nonRectangular.TabIndex = 10;
			this.nonRectangular.Text = "Non-Rectangular";
			this.nonRectangular.UseVisualStyleBackColor = false;
			// 
			// kryptonBorderEdge
			// 
			this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.kryptonBorderEdge.AutoSize = false;
			this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
			this.kryptonBorderEdge.Location = new System.Drawing.Point(29, 18);
			this.kryptonBorderEdge.Name = "kryptonBorderEdge";
			this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 190);
			this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
			// 
			// dimensionsState
			// 
			this.dimensionsState.AutoSize = true;
			this.dimensionsState.Checked = true;
			this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.dimensionsState.Location = new System.Drawing.Point(9, 144);
			this.dimensionsState.Name = "dimensionsState";
			this.dimensionsState.Size = new System.Drawing.Size(15, 14);
			this.dimensionsState.TabIndex = 11;
			this.dimensionsState.Tag = "";
			this.dimensionsState.UseVisualStyleBackColor = true;
			// 
			// machinableState
			// 
			this.machinableState.AutoSize = true;
			this.machinableState.Checked = true;
			this.machinableState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.machinableState.Location = new System.Drawing.Point(9, 121);
			this.machinableState.Name = "machinableState";
			this.machinableState.Size = new System.Drawing.Size(15, 14);
			this.machinableState.TabIndex = 8;
			this.machinableState.Tag = "";
			this.machinableState.UseVisualStyleBackColor = true;
			// 
			// packagingState
			// 
			this.packagingState.AutoSize = true;
			this.packagingState.Checked = true;
			this.packagingState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.packagingState.Location = new System.Drawing.Point(9, 97);
			this.packagingState.Name = "packagingState";
			this.packagingState.Size = new System.Drawing.Size(15, 14);
			this.packagingState.TabIndex = 6;
			this.packagingState.Tag = "";
			this.packagingState.UseVisualStyleBackColor = true;
			// 
			// weightState
			// 
			this.weightState.AutoSize = true;
			this.weightState.Checked = true;
			this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.weightState.Location = new System.Drawing.Point(9, 70);
			this.weightState.Name = "weightState";
			this.weightState.Size = new System.Drawing.Size(15, 14);
			this.weightState.TabIndex = 4;
			this.weightState.Tag = "";
			this.weightState.UseVisualStyleBackColor = true;
			// 
			// dimensionsControl
			// 
			this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
			this.dimensionsControl.Cleared = false;
			this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dimensionsControl.Location = new System.Drawing.Point(107, 138);
			this.dimensionsControl.Name = "dimensionsControl";
			this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
			this.dimensionsControl.TabIndex = 12;
			// 
			// nonMachinable
			// 
			this.nonMachinable.AutoSize = true;
			this.nonMachinable.BackColor = System.Drawing.Color.Transparent;
			this.nonMachinable.Location = new System.Drawing.Point(110, 120);
			this.nonMachinable.Name = "nonMachinable";
			this.nonMachinable.Size = new System.Drawing.Size(102, 17);
			this.nonMachinable.TabIndex = 9;
			this.nonMachinable.Text = "Non-Machinable";
			this.nonMachinable.UseVisualStyleBackColor = false;
			// 
			// labelDimensions
			// 
			this.labelDimensions.AutoSize = true;
			this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
			this.labelDimensions.Location = new System.Drawing.Point(40, 144);
			this.labelDimensions.Name = "labelDimensions";
			this.labelDimensions.Size = new System.Drawing.Size(64, 13);
			this.labelDimensions.TabIndex = 59;
			this.labelDimensions.Text = "Dimensions:";
			// 
			// packagingType
			// 
			this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.packagingType.FormattingEnabled = true;
			this.packagingType.Location = new System.Drawing.Point(110, 94);
			this.packagingType.Name = "packagingType";
			this.packagingType.PromptText = "(Multiple Values)";
			this.packagingType.Size = new System.Drawing.Size(144, 21);
			this.packagingType.TabIndex = 7;
			// 
			// labelPackaging
			// 
			this.labelPackaging.AutoSize = true;
			this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
			this.labelPackaging.Location = new System.Drawing.Point(45, 97);
			this.labelPackaging.Name = "labelPackaging";
			this.labelPackaging.Size = new System.Drawing.Size(59, 13);
			this.labelPackaging.TabIndex = 57;
			this.labelPackaging.Text = "Packaging:";
			// 
			// weight
			// 
			this.weight.AutoSize = true;
			this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.weight.BackColor = System.Drawing.Color.Transparent;
			this.weight.ConfigureTelemetryEntityCounts = null;
			this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.weight.Location = new System.Drawing.Point(110, 68);
			this.weight.Name = "weight";
			this.weight.RangeMax = 300D;
			this.weight.RangeMin = 0D;
			this.weight.Size = new System.Drawing.Size(269, 24);
			this.weight.TabIndex = 5;
			this.weight.Weight = 0D;
			// 
			// labelWeight
			// 
			this.labelWeight.AutoSize = true;
			this.labelWeight.BackColor = System.Drawing.Color.Transparent;
			this.labelWeight.Location = new System.Drawing.Point(59, 71);
			this.labelWeight.Name = "labelWeight";
			this.labelWeight.Size = new System.Drawing.Size(45, 13);
			this.labelWeight.TabIndex = 55;
			this.labelWeight.Text = "Weight:";
			// 
			// labelService
			// 
			this.labelService.AutoSize = true;
			this.labelService.BackColor = System.Drawing.Color.Transparent;
			this.labelService.Location = new System.Drawing.Point(56, 16);
			this.labelService.Name = "labelService";
			this.labelService.Size = new System.Drawing.Size(46, 13);
			this.labelService.TabIndex = 52;
			this.labelService.Text = "Service:";
			// 
			// labelConfirmation
			// 
			this.labelConfirmation.AutoSize = true;
			this.labelConfirmation.BackColor = System.Drawing.Color.Transparent;
			this.labelConfirmation.Location = new System.Drawing.Point(32, 43);
			this.labelConfirmation.Name = "labelConfirmation";
			this.labelConfirmation.Size = new System.Drawing.Size(72, 13);
			this.labelConfirmation.TabIndex = 51;
			this.labelConfirmation.Text = "Confirmation:";
			// 
			// confirmation
			// 
			this.confirmation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.confirmation.FormattingEnabled = true;
			this.confirmation.Location = new System.Drawing.Point(110, 40);
			this.confirmation.Name = "confirmation";
			this.confirmation.PromptText = "(Multiple Values)";
			this.confirmation.Size = new System.Drawing.Size(250, 21);
			this.confirmation.TabIndex = 3;
			// 
			// service
			// 
			this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.service.FormattingEnabled = true;
			this.service.Location = new System.Drawing.Point(110, 13);
			this.service.Name = "service";
			this.service.PromptText = "(Multiple Values)";
			this.service.Size = new System.Drawing.Size(250, 21);
			this.service.TabIndex = 1;
			// 
			// confirmationState
			// 
			this.confirmationState.AutoSize = true;
			this.confirmationState.Checked = true;
			this.confirmationState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.confirmationState.Location = new System.Drawing.Point(9, 45);
			this.confirmationState.Name = "confirmationState";
			this.confirmationState.Size = new System.Drawing.Size(15, 14);
			this.confirmationState.TabIndex = 2;
			this.confirmationState.Tag = "";
			this.confirmationState.UseVisualStyleBackColor = true;
			// 
			// serviceState
			// 
			this.serviceState.AutoSize = true;
			this.serviceState.Checked = true;
			this.serviceState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.serviceState.Location = new System.Drawing.Point(9, 20);
			this.serviceState.Name = "serviceState";
			this.serviceState.Size = new System.Drawing.Size(15, 14);
			this.serviceState.TabIndex = 0;
			this.serviceState.Tag = "";
			this.serviceState.UseVisualStyleBackColor = true;
			// 
			// customsITNstate
			// 
			this.customsITNstate.AutoSize = true;
			this.customsITNstate.Location = new System.Drawing.Point(9, 75);
			this.customsITNstate.Name = "customsITNstate";
			this.customsITNstate.Size = new System.Drawing.Size(15, 14);
			this.customsITNstate.TabIndex = 96;
			this.customsITNstate.Tag = "";
			this.customsITNstate.UseVisualStyleBackColor = true;
			// 
			// labelCustomsITN
			// 
			this.labelCustomsITN.AutoSize = true;
			this.labelCustomsITN.BackColor = System.Drawing.Color.Transparent;
			this.labelCustomsITN.Location = new System.Drawing.Point(112, 76);
			this.labelCustomsITN.Name = "labelCustomsITN";
			this.labelCustomsITN.Size = new System.Drawing.Size(28, 13);
			this.labelCustomsITN.TabIndex = 97;
			this.labelCustomsITN.Text = "ITN:";
			// 
			// customsITN
			// 
			this.customsITN.Location = new System.Drawing.Point(147, 72);
			this.customsITN.Name = "customsITN";
			this.customsITN.Size = new System.Drawing.Size(136, 21);
			this.customsITN.TabIndex = 98;
			// 
			// PostalProfileControlBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl);
			this.Name = "PostalProfileControlBase";
			this.Size = new System.Drawing.Size(439, 659);
			this.tabControl.ResumeLayout(false);
			this.tabPage.ResumeLayout(false);
			this.groupExpressMail.ResumeLayout(false);
			this.groupExpressMail.PerformLayout();
			this.groupReturns.ResumeLayout(false);
			this.groupReturns.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxReturnWarning)).EndInit();
			this.groupInsurance.ResumeLayout(false);
			this.groupInsurance.PerformLayout();
			this.groupBoxCustoms.ResumeLayout(false);
			this.groupBoxCustoms.PerformLayout();
			this.groupBoxFrom.ResumeLayout(false);
			this.groupBoxFrom.PerformLayout();
			this.groupShipment.ResumeLayout(false);
			this.groupShipment.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox customsRecipientTinState;
        private System.Windows.Forms.Label labelCustomsRecipientTin;
        private ShipWorks.UI.Controls.MultiValueTextBox customsRecipientTin;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.CheckBox confirmationState;
        private System.Windows.Forms.CheckBox serviceState;
        private ShipWorks.UI.Controls.MultiValueComboBox confirmation;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.Label labelConfirmation;
        private ShipWorks.Shipping.Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.CheckBox nonMachinable;
        private System.Windows.Forms.CheckBox nonRectangular;
        private System.Windows.Forms.Label labelDimensions;
        private ShipWorks.UI.Controls.MultiValueComboBox packagingType;
        private System.Windows.Forms.Label labelPackaging;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.CheckBox packagingState;
        private System.Windows.Forms.CheckBox weightState;
        private System.Windows.Forms.CheckBox dimensionsState;
        private System.Windows.Forms.CheckBox machinableState;
        private System.Windows.Forms.CheckBox customsContentState;
        private ShipWorks.UI.Controls.MultiValueTextBox contentDescription;
        private ShipWorks.UI.Controls.MultiValueComboBox contentType;
        private System.Windows.Forms.Label labelContent;
        protected System.Windows.Forms.GroupBox groupBoxFrom;
        protected System.Windows.Forms.GroupBox groupShipment;
        protected System.Windows.Forms.CheckBox senderState;
        protected System.Windows.Forms.Label labelSender;
        protected ShipWorks.UI.Controls.MultiValueComboBox originCombo;
        protected System.Windows.Forms.GroupBox groupBoxCustoms;
        protected System.Windows.Forms.TabPage tabPage;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        protected System.Windows.Forms.CheckBox returnShipment;
        protected System.Windows.Forms.CheckBox returnState;
        private System.Windows.Forms.CheckBox insuranceState;
        protected System.Windows.Forms.GroupBox groupReturns;
        protected System.Windows.Forms.GroupBox groupInsurance;
        protected System.Windows.Forms.GroupBox groupExpressMail;
        private System.Windows.Forms.CheckBox expressSignatureRequirement;
        private System.Windows.Forms.CheckBox expressSignatureRequirementState;
        protected Insurance.InsuranceProfileControl insuranceControl;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge4;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge5;
        protected System.Windows.Forms.CheckBox includeReturn;
        protected System.Windows.Forms.CheckBox applyReturnProfile;
        protected ShipWorks.UI.Controls.MultiValueComboBox returnProfileID;
        protected System.Windows.Forms.Label returnProfileIDLabel;
        protected System.Windows.Forms.Label labelReturnWarning;
        protected System.Windows.Forms.PictureBox pictureBoxReturnWarning;
        protected System.Windows.Forms.CheckBox includeReturnState;
        protected System.Windows.Forms.CheckBox applyReturnProfileState;
		private System.Windows.Forms.CheckBox customsITNstate;
		private System.Windows.Forms.Label labelCustomsITN;
		private UI.Controls.MultiValueTextBox customsITN;
	}
}
