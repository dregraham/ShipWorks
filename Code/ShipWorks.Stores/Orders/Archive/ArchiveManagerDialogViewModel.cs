using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.UI;
using ShipWorks.Data.Administration;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View model for the archive manager
    /// </summary>
    [Component]
    public class ArchiveManagerDialogViewModel : IArchiveManagerDialogViewModel, INotifyPropertyChanged
    {
        private readonly Func<IOrderArchiveOrchestrator> createArchiveOrchestrator;
        private readonly Func<IArchiveManagerDialogViewModel, IArchiveManagerDialog> createDialog;
        private readonly IArchiveManagerDataAccess dataAccess;
        private readonly Func<IAsyncMessageHelper> messageHelper;
        private readonly PropertyChangedHandler handler;

        private bool working;
        private bool loadingArchives;
        private bool noArchives;
        private ISqlDatabaseDetail selectedArchive;
        private IEnumerable<ISqlDatabaseDetail> archives;
        private IArchiveManagerDialog dialog;
        private readonly TaskCompletionSource<Unit> dialogCompletionTask = new TaskCompletionSource<Unit>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveManagerDialogViewModel(
            Func<IOrderArchiveOrchestrator> createArchiveOrchestrator,
            Func<IArchiveManagerDialogViewModel, IArchiveManagerDialog> createDialog,
            IArchiveManagerDataAccess dataAccess,
            Func<IAsyncMessageHelper> messageHelper)
        {
            this.dataAccess = dataAccess;
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;
            this.createArchiveOrchestrator = createArchiveOrchestrator;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            Archives = Enumerable.Empty<ISqlDatabaseDetail>();
            ArchiveNow = new RelayCommand(() => ArchiveNowAction().Forget());
            ConnectToArchive = new RelayCommand(() => ConnectToArchiveAction(), () => SelectedArchive != null);
            Close = new RelayCommand(() => dialog?.Close());
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Perform an archive now
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ArchiveNow { get; }

        /// <summary>
        /// Connect to an archive
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ConnectToArchive { get; }

        /// <summary>
        /// Close the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Close { get; }

        /// <summary>
        /// List of archives
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<ISqlDatabaseDetail> Archives
        {
            get => archives;
            set => handler.Set(nameof(Archives), ref archives, value);
        }

        /// <summary>
        /// Selected archive
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ISqlDatabaseDetail SelectedArchive
        {
            get => selectedArchive;
            set => handler.Set(nameof(SelectedArchive), ref selectedArchive, value);
        }

        /// <summary>
        /// The dialog is loading archives
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool LoadingArchives
        {
            get => loadingArchives;
            set => handler.Set(nameof(LoadingArchives), ref loadingArchives, value);
        }

        /// <summary>
        /// There are no archives
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool NoArchives
        {
            get => noArchives;
            set => handler.Set(nameof(NoArchives), ref noArchives, value);
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

            LoadingArchives = true;

            dataAccess
                .GetArchiveDatabases()
                .Do(PopulateArchives);

            messageHelper()
                .ShowDialog(() => dialog)
                .Do(_ =>
                {
                    if (!working)
                    {
                        dialogCompletionTask.SetResult(Unit.Default);
                    }
                });

            return dialogCompletionTask.Task;
        }

        /// <summary>
        /// Populate the archives collection
        /// </summary>
        private void PopulateArchives(IEnumerable<ISqlDatabaseDetail> databases)
        {
            Archives = databases.OrderByDescending(x => x.LastOrderDate);
            LoadingArchives = false;
            NoArchives = Archives.None();
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        private async Task ArchiveNowAction()
        {
            working = true;
            dialog.Close();

            await createArchiveOrchestrator().Archive().Recover(ex => Unit.Default).ConfigureAwait(true);

            ShowManager().Forget();
            working = false;
        }

        /// <summary>
        /// Connect to the selected archive
        /// </summary>
        private void ConnectToArchiveAction()
        {
            working = true;
            dialog.Close();

            if (dataAccess.ChangeDatabase(SelectedArchive))
            {
                dialogCompletionTask.SetResult(Unit.Default);
            }
            else
            {
                ShowManager();
                working = false;
            }
        }
    }
}
