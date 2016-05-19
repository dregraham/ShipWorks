using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    public interface IAddressValidator
    {
        void Validate(AddressAdapter addressAdapter, bool canAdjustAddress, Action<ValidatedAddressEntity, IEnumerable<ValidatedAddressEntity>> saveAction);
    }
}