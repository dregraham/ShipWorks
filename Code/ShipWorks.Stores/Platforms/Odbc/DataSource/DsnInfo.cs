namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// A DTO for the registered ODBC data sources on the client machine.
    /// </summary>
    public class DsnInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DsnInfo"/> class.
        /// </summary>
        /// <param name="name">The name of the DSN.</param>
        /// <param name="driver">The driver used to access the DSN.</param>
        public DsnInfo(string name, string driver)
        {
            Name = name;
            Driver = driver;
        }

        /// <summary>
        /// Gets the name of the DSN.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the ODBC driver used to access the DSN.
        /// </summary>
        /// <value>The driver.</value>
        public string Driver { get; private set; }
    }
}
