using System.Linq;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    /// <summary>
    /// Restricts ShipWorks to a specific set of Ups Account Numbers
    /// </summary>
    public class UpsAccountNumberRestriction : FeatureRestriction
    {
        /// <summary>
        /// Works on the UpsAccountNumbers EditionFeature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.UpsAccountNumbers;

        /// <summary>
        /// Checks to see if the given account is allowed by the given ILicenseCapabilities
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            string account = data as string;

            // Check to see that the account is not blank or null
            // the capabilities has a collection of UpsAccountNumbers
            // the given account is not in the capabilities UpsAccountNumbers collection
            if (!string.IsNullOrWhiteSpace(account) && 
                capabilities.UpsAccountNumbers.Any() && 
                !capabilities.UpsAccountNumbers.Contains(account.ToLower()))
            {
                return EditionRestrictionLevel.Forbidden;
            }

            return EditionRestrictionLevel.None;
        }
    }
}