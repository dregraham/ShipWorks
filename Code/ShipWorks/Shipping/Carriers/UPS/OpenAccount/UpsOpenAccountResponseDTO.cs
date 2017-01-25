using System;
using System.Globalization;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// DTO to store information 
    /// </summary>
    public class UpsOpenAccountResponseDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountResponseDTO"/> class.
        /// </summary>
        public UpsOpenAccountResponseDTO(string shipperNumber, string notifyTime)
        {
            if (string.IsNullOrEmpty(shipperNumber))
            {
                throw new UpsOpenAccountException("Ups didn't return a new account number.");
            }

            AccountNumber = shipperNumber;
            DateTime parsedNotifyTime;
            UpsSmartPickupNotifyTime = null;

            if (DateTime.TryParseExact(notifyTime,"hhmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out parsedNotifyTime))
            {
                UpsSmartPickupNotifyTime = parsedNotifyTime;
            }
        }

        /// <summary>
        /// UPS assigned account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Smart Pickup notify time. If customer doesn't have Smart Pickup, this will be null.
        /// </summary>
        public DateTime? UpsSmartPickupNotifyTime { get; set; }
    }
}
