using System.Data;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    [Component(RegistrationType.Self)]
    public class OdbcImportSubqueryControlViewModel : ViewModelBase
    {
        private DataTable queryResults;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private string customQuery;
        private IOdbcDataSource dataSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcImportSubqueryControlViewModel(IOdbcSampleDataCommand sampleDataCommand)
        {
            this.sampleDataCommand = sampleDataCommand;
            
            ExecuteQueryCommand = new RelayCommand(() => ExecuteQuery());
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
        public void Load(IOdbcDataSource odbcDataSource)
        {
            dataSource = odbcDataSource;
        }
        
        /// <summary>
        /// Validate the custom query
        /// </summary>
        public Result ValidateQuery()
        {
            Result queryResult = string.IsNullOrWhiteSpace(CustomQuery) ? 
                Result.FromError("Please enter a valid query before continuing to the next page.") : 
                ExecuteQuery();

            return queryResult;
        }
        
        /// <summary>
        /// Executes the query.
        /// </summary>
        private Result ExecuteQuery()
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

                return Result.FromSuccess();
            }
            catch (ShipWorksOdbcException ex)
            {
                return Result.FromError(ex.Message);
            }
        }
    }
}