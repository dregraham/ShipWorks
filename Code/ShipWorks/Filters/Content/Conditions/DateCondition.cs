using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using log4net;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Adapter;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base for all conditions based on date fields
    /// </summary>
    public abstract class DateCondition : Condition
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DateCondition));

        DateOperator op = DateOperator.Equal;

        DateTime value1 = DateTime.Now.Date;
        DateTime value2 = DateTime.Now.Date;

        int withinAmount = 1;
        DateWithinUnit withinUnit = DateWithinUnit.Days;
        DateWithinRangeType withinRangeType = DateWithinRangeType.FromToday;

        DateRelativeUnit relativeUnit = DateRelativeUnit.Week;

        /// <summary>
        /// Generate a SQL statement for the given column
        /// </summary>
        protected string GenerateSql(string valueExpression, SqlGenerationContext context)
        {
            DateOperator effectiveOp = op;
            DateTime effectiveValue1 = value1;
            DateTime effectiveValue2 = value2;

            // Convert the user requested operator and dates into an absolute range
            GetEffectiveOperation(ref effectiveOp, ref effectiveValue1, ref effectiveValue2);

            // Now we have to apply utc offset
            effectiveValue1 = ConvertToUniversalTime(effectiveValue1);

            // Between \ Not Between
            if (effectiveOp == DateOperator.Between || effectiveOp == DateOperator.NotBetween)
            {
                // We'll need value2
                effectiveValue2 = ConvertToUniversalTime(effectiveValue2);

                string op1 = ">=";
                string op2 = "<";
                string join = "AND";

                if (effectiveOp == DateOperator.NotBetween)
                {
                    op1 = "<";
                    op2 = ">=";
                    join = "OR";
                }

                // Due to the way our inc\exc operators work, we have to bump the second value to the next day
                effectiveValue2 = effectiveValue2.AddDays(1);

                log.DebugFormat("Effective: {0} {1} {2} {3} {0} {4} {5}", valueExpression, op1, effectiveValue1, join, op2, effectiveValue2);

                // Register the parameters
                string parm1 = context.RegisterParameter(effectiveValue1);
                string parm2 = context.RegisterParameter(effectiveValue2);

                return string.Format("{0} {1} {2} {3} {0} {4} {5}", valueExpression, op1, parm1, join, op2, parm2);
            }
            else
            {
                log.DebugFormat("Effective: {0}, {1}", effectiveOp, effectiveValue1);

                // Register the parameter
                string parm = context.RegisterParameter(effectiveValue1);

                return string.Format("{0} {1} {2}", valueExpression, GetSqlOperator(effectiveOp), parm);
            }
        }

        /// <summary>
        /// Convert the user requested operator and dates into an absolute range
        /// </summary>
        public void GetEffectiveOperation(ref DateOperator effectiveOp, ref DateTime effectiveValue1, ref DateTime effectiveValue2)
        {
            DateTime now = SqlSession.Current.GetLocalDate().Date;

            // Greater than X is really GreaterEqual X + 1
            if (op == DateOperator.GreaterThan)
            {
                effectiveOp = DateOperator.GreaterThanOrEqual;
                effectiveValue1 = effectiveValue1.AddDays(1);
            }

            // LessEqual X is really Less than X + 1
            if (op == DateOperator.LessThanOrEqual)
            {
                effectiveOp = DateOperator.LessThan;
                effectiveValue1 = effectiveValue1.AddDays(1);
            }

            // Today will be between the start and end of today
            if (op == DateOperator.Today)
            {
                effectiveOp = DateOperator.Between;
                effectiveValue1 = now;
                effectiveValue2 = now;
            }

            // Yesterday is a Between of the start and end of yesterday
            if (op == DateOperator.Yesterday)
            {
                effectiveOp = DateOperator.Between;
                effectiveValue1 = now.AddDays(-1);
                effectiveValue2 = now.AddDays(-1);
            }

            // Tomorrow is a Between of the start and end of Tomorrow
            if (op == DateOperator.Tomorrow)
            {
                effectiveOp = DateOperator.Between;
                effectiveValue1 = now.AddDays(1);
                effectiveValue2 = now.AddDays(1);
            }

            // Equal is a Between of the begining and end of the configured day
            if (op == DateOperator.Equal)
            {
                effectiveOp = DateOperator.Between;
                effectiveValue2 = effectiveValue1;
            }

            // Not equal is a Not Between of the beginning and end of the configured day
            if (op == DateOperator.NotEqual)
            {
                effectiveOp = DateOperator.NotBetween;
                effectiveValue2 = effectiveValue1;
            }

            int effectiveWithinAmount = 0;
            DateWithinUnit effectiveWithinUnit = DateWithinUnit.Days;
            DateWithinRangeType effectiveWithinInclusive = DateWithinRangeType.WholeInclusive;

            // "This" is just a WithinTheLast(0), Inclusive
            if (op == DateOperator.This)
            {
                effectiveOp = DateOperator.WithinTheLast;
                effectiveWithinInclusive = DateWithinRangeType.WholeInclusive;
                effectiveWithinAmount = 0;
                effectiveWithinUnit = GetWithinUnit(relativeUnit);
            }

            // "Last" is just a WithinTheLast(1), Exclusive
            if (op == DateOperator.Last)
            {
                effectiveOp = DateOperator.WithinTheLast;
                effectiveWithinInclusive = DateWithinRangeType.WholeExclusive;
                effectiveWithinAmount = 1;
                effectiveWithinUnit = GetWithinUnit(relativeUnit);
            }

            if (op == DateOperator.WithinTheLast)
            {
                effectiveOp = DateOperator.WithinTheLast;
                effectiveWithinInclusive = withinRangeType;
                effectiveWithinAmount = withinAmount;
                effectiveWithinUnit = withinUnit;
            }

            if (effectiveOp == DateOperator.WithinTheLast)
            {
                DateTime firstDate;

                if (effectiveWithinInclusive == DateWithinRangeType.FromToday)
                {
                    firstDate = now;
                }
                else
                {
                    firstDate = GetFirstDateOf(effectiveWithinUnit);
                }

                effectiveValue1 = addDateUnit(firstDate, effectiveWithinUnit, -effectiveWithinAmount);

                if (effectiveWithinInclusive == DateWithinRangeType.WholeExclusive)
                {
                    effectiveOp = DateOperator.Between;
                    effectiveValue2 = firstDate.AddDays(-1);
                }
                else
                {
                    effectiveOp = DateOperator.GreaterThanOrEqual;
                }
            }

            if (op == DateOperator.Next)
            {
                effectiveOp = DateOperator.Between;

                DateTime firstDate;
                firstDate = now;

                switch (withinRangeType)
                {
                    case DateWithinRangeType.WholeInclusive:
                        // Gets the date including the current unit
                        effectiveValue1 = GetFirstDateOf(withinUnit);
                        break;
                    case DateWithinRangeType.WholeExclusive:
                        // Add one unit to exclude the unit we are on
                        effectiveValue1 = addDateUnit(effectiveValue1, withinUnit, 1);
                        break;
                    case DateWithinRangeType.FromToday:
                    default:
                        effectiveValue1 = now;
                        break;
                }

                effectiveValue2 = addDateUnit(effectiveValue1, withinUnit, withinAmount);
            }
        }

        /// <summary>
        /// Translate from one unit type to the other
        /// </summary>
        private DateWithinUnit GetWithinUnit(DateRelativeUnit relativeUnit)
        {
            switch (relativeUnit)
            {
                case DateRelativeUnit.Week:
                    return DateWithinUnit.Weeks;
                case DateRelativeUnit.Month:
                    return DateWithinUnit.Months;
                case DateRelativeUnit.Quarter:
                    return DateWithinUnit.Quarters;
                case DateRelativeUnit.Year:
                    return DateWithinUnit.Years;
            }

            throw new InvalidOperationException("Invalid DateRelativeUnit value.");
        }

        /// <summary>
        /// Get the first date of the specified unit, based on the current date
        /// </summary>
        private DateTime GetFirstDateOf(DateWithinUnit withinUnit)
        {
            DateTime now = SqlSession.Current.GetLocalDate().Date;

            switch (withinUnit)
            {
                case DateWithinUnit.Days:
                    return now;

                case DateWithinUnit.Weeks:
                    return now.AddDays(-((((int) now.DayOfWeek) - 1) % 7));

                case DateWithinUnit.Months:
                    return new DateTime(now.Year, now.Month, 1);

                case DateWithinUnit.Quarters:
                    return new DateTime(now.Year, now.Month - (now.Month - 1) % 3, 1);

                case DateWithinUnit.Years:
                    return new DateTime(now.Year, 1, 1);
            }

            throw new InvalidOperationException("Invalid DateWithinUnit value.");

        }

        /// <summary>
        /// Adds the specified date unit to the supplied date
        /// </summary>
        /// <returns></returns>
        private DateTime addDateUnit(DateTime date, DateWithinUnit unit, int value)
        {
            switch (unit)
            {
                case DateWithinUnit.Days:
                    return date.AddDays(value);
                case DateWithinUnit.Weeks:
                    return date.AddDays((value * 7));
                case DateWithinUnit.Months:
                    return date.AddMonths(value);
                case DateWithinUnit.Quarters:
                    return date.AddMonths((value * 3));
                case DateWithinUnit.Years:
                    return date.AddYears(value);
            }

            throw new InvalidOperationException("Invalid DateWithinUnit value.");
        }

        /// <summary>
        /// Get the SQL syntax for the operator we represent
        /// </summary>
        private string GetSqlOperator(DateOperator dateOperator)
        {
            switch (dateOperator)
            {
                case DateOperator.LessThan:
                    return "<";
                case DateOperator.GreaterThanOrEqual:
                    return ">=";
            }

            throw new InvalidOperationException("Invalid operator evaluated in GetSqlOperator.");
        }

        /// <summary>
        /// Convert the given SQL Server local date\time to the SQL Server UTC
        /// </summary>
        public DateTime ConvertToUniversalTime(DateTime dateTime)
        {
            return ExistingConnectionScope.ExecuteWithCommand(cmd =>
            {
                cmd.CommandText = "SELECT dbo.DateToUniversalTime(@dateTime)";
                cmd.Parameters.AddWithValue("@dateTime", dateTime);

                return (DateTime)SqlCommandProvider.ExecuteScalar(cmd);
            });
        }

        /// <summary>
        /// The operator used for the comparison.
        /// </summary>
        public DateOperator Operator
        {
            get
            {
                return op;
            }
            set
            {
                op = value;
            }
        }

        /// <summary>
        /// Primary value to be evaluated.
        /// </summary>
        public DateTime Value1
        {
            get
            {
                return value1;
            }
            set
            {
                value1 = value.Date;
            }
        }

        /// <summary>
        /// Second value to be evaluated.  Only used for the Between \ NotBetween operators.
        /// </summary>
        public DateTime Value2
        {
            get
            {
                return value2;
            }
            set
            {
                value2 = value.Date;
            }
        }

        /// <summary>
        /// Amount of units when using the Within date operator
        /// </summary>
        public int WithinAmount
        {
            get
            {
                return withinAmount;
            }
            set
            {
                withinAmount = value;
            }
        }

        /// <summary>
        /// The units to be within when using the Within date operator
        /// </summary>
        public DateWithinUnit WithinUnit
        {
            get
            {
                return withinUnit;
            }
            set
            {
                withinUnit = value;
            }
        }

        /// <summary>
        /// Indicates if the Within conditions include the current unit or not.
        /// </summary>
        public DateWithinRangeType WithinRangeType
        {
            get
            {
                return withinRangeType;
            }
            set
            {
                withinRangeType = value;
            }
        }

        /// <summary>
        /// The relative date span when using a relative date operator
        /// </summary>
        public DateRelativeUnit RelativeUnit
        {
            get
            {
                return relativeUnit;
            }
            set
            {
                relativeUnit = value;
            }
        }

        /// <summary>
        /// Indicates if the condition is based on a date operator relative to today
        /// </summary>
        public bool IsRelative()
        {
            return IsRelativeOperator(Operator);
        }

        /// <summary>
        /// Returns true if the operator is based on relative date(s), and not absolute specified dates
        /// </summary>
        public static bool IsRelativeOperator(DateOperator op)
        {
            switch (op)
            {
                case DateOperator.Today:
                case DateOperator.Yesterday:
                case DateOperator.Tomorrow:
                case DateOperator.This:
                case DateOperator.Last:
                case DateOperator.WithinTheLast:
                case DateOperator.Next:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Create the date specific condition editor
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new DateValueEditor(this);
        }
    }
}
