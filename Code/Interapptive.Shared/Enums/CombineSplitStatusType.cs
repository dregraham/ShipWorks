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

    public static class CombineSplitStatusTypeExtensions
    {
        public static CombineSplitStatusType AsCombined(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.None ? 
                CombineSplitStatusType.Combined : 
                CombineSplitStatusType.Both;
        }

        public static bool IsCombined(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.Combined ||
                   value == CombineSplitStatusType.Both;
        }

        public static CombineSplitStatusType AsSplit(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.None ?
                CombineSplitStatusType.Split :
                CombineSplitStatusType.Both;
        }

        public static bool IsSplit(this CombineSplitStatusType value)
        {
            return value == CombineSplitStatusType.Split ||
                   value == CombineSplitStatusType.Both;
        }

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
