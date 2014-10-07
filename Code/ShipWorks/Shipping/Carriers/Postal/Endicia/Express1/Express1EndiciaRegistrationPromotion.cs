using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    public class Express1EndiciaRegistrationPromotion : IRegistrationPromotion
    {
        public IEnumerable<PostalAccountRegistrationType> AvailableRegistrationTypes
        {
            get { return new List<PostalAccountRegistrationType> { PostalAccountRegistrationType.Expedited }; }
        }

        public string GetPromoCode(PostalAccountRegistrationType registrationType)
        {
            return "ShipWorks3";
        }
    }
}
