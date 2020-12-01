using System.Collections.Generic;

namespace ShipWorks.Installer.Environments
{
    /// <summary>
    /// Interface for getting Hub web client environments
    /// </summary>
    public interface IWebClientEnvironmentFactory
    {
        /// <summary>
        /// The currently selected environment
        /// </summary>
        WebClientEnvironment SelectedEnvironment { get; set; }

        /// <summary>
        /// List of available environments
        /// </summary>
        List<WebClientEnvironment> Environments { get; }
    }
}
