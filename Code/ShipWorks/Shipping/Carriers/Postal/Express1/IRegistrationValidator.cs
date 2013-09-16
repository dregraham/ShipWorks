using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    public interface IRegistrationValidator
    {
        List<Express1ValidationError> Validate(Express1Registration registration);
    }
}