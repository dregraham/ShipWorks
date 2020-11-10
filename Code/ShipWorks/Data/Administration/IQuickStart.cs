using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Administration
{
    [Service]
    public interface IQuickStart : IDialog
    {
        /// <summary>
        /// Whether or not this dialog should show
        /// </summary>
        bool ShouldShow { get; }
    }
}
