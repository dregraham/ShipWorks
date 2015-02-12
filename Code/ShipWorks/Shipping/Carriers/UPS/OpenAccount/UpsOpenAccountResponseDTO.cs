using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// DTO to store information 
    /// </summary>
    public class UpsOpenAccountResponseDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountResponse"/> class.
        /// </summary>
        public UpsOpenAccountResponseDTO(string shipperNumber, string notifyTime)
        {
            if (string.IsNullOrEmpty(shipperNumber))
            {
                throw new UpsOpenAccountException("Ups didn't return a new account number.");
            }

            AccountNumber = shipperNumber;
            DateTime parsedNotifyTime;
            NotifyTime = null;

            if (DateTime.TryParseExact(notifyTime,"hhmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out parsedNotifyTime))
            {
                NotifyTime = parsedNotifyTime;
            }
        }

        /// <summary>
        /// UPS assigned account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// SmartPost notify time. If not SmartPost, this will be null.
        /// </summary>
        public DateTime? NotifyTime { get; set; }
    }
}
