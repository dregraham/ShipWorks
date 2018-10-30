using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.Controls.Customs;
using ShipWorks.OrderLookup.Controls.EmailNotifications;
using ShipWorks.OrderLookup.Controls.From;
using ShipWorks.OrderLookup.Controls.LabelOptions;
using ShipWorks.OrderLookup.Controls.Rating;
using ShipWorks.OrderLookup.Controls.Reference;
using ShipWorks.OrderLookup.Controls.ShipmentDetails;
using ShipWorks.OrderLookup.Controls.To;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Factory for creating OrderLookupPanels
    /// </summary>
    [Component]
    public class OrderLookupPanelFactory : IOrderLookupPanelFactory
    {
        /// <summary>s
        /// Get all of the panels using the given scope
        /// </summary>
        public IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> GetPanels(ILifetimeScope scope)
        {
            return new IOrderLookupPanelViewModel<IOrderLookupViewModel>[]
            {
                scope.Resolve<IOrderLookupPanelViewModel<IDetailsViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<ILabelOptionsViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IReferenceViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IEmailNotificationsViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IFromViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IToViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<IRatingViewModel>>(),
                scope.Resolve<IOrderLookupPanelViewModel<ICustomsViewModel>>()
            };
        }
    }
}
