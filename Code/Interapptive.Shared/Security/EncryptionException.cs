using System;
using System.Runtime.Serialization;

namespace Interapptive.Shared.Security
{
    [Serializable]
    public class EncryptionException : Exception
    {
        public EncryptionException()
        {
        }

        public EncryptionException(string message) : base(message)
        {
        }

        public EncryptionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EncryptionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}