namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Save and load scanner configuration
    /// </summary>
    public interface IScannerConfigurationRepository
    {
        /// <summary>
        /// Save the name of the scanner
        /// </summary>
        void Save(string name);

        /// <summary>
        /// Get the name of the current scanner
        /// </summary>
        string Get();
    }
}