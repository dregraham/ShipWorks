namespace ShipWorks.Shipping.Settings.Defaults
{
    partial class ShippingDefaultsControl
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
            this.panelSettingsArea = new System.Windows.Forms.Panel();
            this.toolStripAddSettingsLine = new System.Windows.Forms.ToolStrip();
            this.addSettingsLine = new System.Windows.Forms.ToolStripButton();
            this.labelAdditionalSettings = new System.Windows.Forms.Label();
            this.labelCommonSettings = new System.Windows.Forms.Label();
            this.linkCommonProfile = new System.Windows.Forms.Label();
            this.labelPrimaryProfileInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripAddSettingsLine.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSettingsArea
            // 
            this.panelSettingsArea.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSettingsArea.Location = new System.Drawing.Point(14, 84);
            this.panelSettingsArea.Name = "panelSettingsArea";
            this.panelSettingsArea.Size = new System.Drawing.Size(614, 64);
            this.panelSettingsArea.TabIndex = 5;
            // 
            // toolStripAddSettingsLine
            // 
            this.toolStripAddSettingsLine.BackColor = System.Drawing.Color.Transparent;
            this.toolStripAddSettingsLine.CanOverflow = false;
            this.toolStripAddSettingsLine.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripAddSettingsLine.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripAddSettingsLine.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAddSettingsLine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSettingsLine});
            this.toolStripAddSettingsLine.Location = new System.Drawing.Point(15, 149);
            this.toolStripAddSettingsLine.Name = "toolStripAddSettingsLine";
            this.toolStripAddSettingsLine.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripAddSettingsLine.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripAddSettingsLine.Size = new System.Drawing.Size(77, 25);
            this.toolStripAddSettingsLine.Stretch = true;
            this.toolStripAddSettingsLine.TabIndex = 6;
            // 
            // addSettingsLine
            // 
            this.addSettingsLine.Image = global::ShipWorks.Properties.Resources.add16;
            this.addSettingsLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addSettingsLine.Name = "addSettingsLine";
            this.addSettingsLine.Size = new System.Drawing.Size(75, 22);
            this.addSettingsLine.Text = "Add Rule";
            this.addSettingsLine.Click += new System.EventHandler(this.OnAddDefaultsRule);
            // 
            // labelAdditionalSettings
            // 
            this.labelAdditionalSettings.AutoSize = true;
            this.labelAdditionalSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAdditionalSettings.Location = new System.Drawing.Point(-1, 46);
            this.labelAdditionalSettings.Name = "labelAdditionalSettings";
            this.labelAdditionalSettings.Size = new System.Drawing.Size(114, 13);
            this.labelAdditionalSettings.TabIndex = 3;
            this.labelAdditionalSettings.Text = "Additional Defaults";
            // 
            // labelCommonSettings
            // 
            this.labelCommonSettings.AutoSize = true;
            this.labelCommonSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCommonSettings.Location = new System.Drawing.Point(0, 0);
            this.labelCommonSettings.Name = "labelCommonSettings";
            this.labelCommonSettings.Size = new System.Drawing.Size(84, 13);
            this.labelCommonSettings.TabIndex = 0;
            this.labelCommonSettings.Text = "Base Defaults";
            // 
            // linkCommonProfile
            // 
            this.linkCommonProfile.AutoSize = true;
            this.linkCommonProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkCommonProfile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkCommonProfile.ForeColor = System.Drawing.Color.Blue;
            this.linkCommonProfile.Location = new System.Drawing.Point(255, 19);
            this.linkCommonProfile.Name = "linkCommonProfile";
            this.linkCommonProfile.Size = new System.Drawing.Size(138, 13);
            this.linkCommonProfile.TabIndex = 2;
            this.linkCommonProfile.Text = "USPS w/o Postage Defaults";
            this.linkCommonProfile.Click += new System.EventHandler(this.OnLinkDefaultProfile);
            // 
            // labelPrimaryProfileInfo
            // 
            this.labelPrimaryProfileInfo.AutoSize = true;
            this.labelPrimaryProfileInfo.Location = new System.Drawing.Point(16, 19);
            this.labelPrimaryProfileInfo.Name = "labelPrimaryProfileInfo";
            this.labelPrimaryProfileInfo.Size = new System.Drawing.Size(241, 13);
            this.labelPrimaryProfileInfo.TabIndex = 1;
            this.labelPrimaryProfileInfo.Text = "Apply these settings to every new {0} shipment:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(483, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "The settings of every profile where the order is in the selected filter will be a" +
                "pplied to the shipment.";
            // 
            // ShippingDefaultsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelSettingsArea);
            this.Controls.Add(this.toolStripAddSettingsLine);
            this.Controls.Add(this.labelAdditionalSettings);
            this.Controls.Add(this.labelCommonSettings);
            this.Controls.Add(this.linkCommonProfile);
            this.Controls.Add(this.labelPrimaryProfileInfo);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShippingDefaultsControl";
            this.Size = new System.Drawing.Size(631, 191);
            this.toolStripAddSettingsLine.ResumeLayout(false);
            this.toolStripAddSettingsLine.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelSettingsArea;
        private System.Windows.Forms.ToolStrip toolStripAddSettingsLine;
        private System.Windows.Forms.ToolStripButton addSettingsLine;
        private System.Windows.Forms.Label labelAdditionalSettings;
        private System.Windows.Forms.Label labelCommonSettings;
        private System.Windows.Forms.Label linkCommonProfile;
        private System.Windows.Forms.Label labelPrimaryProfileInfo;
        private System.Windows.Forms.Label label1;
    }
}
