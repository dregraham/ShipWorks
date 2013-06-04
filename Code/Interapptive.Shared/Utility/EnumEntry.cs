using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Represents a single value entry in an Enum type
    /// </summary>
    public struct EnumEntry<T> where T: struct
    {
        T value;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumEntry(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// The enumerated value
        /// </summary>
        [Obfuscation(Exclude = true)]
        public T Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Same as description.  Useful because its common in binding to use Key\Value as the Display\Value member.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Key
        {
            get
            {
                return Description;
            }
        }

        /// <summary>
        /// ToString returns the enum Description
        /// </summary>
        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// The description of the enumerated value.
        /// </summary>
        public string Description
        {
            get
            {
                return EnumHelper.GetDescription((Enum) (object) value);
            }
        }

        /// <summary>
        /// The image of the enumerated value, or null if none.
        /// </summary>
        public Image Image
        {
            get
            {
                return EnumHelper.GetImage((Enum) (object) value);
            }
        }

        /// <summary>
        /// Get the text to be used as the ApiValue for API's
        /// </summary>
        public string ApiValue
        {
            get 
            { 
                return EnumHelper.GetApiValue((Enum) (object) value); 
            }
        }
    }
}
