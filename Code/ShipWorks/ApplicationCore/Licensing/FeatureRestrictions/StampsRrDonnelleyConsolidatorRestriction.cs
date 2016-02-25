using System.Windows.Forms;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsRrDonnelleyConsolidatorRestriction : IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.StampsRrDonnelleyConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if StampsRrDonnelleyConsolidator is enabled
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsRrDonnelleyConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }

        /// <summary>
        /// Nothing to handle, return false
        /// </summary>
        public bool Handle(IWin32Window owner, object data) => false;
    }
}
