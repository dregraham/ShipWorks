namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    partial class AmazonProfileControl
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
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupInsurance = new System.Windows.Forms.GroupBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
            this.insuranceState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge10 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.deliveryExperience = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelDeliveryExperience = new System.Windows.Forms.Label();
            this.deliveryExperienceState = new System.Windows.Forms.CheckBox();
            this.dimensionsState = new System.Windows.Forms.CheckBox();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.weightState = new System.Windows.Forms.CheckBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelSender = new System.Windows.Forms.Label();
            this.originCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.originState = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupShipment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.groupBoxFrom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(425, 499);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.AutoScroll = true;
            this.tabPageSettings.Controls.Add(this.groupBoxFrom);
            this.tabPageSettings.Controls.Add(this.groupInsurance);
            this.tabPageSettings.Controls.Add(this.groupShipment);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(417, 473);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupInsurance
            // 
            this.groupInsurance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupInsurance.Controls.Add(this.insuranceControl);
            this.groupInsurance.Controls.Add(this.insuranceState);
            this.groupInsurance.Controls.Add(this.kryptonBorderEdge10);
            this.groupInsurance.Location = new System.Drawing.Point(6, 236);
            this.groupInsurance.Name = "groupInsurance";
            this.groupInsurance.Size = new System.Drawing.Size(405, 76);
            this.groupInsurance.TabIndex = 11;
            this.groupInsurance.TabStop = false;
            this.groupInsurance.Text = "Insurance";
            // 
            // insuranceControl
            // 
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(35, 21);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(316, 52);
            this.insuranceControl.TabIndex = 97;
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
            // kryptonBorderEdge10
            // 
            this.kryptonBorderEdge10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge10.AutoSize = false;
            this.kryptonBorderEdge10.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge10.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge10.Name = "kryptonBorderEdge10";
            this.kryptonBorderEdge10.Size = new System.Drawing.Size(1, 46);
            this.kryptonBorderEdge10.Text = "kryptonBorderEdge1";
            // 
            // groupShipment
            // 
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.deliveryExperience);
            this.groupShipment.Controls.Add(this.labelDeliveryExperience);
            this.groupShipment.Controls.Add(this.deliveryExperienceState);
            this.groupShipment.Controls.Add(this.dimensionsState);
            this.groupShipment.Controls.Add(this.dimensionsControl);
            this.groupShipment.Controls.Add(this.labelDimensions);
            this.groupShipment.Controls.Add(this.weightState);
            this.groupShipment.Controls.Add(this.weight);
            this.groupShipment.Controls.Add(this.labelWeight);
            this.groupShipment.Controls.Add(this.kryptonBorderEdge);
            this.groupShipment.Location = new System.Drawing.Point(6, 69);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(405, 161);
            this.groupShipment.TabIndex = 5;
            this.groupShipment.TabStop = false;
            this.groupShipment.Text = "Shipment";
            // 
            // deliveryExperience
            // 
            this.deliveryExperience.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deliveryExperience.FormattingEnabled = true;
            this.deliveryExperience.Location = new System.Drawing.Point(112, 128);
            this.deliveryExperience.Name = "deliveryExperience";
            this.deliveryExperience.PromptText = "(Multiple Values)";
            this.deliveryExperience.Size = new System.Drawing.Size(220, 21);
            this.deliveryExperience.TabIndex = 5;
            // 
            // labelDeliveryExperience
            // 
            this.labelDeliveryExperience.AutoSize = true;
            this.labelDeliveryExperience.BackColor = System.Drawing.Color.Transparent;
            this.labelDeliveryExperience.Location = new System.Drawing.Point(34, 131);
            this.labelDeliveryExperience.Name = "labelDeliveryExperience";
            this.labelDeliveryExperience.Size = new System.Drawing.Size(72, 13);
            this.labelDeliveryExperience.TabIndex = 76;
            this.labelDeliveryExperience.Text = "Confirmation:";
            // 
            // deliveryExperienceState
            // 
            this.deliveryExperienceState.AutoSize = true;
            this.deliveryExperienceState.Checked = true;
            this.deliveryExperienceState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deliveryExperienceState.Location = new System.Drawing.Point(9, 131);
            this.deliveryExperienceState.Name = "deliveryExperienceState";
            this.deliveryExperienceState.Size = new System.Drawing.Size(15, 14);
            this.deliveryExperienceState.TabIndex = 74;
            this.deliveryExperienceState.Tag = "";
            this.deliveryExperienceState.UseVisualStyleBackColor = true;
            // 
            // dimensionsState
            // 
            this.dimensionsState.AutoSize = true;
            this.dimensionsState.Checked = true;
            this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsState.Location = new System.Drawing.Point(9, 52);
            this.dimensionsState.Name = "dimensionsState";
            this.dimensionsState.Size = new System.Drawing.Size(15, 14);
            this.dimensionsState.TabIndex = 73;
            this.dimensionsState.Tag = "";
            this.dimensionsState.UseVisualStyleBackColor = true;
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(109, 48);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 2;
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(42, 54);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 71;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // weightState
            // 
            this.weightState.AutoSize = true;
            this.weightState.Checked = true;
            this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.weightState.Location = new System.Drawing.Point(9, 23);
            this.weightState.Name = "weightState";
            this.weightState.Size = new System.Drawing.Size(15, 14);
            this.weightState.TabIndex = 70;
            this.weightState.Tag = "";
            this.weightState.UseVisualStyleBackColor = true;
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(112, 21);
            this.weight.Name = "weight";
            this.weight.RangeMax = 400D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 23);
            this.weight.TabIndex = 1;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(61, 25);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 67;
            this.labelWeight.Text = "Weight:";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 133);
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFrom.Controls.Add(this.kryptonBorderEdge1);
            this.groupBoxFrom.Controls.Add(this.labelSender);
            this.groupBoxFrom.Controls.Add(this.originCombo);
            this.groupBoxFrom.Controls.Add(this.originState);
            this.groupBoxFrom.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(405, 57);
            this.groupBoxFrom.TabIndex = 12;
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
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 29);
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // labelSender
            // 
            this.labelSender.AutoSize = true;
            this.labelSender.Location = new System.Drawing.Point(67, 23);
            this.labelSender.Name = "labelSender";
            this.labelSender.Size = new System.Drawing.Size(39, 13);
            this.labelSender.TabIndex = 12;
            this.labelSender.Text = "Origin:";
            // 
            // originCombo
            // 
            this.originCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.originCombo.FormattingEnabled = true;
            this.originCombo.Location = new System.Drawing.Point(112, 23);
            this.originCombo.Name = "originCombo";
            this.originCombo.PromptText = "(Multiple Values)";
            this.originCombo.Size = new System.Drawing.Size(206, 21);
            this.originCombo.TabIndex = 3;
            // 
            // originState
            // 
            this.originState.AutoSize = true;
            this.originState.Checked = true;
            this.originState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.originState.Location = new System.Drawing.Point(9, 26);
            this.originState.Name = "originState";
            this.originState.Size = new System.Drawing.Size(15, 14);
            this.originState.TabIndex = 2;
            this.originState.Tag = "";
            this.originState.UseVisualStyleBackColor = true;
            // 
            // AmazonProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "AmazonProfileControl";
            this.Size = new System.Drawing.Size(425, 499);
            this.tabControl.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupShipment;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.GroupBox groupInsurance;
        private Insurance.InsuranceProfileControl insuranceControl;
        private System.Windows.Forms.CheckBox insuranceState;
        private System.Windows.Forms.CheckBox weightState;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.CheckBox dimensionsState;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label labelDimensions;
        private System.Windows.Forms.Label labelDeliveryExperience;
        private System.Windows.Forms.CheckBox deliveryExperienceState;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge10;
        private ShipWorks.UI.Controls.MultiValueComboBox deliveryExperience;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private System.Windows.Forms.Label labelSender;
        private ShipWorks.UI.Controls.MultiValueComboBox originCombo;
        private System.Windows.Forms.CheckBox originState;
    }
}
