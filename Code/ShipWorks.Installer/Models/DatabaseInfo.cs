namespace ShipWorks.Installer.Models
{
    /// <summary>
    /// DTO for database information
    /// </summary>
    public class DatabaseInfo
    {
        /// <summary>
        /// The name of the database
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The username used to connect to the database
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password used to connect to the database
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The server instance to connect to
        /// </summary>
        public string ServerInstance { get; set; }
    }
}
