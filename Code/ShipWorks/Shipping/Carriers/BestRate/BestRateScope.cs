using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// A simple class designed to be used with 'using' so that anyone downstream can know if Best Rates is active
    /// </summary>
    public class BestRateScope : IDisposable
    {
        [ThreadStatic]
        static bool active;

        /// <summary>
        /// Constructor - initiates the scope
        /// </summary>
        public BestRateScope()
        {
            if (active)
            {
                throw new InvalidOperationException("Can only have one active scope per-thread at a time.");
            }

            active = true;
        }

        /// <summary>
        /// Indicates if a BestRateScope is active on the current thread
        /// </summary>
        public static bool IsActive
        {
            get { return active; }
        }

        /// <summary>
        /// Dispose - get rid of the scope active state
        /// </summary>
        public void Dispose()
        {
            if (!active)
            {
                return;
            }

            active = false;
        }
    }
}
