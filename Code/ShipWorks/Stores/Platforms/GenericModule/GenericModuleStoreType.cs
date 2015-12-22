using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule.WizardPages;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Text.RegularExpressions;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Communication;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Common.Threading;
using System.Net;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Templates.Processing.TemplateXml;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Grid;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.GenericFile;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Base class for our PHP-module-based integrations as well as the store type used
    /// for generic order downloading from  a URL.
    /// </summary>
    public class GenericModuleStoreType : StoreType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GenericModuleStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.GenericModule; }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public virtual string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/support/solutions/articles/4000048147"; }
        }

        /// <summary>
        /// Identifies this store type
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity)Store;

                Version currentSchemaVersion = GetSchemaVersion(genericStore);

                string moduleUrl = genericStore.ModuleUrl;

                // Basically the only thing this does, is if someone enters a path to a web root (like "http://www.shipworks.com"), without the
                // trailing slash (which the normalization RegEx requires), the "AbsoluteUri" will add the slash. Otherwise, it will be the exact
                // URL as entered.
                Uri moduleUri;
                if (Uri.TryCreate(moduleUrl, UriKind.Absolute, out moduleUri))
                {
                    moduleUrl = moduleUri.AbsoluteUri;
                }

                string identifier = moduleUrl.ToLowerInvariant();

                //New version of finding the identifier, only if the schemaversion is greater than or equal to 1.1.0
                if (currentSchemaVersion.Complete() >= new Version("1.1.0.0").Complete())
                {
                    //Grab the end of the URL, everything after the last "/"
                    string moduleUrlEnd = moduleUrl.Substring(moduleUrl.LastIndexOf("/") + 1);

                    //Check to see if the end of the module url has a "." 
                    if (moduleUrlEnd.Contains("."))
                    {
                        //Use the old version to remove files from the end of the url
                        identifier = Regex.Replace(identifier, "(admin/)?[^/]*(\\.)?[^/]+$", "", RegexOptions.IgnoreCase);
                    }
                }
                else
                {
                    //Old version
                    identifier = Regex.Replace(identifier, "(admin/)?[^/]*(\\.)?[^/]+$", "", RegexOptions.IgnoreCase);
                }

                // append the storecode if there is one
                if (!string.IsNullOrWhiteSpace(genericStore.ModuleOnlineStoreCode))
                {
                    identifier = string.Format("{0}?{1}", identifier, genericStore.ModuleOnlineStoreCode);
                }

                return identifier;
            }
        }

        /// <summary>
        /// Get the log source
        /// </summary>
        public virtual ApiLogSource LogSource
        {
            get { return ApiLogSource.GenericModuleStore; }
        }

        /// <summary>
        /// Gets an instances of the store entity.
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            GenericModuleStoreEntity store = new GenericModuleStoreEntity();

            // set base defaults
            InitializeStoreDefaults(store);

            return store;
        }

        /// <summary>
        /// Initialize the defaults for the generic store type
        /// </summary>
        protected override void InitializeStoreDefaults(StoreEntity store)
        {
            base.InitializeStoreDefaults(store);

            GenericModuleStoreEntity generic = (GenericModuleStoreEntity)store;

            generic.ModuleVersion = "0.0.0";
            generic.ModuleDeveloper = "";
            generic.ModulePlatform = "";

            generic.ModuleUrl = "";
            generic.ModuleUsername = "";
            generic.ModulePassword = "";

            generic.ModuleOnlineStoreCode = "";
            generic.ModuleStatusCodes = "";

            generic.ModuleRequestTimeout = 60;
            generic.ModuleDownloadPageSize = 50;
            generic.ModuleHttpExpect100Continue = true;
            generic.ModuleResponseEncoding = (int)GenericStoreResponseEncoding.UTF8;

        }

        /// <summary>
        /// Read the details of the GenericStore from the module response of the GetStore call and update the module capabilities.
        /// </summary>
        public void InitializeFromOnlineModule()
        {
            GenericModuleStoreEntity generic = (GenericModuleStoreEntity)Store;

            GenericStoreWebClient webClient = this.CreateWebClient();
            GenericModuleResponse webResponse = webClient.GetStore();

            // Create the client for connecting to the module
            generic.StoreName = XPathUtility.Evaluate(webResponse.XPath, "//Store/Name", "");
            generic.Company = XPathUtility.Evaluate(webResponse.XPath, "//Store/CompanyOrOwner", "");
            generic.Email = XPathUtility.Evaluate(webResponse.XPath, "//Store/Email", "");
            generic.Street1 = XPathUtility.Evaluate(webResponse.XPath, "//Store/Street1", "");
            generic.Street2 = XPathUtility.Evaluate(webResponse.XPath, "//Store/Street2", "");
            generic.Street3 = XPathUtility.Evaluate(webResponse.XPath, "//Store/Street3", "");
            generic.City = XPathUtility.Evaluate(webResponse.XPath, "//Store/City", "");
            generic.StateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(webResponse.XPath, "//Store/State", ""));
            generic.PostalCode = XPathUtility.Evaluate(webResponse.XPath, "//Store/PostalCode", "");
            generic.CountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(webResponse.XPath, "//Store/Country", "US"));
            generic.Phone = XPathUtility.Evaluate(webResponse.XPath, "//Store/Phone", "");
            generic.Website = XPathUtility.Evaluate(webResponse.XPath, "//Store/Website", "");

            // Default to US if it wasn't provided.  The XPathUtility won't use the defaultValue argument for an empty node, only a missing node.
            if (string.IsNullOrWhiteSpace(generic.CountryCode))
            {
                generic.CountryCode = "US";
            }

            // Update the store with the module info
            UpdateOnlineModuleInfo();

            // Grab the status selections right away
            if (generic.ModuleOnlineStatusSupport != (int)GenericOnlineStatusSupport.None)
            {
                CreateStatusCodeProvider().UpdateFromOnlineStore();
            }
        }

        /// <summary>
        /// Read the details of the module from a call to the "GetModule" call.  This only does something if the module
        /// has been changed. If nothing has changed false is returned.
        /// </summary>
        [NDependIgnoreLongMethod]
        public void UpdateOnlineModuleInfo()
        {
            GenericStoreWebClient webClient = this.CreateWebClient();
            GenericModuleResponse webResponse = webClient.GetModule();

            GenericModuleStoreEntity store = (GenericModuleStoreEntity)Store;
            XPathNavigator xpath = webResponse.XPath;

            string platform = XPathUtility.Evaluate(xpath, "//Platform", "");
            string developer = XPathUtility.Evaluate(xpath, "//Developer", "");
            string moduleVersion = webResponse.ModuleVersion.ToString();
            string schemaVersion = webResponse.SchemaVersion.ToString();

            // We know to log the developer\platform\version based on when the it changes.
            if (store.ModulePlatform != platform || store.ModuleDeveloper != developer || store.ModuleVersion != moduleVersion || store.SchemaVersion != schemaVersion)
            {
                // Update the known platform\version
                store.ModulePlatform = platform;
                store.ModuleVersion = moduleVersion;
                store.SchemaVersion = schemaVersion;

                // Update module dev\platform in tango, but only if we have a license to log to
                if (store.SetupComplete)
                {
                    // Only update the developer once setup is complete.  This way, we force the "change detection" above to happen for the first time we download, and after
                    // setup is complete
                    store.ModuleDeveloper = developer;

                    try
                    {
                        TangoWebClient.UpdateGenericModuleInfo(store, platform, developer, moduleVersion);
                    }
                    catch (TangoException ex)
                    {
                        throw new GenericStoreException(ex.Message, ex);
                    }
                }
            }

            // Read the module capabilities from the response
            GenericModuleCapabilities capabilities = ReadModuleCapabilities(webResponse);

            // See if it used to support online status
            if (store.ModuleOnlineStatusSupport != (int)GenericOnlineStatusSupport.None)
            {
                // The data type of the online status codes cannot change
                if (capabilities.OnlineStatusSupport != GenericOnlineStatusSupport.None)
                {
                    if (store.ModuleOnlineStatusDataType != (int)capabilities.OnlineStatusDataType)
                    {
                        throw new GenericStoreException("The online module has been updated in an unsupported way: the online status dataType cannot be changed.");
                    }
                }
                // If it no longer supports it, clear out the old status valuds
                else
                {
                    store.ModuleStatusCodes = "<Root />";
                }
            }

            // Update the capabilities
            store.ModuleDownloadStrategy = (int)capabilities.DownloadStrategy;
            store.ModuleOnlineStatusSupport = (int)capabilities.OnlineStatusSupport;
            store.ModuleOnlineStatusDataType = (int)capabilities.OnlineStatusDataType;
            store.ModuleOnlineCustomerSupport = capabilities.OnlineCustomerSupport;
            store.ModuleOnlineCustomerDataType = (int)capabilities.OnlineCustomerDataType;
            store.ModuleOnlineShipmentDetails = capabilities.OnlineShipmentDetails;

            // Read communications settings
            GenericModuleCommunications communications = ReadModuleCommunications(webResponse);

            // update the communications settings
            store.ModuleHttpExpect100Continue = communications.Expect100Continue;
            store.ModuleResponseEncoding = (int)communications.ResponseEncoding;
        }

        /// <summary>
        /// Read the module capabilities from the web response
        /// </summary>
        protected virtual GenericModuleCapabilities ReadModuleCapabilities(GenericModuleResponse webResponse)
        {
            if (webResponse == null)
            {
                throw new ArgumentNullException("webResponse");
            }

            GenericModuleCapabilities caps = new GenericModuleCapabilities();
            caps.ReadModuleResponse(webResponse.XPath);

            return caps;
        }

        /// <summary>
        /// REad the communication settings from the web response
        /// </summary>
        protected virtual GenericModuleCommunications ReadModuleCommunications(GenericModuleResponse webResponse)
        {
            if (webResponse == null)
            {
                throw new ArgumentNullException("webResponse");
            }

            GenericModuleCommunications communications = new GenericModuleCommunications();
            communications.ReadModuleResponse(webResponse.XPath);

            return communications;
        }


        /// <summary>
        /// Returns the identifier for uniquely identify orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new GenericOrderIdentifier(order.OrderNumber, order.OrderNumberComplete);
        }

        /// <summary>
        /// Returns the fields that identify the customer for an order
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            if (((GenericModuleStoreEntity)Store).ModuleOnlineCustomerSupport)
            {
                instanceLookup = true;

                return new EntityField2[] { OrderFields.OnlineCustomerID };
            }
            else
            {
                return base.CreateCustomerIdentifierFields(out instanceLookup);
            }
        }

        /// <summary>
        /// Create the order downloader for the store
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new GenericModuleDownloader(Store);
        }

        /// <summary>
        /// Create the Wizard pages used in the setup wizard to configure the store.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            List<WizardPage> pages = new List<WizardPage>();

            pages.Add(new GenericStoreModulePage(this));

            return pages;
        }

        /// <summary>
        /// Create the control used to configurd the actions for online update after shipping
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            GenericModuleStoreEntity store = (GenericModuleStoreEntity)Store;

            if (store.ModuleOnlineShipmentDetails ||
                store.ModuleOnlineStatusSupport == (int)GenericOnlineStatusSupport.StatusOnly ||
                store.ModuleOnlineStatusSupport == (int)GenericOnlineStatusSupport.StatusWithComment)
            {
                return new GenericStoreModuleActionControl();
            }

            return null;
        }

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            GenericStoreAccountSettingsControl settingsControl = new GenericStoreAccountSettingsControl();
            settingsControl.Initialize(this);

            return settingsControl;
        }

        /// <summary>
        /// Get the required module version for the store
        /// </summary>
        public virtual Version GetRequiredModuleVersion()
        {
            return new Version("3.0.0");
        }

        /// <summary>
        /// Creates an instance of the downloader. To be overridden by derived stores if necessary. 
        /// </summary>
        public virtual GenericStoreWebClient CreateWebClient()
        {
            return new GenericStoreWebClient((GenericModuleStoreEntity)Store);
        }

        /// <summary>
        /// Instantiates the online updater class. Extension point.
        /// </summary>
        public virtual GenericStoreOnlineUpdater CreateOnlineUpdater()
        {
            return new GenericStoreOnlineUpdater((GenericModuleStoreEntity)Store);
        }

        /// <summary>
        /// Get all the online status choices for the store.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            // if this isn't pointed to a field, online status isn't suported
            if (((GenericModuleStoreEntity)Store).ModuleOnlineStatusSupport == (int)GenericOnlineStatusSupport.None)
            {
                return base.GetOnlineStatusChoices();
            }
            else
            {
                List<string> values = new List<string>();

                GenericStoreStatusCodeProvider statusCodeProvider = CreateStatusCodeProvider();

                // Add each status option
                foreach (string name in statusCodeProvider.CodeNames)
                {
                    values.Add(name);
                }

                return values;
            }
        }

        /// <summary>
        /// Customize what columns we support based on the configured properties of the generic store
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            GenericModuleStoreEntity store = (GenericModuleStoreEntity)Store;

            if (column == OnlineGridColumnSupport.LastModified)
            {
                return store.ModuleDownloadStrategy == (int)GenericStoreDownloadStrategy.ByModifiedTime;
            }

            if (column == OnlineGridColumnSupport.OnlineStatus)
            {
                return store.ModuleOnlineStatusSupport != (int)GenericOnlineStatusSupport.None;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Specifies the download policy for the online store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                GenericModuleStoreEntity store = (GenericModuleStoreEntity)Store;

                if (store.ModuleDownloadStrategy == (int)GenericStoreDownloadStrategy.ByOrderNumber)
                {
                    return new InitialDownloadPolicy(InitialDownloadRestrictionType.OrderNumber);
                }
                else
                {
                    return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
                }
            }
        }

        /// <summary>
        /// Create commands specific to this store instance
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            GenericOnlineStatusSupport statusSupport = (GenericOnlineStatusSupport)((GenericModuleStoreEntity)Store).ModuleOnlineStatusSupport;

            List<MenuCommand> commands = new List<MenuCommand>();

            // See if status is supported at all
            if (statusSupport != GenericOnlineStatusSupport.None &&
                statusSupport != GenericOnlineStatusSupport.DownloadOnly)
            {
                GenericStoreStatusCodeProvider statusCodeProvider = CreateStatusCodeProvider();

                // Add each status option
                foreach (object code in statusCodeProvider.CodeValues)
                {
                    MenuCommand command = new MenuCommand(statusCodeProvider.GetCodeName(code), new MenuCommandExecutor(OnSetOnlineStatus));
                    command.Tag = code;

                    commands.Add(command);
                }


                // Check if we can add the ability to manually set status with comment
                if (statusSupport == GenericOnlineStatusSupport.StatusWithComment)
                {
                    // Can only do custom if there are any in the first place
                    if (statusCodeProvider.CodeValues.Count > 0)
                    {
                        MenuCommand withComment = new MenuCommand("Set with comment...", new MenuCommandExecutor(OnSetOnlineStatus));
                        withComment.Tag = null;
                        withComment.BreakBefore = true;

                        commands.Add(withComment);
                    }
                }
            }

            // Check if we can add the ability to upload tracking number
            if (((GenericModuleStoreEntity)Store).ModuleOnlineShipmentDetails)
            {
                // Add the option to Upload shipment details
                MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
                commands.Add(uploadCommand);

                uploadCommand.BreakBefore = true;
            }


            return commands;
        }

        /// <summary>
        /// Upload shipment details for the selected orders
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading the tracking number.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(UploadShipmentDetailsCallback, context.SelectedKeys);
        }

        /// <summary>
        /// The worker thread function that does the actual details uploading
        /// </summary>
        private void UploadShipmentDetailsCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
            }
            else
            {
                try
                {
                    GenericStoreOnlineUpdater updater = CreateOnlineUpdater();
                    updater.UploadTrackingNumber(shipment);
                }
                catch (GenericStoreException ex)
                {
                    // log it
                    log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                    // add the error to issues so we can react later
                    issueAdder.Add(orderID, ex);
                }
            }
        }

        /// <summary>
        /// Set the online status for the selected orders
        /// </summary>
        private void OnSetOnlineStatus(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Set Status",
                "ShipWorks is setting the online status.",
                "Updating order {0} of {1}...");

            MenuCommand command = context.MenuCommand;
            object code = null;
            string comment = "";
            if (command.Tag == null)
            {
                using (OnlineStatusCommentDlg dlg = new OnlineStatusCommentDlg(CreateStatusCodeProvider()))
                {
                    if (dlg.ShowDialog(context.Owner) == DialogResult.OK)
                    {
                        code = dlg.Code;
                        comment = dlg.Comments;
                    }
                    else
                    {
                        // Cancel now
                        context.Complete();
                        return;
                    }
                }
            }
            else
            {
                code = command.Tag;
            }

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(
                SetOnlineStatusCallback,
                context.SelectedKeys,
                new object[] { code, comment });
        }

        /// <summary>
        /// The worker thread function that does the actual status setting
        /// </summary>
        private void SetOnlineStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            object[] state = (object[])userState;
            object code = state[0];
            string comment = (string)state[1];

            try
            {
                GenericStoreOnlineUpdater updater = CreateOnlineUpdater();
                updater.UpdateOrderStatus(orderID, code, comment);
            }
            catch (GenericStoreException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Creates an instance of the status code provider. 
        /// </summary>
        public GenericStoreStatusCodeProvider CreateStatusCodeProvider()
        {
            if (((GenericModuleStoreEntity)Store).ModuleOnlineStatusSupport == (int)GenericOnlineStatusSupport.None)
            {
                throw new InvalidOperationException("Unable to create the status code container because the store does not support online status codes.");
            }

            return new GenericStoreStatusCodeProvider((GenericModuleStoreEntity)Store);
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        public virtual string GetOnlineOrderIdentifier(OrderEntity order)
        {
            return order.OrderNumberComplete.ToString();
        }

        /// <summary>
        /// Get a Version object from the generic module SchemaVersion
        /// </summary>
        private static Version GetSchemaVersion(GenericModuleStoreEntity genericStore)
        {
            try
            {
                return new Version(genericStore.SchemaVersion);
            }
            catch (ArgumentException)
            {
                log.WarnFormat("Store has an invalid schema version of {0} for store {1}", genericStore.SchemaVersion, genericStore.ModuleOnlineStoreCode);
            }
            catch (FormatException)
            {
                log.WarnFormat("Store has an invalid schema version of {0} for store {1}", genericStore.SchemaVersion, genericStore.ModuleOnlineStoreCode);
            }

            return new Version();
        }
    }
}