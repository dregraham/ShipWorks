using System;
using System.Runtime.Serialization;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Exception class for ResourceUtility
    /// </summary>
    [Serializable]
    public class ResourceUtilityException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceUtilityException() 
        { 
        
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceUtilityException(string message) : base(message) 
        { 
        
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceUtilityException(string message, Exception inner) : base(message, inner) 
        { 
        
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ResourceUtilityException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { 
        
        }
    }
}
