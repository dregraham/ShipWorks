using System;
using System.Collections.Generic;
using System.Threading;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Platforms.BigCommerce;
using log4net;

namespace ShipWorks.Stores.Communication.Throttling
{
    /// <summary>
    /// Abstract class that handles the throttling and submitting of requests to a store api per their defined limits.
    /// </summary>
    public abstract class RequestThrottle<TWaitCancelException> : IDisposable where TWaitCancelException: Exception, new()
    {
        private readonly ILog logger;

        // for cancelling waits
        readonly ManualResetEvent cancelledEvent = new ManualResetEvent(false);
        readonly object cancelEventDisposeSyncObj = new object();
        readonly string storeName;

        /// <summary>
        /// Constructor
        /// </summary>
        protected RequestThrottle(string storeName, ILog log)
        {
            logger = log;
            this.storeName = storeName;
        }

        /// <summary>
        /// ExecuteRequest will make a throttled call to webClientMethod and return the result.
        /// If the throttler detects that the number of calls has been reached, the throttler will wait the 
        /// desiered amount of time before making the call to webClientMethod again.  It will continue to make
        /// the calls until a successful call is made, the user clicks cancel, or a cancel exception is thrown.
        /// </summary>
        /// <typeparam name="TWebClientRequestType">Type of the request</typeparam>
        /// <typeparam name="TWebClientReturnType">The type of response that should be returned</typeparam>
        /// <param name="requestThrottleParams">Throttling request parameters</param>
        /// <param name="webClientMethod">Method that will be executed to </param>
        /// <returns>Restul from the api call</returns>
        public virtual TWebClientReturnType ExecuteRequest<TWebClientRequestType, TWebClientReturnType>(RequestThrottleParameters requestThrottleParams, Func<TWebClientRequestType, TWebClientReturnType> webClientMethod)
        {
            if (requestThrottleParams == null)
            {
                throw new ArgumentNullException("requestThrottleParams");
            }
            if (webClientMethod == null)
            {
                throw new ArgumentNullException("webClientMethod");
            }

            while (true)
            {
                try
                {
                    TWebClientReturnType response = webClientMethod((TWebClientRequestType) requestThrottleParams.Request);

                    return response;
                }
                catch (RequestThrottledException)
                {
                    HandleRequestThrottleException(requestThrottleParams);
                }
            }
        }

        /// <summary>
        /// ExecuteRequest will make a throttled call to webClientMethod and return the result.
        /// If the throttler detects that the number of calls has been reached, the throttler will wait the 
        /// desiered amount of time before making the call to webClientMethod again.  It will continue to make
        /// the calls until a successful call is made, the user clicks cancel, or a cancel exception is thrown.
        /// </summary>
        /// <param name="requestThrottleParams">Throttling request parameters</param>
        /// <param name="webClientMethod">Method that will be executed to </param>
        /// <returns>Restul from the api call</returns>
        public virtual void ExecuteRequest(RequestThrottleParameters requestThrottleParams, Action webClientMethod)
        {
            if (requestThrottleParams == null)
            {
                throw new ArgumentNullException("requestThrottleParams");
            }
            if (webClientMethod == null)
            {
                throw new ArgumentNullException("webClientMethod");
            }

            while (true)
            {
                try
                {
                    webClientMethod();

                    return;
                }
                catch (RequestThrottledException)
                {
                    HandleRequestThrottleException(requestThrottleParams);
                }
            }
        }

        private void HandleRequestThrottleException(RequestThrottleParameters requestThrottleParams)
        {
            TimeSpan timeToSleep = requestThrottleParams.RetryInterval;

            string oldDetail = null;

            // If it's that short, don't notify we are waiting
            if (requestThrottleParams.Progress != null && timeToSleep > TimeSpan.FromSeconds(2))
            {
                oldDetail = requestThrottleParams.Progress.Detail;
                requestThrottleParams.Progress.Detail = oldDetail + string.Format("\r\n{0} is limiting download speeds...\r\n", storeName);

                if (timeToSleep > TimeSpan.FromMinutes(1))
                {
                    requestThrottleParams.Progress.Detail += string.Format("(ShipWorks can try again at {0})", (DateTime.Now + timeToSleep).ToShortTimeString());
                }
                else if (timeToSleep > TimeSpan.FromSeconds(2))
                {
                    // Round up to the nearest 10 seconds
                    requestThrottleParams.Progress.Detail += string.Format("(ShipWorks can try again in {0} seconds)", (int) timeToSleep.TotalSeconds);
                }
            }

            logger.InfoFormat("{0} is currently over the API rate limit, pausing for {1}.", storeName, timeToSleep);

            // If we don't have a progress reporter, just do a simple thread sleep
            if (requestThrottleParams.Progress == null)
            {
                Thread.Sleep(timeToSleep);
            }
            else
            {
                // we have a progress reporter, so do a more complex but cancellable sleep
                CancellableWait(requestThrottleParams.Progress, timeToSleep);
            }

            if (requestThrottleParams.Progress != null && oldDetail != null)
            {
                requestThrottleParams.Progress.Detail = oldDetail;
            }
        }

        /// <summary>
        /// Wait for timeToSleep in a manner cancellable by the IProgressReporter
        /// </summary>
        protected void CancellableWait(IProgressReporter progress, TimeSpan timeToSleep)
        {
            // syncrhronize waiting
            cancelledEvent.Reset();

            // on a background thread, wait for the ProgressReporter to indicate a cancel is requested
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncWaitForCancel), new Dictionary<string, object> { { "progress", progress }, { "cancelEvent", cancelledEvent } });

            // wait for the timeToSleep
            if (!cancelledEvent.WaitOne(timeToSleep))
            {
                // we didn't receive a signal, so the user never cancelled it
                // signal back to the background thread that it's ok to stop checking and can exit
                cancelledEvent.Set();
            }
            else
            {
                // we receive the cancel, throw an exception to get out
                throw new TWaitCancelException();
            }
        }

        /// <summary>
        /// Checks to see if the Cancel event is Set or Disposed
        /// </summary>
        private bool IsCanceled(ManualResetEvent cancelEvent)
        {
            if (cancelEvent == null)
            {
                throw new InvalidOperationException("IsCanceled was called with a null cancel event.");
            }

            lock (cancelEventDisposeSyncObj)
            {
                if (cancelEvent.SafeWaitHandle != null && !cancelEvent.SafeWaitHandle.IsClosed)
                {
                    // quick test to see if it is set
                    return cancelEvent.WaitOne(0);
                }

                // cancelevent was already Set and has been Disposed.
                return true;
            }
        }

        /// <summary>
        /// Asyncronously wait for the ProgressReporter to be cancelled
        /// </summary>
        protected void AsyncWaitForCancel(object state)
        {
            // unpackage the parameters
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            IProgressReporter progress = (IProgressReporter)stateDict["progress"];
            ManualResetEvent cancelEvent = (ManualResetEvent)stateDict["cancelEvent"];

            while (!progress.IsCancelRequested && !IsCanceled(cancelEvent))
            {
                // check every 1 seconds
                Thread.Sleep(1000);
            }

            // if the user requested the cancel, signal to the calling thread
            if (progress.IsCancelRequested)
            {
                // The caller could have timeed out waiting and eventually disposed our cancelEvent.  
                // so only Set it if it is isn't closed
                lock (cancelEventDisposeSyncObj)
                {
                    // make sure our event hasn't been Disposed yet
                    if (cancelEvent.SafeWaitHandle != null && !cancelEvent.SafeWaitHandle.IsClosed)
                    {
                        // signal the cancel
                        cancelEvent.Set();
                    }
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // clean up our event
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // clean up our event
                lock (cancelEventDisposeSyncObj)
                {
                    cancelledEvent.Dispose();
                }
            }
        }
    }
}
