using System;
using System.Threading.Tasks;
using log4net;
using ShipWorks.Installer.Api;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for interacting with the Hub
    /// </summary>
    public class HubService : IHubService
    {
        private readonly ILog log;
        private readonly IHubApiClient apiClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubService(Func<Type, ILog> logFactory, IHubApiClient apiClient)
        {
            log = logFactory(typeof(HubService));
            this.apiClient = apiClient;
        }

        /// <summary>
        /// Login to the Hub and save the token
        /// </summary>
        public async Task Login(InstallSettings settings, string username, string password)
        {
            log.Info($"Calling login for user {username}");
            try
            {
                var token = await apiClient.Login(username, password);

                settings.Token = new HubToken
                {
                    Token = token.token,
                    RefreshToken = token.refreshToken,
                    CustomerLicenseKey = token.CustomerLicenseKey,
                    RecurlyTrialEndDate = token.RecurlyTrialEndDate,
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error("An error occurred on login", ex);
                throw new Exception("An unknown error occurred");
            }
        }
    }
}
