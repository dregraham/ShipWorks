using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.Controls.OrderLookup;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.Layout
{
    /// <summary>
    /// OrderLookupLayout
    /// </summary>
    [Component]
    public class OrderLookupLayout : IOrderLookupLayout
    {
        private readonly IOrderLookupPanelFactory panelFactory;
        private readonly IUserSession userSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupLayout(IOrderLookupPanelFactory panelFactory, IUserSession userSettings)
        {
            this.panelFactory = panelFactory;
            this.userSettings = userSettings;
        }

        /// <summary>
        /// Apply the layout to the orderLookupViewModel
        /// </summary>
        public void Apply(IMainOrderLookupViewModel orderLookupViewModel, ILifetimeScope scope)
        {
            IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> panels = panelFactory.GetPanels(scope);
        }

        /// <summary>
        /// Save the layout
        /// </summary>
        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
