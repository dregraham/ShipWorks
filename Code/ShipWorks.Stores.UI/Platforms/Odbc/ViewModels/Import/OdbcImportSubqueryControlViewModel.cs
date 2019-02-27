using System.Data;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    /// <summary>
    /// View model for the OdbcImportSubqueryControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OdbcImportSubqueryControlViewModel : ViewModelBase
    {
        private DataTable queryResults;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private readonly IMessageHelper messageHelper;
        private string customQuery;
        private IOdbcDataSource dataSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcImportSubqueryControlViewModel(IOdbcSampleDataCommand sampleDataCommand, IMessageHelper messageHelper)
        {
            this.sampleDataCommand = sampleDataCommand;
            this.messageHelper = messageHelper;

            ExecuteQueryCommand = new RelayCommand(() => ValidateQuery());
        }

        /// <summary>
        /// Command that executes the query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ExecuteQueryCommand { get; set; }

        /// <summary>
        /// The query results.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DataTable QueryResults
        {
            get => queryResults;
            set => Set(ref queryResults, value);
        }

        /// <summary>
        /// Message to indicate failed query execution or no results
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ResultMessage
        {
            get => resultMessage;
            set => Set(ref resultMessage, value);
        }
        
        /// <summary>
        /// the custom query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CustomQuery
        {
            get => customQuery;
            set => Set(ref customQuery, value);
        }

        /// <summary>
        /// Load the odbc data source into the view model
        /// </summary>
        public void Load(IOdbcDataSource odbcDataSource, string query)
        {
            dataSource = odbcDataSource;
            CustomQuery = query;
        }
        
        /// <summary>
        /// Validate the custom query, returning true if valid and false if not
        /// </summary>
        public bool ValidateQuery()
        {
            if (string.IsNullOrWhiteSpace(CustomQuery))
            {
                messageHelper.ShowError("Please enter a valid query.");
                return false;
            }

            return ExecuteQuery();
        }
        
        /// <summary>
        /// Executes the query, returning true if successful and false if not
        /// </summary>
        private bool ExecuteQuery()
        {
            QueryResults = null;
            ResultMessage = string.Empty;

            try
            {
                QueryResults = sampleDataCommand.Execute(dataSource, CustomQuery, NumberOfSampleResults);

                if (QueryResults.Rows.Count == 0)
                {
                    ResultMessage = "Query returned no results";
                }

                return true;
            }
            catch (ShipWorksOdbcException ex)
            {
                ResultMessage = "Query returned an error";
                messageHelper.ShowError(ex.Message);
                return false;
            }
        }
    }
}