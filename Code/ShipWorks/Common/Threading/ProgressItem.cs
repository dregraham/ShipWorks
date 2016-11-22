using System;
using System.Diagnostics;
using Interapptive.Shared.Threading;
using log4net;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// An individual action that will have its progress tracked
    /// </summary>
    public class ProgressItem : IProgressReporter
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ProgressItem));

        // Name of the action
        string name = string.Empty;

        // Currently displayed detail of the action
        string detail = string.Empty;

        // From 0 to 100
        int percentComplete;

        // Status of this item
        ProgressItemStatus status = ProgressItemStatus.Pending;

        // If set to failure, this allows an exception message to be displayed
        Exception error = null;

        // Gets or sets an object that contains data to associate with the item.
        object tag = null;

        // Indicates if the action can be cancelled
        bool canCancel = true;

        // Indicates if the user has requested cancelation
        bool cancelRequested = false;
        object cancelRequestedLock = new object();

        // Raised when an of the properties of the item changed
        public event EventHandler Changed;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressItem(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
        }

        /// <summary>
        /// Name of the progress item
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The detailed progress of the given item
        /// </summary>
        public string Detail
        {
            get
            {
                return detail;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (detail != value)
                {
                    detail = value;

                    RaiseChanged();
                }
            }
        }

        /// <summary>
        /// The percent complete of the progress, from 0 to 100
        /// </summary>
        public int PercentComplete
        {
            get
            {
                return percentComplete;
            }
            set
            {
                // If its out of range we'll care about it while debugging, but we'll just clip in the wild.
                if (value < 0 || value > 100)
                {
                    log.ErrorFormat("PercentComplete out of range: {0}", value);
                    Debug.Fail("PercentComplete out of range.");

                    value = Math.Max(0, Math.Min(value, 100));
                }

                if (percentComplete != value)
                {
                    percentComplete = value;

                    RaiseChanged();
                }
            }
        }

        /// <summary>
        /// The current status of the progress item
        /// </summary>
        public ProgressItemStatus Status
        {
            get
            {
                return status;
            }
            private set
            {
                bool changed = false;

                if (status != value)
                {
                    status = value;
                    changed = true;
                }

                if (changed)
                {
                    RaiseChanged();
                }
            }
        }

        /// <summary>
        /// If Status is Failure, then this error will display in the progress grid.
        /// </summary>
        public Exception Error
        {
            get
            {
                return error;
            }
        }

        /// <summary>
        /// Gets or sets an object that contains data to associate with the item.
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Indicates if this operation can be canceled
        /// </summary>
        public bool CanCancel
        {
            get
            {
                return canCancel;
            }
            set
            {
                if (canCancel != value)
                {
                    canCancel = value;

                    if (canCancel && cancelRequested && status == ProgressItemStatus.Pending)
                    {
                        // Purposely dont use property, which would raise the change event, since
                        // we do it right after this anyway.
                        status = ProgressItemStatus.Canceled;
                    }

                    RaiseChanged();
                }
            }
        }

        /// <summary>
        /// Indicates if the user has requested cancelation
        /// </summary>
        public bool IsCancelRequested
        {
            get
            {
                lock (cancelRequestedLock)
                {
                    return cancelRequested;
                }
            }
        }

        /// <summary>
        /// Called to indicate the progress item is now running
        /// </summary>
        public void Starting()
        {
            // Its possible to call Starting on a Canceled item, since it could be canceled at any time from
            // another thread there is really no way to know for sure if its canceld before calling this function.
            // It would be a race condition.
            if (status != ProgressItemStatus.Pending && status != ProgressItemStatus.Canceled)
            {
                throw new InvalidOperationException("Cannot start a progress item that is not pending.");
            }

            Status = ProgressItemStatus.Running;
        }

        /// <summary>
        /// Used to indicate the task finished successfully, or due to cancel.
        /// </summary>
        public void Completed()
        {
            if (status != ProgressItemStatus.Running)
            {
                throw new InvalidOperationException("Cannot mark a non-running progress item as completed.");
            }

            if (cancelRequested && PercentComplete < 100)
            {
                Status = ProgressItemStatus.Canceled;
            }
            else
            {
                Status = ProgressItemStatus.Success;
            }
        }

        /// <summary>
        /// Called when the task finishes due to an error
        /// </summary>
        public void Failed(Exception ex)
        {
            if (status != ProgressItemStatus.Running)
            {
                throw new InvalidOperationException("Cannot mark a non-running progress item as failed.");
            }

            error = ex;
            Status = ProgressItemStatus.Failure;
        }

        /// <summary>
        /// Called when the item has not yet been run, but won't be run due to a previous error.
        /// </summary>
        public void Terminate()
        {
            if (status != ProgressItemStatus.Pending)
            {
                throw new InvalidOperationException("Cannot mark a non-pending progress item as terminated.");
            }

            Status = ProgressItemStatus.Terminated;
        }

        /// <summary>
        /// Request cancelation of the ProgressItem
        /// </summary>
        public void Cancel()
        {
            lock (cancelRequestedLock)
            {
                cancelRequested = true;
            }

            if (status == ProgressItemStatus.Pending && CanCancel)
            {
                Status = ProgressItemStatus.Canceled;
            }
            else
            {
                // Even though our status hasnt changed, our effective state has, and listening UI's would be interested.
                RaiseChanged();
            }
        }

        /// <summary>
        /// Raised the Changed event
        /// </summary>
        private void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

    }
}
