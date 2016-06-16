using System;
using log4net;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// A class representing a crash when ShipWorks is running in service execution mode.
    /// </summary>
    public class ServiceCrash
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ServiceCrash));

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCrash" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ServiceCrash(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Submits a crash report (if the service is eligible to do so).
        /// </summary>
        /// <param name="userEmail">The email address of the ShipWorks user the service is running under.</param>
        public void SubmitReport(string userEmail)
        {
            try
            {
                string logName = Guid.NewGuid() + ".zip";
                string logFileToSubmit = CrashSubmitter.CreateCrashLogZip();

                CrashSubmitter.Submit(Exception, userEmail, logName, logFileToSubmit);
            }
            catch (Exception e)
            {
                // Eat any exceptions tha occur as a result of submitting the crash report
                log.Error(e);
            }
        }
    }
}
