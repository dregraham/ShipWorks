using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    public class UpsSurePostRestriction : FeatureRestriction
    {        
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsSurePostRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.UpsSurePost;

        /// <summary>
        /// Checks the license capabilities to see if UpsSurePost is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.UpsSurePost ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}