using System;
using System.Reflection;
using System.Runtime.Serialization;
using Quartz;
using Quartz.Impl.Calendar;

namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// Customized interval calendar for weekly schedules
    /// </summary>
    [Serializable]
    [Obfuscation(Exclude = true)]
    public class IntervalCalendar : BaseCalendar
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public IntervalCalendar() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public IntervalCalendar(ICalendar baseCalendar) : base(baseCalendar) { }

        /// <summary>
        /// Constructor
        /// </summary>
        protected IntervalCalendar(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StartTimeUtc = (DateTimeOffset) info.GetValue("StartTimeUtc", typeof(DateTimeOffset));
            RepeatInterval = info.GetInt32("RepeatInterval");
            RepeatIntervalUnit = (IntervalUnit) info.GetInt32("RepeatIntervalUnit");
        }

        /// <summary>
        /// Get object data for deserialization
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("StartTimeUtc", StartTimeUtc);
            info.AddValue("RepeatInterval", RepeatInterval);
            info.AddValue("RepeatIntervalUnit", (int) RepeatIntervalUnit);
        }

        /// <summary>
        /// Clone the object
        /// </summary>
        public override object Clone()
        {
            var copy = (IntervalCalendar) base.Clone();
            copy.StartTimeUtc = StartTimeUtc;
            copy.RepeatInterval = RepeatInterval;
            copy.RepeatIntervalUnit = RepeatIntervalUnit;
            return copy;
        }

        /// <summary>
        /// Start time in UTC
        /// </summary>
        public DateTimeOffset StartTimeUtc { get; set; }

        /// <summary>
        /// Repeat interval
        /// </summary>
        public int RepeatInterval { get; set; }

        /// <summary>
        /// Unit of repeat interval
        /// </summary>
        public IntervalUnit RepeatIntervalUnit { get; set; }

        /// <summary>
        /// Is this instance equal to the other
        /// </summary>
        public bool Equals(IntervalCalendar other)
        {
            if (object.ReferenceEquals(null, other))
                return false;

            if (GetBaseCalendar() != null && !GetBaseCalendar().Equals(other.GetBaseCalendar()))
                return false;

            return
                this.StartTimeUtc.Equals(other.StartTimeUtc) &&
                this.RepeatInterval.Equals(other.RepeatInterval) &&
                this.RepeatIntervalUnit.Equals(other.RepeatIntervalUnit);
        }

        /// <summary>
        /// Tests for equality
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as IntervalCalendar);
        }

        /// <summary>
        /// Gets the hash code for the object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return
                (GetBaseCalendar() == null ? 0 : GetBaseCalendar().GetHashCode()) ^
                StartTimeUtc.GetHashCode() ^
                RepeatInterval.GetHashCode() ^
                RepeatIntervalUnit.GetHashCode();
        }

        /// <summary>
        /// Tests for equality
        /// </summary>
        public static bool operator ==(IntervalCalendar c1, IntervalCalendar c2)
        {
            if (object.ReferenceEquals(null, c1))
                return object.ReferenceEquals(null, c2);

            return c1.Equals(c2);
        }

        /// <summary>
        /// Tests for inequality
        /// </summary>
        public static bool operator !=(IntervalCalendar c1, IntervalCalendar c2)
        {
            return !(c1 == c2);
        }

        /// <summary>
        /// Is the specified time included in this schedule
        /// </summary>
        public override bool IsTimeIncluded(DateTimeOffset timeUtc)
        {
            timeUtc = timeUtc.ToUniversalTime();

            if (!base.IsTimeIncluded(timeUtc))
            {
                return false;
            }

            var span = timeUtc.Subtract(StartTimeUtc.ToUniversalTime());

            double units;
            switch (RepeatIntervalUnit)
            {
                case IntervalUnit.Millisecond:
                case IntervalUnit.Second:
                case IntervalUnit.Minute:
                case IntervalUnit.Hour:
                case IntervalUnit.Day:
                    throw new NotImplementedException();

                case IntervalUnit.Week:
                    units = span.TotalDays / 7; break;

                case IntervalUnit.Month:
                case IntervalUnit.Year:
                    throw new NotImplementedException();

                default:
                    throw new InvalidOperationException("RepeatIntervalUnit is not set to a known value.");
            }

            return 0 == ((long) units % RepeatInterval);
        }

        /// <summary>
        /// Get the next included time in UTC
        /// </summary>
        public override DateTimeOffset GetNextIncludedTimeUtc(DateTimeOffset timeUtc)
        {
            throw new NotImplementedException();
        }
    }
}
