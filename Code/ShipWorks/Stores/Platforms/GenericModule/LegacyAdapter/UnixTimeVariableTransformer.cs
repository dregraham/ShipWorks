using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter
{
    /// <summary>
    /// PostVariableTransformer for converting a UTC string to unix epoch time string 
    /// </summary>
    public class UnixTimeVariableTransformer : VariableTransformer
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public UnixTimeVariableTransformer() 
            : this (null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UnixTimeVariableTransformer(string newName)
            : base(newName)
        {

        }

        /// <summary>
        /// Turns a UTC string into a unix epoch string for interoiperating with legacy v2 modules.
        /// </summary>
        protected override string TransformValue(string originalValue)
        {
            try
            {
                DateTime parsedDateTime = DateTime.Parse(originalValue);

                return GetUnixDateFormat(parsedDateTime);
            }
            catch (FormatException ex)
            {
                // the parameter value is supposed to be a date/time in UTC.
                throw new InvalidOperationException("UnixTimeVariableTransformer expects a valid System.DateTime string to convert to Unix Epoch.", ex);
            }
        }

        /// <summary>
        /// Get the given date as a unix epoch time.
        /// </summary>
        private string GetUnixDateFormat(DateTime dateTime)
        {
            // starts at 1970
            DateTime firstDate = new DateTime(1970, 1, 1);

            long ticks = Math.Max(0, (dateTime.Ticks - firstDate.Ticks) / 10000000);
            return ticks.ToString();
        }
    }
}
