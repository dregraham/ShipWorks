using ShipWorks.Data.Model.EntityClasses;

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
        bool IsEligible(UpsAccountEntity account);

        /// <summary>
        /// Adds account to RemindLater so it will not be eligible for the duration of the reminder interval
        /// </summary>
        void RemindLater(UpsAccountEntity account);
    }
}