using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.Carriers.Amazon.SFP.DTO;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Terms;
using ShipWorks.Users;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SFP.Terms
{
    /// <summary>
    /// Class for showing the Amazon Terms UI
    /// </summary>
    [Component]
    public class AmazonSfpTermsViewModel : IAmazonSfpTermsViewModel, INotifyPropertyChanged
    {
        private const UserConditionalNotificationType notificationType = UserConditionalNotificationType.AmazonTermsAndConditions;

        private readonly IAmazonTermsWebClient amazonTermsWebClient;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IAsyncMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;
        private readonly ILifetimeScope lifetimeScope;

        private IAmazonSfpTermsDialog dialog;
        private string termsUrl = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSfpTermsViewModel(ILifetimeScope lifetimeScope, 
            ICurrentUserSettings currentUserSettings, 
            IAsyncMessageHelper messageHelper,
            IAmazonTermsWebClient amazonTermsWebClient)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.lifetimeScope = lifetimeScope;
            this.messageHelper = messageHelper;
            this.currentUserSettings = currentUserSettings;
            this.amazonTermsWebClient = amazonTermsWebClient;

            Dismiss = new RelayCommand(() => DismissAction());
            Accept = new RelayCommand(() => AcceptAction());
        }

        /// <summary>
        /// Notify of changing properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Show the terms UI
        /// </summary>
        public async Task Show(AmazonTermsVersion amazonTermsVersion)
        {
            await messageHelper.ShowDialog(() => SetupDialog(amazonTermsVersion));
        }

        /// <summary>
        /// Dismiss the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Dismiss { get; }

        /// <summary>
        /// Accept the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Accept { get; }

        /// <summary>
        /// Terms url
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string TermsUrl
        {
            get { return termsUrl; }
            set { handler.Set(nameof(TermsUrl), ref termsUrl, value); }
        }

        /// <summary>
        /// Version of the Terms
        /// </summary>
        public string TermsVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Have the terms been accepted
        /// </summary>
        public bool TermsAccepted { get; set; }

        /// <summary>
        /// Setup the success dialog
        /// </summary>
        private IDialog SetupDialog(AmazonTermsVersion amazonTermsVersion)
        {
            dialog = lifetimeScope.Resolve<IAmazonSfpTermsDialog>();
            dialog.DataContext = this;
            TermsUrl = amazonTermsVersion.Url;
            TermsVersion = amazonTermsVersion.Version;

            return dialog;
        }

        /// <summary>
        /// Dismiss the dialog
        /// </summary>
        private void DismissAction()
        {
            currentUserSettings.StopShowingNotificationFor(notificationType, TimeSpan.FromDays(1));

            dialog.Close();
        }

        /// <summary>
        /// Accept the dialog
        /// </summary>
        private void AcceptAction()
        {
            var success = false;

            // Make call to Hub to accept current terms
            var acceptTermsTask = Task.Run(async () =>
                success = await amazonTermsWebClient.AcceptTerms(Version.Parse(TermsVersion)).ConfigureAwait(true));

            Task.WaitAll(acceptTermsTask);

            TermsAccepted = success;

            dialog.Close();
        }
    }
}
