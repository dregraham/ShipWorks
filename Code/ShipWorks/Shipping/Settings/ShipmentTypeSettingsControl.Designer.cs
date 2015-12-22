namespace ShipWorks.Shipping.Settings
{
    partial class ShipmentTypeSettingsControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.tabPageProfiles = new System.Windows.Forms.TabPage();
            this.tabPagePrinting = new System.Windows.Forms.TabPage();
            this.tabPageActions = new System.Windows.Forms.TabPage();
            this.defaultsControl = new ShipWorks.Shipping.Settings.Defaults.ShippingDefaultsControl();
            this.printOutputControl = new ShipWorks.Shipping.Settings.Printing.PrintOutputControl();
            this.automationControl = new ShipWorks.Shipping.Settings.ShipmentAutomationControl();
            this.tabControl.SuspendLayout();
            this.tabPageProfiles.SuspendLayout();
            this.tabPagePrinting.SuspendLayout();
            this.tabPageActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Controls.Add(this.tabPageProfiles);
            this.tabControl.Controls.Add(this.tabPagePrinting);
            this.tabControl.Controls.Add(this.tabPageActions);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(556, 480);
            this.tabControl.TabIndex = 0;
            this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.OnTabPageDeselecting);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.AutoScroll = true;
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(548, 454);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // tabPageProfiles
            // 
            this.tabPageProfiles.Controls.Add(this.defaultsControl);
            this.tabPageProfiles.Location = new System.Drawing.Point(4, 22);
            this.tabPageProfiles.Name = "tabPageProfiles";
            this.tabPageProfiles.Padding = new System.Windows.Forms.Padding(9, 9, 6, 6);
            this.tabPageProfiles.Size = new System.Drawing.Size(548, 454);
            this.tabPageProfiles.TabIndex = 1;
            this.tabPageProfiles.Text = "Shipments";
            this.tabPageProfiles.UseVisualStyleBackColor = true;
            // 
            // tabPagePrinting
            // 
            this.tabPagePrinting.Controls.Add(this.printOutputControl);
            this.tabPagePrinting.Location = new System.Drawing.Point(4, 22);
            this.tabPagePrinting.Name = "tabPagePrinting";
            this.tabPagePrinting.Padding = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.tabPagePrinting.Size = new System.Drawing.Size(548, 454);
            this.tabPagePrinting.TabIndex = 2;
            this.tabPagePrinting.Text = "Printing";
            this.tabPagePrinting.UseVisualStyleBackColor = true;
            // 
            // tabPageActions
            // 
            this.tabPageActions.Controls.Add(this.automationControl);
            this.tabPageActions.Location = new System.Drawing.Point(4, 22);
            this.tabPageActions.Name = "tabPageActions";
            this.tabPageActions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActions.Size = new System.Drawing.Size(548, 454);
            this.tabPageActions.TabIndex = 3;
            this.tabPageActions.Text = "Tasks";
            this.tabPageActions.UseVisualStyleBackColor = true;
            // 
            // defaultsControl
            // 
            this.defaultsControl.AutoScroll = true;
            this.defaultsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defaultsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.defaultsControl.Location = new System.Drawing.Point(9, 9);
            this.defaultsControl.Name = "defaultsControl";
            this.defaultsControl.Size = new System.Drawing.Size(533, 439);
            this.defaultsControl.TabIndex = 0;
            this.defaultsControl.ProfileEdited += new System.EventHandler(this.OnProfileEdited);
            // 
            // printOutputControl
            // 
            this.printOutputControl.AutoScroll = true;
            this.printOutputControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printOutputControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.printOutputControl.Location = new System.Drawing.Point(2, 3);
            this.printOutputControl.Name = "printOutputControl";
            this.printOutputControl.Size = new System.Drawing.Size(543, 448);
            this.printOutputControl.TabIndex = 0;
            // 
            // automationControl
            // 
            this.automationControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.automationControl.Location = new System.Drawing.Point(7, 9);
            this.automationControl.Name = "automationControl";
            this.automationControl.Size = new System.Drawing.Size(345, 317);
            this.automationControl.TabIndex = 0;
            // 
            // ShipmentTypeSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShipmentTypeSettingsControl";
            this.Size = new System.Drawing.Size(556, 480);
            this.tabControl.ResumeLayout(false);
            this.tabPageProfiles.ResumeLayout(false);
            this.tabPagePrinting.ResumeLayout(false);
            this.tabPageActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TabPage tabPageProfiles;
        private System.Windows.Forms.TabPage tabPagePrinting;
        private ShipWorks.Shipping.Settings.Defaults.ShippingDefaultsControl defaultsControl;
        private ShipWorks.Shipping.Settings.Printing.PrintOutputControl printOutputControl;
        private System.Windows.Forms.TabPage tabPageActions;
        private ShipWorks.Shipping.Settings.ShipmentAutomationControl automationControl;
    }
}
