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
        StoreTypeCode typeCode = StoreTypeCode.Invalid;

        string key;

        bool isMetered = false;
        bool isTrial = false;

        /// <summary>
        /// Creates a license from a key
        /// </summary>
        public ShipWorksLicense(string key)
        {
            this.key = key;

            ReadLicense(key);
        }

        /// <summary>
        /// The store type the license represents
        /// </summary>
        public StoreTypeCode StoreTypeCode
        {
            get
            {
                return typeCode;
            }
        }

        /// <summary>
        /// The actual license key
        /// </summary>
        public string Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Indiciates if this is a metered license that is billed monthly, as opposed
        /// to our old legacy licenses.
        /// </summary>
        public bool IsMetered
        {
            get
            {
                return isMetered;
            }
        }

        /// <summary>
        /// Indiciates if this is a trial license.
        /// </summary>
        public bool IsTrial
        {
            get
            {
                return isTrial;
            }
        }

        /// <summary>
        /// Indicates if this instance represents a valid license.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return StoreTypeCode != StoreTypeCode.Invalid;
            }
        }

        /// <summary>
        /// Gets a user-presentable string description of the license.
        /// </summary>
        public string Description
        {
            get
            {
                if (StoreTypeCode == StoreTypeCode.Invalid)
                {
                    return "No License";
                }
                else
                {
                    return StoreTypeManager.GetType(StoreTypeCode).StoreTypeName;
                }
            }
        }

        /// <summary>
        /// Read the details out of the license.
        /// </summary>
        private void ReadLicense(string key)
        {
            typeCode = StoreTypeCode.Invalid;

            if (key.Length == 0)
            {
                isTrial = HasInTrial(key);
                return;
            }

            // Try to decode it as the new metered version first.
            RawLicense license = LicenseDecoder.Decode(key, "3.m");

            // Now see if its a freemium license
            if (license == null)
            {
                license = LicenseDecoder.Decode(key, "2.f");
            }

	        // Now see if its a freemium license
            if (license == null)
            {
                license = LicenseDecoder.Decode(key, "2.m");
            }

            // Try the new UPS Only salt
            if (license == null)
            {
                license = LicenseDecoder.Decode(key, "3.u");
            }

            // Now try to decode as using the old legacy salt
            if (license == null)
            {
                license = LicenseDecoder.Decode(key, "2.x");
            }

            // If it fails for any reason, its invalid
            if (license == null)
            {
                return;
            }

            // For our new metered licenses, there is always an M in the 4th location.
            isMetered = license.Data1[3] == 'M';

            // See if its a trial
            if (isMetered && license.Data2[0] == 'T')
            {
                isTrial = true;
            }
            else
            {
                isTrial = HasInTrial(key);
            }

            // Look for this store type
            foreach (StoreType storeType in StoreTypeManager.StoreTypes)
            {
                if (license.Data1.StartsWith(storeType.LicenseSalt))
                {
                    typeCode = storeType.TypeCode;
                    break;
                }
            }
        }

        /// <summary>
        /// Is there a license that is in trial
        /// </summary>
        private bool HasInTrial(string key)
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
                            .FirstOrDefault(l => l.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))?
                            .IsInTrial == true;
                    }

                    return licenses?.Any(l => l.IsInTrial) == true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error occurred in ShipWorksLicense.HasInTrial", ex);
                return false;
            }
        }
    }
}
