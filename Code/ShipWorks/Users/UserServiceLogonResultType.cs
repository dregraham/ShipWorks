namespace ShipWorks.Users
{
    /// <summary>
    /// The result of logging in using the UserService.
    /// </summary>
    /// <remarks>
    /// Represents success or the reason why the login failed.
    /// </remarks>
    public enum UserServiceLogonResultType
    {
        // The credentials are invalid
        InvalidCredentials,
        
        // The tango account is disabled
        TangoAccountDisabled,

        // Logon was a success
        Success
    }
}
