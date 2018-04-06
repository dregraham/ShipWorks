using System;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View model for the archive manager
    /// </summary>
    [Component]
    public class ArchiveManagerViewModel : IArchiveManagerViewModel, INotifyPropertyChanged
    {
        private readonly Func<IOrderArchiveOrchestrator> createArchiveOrchestrator;
        private readonly Func<IArchiveManagerViewModel, IArchiveManagerDialog> createDialog;
        private readonly Func<IAsyncMessageHelper> messageHelper;
        private readonly PropertyChangedHandler handler;

        private bool isBusy;
        private IArchiveManagerDialog dialog;
        private TaskCompletionSource<Unit> dialogCompletionTask = new TaskCompletionSource<Unit>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveManagerViewModel(
            Func<IOrderArchiveOrchestrator> createArchiveOrchestrator,
            Func<IArchiveManagerViewModel, IArchiveManagerDialog> createDialog,
            Func<IAsyncMessageHelper> messageHelper)
        {
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;
            this.createArchiveOrchestrator = createArchiveOrchestrator;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ArchiveNow = new RelayCommand(() => ArchiveNowAction());
            Close = new RelayCommand(() => dialog?.Close());
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Perform an archive now
        /// </summary>
        public ICommand ArchiveNow { get; }

        /// <summary>
        /// Close the dialog
        /// </summary>
        public ICommand Close { get; }

        /// <summary>
        /// Is the application busy
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set => handler.Set(nameof(IsBusy), ref isBusy, value);
        }

        /// <summary>
        /// Show the archive manager dialog
        /// </summary>
        /// <remarks>
        /// We're returning a TaskCompletionSource instead of the ShowDialog task because we may need to
        /// close and reopen this dialog and we need to maintain the same scope.
        /// </remarks>
        public Task<Unit> ShowManager()
        {
            dialog = createDialog(this);
            messageHelper()
                .ShowDialog(() => dialog)
                .Do(_ =>
                {
                    if (!IsBusy)
                    {
                        dialogCompletionTask.SetResult(Unit.Default);
                    }
                });

            return dialogCompletionTask.Task;
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        private async void ArchiveNowAction()
        {
            IsBusy = true;
            dialog.Close();

            await createArchiveOrchestrator().Archive().Recover(ex => Unit.Default).ConfigureAwait(true);

            ShowManager();
            IsBusy = false;
        }
    }
}
