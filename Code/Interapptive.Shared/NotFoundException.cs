using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Thrown when something could not be found.  Usually as in a collection.
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() 
        { 
        
        }

        public NotFoundException(string message) : base(message) 
        { 
        
        }

        public NotFoundException(string message, Exception inner) : base(message, inner) 
        { 
        
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { 
        
        }
    }
}
