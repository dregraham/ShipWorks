using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaInsuranceRestriction : FeatureRestriction, IFeatureRestriction
    {
        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.EndiciaInsurance;

        /// <summary>
        /// Checks the restriction for this feature
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaInsurance ? 
                EditionRestrictionLevel.None : 
                EditionRestrictionLevel.Hidden;
        }
    }
}
