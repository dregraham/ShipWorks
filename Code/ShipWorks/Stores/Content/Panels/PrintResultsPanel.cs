using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Diagnostics;
using ShipWorks.Data.Grid;
using ShipWorks.UI;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Templates.Printing;
using Interapptive.Shared.UI;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// User control for displaying the payment details of an order
    /// </summary>
    public partial class PrintResultsPanel : SingleSelectPanelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintResultsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.PrintResultEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders, FilterTarget.Customers }; }
        }

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket(PrintResultFields.RelatedObjectID == entityID);

            if (EntityUtility.GetEntityType(entityID) == EntityType.CustomerEntity)
            {
                FieldCompareSetPredicate predicate = new FieldCompareSetPredicate(
                    PrintResultFields.RelatedObjectID, null, OrderFields.OrderID, null, SetOperator.In, OrderFields.CustomerID == entityID);

                bucket.PredicateExpression.AddWithOr(predicate);
            }

            return new QueryableEntityGateway(EntityType.PrintResultEntity, bucket);
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = (EntityGridRow) e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The print data is not yet loaded.");
                return;
            }

            long entityID = row.EntityID.Value;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.View)
            {
                ViewPrintResult(entityID);
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            ViewPrintResult(entityID);
        }

        /// <summary>
        /// View menu has been opened
        /// </summary>
        private void OnView(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                ViewPrintResult(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// View the result of a print job
        /// </summary>
        private void ViewPrintResult(long printResultID)
        {
            using (PrintResultViewerDlg dlg = new PrintResultViewerDlg(printResultID))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuView.Enabled = entityGrid.Selection.Count == 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;
        }
    }
}
