using System;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// An event dispatcher intended as a means to notify the shipping settings dialog when 
    /// an USPS account has been created. This was introduced as a means to address the 
    /// problem/complexity of a setup wizard being launched/completed from a control hosted 
    /// multiple levels deep within the ShippingSettingsDlg where shipment types get disabled 
    /// as a result of the setup wizard being completed.
    /// </summary>
    public static class ShippingSettingsEventDispatcher
    {
        /// <summary>
        /// Occurs when a USPS account is created from the ShippingSettingsDlg.
        /// </summary>
        public static event EventHandler<ShippingSettingsEventArgs> UspsAccountCreated;

        public static event EventHandler<ShippingSettingsEventArgs> StampsUspsAutomaticExpeditedChanged;

        /// <summary>
        /// Notifies any listeners when a USPS account has been created.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void DispatchUspsAccountCreated(object source, ShippingSettingsEventArgs eventArgs)
        {
            if (UspsAccountCreated != null)
            {
                UspsAccountCreated(source, eventArgs);
            }
        }

        /// <summary>
        /// Notifies any listeners when an USPS shipping setting has changed.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void DispatchUspsAutomaticExpeditedChanged(object source, ShippingSettingsEventArgs eventArgs)
        {
            if (StampsUspsAutomaticExpeditedChanged != null)
            {
                StampsUspsAutomaticExpeditedChanged(source, eventArgs);
            }
        }
    }
}
