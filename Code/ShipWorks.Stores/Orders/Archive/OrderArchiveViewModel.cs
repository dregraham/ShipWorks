using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Stores.Orders.Archive.Errors;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View Model for the archive orders dialog
    /// </summary>
    [Component(Service = typeof(IOrderArchiveViewModel))]
    public class OrderArchiveViewModel : IOrderArchiveViewModel, INotifyPropertyChanged
    {
        private readonly IAsyncMessageHelper messageHelper;
        private readonly IOrderArchiveDialog archiveOrdersDialog;
        private readonly PropertyChangedHandler handler;
        private readonly IOrderArchiveDataAccess dataAccess;

        private DateTime archiveDate;
        private bool isLoadingCounts;
        private long orderCounts;
        private bool isDateInFuture;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveViewModel(
            IAsyncMessageHelper messageHelper,
            IOrderArchiveDialog archiveOrdersDialog,
            IDateTimeProvider dateTimeProvider,
            IOrderArchiveDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            this.archiveOrdersDialog = archiveOrdersDialog;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            handler.Where(x => x == nameof(ArchiveDate))
                .Where(x => IsLoadingCounts == false)
                .SelectMany(_ => Observable.FromAsync(() => UpdateOrderCounts()))
                .Subscribe();
            handler.Where(x => x == nameof(ArchiveDate))
                .Do(x => IsDateInFuture = ArchiveDate.Date > dateTimeProvider.Now.Date)
                .Subscribe();

            ConfirmArchive = new RelayCommand(() => ConfirmArchiveAction());
            CancelArchive = new RelayCommand(() => CancelArchiveAction());

            ArchiveDate = dateTimeProvider.Now.Subtract(TimeSpan.FromDays(90));
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Confirm archive of the orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ConfirmArchive { get; }

        /// <summary>
        /// Cancel archive orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CancelArchive { get; }

        /// <summary>
        /// Selected order archive cutoff date
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DateTime ArchiveDate
        {
            get { return archiveDate; }
            set { handler.Set(nameof(ArchiveDate), ref archiveDate, value); }
        }

        /// <summary>
        /// Is the view model loading order counts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsLoadingCounts
        {
            get { return isLoadingCounts; }
            set { handler.Set(nameof(IsLoadingCounts), ref isLoadingCounts, value); }
        }

        /// <summary>
        /// Number of orders that will be archived
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long OrderCounts
        {
            get { return orderCounts; }
            set { handler.Set(nameof(OrderCounts), ref orderCounts, value); }
        }

        /// <summary>
        /// Is the archive date in the future
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsDateInFuture
        {
            get { return isDateInFuture; }
            set { handler.Set(nameof(IsDateInFuture), ref isDateInFuture, value); }
        }

        /// <summary>
        /// Get order archive details from user
        /// </summary>
        public Task<DateTime> GetArchiveDateFromUser()
        {
            return messageHelper
                .ShowDialog(SetupDialog)
                .Bind(x => x == true ?
                    Task.FromResult(ArchiveDate) :
                    Task.FromException<DateTime>(Error.Canceled));
        }

        /// <summary>
        /// Setup the archive orders dialog
        /// </summary>
        private IDialog SetupDialog()
        {
            archiveOrdersDialog.DataContext = this;
            return archiveOrdersDialog;
        }

        /// <summary>
        /// Handle the confirmation of archiving orders
        /// </summary>
        private void ConfirmArchiveAction()
        {
            archiveOrdersDialog.DialogResult = true;
            archiveOrdersDialog.Close();
        }

        /// <summary>
        /// Cancel archiving orders
        /// </summary>
        private void CancelArchiveAction() =>
            archiveOrdersDialog.Close();

        /// <summary>
        /// Update the count of orders that will be archived
        /// </summary>
        private async Task UpdateOrderCounts()
        {
            IsLoadingCounts = true;

            OrderCounts = await dataAccess.GetCountOfOrdersToArchive(ArchiveDate).ConfigureAwait(false);

            IsLoadingCounts = false;
        }
    }
}
