using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Input;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View model for OdbcCustomQueryDlg
    /// </summary>
    public class OdbcCustomQueryDlgViewModel : IOdbcCustomQueryDlgViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private readonly IOdbcColumnSource columnSource;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private string query;
        private DataTable results;
        private readonly PropertyChangedHandler handler;
        private bool validQuery;
        private string resultMessage;
        public event PropertyChangedEventHandler PropertyChanged;
        private const int NumberOfSampleResults = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryDlgViewModel"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="columnSource">The column source.</param>
        /// <param name="sampleDataCommand"></param>
        /// <param name="messageHelper"></param>
        /// <param name="logFactory">The log factory.</param>
        public OdbcCustomQueryDlgViewModel(IOdbcDataSource dataSource, IOdbcColumnSource columnSource,
            IOdbcSampleDataCommand sampleDataCommand, IMessageHelper messageHelper, Func<Type, ILog> logFactory)
        {
            this.dataSource = dataSource;
            this.columnSource = columnSource;
            this.messageHelper = messageHelper;
            log = logFactory(typeof(OdbcCustomQueryDlgViewModel));

            this.sampleDataCommand = sampleDataCommand;

            Execute = new RelayCommand(ExecuteQuery);
            Ok = new RelayCommand<IDialog>(SaveQuery);
            Cancel = new RelayCommand<IDialog>(CloseDialog);

            query = columnSource.Query;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The query.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Query
        {
            get { return query; }
            set { handler.Set(nameof(Query), ref query, value); }
        }

        /// <summary>
        /// The results of the query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DataTable Results
        {
            get { return results; }
            set
            {
                if (results != value)
                {
                    results?.Dispose();
                }

                handler.Set(nameof(Results), ref results, value);
            }
        }

        /// <summary>
        /// The execute command
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Execute { get; set; }

        /// <summary>
        /// The OK command
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Ok { get; set; }
        
        /// <summary>
        /// The cancel command
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Cancel { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ResultMessage
        {
            get { return resultMessage; }
            set { handler.Set(nameof(ResultMessage), ref resultMessage, value); }
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        private void ExecuteQuery()
        {
            Results = null;
            ResultMessage = string.Empty;

            try
            {
                Results = sampleDataCommand.Execute(dataSource, Query, NumberOfSampleResults);
                if (Results.Rows.Count==0)
                {
                    ResultMessage = "Query returned no results";
                }
                validQuery = true;
            }
            catch (ShipWorksOdbcException ex)
            {
                log.Error(ex.Message);
                messageHelper.ShowError(ex.Message);
                validQuery = false;
            }
        }

        /// <summary>
        /// Saves the query.
        /// </summary>
        /// <param name="odbcCustomQueryDlg">The ODBC custom query dialog.</param>
        private void SaveQuery(IDialog odbcCustomQueryDlg)
        {
            ExecuteQuery();

            if (validQuery)
            {
                columnSource.Query = Query;

                odbcCustomQueryDlg.DialogResult = true;
                CloseDialog(odbcCustomQueryDlg);
            }
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="odbcCustomQueryDlg">The ODBC custom query dialog.</param>
        private void CloseDialog(IDialog odbcCustomQueryDlg)
        {
            odbcCustomQueryDlg.Close();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            results?.Dispose();
        }
    }
}