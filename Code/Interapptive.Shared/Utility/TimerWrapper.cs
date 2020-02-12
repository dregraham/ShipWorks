using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Wraps a timer for unit tests
    /// </summary>
    [Component]
    public class TimerWrapper : ITimer
    {
        private Timer timer = new Timer();

        /// <summary>
        /// Time between calling Elapsed event
        /// </summary>
        public double Interval
        {
            get => timer.Interval;
            set => timer.Interval = value;
        }

        /// <summary>
        /// Event to call when timer is ellapsed
        /// </summary>
        public event ElapsedEventHandler Elapsed
        {
            add { timer.Elapsed += value; }
            remove { timer.Elapsed -= value; }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start() => timer.Start();

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop() => timer.Stop();
    }
}
