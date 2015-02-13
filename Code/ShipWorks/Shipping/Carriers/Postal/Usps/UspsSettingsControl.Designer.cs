using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsSettingsControl
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
            this.labelAccountType = new System.Windows.Forms.Label();
            this.accountControl = new ShipWorks.Shipping.Carriers.Postal.Stamps.StampsAccountManagerControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.Postal.Stamps.StampsOptionsControl();
            this.express1Options = new ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.Express1StampsSingleSourceControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.express1SettingsControl = new ShipWorks.Shipping.Carriers.Postal.Express1.AutomaticExpress1ControlBase();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountType.Location = new System.Drawing.Point(12, 5);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(132, 13);
            this.labelAccountType.TabIndex = 0;
            this.labelAccountType.Text = "Stamps.com Accounts";
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.Location = new System.Drawing.Point(12, 21);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(459, 104);
            this.accountControl.StampsResellerType = ShipWorks.Shipping.Carriers.Postal.Stamps.StampsResellerType.None;
            this.accountControl.TabIndex = 1;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(0, -1);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.ShipmentTypeCode = ShipWorks.Shipping.ShipmentTypeCode.Stamps;
            this.optionsControl.Size = new System.Drawing.Size(435, 54);
            this.optionsControl.TabIndex = 4;
            // 
            // express1Options
            // 
            this.express1Options.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.express1Options.Location = new System.Drawing.Point(4, 59);
            this.express1Options.Name = "express1Options";
            this.express1Options.Size = new System.Drawing.Size(421, 41);
            this.express1Options.TabIndex = 5;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.labelAccountType);
            this.panelBottom.Controls.Add(this.accountControl);
            this.panelBottom.Location = new System.Drawing.Point(-4, 278);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(495, 131);
            this.panelBottom.TabIndex = 6;
            // 
            // express1SettingsControl
            // 
            this.express1SettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.express1SettingsControl.Location = new System.Drawing.Point(5, 112);
            this.express1SettingsControl.Name = "express1SettingsControl";
            this.express1SettingsControl.Size = new System.Drawing.Size(468, 160);
            this.express1SettingsControl.TabIndex = 7;
            // 
            // StampsSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.express1SettingsControl);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.express1Options);
            this.Controls.Add(this.optionsControl);
            this.Name = "UspsSettingsControl";
            this.Size = new System.Drawing.Size(499, 430);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAccountType;
        private StampsAccountManagerControl accountControl;
        private StampsOptionsControl optionsControl;
        private Stamps.Express1.Express1StampsSingleSourceControl express1Options;
        private System.Windows.Forms.Panel panelBottom;
        private AutomaticExpress1ControlBase express1SettingsControl;
    }
}
