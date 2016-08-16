using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public interface IFeatureRestriction
    {
        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        EditionFeature EditionFeature { get; }

        /// <summary>
        /// Checks the restriction for this feature
        /// </summary>
        EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data);

        /// <summary>
        /// Checks the restriction for this feature with a reason for the restriction
        /// </summary>
        EnumResult<EditionRestrictionLevel> CheckWithReason(ILicenseCapabilities capabilities, object data);

        /// <summary>
        /// Handles the restriction for this feature
        /// </summary>
        bool Handle(IWin32Window owner, ILicenseCapabilities capabilities, object data);
    }
}