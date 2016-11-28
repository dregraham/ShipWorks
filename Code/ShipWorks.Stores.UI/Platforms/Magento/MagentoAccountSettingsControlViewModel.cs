using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Security;
using System.Security;
using System.Runtime.InteropServices;

namespace ShipWorks.Stores.UI.Platforms.Magento
{
    /// <summary>
    /// ViewModel for MagentoAccountSettingsControl
    /// </summary>
    [Component]
    public class MagentoAccountSettingsControlViewModel : IMagentoAccountSettingsControlViewModel
    {
        public const string UrlNotInValidFormat = "Store Url not in valid format";
        public const string CouldNotConnect = "Could not connect to Magento.";
        public const string UrlDoesntMatchProbe = "Could not connect to Magento using provided Url.";
        private MagentoVersion magentoVersion;
        private string username;
        private SecureString password;
        private string storeUrl;
        private string storeCode;

        protected event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IIndex<MagentoVersion, IMagentoProbe> magentoProbes;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoAccountSettingsControlViewModel"/> class.
        /// </summary>
        public MagentoAccountSettingsControlViewModel(
            IIndex<MagentoVersion, IMagentoProbe> magentoProbes, 
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.magentoProbes = magentoProbes;
        }

        /// <summary>
        /// Gets or sets the magento version.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public MagentoVersion MagentoVersion
        {
            get { return magentoVersion; }
            set { handler.Set(nameof(MagentoVersion), ref magentoVersion, value); }
        }

        /// <summary>
        /// Magento Username
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Username
        {
            get { return username; }
            set { handler.Set(nameof(Username), ref username, value); }
        }

        /// <summary>
        /// Magento Password
        /// </summary>
        [Obfuscation(Exclude = true)]
        public SecureString Password
        {
            get { return password; }
            set { handler.Set(nameof(Password), ref password, value); }
        }

        /// <summary>
        /// Magento Url
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string StoreUrl
        {
            get { return storeUrl; }
            set { handler.Set(nameof(StoreUrl), ref storeUrl, value); }
        }

        /// <summary>
        /// Magento1 Store Code
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string StoreCode
        {
            get { return storeCode; }
            set { handler.Set(nameof(StoreCode), ref storeCode, value); }
        }

        /// <summary>
        /// Saves the specified store.
        /// </summary>
        public void Save(MagentoStoreEntity store)
        {
            store.MagentoVersion = (int) MagentoVersion;
            store.ModuleUsername = username;

            store.ModulePassword = 
                encryptionProviderFactory.CreateSecureTextEncryptionProvider(username)
                .Encrypt(password.ToInsecureString());

            store.ModuleUrl = storeUrl;
            store.ModuleOnlineStoreCode = StoreCode;

            ValidateSettings(store);

            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType) StoreTypeManager.GetType(store);
                storeType.InitializeFromOnlineModule();
            }
            catch (GenericStoreException ex)
            {
                throw new MagentoException($"Could not connect to Magento: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates the settings.
        /// </summary>
        /// <exception cref="MagentoException">
        /// Could not connect to Magento.
        /// or Provided Url is invalid.
        /// </exception>
        private void ValidateSettings(MagentoStoreEntity store)
        {
            IMagentoProbe probe = magentoProbes[(MagentoVersion)store.MagentoVersion];

            Uri storeUri;
            if(!Uri.TryCreate(store.ModuleUrl, UriKind.Absolute, out storeUri))
            {
                throw new MagentoException(UrlNotInValidFormat);
            }

            GenericResult<Uri> compatibleUrlResult = probe.FindCompatibleUrl(store);

            if(!compatibleUrlResult.Success)
            {
                throw new MagentoException(CouldNotConnect);
            }

            if(!Uri.Equals(storeUri, compatibleUrlResult.Value))
            {
                throw new MagentoException(UrlDoesntMatchProbe +
                    $"{Environment.NewLine}{Environment.NewLine}" +
                    $"Did you mean {compatibleUrlResult.Value}?");
            }
        }

        /// <summary>
        /// Loads the specified store.
        /// </summary>
        /// <param name="store">The store.</param>
        public void Load(MagentoStoreEntity store)
        {
            MagentoVersion = (MagentoVersion) store.MagentoVersion;
            Username = store.ModuleUsername;
            Password = encryptionProviderFactory
                .CreateSecureTextEncryptionProvider(username)
                .Decrypt(store.ModulePassword)
                .ToSecureString();
            StoreUrl = store.ModuleUrl;
            StoreCode = store.ModuleOnlineStoreCode;
        }
    }
}
