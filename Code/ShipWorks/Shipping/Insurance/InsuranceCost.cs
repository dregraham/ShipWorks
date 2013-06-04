using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Insurance
{
    public class InsuranceCost
    {
        string infoMessage;

        public decimal? ShipWorks { get; set; }
        public decimal? Carrier { get; set; }
        public string InfoMessage { get { return infoMessage; } }
        public bool AdvertisePennyOne { get; set; }

        /// <summary>
        /// Add the given message to be displayed to the user
        /// </summary>
        public void AddInfoMessage(string message)
        {
            if (infoMessage != null)
            {
                infoMessage += "\n\n" + message;
            }
            else
            {
                infoMessage = message;
            }
        }
    }
}
