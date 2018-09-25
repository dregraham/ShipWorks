using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.Users;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.OrderLookup.ShipmentHistory.Controls
{
    /// <summary>
    /// Grid for displaying shipment history
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ShipmentHistoryGrid : SingleSelectPanelBase
    {
        private const long ReloadValue = 0;
        private const long FilterValue = 1;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly Func<IUserSession> getUserSession;
        private List<ShipmentEntity> entities;
        private string searchText;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryGrid() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryGrid(ISqlAdapterFactory sqlAdapterFactory, IDateTimeProvider dateTimeProvider, Func<IUserSession> getUserSession) : this()
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.getUserSession = getUserSession;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Handle the load event
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            entityGrid.Dock = DockStyle.Fill;
            addLink.Visible = false;
        }

        /// <summary>
        /// Perform a search using the given text
        /// </summary>
        public void Search(string text)
        {
            searchText = text;

            ChangeContent(FilterValue);
        }

        /// <summary>
        /// Reload the contents of the grid
        /// </summary>
        public void Reload()
        {
            searchText = string.Empty;

            ChangeContent(ReloadValue);
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Customers };

        /// <summary>
        /// Create the gateway used by the underlying entity grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID) =>
            new LocalCollectionEntityGateway<ShipmentEntity>(entityID == FilterValue ? FilterEntities() : LoadEntities());

        /// <summary>
        /// Load the entities from the database
        /// </summary>
        /// <returns></returns>
        private List<ShipmentEntity> LoadEntities()
        {
            var currentUser = getUserSession();

            var query = new QueryFactory();

            var shipmentQuery = query.Shipment
                .Where(ShipmentFields.Processed == true)
                .AndWhere(ShipmentFields.ProcessedDate >= dateTimeProvider.GetUtcNow().Date)
                .AndWhere(ShipmentFields.ProcessedUserID == currentUser.User.UserID)
                .AndWhere(ShipmentFields.ProcessedComputerID == currentUser.Computer.ComputerID)
                .WithPath(ShipmentEntity.PrefetchPathOrder);

            return entities = Using(
                    sqlAdapterFactory.Create(),
                    x => x.FetchQuery(shipmentQuery))
                .OfType<ShipmentEntity>()
                .ToList();
        }

        /// <summary>
        /// Filter the list of entities
        /// </summary>
        private List<ShipmentEntity> FilterEntities() =>
            entities.Where(x =>
                    x.TrackingNumber?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    x.Order.OrderNumberComplete.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
    }
}
