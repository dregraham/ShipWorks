using System;
using System.Threading;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// A simple class designed to be used with 'using' so that anyone downstream can know if Best Rates is active
    /// </summary>
    public class BestRateScope : IDisposable
    {
        static AsyncLocal<bool> active = new AsyncLocal<bool>();

        /// <summary>
        /// Constructor - initiates the scope
        /// </summary>
        public BestRateScope()
        {
            if (active.Value)
            {
                throw new InvalidOperationException("Can only have one active scope per-thread at a time.");
            }

            active.Value = true;
        }

        /// <summary>
        /// Indicates if a BestRateScope is active on the current thread
        /// </summary>
        public static bool IsActive => active.Value;

        /// <summary>
        /// Dispose - get rid of the scope active state
        /// </summary>
        public void Dispose()
        {
            if (!active.Value)
            {
                return;
            }

            active.Value = false;
        }
    }
}
