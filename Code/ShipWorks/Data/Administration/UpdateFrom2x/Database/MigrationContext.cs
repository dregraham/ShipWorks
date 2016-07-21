using System;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// Contextual information valid during the duration of a V2 -> V3 data migration
    /// </summary>
    public sealed class MigrationContext : IDisposable
    {
        MigrationPropertyBag propertyBag;
        MigrationTaskExecutionPhase executionPhase;
        MigrationTaskBase currentTask;
        IProgressReporter progressItem;

        /// <summary>
        /// Current progress reporting item
        /// </summary>
        public IProgressReporter ProgressItem
        {
            get { return progressItem; }
            set { progressItem = value; }
        }

        /// <summary>
        /// The task that is about to run, is running, or just completed running
        /// </summary>
        public MigrationTaskBase CurrentTask
        {
            get { return instance.currentTask; }
            set { instance.currentTask = value; }
        }

        /// <summary>
        /// Migration properties valid for the duration of the migration
        /// </summary>
        public MigrationPropertyBag PropertyBag
        {
            get { return instance.propertyBag; }
        }

        /// <summary>
        /// Current phase of the migration operation
        /// </summary>
        public MigrationTaskExecutionPhase ExecutionPhase
        {
            get { return instance.executionPhase; }
            set { instance.executionPhase = value; }
        }

        // singleton
        static MigrationContext instance;

        /// <summary>
        /// Singleton Accessor
        /// </summary>
        public static MigrationContext Current
        {
            get { return instance; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MigrationContext(MigrationPropertyBag properties)
        {
            if (instance != null)
            {
                throw new InvalidOperationException("MigrationContexts cannot be nested.");
            }

            propertyBag = properties;

            instance = this;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (instance != null)
            {
                instance = null;
            }
        }
    }
}
