using System.Data;
using System.Windows.Input;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View model for OdbcCustomQueryDlg
    /// </summary>
    public interface IOdbcCustomQueryDlgViewModel
    {
        /// <summary>
        /// The query.
        /// </summary>
        string Query { get; set; }

        /// <summary>
        /// The results of the query
        /// </summary>
        DataTable Results { get; set; }

        /// <summary>
        /// The execute command
        /// </summary>
        ICommand Execute { get; set; }

        /// <summary>
        /// The OK command
        /// </summary>
        ICommand Ok { get; set; }
    }
}