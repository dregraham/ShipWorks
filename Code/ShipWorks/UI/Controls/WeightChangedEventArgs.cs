using System;
using Interapptive.Shared.IO.Hardware.Scales;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// WeightChanged EventArgs
    /// </summary>
    public class WeightChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WeightChangedEventArgs(ScaleReadResult result)
        {
            Result = result;
        }

        /// <summary>
        /// The result from the scale
        /// </summary>
        public ScaleReadResult Result { get; }
    }
}
