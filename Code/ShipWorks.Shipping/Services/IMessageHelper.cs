namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Display messages to the user without taking a dependency on the UI
    /// </summary>
    public interface IMessageHelper
    {
        /// <summary>
        /// Show an error
        /// </summary>
        void ShowError(string message);

        /// <summary>
        /// Show an information message
        /// </summary>
        void ShowInformation(string message);
    }
}