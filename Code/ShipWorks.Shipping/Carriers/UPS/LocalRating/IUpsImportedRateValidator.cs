using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Validates imported rates.
    /// </summary>
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