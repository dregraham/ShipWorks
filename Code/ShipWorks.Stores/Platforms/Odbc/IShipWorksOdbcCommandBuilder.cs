using System;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public interface IShipWorksOdbcCommandBuilder : IDisposable
    {
        /// <summary>
        /// Given an unquoted identifier in the correct catalog case, returns the correct
        /// quoted form of that identifier. This includes correctly escaping any embedded
        /// quotes in the identifier.
        /// </summary>
        /// <param name="unquotedIdentifier">The original unquoted identifier.</param>
        /// <returns>The quoted version of the identifier. Embedded quotes within the identifier are
        /// correctly escaped.
        /// </returns>
        string QuoteIdentifier(string unquotedIdentifier);
    }
}