using System;
using System.Linq;
using Autofac;
using log4net;
using ShipWorks.ApplicationCore.Licensing.Decoding;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Represents a license in ShipWorks
    /// </summary>
    public class ShipWorksLicense : IShipWorksLicense
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksLicense));

        /// <summary>
        /// Creates a license from a key
        /// </summary>
        public ShipWorksLicense(string key)
        {
            Key = key;

            ReadLicense();
        }

        /// <summary>
        /// The store type the license represents
        /// </summary>
        public StoreTypeCode StoreTypeCode { get; private set; } = StoreTypeCode.Invalid;

        /// <summary>
        /// The actual license key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Indicates if this is a metered license that is billed monthly, as opposed
        /// to our old legacy licenses.
        /// </summary>
        public bool IsMetered { get; private set; } = false;

        /// <summary>
        /// Indicates if this is a trial license.
        /// </summary>
        public bool IsTrial { get; private set; } = false;

        /// <summary>
        /// Indicates if this instance represents a valid license.
        /// </summary>
        public bool IsValid => StoreTypeCode != StoreTypeCode.Invalid;

        /// <summary>
        /// Read the details out of the license.
        /// </summary>
        private void ReadLicense()
        {
            StoreTypeCode = StoreTypeCode.Invalid;

            if (Key.Length == 0)
            {
                IsTrial = HasInTrial();
                return;
            }

            // Try to decode it as the new metered version first.
            RawLicense license = LicenseDecoder.Decode(Key, "3.m");

            // Now see if its a freemium license
            if (license == null)
            {
                license = LicenseDecoder.Decode(Key, "2.f");
            }

	        // Now see if its a freemium license
            if (license == null)
            {
                license = LicenseDecoder.Decode(Key, "2.m");
            }

            // Try the new UPS Only salt
            if (license == null)
            {
                license = LicenseDecoder.Decode(Key, "3.u");
            }

            // Now try to decode as using the old legacy salt
            if (license == null)
            {
                license = LicenseDecoder.Decode(Key, "2.x");
            }

            // If it fails for any reason, its invalid
            if (license == null)
            {
                return;
            }

            // For our new metered licenses, there is always an M in the 4th location.
            IsMetered = license.Data1[3] == 'M';

            // See if its a trial
            IsTrial = IsMetered && license.Data2[0] == 'T' || HasInTrial();

            // Look for this store type
            foreach (StoreType storeType in StoreTypeManager.StoreTypes)
            {
                if (license.Data1.StartsWith(storeType.LicenseSalt))
                {
                    StoreTypeCode = storeType.TypeCode;
                    break;
                }
            }
        }

        /// <summary>
        /// Is there a license that is in trial
        /// </summary>
        private bool HasInTrial()
        {
            // To be safe, if anything throws, just return false so that ShipWorks acts as it did before.
            try
            {
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var licenseService = lifetimeScope.Resolve<ILicenseService>();
                    var licenses = licenseService
                        .GetLicenses();

                    if (licenseService.IsLegacy)
                    {
                        return licenses?
                            .FirstOrDefault(l => l.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase))?
                            .IsInTrial == true;
                    }

                    return licenses?.Any(l => l.IsInTrial) == true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred in ShipWorksLicense.HasInTrial", ex);
                return false;
            }
        }
    }
}
