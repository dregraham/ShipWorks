using System.Collections.Generic;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Templates.Controls.DefaultPickListTemplate
{
    /// <summary>
    /// Represents the pick list printing service
    /// </summary>
    public interface IPickListPrintingService : IMainGridControlPipeline
    {
        /// <summary>
        /// Print a pick list using the given order keys
        /// </summary>
        void PrintPickList(IEnumerable<long> selectedOrderIDs);
    }
}