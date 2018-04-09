using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.UI;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

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
        private readonly Func<ISqlSession> getSqlSession;
        private readonly IShipWorksDatabaseUtility databaseUtility;
        private readonly Func<IAsyncMessageHelper> messageHelper;
        private readonly PropertyChangedHandler handler;

        private bool performingManualArchive;
        private bool loadingArchives;
        private IEnumerable<ISqlDatabaseDetail> archives;
        private IArchiveManagerDialog dialog;
        private readonly TaskCompletionSource<Unit> dialogCompletionTask = new TaskCompletionSource<Unit>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveManagerViewModel(
            Func<IOrderArchiveOrchestrator> createArchiveOrchestrator,
            Func<IArchiveManagerViewModel, IArchiveManagerDialog> createDialog,
            Func<ISqlSession> getSqlSession,
            IShipWorksDatabaseUtility databaseUtility,
            Func<IAsyncMessageHelper> messageHelper)
        {
            this.getSqlSession = getSqlSession;
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;
            this.databaseUtility = databaseUtility;
            this.createArchiveOrchestrator = createArchiveOrchestrator;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            Archives = Enumerable.Empty<ISqlDatabaseDetail>();
            ArchiveNow = new RelayCommand(() => ArchiveNowAction().Forget());
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
        /// The dialog is loading archives
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool LoadingArchives
        {
            get => loadingArchives;
            set => handler.Set(nameof(LoadingArchives), ref loadingArchives, value);
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
            Functional.UsingAsync(
                    getSqlSession().OpenConnection(),
                    databaseUtility.GetDatabaseDetails)
                .Do(PopulateArchives);

            messageHelper()
                .ShowDialog(() => dialog)
                .Do(_ =>
                {
                    if (!performingManualArchive)
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
            Archives = databases
                .Where(x => x.IsArchive && getSqlSession().DatabaseIdentifier == x.Guid)
                .OrderByDescending(x => x.LastOrderDate);
            LoadingArchives = false;
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        private async Task ArchiveNowAction()
        {
            performingManualArchive = true;
            dialog.Close();

            await createArchiveOrchestrator().Archive().Recover(ex => Unit.Default).ConfigureAwait(true);

            ShowManager().Forget();
            performingManualArchive = false;
        }
    }
}
