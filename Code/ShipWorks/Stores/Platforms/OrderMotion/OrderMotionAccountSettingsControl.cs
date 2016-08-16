using System;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using System.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;

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
        /// Load the settings from the store into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            OrderMotionStoreEntity orderMotion = (OrderMotionStoreEntity)store;

            emailAccountControl.InitializeForStore(orderMotion);

            bizIdTextBox.Text = SecureText.Decrypt(orderMotion.OrderMotionBizID, "HttpBizID");
        }

        /// <summary>
        /// Save the settings from the store into the UI
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            OrderMotionStoreEntity orderMotion = (OrderMotionStoreEntity)store;

            // cleanup the BizID
            string bizId = bizIdTextBox.Text.Replace(Environment.NewLine, "");
            bizId = bizId.Replace(" ", "");
            orderMotion.OrderMotionBizID = SecureText.Encrypt(bizId, "HttpBizID");

            // test connection
            try
            {
                OrderMotionWebClient client = new OrderMotionWebClient(orderMotion);
                client.TestConnection();
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
