using ShipWorks.Users.Logon;

namespace ShipWorks.Users
{
    public interface IUserSessionWrapper
    {
        bool Logon(LogonCredentials credentials);

        bool LogonLastUser();
    }
}
