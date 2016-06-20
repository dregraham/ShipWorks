using System.Data.Common;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    ///
    /// </summary>
    public class ShipWorksOdbcDataAdapter : IShipWorksOdbcDataAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcDataAdapter"/> class.
        /// </summary>
        /// <param name="selectCommandText">The select command text.</param>
        /// <param name="selectConnection">The select connection.</param>
        public ShipWorksOdbcDataAdapter(string selectCommandText, DbConnection selectConnection)
        {
            Adapter = new OdbcDataAdapter(selectCommandText, (OdbcConnection) selectConnection);
        }

        /// <summary>
        /// Gets the actual OdbcDataAdapter.
        /// </summary>
        public OdbcDataAdapter Adapter { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Adapter.Dispose();
        }
    }
}