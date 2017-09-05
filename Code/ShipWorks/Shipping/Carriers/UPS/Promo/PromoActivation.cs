using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// Holds information about PromoActivation
    /// </summary>
    public class PromoActivation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PromoActivation(PromoDiscountResponse upsResponse)
        {
            IsSuccessful = ((upsResponse?.Response?.ResponseStatus?.Code ?? "0") == "1");
            Info = upsResponse?.Response?.Alert?.FirstOrDefault()?.Description ?? string.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PromoActivation(Exception exception)
        {
            Exception = exception;
            IsSuccessful = false;
            Info = exception.Message;
        }

        /// <summary>
        /// Gets any information if there were any alerts
        /// </summary>
        public string Info { get; }

        /// <summary>
        /// Returns true if activation was sucessful.
        /// </summary>
        public bool IsSuccessful { get; }

        public Exception Exception { get; }
    }
}