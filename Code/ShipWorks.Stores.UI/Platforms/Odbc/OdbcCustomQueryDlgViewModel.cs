﻿using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Input;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View model for OdbcCustomQueryDlg
    /// </summary>
    public class OdbcCustomQueryDlgViewModel : IOdbcCustomQueryDlgViewModel, INotifyPropertyChanged
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private readonly IOdbcColumnSource columnSource;
        private readonly Func<Type, ILog> logFactory;
        private string query;
        private DataTable results;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private const int NumberOfSampleResults = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryDlgViewModel"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dbProviderFactory">The database provider factory.</param>
        /// <param name="columnSource">The column source.</param>
        /// <param name="logFactory">The log factory.</param>
        public OdbcCustomQueryDlgViewModel(IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcColumnSource columnSource, Func<Type, ILog> logFactory)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.columnSource = columnSource;
            this.logFactory = logFactory;

            sampleDataCommand = new OdbcSampleDataCommand(dbProviderFactory,
                logFactory(typeof(OdbcSampleDataCommand)));

            Execute = new RelayCommand(ExecuteQuery);
            Ok = new RelayCommand<OdbcCustomQueryDlg>(SaveQuery);
            results = new DataTable();

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
            set { handler.Set(nameof(Results), ref results, value); }
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
        /// Loads the specified custom query.
        /// </summary>
        /// <param name="customQuery">The custom query.</param>
        public void Load(string customQuery)
        {
            Query = customQuery;
        }

        /// <summary>
        /// The cancel command
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Cancel { get; set; }

        /// <summary>
        /// Executes the query.
        /// </summary>
        private void ExecuteQuery()
        {
            Results = sampleDataCommand.Execute(dataSource, Query, NumberOfSampleResults);
        }

        /// <summary>
        /// Saves the query.
        /// </summary>
        /// <param name="odbcCustomQueryDlg">The ODBC custom query dialog.</param>
        private void SaveQuery(OdbcCustomQueryDlg odbcCustomQueryDlg)
        {
            columnSource.Load(dataSource, logFactory(typeof(OdbcColumnSource)), Query, dbProviderFactory);
            odbcCustomQueryDlg.Close();
        }
    }
}