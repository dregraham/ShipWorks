using Interapptive.Shared.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// UpsPromo that tracks telemetry data
    /// </summary>
    public class TelemetricUpsPromo
    {
        readonly ITrackedEvent telemetryEvent;
        readonly IUpsPromo upsPromo;

        /// <summary>
        /// Constructor
        /// </summary>
        public TelemetricUpsPromo(ITrackedEvent telemetryEvent, IUpsPromo upsPromo)
        {
            this.upsPromo = upsPromo;
            this.telemetryEvent = telemetryEvent;
        }

        /// <summary>
        /// Terms and conditions for the promo
        /// </summary>
        public PromoAcceptanceTerms Terms => upsPromo.Terms;

        /// <summary>
        /// Activates the Promo Code
        /// </summary>
        public void Apply(bool existingAccount)
        {
            upsPromo.Apply();
            LogResult("Applied", existingAccount, upsPromo);
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
        private void LogResult(string result, bool existingAccount = false, IUpsPromo promo = null)
        {
            telemetryEvent.AddProperty("Ups.Promo.Result", result);

            if (existingAccount && promo != null)
            {
                telemetryEvent.AddProperty("Ups.Promo.AppliedToExistingAccount", "true");
                telemetryEvent.AddProperty("Ups.Promo.AccountNumber", promo.AccountNumber);
            }
            else
            {
                telemetryEvent.AddProperty("Ups.Promo.AppliedToExistingAccount", "N/A");
                telemetryEvent.AddProperty("Ups.Promo.AccountNumber", "N/A");
            }
        }
    }
}
