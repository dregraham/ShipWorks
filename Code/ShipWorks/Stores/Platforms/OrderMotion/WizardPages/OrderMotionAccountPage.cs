using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

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
            StepNextAsync = OnStepNext;
        }

        /// <summary>
        /// Moving to the next page in the Setup Wizard
        /// </summary>
        private async Task OnStepNext(object sender, WizardStepEventArgs e)
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
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var client = lifetimeScope.Resolve<IOrderMotionWebClient>();
                    await client.TestConnection(store).ConfigureAwait(true);
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
