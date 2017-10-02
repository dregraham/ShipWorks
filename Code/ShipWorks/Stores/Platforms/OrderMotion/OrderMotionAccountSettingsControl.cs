using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Control for configuring account settings for OrderMotion
    /// </summary>
    public partial class OrderMotionAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Should the save operation use the async version
        /// </summary>
        public override bool IsSaveAsync => true;

        /// <summary>
        /// Load the settings from the store into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            OrderMotionStoreEntity orderMotion = (OrderMotionStoreEntity) store;

            emailAccountControl.InitializeForStore(orderMotion);

            bizIdTextBox.Text = SecureText.Decrypt(orderMotion.OrderMotionBizID, "HttpBizID");
        }

        /// <summary>
        /// Save the settings from the store into the UI
        /// </summary>
        public override async Task<bool> SaveToEntityAsync(StoreEntity store)
        {
            OrderMotionStoreEntity orderMotion = (OrderMotionStoreEntity) store;

            // cleanup the BizID
            string bizId = bizIdTextBox.Text.Replace(Environment.NewLine, "");
            bizId = bizId.Replace(" ", "");
            orderMotion.OrderMotionBizID = SecureText.Encrypt(bizId, "HttpBizID");

            // test connection
            try
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var client = lifetimeScope.Resolve<IOrderMotionWebClient>();
                    await client.TestConnection(orderMotion).ConfigureAwait(true);
                }
            }
            catch (OrderMotionException ex)
            {
                WebException webException = ex.InnerException as WebException;
                if (webException != null)
                {
                    if (webException.Status == WebExceptionStatus.ProtocolError)
                    {
                        MessageHelper.ShowError(this, "An error occurred while connecting to the server, please check that the entered HTTP BizID is correct: " + ex.Message);
                        return false;
                    }
                }

                MessageHelper.ShowError(this, "ShipWorks was unable to connect to OrderMotion: " + ex.Message);
                return false;
            }

            return true;
        }
    }
}
