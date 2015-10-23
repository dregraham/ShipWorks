using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages
{
    /// <summary>
    /// Interaction logic for YahooApiAccountSettings.xaml
    /// </summary>
    public partial class YahooApiAccountSettings 
    {
        public YahooApiAccountSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;

            //YahooStoreEntity store = GetStore<YahooStoreEntity>();

            //store.YahooStoreID = StoreIdTextBox.Text;
            //store.AccessToken = accessTokenTextBox.Text;
            //if (StoreIdTextBox.Text.Length == 0)
            //{
            //    MessageHelper.ShowError(this, "Please enter your Store URL");
            //    e.NextPage = this;
            //    return;
            //}

            //if (accessTokenTextBox.Text.Length == 0)
            //{
            //    MessageHelper.ShowError(this, "Please enter your Access Token");
            //    e.NextPage = this;
            //    return;
            //}

            //try
            //{
            //    LemonStandWebClient client = new LemonStandWebClient(store);
            //    //Check to see if we have access to LemonStand with the new creds
            //    //Ask for some orders
            //    try
            //    {
            //        client.GetOrderStatuses();
            //        LemonStandStatusCodeProvider statusProvider = new LemonStandStatusCodeProvider(store);
            //        statusProvider.UpdateFromOnlineStore();

            //    }
            //    catch (Exception ex)
            //    {
            //        if (ex.Message.Equals("The remote server returned an error: (401) Unauthorized."))
            //        {
            //            MessageHelper.ShowError(this, "Invalid access token");
            //        }
            //        else
            //        {
            //            MessageHelper.ShowError(this, "Invalid store URL");
            //        }
            //        e.NextPage = this;
            //    }
            //}
            //catch (YahooException ex)
            //{
            //    ShowConnectionException(ex);
            //    e.NextPage = this;
            //}
        }
    }
}
