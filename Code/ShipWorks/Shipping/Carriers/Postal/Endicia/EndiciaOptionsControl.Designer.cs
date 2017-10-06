namespace ShipWorks.Shipping.Carriers.Postal.Endicia
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
            this.labelLabels = new System.Windows.Forms.Label();
            this.labelCustoms = new System.Windows.Forms.Label();
            this.customsCertify = new System.Windows.Forms.CheckBox();
            this.labelSigner = new System.Windows.Forms.Label();
            this.customsSigner = new System.Windows.Forms.TextBox();
            this.labelSignerInfo = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl();
            this.shippingCutoff = new ShipWorks.Shipping.Editing.ShippingDateCutoffControl();
            this.customsPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.customsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLabels
            // 
            this.labelLabels.AutoSize = true;
            this.labelLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabels.Location = new System.Drawing.Point(8, 6);
            this.labelLabels.Name = "labelLabels";
            this.labelLabels.Size = new System.Drawing.Size(43, 13);
            this.labelLabels.TabIndex = 0;
            this.labelLabels.Text = "Labels";
            // 
            // labelCustoms
            // 
            this.labelCustoms.AutoSize = true;
            this.labelCustoms.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustoms.Location = new System.Drawing.Point(8, 2);
            this.labelCustoms.Name = "labelCustoms";
            this.labelCustoms.Size = new System.Drawing.Size(56, 13);
            this.labelCustoms.TabIndex = 5;
            this.labelCustoms.Text = "Customs";
            // 
            // customsCertify
            // 
            this.customsCertify.AutoSize = true;
            this.customsCertify.Location = new System.Drawing.Point(27, 22);
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
            this.labelSigner.Location = new System.Drawing.Point(43, 44);
            this.labelSigner.Name = "labelSigner";
            this.labelSigner.Size = new System.Drawing.Size(142, 13);
            this.labelSigner.TabIndex = 7;
            this.labelSigner.Text = "Electronic signature (name):";
            // 
            // customsSigner
            // 
            this.customsSigner.Enabled = false;
            this.customsSigner.Location = new System.Drawing.Point(189, 41);
            this.fieldLengthProvider.SetMaxLengthSource(this.customsSigner, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaCustomsSigner);
            this.customsSigner.Name = "customsSigner";
            this.customsSigner.Size = new System.Drawing.Size(229, 21);
            this.customsSigner.TabIndex = 8;
            // 
            // labelSignerInfo
            // 
            this.labelSignerInfo.AutoSize = true;
            this.labelSignerInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelSignerInfo.Location = new System.Drawing.Point(186, 65);
            this.labelSignerInfo.Name = "labelSignerInfo";
            this.labelSignerInfo.Size = new System.Drawing.Size(245, 13);
            this.labelSignerInfo.TabIndex = 9;
            this.labelSignerInfo.Text = "(MUST be a person\'s name, not a company name)";
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(24, 27);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(384, 25);
            this.requestedLabelFormat.TabIndex = 60;
            // 
            // shippingCutoff
            // 
            this.shippingCutoff.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.shippingCutoff.Location = new System.Drawing.Point(24, 58);
            this.shippingCutoff.Name = "shippingCutoff";
            this.shippingCutoff.Size = new System.Drawing.Size(467, 22);
            this.shippingCutoff.TabIndex = 61;
            // 
            // customsPanel
            // 
            this.customsPanel.Controls.Add(this.labelSignerInfo);
            this.customsPanel.Controls.Add(this.customsSigner);
            this.customsPanel.Controls.Add(this.labelSigner);
            this.customsPanel.Controls.Add(this.customsCertify);
            this.customsPanel.Controls.Add(this.labelCustoms);
            this.customsPanel.Location = new System.Drawing.Point(0, 90);
            this.customsPanel.Name = "customsPanel";
            this.customsPanel.Size = new System.Drawing.Size(520, 92);
            this.customsPanel.TabIndex = 62;
            // 
            // EndiciaOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customsPanel);
            this.Controls.Add(this.shippingCutoff);
            this.Controls.Add(this.requestedLabelFormat);
            this.Controls.Add(this.labelLabels);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EndiciaOptionsControl";
            this.Size = new System.Drawing.Size(522, 182);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.customsPanel.ResumeLayout(false);
            this.customsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLabels;
        private System.Windows.Forms.Label labelCustoms;
        private System.Windows.Forms.CheckBox customsCertify;
        private System.Windows.Forms.Label labelSigner;
        private System.Windows.Forms.TextBox customsSigner;
        private System.Windows.Forms.Label labelSignerInfo;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private Editing.RequestedLabelFormatOptionControl requestedLabelFormat;
        private Editing.ShippingDateCutoffControl shippingCutoff;
        private System.Windows.Forms.Panel customsPanel;
    }
}
