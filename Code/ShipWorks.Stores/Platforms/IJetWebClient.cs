using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms
{
    /// <summary>
    /// Interface for JetWebClient
    /// </summary>
    public interface IJetWebClient
    {
        // Given a username and password, get a token.
        GenericResult<string> GetToken(string username, string password);
    }
}
