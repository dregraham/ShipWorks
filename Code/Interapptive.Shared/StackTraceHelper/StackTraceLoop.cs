namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Represents a loop in a causality chain
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public struct StackTraceLoop
    {
        /// <summary>
        /// Loop constructor
        /// </summary>
        /// <param name="highIndex">Index of the most recent loop element</param>
        /// <param name="lowIndex">Index of the oldest loop element</param>
        /// <param name="count">Number of iterations of the loop</param>
        public StackTraceLoop(int highIndex, int lowIndex, int count)
        {
            if (highIndex < lowIndex)
            {
                throw new AsyncFlowDiagnosticsException($"highIndex is expected to be strictly higher than lowIndex: {highIndex}, {lowIndex}");
            }

            HighIndex = highIndex;
            LowIndex = lowIndex;
            Count = count;
        }

        /// <summary>
        /// Index of the most recent loop element
        /// </summary>
        public int HighIndex { get; }

        /// <summary>
        /// Index of the oldest loop element
        /// </summary>
        public int LowIndex { get; }

        /// <summary>
        /// Number of iterations of the loop
        /// </summary>
        public int Count { get; }
    }
}