using System;
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
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    /// <summary>
    /// View model for the OdbcImportParameterizedQueryControl
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OdbcImportParameterizedQueryControlViewModel : ViewModelBase
    {
        private DataTable queryResults;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private readonly IMessageHelper messageHelper;
        private string customQuery;
        private IOdbcDataSource dataSource;
        private string sampleParameterValue;
        private OdbcImportStrategy importStrategy;
        private const string DefaultOrderNumberParameter = "'0'";
        private readonly string DefaultDateParameter = $"'{DateTime.Today:d}'";

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcImportParameterizedQueryControlViewModel(IOdbcSampleDataCommand sampleDataCommand, IMessageHelper messageHelper)
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
                    $"Enter a query below, using a ? to represent the {parameterType} parameter. If you would like to test your query, you can use the default value or enter your own sample parameter.";
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
        public void Load(IOdbcDataSource odbcDataSource, OdbcImportStrategy odbcImportStrategy, string query)
        {
            dataSource = odbcDataSource;
            importStrategy = odbcImportStrategy;
            CustomQuery = query;

            SetSampleParameterValue();
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
            
            // Ensure we have a sample value to use
            SetSampleParameterValue();

            try
            {
                QueryResults = sampleDataCommand.Execute(dataSource, CustomQuery.Replace("?", SampleParameterValue), NumberOfSampleResults);

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

        /// <summary>
        /// Sets the Sample Parameter Value to the default if blank
        /// </summary>
        private void SetSampleParameterValue()
        {
            if (string.IsNullOrWhiteSpace(SampleParameterValue))
            {
                SampleParameterValue = importStrategy == OdbcImportStrategy.OnDemand
                    ? DefaultOrderNumberParameter
                    : DefaultDateParameter;
            }
        }
    }
}