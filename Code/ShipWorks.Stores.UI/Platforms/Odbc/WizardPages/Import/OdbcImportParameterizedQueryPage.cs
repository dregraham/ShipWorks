﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import
{
    /// <summary>
    /// Wizard page for entering a parameterized Odbc import query
    /// </summary>
    public partial class OdbcImportParameterizedQueryPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly Func<OdbcImportParameterizedQueryControlViewModel> viewModelFactory;
        private OdbcImportParameterizedQueryControlViewModel viewModel;
        private readonly IOdbcDataSourceService dataSourceService;
        private OdbcStoreEntity store;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcImportParameterizedQueryPage(Func<OdbcImportParameterizedQueryControlViewModel> viewModelFactory, 
                                                IOdbcDataSourceService dataSourceService)
        {
            this.viewModelFactory = viewModelFactory;
            this.dataSourceService = dataSourceService;

            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
            StepBack += OnBack;
        }
        
        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 3;
        
        /// <summary>
        /// Called when [stepping into].
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();
            
            if (store.ImportColumnSourceType != (int) OdbcColumnSourceType.CustomParameterizedQuery ||
                !store.IsMappingRequired)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
                return;
            }

            viewModel = viewModelFactory();
            viewModel.Load(dataSourceService.GetImportDataSource(store), (OdbcImportStrategy) store.ImportStrategy, store.ImportColumnSource);

            parameterizedQueryControl.DataContext = viewModel;
        }

        /// <summary>
        /// Validate the query and if valid, save it to the ODBC Store. If invalid, display error message
        /// </summary>
        private void OnNext(object sender, WizardStepEventArgs e)
        {
            bool isQueryValid = viewModel.ValidateQuery();
            
            if (!isQueryValid)
            {
                e.NextPage = this;
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
