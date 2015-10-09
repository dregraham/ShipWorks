using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// UpsPromoPolicy Interface
    /// </summary>
    public interface IUpsPromoPolicy
    {
        /// <summary>
        /// Determines whether the specified account is eligible for the promo
        /// </summary>
        bool IsEligible(IUpsPromo promo);

        /// <summary>
        /// Adds account to RemindLater so it will not be eligible for the duration of the reminder interval
        /// </summary>
        void RemindLater(IUpsPromo promo);
    }
}