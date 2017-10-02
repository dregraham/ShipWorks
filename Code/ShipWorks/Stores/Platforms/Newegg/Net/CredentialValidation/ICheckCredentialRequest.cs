
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation
{
    /// <summary>
    /// Interface for making a request to check for valid credentials.
    /// </summary>
    public interface ICheckCredentialRequest
    {
        /// <summary>
        /// Makes a request to determine if the credentials are valid.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>Returns true if the credentials are valid; otherwise false.</returns>
        Task<bool> AreCredentialsValid(Credentials credentials);
    }
}
