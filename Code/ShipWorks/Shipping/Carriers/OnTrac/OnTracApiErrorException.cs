using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Exceptions related to OnTrac API Errors
    /// </summary>
    public class OnTracApiErrorException : OnTracException
    {
        private const string onTracDeliveryAreaMessage = "OnTrac services all zip codes in CA.\nIn AZ,CO, ID, NV, OR, UT and WA, OnTrac services all major metro areas, but not all zip codes.";
        
        static private readonly List<DetailsReplacement> ErrorReplacements = new List<DetailsReplacement>
            {
                new DetailsReplacement("Invalid Username", "The specified account number and password are not correct.", false),
                new DetailsReplacement("Delivery Zip Not Serviced", "The \"Ship To\" zip code entered is invalid or not serviced by OnTrac.  Please verify that the \"Ship To\" zip code is correct and try again.\n" + onTracDeliveryAreaMessage, true),
                new DetailsReplacement("Invalid Shipper State",     "The \"Ship From\" state entered is invalid or not serviced by OnTrac.  Please verify that the \"Ship From\" state is correct and try again.\n" + onTracDeliveryAreaMessage, true),
                new DetailsReplacement("Pickup Zip Not Serviced",   "The \"Ship From\" zip code entered is invalid or not serviced by OnTrac.\n" + onTracDeliveryAreaMessage, true)
            };

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracApiErrorException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracApiErrorException(string apiError)
            : base(apiError)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracApiErrorException(string apiError, Exception inner)
            : base(apiError, inner)
        {

        }

        /// <summary>
        /// Override the outgoing message to provide customized "English" display to users.
        /// </summary>
        public override string Message
        {
            get
            {
                DetailsReplacement details = FindReplacement();
                return (details != null) ? details.NewMessage : string.Format("OnTrac returned the following error:\n{0}", base.Message);
            }
        }

        /// <summary>
        /// Was the exception generated because OnTrac does not service an address?
        /// </summary>
        public override bool DoesNotServiceLocation
        {
            get
            {
                DetailsReplacement details = FindReplacement();
                return (details != null) && details.DoesNotServiceLocation;
            }
            protected set
            {
                base.DoesNotServiceLocation = value;
            }
        }

        /// <summary>
        /// Gets a message replacement if there is one
        /// </summary>
        private DetailsReplacement FindReplacement()
        {
            return ErrorReplacements.FirstOrDefault(d => base.Message.Contains(d.OriginalMessage));
        }

        /// <summary>
        /// Container for message replacements
        /// </summary>
        private class DetailsReplacement
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public DetailsReplacement(string originalMessage, string newMessage, bool doesNotServiceLocation)
            {
                OriginalMessage = originalMessage;
                NewMessage = newMessage;
                DoesNotServiceLocation = doesNotServiceLocation;
            }

            /// <summary>
            /// Fragment of the original error message from OnTrac's api
            /// </summary>
            public string OriginalMessage { get; private set; }

            /// <summary>
            /// Replacement message
            /// </summary>
            public string NewMessage { get; private set; }

            /// <summary>
            /// Is the error generated because OnTrac does not service the location?
            /// </summary>
            public bool DoesNotServiceLocation { get; private set; }
        }
    }
}
