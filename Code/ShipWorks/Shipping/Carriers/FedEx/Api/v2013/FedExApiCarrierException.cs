using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ShipWorks.Shipping.Carriers.Api;
using RegistrationNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration.Notification;
using ShipNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.Notification;
using RateNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate.Notification;
using TrackNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Track.Notification;
using PackageMovementNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.Notification;
using AddressValidationNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.AddressValidation.Notification;
using CloseNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.Notification;
using GlobalShipAddressNotification = ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress.Notification;
using CloseNotification2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close.Notification;
using RateNotification2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate.Notification;
using TrackNotification2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track.Notification;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013
{
    /// <summary>
    /// Exception occured that FedEx returned a non Soap-fault error to
    /// </summary>
    [Serializable]
    public class FedExApiCarrierException : CarrierException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiCarrierException(ShipNotification[] notifications)
            : base(GenerateMessage(GetOneError(notifications.Select(x=> new KeyValuePair<string,string>(x.Code,x.Message)))))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiCarrierException(RegistrationNotification[] notifications)
            :base(GenerateMessage(notifications.Select(n => n.Message ?? n.Code)))
        {
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public FedExApiException(RateNotification[] notifications)
        //{
        //    GenerateMessage(notifications.Select(n => n.Message ?? n.Code));
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiCarrierException(TrackNotification2013[] notifications)
            : base(GenerateMessage(notifications.Select(n => n.Message ?? n.Code)))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiCarrierException(PackageMovementNotification[] notifications)
            : base(GenerateMessage(notifications.Select(n => n.Message ?? n.Code)))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiCarrierException(GlobalShipAddressNotification[] notifications)
            : base(GenerateMessage(notifications.Select(n => n.Message ?? n.Code)))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiCarrierException(CloseNotification2013[] notifications)
                :base(GenerateMessage(notifications.Select(n => n.Message ?? n.Code)))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExApiCarrierException(RateNotification2013[] notifications)
            : base(GenerateMessage(GetOneError(notifications.Select(x=> new KeyValuePair<string,string>(x.Code,x.Message)))))
        {
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public FedExApiException(AddressValidationNotification[] notifications)
        //{
        //    GenerateMessage(notifications.Select(n => n.Message ?? n.Code));
        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public FedExApiException(CloseNotification[] notifications)
        //{
        //    GenerateMessage(notifications.Select(n => n.Message ?? n.Code));
        //}

        /// <summary>
        /// Gets the one error. If more than 1, makes sure not to take the "Insured Value is not allowed for SmartPost
        /// </summary>
        private static IEnumerable<string> GetOneError(IEnumerable<KeyValuePair<string, string>> notifications)
        {
            if (notifications.Count() > 1)
            {
                return notifications.Where(x => x.Key != "960").Take(1).Select(n => n.Value ?? n.Key);
            }
            else
            {
                return notifications.Select(n => n.Value ?? n.Key);
            }
        }


        /// <summary>
        /// Generate the error message to display based on the messages returned
        /// </summary>
        private static string GenerateMessage(IEnumerable<string> errors)
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

            return sb.ToString();
        }

        /// <summary>
        /// Make the given error mesage a little more human readable
        /// </summary>
        private static string CleanseError(string error)
        {
            error = error.Replace("Nbr", "Number");
            error = error.Replace("null", "blank");

            error = error.Trim();

            if (!error.EndsWith("."))
            {
                error += ".";
            }

            Match match = Regex.Match(error, "[0-9] - [A-Za-z]");
            if (match.Success)
            {
                error = error.Substring(match.Index + match.Length - 1);
            }

            // When fedex says insured value, they mean declared value
            error = error.Replace("Insured value", "Declared value");
            error = error.Replace("Insured Value", "Declared value");

            return error;
        }
    }
}
