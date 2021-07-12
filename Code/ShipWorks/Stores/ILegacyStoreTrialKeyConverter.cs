namespace ShipWorks.Stores
{
    /// <summary>
    /// Service to convert legacy trial store keys
    /// </summary>
    public interface ILegacyStoreTrialKeyConverter
    {
        /// <summary>
        /// Convert legacy store trial keys into real keys
        /// </summary>
        void ConvertTrials();
    }
}