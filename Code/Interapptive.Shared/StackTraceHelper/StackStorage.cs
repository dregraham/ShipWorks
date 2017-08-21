using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Stores stack trace segment lists per thread
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public class StackStorage
    {
        private readonly ConcurrentDictionary<long, StackTraceNode> stacksByTask = new ConcurrentDictionary<long, StackTraceNode>();
        private readonly bool storeFileLineColumnInfo;
        private readonly bool justMyCode;

        private AsyncLocal<StackTraceNode> preservedStacks = new AsyncLocal<StackTraceNode>();

        /// <summary>
        /// Constructor for stack trace segment lists storage
        /// </summary>
        /// <param name="storeFileLineColumnInfo">Whether to extract source code information</param>
        /// <param name="justMyCode">Avoid framework boilerplate</param>

        public StackStorage(bool storeFileLineColumnInfo, bool justMyCode)
        {
            this.storeFileLineColumnInfo = storeFileLineColumnInfo;
            this.justMyCode = justMyCode;
        }

        /// <summary>
        /// Prepend current (synchronous) stack trace segment a stack to currently stored list for current thread and store the result in the dictionary by taskId (not clearing up thread-local storage)
        /// </summary>
        /// <param name="taskId">Correlation id to store stack trace list under temporarily, while asynchronous operation is being scheduled onto a possibly different thread</param>
        /// <param name="synchronous">TaskWaitBehavior.Synchronous, if a task is being executed synchronously, or has completed already</param>
        public void StoreStack(long taskId, bool synchronous)
        {
            var toStore = synchronous ? null : preservedStacks.Value.Prepend(GetCurrentSegment());

            stacksByTask.AddOrUpdate(taskId, toStore, (tid, existing) => existing.SelectLongest(toStore));
        }

        /// <summary>
        /// Remove temporarily stored stack trace list from the dictionary and store it back to thread-local storage
        /// </summary>
        /// <param name="taskId">Correlation id</param>
        public void RestoreStack(long taskId)
        {
            StackTraceNode stored = null;
            if (stacksByTask.TryRemove(taskId, out stored))
            {
                preservedStacks.Value = preservedStacks.Value.SelectLongest(stored);
            }
            else
            {
                Trace.WriteLine($"Stack information not stored in transitional repository for task {taskId} and thread {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        /// <summary>
        /// Clear thread-local storage, because a new unrelated work item has started to execute on current thread
        /// </summary>
        public void ResetLocal() => preservedStacks = null;

        /// <summary>
        /// Get formatted causality chain for current thread
        /// </summary>
        /// <returns>Formatted causality chain</returns>
        public string GetAggregateStackString()
        {
            var builder = new StringBuilder();
            foreach (var frame in GetCurrentSegment())
            {
                builder.AppendLine(frame.ToString().PrettifyFrame());
            }

            builder.Append(preservedStacks?.Value?.ToStringEx() ?? string.Empty);
            return builder.ToString();
        }

        /// <summary>
        /// Captures current stack trace
        /// </summary>
        /// <returns>Current stack frames</returns>
        private IEnumerable<StackFrameSlim> GetCurrentSegment() =>
            new StackTraceSlim(storeFileLineColumnInfo).Frames.Where(f => (!f.IsHidden(justMyCode)));
    }
}
