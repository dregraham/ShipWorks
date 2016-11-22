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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.UI.Platforms.Magento.WizardPages
{
    /// <summary>
    /// ViewModel for MagentoWizardSettingsControl
    /// </summary>
    [Component]
    public class MagentoStoreSetupControlViewModel : INotifyPropertyChanged, IMagentoWizardSettingsControlViewModel
    {
        private bool isMagento1;
        private string username;
        private SecureString password;
        private string storeUrl;
        private string storeCode;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IIndex<MagentoVersion, IMagentoProbe> magentoProbes;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoStoreSetupControlViewModel(IIndex<MagentoVersion, IMagentoProbe> magentoProbes, IEncryptionProviderFactory encryptionProviderFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            this.magentoProbes = magentoProbes;
            this.encryptionProviderFactory = encryptionProviderFactory;
            IsMagento1 = true;
        }

        /// <summary>
        /// True if Magento1, False if Magento2
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsMagento1
        {
            get { return isMagento1; }
            set { handler.Set(nameof(IsMagento1), ref isMagento1, value); }
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
        /// Validate and Save Store
        /// </summary>
        /// <param name="store"></param>
        public void Save(MagentoStoreEntity store)
        {
            if (!Uri.IsWellFormedUriString(storeUrl, UriKind.Absolute))
            {
                throw new MagentoException("Store Url not in an a valid format.");
            }

            store.ModulePassword =
                encryptionProviderFactory.CreateSecureTextEncryptionProvider(username).
                Encrypt(Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password)));

            store.ModuleUsername = username;
            store.ModuleUrl = storeUrl;
            store.ModuleOnlineStoreCode = StoreCode;

            ValidateSettings(store);

            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType)StoreTypeManager.GetType(store);
                storeType.InitializeFromOnlineModule();
            }
            catch (GenericStoreException ex)
            {
                throw new MagentoException($"Could not connect to Magento: {ex.Message}");
            }
        }

        /// <summary>
        /// Validate Settings.
        /// </summary>
        private void ValidateSettings(MagentoStoreEntity store)
        {
            MagentoVersion firstVersionToTry;
            MagentoVersion secondVersionToTry;
            if (isMagento1)
            {
                firstVersionToTry = MagentoVersion.MagentoConnect;
                secondVersionToTry = MagentoVersion.PhpFile;
            }
            else
            {
                firstVersionToTry = MagentoVersion.MagentoTwo;
                secondVersionToTry = MagentoVersion.MagentoTwoREST;
            }

            FindUrl(firstVersionToTry, secondVersionToTry, store);
        }

        /// <summary>
        /// Given 2 versions to try, see if either of them can connect to Magento. If they can, return success and the url to conenct with.
        /// </summary>
        private void FindUrl(MagentoVersion firstVersionToTry, MagentoVersion secondVersionToTry, MagentoStoreEntity store)
        {
            try
            {
                IMagentoProbe probe = magentoProbes[firstVersionToTry];
                GenericResult<Uri> compatibleUrlResult = probe.FindCompatibleUrl(store);
                store.MagentoVersion = (int) firstVersionToTry;
                if (!compatibleUrlResult.Success)
                {
                    probe = magentoProbes[secondVersionToTry];
                    compatibleUrlResult = probe.FindCompatibleUrl(store);
                    store.MagentoVersion = (int) secondVersionToTry;
                }

                if (!compatibleUrlResult.Success)
                {
                    throw new MagentoException("Could not connect to Magento");
                }

                store.ModuleUrl = compatibleUrlResult.Value.ToString();
            }
            catch (GenericStoreException ex)
            {
                throw new MagentoException(ex);
            }
        }
    }
}
