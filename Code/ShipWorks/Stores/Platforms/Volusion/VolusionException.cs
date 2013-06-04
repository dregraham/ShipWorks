using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Exception to wrap error messages and codes coming back from failed CA calls
    /// </summary>
    [Serializable]
    public class VolusionException : Exception
    {
        /// <summary>
        /// If the response is invalid Xml, the original string will be here
        /// </summary>
        private string badResponseData = "";

        /// <summary>
        /// Invalid Xml returned by Volusion
        /// </summary>
        public string BadResponseData
        {
            get { return badResponseData; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor for specifying the response from Volusion wasn't xml
        /// </summary>
        public VolusionException(string message, string badResponseData)
            : base(message)
        {
            this.badResponseData = badResponseData;
        }

        /// <summary>
        /// Constructor for just message
        /// </summary>
        public VolusionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected VolusionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            badResponseData = info.GetString("badResponseData");
        }

        /// <summary>
        /// Deserilization 
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("badResponseData", badResponseData);
        }
    }
}
