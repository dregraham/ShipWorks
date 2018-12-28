using System.Collections.Generic;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Templates.Controls.DefaultPickListTemplate
{
    public interface IPickListPrintingService : IMainGridControlPipeline
    {
        void PrintPickList(IEnumerable<long> selectedOrderIDs);
    }
}