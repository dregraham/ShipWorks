using System;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using Interapptive.Shared.UI;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using System.Net;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.OrderMotion.WizardPages
{
    /// <summary>
    /// Setup wizard page for configuring OrderMotion account details
    /// </summary>
    public partial class OrderMotionAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next page in the Setup Wizard
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (bizIdTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Your account's HTTP BizId is required.");
                e.NextPage = this;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            OrderMotionStoreEntity store = GetStore<OrderMotionStoreEntity>();

            // cleanup the BizID
            string bizId = bizIdTextBox.Text.Replace(Environment.NewLine, "");
            bizId = bizId.Replace(" ", "");
            store.OrderMotionBizID = SecureText.Encrypt(bizId, "HttpBizID");

            // test connection
            try
            {
                OrderMotionWebClient client = new OrderMotionWebClient(store);
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
                        e.NextPage = this;
                        return;
                    }
                }

                MessageHelper.ShowError(this, "ShipWorks was unable to connect to OrderMotion: " + ex.Message);
                e.NextPage = this;
            }
        }
    }
}
