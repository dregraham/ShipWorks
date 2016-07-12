using System;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using System.Reflection;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// ViewModel for OdbcUploadMapSettingsControl
    /// </summary>
    public class OdbcUploadMapSettingsControlViewModel : OdbcMapSettingsControlViewModel
    {
        private readonly Func<string, IDialog> dialogFactory;
        private bool columnSourceIsTable = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadMapSettingsControlViewModel"/> class.
        /// </summary>
        /// <param name="dialogFactory">The dialog factory.</param>
        /// <param name="sampleDataCommand">The sample data command.</param>
        /// <param name="logFactory">The log factory.</param>
        /// <param name="messageHelper">The message helper.</param>
        /// <param name="columnSourceFactory">The column source factory.</param>
        public OdbcUploadMapSettingsControlViewModel(Func<string, IDialog> dialogFactory, IOdbcSampleDataCommand sampleDataCommand,
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
        /// Saves the map settings.
        /// </summary>
        /// <param name="store"></param>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.UploadColumnSourceType = ColumnSourceIsTable ?
                (int)OdbcColumnSourceType.Table :
                (int)OdbcColumnSourceType.CustomQuery;

            store.UploadColumnSource = ColumnSourceIsTable ?
                SelectedTable.Name :
                CustomQuery;
        }
    }
}