using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using System.IO;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.PayPal
{
    [Serializable]
    public class PayPalException : Exception
    {
        

        // collection of errors returned by a PayPal call
        private List<PayPalErrorItem> errors = new List<PayPalErrorItem>();
        public IList<PayPalErrorItem> Errors
        {
            get { return errors.AsReadOnly(); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalException(string message) 
            : base(message)
        {

        }

        public PayPalException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        protected PayPalException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            errors = info.GetValue("errors", typeof(List<PayPalErrorItem>)) as List<PayPalErrorItem>;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // add our itemns
            info.AddValue("errors", errors);
        }


        public PayPalException(AbstractResponseType ppResponse) 
            : base(GetExceptionText(ppResponse))
        {
            if (ppResponse == null)
            {
                throw new ArgumentNullException("ppResponse");
            }

            // takes the errors from the response
            foreach (ErrorType error in ppResponse.Errors)
            {
                errors.Add(new PayPalErrorItem(error.ErrorCode, error.ShortMessage));
            }
        }

        /// <summary>
        /// Builds the exception message from the errors in the class
        /// </summary>
        private static string GetExceptionText(AbstractResponseType response)
        {
            if (response.Errors.Length == 0)
            {
                return "";
            }

            string message = "The PayPal server returned the following errors:\n" + Environment.NewLine;
            foreach (ErrorType error in response.Errors)
            {
                message += string.Format("{0} (#{1})\n", error.LongMessage, error.ErrorCode);
            }

            return message;
        }
    }
}
