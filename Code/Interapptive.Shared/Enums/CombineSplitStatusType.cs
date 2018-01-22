using System.ComponentModel;
using System.Reflection;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Enum representing an order's combined, split, or none status
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum CombineSplitStatusType
    {
        [Description("None")]
        None = 0,

        [Description("Combined")]
        Combined = 1,

        [Description("Split")]
        Split = 2,

        [Description("Both")]
        Both = 3
    }

    /// <summary>
    /// Extension methods for CombineSplitStatusType
    /// </summary>
    public static class CombineSplitStatusTypeExtensions
    {
        /// <summary>
        /// Determines if the "combined" value should be Combined or Both, depending on current value.
        /// </summary>
        public static CombineSplitStatusType AsCombined(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.None || value == CombineSplitStatusType.Combined ? 
                CombineSplitStatusType.Combined : 
                CombineSplitStatusType.Both;
        }

        /// <summary>
        /// Determines if the given value is a combined status, i.e. Combined or Both
        /// </summary>
        public static bool IsCombined(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.Combined ||
                   value == CombineSplitStatusType.Both;
        }

        /// <summary>
        /// Determines if the "split" value should be Split or Both, depending on current value.
        /// </summary>
        public static CombineSplitStatusType AsSplit(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.None || value == CombineSplitStatusType.Split ?
                CombineSplitStatusType.Split :
                CombineSplitStatusType.Both;
        }

        /// <summary>
        /// Determines if the given value is a split status, i.e. Split or Both
        /// </summary>
        public static bool IsSplit(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.Split ||
                   value == CombineSplitStatusType.Both;
        }

        /// <summary>
        /// Determines if the given value is neither split, combined, or both.  i.e. None
        /// </summary>
        public static bool IsEither(this CombineSplitStatusType value)
        {
            return value != CombineSplitStatusType.None;
        }

        /// <summary>
        /// Compares two values to determine if they are equal.
        /// </summary>
        public static bool IsEqualTo(this CombineSplitStatusType first, CombineSplitStatusType second)
        {
            if (first == second)
            {
                return true;
            }

            if (first == CombineSplitStatusType.Both && second != CombineSplitStatusType.None)
            {
                return true;
            }

            if (second == CombineSplitStatusType.Both && first != CombineSplitStatusType.None)
            {
                return true;
            }

            return first == second;
        }
    }
}
