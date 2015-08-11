using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using ShipWorks.Actions;
using ShipWorks.Actions.Scheduling.QuartzNet;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Threading;
using Quartz.Impl;
using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using Quartz.Spi;
using ShipWorks.Tests.Integration.MSTest.Actions.Scheduling;
using ShipWorks.Tests.Integration.MSTest.Fixtures;
using System.Linq;
using ShipWorks.Tests.Integration.MSTest.Utilities;
using log4net;

namespace ShipWorks.Tests.Integration.MSTest
{
    public class HourlyActionScheduleTest : ActionScheduleTest
    {
        private readonly ILog log;
        private bool testComplete = false;
        private string startTime = DateTime.Now.ToString();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        int count = 0;

        public HourlyActionScheduleTest()
        {
            log = LogManager.GetLogger(typeof (HourlyActionScheduleTest));
        }

        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        /// Initialize for a test to verify jobs run when expected.
        /// </summary>
        public void InitializeForHourlyActionScheduleTestTimesShouldRun()
        {
            InitialRunDateTime = new DateTime(2013, 6, 21, 10, 0, 0);

            TotalRunsMade = 0;
            FiresEvery = 1;
            FireTimeGracePeriod = TimeSpan.FromMinutes(2);

            TestTimes = new List<TestTime>();
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 21, 10, 01, 58), ExpectedFireTime = new DateTime(2013, 6, 21, 10, 01, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 21, 10, 59, 58), ExpectedFireTime = new DateTime(2013, 6, 21, 11, 00, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 21, 13, 01, 58), ExpectedFireTime = new DateTime(2013, 6, 21, 13, 01, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 21, 14, 01, 58), ExpectedFireTime = new DateTime(2013, 6, 21, 14, 01, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 21, 23, 59, 58), ExpectedFireTime = new DateTime(2013, 6, 22, 00, 00, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 22, 00, 59, 58), ExpectedFireTime = new DateTime(2013, 6, 22, 01, 00, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 22, 01, 59, 58), ExpectedFireTime = new DateTime(2013, 6, 22, 02, 00, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 22, 10, 00, 00), ExpectedFireTime = new DateTime(2013, 6, 22, 10, 00, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 7, 01, 10, 01, 58), ExpectedFireTime = new DateTime(2013, 7, 01, 10, 01, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 7, 01, 13, 00, 58), ExpectedFireTime = new DateTime(2013, 7, 01, 13, 00, 00) });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 7, 01, 15, 00, 58), ExpectedFireTime = new DateTime(2013, 7, 01, 15, 00, 00) });

            TotalRunsExpected = TestTimes.Count;

            SystemTimeUtilities.UpdateSystemTime(InitialRunDateTime);
        }

        [Fact]
        [Ignore]
        public void Schedule_VerifyJobsRunAtCorrectTimes_Test()
        {
            InitializeForHourlyActionScheduleTestTimesShouldRun();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                SqlSchedulerFactory thisSchedulerFactory = new SqlSchedulerFactory();
                IActionScheduleAdapter thisSchedulerAdapter = new HourlyActionScheduleAdapter();

                // Find our action to test with.
                ActionEntity action = ActionManager.Actions.First(a => a.Name == "_Hourly");
                if (action == null)
                {
                    throw new Exception(
                        "To be able to run this test, create an action of Hourly type, with the name _Hourly.");
                }

                // Create a new hourly action schedule, and initialize it for the test.
                ActionSchedule schedule = new HourlyActionSchedule()
                    {
                        FrequencyInHours = FiresEvery,
                        StartDateTimeInUtc = InitialRunDateTime.ToUniversalTime()
                    };

                QuartzSchedulingEngine quartzSchedulingEngine = new QuartzSchedulingEngine(thisSchedulerFactory,
                                                                                           thisSchedulerAdapter, 
                                                                                           log);

                quartzSchedulingEngine.Schedule(action, schedule);

                // Add a job listener so we can find out of a job fires.
                QuartzJobListener quartzJobListener = new QuartzJobListener();
                quartzJobListener.JobExecutedHandler += OnJobExecuted;
                IMatcher<JobKey> matcher = KeyMatcher<JobKey>.KeyEquals(new JobKey(action.ActionID.ToString()));
                thisSchedulerFactory.GetScheduler().ListenerManager.AddJobListener(quartzJobListener, matcher);

                // Start the scheduler in a separate thread.
                new Task(() => quartzSchedulingEngine.RunAsync(cancellationTokenSource.Token)).Start();

                // Keep checking for complete or cancellation every half second for up to 5 minutes
                while (!testComplete && count <= 600)
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        break;
                    }
                    Thread.Sleep(500);
                    count++;
                }
            }
            catch (Exception)
            {
                // Some exception happened, cancel processing and then throw
                cancellationTokenSource.Cancel(true);
                throw;
            }
            finally
            {
                try
                {
                    // Try to set the system time back to the real time
                    sw.Stop();
                    DateTime currentDateTime = DateTime.Parse(startTime);
                    currentDateTime = currentDateTime.AddTicks(sw.ElapsedTicks).AddMilliseconds(count * 500);
                    SystemTimeUtilities.UpdateSystemTime(currentDateTime);
                    cancellationTokenSource.Cancel(false);
                }
                catch (Exception ex)
                {
                    // If we can't, just eat the exception and carry on.
                    string msg = ex.Message;
                }
            }

        }

        /// <summary>
        /// Event fired when Quartz executes a job.  Used to verify the job ran.
        /// </summary>
        /// <param name="actionID">The ID of the action being tested.</param>
        /// <param name="fireTime">The time that the job was executed.</param>
        /// <param name="nextScheduledFireTime">The time the next job should execute.</param>
        void OnJobExecuted(string actionID, DateTime fireTime, DateTime nextScheduledFireTime)
        {
            // Reset the timer count, so we get another timeframe.
            count = 0;

            // Try and log that job executed.  Not using log4net as that file gets cluttered...fast.  This allows us to 
            // see only these entries.
            try
            {
                System.IO.File.AppendAllText(@"C:\ProgramData\Interapptive\ShipWorks\Instances\JobRuns.txt",
                    string.Format("ActionJob: {0} Running at {1}", actionID, DateTime.Now.ToString("r") + Environment.NewLine));
            }
            catch 
            {
            }

            // Grab the current test run params
            TestTime tt = TestTimes[TotalRunsMade];

            // Check to see that the job executed within the expected time period, including a grace period.
            // (Quartz seems to fire a bit late when changing the system time around a lot like we are doing.)
            if (fireTime > tt.ExpectedFireTime.AddMinutes(FireTimeGracePeriod.Minutes))
            {
                // The job didn't fire during the expected time period, so cancel processing and throw.
                cancellationTokenSource.Cancel(true);
                string errorMessage = string.Format("Job fired too late!  ActionID: '{0}', TestTime.ExpectedFireTime: '{1}', FireTimeGracePeriod: '{2}'", actionID, tt.ExpectedFireTime, FireTimeGracePeriod);
                throw new Exception(errorMessage);
            }

            // Increment to the next test run param
            TotalRunsMade++;

            // If there are no more test runs, set complete and return.
            if (TotalRunsMade == TotalRunsExpected)
            {
                testComplete = true;
                return;
            }

            // Grab the next test run param and set the system time to the new test time.
            tt = TestTimes[TotalRunsMade];
            SystemTimeUtilities.UpdateSystemTime(tt.LocalTime);
        }

        /// <summary>
        /// Initialize for a test for when jobs should NOT run.
        /// </summary>
        public void InitializeForHourlyActionScheduleTestTimesShouldNotRun()
        {
            InitialRunDateTime = new DateTime(2013, 7, 21, 10, 0, 0);

            TotalRunsMade = 0;
            FiresEvery = 1;
            FireTimeGracePeriod = TimeSpan.FromMinutes(2);

            TestTimes = new List<TestTime>();
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 21, 09, 59, 58), ExpectedFireTime = DateTime.MinValue });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 21, 10, 59, 58), ExpectedFireTime = DateTime.MinValue });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 29, 13, 01, 58), ExpectedFireTime = DateTime.MinValue });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 6, 29, 14, 01, 58), ExpectedFireTime = DateTime.MinValue });
            TestTimes.Add(new TestTime() { LocalTime = new DateTime(2013, 7, 21, 08, 59, 58), ExpectedFireTime = DateTime.MinValue });

            TotalRunsExpected = TestTimes.Count;

            SystemTimeUtilities.UpdateSystemTime(InitialRunDateTime);
        }

        [Fact]
        [Ignore]
        public void Schedule_VerifyJobsDontRunAtIncorrectTimes_Test()
        {
            InitializeForHourlyActionScheduleTestTimesShouldNotRun();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                SqlSchedulerFactory thisSchedulerFactory = new SqlSchedulerFactory();
                IActionScheduleAdapter thisSchedulerAdapter = new HourlyActionScheduleAdapter();

                ActionEntity action = ActionManager.Actions.First(a => a.Name == "_Hourly");
                if (action == null)
                {
                    throw new Exception(
                        "To be able to run this test, create an action of Hourly type, with the name _Hourly.");
                }

                ActionSchedule schedule = new HourlyActionSchedule()
                {
                    FrequencyInHours = FiresEvery,
                    StartDateTimeInUtc = InitialRunDateTime.ToUniversalTime()
                };

                QuartzSchedulingEngine quartzSchedulingEngine = new QuartzSchedulingEngine(thisSchedulerFactory,
                                                                                           thisSchedulerAdapter, 
                                                                                           log);

                quartzSchedulingEngine.Schedule(action, schedule);

                QuartzJobListener quartzJobListener = new QuartzJobListener();
                quartzJobListener.JobExecutedHandler += OnJobShouldNotExecute;
                IMatcher<JobKey> matcher = KeyMatcher<JobKey>.KeyEquals(new JobKey(action.ActionID.ToString()));
                thisSchedulerFactory.GetScheduler().ListenerManager.AddJobListener(quartzJobListener, matcher);


                new Task(() => quartzSchedulingEngine.RunAsync(cancellationTokenSource.Token)).Start();

                // Iterate through each test run
                foreach (var testTime in TestTimes)
                {
                    // Set the system time to the test run time
                    SystemTimeUtilities.UpdateSystemTime(testTime.LocalTime);

                    // Sleep for the grace period to allow the job to fire, hoping that it won't.
                    Thread.Sleep(FireTimeGracePeriod);

                    // Check to see if a cancellation request was made, and throw if it was.
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (Exception)
            {
                // An exception occurred, so cancel processing and throw.
                cancellationTokenSource.Cancel(true);
                throw;
            }
            finally
            {
                try
                {
                    // Try to set the system time back to real time.
                    sw.Stop();
                    DateTime currentDateTime = DateTime.Parse(startTime);
                    currentDateTime = currentDateTime.AddTicks(sw.ElapsedTicks).AddMilliseconds(count * 500);
                    SystemTimeUtilities.UpdateSystemTime(currentDateTime);
                    cancellationTokenSource.Cancel(false);
                }
                catch
                {
                    // Just carry on if we can't
                }
            }
        }

        /// <summary>
        /// Job listener event tied to the schedule.
        /// In this case, the job should not execute, so if it does, cancel processing and throw an exception.
        /// </summary>
        void OnJobShouldNotExecute(string actionID, DateTime fireTime, DateTime nextScheduledFireTime)
        {
            // Reset the timer count, so we get another timeframe.
            count = 0;

            // Try and log that job executed.  Not using log4net as that file gets cluttered...fast.  This allows us to 
            // see only these entries.
            try
            {
                System.IO.File.AppendAllText(@"C:\ProgramData\Interapptive\ShipWorks\Instances\JobRuns.txt",
                    string.Format("Job should NOT have run!  ActionJob: {0} Running at {1}", actionID, DateTime.Now.ToString("r") + Environment.NewLine));
            }
            catch
            {
            }

            cancellationTokenSource.Cancel();
            throw new Exception("Job should not have executed!");
        }
    }
}
