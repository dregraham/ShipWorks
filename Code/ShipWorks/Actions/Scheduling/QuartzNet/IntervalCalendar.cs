using Quartz;
using Quartz.Impl.Calendar;
using Quartz.Impl.Triggers;
using Quartz.Util;
using System;
using System.Runtime.Serialization;


namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    [Serializable]
    public class IntervalCalendar : BaseCalendar
    {
        public IntervalCalendar()
            : base() { }

        public IntervalCalendar(ICalendar baseCalendar)
            : base(baseCalendar) { }


        protected IntervalCalendar(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StartTimeUtc = (DateTimeOffset)info.GetValue("StartTimeUtc", typeof(DateTimeOffset));
            RepeatInterval = info.GetInt32("RepeatInterval");
            RepeatIntervalUnit = (IntervalUnit)info.GetInt32("RepeatIntervalUnit");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("StartTimeUtc", StartTimeUtc);
            info.AddValue("RepeatInterval", RepeatInterval);
            info.AddValue("RepeatIntervalUnit", (int)RepeatIntervalUnit);
        }

        public override object Clone()
        {
            var copy = (IntervalCalendar)base.Clone();
            copy.StartTimeUtc = this.StartTimeUtc;
            copy.RepeatInterval = this.RepeatInterval;
            copy.RepeatIntervalUnit = this.RepeatIntervalUnit;
            return copy;
        }


        public DateTimeOffset StartTimeUtc { get; set; }
        public int RepeatInterval { get; set; }
        public IntervalUnit RepeatIntervalUnit { get; set; }


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

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IntervalCalendar);
        }

        public override int GetHashCode()
        {
            return
                (GetBaseCalendar() == null ? 0 : GetBaseCalendar().GetHashCode()) ^
                StartTimeUtc.GetHashCode() ^
                RepeatInterval.GetHashCode() ^
                RepeatIntervalUnit.GetHashCode();
        }


        public static bool operator ==(IntervalCalendar c1, IntervalCalendar c2)
        {
            if (object.ReferenceEquals(null, c1))
                return object.ReferenceEquals(null, c2);

            return c1.Equals(c2);
        }

        public static bool operator !=(IntervalCalendar c1, IntervalCalendar c2)
        {
            return !(c1 == c2);
        }


        public override bool IsTimeIncluded(DateTimeOffset timeUtc)
        {
            timeUtc = timeUtc.ToUniversalTime();

            if (!base.IsTimeIncluded(timeUtc))
                return false;

            var span = timeUtc.Subtract(StartTimeUtc.ToUniversalTime());

            double units;
            switch(RepeatIntervalUnit)
            {
                case IntervalUnit.Millisecond:
                case IntervalUnit.Second:
                case IntervalUnit.Minute:
                case IntervalUnit.Hour:
                case IntervalUnit.Day:
                    throw new NotImplementedException();

                case IntervalUnit.Week: units = span.TotalDays / 7; break;

                case IntervalUnit.Month:
                case IntervalUnit.Year:
                    throw new NotImplementedException();

                default:
                    throw new InvalidOperationException("RepeatIntervalUnit is not set to a known value.");
            }

            return 0 == ((long)units % RepeatInterval);
        }


        public override DateTimeOffset GetNextIncludedTimeUtc(DateTimeOffset timeUtc)
        {
            throw new NotImplementedException();
        }
    }
}
