using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    public interface IUpsOpenAccountRepository
    {
        /// <summary>
        /// Saves the given UPS account entity to the underlying data source.
        /// </summary>
        /// <param name="account">The account.</param>
        void Save(UpsAccountEntity account);
    }
}
