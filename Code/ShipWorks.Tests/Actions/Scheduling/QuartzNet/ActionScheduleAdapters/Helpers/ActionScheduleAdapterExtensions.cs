using Quartz;
using Quartz.Spi;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet;
using System;
using System.Collections.Generic;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    public static class ActionScheduleExtensions
    {
        public static IList<DateTimeOffset> ComputeFireTimes<TSchedule, TAdapter>(this TSchedule schedule, TAdapter adapter, int maxTimes)
            where TSchedule : ActionSchedule
            where TAdapter : ActionScheduleAdapter<TSchedule>
        {
            var trigger = TriggerBuilder.Create()
                .StartAt(schedule.StartDateTimeInUtc)
                .WithSchedule(adapter.Adapt(schedule))
                .Build();

            return TriggerUtils.ComputeFireTimes((IOperableTrigger)trigger, null, maxTimes);
        }
    }
}
