namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Gives us the installed schema version
    /// </summary>
    public interface ISqlSchemaVersion
    {
        /// <summary>
        /// Determines if on a version where customer license is supported
        /// </summary>
        /// <returns></returns>
        bool IsCustomerLicenseSupported();
    }
}