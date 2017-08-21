namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Helper class that calculates polynomial hash, which allows for O(1) comparison of arbitrary sub-lists in a causality chain.
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public static class PolyHash
    {
        private const long factor = 1542691;

        /// <summary>
        /// Adds one element to the hash
        /// </summary>
        /// <param name="aggregate">Current hash value</param>
        /// <param name="next">New element value</param>
        /// <returns>New hash value</returns>
        public static long Append(long aggregate, long nextValue) => aggregate * factor + nextValue;

        /// <summary>
        /// Calculates the hash for a list interval, given the hashes of super-list and sub-list sharing an end, and difference between their cardinality.
        /// E.g. given a super-list of elements N down to 0 and a sub-list M down to 0, where N >= M, and cardinality difference (N - M), returns a hash for interval N down to M+1.
        /// </summary>
        /// <param name="higher">Hash of a super-list</param>
        /// <param name="lower">Hash of a sub-list</param>
        /// <param name="steps">Cardinality difference</param>
        /// <returns>Hash of the interval</returns>
        public static long Subtract(long higher, long lower, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                lower = lower * factor;
            }

            return higher - lower;
        }
    }
}