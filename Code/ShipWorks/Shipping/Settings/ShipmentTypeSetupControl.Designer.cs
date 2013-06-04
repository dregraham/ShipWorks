namespace ShipWorks.Shipping.Settings
{
    partial class ShipmentTypeSetupControl
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
            this.setup = new System.Windows.Forms.Button();
            this.labelSetup = new System.Windows.Forms.Label();
            this.panelSetup = new System.Windows.Forms.Panel();
            this.panelUpgrade = new System.Windows.Forms.Panel();
            this.labelUpgrade = new System.Windows.Forms.Label();
            this.upgrade = new System.Windows.Forms.Button();
            this.panelSetup.SuspendLayout();
            this.panelUpgrade.SuspendLayout();
            this.SuspendLayout();
            // 
            // setup
            // 
            this.setup.Location = new System.Drawing.Point(23, 20);
            this.setup.Name = "setup";
            this.setup.Size = new System.Drawing.Size(75, 23);
            this.setup.TabIndex = 3;
            this.setup.Text = "Setup...";
            this.setup.UseVisualStyleBackColor = true;
            this.setup.Click += new System.EventHandler(this.OnSetup);
            // 
            // labelSetup
            // 
            this.labelSetup.AutoSize = true;
            this.labelSetup.Location = new System.Drawing.Point(3, 2);
            this.labelSetup.Name = "labelSetup";
            this.labelSetup.Size = new System.Drawing.Size(212, 13);
            this.labelSetup.TabIndex = 2;
            this.labelSetup.Text = "ShipWorks has not yet been setup for {0}.";
            // 
            // panelSetup
            // 
            this.panelSetup.Controls.Add(this.labelSetup);
            this.panelSetup.Controls.Add(this.setup);
            this.panelSetup.Location = new System.Drawing.Point(3, 6);
            this.panelSetup.Name = "panelSetup";
            this.panelSetup.Size = new System.Drawing.Size(380, 100);
            this.panelSetup.TabIndex = 4;
            // 
            // panelUpgrade
            // 
            this.panelUpgrade.Controls.Add(this.labelUpgrade);
            this.panelUpgrade.Controls.Add(this.upgrade);
            this.panelUpgrade.Location = new System.Drawing.Point(3, 112);
            this.panelUpgrade.Name = "panelUpgrade";
            this.panelUpgrade.Size = new System.Drawing.Size(380, 100);
            this.panelUpgrade.TabIndex = 5;
            // 
            // labelUpgrade
            // 
            this.labelUpgrade.AutoSize = true;
            this.labelUpgrade.Location = new System.Drawing.Point(3, 2);
            this.labelUpgrade.Name = "labelUpgrade";
            this.labelUpgrade.Size = new System.Drawing.Size(265, 13);
            this.labelUpgrade.TabIndex = 2;
            this.labelUpgrade.Text = "You muse upgrade your ShipWorks edition to use {0}.";
            // 
            // upgrade
            // 
            this.upgrade.Image = global::ShipWorks.Properties.Resources.lock16;
            this.upgrade.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.upgrade.Location = new System.Drawing.Point(23, 20);
            this.upgrade.Name = "upgrade";
            this.upgrade.Size = new System.Drawing.Size(83, 23);
            this.upgrade.TabIndex = 3;
            this.upgrade.Text = "Upgrade...";
            this.upgrade.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.upgrade.UseVisualStyleBackColor = true;
            this.upgrade.Click += new System.EventHandler(this.OnUpgrade);
            // 
            // ShipmentTypeSetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUpgrade);
            this.Controls.Add(this.panelSetup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShipmentTypeSetupControl";
            this.Size = new System.Drawing.Size(440, 228);
            this.panelSetup.ResumeLayout(false);
            this.panelSetup.PerformLayout();
            this.panelUpgrade.ResumeLayout(false);
            this.panelUpgrade.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button setup;
        private System.Windows.Forms.Label labelSetup;
        private System.Windows.Forms.Panel panelSetup;
        private System.Windows.Forms.Panel panelUpgrade;
        private System.Windows.Forms.Label labelUpgrade;
        private System.Windows.Forms.Button upgrade;
    }
}
