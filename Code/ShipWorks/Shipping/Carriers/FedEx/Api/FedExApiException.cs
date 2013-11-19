using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using RegistrationNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.Notification;
using ShipNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.Notification;
using RateNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate.Notification;
using TrackNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Track.Notification;
using PackageMovementNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.Notification;
using AddressValidationNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.AddressValidation.Notification;
using CloseNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.CloseService;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Exception occured that FedEx returned a non Soap-fault error to
    /// </summary>
    class FedExApiException : FedExException
    {
        string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiException(AddressValidationNotification[] notifications)
        {
            GenerateMessage(notifications.Select(n => n.Message ?? n.Code));
        }

        /// <summary>
        /// Generate the error message to display based on the messages returned
        /// </summary>
        private void GenerateMessage(IEnumerable<string> errors)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("FedEx returned the following error{0}:", errors.Count() > 1 ? "s" : "");

            foreach (string error in errors)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }

                sb.Append("    " + CleanseError(error));

            }

            message = sb.ToString();
        }

        /// <summary>
        /// Make the given error mesage a little more human readable
        /// </summary>
        private string CleanseError(string error)
        {
            error = error.Replace("Nbr", "Number");
            error = error.Replace("null", "blank");

            error = error.Trim();
             
            if (!error.EndsWith("."))
            {
                error += ".";
            }

            Match match = Regex.Match(error, " - [A-Za-z]");
            if (match.Success)
            {
                error = error.Substring(match.Index + match.Length - 1);
            }

            // When fedex says insured value, they mean declared value
            error = error.Replace("Insured value", "Declared value");
            error = error.Replace("Insured Value", "Declared value");

            return error;
        }

        /// <summary>
        /// The exception message
        /// </summary>
        public override string Message
        {
            get
            {
                return message;
            }
        }
    }
}
