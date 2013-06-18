using ShipWorks.Actions.Scheduling.ActionSchedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// An action schedule adapter that delegates to type-specific adapters (derived from <see cref="ActionScheduleAdapter&lt;TSchedule&gt;"/>)
    /// discovered at runtime by reflecting the current assembly.
    /// </summary>
    public class ReflectingActionScheduleAdapter : IActionScheduleAdapter
    {
        readonly IDictionary<Type, IActionScheduleAdapter> adapterCache = new Dictionary<Type, IActionScheduleAdapter>();


        public Quartz.IScheduleBuilder Adapt(ActionSchedule schedule)
        {
            if (null == schedule)
                throw new ArgumentNullException("schedule");

            var scheduleType = schedule.GetType();

            IActionScheduleAdapter adapter;
            if (!adapterCache.TryGetValue(scheduleType, out adapter))
            {
                var adapterType =
                    Assembly.GetExecutingAssembly().GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(ActionScheduleAdapter<>).MakeGenericType(scheduleType)))
                        .FirstOrDefault();

                if(null != adapterType)
                    adapter = (IActionScheduleAdapter)Activator.CreateInstance(adapterType);

                adapterCache.Add(scheduleType, adapter);
            }

            if (null == adapter)
                throw new ArgumentException(string.Format("No adapter is available for type '{0}'.", scheduleType), "schedule");

            return adapter.Adapt(schedule);
        }
    }
}
