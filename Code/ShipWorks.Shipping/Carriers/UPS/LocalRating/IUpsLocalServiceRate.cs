using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    public interface IUpsLocalServiceRate
    {
        /// <summary>
        /// Adds the amount.
        /// </summary>
        void AddAmount(decimal amount, string surchargeName);

        /// <summary>
        /// Logs the specified log.
        /// </summary>
        void Log(StringBuilder logEntry);

        /// <summary>
        /// The guaranteed days automatic delivery if provided by Ups
        /// </summary>
        int? GuaranteedDaysToDelivery { get; }

        /// <summary>
        /// The service the rate is for
        /// </summary>
        UpsServiceType Service { get; }

        /// <summary>
        /// The cost to ship with the service
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Indicates if the rate is a negotiated "Account Based Rate" (ABR)
        /// </summary>
        bool Negotiated { get; }
    }
}