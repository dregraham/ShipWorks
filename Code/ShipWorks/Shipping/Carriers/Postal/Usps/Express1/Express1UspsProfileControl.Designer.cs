﻿namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    partial class Express1UspsProfileControl
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory3 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.labelAccount = new System.Windows.Forms.Label();
            this.uspsAccount = new System.Windows.Forms.ComboBox();
            this.stateAccount = new System.Windows.Forms.CheckBox();
            this.stateStealth = new System.Windows.Forms.CheckBox();
            this.hidePostage = new System.Windows.Forms.CheckBox();
            this.labelStealth = new System.Windows.Forms.Label();
            this.groupTo = new System.Windows.Forms.GroupBox();
            this.requireFullAddressValidation = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge31 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelValidation = new System.Windows.Forms.Label();
            this.validationState = new System.Windows.Forms.CheckBox();
            this.memo1 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.stateMemo1 = new System.Windows.Forms.CheckBox();
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge11 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.memo2 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.memo3 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelMemo3 = new System.Windows.Forms.Label();
            this.labelMemo2 = new System.Windows.Forms.Label();
            this.labelMemo1 = new System.Windows.Forms.Label();
            this.stateMemo3 = new System.Windows.Forms.CheckBox();
            this.stateMemo2 = new System.Windows.Forms.CheckBox();
            this.groupBoxFrom.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.groupBoxCustoms.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.groupReturns.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupExpressMail.SuspendLayout();
            this.groupTo.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Controls.Add(this.labelAccount);
            this.groupBoxFrom.Controls.Add(this.uspsAccount);
            this.groupBoxFrom.Controls.Add(this.stateAccount);
            this.groupBoxFrom.Size = new System.Drawing.Size(383, 75);
            this.groupBoxFrom.Controls.SetChildIndex(this.stateAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.senderState, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.uspsAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.originCombo, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelSender, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.kryptonBorderEdge1, 0);
            // 
            // groupShipment
            // 
            this.groupShipment.Controls.Add(this.stateMemo3);
            this.groupShipment.Controls.Add(this.stateMemo2);
            this.groupShipment.Controls.Add(this.labelMemo3);
            this.groupShipment.Controls.Add(this.labelMemo2);
            this.groupShipment.Controls.Add(this.labelMemo1);
            this.groupShipment.Controls.Add(this.memo3);
            this.groupShipment.Controls.Add(this.memo2);
            this.groupShipment.Controls.Add(this.stateMemo1);
            this.groupShipment.Controls.Add(this.memo1);
            this.groupShipment.Controls.Add(this.hidePostage);
            this.groupShipment.Controls.Add(this.labelStealth);
            this.groupShipment.Controls.Add(this.stateStealth);
            this.groupShipment.Location = new System.Drawing.Point(8, 144);
            this.groupShipment.Size = new System.Drawing.Size(383, 328);
            this.groupShipment.TabIndex = 2;
            this.groupShipment.Controls.SetChildIndex(this.kryptonBorderEdge, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateStealth, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelStealth, 0);
            this.groupShipment.Controls.SetChildIndex(this.hidePostage, 0);
            this.groupShipment.Controls.SetChildIndex(this.memo1, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateMemo1, 0);
            this.groupShipment.Controls.SetChildIndex(this.memo2, 0);
            this.groupShipment.Controls.SetChildIndex(this.memo3, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelMemo1, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelMemo2, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelMemo3, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateMemo2, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateMemo3, 0);
            // 
            // senderState
            // 
            this.senderState.Location = new System.Drawing.Point(9, 47);
            this.senderState.TabIndex = 2;
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
            // groupBoxCustoms
            // 
            this.groupBoxCustoms.Location = new System.Drawing.Point(8, 628);
            this.groupBoxCustoms.Size = new System.Drawing.Size(400, 54);
            this.groupBoxCustoms.TabIndex = 5;
            // 
            // tabPage
            // 
            this.tabPage.Controls.Add(this.groupTo);
            this.tabPage.Controls.Add(this.groupLabels);
            this.tabPage.Size = new System.Drawing.Size(431, 757);
            this.tabPage.Controls.SetChildIndex(this.groupExpressMail, 0);
            this.tabPage.Controls.SetChildIndex(this.groupInsurance, 0);
            this.tabPage.Controls.SetChildIndex(this.groupLabels, 0);
            this.tabPage.Controls.SetChildIndex(this.groupReturns, 0);
            this.tabPage.Controls.SetChildIndex(this.groupShipment, 0);
            this.tabPage.Controls.SetChildIndex(this.groupBoxFrom, 0);
            this.tabPage.Controls.SetChildIndex(this.groupBoxCustoms, 0);
            this.tabPage.Controls.SetChildIndex(this.groupTo, 0);
            // 
            // groupReturns
            // 
            this.groupReturns.Location = new System.Drawing.Point(8, 749);
            this.groupReturns.Size = new System.Drawing.Size(383, 53);
            this.groupReturns.TabIndex = 7;
            this.groupReturns.Controls.SetChildIndex(this.returnShipment, 0);
            // 
            // groupInsurance
            // 
            this.groupInsurance.Location = new System.Drawing.Point(8, 540);
            this.groupInsurance.Size = new System.Drawing.Size(400, 82);
            this.groupInsurance.TabIndex = 4;
            this.groupInsurance.Controls.SetChildIndex(this.insuranceControl, 0);
            // 
            // groupExpressMail
            // 
            this.groupExpressMail.Location = new System.Drawing.Point(8, 689);
            this.groupExpressMail.Size = new System.Drawing.Size(400, 53);
            this.groupExpressMail.TabIndex = 6;
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 47);
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 296);
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
            // uspsAccount
            // 
            this.uspsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uspsAccount.FormattingEnabled = true;
            this.uspsAccount.Location = new System.Drawing.Point(110, 17);
            this.uspsAccount.Name = "uspsAccount";
            this.uspsAccount.Size = new System.Drawing.Size(144, 21);
            this.uspsAccount.TabIndex = 1;
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
            this.stateStealth.TabIndex = 13;
            this.stateStealth.UseVisualStyleBackColor = true;
            // 
            // hidePostage
            // 
            this.hidePostage.AutoSize = true;
            this.hidePostage.BackColor = System.Drawing.Color.Transparent;
            this.hidePostage.Location = new System.Drawing.Point(110, 213);
            this.hidePostage.Name = "hidePostage";
            this.hidePostage.Size = new System.Drawing.Size(89, 17);
            this.hidePostage.TabIndex = 14;
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
            // groupTo
            // 
            this.groupTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTo.Controls.Add(this.requireFullAddressValidation);
            this.groupTo.Controls.Add(this.kryptonBorderEdge31);
            this.groupTo.Controls.Add(this.labelValidation);
            this.groupTo.Controls.Add(this.validationState);
            this.groupTo.Location = new System.Drawing.Point(8, 86);
            this.groupTo.Name = "groupTo";
            this.groupTo.Size = new System.Drawing.Size(383, 52);
            this.groupTo.TabIndex = 1;
            this.groupTo.TabStop = false;
            this.groupTo.Text = "To";
            // 
            // requireFullAddressValidation
            // 
            this.requireFullAddressValidation.AutoSize = true;
            this.requireFullAddressValidation.Location = new System.Drawing.Point(110, 23);
            this.requireFullAddressValidation.Name = "requireFullAddressValidation";
            this.requireFullAddressValidation.Size = new System.Drawing.Size(198, 17);
            this.requireFullAddressValidation.TabIndex = 1;
            this.requireFullAddressValidation.Text = "Require full USPS address validation";
            this.requireFullAddressValidation.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge31
            // 
            this.kryptonBorderEdge31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge31.AutoSize = false;
            this.kryptonBorderEdge31.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge31.Location = new System.Drawing.Point(30, 14);
            this.kryptonBorderEdge31.Name = "kryptonBorderEdge31";
            this.kryptonBorderEdge31.Size = new System.Drawing.Size(1, 29);
            this.kryptonBorderEdge31.Text = "kryptonBorderEdge31";
            // 
            // labelValidation
            // 
            this.labelValidation.AutoSize = true;
            this.labelValidation.Location = new System.Drawing.Point(47, 23);
            this.labelValidation.Name = "labelValidation";
            this.labelValidation.Size = new System.Drawing.Size(57, 13);
            this.labelValidation.TabIndex = 15;
            this.labelValidation.Text = "Validation:";
            // 
            // validationState
            // 
            this.validationState.AutoSize = true;
            this.validationState.Checked = true;
            this.validationState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.validationState.Location = new System.Drawing.Point(9, 23);
            this.validationState.Name = "validationState";
            this.validationState.Size = new System.Drawing.Size(15, 14);
            this.validationState.TabIndex = 0;
            this.validationState.UseVisualStyleBackColor = true;
            // 
            // memo1
            // 
            this.memo1.Location = new System.Drawing.Point(107, 236);
            this.memo1.MaxLength = 32767;
            this.memo1.Name = "memo1";
            this.memo1.Size = new System.Drawing.Size(210, 21);
            this.memo1.TabIndex = 16;
            this.memo1.TokenSuggestionFactory = commonTokenSuggestionsFactory3;
            // 
            // stateMemo1
            // 
            this.stateMemo1.AutoSize = true;
            this.stateMemo1.Checked = true;
            this.stateMemo1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stateMemo1.Location = new System.Drawing.Point(9, 240);
            this.stateMemo1.Name = "stateMemo1";
            this.stateMemo1.Size = new System.Drawing.Size(15, 14);
            this.stateMemo1.TabIndex = 15;
            this.stateMemo1.UseVisualStyleBackColor = true;
            // 
            // groupLabels
            // 
            this.groupLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabels.Controls.Add(this.requestedLabelFormat);
            this.groupLabels.Controls.Add(this.requestedLabelFormatState);
            this.groupLabels.Controls.Add(this.kryptonBorderEdge11);
            this.groupLabels.Location = new System.Drawing.Point(8, 477);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Size = new System.Drawing.Size(383, 58);
            this.groupLabels.TabIndex = 3;
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
            // memo2
            // 
            this.memo2.Location = new System.Drawing.Point(107, 263);
            this.memo2.MaxLength = 32767;
            this.memo2.Name = "memo2";
            this.memo2.Size = new System.Drawing.Size(210, 21);
            this.memo2.TabIndex = 18;
            this.memo2.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            // 
            // memo3
            // 
            this.memo3.Location = new System.Drawing.Point(107, 290);
            this.memo3.MaxLength = 32767;
            this.memo3.Name = "memo3";
            this.memo3.Size = new System.Drawing.Size(210, 21);
            this.memo3.TabIndex = 20;
            this.memo3.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // labelMemo3
            // 
            this.labelMemo3.Location = new System.Drawing.Point(54, 290);
            this.labelMemo3.Name = "labelMemo3";
            this.labelMemo3.Size = new System.Drawing.Size(50, 21);
            this.labelMemo3.TabIndex = 78;
            this.labelMemo3.Text = "Memo 3:";
            this.labelMemo3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMemo2
            // 
            this.labelMemo2.Location = new System.Drawing.Point(54, 263);
            this.labelMemo2.Name = "labelMemo2";
            this.labelMemo2.Size = new System.Drawing.Size(50, 21);
            this.labelMemo2.TabIndex = 77;
            this.labelMemo2.Text = "Memo 2:";
            this.labelMemo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMemo1
            // 
            this.labelMemo1.Location = new System.Drawing.Point(54, 236);
            this.labelMemo1.Name = "labelMemo1";
            this.labelMemo1.Size = new System.Drawing.Size(50, 21);
            this.labelMemo1.TabIndex = 76;
            this.labelMemo1.Text = "Memo 1:";
            this.labelMemo1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stateMemo3
            // 
            this.stateMemo3.AutoSize = true;
            this.stateMemo3.Checked = true;
            this.stateMemo3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stateMemo3.Location = new System.Drawing.Point(9, 294);
            this.stateMemo3.Name = "stateMemo3";
            this.stateMemo3.Size = new System.Drawing.Size(15, 14);
            this.stateMemo3.TabIndex = 19;
            this.stateMemo3.UseVisualStyleBackColor = true;
            // 
            // stateMemo2
            // 
            this.stateMemo2.AutoSize = true;
            this.stateMemo2.Checked = true;
            this.stateMemo2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stateMemo2.Location = new System.Drawing.Point(9, 267);
            this.stateMemo2.Name = "stateMemo2";
            this.stateMemo2.Size = new System.Drawing.Size(15, 14);
            this.stateMemo2.TabIndex = 17;
            this.stateMemo2.UseVisualStyleBackColor = true;
            // 
            // Express1UspsProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Express1UspsProfileControl";
            this.Size = new System.Drawing.Size(439, 783);
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
            this.groupTo.ResumeLayout(false);
            this.groupTo.PerformLayout();
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox uspsAccount;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.CheckBox stateAccount;
        private System.Windows.Forms.CheckBox stateStealth;
        private System.Windows.Forms.CheckBox hidePostage;
        private System.Windows.Forms.Label labelStealth;
        protected System.Windows.Forms.GroupBox groupTo;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge31;
        private System.Windows.Forms.Label labelValidation;
        private System.Windows.Forms.CheckBox validationState;
        private System.Windows.Forms.CheckBox requireFullAddressValidation;
        private Templates.Tokens.TemplateTokenTextBox memo1;
        private System.Windows.Forms.CheckBox stateMemo1;
        protected System.Windows.Forms.GroupBox groupLabels;
        protected ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
        private Templates.Tokens.TemplateTokenTextBox memo3;
        private Templates.Tokens.TemplateTokenTextBox memo2;
        private System.Windows.Forms.Label labelMemo3;
        private System.Windows.Forms.Label labelMemo2;
        private System.Windows.Forms.Label labelMemo1;
        private System.Windows.Forms.CheckBox stateMemo3;
        private System.Windows.Forms.CheckBox stateMemo2;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge11;
    }
}
