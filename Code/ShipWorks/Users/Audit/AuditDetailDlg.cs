using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Window that shows a list of events that occurred for a given top-level Audit entry
    /// </summary>
    public partial class AuditDetailDlg : Form
    {
        static Guid gridSettingsKey = new Guid("{484550F6-E2D6-4e5d-A7E9-B30D77168188}");

        AuditEntity audit;
        EntityCacheEntityProvider entityProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditDetailDlg(AuditEntity audit)
        {
            InitializeComponent();

            this.audit = audit;

            WindowStateSaver wss = new WindowStateSaver(this);
            wss.ManageSplitter(splitContainer);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.AuditChangeEntity);
            prefetch.Add(AuditChangeEntity.PrefetchPathAuditChangeDetails);

            entityProvider = new EntityCacheEntityProvider(EntityType.AuditChangeEntity, prefetch, false);
            sandGrid.Rows.Clear();

            LoadChangesGrid();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (entityProvider != null)
                {
                    entityProvider.Dispose();
                    entityProvider = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Load the grid on the left with the changes
        /// </summary>
        private void LoadChangesGrid()
        {
            // Prepare for paging
            entityGrid.InitializeGrid();

            // Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.AuditChanges, layout =>
                {
                    layout.DefaultSortColumnGuid = layout.AllColumns[AuditChangeFields.EntityID].Definition.ColumnGuid;
                    layout.DefaultSortOrder = ListSortDirection.Ascending;

                    layout.LastSortColumnGuid = layout.DefaultSortColumnGuid;
                    layout.LastSortOrder = layout.DefaultSortOrder;
                }));
            entityGrid.SaveColumnsOnClose(this);

            // Load the rows
            entityGrid.OpenGateway(new QueryableEntityGateway(entityProvider, new RelationPredicateBucket(AuditChangeFields.AuditID == audit.AuditID)));

            if (entityGrid.Rows.Count > 0)
            {
                entityGrid.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Selected audit row is changing
        /// </summary>
        private void OnChangeSelectedAuditChange(object sender, SelectionChangedEventArgs e)
        {
            sandGrid.Rows.Clear();

            if (entityGrid.Selection.Count == 0)
            {
                return;
            }

            // Get the audit change and details
            AuditChangeEntity change = (AuditChangeEntity) entityProvider.GetEntity(entityGrid.Selection.Keys.First(), true);

            bool isDelete = change.ChangeType == (int) AuditChangeType.Delete;
            bool isInsert = change.ChangeType == (int) AuditChangeType.Insert;

            gridColumnAfter.Visible = !isDelete;
            gridColumnAfter.HeaderText = isInsert ? "Added Values" : "After";

            gridColumnBefore.Visible = !isInsert;
            gridColumnBefore.HeaderText = isDelete ? "Deleted Values" : "Before";

            // Go through and add a detail row for each one
            foreach (AuditChangeDetailEntity detail in change.AuditChangeDetails)
            {
                // If its hidden, don't do it at all
                if (detail.DisplayFormat == AuditDisplayFormat.Hidden)
                {
                    continue;
                }

                // Create the label
                string label = GetDetailLabelText(change, detail);

                string oldValue =
                        AuditDisplayFormat.FormatAuditValue(
                            detail.TextOld != null ? detail.TextOld : detail.VariantOld,
                            detail.DisplayFormat);

                string newValue =
                        AuditDisplayFormat.FormatAuditValue(
                            detail.TextNew != null ? detail.TextNew : detail.VariantNew,
                            detail.DisplayFormat);

                GridRow row = new GridRow(new string[] { label, oldValue + "  ", newValue + "  " });
                row.Height = 0;

                sandGrid.Rows.Add(row);
            }
        }

        /// <summary>
        /// Get the label for the given detail row
        /// </summary>
        private string GetDetailLabelText(AuditChangeEntity change, AuditChangeDetailEntity detail)
        {
            string text = detail.DisplayName;

            // Strip "ID"
            if (text.EndsWith("ID"))
            {
                text = text.Substring(0, text.Length - 2);
            }

            // Remove "Dims"
            if (text.StartsWith("Dims"))
            {
                text = text.Replace("Dims", "");
            }

            StringBuilder sb = new StringBuilder();
            foreach (char thisChar in text)
            {
                if (sb.Length > 0)
                {
                    char lastChar = sb[sb.Length - 1];

                    if (char.IsLower(lastChar) && !char.IsLower(thisChar))
                    {
                        sb.Append(' ');
                    }
                }

                sb.Append(thisChar);
            }
            text = sb.ToString();

            EntityType changeEntity = EntityUtility.GetEntityType(change.EntityID);

            // Orders and Customers special case address processing
            if (changeEntity == EntityType.OrderEntity || changeEntity == EntityType.CustomerEntity)
            {
                if (text.StartsWith("Bill "))
                {
                    text = text.Substring(5) + " (Billing)";
                }

                if (text.StartsWith("Ship "))
                {
                    text = text.Substring(5) + " (Shipping)";
                }
            }

            // Shipments special case address processing
            if (changeEntity == EntityType.ShipmentEntity)
            {
                if (text.StartsWith("Ship ") && !text.Contains("Date"))
                {
                    text = text.Substring(5) + " (To)";
                }

                if (text.StartsWith("Origin "))
                {
                    text = text.Substring(7) + " (From)";
                }
            }

            return text + ":  ";
        }
    }
}
