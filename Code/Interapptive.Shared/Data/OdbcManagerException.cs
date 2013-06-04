using System;
using System.Runtime.Serialization;

namespace Interapptive.Shared.Data
{
    [Serializable]
    public class OdbcManagerException : Exception
    {
        public OdbcManagerException() 
        { 
        
        }

        public OdbcManagerException(string message) : base(message) 
        { 
        
        }

        public OdbcManagerException(string message, Exception inner)
            : base(message, inner) 
        { 
        
        }

        protected OdbcManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { 
        
        }
    }
}
