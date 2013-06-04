using System;
using System.Runtime.Serialization;

namespace Interapptive.Shared.IO.Text.Ini
{
    /// <summary>
    /// IniFile specific exception
    /// </summary>
    [Serializable]
    public class IniFileException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public IniFileException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IniFileException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IniFileException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected IniFileException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {

        }
    }
}
