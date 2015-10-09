using System;
using System.Collections.Concurrent;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;

namespace ShipWorks.Shipping.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// Static Class - 
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
        public bool IsEligible(UpsAccountEntity account)
        {
            // If PromoStatus isn't none, they already made a choice. Return False
            if ((UpsPromoStatus) account.PromoStatus != UpsPromoStatus.None)
            {
                return false;
            }

            // PromoStatus is none, check the remindLaterAccounts to see if they are to be reminded later
            if (remindLaterAccounts.ContainsKey(account.UpsAccountID))
            {
                // RemindLater has been called for the account. If past the interval return true, else false
                return remindLaterAccounts[account.UpsAccountID].AddHours(reminderInterval) < dateTimeProvider.Now;
            }

            // PromoStatus is none and they havn't asked to be reminded later.
            return true;
        }

        /// <summary>
        /// Adds account to RemindLater so it will not be eligible for the duration of the reminder interval
        /// </summary>
        public void RemindLater(UpsAccountEntity account)
        {
            var now = dateTimeProvider.Now;

            remindLaterAccounts.AddOrUpdate(
                account.UpsAccountID,
                now,
                (key, oldvalue) => now);
        }

    }
}
