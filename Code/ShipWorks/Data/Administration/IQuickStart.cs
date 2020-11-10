using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Administration
{
    [Service]
    public interface IQuickStart : IDialog
    {
        bool ShouldShow { get; }
    }
}
