using System.Windows.Forms;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public abstract class FeatureRestriction : IFeatureRestriction
    {
        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public abstract EditionFeature EditionFeature { get; }

        /// <summary>
        /// Checks the restriction for this feature
        /// </summary>
        public abstract EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data);

        /// <summary>
        /// Nothing to handle, return false
        /// </summary>
        public virtual bool Handle(IWin32Window owner, ILicenseCapabilities capabilities, object data)
        {
            EditionRestrictionLevel level = Check(capabilities, data);
            return level == EditionRestrictionLevel.None;
        }
    }
}
