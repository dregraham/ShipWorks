using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// Exceptions relating to the V2 -> V3 data migration 
    /// process.
    /// </summary>
    [Serializable]
    public class MigrationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MigrationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor for just message
        /// </summary>
        public MigrationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected MigrationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Deserilization 
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
