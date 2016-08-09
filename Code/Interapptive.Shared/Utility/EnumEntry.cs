using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Represents a single value entry in an Enum type
    /// </summary>
    public struct EnumEntry<T> where T : struct
    {
        T value;
        DescriptionAttribute description;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumEntry(T value, DescriptionAttribute description)
        {
            this.value = value;
            this.description = description;
            ApiValue = EnumHelper.GetApiValue((Enum) (object) value);
        }

        /// <summary>
        /// The enumerated value
        /// </summary>
        [Obfuscation(Exclude = true)]
        public T Value => value;

        /// <summary>
        /// Same as description.  Useful because its common in binding to use Key\Value as the Display\Value member.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Key => Description;

        /// <summary>
        /// ToString returns the enum Description
        /// </summary>
        public override string ToString() => Description;

        /// <summary>
        /// The description of the enumerated value.
        /// </summary>
        public string Description => description.Description;

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
        public string ApiValue { get; }
    }
}
