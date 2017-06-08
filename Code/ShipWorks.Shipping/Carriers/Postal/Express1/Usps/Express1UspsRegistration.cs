using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Usps
{
    /// <summary>
    /// Registration for Express1 Usps
    /// </summary>
    [Component(RegistrationType.Self)]
    public class Express1UspsRegistration : Express1Registration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsRegistration(UspsExpress1RegistrationGateway gateway,
                UspsExpress1RegistrationRepository repository,
                UspsExpress1PasswordEncryptionStrategy encryptionStrategy,
                Express1RegistrationValidator validator) :
            base(ShipmentTypeCode.Express1Usps, gateway, repository, encryptionStrategy, validator)
        {
        }
    }
}
