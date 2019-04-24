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
        private const string EnvironmentOtherActivationUrl = "EnvironmentOtherActivationUrl";

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
            string otherActivationUrl = InterapptiveOnly.Registry.GetValue(EnvironmentOtherActivationUrl, "");

            environments = new List<WebClientEnvironment>
            {
                CreateProductionEnvironment(),
                CreateQascEnvironment(),
                CreateStagingEnvironment(),
                CreateWarehouseQaEnvironment(),
                CreateWarehouseStagingEnvironment(),
                CreateLocalhostEnvironment(),
                CreateOtherEnvironment(otherTangoUrl, otherWarehouseUrl, otherActivationUrl)
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
                InterapptiveOnly.Registry.SetValue(EnvironmentOtherActivationUrl, SelectedEnvironment.ActivationUrl);
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
                ActivationUrl = "https://www.interapptive.com/ShipWorksNet/ActivationV1.svc",
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
                ActivationUrl = "https://qasc.interapptive.warehouseapp.link/ShipWorksNet/ActivationV1.svc",
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
                ActivationUrl = "https://staging.interapptive.warehouseapp.link/ShipWorksNet/ActivationV1.svc",
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
                TangoUrl = "https://qa.fakeshipworksnet.warehouseapp.link/ShipWorksNet/ShipWorksV1.svc/account/shipworks",
                ActivationUrl = "https://qa.fakeshipworksnet.warehouseapp.link/ShipWorksNet/ActivationV1.svc",
                WarehouseUrl = "https://qa.www.warehouseapp.link/",
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
                ActivationUrl = "https://www.interapptive.com/ShipWorksNet/ActivationV1.svc",
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
                ActivationUrl = "http://localhost:4002/ShipWorksNet/ActivationV1.svc",
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
        private WebClientEnvironment CreateOtherEnvironment(string tangoUrl, string warehouseUrl, string activationUrl)
        {
            return new WebClientEnvironment()
            {
                Name = "Other",
                TangoUrl = tangoUrl,
                WarehouseUrl = warehouseUrl,
                ActivationUrl = activationUrl,
                HeaderShipWorksUsername = "none",
                HeaderShipWorksPassword = "none",
                SoapAction = "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost",
                ForcePreCallCertificationValidation = false
            };
        }
    }
}
