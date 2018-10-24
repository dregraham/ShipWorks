using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        string defaultLayout = "[[{\"Name\":\"IFromViewModel\",\"Expanded\":false},{\"Name\":\"IToViewModel\",\"Expanded\":true}],[{\"Name\":\"IDetailsViewModel\",\"Expanded\":true},{\"Name\":\"ILabelOptionsViewModel\",\"Expanded\":false},{\"Name\":\"IReferenceViewModel\",\"Expanded\":false},{\"Name\":\"IEmailNotificationsViewModel\",\"Expanded\":false}],[{\"Name\":\"ICustomsViewModel\",\"Expanded\":true},{\"Name\":\"IRatingViewModel\",\"Expanded\":true}]]";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupLayout(IOrderLookupPanelFactory panelFactory, IUserSession userSession, Func<Type, ILog> createLogger)
        {
            this.panelFactory = panelFactory;
            this.userSession = userSession;

            log = createLogger(GetType());
        }

        /// <summary>
        /// Apply the layout to the orderLookupViewModel
        /// </summary>
        public void Apply(IMainOrderLookupViewModel orderLookupViewModel, ILifetimeScope scope)
        {

            IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> allPanels = panelFactory.GetPanels(scope);

            List<List<PanelInfo>> layout = GetLayout();

            orderLookupViewModel.LeftColumn = GetColumn(0, layout, allPanels);
            orderLookupViewModel.MiddleColumn = GetColumn(1, layout, allPanels);
            orderLookupViewModel.RightColumn = GetColumn(2, layout, allPanels);
        }

        /// <summary>
        /// Retrieves the layout. If anything goes wrong, a default layout is returned.
        /// </summary>
        private List<List<PanelInfo>> GetLayout()
        {
            string serializedLayout = userSession.User.Settings.GridMenuLayout;
            if (string.IsNullOrWhiteSpace(serializedLayout))
            {
                serializedLayout = defaultLayout;
            }

            List<List<PanelInfo>> layout;
            try
            {
                layout = JsonConvert.DeserializeObject<List<List<PanelInfo>>>(serializedLayout);
            }
            catch (JsonException ex)
            {
                log.Error(ex);
                layout = JsonConvert.DeserializeObject<List<List<PanelInfo>>>(defaultLayout);
            }

            return layout;
        }

        /// <summary>
        /// Get a column. If the column index is not defined, return an empty collection
        /// </summary>
        private ObservableCollection<IOrderLookupPanelViewModel<IOrderLookupViewModel>> GetColumn(int columnIndex, List<List<PanelInfo>> layout, IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> allPanels)
        {
            List<IOrderLookupPanelViewModel<IOrderLookupViewModel>> panels = new List<IOrderLookupPanelViewModel<IOrderLookupViewModel>>();

            if (layout.Count > columnIndex)
            {
                List<PanelInfo> column = layout[columnIndex];
                foreach (var panelInfo in column)
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
            userSession.User.Settings.GridMenuLayout = 
                JsonConvert.SerializeObject(new[] { orderLookupViewModel.LeftColumn, orderLookupViewModel.MiddleColumn, orderLookupViewModel.RightColumn });
        }

        /// <summary>
        /// Panel Information
        /// </summary>
        private struct PanelInfo
        {
            public string Name { get; set; }
            public bool Expanded { get; set; }
        }
    }
}
