using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// TplEtwProvider and FrameworkEventSource listener base class
    /// </summary>
    public abstract class TplFrameworkListenerBase : EventListener
    {
        protected static void EnsureInitialized<T>(Func<T> constructor, ref T storage) where T : TplFrameworkListenerBase
        {
            // Sometimes EventListener constructor may throw a "Collection was modified" exception,
            // when event sources are being created simultaneously with listener instantiation.
            // To work around that subscriptions are delayed till after constructor successfully executes.
            LazyInitializer.EnsureInitialized(ref storage, () =>
            {
                T listener;
                while (true)
                {
                    try
                    {
                        listener = constructor();
                        break;
                    }
                    catch (InvalidOperationException)
                    {

                    }
                }

                listener.EnableSubscriptions();
                listener.Initialized.Wait();
                return listener;
            });
        }

        private List<EventSource> _delayedSubscriptions;
        private TaskCompletionSource<object> _initializedTcs;

        private List<EventSource> DelayedSubscriptions
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref _delayedSubscriptions);
                return _delayedSubscriptions;
            }
        }

        private TaskCompletionSource<object> InitializedTcs
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref _initializedTcs);
                return _initializedTcs;
            }
        }

        /// <summary>
        /// Completes when both event sources are subscribed to
        /// </summary>
        public Task Initialized => InitializedTcs.Task;

        /// <summary>
        /// Adds event source to the list of subscriptions
        /// and subscribes to them if event listener initialization has already completed (subscriptionsEnabled == true)
        /// </summary>
        /// <param name="eventSource">Event source just created or existing one
        /// passed in to a newly created event listener by eventing infrastructure</param>
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            lock (DelayedSubscriptions)
            {
                if (eventSource.Guid == EventConstants.Tpl.GUID || eventSource.Guid == EventConstants.Framework.GUID)
                {
                    DelayedSubscriptions.Add(eventSource);
                }
            }

            if (subscriptionsEnabled)
            {
                ProcessDelayedSubscriptions();
            }
        }

        private bool subscriptionsEnabled = false;

        /// <summary>
        /// Indicate that event listener constructor has completed successfully and event sources stored
        /// in delayed subscription list can now be subscribed to
        /// </summary>
        public void EnableSubscriptions()
        {
            subscriptionsEnabled = true;
            ProcessDelayedSubscriptions();
        }

        /// <summary>
        /// Actually subscribes event listener to sources
        /// </summary>
        private void ProcessDelayedSubscriptions()
        {
            List<EventSource> toSubscribe = new List<EventSource>();
            lock (DelayedSubscriptions)
            {
                toSubscribe.AddRange(DelayedSubscriptions);
                DelayedSubscriptions.Clear();
            }

            foreach (var eventSource in toSubscribe)
            {
                if (eventSource.Guid == EventConstants.Tpl.GUID)
                {
                    EnableEvents(eventSource, EventLevel.LogAlways);
                    TryCompleteInitialization();
                }
                else if (eventSource.Guid == EventConstants.Framework.GUID)
                {
                    EnableEvents(eventSource, EventLevel.LogAlways, EventConstants.Framework.Keywords.ThreadPool);
                    TryCompleteInitialization();
                }
            }
        }

        /// <summary>
        /// Set Initialized task as complete if subscribed to both sources
        /// </summary>
        private int initializationCount;

        private void TryCompleteInitialization()
        {
            if (Interlocked.Increment(ref initializationCount) == 2)
            {
                InitializedTcs.SetResult(null);
            }
        }

        /// <summary>
        /// Handling events
        /// </summary>
        /// <param name="eventData">Event data</param>
        protected sealed override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.EventSource.Guid == EventConstants.Tpl.GUID)
            {
                switch (eventData.EventId)
                {
                    case EventConstants.Tpl.TASKSCHEDULED_ID:
                        TaskScheduled(GetTaskId(eventData));
                        break;
                    case EventConstants.Tpl.TASKWAITBEGIN_ID:
                        TaskWaitBegin(GetTaskId(eventData), GetWaitBehavior(eventData));
                        break;
                    case EventConstants.Tpl.TASKWAITEND_ID:
                        TaskWaitEnd(GetTaskId(eventData));
                        break;
                }
            }
            else if (eventData.EventSource.Guid == EventConstants.Framework.GUID)
            {
                switch (eventData.EventId)
                {
                    case EventConstants.Framework.THREADPOOLDEQUEUEWORK_ID:
                        ResetLocal();
                        break;
                }
            }
        }

        private int GetTaskId(EventWrittenEventArgs eventData) => (int) eventData.Payload[2];

        private EventConstants.Tpl.TaskWaitBehavior GetWaitBehavior(EventWrittenEventArgs eventData) =>
            (EventConstants.Tpl.TaskWaitBehavior) (int) eventData.Payload[3];

        protected virtual void TaskScheduled(int taskId)
        {

        }

        protected virtual void TaskWaitBegin(int taskId, EventConstants.Tpl.TaskWaitBehavior behavior)
        {

        }

        protected virtual void TaskWaitEnd(int taskId)
        {

        }

        /// <summary>
        /// Indicates that new unrelated work item starts executing on current thread
        /// </summary>
        protected virtual void ResetLocal()
        {

        }
    }
}
