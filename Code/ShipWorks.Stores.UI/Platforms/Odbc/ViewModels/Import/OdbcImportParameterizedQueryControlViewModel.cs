using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    [Component(RegistrationType.Self)]
    public class OdbcImportParameterizedQueryControlViewModel : ViewModelBase
    {
        private DataTable queryResults;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private readonly Func<string, IDialog> dialogFactory;
        private string customQuery;
        private IOdbcDataSource dataSource;
        private string sampleParameterValue;
        private string parameterizedQueryInfo;
        private OdbcImportStrategy importStrategy;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcImportParameterizedQueryControlViewModel(IOdbcSampleDataCommand sampleDataCommand, Func<string, IDialog> dialogFactory)
        {
            this.sampleDataCommand = sampleDataCommand;
            this.dialogFactory = dialogFactory;

            ExecuteQueryCommand = new RelayCommand(() => ExecuteQuery(),
                                                   () => !string.IsNullOrWhiteSpace(CustomQuery) &&
                                                         !string.IsNullOrWhiteSpace(SampleParameterValue));
        }

        /// <summary>
        /// Command that executes the query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ExecuteQueryCommand { get; set; }

        /// <summary>
        /// Info regarding parameterized queries
        /// </summary>
        public string ParameterizedQueryInfo
        {
            get
            {
                string parameterType = importStrategy == OdbcImportStrategy.OnDemand ?
                    "Order Number" :
                    "Last Modified Date";

                return
                    $"Enter a query below, using a ? to represent the {parameterType} parameter. Please also enter a sample parameter value to test the query with, below the query editor.";
            }
        }

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
        /// The sample value to use in place of the parameter for testing the custom query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SampleParameterValue
        {
            get => sampleParameterValue;
            set => Set(ref sampleParameterValue, value);
        }

        /// <summary>
        /// Load the odbc data source into the view model
        /// </summary>
        public void Load(IOdbcDataSource odbcDataSource, OdbcImportStrategy odbcImportStrategy)
        {
            dataSource = odbcDataSource;
            importStrategy = odbcImportStrategy;
            
            IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
            warningDlg.ShowDialog();
        }

        /// <summary>
        /// Validate the custom query
        /// </summary>
        public Result ValidateQuery()
        {
            Result queryResult;

            StringBuilder errors = new StringBuilder();
            
            if (string.IsNullOrWhiteSpace(CustomQuery))
            {
                errors.AppendLine("Please enter a valid query before continuing to the next page.");
            }

            if (string.IsNullOrWhiteSpace(SampleParameterValue))
            {
                errors.AppendLine("Please enter a sample parameter value to test the query with.");
            }

            queryResult = errors.Length == 0 ? 
                ExecuteQuery() : 
                Result.FromError(errors.ToString());

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
                QueryResults = sampleDataCommand.Execute(dataSource, CustomQuery.Replace("?", SampleParameterValue), NumberOfSampleResults);

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