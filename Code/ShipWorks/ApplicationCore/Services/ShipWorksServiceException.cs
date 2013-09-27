using System;
using System.Runtime.Serialization;


namespace ShipWorks.ApplicationCore.Services
{
    [Serializable]
    public class ShipWorksServiceException : Exception
    {
        public ShipWorksServiceException(string message)
            : base(message) 
        { 
        
        }

        public ShipWorksServiceException(string message, Exception innerException)
            : base(message, innerException) 
        { 
        
        }

        protected ShipWorksServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { 
        
        }
    }
}
