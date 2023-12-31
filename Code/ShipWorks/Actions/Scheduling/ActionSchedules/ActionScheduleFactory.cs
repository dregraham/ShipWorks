﻿using System;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Factory for creating ActionSchedules
    /// </summary>
    public static class ActionScheduleFactory
    {
        /// <summary>
        /// Creates an action schedule for the given ActionScheduleType
        /// </summary>
        /// <param name="actionScheduleType">The ActionScheduleType for which to create an action schedule.</param>
        public static ActionSchedule CreateActionSchedule(ActionScheduleType actionScheduleType)
        {
            switch (actionScheduleType)
            {
                case ActionScheduleType.OneTime:
                    return new OneTimeActionSchedule();
                
                case ActionScheduleType.Hourly:
                    return new HourlyActionSchedule();
                
                case ActionScheduleType.Daily:
                    return new DailyActionSchedule();
                
                case ActionScheduleType.Weekly:
                    return new WeeklyActionSchedule();

                case ActionScheduleType.Monthly:
                    return new MonthlyActionSchedule();

                default:
                    throw new ArgumentOutOfRangeException("actionScheduleType");
            }
        }
    }
}
