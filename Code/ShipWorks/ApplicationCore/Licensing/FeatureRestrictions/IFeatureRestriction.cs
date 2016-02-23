using System.Windows.Forms;
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
        /// Handles the restriction for this feature
        /// </summary>
        bool Handle(IWin32Window owner, object data);
    }
}