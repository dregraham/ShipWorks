namespace ShipWorks.Users.Logon
{
    /// <summary>
    /// Credentials used for logon
    /// </summary>
    public class LogonCredentials
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogonCredentials(string username, string password, bool remember)
        {
            Username = username;
            Password = password;
            Remember = remember;
        }

        /// <summary>
        /// The username to logon
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to logon with
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Whether or not to remember the username and password for auto logon
        /// </summary>
        public bool Remember { get; set; }
    }
}
