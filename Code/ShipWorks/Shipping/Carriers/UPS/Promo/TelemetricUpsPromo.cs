using Interapptive.Shared.Metrics;
using System;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// UpsPromo that tracks telemetry data
    /// </summary>
    public class TelemetricUpsPromo : IUpsPromo
    {
        private readonly IUpsPromo upsPromo;
        private readonly Func<string, ITrackedEvent> telemetryEventFunc;
        private readonly UpsPromoSource source;
        private readonly UpsPromoAccountType accountType;

        /// <summary>
        /// Constructor
        /// </summary>
        public TelemetricUpsPromo(Func<string, ITrackedEvent> telemetryEventFunc, IUpsPromo upsPromo, UpsPromoSource source, UpsPromoAccountType accountType)
        {
            this.telemetryEventFunc = telemetryEventFunc;
            this.upsPromo = upsPromo;
            this.source = source;
            this.accountType = accountType;
        }

        /// <summary>
        /// The Access License Number
        /// </summary>
        public string AccessLicenseNumber => upsPromo.AccessLicenseNumber;

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        public long AccountId => upsPromo.AccountId;

        /// <summary>
        /// The UPS Account Number
        /// </summary>
        public string AccountNumber => upsPromo.AccountNumber;

        /// <summary>
        /// The Country Code of the UPS Account
        /// </summary>
        public string CountryCode => upsPromo.CountryCode;

        /// <summary>
        /// The UPS Accounts Password
        /// </summary>
        public string Password => upsPromo.Password;

        /// <summary>
        /// Gets the promo code
        /// </summary>
        public string PromoCode => upsPromo.PromoCode;

        /// <summary>
        /// Terms and conditions for the promo
        /// </summary>
        public PromoAcceptanceTerms Terms => upsPromo.Terms;

        /// <summary>
        /// The UPS Accounts UserId
        /// </summary>
        public string Username => upsPromo.Username;

        /// <summary>
        /// Activates the Promo Code
        /// </summary>
        public PromoActivation Apply()
        {
            PromoActivation result = upsPromo.Apply();

            LogResult("Accepted", result);

            return result;
        }

        /// <summary>
        /// Sets the PromoStatus of the UpsAccount to Declined
        /// </summary>
        public void Decline()
        {
            upsPromo.Decline();
            LogResult("Declined");
        }

        /// <summary>
        /// Get the promo status
        /// </summary>
        /// <returns></returns>
        public UpsPromoStatus GetStatus() => upsPromo.GetStatus();

        /// <summary>
        /// Remind the user of the paromo later
        /// </summary>
        public void RemindMe()
        {
            upsPromo.RemindMe();
            LogResult("Remind Later");
        }

        /// <summary>
        /// Log the results to telemetry
        /// </summary>
        private void LogResult(string action)
        {
            ITrackedEvent telemetryEvent = telemetryEventFunc("Ups.Promo");

            LogResult(action, telemetryEvent);

            telemetryEvent.Dispose();
        }

        /// <summary>
        /// Log the results to telemetry
        /// </summary>
        private void LogResult(string action, PromoActivation activation)
        {
            ITrackedEvent telemetryEvent = telemetryEventFunc("Ups.Promo");

            telemetryEvent.AddProperty("Ups.Promo.Acceptance.Result",
                activation?.IsSuccessful ?? false ? "Applied" : "Declined by UPS");
            LogResult(action, telemetryEvent);

            telemetryEvent.Dispose();
        }

        /// <summary>
        /// Log the results to the telemetry event
        /// </summary>
        private void LogResult(string action, ITrackedEvent telemetryEvent)
        {
            telemetryEvent.AddProperty("Ups.Promo.Result", action);
            telemetryEvent.AddProperty("Ups.Promo.AppliedToExistingAccount", source == UpsPromoSource.PromoFootnote ? "true" : "false");
            telemetryEvent.AddProperty("Ups.Promo.AccountType", EnumHelper.GetDescription(accountType));
            telemetryEvent.AddProperty("Ups.Promo.AccountNumber", upsPromo.AccountNumber);
        }
    }
}
