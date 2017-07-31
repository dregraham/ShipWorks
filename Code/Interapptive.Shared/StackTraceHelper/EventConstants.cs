using System;
using System.Diagnostics.Tracing;

namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// A set of constants from TplEtwProvider and FrameworkEventSource
    /// </summary>
    public static class EventConstants
    {

        /// <summary>
        /// An excerpt from TplEtwProvider
        /// </summary>
        public abstract class Tpl
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

            //' Tasks.Loop == 1
            //Public MustOverride Sub ParallelLoopBegin(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal ForkJoinContextID As Integer, ByVal OperationType As ForkJoinOperationType, ByVal InclusiveFrom As Long, ByVal ExclusiveTo As Long)

            //Public MustOverride Sub ParallelLoopEnd(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal ForkJoinContextID As Integer, ByVal TotalIterations As Long)

            //' Tasks.Invoke == 2
            //Public MustOverride Sub ParallelInvokeBegin(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal ForkJoinContextID As Integer, ByVal OperationType As ForkJoinOperationType, ByVal ActionCount As Integer)

            //Public MustOverride Sub ParallelInvokeEnd(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal ForkJoinContextID As Integer)

            //' Tasks.ForkJoin == 5
            //Public MustOverride Sub ParallelFork(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal ForkJoinContextID As Integer)

            //Public MustOverride Sub ParallelJoin(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal ForkJoinContextID As Integer)

            //' No task
            //Public MustOverride Sub TaskScheduled(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal TaskID As Integer, ByVal CreatingTaskID As Integer, ByVal TaskCreationOptions As Integer)

            //' Tasks.TaskExecute == 3
            //Public MustOverride Sub TaskStarted(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal TaskID As Integer)

            //Public MustOverride Sub TaskCompleted(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal TaskID As Integer, ByVal IsExceptional As Boolean)

            //' Tasks.TaskWait == 4
            //Public MustOverride Sub TaskWaitBegin(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal TaskID As Integer, ByVal Behavior As TaskWaitBehavior)

            //Public MustOverride Sub TaskWaitEnd(ByVal OriginatingTaskSchedulerID As Integer, ByVal OriginatingTaskID As Integer, ByVal TaskID As Integer)

            public enum ForkJoinOperationType
            {
                ParallelInvoke = 1,
                ParallelFor = 2,
                ParallelForEach = 3
            }

            public static class Tasks
            {
                public const EventTask Loop1 = (EventTask) 1;
                public const EventTask Invoke = (EventTask) 2;
                public const EventTask ForkJoin = (EventTask) 5;
                public const EventTask TaskExecute = (EventTask) 3;
                public const EventTask TaskWait = (EventTask) 4;
            }

            public enum TaskWaitBehavior
            {
                Synchronous = 1,
                Asynchronous = 2
            }
        }

        /// <summary>
        /// An excerpt frm FrameworkEventSource
        /// </summary>
        public sealed class Framework
        {
            public static readonly Guid GUID = new Guid("8E9F5090-2D75-4d03-8A81-E5AFBF85DAF1");

            public const int THREADPOOLDEQUEUEWORK_ID = 31;
            public const int THREADPOOLENQUEUEWORK_ID = 30;

            //' Keywords.ThreadPool == 2L
            //Public MustOverride Sub ThreadPoolDequeueWork(ByVal workID As Long)

            //Public MustOverride Sub ThreadPoolEnqueueWork(ByVal workID As Long)

            public static class Keywords
            {
                public const EventKeywords Loader = (EventKeywords) 1L;
                public const EventKeywords ThreadPool = (EventKeywords) 2L;
            }
        }
    }
}