using Interapptive.Shared.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// UpsPromo that tracks telemetry data
    /// </summary>
    public class TelemetricUpsPromo : IUpsPromo
    {
        readonly ITrackedEvent telemetryEvent;
        readonly IUpsPromo upsPromo;
        readonly bool existingAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public TelemetricUpsPromo(ITrackedEvent telemetryEvent, IUpsPromo upsPromo, bool existingAccount)
        {
            this.existingAccount = existingAccount;
            this.upsPromo = upsPromo;
            this.telemetryEvent = telemetryEvent;
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
        public void Apply()
        {
            upsPromo.Apply();
            LogResult("Applied");
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
        private void LogResult(string result)
        {
            telemetryEvent.AddProperty("Ups.Promo.Result", result);
            telemetryEvent.AddProperty("Ups.Promo.AppliedToExistingAccount", existingAccount ? "true" : "false");
            telemetryEvent.AddProperty("Ups.Promo.AccountNumber", upsPromo.AccountNumber);

            telemetryEvent.Dispose();
        }
    }
}
