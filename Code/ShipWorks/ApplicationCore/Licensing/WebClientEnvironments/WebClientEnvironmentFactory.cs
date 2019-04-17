using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using log4net;

namespace ShipWorks.ApplicationCore.Licensing.WebClientEnvironments
{
    /// <summary>
    /// Class for getting ShipWorks web client environments
    /// </summary>
    [Component(RegistrationType.Self, SingleInstance = true)]
    public class WebClientEnvironmentFactory
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(WebClientEnvironmentFactory));
        private readonly IEncryptionProvider encryptionProvider;
        private List<WebClientEnvironment> environments;
        private const string EnvironmentSelectedName = "EnvironmentSelectedName";
        private const string EnvironmentOtherTangoUrl = "EnvironmentOtherTangoUrl";
        private const string EnvironmentOtherWarehouseUrl = "EnvironmentOtherWarehouseUrl";

        /// <summary>
        /// Constructor
        /// </summary>
        public WebClientEnvironmentFactory(IEncryptionProviderFactory encryptionProviderFactory)
        {
            encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider("interapptive");

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
            // If we are a production release, only allow Production environment.
            if (Assembly.GetExecutingAssembly().GetName().Version.Major != 0)
            {
                SelectedEnvironment = CreateProductionEnvironment();
                environments = new List<WebClientEnvironment>
                {
                    SelectedEnvironment
                };

                return;
            }

            string selectedEnvironmentName = InterapptiveOnly.Registry.GetValue(EnvironmentSelectedName, "");
            string otherTangoUrl = InterapptiveOnly.Registry.GetValue(EnvironmentOtherTangoUrl, "");
            string otherWarehouseUrl = InterapptiveOnly.Registry.GetValue(EnvironmentOtherWarehouseUrl, "");

            environments = new List<WebClientEnvironment>
            {
                CreateProductionEnvironment(),
                CreateQascEnvironment(),
                CreateStagingEnvironment(),
                CreateWarehouseQaEnvironment(),
                CreateWarehouseStagingEnvironment(),
                CreateLocalhostEnvironment(),
                CreateOtherEnvironment(otherTangoUrl, otherWarehouseUrl)
            };

            SelectedEnvironment = environments.FirstOrDefault(env => env.Name == selectedEnvironmentName);

            if (SelectedEnvironment == null)
            {
                // Just in case something went bad, just default to production.
                SelectedEnvironment = CreateProductionEnvironment();
            }
        }

        /// <summary>
        /// The currently selected environment
        /// </summary>
        public WebClientEnvironment SelectedEnvironment { get; set; }

        /// <summary>
        /// Save the selected environment for use.
        /// </summary>
        public void SaveSelection()
        {
            InterapptiveOnly.Registry.SetValue(EnvironmentSelectedName, SelectedEnvironment.Name);

            if (SelectedEnvironment.Name == "Other")
            {
                InterapptiveOnly.Registry.SetValue(EnvironmentOtherTangoUrl, SelectedEnvironment.TangoUrl);
                InterapptiveOnly.Registry.SetValue(EnvironmentOtherWarehouseUrl, SelectedEnvironment.WarehouseUrl);
            }
        }

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
                TangoUrl = "https://www.interapptive.com/ShipWorksNet/ShipWorksV1.svc/account/shipworks",
                WarehouseUrl = "https://warehouse.interapptive.com/",
                HeaderShipWorksUsername = encryptionProvider.Decrypt("C5NOiKdNaM/324R7sIjFUA=="),
                HeaderShipWorksPassword = encryptionProvider.Decrypt("lavEgsQoKGM="),
                SoapAction = "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost",
                ForcePreCallCertificationValidation = true
            };
        }

        /// <summary>
        /// Create a QASC environment
        /// </summary>
        private WebClientEnvironment CreateQascEnvironment()
        {
            return new WebClientEnvironment()
            {
                Name = "QASC",
                TangoUrl = "https://qasc.interapptive.warehouseapp.link/ShipWorksNet/ShipWorksV1.svc/account/shipworks",
                WarehouseUrl = "",
                HeaderShipWorksUsername = encryptionProvider.Decrypt("C5NOiKdNaM/324R7sIjFUA=="),
                HeaderShipWorksPassword = encryptionProvider.Decrypt("lavEgsQoKGM="),
                SoapAction = "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost",
                ForcePreCallCertificationValidation = true
            };
        }

        /// <summary>
        /// Create a Staging environment
        /// </summary>
        private WebClientEnvironment CreateStagingEnvironment()
        {
            return new WebClientEnvironment()
            {
                Name = "Staging",
                TangoUrl = "https://staging.interapptive.warehouseapp.link/ShipWorksNet/ShipWorksV1.svc/account/shipworks",
                WarehouseUrl = "",
                HeaderShipWorksUsername = encryptionProvider.Decrypt("C5NOiKdNaM/324R7sIjFUA=="),
                HeaderShipWorksPassword = encryptionProvider.Decrypt("lavEgsQoKGM="),
                SoapAction = "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost",
                ForcePreCallCertificationValidation = true
            };
        }

        /// <summary>
        /// Create a Warehouse - QA environment
        /// </summary>
        private WebClientEnvironment CreateWarehouseQaEnvironment()
        {
            return new WebClientEnvironment()
            {
                Name = "Warehouse - QA",
                TangoUrl = "http://qa.fakeshipworksnet.warehouseapp.link/ShipWorksNet/ShipWorksV1.svc/account/shipworks",
                WarehouseUrl = "http://qa.www.warehouseapp.link/",
                HeaderShipWorksUsername = "none",
                HeaderShipWorksPassword = "none",
                SoapAction = "",
                ForcePreCallCertificationValidation = false
            };
        }

        /// <summary>
        /// Create a Warehouse - Staging environment
        /// </summary>
        private WebClientEnvironment CreateWarehouseStagingEnvironment()
        {
            return new WebClientEnvironment()
            {
                Name = "Warehouse - Staging",
                TangoUrl = "https://www.interapptive.com/ShipWorksNet/ShipWorksV1.svc/account/shipworks",
                WarehouseUrl = "http://staging.www.warehouseapp.link/",
                HeaderShipWorksUsername = "none",
                HeaderShipWorksPassword = "none",
                SoapAction = "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost",
                ForcePreCallCertificationValidation = false
            };
        }

        /// <summary>
        /// Create a localhost environment
        /// </summary>
        private WebClientEnvironment CreateLocalhostEnvironment()
        {
            return new WebClientEnvironment()
            {
                Name = "Localhost",
                TangoUrl = "http://localhost:4002/ShipWorksNet/ShipWorksV1.svc/account/shipworks",
                WarehouseUrl = "http://localhost:4001/",
                HeaderShipWorksUsername = "none",
                HeaderShipWorksPassword = "none",
                SoapAction = "",
                ForcePreCallCertificationValidation = false
            };
        }

        /// <summary>
        /// Create an "other" environment
        /// </summary>
        private WebClientEnvironment CreateOtherEnvironment(string tangoUrl, string warehouseUrl)
        {
            return new WebClientEnvironment()
            {
                Name = "Other",
                TangoUrl = tangoUrl,
                WarehouseUrl = warehouseUrl,
                HeaderShipWorksUsername = "none",
                HeaderShipWorksPassword = "none",
                SoapAction = "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost",
                ForcePreCallCertificationValidation = false
            };
        }
    }
}
