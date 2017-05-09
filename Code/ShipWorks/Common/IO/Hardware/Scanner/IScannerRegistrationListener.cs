using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Listens for a barcode scanner to register
    /// </summary>
    [Service]
    public interface IScannerRegistrationListener
    {
        /// <summary>
        /// Start listening for a barcode scanner to register
        /// </summary>
        void Start();

        /// <summary>
        /// Stop listening for a barcode scanner to register
        /// </summary>
        void Stop();
    }
}