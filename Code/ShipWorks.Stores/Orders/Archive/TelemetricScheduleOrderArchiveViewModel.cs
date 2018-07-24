using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Administration;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Wrapper for the ScheduleOrderArchiveViewModel that collects telemetry
    /// </summary>
    [Component]
    public class TelemetricScheduleOrderArchiveViewModel : IScheduleOrderArchiveViewModel
    {
        private readonly ScheduleOrderArchiveViewModel actualViewModel;
        private readonly Func<string, ITrackedDurationEvent> startDurationEvent;
        private readonly IOrderArchiveDataAccess dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        public TelemetricScheduleOrderArchiveViewModel(
            ScheduleOrderArchiveViewModel actualViewModel,
            Func<string, ITrackedDurationEvent> startDurationEvent,
            IOrderArchiveDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            this.startDurationEvent = startDurationEvent;
            this.actualViewModel = actualViewModel;
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        public Task Show() =>
            Functional.UsingAsync(
                startDurationEvent("Orders.Archiving.Automatic.Setup"),
                e => actualViewModel.Show().Bind(AddResults(e)));

        /// <summary>
        /// Add the results of the setup dialog
        /// </summary>
        private Func<(bool completed, bool enabled, int numberOfDaysToKeep), Task<Unit>> AddResults(ITrackedDurationEvent telemetryEvent) =>
            async (results) =>
            {
                var (totalOrders, _) = await dataAccess.GetOrderCountsForTelemetry(DateTime.Now).ConfigureAwait(false);

                telemetryEvent.AddProperty("Orders.Archiving.Automatic.Setup.Result", results.completed ? "Completed" : "Aborted");
                telemetryEvent.AddProperty("Orders.Archiving.OriginalDb.SizeInMB", (SqlDiskUsage.GetDatabaseSpaceUsed() / Telemetry.Megabyte).ToString("#0.0"));
                telemetryEvent.AddMetric("Orders.Archiving.Automatic.Setup.RetentionPeriodInDays", results.numberOfDaysToKeep);
                telemetryEvent.AddMetric("Orders.Archiving.OriginalDb.OrderQuantity", totalOrders);

                if (results.completed)
                {
                    telemetryEvent.AddProperty("Orders.Archiving.Automatic.Setup.State", results.enabled ? "Enabled" : "Disabled");
                }

                return Unit.Default;
            };
    }
}
