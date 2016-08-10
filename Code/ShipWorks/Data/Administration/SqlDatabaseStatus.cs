namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// The various status types a SqlDatabaseDetail object can represent about a database
    /// </summary>
    public enum SqlDatabaseStatus
    {
        /// <summary>
        /// The database is a ShipWorks database (although not necessarily the correct schema version)
        /// </summary>
        ShipWorks,

        /// <summary>
        /// The database is not a ShipWorks database
        /// </summary>
        NonShipWorks,

        /// <summary>
        /// Don't know - no permission access to the database
        /// </summary>
        NoAccess
    }
}
