﻿using System;

namespace ShipWorks.Tests.Shared.ExtensionMethods
{
    /// <summary>
    /// DateTime Extension methods used in tests
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the next day specified.
        /// </summary>
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int) from.DayOfWeek;
            int target = (int) dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}