using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using Newtonsoft.Json;
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
        private readonly IUserSession userSession;
        private readonly ILog log;
        private readonly IEnumerable<IEnumerable<PanelInfo>> defaultLayout;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupLayout(IOrderLookupPanelFactory panelFactory, IUserSession userSession, Func<Type, ILog> createLogger)
        {
            this.panelFactory = panelFactory;
            this.userSession = userSession;

            log = createLogger(GetType());

            defaultLayout = new OrderLookupLayoutDefaults().GetDefaults();
        }

        /// <summary>
        /// Apply the layout to the orderLookupViewModel
        /// </summary>
        public void Apply(IMainOrderLookupViewModel orderLookupViewModel, ILifetimeScope scope)
        {
            IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> allPanels = panelFactory.GetPanels(scope);

            IEnumerable<IEnumerable<PanelInfo>> layout = GetLayout();

            orderLookupViewModel.LeftColumn = GetColumn(0, layout, allPanels);
            orderLookupViewModel.MiddleColumn = GetColumn(1, layout, allPanels);
            orderLookupViewModel.RightColumn = GetColumn(2, layout, allPanels);

            IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> missingPanels = allPanels
                            .Except(orderLookupViewModel.LeftColumn)
                            .Except(orderLookupViewModel.MiddleColumn)
                            .Except(orderLookupViewModel.RightColumn);

            foreach (IOrderLookupPanelViewModel<IOrderLookupViewModel> missingPanel in missingPanels)
            {
                orderLookupViewModel.RightColumn.Add(missingPanel);
            }
        }

        /// <summary>
        /// Retrieves the layout. If anything goes wrong, a default layout is returned.
        /// </summary>
        private IEnumerable<IEnumerable<PanelInfo>> GetLayout()
        {
            string serializedLayout = userSession.User.Settings.OrderLookupLayout;
            if (string.IsNullOrWhiteSpace(serializedLayout))
            {
                return defaultLayout;
            }

            IEnumerable<IEnumerable<PanelInfo>> layout;
            try
            {
                layout = JsonConvert.DeserializeObject<List<List<PanelInfo>>>(serializedLayout);
            }
            catch (JsonException ex)
            {
                log.Error(ex);
                return defaultLayout;
            }

            return layout;
        }

        /// <summary>
        /// Get a column. If the column index is not defined, return an empty collection
        /// </summary>
        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> GetColumn(int columnIndex, IEnumerable<IEnumerable<PanelInfo>> layout, IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> allPanels)
        {
            List<IOrderLookupPanelViewModel<IOrderLookupViewModel>> panels = new List<IOrderLookupPanelViewModel<IOrderLookupViewModel>>();

            if (layout.Count() > columnIndex)
            {
                List<PanelInfo> column = layout.ToArray()[columnIndex].ToList();
                foreach (PanelInfo panelInfo in column)
                {
                    IOrderLookupPanelViewModel<IOrderLookupViewModel> panelToAdd = allPanels.SingleOrDefault(p => p.Name == panelInfo.Name);
                    panelToAdd.Expanded = panelInfo.Expanded;
                    panels.Add(panelToAdd);
                }
            }

            return new ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>>(panels);
        }

        /// <summary>
        /// Save the layout
        /// </summary>
        public void Save(IMainOrderLookupViewModel orderLookupViewModel)
        {
            userSession.User.Settings.OrderLookupLayout = 
                JsonConvert.SerializeObject(new[] { orderLookupViewModel.LeftColumn, orderLookupViewModel.MiddleColumn, orderLookupViewModel.RightColumn });
        }
    }
}
