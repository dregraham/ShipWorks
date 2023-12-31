﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Settings to use Express1 as a single-source for all postal shipping services
    /// </summary>
    public class Express1UspsSingleSourceControl : Express1SingleSourceControl
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

            singleSourceCheckBox.Checked = settings.Express1UspsSingleSource;
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

            settings.Express1UspsSingleSource = singleSourceCheckBox.Checked;
        }
    }
}
