using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Extensions for StackTraceNode. Implemented as extensions to allow executing on a null list head
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public static class StackTraceNodeExtensions
    {
        private const string loopHeaderFormat1 = "\u21ba {0} times {{";
        private const string loopFooter = "}";
        private const string indentStop = "    ";
        private static readonly Regex lambdaRegex = new Regex(@"^(?<container>.*?)\+<>c__DisplayClass\d+\.<(?<method>[^>]+)>b__[0-9A-Fa-f]+\(\)");
        private static readonly Regex stateMachineRegex = new Regex(@"^(?<container>.*?)\+(?:<>c__DisplayClass\d+\.)?<(?<method>[^>]+)>d__[0-9A-Fa-f]+\.MoveNext\(\)");
        private static readonly Regex methodNameRegex = new Regex(@"^(?<fullName>[^() ]{10,})\((?<args>[^)]*)\)");

        /// <summary>
        /// Returns the longest stack trace list
        /// </summary>
        /// <param name="a">First stack trace list</param>
        /// <param name="b">Second stack trace list</param>
        /// <returns>The longest stack trace list of the two</returns>
        public static StackTraceNode SelectLongest(this StackTraceNode a, StackTraceNode b) =>
            a.TotalCountWithLoops() > b.TotalCountWithLoops() ? a : b;

        /// <summary>
        /// Adds new element to the linked list and returns the new head
        /// </summary>
        /// <param name="node">Old list head</param>
        /// <param name="topFrames">Stack frames to prepend. If empty, old list head will be returned</param>
        /// <returns>Head of the list with prepended frames</returns>
        public static StackTraceNode Prepend(this StackTraceNode node, IEnumerable<StackFrameSlim> topFrames)
        {
            if (!topFrames.Any())
            {
                return node;
            }

            var frames = topFrames.Reverse().Concat(Enumerable.Repeat(StackFrameSlim.AsyncBoundary, 1)).ToArray();

            var lastHash = node == null || node.Value.Count == 0 ? 0 : node.Value.Hashes[node.Value.Count - 1];
            var hashes = new long[frames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                hashes[i] = PolyHash.Append(lastHash, frames[i].LongHash);

                lastHash = hashes[i];
            }

            var loops = node == null ? new StackTraceLoop[0] : node.Value.Loops.ToArray();

            var newAgg = new StackTraceSegment(frames, hashes, loops, node.TotalCount() + frames.Length, node.TotalCountWithLoops() + frames.Length);
            node = new StackTraceNode(newAgg, node);

            node = node.CollapseLoops();

            return node;
        }

        /// <summary>
        /// Walks the list and calculates sum of element values
        /// </summary>
        /// <param name="node">List head</param>
        /// <param name="getElement">Value getter</param>
        /// <returns>Sum of values</returns>
        private static int Sum(this StackTraceNode node, Func<StackTraceSegment, int> getElement)
        {
            int sum = 0;
            while (node != null)
            {
                sum += getElement(node.Value);
                node = node.Next;
            }

            return sum;
        }

        /// <summary>
        /// Count of elements in the list as if loops were expanded
        /// </summary>
        /// <param name="node">List head</param>
        /// <returns>Total count of elements as if loops were expanded</returns>
        public static int TotalCountWithLoops(this StackTraceNode node) =>
            node == null ? 0 : node.Value.TotalCountWithLoops;

        /// <summary>
        /// Count of elements with loops collapsed (as if only one iteration of each counts)
        /// </summary>
        /// <param name="node">List head</param>
        /// <returns>Count of elements with loops collapsed</returns>
        public static int TotalCount(this StackTraceNode node) =>
            node == null ? 0 : node.Value.TotalCount;

        /// <summary>
        /// Number of loops in the list
        /// </summary>
        /// <param name="node">List head</param>
        /// <returns>Number of loops in the list</returns>
        public static int LoopsCount(this StackTraceNode node) =>
            node == null ? 0 : node.Value.Loops.Length;

        /// <summary>
        /// Get stack frame or its hash by index, using continuous numbering over the entire list
        /// </summary>
        /// <typeparam name="T">Stack frame or hash type</typeparam>
        /// <param name="node">List head</param>
        /// <param name="index">Index of the frame of interest, using continuous numbering over the entire list</param>
        /// <param name="arrayGetter">Function returning stack frame or hash array from stack trace segment</param>
        /// <returns>Stack frame or its hash</returns>
        [SuppressMessage("ShipWorks", "SW0002",
            Justification = "The parameter name is only used for exception messages")]
        private static T GetFragment<T>(this StackTraceNode node, int index, Func<StackTraceSegment, T[]> arrayGetter)
        {
            var curCount = node.TotalCount();
            if (index >= curCount || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            while (node != null)
            {
                if (index < curCount && index >= curCount - node.Value.Count)
                {
                    return arrayGetter(node.Value)[index - curCount + node.Value.Count];
                }

                curCount -= node.Value.Count;
                node = node.Next;
            }

            throw new InvalidOperationException("Could not get the frame by index in a linked list");
        }

        /// <summary>
        /// Get the stack frame by index, using continuous numbering over the entire list
        /// </summary>
        /// <param name="node">List head</param>
        /// <param name="index">Index of the frame of interest, using continuous numbering over the entire list</param>
        /// <returns>Stack frame</returns>
        public static StackFrameSlim GetFrame(this StackTraceNode node, int index) =>
            node.GetFragment(index, t => t.Frames);

        /// <summary>
        /// Gets hash of a frame by index (continuous numbering over the entire list)
        /// </summary>
        /// <param name="node">List head</param>
        /// <param name="index">Index (continuous numbering over the entire list)</param>
        /// <returns>Hash of a stack frame in question</returns>
        public static long GetHash(this StackTraceNode node, int index) =>
            index < 0 ? 0 : node.GetFragment(index, t => t.Hashes);

        /// <summary>
        /// Gets loop by index
        /// </summary>
        /// <param name="node">List head</param>
        /// <param name="index">Index</param>
        /// <returns>Loop</returns>
        public static StackTraceLoop GetLoop(this StackTraceNode node, int index) =>
            node.Value.Loops[index];

        /// <summary>
        /// Makes 'c__DisplayClass' frame name more readable, by replacing it with the name of corresponding state machine (which is a method name)
        /// </summary>
        /// <param name="frameStr">A frame name string possibly with 'c__DisplayClass' content</param>
        /// <param name="args">Method arguments (if null replaced by '...' symbol)</param>
        /// <returns>A frame string with 'c__DisplayClass' replaced with actual state machine name</returns>
        public static string PrettifyFrame(this string frameStr, string args = null)
        {
            var replacement = "${container}.${method}(" + args ?? ((char) 0x2026).ToString() + ")";
            frameStr = stateMachineRegex.Replace(frameStr, replacement);
            frameStr = lambdaRegex.Replace(frameStr, replacement);
            return frameStr;
        }

        /// <summary>
        /// Formats the list nicely
        /// </summary>
        /// <param name="node">List head</param>
        /// <returns>Nicely formatted concatenated ca
        public static string ToStringEx(this StackTraceNode node) => node.ToStringEx(string.Empty);

        /// <summary>
        /// Formats the list nicely
        /// </summary>
        /// <param name="node">List head</param>
        /// <param name="indent">Indentation</param>
        /// <returns>Nicely formatted concatenated causality chain</returns>
        public static string ToStringEx(this StackTraceNode node, string indent)
        {
            if (node == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            int count = node.TotalCount();
            var printNext = true;
            for (int i = count - 1; i >= 0; i--)
            {
                for (int j = 0; j < node.LoopsCount(); j++)
                {
                    if (node.GetLoop(j).HighIndex == i)
                    {
                        builder.AppendFormat(indent + loopHeaderFormat1 + Environment.NewLine, node.GetLoop(j).Count);
                        indent += indentStop;
                    }
                }

                if (printNext)
                {
                    printNext = true;
                    var nextMatch = i > 0 ? methodNameRegex.Match(node.GetFrame(i - 1).ToString()) : null;
                    var curStr = node.GetFrame(i).ToString();

                    curStr = PrettifyFrame(curStr, nextMatch?.Success == true && (printNext = !curStr.StartsWith(nextMatch.Groups["fullName"].Value, StringComparison.Ordinal)) ? nextMatch.Groups["args"].Value : null);
                    printNext = nextMatch?.Success == true ? !curStr.StartsWith(nextMatch.Value, StringComparison.Ordinal) : true;
                    builder.AppendLine(indent + curStr);
                }
                else
                {
                    printNext = true;
                }

                for (int j = 0; j < node.LoopsCount(); j++)
                {
                    if (node.GetLoop(j).LowIndex == i)
                    {
                        indent = indent.Substring(0, indent.Length - indentStop.Length);
                        builder.AppendLine(indent + loopFooter);
                    }
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Finds and collapses loops starting from the list head by finding a longest list prefix that is also a prefix of a sublist with this prefix removed.
        /// </summary>
        /// <param name="node">List head</param>
        /// <returns>new list head if loops were collapsed, unchanged list head otherwise</returns>
        private static StackTraceNode CollapseLoops(this StackTraceNode node)
        {
            while (true)
            {
                var slowLoopBraces = 0;
                var fastLoopBraces = 0;
                var count = node.TotalCount();
                var loopHigh = count;
                var loopLow = count;
                var slow = count - 1;
                var fast = count - 2;

                while (slow >= 0 && fast >= 0)
                {
                    for (int i = 0; i < node.LoopsCount(); i++)
                    {
                        if (node.GetLoop(i).HighIndex == slow)
                        {
                            slowLoopBraces += 1;
                        }
                        if (node.GetLoop(i).LowIndex == slow)
                        {
                            slowLoopBraces -= 1;
                        }
                        if (node.GetLoop(i).HighIndex == fast + 1)
                        {
                            fastLoopBraces += 1;
                        }
                        if (node.GetLoop(i).LowIndex == fast + 1)
                        {
                            fastLoopBraces -= 1;
                        }
                        if (node.GetLoop(i).HighIndex == fast)
                        {
                            fastLoopBraces += 1;
                        }
                        if (node.GetLoop(i).LowIndex == fast)
                        {
                            fastLoopBraces -= 1;
                        }
                    }

                    if (PolyHash.Subtract(node.GetHash(count - 1), node.GetHash(slow - 1), count - slow) == PolyHash.Subtract(node.GetHash(slow - 1), node.GetHash(fast - 1), slow - fast))
                    {
                        if (slowLoopBraces == 0 && fastLoopBraces == 0)
                        {
                            loopHigh = slow - 1;
                            loopLow = fast;
                        }
                    }

                    slow -= 1;
                    fast -= 2;
                }

                if (loopHigh >= count)
                {
                    break;
                }

                var loopLength = loopHigh - loopLow + 1;
                StackTraceNode curNode;
                curNode = node;

                while (curNode.Next != null && loopHigh < curNode.TotalCount() - curNode.Value.Count)
                {
                    curNode = curNode.Next;
                }

                var boundary = curNode.TotalCount() - curNode.Value.Count;

                var frames = curNode.Value.Frames.Take(loopHigh - boundary + 1).ToArray();
                var hashes = curNode.Value.Hashes.Take(loopHigh - boundary + 1).ToArray();
                var loopsList = node.Value.Loops.ToList();

                var foundLoop = false;
                for (int i = 0; i < loopsList.Count; i++)
                {
                    if (loopsList[i].HighIndex == loopHigh && loopsList[i].LowIndex == loopLow)
                    {
                        loopsList[i] = new StackTraceLoop(loopHigh, loopLow, loopsList[i].Count + 1);
                        foundLoop = true;
                        break;
                    }
                }

                if (!foundLoop)
                {
                    loopsList.Add(new StackTraceLoop(loopHigh, loopLow, 2));
                }

                for (int i = 0; i < loopsList.Count; i++)
                {
                    if (loopsList[i].HighIndex > loopHigh || loopsList[i].LowIndex > loopHigh)
                    {
                        if (loopsList[i].HighIndex <= loopHigh || loopsList[i].LowIndex <= loopHigh)
                        {
                            throw new AsyncFlowDiagnosticsException("Loops cannot be broken apart by other loops");
                        }

                        var newHigh = loopsList[i].HighIndex - loopLength;
                        var newLow = loopsList[i].LowIndex - loopLength;
                        var existingLoop = loopsList.SingleOrDefault(x => x.HighIndex == newHigh && x.LowIndex == newLow);
                        loopsList[i] = new StackTraceLoop(newHigh, newLow, Math.Max(loopsList[i].Count, existingLoop.Count));
                        loopsList.Remove(existingLoop);
                    }
                }

                var newSegment = new StackTraceSegment(frames, hashes, loopsList.ToArray(), curNode.Next.TotalCount() + frames.Length, node.TotalCountWithLoops());
                node = new StackTraceNode(newSegment, curNode.Next);
            }

            return node;
        }
    }
}