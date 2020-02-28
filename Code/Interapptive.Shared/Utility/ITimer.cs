using System.Timers;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Timer to call a method on a specific interval
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Time between calling Elapsed event
        /// </summary>
        double Interval { get; set; }

        /// <summary>
        /// Event to call when timer is ellapsed
        /// </summary>
        event ElapsedEventHandler Elapsed;

        /// <summary>
        /// Starts the timer
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the timer
        /// </summary>
        void Stop();
    }
}