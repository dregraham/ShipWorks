﻿using System;
using System.Collections.Generic;
using System.Linq;
using CultureAttribute;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using ShipWorks.Tests.Shared;
using Xunit;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
	[UseCulture("en-US")]
	public class DailyActionScheduleAdapterTests
	{
		private readonly DailyActionScheduleAdapter target;
		private TimeSpan ThreeDays = new TimeSpan(3, 0, 0, 0);
		private TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);
		private TimeSpan OneHour = new TimeSpan(0, 1, 0, 0);
		private readonly TimeZoneInfo stLouisTimeZoneInfo;

		public DailyActionScheduleAdapterTests()
		{
			stLouisTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
			var autoMock = AutoMockExtensions.GetLooseThatReturnsMocks();
			autoMock.Mock<IDateTimeProvider>().SetupGet(x => x.TimeZoneInfo).Returns(stLouisTimeZoneInfo);//StLouis time zone
			target = autoMock.Create<DailyActionScheduleAdapter>();
		}

		[Fact]
		public void FiresAtStartTime()
		{
			DailyActionSchedule schedule = new DailyActionSchedule { StartDateTimeInUtc = DateTime.UtcNow };

			IList<DateTimeOffset> fireTimes = schedule.ComputeFireTimes(target, 1);

			Assert.Equal(schedule.StartDateTimeInUtc, fireTimes[0].DateTime);
		}

		[Fact]
		public void FiresAtSpecifiedFrequency()
		{
			DailyActionSchedule schedule = new DailyActionSchedule { FrequencyInDays = 3, StartDateTimeInUtc = new DateTime(2015, 1, 1) };

			DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

			TimeSpan[] intervals = fireTimes.Skip(1)
				.Zip(fireTimes, (x, x0) => x - x0)
				.ToArray();

			Assert.True(intervals.All(x => x.Days == schedule.FrequencyInDays));
		}

		[Fact]
		public void FiresAtSpecifiedFrequency_WhenDstEnds()
		{
			DailyActionSchedule schedule = new DailyActionSchedule
			{
				FrequencyInDays = 3,
				StartDateTimeInUtc = DateTime.Parse("10/27/2015").AddHours(4).ToUniversalTime(stLouisTimeZoneInfo)
			};

			DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

			TimeSpan[] intervals = fireTimes.Skip(1)
				.Zip(fireTimes, (x, x0) => x - x0)
				.ToArray();

			Assert.True(intervals[0] == ThreeDays);
			Assert.True(intervals[1] == ThreeDays.Add(OneHour));
			Assert.True(intervals[2] == ThreeDays);
			Assert.True(intervals[3] == ThreeDays);
		}

		[Fact]
		public void FiresAtSpecifiedFrequency_WhenDstStarts()
		{
			DailyActionSchedule schedule = new DailyActionSchedule { FrequencyInDays = 3, StartDateTimeInUtc = DateTime.Parse("3/4/2015").AddHours(4).ToUniversalTime(stLouisTimeZoneInfo) };

			DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

			TimeSpan[] intervals = fireTimes.Skip(1)
				.Zip(fireTimes, (x, x0) => x - x0)
				.ToArray();

			Assert.True(intervals[0] == ThreeDays);
			Assert.True(intervals[1] == ThreeDays.Subtract(OneHour));
			Assert.True(intervals[2] == ThreeDays);
			Assert.True(intervals[3] == ThreeDays);
		}

		[Fact]
		public void FiresOnceWhenHourRepeats_WhenDstEnds()
		{
			DailyActionSchedule schedule = new DailyActionSchedule { FrequencyInDays = 1, StartDateTimeInUtc = DateTime.Parse("10/30/2015 02:30:00 AM").ToUniversalTime(stLouisTimeZoneInfo) };

			DateTimeOffset[] fireTimes = schedule.ComputeFireTimes(target, 5).ToArray();

			TimeSpan[] intervals = fireTimes.Skip(1)
				.Zip(fireTimes, (x, x0) => x - x0)
				.ToArray();

			Assert.True(intervals[0] == OneDay);
			Assert.True(intervals[1] == OneDay.Add(OneHour));
			Assert.True(intervals[2] == OneDay);
			Assert.True(intervals[3] == OneDay);
		}
	}
}
