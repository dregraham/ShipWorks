using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Exception to wrap error messages and codes coming back from failed Groupon calls
    /// </summary>
    [Serializable]
    class GrouponException : Exception
    {
        /// <summary>
        /// If the response is invalid Json, the original string will be here
        /// </summary>
        private string badResponseData = "";

        /// <summary>
        /// Invalid Json returned by Groupon
        /// </summary>
        public string BadResponseData
        {
            get { return badResponseData; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor for specifying the response from Groupon wasn't valid
        /// </summary>
        public GrouponException(string message, string badResponseData)
            : base(message)
        {
            this.badResponseData = badResponseData;
        }

        /// <summary>
        /// Constructor for just message
        /// </summary>
        public GrouponException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected GrouponException(SerializationInfo info, StreamingContext context)
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
