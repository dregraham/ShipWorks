using System;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// ViewModel for OdbcMapSettingsControl
    /// </summary>
    public class OdbcImportMapSettingsControlViewModel : OdbcMapSettingsControlViewModel, INotifyPropertyChanged
    {
        private bool columnSourceIsTable = true;
        private bool downloadStrategyIsLastModified = true;
        private readonly Func<string, IDialog> dialogFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcImportMapSettingsControlViewModel(Func<string, IDialog> dialogFactory, IOdbcSampleDataCommand sampleDataCommand,
            Func<Type, ILog> logFactory, IMessageHelper messageHelper, Func<string, IOdbcColumnSource> columnSourceFactory) :
            base(sampleDataCommand, logFactory, messageHelper, columnSourceFactory)
        {
            this.dialogFactory = dialogFactory;
        }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override bool ColumnSourceIsTable
        {
            get { return columnSourceIsTable; }
            set
            {
                Handler.Set(nameof(ColumnSourceIsTable), ref columnSourceIsTable, value);

                // Show warning dlg when query is selected
                if (!value)
                {
                    IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
                    warningDlg.ShowDialog();
                }

                ColumnSource = value ? SelectedTable : CustomQueryColumnSource;
            }
        }

        /// <summary>
        /// Whether the download strategy is last modified.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DownloadStrategyIsLastModified
        {
            get { return downloadStrategyIsLastModified; }
            set { Handler.Set(nameof(DownloadStrategyIsLastModified), ref downloadStrategyIsLastModified, value); }
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.ImportStrategy = DownloadStrategyIsLastModified ?
                (int) OdbcImportStrategy.ByModifiedTime :
                (int) OdbcImportStrategy.All;

            store.ImportSourceType = ColumnSourceIsTable ?
                (int) OdbcColumnSourceType.Table :
                (int) OdbcColumnSourceType.CustomQuery;

            store.ImportColumnSource = ColumnSourceIsTable ?
                SelectedTable.Name :
                CustomQuery;
        }
    }
}
