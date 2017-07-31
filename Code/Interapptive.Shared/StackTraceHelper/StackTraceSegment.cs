namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// Stack trace segment
    /// </summary>
    public class StackTraceSegment
    {
        // Pertaining to segments

        /// <summary>
        /// Frames belonging to this segment
        /// </summary>
        public StackFrameSlim[] Frames { get; }

        /// <summary>
        /// Hashes belonging to this segment
        /// </summary>
        public long[] Hashes { get; }

        // Pertaining to the whole sublist starting at this element

        /// <summary>
        /// Loops belonging to the whole list
        /// </summary>
        public StackTraceLoop[] Loops { get; }

        /// <summary>
        /// Total element count with loops collapsed in the entire list
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Total element count in the entire list with loops expanded
        /// </summary>
        public int TotalCountWithLoops { get; }

        /// <summary>
        /// Constructor of a stack trace segment
        /// </summary>
        /// <param name="frames">Frames for the segment</param>
        /// <param name="hashesUpTo">Hashes for the segment</param>
        /// <param name="loops">Loops for the entire list</param>
        /// <param name="totalCount">Count with loops collapsed for the entire list</param>
        /// <param name="countWithLoops">Count with loops expanded for the entire list</param>
        public StackTraceSegment(StackFrameSlim[] frames, long[] hashesUpTo, StackTraceLoop[] loops, int totalCount, int countWithLoops)
        {
            Frames = frames;
            Hashes = hashesUpTo;
            Loops = loops;

            TotalCount = totalCount;

            TotalCountWithLoops = countWithLoops;
        }

        /// <summary>
        /// Frames count in this segment itself
        /// </summary>
        public int Count => Frames.Length;
    }
}