using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    public interface IUpsImportedRateValidator
    {
        /// <summary>
        /// Validates rate information.
        /// </summary>
        /// <exception cref="UpsLocalRatingException" />
        void Validate(List<IUpsPackageRateEntity> packageRates,
            List<IUpsLetterRateEntity> letterRates,
            List<IUpsPricePerPoundEntity> pricesPerPound);
    }
}