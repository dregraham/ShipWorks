using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Store type for NetworkSolutions integration
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what Network Solutions currently uses")]
    [KeyedComponent(typeof(StoreType), StoreTypeCode.NetworkSolutions)]
    [Component(RegistrationType.Self)]
    class NetworkSolutionsStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NetworkSolutionsStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Identifying type code
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.NetworkSolutions;

        /// <summary>
        /// Creates an instance of the NetworkSolutionsStoreEntity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            NetworkSolutionsStoreEntity storeEntity = new NetworkSolutionsStoreEntity();

            InitializeStoreDefaults(storeEntity);

            storeEntity.UserToken = "";
            storeEntity.DownloadOrderStatuses = "";
            storeEntity.StatusCodes = "";
            storeEntity.StoreUrl = "";

            return storeEntity;
        }

        /// <summary>
        /// Creates a NetworkSolutions order
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new NetworkSolutionsOrderEntity();
        }

        /// <summary>
        /// Creates the order identifier for locating orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            NetworkSolutionsOrderEntity orderEntity = order as NetworkSolutionsOrderEntity;
            if (orderEntity == null)
            {
                throw new InvalidCastException("A non NetworkSolutions order was passed to the NetworkSolutionsStoreType.");
            }

            return new NetworkSolutionsOrderIdentifier(orderEntity.NetworkSolutionsOrderID);
        }

        /// <summary>
        /// Create the wizard pages for the Add Store Wizard
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>()
            {
                new WizardPages.NetworkSolutionsAccountPage(),
                new WizardPages.NetworkSolutionsDownloadStatusPage()
            };
        }

        /// <summary>
        /// Create the control used to configure the Online Update tasks in the setup wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new WizardPages.NetworkSolutionsOnlineUpdateActionControl();
        }

        /// <summary>
        /// Create the account settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new NetworkSolutionsAccountSettingsControl();
        }

        /// <summary>
        /// Create the store settings control
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new NetworkSolutionsStoreSettingsControl();
        }

        /// <summary>
        /// Get the store identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                NetworkSolutionsStoreEntity store = Store as NetworkSolutionsStoreEntity;

                if (!string.IsNullOrEmpty(store.StoreUrl))
                {
                    return store.StoreUrl;
                }
                else
                {
                    // The only way StoreUrl wouldn't be filled in is if the auto-conversion processes during 2x migration could
                    // not retrieve it due to some error.  In that case we will fall back to the old 2x way of licensing this store.
                    // We can't use the actual token, because its secure.
                    byte[] bytes = Encoding.UTF8.GetBytes(store.UserToken);

                    using (MD5 md5 = new MD5CryptoServiceProvider())
                    {
                        // Generate the hash
                        string result = Convert.ToBase64String(md5.ComputeHash(bytes));

                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the possible online order statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            NetworkSolutionsStatusCodeProvider statusCodes = new NetworkSolutionsStatusCodeProvider((NetworkSolutionsStoreEntity) Store);

            return statusCodes.CodeNames;
        }

        /// <summary>
        /// Generate NS specific elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<NetworkSolutionsOrderEntity>(() => (NetworkSolutionsOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("NetworkSolutions");
            outline.AddElement("NetworkSolutionsOrderId", () => order.Value.NetworkSolutionsOrderID);
        }

        /// <summary>
        /// Indicates if the display of the given "Online" column is allowed.
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }
    }
}
