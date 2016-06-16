using System;
using System.ComponentModel;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Add deobfuscated exception type to an ExceptionTelemetry object
    /// </summary>
    internal class DeobfuscationProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor next;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeobfuscationProcessor(ITelemetryProcessor next)
        {
            this.next = next;
        }

        /// <summary>
        /// Process the telemetry item
        /// </summary>
        public void Process(ITelemetry item)
        {
            var exceptionItem = item as ExceptionTelemetry;
            if (exceptionItem != null)
            {
                var exceptionType = exceptionItem.Exception?.GetType();
                if (exceptionType != null)
                {
                    DescriptionAttribute exceptionDescription =
                        Attribute.GetCustomAttribute(exceptionType, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    item.Context.Properties.Add("DeobfuscatedType", exceptionDescription?.Description ?? exceptionType.Name);
                }
            }

            next.Process(item);
        }
    }
}