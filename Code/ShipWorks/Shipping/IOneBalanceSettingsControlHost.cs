namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for the One Balance Settings Control Host
    /// </summary>
    public interface IOneBalanceSettingsControlHost
    {
        /// <summary>
        /// Send the auto fund settings to Stamps
        /// </summary>
        void SaveSettings();
    }
}
