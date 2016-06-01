using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for getting a TangoWebClient.
    /// </summary>
    public interface ITangoWebClientFactory
    {
        /// <summary>
        /// Creates an instance of ITangoWebClient. This allows us to return a mock/fake instance of an 
        /// ITangoWebClient for Interapptive users that have the appropriate registry setting configured;
        /// otherwise the normal web client will be returned.
        /// </summary>
        ITangoWebClient CreateWebClient();
    }
}
