namespace ShipWorks.Shipping.Carriers.Other
{
    partial class OtherProfileControl
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
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.panelShipmentOptions = new System.Windows.Forms.Panel();
            this.labelCarrier = new System.Windows.Forms.Label();
            this.carrier = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.service = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelService = new System.Windows.Forms.Label();
            this.panelShipmentState = new System.Windows.Forms.Panel();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.carrierState = new System.Windows.Forms.CheckBox();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.panelFromOptions = new System.Windows.Forms.Panel();
            this.labelSender = new System.Windows.Forms.Label();
            this.originCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.panelFromState = new System.Windows.Forms.Panel();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.senderState = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage = new System.Windows.Forms.TabPage();
            this.groupReturns = new System.Windows.Forms.GroupBox();
            this.returnShipment = new System.Windows.Forms.CheckBox();
            this.returnState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupInsurance = new System.Windows.Forms.GroupBox();
            this.insuranceState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
            this.groupShipment.SuspendLayout();
            this.panelShipmentOptions.SuspendLayout();
            this.panelShipmentState.SuspendLayout();
            this.groupBoxFrom.SuspendLayout();
            this.panelFromOptions.SuspendLayout();
            this.panelFromState.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.groupReturns.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupShipment
            // 
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.panelShipmentOptions);
            this.groupShipment.Controls.Add(this.panelShipmentState);
            this.groupShipment.Location = new System.Drawing.Point(8, 63);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(417, 74);
            this.groupShipment.TabIndex = 1;
            this.groupShipment.TabStop = false;
            this.groupShipment.Text = "Shipment";
            // 
            // panelShipmentOptions
            // 
            this.panelShipmentOptions.Controls.Add(this.labelCarrier);
            this.panelShipmentOptions.Controls.Add(this.carrier);
            this.panelShipmentOptions.Controls.Add(this.service);
            this.panelShipmentOptions.Controls.Add(this.labelService);
            this.panelShipmentOptions.Location = new System.Drawing.Point(30, 13);
            this.panelShipmentOptions.Name = "panelShipmentOptions";
            this.panelShipmentOptions.Size = new System.Drawing.Size(286, 56);
            this.panelShipmentOptions.TabIndex = 10;
            // 
            // labelCarrier
            // 
            this.labelCarrier.AutoSize = true;
            this.labelCarrier.BackColor = System.Drawing.Color.Transparent;
            this.labelCarrier.Location = new System.Drawing.Point(3, 7);
            this.labelCarrier.Name = "labelCarrier";
            this.labelCarrier.Size = new System.Drawing.Size(73, 13);
            this.labelCarrier.TabIndex = 4;
            this.labelCarrier.Text = "Carrier name:";
            // 
            // carrier
            // 
            this.carrier.Location = new System.Drawing.Point(80, 3);
            this.fieldLengthProvider.SetMaxLengthSource(this.carrier, ShipWorks.Data.Utility.EntityFieldLengthSource.ShipmentOtherCarrier);
            this.carrier.Name = "carrier";
            this.carrier.Size = new System.Drawing.Size(198, 21);
            this.carrier.TabIndex = 5;
            // 
            // service
            // 
            this.service.Location = new System.Drawing.Point(80, 29);
            this.fieldLengthProvider.SetMaxLengthSource(this.service, ShipWorks.Data.Utility.EntityFieldLengthSource.ShipmentOtherService);
            this.service.Name = "service";
            this.service.Size = new System.Drawing.Size(198, 21);
            this.service.TabIndex = 7;
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(28, 32);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 6;
            this.labelService.Text = "Service:";
            // 
            // panelShipmentState
            // 
            this.panelShipmentState.Controls.Add(this.kryptonBorderEdge);
            this.panelShipmentState.Controls.Add(this.serviceState);
            this.panelShipmentState.Controls.Add(this.carrierState);
            this.panelShipmentState.Location = new System.Drawing.Point(6, 18);
            this.panelShipmentState.Name = "panelShipmentState";
            this.panelShipmentState.Size = new System.Drawing.Size(23, 47);
            this.panelShipmentState.TabIndex = 9;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Right;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(22, 0);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 47);
            this.kryptonBorderEdge.TabIndex = 4;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // serviceState
            // 
            this.serviceState.AutoSize = true;
            this.serviceState.Checked = true;
            this.serviceState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.serviceState.Location = new System.Drawing.Point(3, 28);
            this.serviceState.Name = "serviceState";
            this.serviceState.Size = new System.Drawing.Size(15, 14);
            this.serviceState.TabIndex = 3;
            this.serviceState.UseVisualStyleBackColor = true;
            // 
            // carrierState
            // 
            this.carrierState.AutoSize = true;
            this.carrierState.Checked = true;
            this.carrierState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.carrierState.Location = new System.Drawing.Point(3, 2);
            this.carrierState.Name = "carrierState";
            this.carrierState.Size = new System.Drawing.Size(15, 14);
            this.carrierState.TabIndex = 2;
            this.carrierState.UseVisualStyleBackColor = true;
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFrom.Controls.Add(this.panelFromOptions);
            this.groupBoxFrom.Controls.Add(this.panelFromState);
            this.groupBoxFrom.Location = new System.Drawing.Point(8, 6);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(417, 52);
            this.groupBoxFrom.TabIndex = 0;
            this.groupBoxFrom.TabStop = false;
            this.groupBoxFrom.Text = "From";
            // 
            // panelFromOptions
            // 
            this.panelFromOptions.Controls.Add(this.labelSender);
            this.panelFromOptions.Controls.Add(this.originCombo);
            this.panelFromOptions.Location = new System.Drawing.Point(30, 13);
            this.panelFromOptions.Name = "panelFromOptions";
            this.panelFromOptions.Size = new System.Drawing.Size(397, 33);
            this.panelFromOptions.TabIndex = 1;
            // 
            // labelSender
            // 
            this.labelSender.AutoSize = true;
            this.labelSender.Location = new System.Drawing.Point(35, 10);
            this.labelSender.Name = "labelSender";
            this.labelSender.Size = new System.Drawing.Size(39, 13);
            this.labelSender.TabIndex = 10;
            this.labelSender.Text = "Origin:";
            // 
            // originCombo
            // 
            this.originCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.originCombo.FormattingEnabled = true;
            this.originCombo.Location = new System.Drawing.Point(80, 7);
            this.originCombo.Name = "originCombo";
            this.originCombo.PromptText = "(Multiple Values)";
            this.originCombo.Size = new System.Drawing.Size(206, 21);
            this.originCombo.TabIndex = 8;
            // 
            // panelFromState
            // 
            this.panelFromState.Controls.Add(this.kryptonBorderEdge1);
            this.panelFromState.Controls.Add(this.senderState);
            this.panelFromState.Location = new System.Drawing.Point(6, 18);
            this.panelFromState.Name = "panelFromState";
            this.panelFromState.Size = new System.Drawing.Size(23, 25);
            this.panelFromState.TabIndex = 0;
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge1.Dock = System.Windows.Forms.DockStyle.Right;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(22, 0);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 25);
            this.kryptonBorderEdge1.TabIndex = 4;
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // senderState
            // 
            this.senderState.AutoSize = true;
            this.senderState.Checked = true;
            this.senderState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.senderState.Location = new System.Drawing.Point(3, 4);
            this.senderState.Name = "senderState";
            this.senderState.Size = new System.Drawing.Size(15, 14);
            this.senderState.TabIndex = 2;
            this.senderState.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(439, 370);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage
            // 
            this.tabPage.Controls.Add(this.groupReturns);
            this.tabPage.Controls.Add(this.groupInsurance);
            this.tabPage.Controls.Add(this.groupBoxFrom);
            this.tabPage.Controls.Add(this.groupShipment);
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Name = "tabPage";
            this.tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage.Size = new System.Drawing.Size(431, 344);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "Settings";
            this.tabPage.UseVisualStyleBackColor = true;
            // 
            // groupReturns
            // 
            this.groupReturns.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupReturns.Controls.Add(this.returnShipment);
            this.groupReturns.Controls.Add(this.returnState);
            this.groupReturns.Controls.Add(this.kryptonBorderEdge2);
            this.groupReturns.Location = new System.Drawing.Point(8, 231);
            this.groupReturns.Name = "groupReturns";
            this.groupReturns.Size = new System.Drawing.Size(417, 53);
            this.groupReturns.TabIndex = 6;
            this.groupReturns.TabStop = false;
            this.groupReturns.Text = "Return Shipment";
            // 
            // returnShipment
            // 
            this.returnShipment.AutoSize = true;
            this.returnShipment.Location = new System.Drawing.Point(47, 22);
            this.returnShipment.Name = "returnShipment";
            this.returnShipment.Size = new System.Drawing.Size(140, 17);
            this.returnShipment.TabIndex = 97;
            this.returnShipment.Text = "This is a return shipment";
            this.returnShipment.UseVisualStyleBackColor = true;
            // 
            // returnState
            // 
            this.returnState.AutoSize = true;
            this.returnState.Location = new System.Drawing.Point(9, 23);
            this.returnState.Name = "returnState";
            this.returnState.Size = new System.Drawing.Size(15, 14);
            this.returnState.TabIndex = 0;
            this.returnState.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge2
            // 
            this.kryptonBorderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge2.AutoSize = false;
            this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 23);
            this.kryptonBorderEdge2.TabIndex = 96;
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge1";
            // 
            // groupInsurance
            // 
            this.groupInsurance.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupInsurance.Controls.Add(this.insuranceControl);
            this.groupInsurance.Controls.Add(this.insuranceState);
            this.groupInsurance.Controls.Add(this.kryptonBorderEdge3);
            this.groupInsurance.Location = new System.Drawing.Point(8, 143);
            this.groupInsurance.Name = "groupInsurance";
            this.groupInsurance.Size = new System.Drawing.Size(417, 82);
            this.groupInsurance.TabIndex = 5;
            this.groupInsurance.TabStop = false;
            this.groupInsurance.Text = "Insurance";
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
            // kryptonBorderEdge3
            // 
            this.kryptonBorderEdge3.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge3.AutoSize = false;
            this.kryptonBorderEdge3.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge3.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge3.Name = "kryptonBorderEdge3";
            this.kryptonBorderEdge3.Size = new System.Drawing.Size(1, 52);
            this.kryptonBorderEdge3.TabIndex = 96;
            this.kryptonBorderEdge3.Text = "kryptonBorderEdge1";
            // 
            // insuranceControl
            // 
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(43, 21);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(333, 52);
            this.insuranceControl.TabIndex = 97;
            // 
            // OtherProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "OtherProfileControl";
            this.Size = new System.Drawing.Size(439, 370);
            this.groupShipment.ResumeLayout(false);
            this.panelShipmentOptions.ResumeLayout(false);
            this.panelShipmentOptions.PerformLayout();
            this.panelShipmentState.ResumeLayout(false);
            this.panelShipmentState.PerformLayout();
            this.groupBoxFrom.ResumeLayout(false);
            this.panelFromOptions.ResumeLayout(false);
            this.panelFromOptions.PerformLayout();
            this.panelFromState.ResumeLayout(false);
            this.panelFromState.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage.ResumeLayout(false);
            this.groupReturns.ResumeLayout(false);
            this.groupReturns.PerformLayout();
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupShipment;
        private System.Windows.Forms.Label labelCarrier;
        private ShipWorks.UI.Controls.MultiValueTextBox service;
        private ShipWorks.UI.Controls.MultiValueTextBox carrier;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private System.Windows.Forms.Panel panelShipmentOptions;
        private System.Windows.Forms.Panel panelShipmentState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.CheckBox serviceState;
        private System.Windows.Forms.CheckBox carrierState;
        private System.Windows.Forms.Panel panelFromOptions;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private System.Windows.Forms.CheckBox senderState;
        private System.Windows.Forms.Label labelSender;
        private ShipWorks.UI.Controls.MultiValueComboBox originCombo;
        private System.Windows.Forms.Panel panelFromState;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.GroupBox groupReturns;
        private System.Windows.Forms.CheckBox returnState;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private System.Windows.Forms.GroupBox groupInsurance;
        private System.Windows.Forms.CheckBox insuranceState;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        private System.Windows.Forms.CheckBox returnShipment;
        private Insurance.InsuranceProfileControl insuranceControl;
    }
}
