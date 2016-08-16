using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic Enum class that can be used to return an enum value and message.
    /// </summary>
    public class EnumResult<TEnum> where TEnum : struct
    {
        protected EnumResult()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumResult(TEnum value, string message)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new NotSupportedException("EnumResult Generic Type must be of type Enum.");
            }

            Value = value;
            Message = message;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumResult(TEnum value) : this(value, string.Empty)
        {
        }

        /// <summary>
        /// The value being returned
        /// </summary>
        public TEnum Value { get; }

        /// <summary>
        /// Message accompanying the value.
        /// </summary>
        public string Message { get; }
    }
}
