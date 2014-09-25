using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Settings to use Express1 as a single-source for all postal shipping services
    /// </summary>
    public partial class Express1EndiciaSingleSourceControl : Express1SingleSourceControl
    {
        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings(ShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            singleSourceCheckBox.Checked = settings.Express1EndiciaSingleSource;
        }

        /// <summary>
        /// Save the UI settings to the entity
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Express1EndiciaSingleSource = singleSourceCheckBox.Checked;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // singleSourceCheckBox
            // 
            this.singleSourceCheckBox.Location = new System.Drawing.Point(23, 22);
            // 
            // Express1EndiciaSingleSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "Express1EndiciaSingleSourceControl";
            this.Controls.SetChildIndex(this.singleSourceCheckBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
