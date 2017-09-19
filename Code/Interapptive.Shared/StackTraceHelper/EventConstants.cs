using System;
using System.Diagnostics.Tracing;

namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// A set of constants from TplEtwProvider and FrameworkEventSource
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public static class EventConstants
    {
        /// <summary>
        /// An excerpt from TplEtwProvider
        /// </summary>
        public static class Tpl
        {
            public static readonly Guid GUID = new Guid("2e5dba47-a3d2-4d16-8ee0-6671ffdcd7b5");

            public const int PARALLELLOOPBEGIN_ID = 1;
            public const int PARALLELLOOPEND_ID = 2;

            public const int PARALLELINVOKEBEGIN_ID = 3;
            public const int PARALLELINVOKEEND_ID = 4;

            public const int PARALLELFORK_ID = 5;
            public const int PARALLELJOIN_ID = 6;

            public const int TASKSCHEDULED_ID = 7;

            public const int TASKSTARTED_ID = 8;
            public const int TASKCOMPLETED_ID = 9;

            public const int TASKWAITBEGIN_ID = 10;
            public const int TASKWAITEND_ID = 11;

            /// <summary>
            /// Type of async action
            /// </summary>
            public enum ForkJoinOperationType
            {
                ParallelInvoke = 1,
                ParallelFor = 2,
                ParallelForEach = 3
            }

            /// <summary>
            /// Task event types
            /// </summary>
            public static class Tasks
            {
                public const EventTask Loop1 = (EventTask) 1;
                public const EventTask Invoke = (EventTask) 2;
                public const EventTask ForkJoin = (EventTask) 5;
                public const EventTask TaskExecute = (EventTask) 3;
                public const EventTask TaskWait = (EventTask) 4;
            }

            /// <summary>
            /// Wait behavior
            /// </summary>
            public enum TaskWaitBehavior
            {
                Synchronous = 1,
                Asynchronous = 2
            }
        }

        /// <summary>
        /// An excerpt from FrameworkEventSource
        /// </summary>
        public static class Framework
        {
            public static readonly Guid GUID = new Guid("8E9F5090-2D75-4d03-8A81-E5AFBF85DAF1");

            public const int THREADPOOLDEQUEUEWORK_ID = 31;
            public const int THREADPOOLENQUEUEWORK_ID = 30;

            /// <summary>
            /// Keywords to look for
            /// </summary>
            public static class Keywords
            {
                public const EventKeywords Loader = (EventKeywords) 1L;
                public const EventKeywords ThreadPool = (EventKeywords) 2L;
            }
        }
    }
}