using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private OrderLookupLayoutDefaults defaults;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupLayout(IOrderLookupPanelFactory panelFactory, IUserSession userSession, Func<Type, ILog> createLogger)
        {
            this.panelFactory = panelFactory;
            this.userSession = userSession;

            log = createLogger(GetType());

            defaults = new OrderLookupLayoutDefaults();
        }

        /// <summary>
        /// Apply the layout to the orderLookupViewModel
        /// </summary>
        public void Apply(IMainOrderLookupViewModel orderLookupViewModel, ILifetimeScope scope)
        {
            IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> allPanels = panelFactory.GetPanels(scope);

            (GridLength leftColumnWidth, GridLength middleColumnWidth, IEnumerable<IEnumerable<PanelInfo>> panels) = GetLayout();

            orderLookupViewModel.LeftColumn = GetColumn(0, panels, allPanels);
            orderLookupViewModel.MiddleColumn = GetColumn(1, panels, allPanels);
            orderLookupViewModel.RightColumn = GetColumn(2, panels, allPanels);

            IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> missingPanels = allPanels
                            .Except(orderLookupViewModel.LeftColumn)
                            .Except(orderLookupViewModel.MiddleColumn)
                            .Except(orderLookupViewModel.RightColumn);

            foreach (IOrderLookupPanelViewModel<IOrderLookupViewModel> missingPanel in missingPanels)
            {
                orderLookupViewModel.RightColumn.Add(missingPanel);
            }

            orderLookupViewModel.LeftColumnWidth = leftColumnWidth;
            orderLookupViewModel.MiddleColumnWidth = middleColumnWidth;
        }

        /// <summary>
        /// Retrieves the layout. If anything goes wrong, a default layout is returned.
        /// </summary>
        private (GridLength, GridLength, IEnumerable<IEnumerable<PanelInfo>>) GetLayout()
        {
            string serializedLayout = userSession.User.Settings.OrderLookupLayout;
            if (string.IsNullOrWhiteSpace(serializedLayout))
            {
                return (defaults.LeftColumnWidth, defaults.MiddleColumnWidth, defaults.GetDefaults());
            }

            try
            {
                JObject jLayout = JObject.Parse(serializedLayout);
                GridLength leftColumnWidth = JsonConvert.DeserializeObject<GridLength>((string)jLayout.SelectToken("LeftColumnWidth"));
                GridLength middleColumnWidth = JsonConvert.DeserializeObject<GridLength>((string) jLayout.SelectToken("MiddleColumnWidth"));
                List<List<PanelInfo>> panels = JsonConvert.DeserializeObject<List<List<PanelInfo>>>((string) jLayout.SelectToken("Panels"));
                return (leftColumnWidth, middleColumnWidth, panels);
            }
            catch (JsonException ex)
            {
                log.Error(ex);
                return (defaults.LeftColumnWidth, defaults.MiddleColumnWidth, defaults.GetDefaults());
            }
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
            string panels = JsonConvert.SerializeObject(new[] { orderLookupViewModel.LeftColumn, orderLookupViewModel.MiddleColumn, orderLookupViewModel.RightColumn });

            JObject layout = new JObject(
                            new JProperty("LeftColumnWidth", JsonConvert.SerializeObject(orderLookupViewModel.LeftColumnWidth)),
                            new JProperty("MiddleColumnWidth", JsonConvert.SerializeObject(orderLookupViewModel.MiddleColumnWidth)),
                            new JProperty("Panels", panels));

            userSession.User.Settings.OrderLookupLayout = JsonConvert.SerializeObject(layout);
        }
    }
}
