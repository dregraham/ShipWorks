using System.Collections.Generic;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic class that can be used to return telemetry data along with a result
    /// </summary>
    public struct TelemetricResult<T>
    {
        /// <summary>
        /// Contsructor
        /// </summary>
        public TelemetricResult(T value, Dictionary<string, string> telemetry)
        {
            Value = value;
            Telemetry = telemetry;
        }
        
        /// <summary>
        /// The actual result
        /// </summary>
        public T Value { get; }
        
        /// <summary>
        /// The telemetry data associated with the result
        /// </summary>
        public Dictionary<string, string> Telemetry { get; }
    }
}