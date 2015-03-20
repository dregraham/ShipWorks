using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Represents an error returned by the Endicia API
    /// </summary>
    public class EndiciaApiException : EndiciaException
    {
        int code;
        PostalServiceType postalService;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaApiException(int code, string message, PostalServiceType postalService = (PostalServiceType) (-1))
            : base(message)
        {
            this.code = code;
            this.postalService = postalService;
        }

        /// <summary>
        /// The error code returned by endicia
        /// </summary>
        public int ErrorCode
        {
            get { return code; }
        }

        /// <summary>
        /// The original error message given by Endicia.  The Message property overrides this in some cases to provide a better english translation.
        /// </summary>
        public string ErrorDetail
        {
            get { return base.Message; }
        }

        /// <summary>
        /// Custom messaging
        /// </summary>
        public override string Message
        {
            get
            {
                if (code == 412 && base.Message.Contains("FlatRatePaddedEnvelope"))
                {
                    return "Flat Rate Padded Envelope is only available for Commercial Plus customers.";
                }

                if (code == 1001)
                {
                    if (base.Message.Contains("MailpieceShape"))
                    {
                        return "The packaging type is not valid for the selected service.";
                    }

                    if (base.Message.Contains("MailClass"))
                    {
                        if (ShipmentTypeManager.IsEndiciaDhl(postalService))
                        {
                            return "Your Endicia account has not been enabled to use the selected DHL service.";
                        }
                        else if (ShipmentTypeManager.IsConsolidator(postalService))
                        {
                            return "Your Endicia account has not been enabled to use the selected service.";
                        }
                    }

                    if (base.Message.Contains("SortType"))
                    {
                        return "The selected Sort Type in the 'Entry Facility' section is not valid for your shipper account.";
                    }
                }

                if (code == 12507)
                {
                    return "The Endicia account passphrase is incorrect.";
                }

                if (code == 3027)
                {
                    return "Your Endicia account does not support shipping with the selected DHL GM service.";
                }

                // Strip endicia internal error log ids
                string message = Regex.Replace(base.Message, "Error encountered \\(Log ID:.*?\\)", "", RegexOptions.IgnoreCase);

                // Strip the error number
                message = Regex.Replace(message, "\\[\\d+\\]", "", RegexOptions.IgnoreCase);

                // Strip extra spaces that were created
                message = Regex.Replace(message, "[ ]{2,50}", " ");

                // Replace service codes with service names.
                foreach (var postalEnum in EnumHelper.GetEnumList<PostalServiceType>().Where(entry => !string.IsNullOrWhiteSpace(entry.ApiValue)))
                {
                    message = message.Replace(postalEnum.ApiValue, "'" + postalEnum.Description + "'");
                }

                return message.Trim();
            }
        }

    }
}
