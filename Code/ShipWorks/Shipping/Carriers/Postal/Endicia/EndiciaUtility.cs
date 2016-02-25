using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    ///     Utility class for working with Endicia
    /// </summary>
    public static class EndiciaUtility
    {
        /// <summary>
        ///     Indicates if Endicia insurance is both allowed, and turned on and activated
        /// </summary>
        public static bool IsEndiciaInsuranceActive
        {
            get
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    LicenseService licenseService = scope.Resolve<LicenseService>();
                    EditionRestrictionLevel restrictionLevel =
                        licenseService.CheckRestriction(EditionFeature.EndiciaInsurance, null);

                    return
                        restrictionLevel == EditionRestrictionLevel.None &&
                        ShippingSettings.Fetch().EndiciaInsuranceProvider == (int) InsuranceProvider.Carrier;
                }
            }
        }
    }
}