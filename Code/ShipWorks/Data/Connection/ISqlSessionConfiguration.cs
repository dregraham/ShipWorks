

namespace ShipWorks.Data.Connection
{
    public interface ISqlSessionConfiguration
    {
        string ServerInstance { get; set; }
        string DatabaseName { get; set; }

        string Username { get; set; }
        string Password { get; set; }

        bool WindowsAuth { get; set; }

        string GetConnectionString();
    }
}
