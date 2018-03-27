using System.Security.AccessControl;
using Microsoft.Web.Services3.Security;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaProfileControl
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
            this.labelAccount = new System.Windows.Forms.Label();
            this.endiciaAccount = new System.Windows.Forms.ComboBox();
            this.stateAccount = new System.Windows.Forms.CheckBox();
            this.stateStealth = new System.Windows.Forms.CheckBox();
            this.hidePostage = new System.Windows.Forms.CheckBox();
            this.labelStealth = new System.Windows.Forms.Label();
            this.noPostage = new System.Windows.Forms.CheckBox();
            this.labelNoPostage = new System.Windows.Forms.Label();
            this.stateNoPostage = new System.Windows.Forms.CheckBox();
            this.groupBoxRubberStamps = new System.Windows.Forms.GroupBox();
            this.stateReferenceID = new System.Windows.Forms.CheckBox();
            this.stateRubberStamp3 = new System.Windows.Forms.CheckBox();
            this.stateRubberStamp2 = new System.Windows.Forms.CheckBox();
            this.stateRubberStamp1 = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge31 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelReferenceIdInfo = new System.Windows.Forms.Label();
            this.referenceID = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReferenceID = new System.Windows.Forms.Label();
            this.labelRubberStampWarning = new System.Windows.Forms.Label();
            this.rubberStamp3 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelRubberStamp3 = new System.Windows.Forms.Label();
            this.rubberStamp2 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelRubberStamp2 = new System.Windows.Forms.Label();
            this.rubberStamp1 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelRubberStamp1 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.groupParcelSelect = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge6 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelEntryFacility = new System.Windows.Forms.Label();
            this.entryFacility = new System.Windows.Forms.ComboBox();
            this.stateEntryFacility = new System.Windows.Forms.CheckBox();
            this.labelSortType = new System.Windows.Forms.Label();
            this.sortType = new System.Windows.Forms.ComboBox();
            this.stateSortType = new System.Windows.Forms.CheckBox();
            this.scanBasedPayment = new System.Windows.Forms.CheckBox();
            this.scanBasedPaymentState = new System.Windows.Forms.CheckBox();
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge11 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupBoxFrom.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.groupBoxCustoms.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.groupReturns.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupExpressMail.SuspendLayout();
            this.groupBoxRubberStamps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.groupParcelSelect.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Controls.Add(this.labelAccount);
            this.groupBoxFrom.Controls.Add(this.endiciaAccount);
            this.groupBoxFrom.Controls.Add(this.stateAccount);
            this.groupBoxFrom.Size = new System.Drawing.Size(411, 82);
            this.groupBoxFrom.Controls.SetChildIndex(this.stateAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.senderState, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.endiciaAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.originCombo, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelSender, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.kryptonBorderEdge1, 0);
            // 
            // groupShipment
            // 
            this.groupShipment.Controls.Add(this.stateNoPostage);
            this.groupShipment.Controls.Add(this.noPostage);
            this.groupShipment.Controls.Add(this.labelNoPostage);
            this.groupShipment.Controls.Add(this.hidePostage);
            this.groupShipment.Controls.Add(this.labelStealth);
            this.groupShipment.Controls.Add(this.stateStealth);
            this.groupShipment.Location = new System.Drawing.Point(8, 94);
            this.groupShipment.Size = new System.Drawing.Size(411, 264);
            this.groupShipment.Controls.SetChildIndex(this.kryptonBorderEdge, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateStealth, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelStealth, 0);
            this.groupShipment.Controls.SetChildIndex(this.hidePostage, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelNoPostage, 0);
            this.groupShipment.Controls.SetChildIndex(this.noPostage, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateNoPostage, 0);
            // 
            // senderState
            // 
            this.senderState.Location = new System.Drawing.Point(9, 47);
            // 
            // labelSender
            // 
            this.labelSender.Location = new System.Drawing.Point(65, 47);
            // 
            // originCombo
            // 
            this.originCombo.Location = new System.Drawing.Point(110, 44);
            this.originCombo.TabIndex = 3;
			// 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 54);
            // 
            // groupBoxCustoms
            // 
            this.groupBoxCustoms.Location = new System.Drawing.Point(8, 514);
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 232);
            // 
            // tabPage
            // 
            this.tabPage.Controls.Add(this.groupLabels);
            this.tabPage.Controls.Add(this.groupBoxRubberStamps);
            this.tabPage.Controls.Add(this.groupParcelSelect);
            this.tabPage.Size = new System.Drawing.Size(431, 957);
            this.tabPage.Controls.SetChildIndex(this.groupParcelSelect, 0);
            this.tabPage.Controls.SetChildIndex(this.groupReturns, 0);
            this.tabPage.Controls.SetChildIndex(this.groupExpressMail, 0);
            this.tabPage.Controls.SetChildIndex(this.groupBoxRubberStamps, 0);
            this.tabPage.Controls.SetChildIndex(this.groupBoxCustoms, 0);
            this.tabPage.Controls.SetChildIndex(this.groupInsurance, 0);
            this.tabPage.Controls.SetChildIndex(this.groupLabels, 0);
            this.tabPage.Controls.SetChildIndex(this.groupShipment, 0);
            this.tabPage.Controls.SetChildIndex(this.groupBoxFrom, 0);
            // 
            // returnShipment
            // 
            this.returnShipment.TabIndex = 1;
            // 
            // groupReturns
            // 
            this.groupReturns.Controls.Add(this.scanBasedPaymentState);
            this.groupReturns.Controls.Add(this.scanBasedPayment);
            this.groupReturns.Location = new System.Drawing.Point(8, 923);
            this.groupReturns.Size = new System.Drawing.Size(417, 76);
            this.groupReturns.Controls.SetChildIndex(this.returnShipment, 0);
            this.groupReturns.Controls.SetChildIndex(this.scanBasedPayment, 0);
            this.groupReturns.Controls.SetChildIndex(this.scanBasedPaymentState, 0);
            // 
            // groupInsurance
            // 
            this.groupInsurance.Location = new System.Drawing.Point(8, 427);
            this.groupInsurance.Controls.SetChildIndex(this.insuranceControl, 0);
            // 
            // groupExpressMail
            // 
            this.groupExpressMail.Location = new System.Drawing.Point(8, 761);
            // 
            // insuranceControl
            // 
            this.insuranceControl.TabIndex = 1;
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(52, 20);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 14;
            this.labelAccount.Text = "Account:";
            // 
            // endiciaAccount
            // 
            this.endiciaAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endiciaAccount.FormattingEnabled = true;
            this.endiciaAccount.Location = new System.Drawing.Point(110, 17);
            this.endiciaAccount.Name = "endiciaAccount";
            this.endiciaAccount.Size = new System.Drawing.Size(206, 21);
            this.endiciaAccount.TabIndex = 1;
            // 
            // stateAccount
            // 
            this.stateAccount.AutoSize = true;
            this.stateAccount.Location = new System.Drawing.Point(9, 20);
            this.stateAccount.Name = "stateAccount";
            this.stateAccount.Size = new System.Drawing.Size(15, 14);
            this.stateAccount.TabIndex = 0;
            this.stateAccount.UseVisualStyleBackColor = true;
            // 
            // stateStealth
            // 
            this.stateStealth.AutoSize = true;
            this.stateStealth.Location = new System.Drawing.Point(9, 213);
            this.stateStealth.Name = "stateStealth";
            this.stateStealth.Size = new System.Drawing.Size(15, 14);
            this.stateStealth.TabIndex = 0;
            this.stateStealth.UseVisualStyleBackColor = true;
            // 
            // hidePostage
            // 
            this.hidePostage.AutoSize = true;
            this.hidePostage.BackColor = System.Drawing.Color.Transparent;
            this.hidePostage.Location = new System.Drawing.Point(110, 213);
            this.hidePostage.Name = "hidePostage";
            this.hidePostage.Size = new System.Drawing.Size(89, 17);
            this.hidePostage.TabIndex = 1;
            this.hidePostage.Text = "Hide Postage";
            this.hidePostage.UseVisualStyleBackColor = false;
            // 
            // labelStealth
            // 
            this.labelStealth.AutoSize = true;
            this.labelStealth.BackColor = System.Drawing.Color.Transparent;
            this.labelStealth.Location = new System.Drawing.Point(56, 214);
            this.labelStealth.Name = "labelStealth";
            this.labelStealth.Size = new System.Drawing.Size(45, 13);
            this.labelStealth.TabIndex = 68;
            this.labelStealth.Text = "Stealth:";
            // 
            // noPostage
            // 
            this.noPostage.AutoSize = true;
            this.noPostage.BackColor = System.Drawing.Color.Transparent;
            this.noPostage.Location = new System.Drawing.Point(110, 236);
            this.noPostage.Name = "noPostage";
            this.noPostage.Size = new System.Drawing.Size(214, 17);
            this.noPostage.TabIndex = 3;
            this.noPostage.Text = "Generate label that is not postage-paid";
            this.noPostage.UseVisualStyleBackColor = false;
            // 
            // labelNoPostage
            // 
            this.labelNoPostage.AutoSize = true;
            this.labelNoPostage.BackColor = System.Drawing.Color.Transparent;
            this.labelNoPostage.Location = new System.Drawing.Point(35, 237);
            this.labelNoPostage.Name = "labelNoPostage";
            this.labelNoPostage.Size = new System.Drawing.Size(66, 13);
            this.labelNoPostage.TabIndex = 70;
            this.labelNoPostage.Text = "No Postage:";
            // 
            // stateNoPostage
            // 
            this.stateNoPostage.AutoSize = true;
            this.stateNoPostage.Location = new System.Drawing.Point(9, 237);
            this.stateNoPostage.Name = "stateNoPostage";
            this.stateNoPostage.Size = new System.Drawing.Size(15, 14);
            this.stateNoPostage.TabIndex = 2;
            this.stateNoPostage.UseVisualStyleBackColor = true;
            // 
            // groupBoxRubberStamps
            // 
            this.groupBoxRubberStamps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRubberStamps.Controls.Add(this.stateReferenceID);
            this.groupBoxRubberStamps.Controls.Add(this.stateRubberStamp3);
            this.groupBoxRubberStamps.Controls.Add(this.stateRubberStamp2);
            this.groupBoxRubberStamps.Controls.Add(this.stateRubberStamp1);
            this.groupBoxRubberStamps.Controls.Add(this.kryptonBorderEdge31);
            this.groupBoxRubberStamps.Controls.Add(this.labelReferenceIdInfo);
            this.groupBoxRubberStamps.Controls.Add(this.referenceID);
            this.groupBoxRubberStamps.Controls.Add(this.labelReferenceID);
            this.groupBoxRubberStamps.Controls.Add(this.labelRubberStampWarning);
            this.groupBoxRubberStamps.Controls.Add(this.rubberStamp3);
            this.groupBoxRubberStamps.Controls.Add(this.labelRubberStamp3);
            this.groupBoxRubberStamps.Controls.Add(this.rubberStamp2);
            this.groupBoxRubberStamps.Controls.Add(this.labelRubberStamp2);
            this.groupBoxRubberStamps.Controls.Add(this.rubberStamp1);
            this.groupBoxRubberStamps.Controls.Add(this.labelRubberStamp1);
            this.groupBoxRubberStamps.Location = new System.Drawing.Point(8, 574);
            this.groupBoxRubberStamps.Name = "groupBoxRubberStamps";
            this.groupBoxRubberStamps.Size = new System.Drawing.Size(411, 180);
            this.groupBoxRubberStamps.TabIndex = 4;
            this.groupBoxRubberStamps.TabStop = false;
            this.groupBoxRubberStamps.Text = "Rubber Stamps";
            // 
            // stateReferenceID
            // 
            this.stateReferenceID.AutoSize = true;
            this.stateReferenceID.Location = new System.Drawing.Point(9, 134);
            this.stateReferenceID.Name = "stateReferenceID";
            this.stateReferenceID.Size = new System.Drawing.Size(15, 14);
            this.stateReferenceID.TabIndex = 6;
            this.stateReferenceID.UseVisualStyleBackColor = true;
            // 
            // stateRubberStamp3
            // 
            this.stateRubberStamp3.AutoSize = true;
            this.stateRubberStamp3.Location = new System.Drawing.Point(9, 74);
            this.stateRubberStamp3.Name = "stateRubberStamp3";
            this.stateRubberStamp3.Size = new System.Drawing.Size(15, 14);
            this.stateRubberStamp3.TabIndex = 4;
            this.stateRubberStamp3.UseVisualStyleBackColor = true;
            // 
            // stateRubberStamp2
            // 
            this.stateRubberStamp2.AutoSize = true;
            this.stateRubberStamp2.Location = new System.Drawing.Point(9, 46);
            this.stateRubberStamp2.Name = "stateRubberStamp2";
            this.stateRubberStamp2.Size = new System.Drawing.Size(15, 14);
            this.stateRubberStamp2.TabIndex = 2;
            this.stateRubberStamp2.UseVisualStyleBackColor = true;
            // 
            // stateRubberStamp1
            // 
            this.stateRubberStamp1.AutoSize = true;
            this.stateRubberStamp1.Location = new System.Drawing.Point(9, 20);
            this.stateRubberStamp1.Name = "stateRubberStamp1";
            this.stateRubberStamp1.Size = new System.Drawing.Size(15, 14);
            this.stateRubberStamp1.TabIndex = 0;
            this.stateRubberStamp1.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge31
            // 
            this.kryptonBorderEdge31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge31.AutoSize = false;
            this.kryptonBorderEdge31.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge31.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge31.Name = "kryptonBorderEdge31";
            this.kryptonBorderEdge31.Size = new System.Drawing.Size(1, 150);
            this.kryptonBorderEdge31.Text = "kryptonBorderEdge1";
            //
            // kryptonBorderEdge3
            //
            this.kryptonBorderEdge3.Size = new System.Drawing.Size(1, 47);
            // 
            // labelReferenceIdInfo
            // 
            this.labelReferenceIdInfo.AutoSize = true;
            this.labelReferenceIdInfo.BackColor = System.Drawing.Color.White;
            this.labelReferenceIdInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelReferenceIdInfo.Location = new System.Drawing.Point(136, 155);
            this.labelReferenceIdInfo.Name = "labelReferenceIdInfo";
            this.labelReferenceIdInfo.Size = new System.Drawing.Size(247, 13);
            this.labelReferenceIdInfo.TabIndex = 95;
            this.labelReferenceIdInfo.Text = "This is used to lookup the shipment in Endicia logs.";
            // 
            // referenceID
            // 
            this.referenceID.Location = new System.Drawing.Point(139, 131);
            this.referenceID.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceID, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaReference);
            this.referenceID.Name = "referenceID";
            this.referenceID.Size = new System.Drawing.Size(236, 21);
            this.referenceID.TabIndex = 7;
            // 
            // labelReferenceID
            // 
            this.labelReferenceID.AutoSize = true;
            this.labelReferenceID.BackColor = System.Drawing.Color.White;
            this.labelReferenceID.Location = new System.Drawing.Point(58, 134);
            this.labelReferenceID.Name = "labelReferenceID";
            this.labelReferenceID.Size = new System.Drawing.Size(75, 13);
            this.labelReferenceID.TabIndex = 93;
            this.labelReferenceID.Text = "Reference ID:";
            // 
            // labelRubberStampWarning
            // 
            this.labelRubberStampWarning.AutoSize = true;
            this.labelRubberStampWarning.BackColor = System.Drawing.Color.White;
            this.labelRubberStampWarning.ForeColor = System.Drawing.Color.DimGray;
            this.labelRubberStampWarning.Location = new System.Drawing.Point(69, 100);
            this.labelRubberStampWarning.Name = "labelRubberStampWarning";
            this.labelRubberStampWarning.Size = new System.Drawing.Size(328, 13);
            this.labelRubberStampWarning.TabIndex = 92;
            this.labelRubberStampWarning.Text = "Express Mail and International labels do not display rubber stamps.";
            // 
            // rubberStamp3
            // 
            this.rubberStamp3.Location = new System.Drawing.Point(139, 72);
            this.rubberStamp3.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.rubberStamp3, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaRubberStamp);
            this.rubberStamp3.Name = "rubberStamp3";
            this.rubberStamp3.Size = new System.Drawing.Size(236, 21);
            this.rubberStamp3.TabIndex = 5;
            // 
            // labelRubberStamp3
            // 
            this.labelRubberStamp3.AutoSize = true;
            this.labelRubberStamp3.BackColor = System.Drawing.Color.White;
            this.labelRubberStamp3.Location = new System.Drawing.Point(48, 74);
            this.labelRubberStamp3.Name = "labelRubberStamp3";
            this.labelRubberStamp3.Size = new System.Drawing.Size(88, 13);
            this.labelRubberStamp3.TabIndex = 90;
            this.labelRubberStamp3.Text = "Rubber Stamp 3:";
            // 
            // rubberStamp2
            // 
            this.rubberStamp2.Location = new System.Drawing.Point(139, 45);
            this.rubberStamp2.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.rubberStamp2, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaRubberStamp);
            this.rubberStamp2.Name = "rubberStamp2";
            this.rubberStamp2.Size = new System.Drawing.Size(236, 21);
            this.rubberStamp2.TabIndex = 3;
            // 
            // labelRubberStamp2
            // 
            this.labelRubberStamp2.AutoSize = true;
            this.labelRubberStamp2.BackColor = System.Drawing.Color.White;
            this.labelRubberStamp2.Location = new System.Drawing.Point(48, 47);
            this.labelRubberStamp2.Name = "labelRubberStamp2";
            this.labelRubberStamp2.Size = new System.Drawing.Size(88, 13);
            this.labelRubberStamp2.TabIndex = 88;
            this.labelRubberStamp2.Text = "Rubber Stamp 2:";
            // 
            // rubberStamp1
            // 
            this.rubberStamp1.Location = new System.Drawing.Point(139, 18);
            this.rubberStamp1.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.rubberStamp1, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaRubberStamp);
            this.rubberStamp1.Name = "rubberStamp1";
            this.rubberStamp1.Size = new System.Drawing.Size(236, 21);
            this.rubberStamp1.TabIndex = 1;
            // 
            // labelRubberStamp1
            // 
            this.labelRubberStamp1.AutoSize = true;
            this.labelRubberStamp1.BackColor = System.Drawing.Color.White;
            this.labelRubberStamp1.Location = new System.Drawing.Point(48, 20);
            this.labelRubberStamp1.Name = "labelRubberStamp1";
            this.labelRubberStamp1.Size = new System.Drawing.Size(88, 13);
            this.labelRubberStamp1.TabIndex = 86;
            this.labelRubberStamp1.Text = "Rubber Stamp 1:";
            // 
            // groupParcelSelect
            // 
            this.groupParcelSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupParcelSelect.Controls.Add(this.kryptonBorderEdge6);
            this.groupParcelSelect.Controls.Add(this.labelEntryFacility);
            this.groupParcelSelect.Controls.Add(this.entryFacility);
            this.groupParcelSelect.Controls.Add(this.stateEntryFacility);
            this.groupParcelSelect.Controls.Add(this.labelSortType);
            this.groupParcelSelect.Controls.Add(this.sortType);
            this.groupParcelSelect.Controls.Add(this.stateSortType);
            this.groupParcelSelect.Location = new System.Drawing.Point(8, 822);
            this.groupParcelSelect.Name = "groupParcelSelect";
            this.groupParcelSelect.Size = new System.Drawing.Size(417, 90);
            this.groupParcelSelect.TabIndex = 10;
            this.groupParcelSelect.TabStop = false;
            this.groupParcelSelect.Text = "Parcel Select";
            // 
            // kryptonBorderEdge6
            // 
            this.kryptonBorderEdge6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge6.AutoSize = false;
            this.kryptonBorderEdge6.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge6.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge6.Name = "kryptonBorderEdge6";
            this.kryptonBorderEdge6.Size = new System.Drawing.Size(1, 60);
            this.kryptonBorderEdge6.Text = "kryptonBorderEdge1";
            // 
            // labelEntryFacility
            // 
            this.labelEntryFacility.AutoSize = true;
            this.labelEntryFacility.Location = new System.Drawing.Point(46, 56);
            this.labelEntryFacility.Name = "labelEntryFacility";
            this.labelEntryFacility.Size = new System.Drawing.Size(73, 13);
            this.labelEntryFacility.TabIndex = 21;
            this.labelEntryFacility.Text = "Entry Facility:";
            // 
            // entryFacility
            // 
            this.entryFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.entryFacility.FormattingEnabled = true;
            this.entryFacility.Location = new System.Drawing.Point(121, 53);
            this.entryFacility.Name = "entryFacility";
            this.entryFacility.Size = new System.Drawing.Size(206, 21);
            this.entryFacility.TabIndex = 3;
            // 
            // stateEntryFacility
            // 
            this.stateEntryFacility.AutoSize = true;
            this.stateEntryFacility.Location = new System.Drawing.Point(9, 56);
            this.stateEntryFacility.Name = "stateEntryFacility";
            this.stateEntryFacility.Size = new System.Drawing.Size(15, 14);
            this.stateEntryFacility.TabIndex = 2;
            this.stateEntryFacility.UseVisualStyleBackColor = true;
            // 
            // labelSortType
            // 
            this.labelSortType.AutoSize = true;
            this.labelSortType.Location = new System.Drawing.Point(60, 29);
            this.labelSortType.Name = "labelSortType";
            this.labelSortType.Size = new System.Drawing.Size(58, 13);
            this.labelSortType.TabIndex = 17;
            this.labelSortType.Text = "Sort Type:";
            // 
            // sortType
            // 
            this.sortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortType.FormattingEnabled = true;
            this.sortType.Location = new System.Drawing.Point(121, 26);
            this.sortType.Name = "sortType";
            this.sortType.Size = new System.Drawing.Size(206, 21);
            this.sortType.TabIndex = 1;
            // 
            // stateSortType
            // 
            this.stateSortType.AutoSize = true;
            this.stateSortType.Location = new System.Drawing.Point(9, 29);
            this.stateSortType.Name = "stateSortType";
            this.stateSortType.Size = new System.Drawing.Size(15, 14);
            this.stateSortType.TabIndex = 0;
            this.stateSortType.UseVisualStyleBackColor = true;
            // 
            // scanBasedPayment
            // 
            this.scanBasedPayment.AutoSize = true;
            this.scanBasedPayment.BackColor = System.Drawing.Color.Transparent;
            this.scanBasedPayment.Location = new System.Drawing.Point(47, 45);
            this.scanBasedPayment.Name = "scanBasedPayment";
            this.scanBasedPayment.Size = new System.Drawing.Size(127, 17);
            this.scanBasedPayment.TabIndex = 3;
            this.scanBasedPayment.Text = "Scan-Based Payment";
            this.scanBasedPayment.UseVisualStyleBackColor = false;
            // 
            // scanBasedPaymentState
            // 
            this.scanBasedPaymentState.AutoSize = true;
            this.scanBasedPaymentState.Location = new System.Drawing.Point(9, 46);
            this.scanBasedPaymentState.Name = "scanBasedPaymentState";
            this.scanBasedPaymentState.Size = new System.Drawing.Size(15, 14);
            this.scanBasedPaymentState.TabIndex = 2;
            this.scanBasedPaymentState.UseVisualStyleBackColor = true;
            // 
            // groupLabels
            // 
            this.groupLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabels.Controls.Add(this.requestedLabelFormat);
            this.groupLabels.Controls.Add(this.requestedLabelFormatState);
            this.groupLabels.Controls.Add(this.kryptonBorderEdge11);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Location = new System.Drawing.Point(8, 364);
            this.groupLabels.Size = new System.Drawing.Size(411, 58);
            this.groupLabels.TabIndex = 13;
            this.groupLabels.TabStop = false;
            this.groupLabels.Text = "Labels";
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(35, 22);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(267, 21);
            this.requestedLabelFormat.State = false;
            this.requestedLabelFormat.TabIndex = 1;
            // 
            // requestedLabelFormatState
            // 
            this.requestedLabelFormatState.AutoSize = true;
            this.requestedLabelFormatState.Location = new System.Drawing.Point(9, 25);
            this.requestedLabelFormatState.Name = "requestedLabelFormatState";
            this.requestedLabelFormatState.Size = new System.Drawing.Size(15, 14);
            this.requestedLabelFormatState.TabIndex = 0;
            this.requestedLabelFormatState.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge11
            // 
            this.kryptonBorderEdge11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge11.AutoSize = false;
            this.kryptonBorderEdge11.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge11.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge11.Name = "kryptonBorderEdge11";
            this.kryptonBorderEdge11.Size = new System.Drawing.Size(1, 28);
            this.kryptonBorderEdge11.Text = "kryptonBorderEdge11";
            // 
            // EndiciaProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "EndiciaProfileControl";
            this.Size = new System.Drawing.Size(439, 983);
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.groupBoxCustoms.ResumeLayout(false);
            this.groupBoxCustoms.PerformLayout();
            this.tabPage.ResumeLayout(false);
            this.groupReturns.ResumeLayout(false);
            this.groupReturns.PerformLayout();
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupExpressMail.ResumeLayout(false);
            this.groupExpressMail.PerformLayout();
            this.groupBoxRubberStamps.ResumeLayout(false);
            this.groupBoxRubberStamps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.groupParcelSelect.ResumeLayout(false);
            this.groupParcelSelect.PerformLayout();
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox endiciaAccount;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.CheckBox stateAccount;
        private System.Windows.Forms.CheckBox stateStealth;
        private System.Windows.Forms.CheckBox hidePostage;
        private System.Windows.Forms.Label labelStealth;
        private System.Windows.Forms.CheckBox noPostage;
        private System.Windows.Forms.Label labelNoPostage;
        private System.Windows.Forms.CheckBox stateNoPostage;
        private System.Windows.Forms.GroupBox groupBoxRubberStamps;
        private System.Windows.Forms.Label labelReferenceIdInfo;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox referenceID;
        private System.Windows.Forms.Label labelReferenceID;
        private System.Windows.Forms.Label labelRubberStampWarning;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox rubberStamp3;
        private System.Windows.Forms.Label labelRubberStamp3;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox rubberStamp2;
        private System.Windows.Forms.Label labelRubberStamp2;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox rubberStamp1;
        private System.Windows.Forms.Label labelRubberStamp1;
        private System.Windows.Forms.CheckBox stateReferenceID;
        private System.Windows.Forms.CheckBox stateRubberStamp3;
        private System.Windows.Forms.CheckBox stateRubberStamp2;
        private System.Windows.Forms.CheckBox stateRubberStamp1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.GroupBox groupParcelSelect;
        private System.Windows.Forms.Label labelEntryFacility;
        private System.Windows.Forms.CheckBox stateEntryFacility;
        private System.Windows.Forms.Label labelSortType;
        private System.Windows.Forms.CheckBox stateSortType;
        private System.Windows.Forms.ComboBox entryFacility;
        private System.Windows.Forms.ComboBox sortType;
        private System.Windows.Forms.CheckBox scanBasedPayment;
        private System.Windows.Forms.CheckBox scanBasedPaymentState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge31;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge6;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge11;
        protected System.Windows.Forms.GroupBox groupLabels;
        protected ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
    }
}
