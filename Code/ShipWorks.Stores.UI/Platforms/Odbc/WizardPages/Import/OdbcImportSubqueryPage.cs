using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import
{
    public partial class OdbcImportSubqueryPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly Func<OdbcImportSubqueryControlViewModel> viewModelFactory;
        private OdbcImportSubqueryControlViewModel viewModel;
        private readonly IOdbcDataSourceService dataSourceService;
        private readonly IMessageHelper messageHelper;
        private OdbcStoreEntity store;

        public OdbcImportSubqueryPage(Func<OdbcImportSubqueryControlViewModel> viewModelFactory, IOdbcDataSourceService dataSourceService, IMessageHelper messageHelper)
        {
            this.viewModelFactory = viewModelFactory;
            this.dataSourceService = dataSourceService;
            this.messageHelper = messageHelper;

            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
            StepBack += OnBack;
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 2;
        
        /// <summary>
        /// Called when [stepping into].
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();
            
            if (store.ImportColumnSourceType != (int) OdbcColumnSourceType.CustomSubQuery)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
                return;
            }

            viewModel = viewModelFactory();
            viewModel.Load(dataSourceService.GetImportDataSource(store));

            subqueryControl.DataContext = viewModel;
        }

        /// <summary>
        /// Validate the query and if valid, save it to the ODBC Store. If invalid, display error message
        /// </summary>
        private void OnNext(object sender, WizardStepEventArgs e)
        {
            Result queryResult = viewModel.ValidateQuery();
            
            if (queryResult.Failure)
            {
                e.NextPage = this;
                messageHelper.ShowError(this, queryResult.Message);
                return;
            }

            store.ImportColumnSource = viewModel.CustomQuery;
        }

        /// <summary>
        /// Save the query without validation. Validation will happen when they come back to this page and click next.
        /// </summary>
        private void OnBack(object sender, WizardStepEventArgs e)
        {
            store.ImportColumnSource = viewModel.CustomQuery;
        }
    }
}
