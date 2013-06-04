﻿namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaOptionsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EndiciaOptionsControl));
            this.thermalType = new System.Windows.Forms.ComboBox();
            this.labelThermalType = new System.Windows.Forms.Label();
            this.labelLabels = new System.Windows.Forms.Label();
            this.thermalPrinter = new System.Windows.Forms.CheckBox();
            this.labelCustoms = new System.Windows.Forms.Label();
            this.customsCertify = new System.Windows.Forms.CheckBox();
            this.labelSigner = new System.Windows.Forms.Label();
            this.customsSigner = new System.Windows.Forms.TextBox();
            this.labelSignerInfo = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.infotipLabelType = new ShipWorks.UI.Controls.InfoTip();
            this.thermalDocTabType = new System.Windows.Forms.ComboBox();
            this.labelThermalDocTabType = new System.Windows.Forms.Label();
            this.thermalDocTab = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // thermalType
            // 
            this.thermalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalType.Enabled = false;
            this.thermalType.FormattingEnabled = true;
            this.thermalType.Location = new System.Drawing.Point(121, 48);
            this.thermalType.Name = "thermalType";
            this.thermalType.Size = new System.Drawing.Size(115, 21);
            this.thermalType.TabIndex = 3;
            // 
            // labelThermalType
            // 
            this.labelThermalType.AutoSize = true;
            this.labelThermalType.Enabled = false;
            this.labelThermalType.Location = new System.Drawing.Point(45, 51);
            this.labelThermalType.Name = "labelThermalType";
            this.labelThermalType.Size = new System.Drawing.Size(74, 13);
            this.labelThermalType.TabIndex = 2;
            this.labelThermalType.Text = "Thermal type:";
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelLabels.Location = new System.Drawing.Point(8, 6);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 0;
            this.labelLabels.Text = "Labels";
            // 
            // thermalPrinter
            // 
            this.thermalPrinter.AutoSize = true;
            this.thermalPrinter.Location = new System.Drawing.Point(27, 28);
            this.thermalPrinter.Name = "thermalPrinter";
            this.thermalPrinter.Size = new System.Drawing.Size(253, 17);
            this.thermalPrinter.TabIndex = 1;
            this.thermalPrinter.Text = "The labels will be printed with a thermal printer.";
            this.thermalPrinter.UseVisualStyleBackColor = true;
            this.thermalPrinter.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
            // 
            // labelCustoms
            // 
            this.labelCustoms.AutoSize = true;
            this.labelCustoms.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCustoms.Location = new System.Drawing.Point(8, 79);
            this.labelCustoms.Name = "labelCustoms";
            this.labelCustoms.Size = new System.Drawing.Size(56, 13);
            this.labelCustoms.TabIndex = 5;
            this.labelCustoms.Text = "Customs";
            // 
            // customsCertify
            // 
            this.customsCertify.AutoSize = true;
            this.customsCertify.Location = new System.Drawing.Point(27, 99);
            this.customsCertify.Name = "customsCertify";
            this.customsCertify.Size = new System.Drawing.Size(267, 17);
            this.customsCertify.TabIndex = 6;
            this.customsCertify.Text = "I certify that my customs declarations are correct:";
            this.customsCertify.UseVisualStyleBackColor = true;
            this.customsCertify.CheckedChanged += new System.EventHandler(this.OnChangeCustomsCertify);
            // 
            // labelSigner
            // 
            this.labelSigner.AutoSize = true;
            this.labelSigner.Location = new System.Drawing.Point(44, 121);
            this.labelSigner.Name = "labelSigner";
            this.labelSigner.Size = new System.Drawing.Size(142, 13);
            this.labelSigner.TabIndex = 7;
            this.labelSigner.Text = "Electronic signature (name):";
            // 
            // customsSigner
            // 
            this.customsSigner.Enabled = false;
            this.customsSigner.Location = new System.Drawing.Point(190, 118);
            this.fieldLengthProvider.SetMaxLengthSource(this.customsSigner, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaCustomsSigner);
            this.customsSigner.Name = "customsSigner";
            this.customsSigner.Size = new System.Drawing.Size(229, 21);
            this.customsSigner.TabIndex = 8;
            // 
            // labelSignerInfo
            // 
            this.labelSignerInfo.AutoSize = true;
            this.labelSignerInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelSignerInfo.Location = new System.Drawing.Point(187, 142);
            this.labelSignerInfo.Name = "labelSignerInfo";
            this.labelSignerInfo.Size = new System.Drawing.Size(245, 13);
            this.labelSignerInfo.TabIndex = 9;
            this.labelSignerInfo.Text = "(MUST be a person\'s name, not a company name)";
            // 
            // infotipLabelType
            // 
            this.infotipLabelType.Caption = resources.GetString("infotipLabelType.Caption");
            this.infotipLabelType.Location = new System.Drawing.Point(279, 30);
            this.infotipLabelType.Name = "infotipLabelType";
            this.infotipLabelType.Size = new System.Drawing.Size(12, 12);
            this.infotipLabelType.TabIndex = 46;
            this.infotipLabelType.Title = "Printer Type";
            // 
            // thermalDocTabType
            // 
            this.thermalDocTabType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thermalDocTabType.Enabled = false;
            this.thermalDocTabType.FormattingEnabled = true;
            this.thermalDocTabType.Location = new System.Drawing.Point(321, 42);
            this.thermalDocTabType.Name = "thermalDocTabType";
            this.thermalDocTabType.Size = new System.Drawing.Size(149, 21);
            this.thermalDocTabType.TabIndex = 49;
            this.thermalDocTabType.Visible = false;
            // 
            // labelThermalDocTabType
            // 
            this.labelThermalDocTabType.AutoSize = true;
            this.labelThermalDocTabType.Enabled = false;
            this.labelThermalDocTabType.Location = new System.Drawing.Point(318, 26);
            this.labelThermalDocTabType.Name = "labelThermalDocTabType";
            this.labelThermalDocTabType.Size = new System.Drawing.Size(192, 13);
            this.labelThermalDocTabType.TabIndex = 48;
            this.labelThermalDocTabType.Text = "The doc-tab emerges from the printer:";
            this.labelThermalDocTabType.Visible = false;
            // 
            // thermalDocTab
            // 
            this.thermalDocTab.AutoSize = true;
            this.thermalDocTab.Enabled = false;
            this.thermalDocTab.Location = new System.Drawing.Point(321, 6);
            this.thermalDocTab.Name = "thermalDocTab";
            this.thermalDocTab.Size = new System.Drawing.Size(166, 17);
            this.thermalDocTab.TabIndex = 47;
            this.thermalDocTab.Text = "My label stock has a doc-tab.";
            this.thermalDocTab.UseVisualStyleBackColor = true;
            this.thermalDocTab.Visible = false;
            this.thermalDocTab.CheckedChanged += new System.EventHandler(this.OnUpdateThermalUI);
            // 
            // EndiciaOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.thermalDocTabType);
            this.Controls.Add(this.labelThermalDocTabType);
            this.Controls.Add(this.thermalDocTab);
            this.Controls.Add(this.infotipLabelType);
            this.Controls.Add(this.labelSignerInfo);
            this.Controls.Add(this.customsSigner);
            this.Controls.Add(this.labelSigner);
            this.Controls.Add(this.customsCertify);
            this.Controls.Add(this.labelCustoms);
            this.Controls.Add(this.thermalType);
            this.Controls.Add(this.labelThermalType);
            this.Controls.Add(this.labelLabels);
            this.Controls.Add(this.thermalPrinter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EndiciaOptionsControl";
            this.Size = new System.Drawing.Size(435, 178);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox thermalType;
        private System.Windows.Forms.Label labelThermalType;
        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.CheckBox thermalPrinter;
        private System.Windows.Forms.Label labelCustoms;
        private System.Windows.Forms.CheckBox customsCertify;
        private System.Windows.Forms.Label labelSigner;
        private System.Windows.Forms.TextBox customsSigner;
        private System.Windows.Forms.Label labelSignerInfo;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infotipLabelType;
        private System.Windows.Forms.ComboBox thermalDocTabType;
        private System.Windows.Forms.Label labelThermalDocTabType;
        private System.Windows.Forms.CheckBox thermalDocTab;

    }
}
