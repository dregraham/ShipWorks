using System.Windows.Forms;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaConsolidatorRestriction : IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.EndiciaConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if Endicia consolidators are enabled
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }

        /// <summary>
        /// Nothing to handle, return false
        /// </summary>
        public bool Handle(IWin32Window owner, object data)
        {
            return false;
        }
    }
}