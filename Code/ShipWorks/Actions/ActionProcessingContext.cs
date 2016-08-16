using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Email;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Provides stateful information that can be passed to tasks during action running
    /// </summary>
    public class ActionProcessingContext : IDisposable
    {
        static int activeProcessors = 0;

        bool disposed = false;

        List<EmailOutboundEntity> emailGenerated = new List<EmailOutboundEntity>();
        Dictionary<long, TemplateEntity> templateCache = new Dictionary<long, TemplateEntity>();
        Dictionary<long, LabelSheetEntity> labelSheetCache = new Dictionary<long, LabelSheetEntity>();
        List<ActionPostponement> postponements = new List<ActionPostponement>();

        bool flushingPostponed = false;

        // We take an applock for the duration of the context on this ID.  Useful for knowing which postponed queues are still being used.  Potentially
        // other uses in the future?
        string contextLockName = Guid.NewGuid().ToString();
        SqlAppResourceLock contextSqlLock;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionProcessingContext()
        {
            Interlocked.Increment(ref activeProcessors);

            // Take the lock for the duration of the context
            contextSqlLock = new SqlAppResourceLock(contextLockName);
        }

        /// <summary>
        /// A SqlAppResource lock is taken on this guid while the context is alive.  This can be used to determine what things are still active or not.
        /// </summary>
        public string ContextLockName
        {
            get { return contextLockName; }
        }

        /// <summary>
        /// Indicates if there are any ActionProcessingContext objects currently in existence
        /// </summary>
        public static bool IsProcessing
        {
            get { return activeProcessors > 0; }
        }

        /// <summary>
        /// The list of all email messages generated during task processing
        /// </summary>
        public List<EmailOutboundEntity> EmailGenerated
        {
            get { return emailGenerated; }
        }

        /// <summary>
        /// A cache of templates that have been used so far.  This is so each task can be sure to use the same version of a template.
        /// </summary>
        public Dictionary<long, TemplateEntity> TemplateCache
        {
            get { return templateCache; }
        }

        /// <summary>
        /// A cache of label sheets that have been used so far.  This is so each task can be sure to use the same version of a sheet.
        /// </summary>
        public Dictionary<long, LabelSheetEntity> LabelSheetCache
        {
            get { return labelSheetCache; }
        }

        /// <summary>
        /// The list of current postponements. Each postponement represents a single queue that has postponed a single step.
        /// </summary>
        public List<ActionPostponement> Postponements
        {
            get { return postponements; }
        }

        /// <summary>
        /// Indicates if all the postponed steps are currently being forcibly flushed out and completed
        /// </summary>
        public bool FlushingPostponed
        {
            get { return flushingPostponed; }
            set { flushingPostponed = value; }
        }

        /// <summary>
        /// Dispose of the context
        /// </summary>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            Interlocked.Decrement(ref activeProcessors);

            contextSqlLock.Dispose();
            contextSqlLock = null;

            // There should never be leftover queues.  But in a release, we don't want to throw an exception here
            // b\c it could hide a real exception, if the reason we have leftover queues is b\c an exception was thrown
            // during processing
            Debug.Assert(Postponements.Count == 0, "Should not have leftover postponed queues.");

            // If there are emails that were generated, we need to start emailing - but that has to be done on the UI
            if (EmailGenerated.Count > 0)
            {
                if (Program.ExecutionMode.IsUIDisplayed)
                {
                    Program.MainForm.BeginInvoke(new MethodInvoker(StartEmailingOnUI));
                }
                else
                {
                    EmailCommunicator.StartEmailingMessages(EmailGenerated);
                }
            }
        }

        /// <summary>
        /// Completion functionality that must be on the UI thread
        /// </summary>
        private void StartEmailingOnUI()
        {
            // Initiate sending of all the email messages that were sent
            if (EmailGenerated.Count > 0)
            {
                EmailCommunicator.StartEmailingMessages(EmailGenerated);
            }
        }
    }
}
