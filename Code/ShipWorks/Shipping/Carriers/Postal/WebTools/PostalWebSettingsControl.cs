using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// User control for USPS WebTools settings
    /// </summary>
    public partial class PostalWebSettingsControl : SettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            originManagerControl.Initialize();
        }

        /// <summary>
        /// Refresh the content of the control
        /// </summary>
        public override void RefreshContent()
        {
            base.RefreshContent();

            originManagerControl.Initialize();
        }
    }
}
