using System;
using System.Collections.Concurrent;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// PromoPolicy class
    /// </summary>
    public class UpsPromoPolicy : IUpsPromoPolicy
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ConcurrentDictionary<long, DateTime> remindLaterAccounts;
        private const int reminderInterval = 8;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoPolicy(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
            remindLaterAccounts = new ConcurrentDictionary<long, DateTime>();
        }

        /// <summary>
        /// Determines whether the specified account is eligible.
        /// </summary>
        public bool IsEligible(IUpsPromo promo)
        {
            // If PromoStatus isn't none, they already made a choice. Return False
            if (promo.GetStatus() != UpsPromoStatus.None)
            {
                return false;
            }

            // PromoStatus is none, check the remindLaterAccounts to see if they are to be reminded later
            if (remindLaterAccounts.ContainsKey(promo.AccountId))
            {
                // RemindLater has been called for the account. If past the interval return true, else false

                DateTime reminderExpiration = remindLaterAccounts[promo.AccountId].AddHours(reminderInterval);
                return reminderExpiration < dateTimeProvider.Now;
            }

            // PromoStatus is none and they havn't asked to be reminded later.
            return true;
        }

        /// <summary>
        /// Adds account to RemindLater so it will not be eligible for the duration of the reminder interval
        /// </summary>
        public void RemindLater(IUpsPromo promo)
        {
            DateTime now = dateTimeProvider.Now;

            remindLaterAccounts.AddOrUpdate(
                promo.AccountId,
                now,
                (key, oldvalue) => now);
        }
    }
}
