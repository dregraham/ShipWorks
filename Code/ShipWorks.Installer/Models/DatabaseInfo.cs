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
        /// The status of the db
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The last activity time and user
        /// </summary>
        public string LastActivity { get; set; }

        /// <summary>
        /// The latest order number and date
        /// </summary>
        public string LatestOrder { get; set; }
    }
}
