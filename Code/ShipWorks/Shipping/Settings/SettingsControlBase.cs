﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Base control for per-service shipment settings controls
    /// </summary>
    public partial class SettingsControlBase : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Can be overridden in derived classes to load service settings
        /// </summary>
        public virtual void LoadSettings()
        {

        }

        /// <summary>
        /// Can be overridden in derived classes to save service settings
        /// </summary>
        public virtual void SaveSettings(ShippingSettingsEntity settings)
        {

        }

        /// <summary>
        /// Called to notify the settings control to refresh itself due to an outside change
        /// </summary>
        public virtual void RefreshContent()
        {

        }
    }
}
