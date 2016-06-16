using System;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Wrapper for OdbcCommandBuilder. Needed for unit testing.
    /// </summary>
    public class ShipWorksOdbcCommandBuilder : IShipWorksOdbcCommandBuilder
    {
        private readonly OdbcCommandBuilder commandBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcCommandBuilder"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public ShipWorksOdbcCommandBuilder(OdbcDataAdapter adapter)
        {
            commandBuilder = new OdbcCommandBuilder(adapter);
        }

        /// <summary>
        /// Given an unquoted identifier in the correct catalog case, returns the correct
        /// quoted form of that identifier. This includes correctly escaping any embedded
        /// quotes in the identifier.
        /// </summary>
        /// <param name="unquotedIdentifier">The original unquoted identifier.</param>
        /// <returns>
        /// The quoted version of the identifier. Embedded quotes within the identifier are
        /// correctly escaped.
        /// </returns>
        public string QuoteIdentifier(string unquotedIdentifier)
        {
            return commandBuilder.QuoteIdentifier(unquotedIdentifier);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            commandBuilder.Dispose();
        }
    }
}