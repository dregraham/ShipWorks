using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic Enum class that can be used to return an enum value and message.
    /// </summary>
    public class EnumResult<TEnum> where TEnum : struct
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EnumResult(TEnum value)
        {
            if (!typeof(TEnum).IsEnum) throw new NotSupportedException("EnumResult Generic Type must be of type Enum.");

            Value = value;
        }

        /// <summary>
        /// The value being returned
        /// </summary>
        public TEnum Value { get; private set; }

        /// <summary>
        /// Message accompanying the value.
        /// </summary>
        public string Message { get; set; }
    }
}
