namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Interface implemented by the ODBC store wizard pages
    /// </summary>
    public interface IOdbcWizardPage
    {
        /// <summary>
        /// The position of the wizard page in the collection of pages
        /// </summary>
        int Position { get; }
    }
}