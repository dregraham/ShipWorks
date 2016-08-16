using System.Linq;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility functions for working with bytes
    /// </summary>
    public static class ByteUtility
    {
        /// <summary>
        /// Indicates if the given byte arrays are equivalent
        /// </summary>
        public static bool AreEqual(byte[] left, byte[] right)
        {
            if (left == null && right == null)
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Searches for the specified needle in the given haystack
        /// </summary>
        public static bool Contains(byte[] haystack, byte[] needle)
        {
            if (haystack == null || needle == null)
            {
                return false;
            }

            if (haystack.Length == 0)
            {
                return false;
            }

            if (needle.Length > haystack.Length)
            {
                return false;
            }

            for (int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                bool match = true;

                for (int needleIndex = 0; needleIndex < needle.Length; needleIndex++)
                {
                    match = haystack[i + needleIndex] == needle[needleIndex];

                    if (!match)
                    {
                        break;
                    }
                }

                if (match)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Convert a byte array to a hex string
        /// </summary>
        public static string ToHexString(this byte[] value)
        {
            return value == null ?
                string.Empty :
                value.Select(x => x.ToString("X2")).Aggregate(string.Empty, (x, y) => x + y);
        }
    }
}
