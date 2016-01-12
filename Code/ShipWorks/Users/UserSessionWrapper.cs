using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    public class UserSessionWrapper : IUserSessionWrapper
    {
        public bool Logon(LogonCredentials credentials)
        {
            return UserSession.Logon(credentials.Username, credentials.Password, credentials.Remember);
        }

        public bool LogonLastUser()
        {
            return UserSession.LogonLastUser();
        }
    }
}