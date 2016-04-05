using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.ShipmentTypeFunctionality
{
    /// An implementation of the IFeatureRestriction interface that checks whether shipping 
    /// account registration is restricted based on an instance of ILicenseCapabilities.
    public class AccountRegistrationRestriction : ShipmentTypeFunctionalityRestriction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRegistrationRestriction"/> class.
        /// </summary>
        public AccountRegistrationRestriction(IMessageHelper messageHelper)
            : base(ShipmentTypeRestrictionType.AccountRegistration, messageHelper)
        {            
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public override EditionFeature EditionFeature
        {
            get { return EditionFeature.ShipmentTypeRegistration; }
        }
    }
}
