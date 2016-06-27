using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcCustomQueryDlgViewModel : IOdbcCustomQueryDlgViewModel
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

        public OdbcCustomQueryDlgViewModel(IOdbcDataSource dataSource, IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcSampleDataCommand sampleDataCommand, IOdbcColumnSource columnSource, Func<Type, ILog> logFactory)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.sampleDataCommand = sampleDataCommand;
            this.columnSource = columnSource;
            this.logFactory = logFactory;

            Execute = new RelayCommand(ExecuteQuery);
            Ok = new RelayCommand(SaveQuery);

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        [Obfuscation(Exclude = true)]
        public string Query
        {
            get { return query; }
            set { handler.Set(nameof(Query), ref query, value); }
        }

        [Obfuscation(Exclude = true)]
        public DataTable Results
        {
            get { return results; }
            set { handler.Set(nameof(Results), ref results, value); }
        }

        [Obfuscation(Exclude = true)]
        public ICommand Execute { get; set; }

        [Obfuscation(Exclude = true)]
        public ICommand Ok { get; set; }

        [Obfuscation(Exclude = true)]
        public ICommand Cancel { get; set; }

        private void ExecuteQuery()
        {
            Results = sampleDataCommand.Execute(dataSource, Query);
        }

        private void SaveQuery()
        {
            columnSource.Load(dataSource, logFactory(typeof(OdbcColumnSource)), Query, dbProviderFactory);
        }
    }
}