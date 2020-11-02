using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ShipWorks.Installer.Utilities;

namespace ShipWorks.Installer.Environments
{
    /// <summary>
    /// Class for getting Hub web client environments
    /// </summary>
    public class WebClientEnvironmentFactory : IWebClientEnvironmentFactory
    {
        // Logger
        private readonly ILog log;
        private const string InnoSetupInstallerProductionUrl = "https://prod-sw-downloads-app.s3.amazonaws.com/ShipWorksInstaller.exe";
        private const string EnvironmentSelectedName = "EnvironmentSelectedName";
        private const string EnvironmentOtherWarehouseUrl = "EnvironmentOtherWarehouseUrl";
        private const string EnvironmentOtherInnoInstallerUrl = "EnvironmentOtherInnoInstallerUrl";
        private List<WebClientEnvironment> environments;

        /// <summary>
        /// Constructor
        /// </summary>
        public WebClientEnvironmentFactory(Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(WebClientEnvironmentFactory));

            try
            {
                Load();
            }
            catch (Exception ex)
            {
                log.Error(ex);

                // Something bad happened, just use production.
                SelectedEnvironment = CreateProductionEnvironment();
            }
        }

        /// <summary>
        /// Load the environments and selection
        /// </summary>
        private void Load()
        {
            // If we are not an Interapptive user, only allow Production environment.
            if (!InterapptiveOnly.IsInterapptiveUser)
            {
                log.Info("Using Production Web Client Environment.");
                SelectedEnvironment = CreateProductionEnvironment();
                environments = new List<WebClientEnvironment>
                {
                    SelectedEnvironment
                };

                return;
            }

            string selectedEnvironmentName = InterapptiveOnly.Registry.GetValue(EnvironmentSelectedName, "");
            string otherWarehouseUrl = InterapptiveOnly.Registry.GetValue(EnvironmentOtherWarehouseUrl, "");
            string innoInstallerUrl = InterapptiveOnly.Registry.GetValue(EnvironmentOtherInnoInstallerUrl, "");

            environments = new List<WebClientEnvironment>
            {
                CreateProductionEnvironment(),
                CreateLocalhostEnvironment(innoInstallerUrl),
                CreateOtherEnvironment(otherWarehouseUrl, innoInstallerUrl)
            };

            SelectedEnvironment = Environments.FirstOrDefault(env => env.Name == selectedEnvironmentName);

            if (SelectedEnvironment == null)
            {
                // Just in case something went bad, just default to production.
                SelectedEnvironment = CreateProductionEnvironment();
            }

            log.Info($"Using {SelectedEnvironment.Name} Web Client Environment.");
        }

        /// <summary>
        /// The currently selected environment
        /// </summary>
        public WebClientEnvironment SelectedEnvironment { get; set; }


        /// <summary>
        /// List of available environments
        /// </summary>
        public List<WebClientEnvironment> Environments => environments;

        /// <summary>
        /// Create a production environment
        /// </summary>
        private WebClientEnvironment CreateProductionEnvironment()
        {
            return new WebClientEnvironment()
            {
                Name = "Production",
                WarehouseUrl = "https://hub.shipworks.com/",
                InnoInstallerUrl = InnoSetupInstallerProductionUrl
            };
        }

        /// <summary>
        /// Create a localhost environment
        /// </summary>
        private WebClientEnvironment CreateLocalhostEnvironment(string innoInstallerUrl)
        {
            return new WebClientEnvironment()
            {
                Name = "Localhost",
                WarehouseUrl = "http://localhost:4001/",
                InnoInstallerUrl = string.IsNullOrWhiteSpace(innoInstallerUrl) ? InnoSetupInstallerProductionUrl : innoInstallerUrl
            };
        }

        /// <summary>
        /// Create an "other" environment
        /// </summary>
        private WebClientEnvironment CreateOtherEnvironment(string warehouseUrl, string innoInstallerUrl)
        {
            return new WebClientEnvironment()
            {
                Name = "Other",
                WarehouseUrl = warehouseUrl,
                InnoInstallerUrl = innoInstallerUrl
            };
        }
    }
}
