namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
	/// Singly linked list node
	/// </summary>
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
