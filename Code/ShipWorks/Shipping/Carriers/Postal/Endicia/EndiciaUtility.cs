using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Editions;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Utility class for working with Endicia
    /// </summary>
    public static class EndiciaUtility
    {
        /// <summary>
        /// Indicates if Endicia insurance is both allowed, and turned on and activated
        /// </summary>
        public static bool IsEndiciaInsuranceActive
        {
            get
            {
                return
                    EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.EndiciaInsurance).Level == EditionRestrictionLevel.None &&
                    ShippingSettings.Fetch().EndiciaInsuranceProvider == (int) InsuranceProvider.Carrier;
            }
        }
    }
}
