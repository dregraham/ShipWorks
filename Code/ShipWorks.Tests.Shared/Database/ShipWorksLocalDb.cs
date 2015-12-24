using SQL.LocalDB.Test;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Temp local db that uses the ShipWorks connection string
    /// </summary>
    public class ShipWorksLocalDb : TempLocalDb
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksLocalDb(string databaseName) : base(databaseName)
        {

        }

        /// <summary>
        /// Get a ShipWorks specific version of the connection string
        /// </summary>
        public override string ConnectionString =>
            base.ConnectionString +
                    ";Connect Timeout=10;Application Name=ShipWorks;Workstation ID=0000100001;Transaction Binding=\"Explicit Unbind\"";
    }
}
