using Interapptive.Shared.Utility;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Bridge for the ShipWorks exe to communicate with the ShipWorks Escalator service
    /// </summary>
    public interface IShipWorksCommunicationBridge
    {
        /// <summary>
        /// Event when bridge receives a message
        /// </summary>
        event ShipWorksCommunicationBridge.DelegateMessage OnMessage;

        /// <summary>
        /// Generate a new pipe server and wait for connections
        /// </summary>
        void StartPipeServer();

        /// <summary>
        /// Send a message
        /// </summary>
        Result SendMessage(string message);

        /// <summary>
        /// Check if the communication bridge is available
        /// </summary>
        bool IsAvailable();
    }
}