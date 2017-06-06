using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Endicia
{
    /// <summary>
    /// Registration for Express1 Endicia
    /// </summary>
    [Component(RegistrationType.Self)]
    public class Express1EndiciaRegistration : Express1Registration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaRegistration(EndiciaExpress1RegistrationGateway gateway,
                EndiciaExpress1RegistrationRepository repository,
                EndiciaExpress1PasswordEncryptionStrategy encryptionStrategy,
                Express1RegistrationValidator validator) :
            base(ShipmentTypeCode.Express1Endicia, gateway, repository, encryptionStrategy, validator)
        {
        }
    }
}
