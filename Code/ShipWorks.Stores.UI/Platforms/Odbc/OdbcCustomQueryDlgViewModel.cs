using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Input;
using ShipWorks.Core.UI;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcCustomQueryDlgViewModel : IOdbcCustomQueryDlgViewModel
    {
        private string query;
        private DataTable results;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        public OdbcCustomQueryDlgViewModel()
        {
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
    }
}