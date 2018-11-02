using System;
using System.Runtime.CompilerServices;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Defines a messenger interface
    /// </summary>
    public interface IMessenger : IObservable<IShipWorksMessage>
    {
        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        void Send<T>(T message, [CallerMemberName] string callerName = "") where T : IShipWorksMessage;

        /// <summary>
        /// Available schedulers
        /// </summary>
        ISchedulerProvider Schedulers { get; }
    }
}