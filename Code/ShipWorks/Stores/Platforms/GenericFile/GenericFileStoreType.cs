using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email.Accounts;
using ShipWorks.FileTransfer;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Csv;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Excel;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Xml;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using ShipWorks.Stores.Platforms.GenericFile.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// StoreType for the GenericFileStore type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.GenericFile)]
    [Component(RegistrationType.Self)]
    public class GenericFileStoreType : StoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// The type code of the generic store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.GenericFile;

        /// <summary>
        /// The uniquely identifiable tango license identifier for the store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                var generic = (GenericFileStoreEntity) Store;

                switch ((GenericFileSourceTypeCode) generic.FileSource)
                {
                    case GenericFileSourceTypeCode.Disk:
                        return $"{TruncateFolderPath(generic.DiskFolder)} ({SystemData.Fetch().DatabaseID:D})";

                    case GenericFileSourceTypeCode.FTP:
                        return string.Format("{0}{1}", FtpAccountManager.GetAccount(generic.FtpAccountID.Value).Host, generic.FtpFolder);

                    case GenericFileSourceTypeCode.Email:
                        return EmailAccountManager.GetAccount(generic.EmailAccountID.Value).EmailAddress;

                    default:
                        throw new InvalidOperationException("Invalid generic file source type: " + generic.FileSource);
                }
            }
        }

        /// <summary>
        /// truncate the folder path to 111 characters because tango limits the length to 150 char total
        /// 111 characters plus the database guid, parenthesis and space equals 150 characters
        /// </summary>
        private static string TruncateFolderPath(string path)
        {
            // find the start position that will give us the last 111 characters of the string
            // if the string is less than 111 start at the beginning
            int start = path.Length - 111 > 0 ? path.Length - 111 : 0;

            return path.Substring(start);
        }

        /// <summary>
        /// Gets an instances of the store entity.
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            GenericFileStoreEntity store = new GenericFileStoreEntity();

            // set base defaults
            InitializeStoreDefaults(store);

            store.FileFormat = (int) GenericFileFormat.Csv;
            store.FileSource = (int) -1;

            store.DiskFolder = "";

            store.FtpAccountID = null;
            store.FtpFolder = "";

            store.EmailAccountID = null;
            store.EmailIncomingFolder = "";
            store.EmailOnlyUnread = true;
            store.EmailFolderValidityID = 0;
            store.EmailFolderLastMessageID = 0;

            store.NamePatternMatch = null;
            store.NamePatternSkip = null;

            store.SuccessAction = (int) GenericFileSuccessAction.Move;
            store.SuccessMoveFolder = "";
            store.ErrorAction = (int) GenericFileErrorAction.Stop;
            store.ErrorMoveFolder = "";

            store.XmlXsltContent = null;
            store.FlatImportMap = "";

            return store;
        }

        /// <summary>
        /// Support for online columns
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create the fields used to identify unique customers
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new EntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create an identifier that uniquely identifies the order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new GenericFileOrderIdentifier(order.OrderNumber, order.OrderNumberComplete);
        }

        /// <summary>
        /// Create the wizard pages for adding a new store
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
                {
                    new GenericStoreFileFormatPage(),
                    new GenericStoreFileSourcePage(),
                    new GenericStoreXmlSetupPage(),
                    new GenericStoreCsvSetupPage(),
                    new GenericStoreExcelSetupPage()
                };
        }

        /// <summary>
        /// The settings control for managing the settings of the generic store account
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            GenericFileStoreEntity generic = (GenericFileStoreEntity) Store;

            switch ((GenericFileFormat) generic.FileFormat)
            {
                case GenericFileFormat.Xml: return new GenericFileXmlAccountSettingsControl();
                case GenericFileFormat.Csv: return new GenericFileCsvAccountSettingsControl();
                case GenericFileFormat.Excel: return new GenericFileExcelAccountSettingsControl();
            }

            throw new InvalidOperationException("Unknown FileFormat: " + generic.FileFormat);
        }

        /// <summary>
        /// Delete any additional data associated with the store
        /// </summary>
        public override void DeleteStoreAdditionalData(SqlAdapter adapter)
        {
            base.DeleteStoreAdditionalData(adapter);

            GenericFileStoreEntity generic = (GenericFileStoreEntity) Store;

            if (generic.EmailAccountID != null)
            {
                adapter.DeleteEntity(new EmailAccountEntity(generic.EmailAccountID.Value));
            }

            if (generic.FtpAccountID != null)
            {
                adapter.DeleteEntity(new FtpAccountEntity(generic.FtpAccountID.Value));
            }
        }

        /// <summary>
        /// Generic file does not support uploading tracking or downloading by ordernumber
        /// or days back so we return false to skip this page
        /// </summary>
        /// <returns></returns>
        public override bool ShowTaskWizardPage() => false;
    }
}
