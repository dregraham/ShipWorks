namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    partial class StampsProfileControl
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
            this.labelAccount = new System.Windows.Forms.Label();
            this.stampsAccount = new System.Windows.Forms.ComboBox();
            this.stateAccount = new System.Windows.Forms.CheckBox();
            this.stateStealth = new System.Windows.Forms.CheckBox();
            this.hidePostage = new System.Windows.Forms.CheckBox();
            this.labelStealth = new System.Windows.Forms.Label();
            this.groupTo = new System.Windows.Forms.GroupBox();
            this.requireFullAddressValidation = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge31 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelValidation = new System.Windows.Forms.Label();
            this.validationState = new System.Windows.Forms.CheckBox();
            this.labelMemo = new System.Windows.Forms.Label();
            this.memo = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.stateMemo = new System.Windows.Forms.CheckBox();
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge11 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupBoxFrom.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.groupReturns.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.groupExpressMail.SuspendLayout();
            this.groupTo.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Controls.Add(this.labelAccount);
            this.groupBoxFrom.Controls.Add(this.stampsAccount);
            this.groupBoxFrom.Controls.Add(this.stateAccount);
            this.groupBoxFrom.Size = new System.Drawing.Size(417, 82);
            this.groupBoxFrom.Controls.SetChildIndex(this.stateAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.senderState, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.stampsAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelAccount, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.originCombo, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelSender, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.kryptonBorderEdge1, 0);
            // 
            // groupShipment
            // 
            this.groupShipment.Controls.Add(this.stateMemo);
            this.groupShipment.Controls.Add(this.memo);
            this.groupShipment.Controls.Add(this.labelMemo);
            this.groupShipment.Controls.Add(this.hidePostage);
            this.groupShipment.Controls.Add(this.labelStealth);
            this.groupShipment.Controls.Add(this.stateStealth);
            this.groupShipment.Location = new System.Drawing.Point(8, 152);
            this.groupShipment.Size = new System.Drawing.Size(417, 270);
            this.groupShipment.Controls.SetChildIndex(this.kryptonBorderEdge, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateStealth, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelStealth, 0);
            this.groupShipment.Controls.SetChildIndex(this.hidePostage, 0);
            this.groupShipment.Controls.SetChildIndex(this.labelMemo, 0);
            this.groupShipment.Controls.SetChildIndex(this.memo, 0);
            this.groupShipment.Controls.SetChildIndex(this.stateMemo, 0);
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
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 54);
            // 
            // groupBoxCustoms
            // 
            this.groupBoxCustoms.Location = new System.Drawing.Point(8, 579);
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 238);
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
            this.groupReturns.Location = new System.Drawing.Point(8, 700);
            // 
            // groupLabels
            // 
            this.groupLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabels.Controls.Add(this.requestedLabelFormat);
            this.groupLabels.Controls.Add(this.requestedLabelFormatState);
            this.groupLabels.Controls.Add(this.kryptonBorderEdge11);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Location = new System.Drawing.Point(8, 428);
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
            this.requestedLabelFormat.TabIndex = 101;
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
            // groupInsurance
            // 
            this.groupInsurance.Location = new System.Drawing.Point(8, 491);
            // 
            // groupExpressMail
            // 
            this.groupExpressMail.Location = new System.Drawing.Point(8, 640);
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
            this.stampsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stampsAccount.FormattingEnabled = true;
            this.stampsAccount.Location = new System.Drawing.Point(110, 17);
            this.stampsAccount.Name = "stampsAccount";
            this.stampsAccount.Size = new System.Drawing.Size(144, 21);
            this.stampsAccount.TabIndex = 1;
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
            this.stateStealth.TabIndex = 67;
            this.stateStealth.UseVisualStyleBackColor = true;
            // 
            // hidePostage
            // 
            this.hidePostage.AutoSize = true;
            this.hidePostage.BackColor = System.Drawing.Color.Transparent;
            this.hidePostage.Location = new System.Drawing.Point(110, 213);
            this.hidePostage.Name = "hidePostage";
            this.hidePostage.Size = new System.Drawing.Size(89, 17);
            this.hidePostage.TabIndex = 69;
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
            this.groupTo.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTo.Controls.Add(this.requireFullAddressValidation);
            this.groupTo.Controls.Add(this.kryptonBorderEdge31);
            this.groupTo.Controls.Add(this.labelValidation);
            this.groupTo.Controls.Add(this.validationState);
            this.groupTo.Location = new System.Drawing.Point(8, 94);
            this.groupTo.Name = "groupTo";
            this.groupTo.Size = new System.Drawing.Size(417, 52);
            this.groupTo.TabIndex = 4;
            this.groupTo.TabStop = false;
            this.groupTo.Text = "To";
            // 
            // requireFullAddressValidation
            // 
            this.requireFullAddressValidation.AutoSize = true;
            this.requireFullAddressValidation.Location = new System.Drawing.Point(110, 23);
            this.requireFullAddressValidation.Name = "requireFullAddressValidation";
            this.requireFullAddressValidation.Size = new System.Drawing.Size(228, 17);
            this.requireFullAddressValidation.TabIndex = 17;
            this.requireFullAddressValidation.Text = "Require full Stamps.com address validation";
            this.requireFullAddressValidation.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge31
            // 
            this.kryptonBorderEdge31.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge31.AutoSize = false;
            this.kryptonBorderEdge31.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge31.Location = new System.Drawing.Point(30, 14);
            this.kryptonBorderEdge31.Name = "kryptonBorderEdge31";
            this.kryptonBorderEdge31.Size = new System.Drawing.Size(1, 29);
            this.kryptonBorderEdge31.TabIndex = 16;
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
            // labelMemo
            // 
            this.labelMemo.Location = new System.Drawing.Point(61, 236);
            this.labelMemo.Name = "labelMemo";
            this.labelMemo.Size = new System.Drawing.Size(40, 21);
            this.labelMemo.TabIndex = 70;
            this.labelMemo.Text = "Memo:";
            this.labelMemo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // memo
            // 
            this.memo.Location = new System.Drawing.Point(107, 236);
            this.memo.MaxLength = 32767;
            this.memo.Name = "memo";
            this.memo.Size = new System.Drawing.Size(210, 21);
            this.memo.TabIndex = 71;
            // 
            // stateMemo
            // 
            this.stateMemo.AutoSize = true;
            this.stateMemo.Checked = true;
            this.stateMemo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stateMemo.Location = new System.Drawing.Point(9, 240);
            this.stateMemo.Name = "stateMemo";
            this.stateMemo.Size = new System.Drawing.Size(15, 14);
            this.stateMemo.TabIndex = 72;
            this.stateMemo.UseVisualStyleBackColor = true;
            // 
            // StampsProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "StampsProfileControl";
            this.Size = new System.Drawing.Size(439, 783);
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.tabPage.ResumeLayout(false);
            this.groupReturns.ResumeLayout(false);
            this.groupReturns.PerformLayout();
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.groupExpressMail.ResumeLayout(false);
            this.groupExpressMail.PerformLayout();
            this.groupTo.ResumeLayout(false);
            this.groupTo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox stampsAccount;
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
        private System.Windows.Forms.Label labelMemo;
        private Templates.Tokens.TemplateTokenTextBox memo;
        private System.Windows.Forms.CheckBox stateMemo;

        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge11;
        protected System.Windows.Forms.GroupBox groupLabels;
        protected ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
    }
}
