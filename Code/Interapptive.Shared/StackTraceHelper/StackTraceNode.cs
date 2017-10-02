namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Singly linked list node
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public class StackTraceNode
    {
        /// <summary>
        /// Node constructor
        /// </summary>
        /// <param name="value">Stack trace segment</param>
        /// <param name="next">Next list node</param>
        public StackTraceNode(StackTraceSegment value, StackTraceNode next)
        {
            Value = value;
            Next = next;
        }

        /// <summary>
        /// Stack trace segment
        /// </summary>
        public StackTraceSegment Value { get; }

        /// <summary>
        /// Next list node
        /// </summary>
        public StackTraceNode Next { get; }
    }
}
