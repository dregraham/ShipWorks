using System.Collections.Generic;
using ShipWorks.ApplicationCore.Interaction;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Factory for creating Walmart online update instance commands
    /// </summary>
    public interface IWalmartOnlineUpdateInstanceCommands
    {
        /// <summary>
        /// Creates the online update instance commands.
        /// </summary>
        IEnumerable<MenuCommand> Create();
    }
}