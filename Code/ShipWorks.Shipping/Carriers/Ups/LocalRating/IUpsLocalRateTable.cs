using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    public interface IUpsLocalRateTable
    {
        void Load(Stream stream);

        void AddRates(IEnumerable<UpsLocalRateEntity> rates);

        void AddSurcharges(IEnumerable<UpsLocalRateSurchargeEntity> surcharges);

        void Save(UpsAccountEntity entity);
    }
}
