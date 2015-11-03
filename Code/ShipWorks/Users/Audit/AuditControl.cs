using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.HelperClasses;
using Divelements.SandGrid;
using ShipWorks.Data.Grid;
using ShipWorks.UI.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Users.Security;
using ShipWorks.Properties;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using System.Diagnostics;
using System.Threading;
using log4net;
using ShipWorks.ApplicationCore.Crashes;
using System.Globalization;
using Interapptive.Shared;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Caching;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Control that allows for viewing and searching the audit
    /// </summary>
    public partial class AuditControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AuditControl));

        // Locked criteria.  Like on the User Editor window, you'd lock the active user in place.
        Predicate lockedUserCritiera = null;
        Predicate lockedSearchTextCriteria = null;

        // The current date condition
        DateCondition dateCondition = new OrderDateCondition();

        // Search critiera
        Predicate searchTextCriteria = null;
        Predicate searchOptionsCriteria = null;
        object searchCriteriaLock = new object();

        // Set when the user has changed what's typed in the Related To search box
        AutoResetEvent searchChangedEvent = null;
        volatile bool searchTerminated = false;
        Image searchImageReady = null;

        // Used to be the entity cache, monitor for changes, and notify us when the grid needs refreshed
        EntityCacheEntityProvider entityProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuditControl()
        {
            InitializeComponent();

            buttonSearching.Visible = relatedToBox.Checked;
            searchImageReady = buttonSearching.ImageStates.ImageDisabled;
        }

        /// <summary>
        /// Initialze the control and grid
        /// </summary>
        public void Initialize(Guid gridSettingsKey, Action<GridColumnLayout> layoutInitializer)
        {
            // Create the entity provider for caching and refreshing
            entityProvider = new EntityCacheEntityProvider(EntityType.AuditEntity);
            entityProvider.EntityChangesDetected += new EntityCacheChangeMonitoredChangedEventHandler(OnEntityProviderChangeDetected);

            // Initialize the grid
            entityGrid.InitializeGrid();

            // Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.Audit, layout =>
                {
                    layout.DefaultSortColumnGuid = layout.AllColumns[AuditFields.Date].Definition.ColumnGuid;
                    layout.DefaultSortOrder = ListSortDirection.Descending;

                    if (layoutInitializer != null)
                    {
                        layoutInitializer(layout);
                    }
                }));
            entityGrid.SaveColumnsOnClose((Form) TopLevelControl);

            LoadUserCombo();
            LoadComputerCombo();
            LoadReasonCombo();
            LoadActionCombo();
            LoadDateEditor();

            UpdateSearchOptionsCriteria();

            // Start the refresh timer.
            timer.Interval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Start();

            // Create the gating event for the search thread
            searchChangedEvent = new AutoResetEvent(false);

            // Create the search thread
            Thread searchThread = new Thread(ExceptionMonitor.WrapThread(ThreadRelatedToCriteria));
            searchThread.IsBackground = true;
            searchThread.Name = "Related To Search";
            searchThread.Start();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                searchTerminated = true;
                searchChangedEvent.Set();

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
        /// The entity provider detected changes to the underlying data.  The grid needs updated.
        /// </summary>
        private void OnEntityProviderChangeDetected(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
        {
            if (e.Inserted.Count + e.Deleted.Count > 0)
            {
                entityGrid.ReloadGridRows();
            }
            else
            {
                entityGrid.UpdateGridRows();
            }
        }

        /// <summary>
        /// Load the user combobox
        /// </summary>
        private void LoadUserCombo()
        {
            userCombo.SelectedIndexChanged -= OnSearchValueChanged;

            userCombo.Items.Add(new ImageComboBoxItem(SuperUser.DisplayName, SuperUser.UserID, Resources.sw_cubes_16));

            foreach (UserEntity user in UserManager.GetUsers(true))
            {
                userCombo.Items.Add(new ImageComboBoxItem(user.Username + (user.IsDeleted ? " (Deleted)" : ""), user.UserID, user.IsDeleted ? Resources.user_deleted_16 : Resources.user_16));
            }

            userCombo.SelectedIndexChanged += OnSearchValueChanged;
        }

        /// <summary>
        /// Load the combobox for choosing which computer the edit was on
        /// </summary>
        private void LoadComputerCombo()
        {
            computerCombo.SelectedIndexChanged -= OnSearchValueChanged;

            computerCombo.DisplayMember = "Key";
            computerCombo.ValueMember = "Value";
            computerCombo.DataSource = ComputerManager.Computers.Select(c => new KeyValuePair<string, long>(c.Name, c.ComputerID)).ToList();
            computerCombo.SelectedIndex = -1;

            computerCombo.SelectedIndexChanged += OnSearchValueChanged;
        }

        /// <summary>
        /// Load the reason combobox
        /// </summary>
        private void LoadReasonCombo()
        {
            reasonCombo.SelectedIndexChanged -= OnSearchValueChanged;

            reasonCombo.DisplayMember = "Key";
            reasonCombo.ValueMember = "Value";
            reasonCombo.DataSource = EnumHelper.GetEnumList<AuditReasonType>().Where(e => e.Value != AuditReasonType.Default).ToList();
            reasonCombo.SelectedIndex = -1;

            reasonCombo.SelectedIndexChanged += OnSearchValueChanged;
        }

        /// <summary>
        /// Load the action combobox
        /// </summary>
        private void LoadActionCombo()
        {
            actionCombo.SelectedIndexChanged -= OnSearchValueChanged;

            actionCombo.DisplayMember = "Key";
            actionCombo.ValueMember = "Value";
            actionCombo.DataSource = EnumHelper.GetEnumList<AuditActionType>().Where(e => e.Value != AuditActionType.Undetermined).ToList();
            actionCombo.SelectedIndex = -1;

            actionCombo.SelectedIndexChanged += OnSearchValueChanged;
        }

        /// <summary>
        /// Load the date editor
        /// </summary>
        private void LoadDateEditor()
        {
            // Reuse the filter date editor
            DateValueEditor editor = new DateValueEditor(dateCondition);

            panelDate.Controls.Add(editor);

            editor.ContentChanged += this.OnSearchValueChanged;
        }

        /// <summary>
        /// User double-clicked a row
        /// </summary>
        private void OnRowActivate(object sender, GridRowEventArgs e)
        {
            AuditUtility.ShowAuditDetail(this, ((EntityGridRow) e.Row).EntityID.Value);
        }

        /// <summary>
        /// Timer to ensure we keep processing new incoming audits
        /// </summary>
        private void OnTimer(object sender, EventArgs e)
        {
            if (Visible)
            {
                try
                {
                    AuditProcessor.ProcessAudits();
                }
                catch (SqlException ex)
                {
                    log.Error("A SqlException, probably a Deadlock, was thrown.  Just continue and not crash so as to not make the customer angry.", ex);
                }
                catch (Exception ex)
                {
                    // For some reason, the SqlException was not being caught and a customer was still getting a deadlock crash.  So checking the inner exception
                    // to see if it's a SqlException and logging as above.  If it's not, just carry on.  ProcessAudits will be called from other places and exceptions 
                    // handled there.
                    if (ex.InnerException != null && ex.InnerException is SqlException)
                    {
                        log.Error("A SqlException, probably a Deadlock, was thrown.  Just continue and not crash so as to not make the customer angry.", ex.InnerException);
                    }
                    else
                    {
                        log.Error("An unexpected exception type was thrown.  Just continue and not crash so as to not make the customer angry.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Lock the search criteria into displaying content only for the given user
        /// </summary>
        public void LockUserSearchCriteria(long userID)
        {
            if (lockedUserCritiera != null)
            {
                throw new InvalidOperationException("User criteria has already been locked");
            }

            // Set the new critiera
            lockedUserCritiera = AuditFields.UserID == userID;

            // Update the UI
            userBox.Visible = false;
            userCombo.Visible = false;
            panelUserLocked.Top = userCombo.Top - 1;

            // Update the filter
            UpdateSearchOptionsCriteria();
            UpdateQueryFilter();
        }

        /// <summary>
        /// Lock the search criteria for related object to the given orderid
        /// </summary>
        public void LockOrderSearchCriteria(long orderID)
        {
            if (lockedSearchTextCriteria != null)
            {
                throw new InvalidOperationException("Order criteria has already been locked.");
            }

            // Look for any changes directly to the order
            PredicateExpression expression = new PredicateExpression(AuditFields.ObjectID == orderID);

            // Look for changes to order's customer
            List<long> customerID = DataProvider.GetRelatedKeys(orderID, EntityType.CustomerEntity);
            if (customerID.Count == 1)
            {
                expression.AddWithOr(AuditFields.ObjectID == customerID[0]);
            }

            // Look for any of the order's shipments (that still exist)
            expression.AddWithOr(new FieldCompareSetPredicate(AuditFields.ObjectID, null, ShipmentFields.ShipmentID, null, SetOperator.In,
                ShipmentFields.OrderID == orderID));

            Stopwatch sw = Stopwatch.StartNew();

            // Find logs that represent shipments that were created for this order
            AuditChangeCollection auditChanges = AuditChangeCollection.Fetch(SqlAdapter.Default,
                new FieldCompareSetPredicate(AuditChangeFields.AuditChangeID, null, AuditChangeDetailFields.AuditChangeID, null, SetOperator.In,
                    AuditChangeDetailFields.VariantNew == orderID));

            List<long> shipmentKeys = new List<long>();
            foreach (AuditChangeEntity change in auditChanges.Where(c => EntityUtility.GetEntityType(c.ObjectID) == EntityType.ShipmentEntity))
            {
                shipmentKeys.Add(change.ObjectID);
            }

            Debug.WriteLine("Get audit shipment keys: " + sw.Elapsed.TotalSeconds);

            expression.AddWithOr(AuditFields.ObjectID == shipmentKeys);
            
            // Set the new criteria
            lockedSearchTextCriteria = expression;

            // Update the UI
            panelRelatedTo.Visible = false;

            // Update filter
            UpdateQueryFilter();
        }

        /// <summary>
        /// The state of one of the search checkbox's has changed
        /// </summary>
        private void OnSearchCheckChanged(object sender, EventArgs e)
        {
            userCombo.Enabled = userBox.Checked;
            if (userCombo.Enabled && userCombo.SelectedIndex == -1)
            {
                userCombo.SelectedIndex = 0;
            }

            computerCombo.Enabled = computerBox.Checked;
            if (computerCombo.Enabled && computerCombo.SelectedIndex == -1)
            {
                computerCombo.SelectedIndex = 0;
            }

            reasonCombo.Enabled = reasonBox.Checked;
            if (reasonCombo.Enabled && reasonCombo.SelectedIndex == -1)
            {
                reasonCombo.SelectedIndex = 0;
            }

            actionCombo.Enabled = actionBox.Checked;
            if (actionCombo.Enabled && actionCombo.SelectedIndex == -1)
            {
                actionCombo.SelectedIndex = 0;
            }

            panelDate.Enabled = dateBox.Checked;

            buttonSearching.Visible = relatedToBox.Checked;
            searchBox.Enabled = relatedToBox.Checked;

            UpdateSearchOptionsCriteria();
            UpdateQueryFilter();
        }

        /// <summary>
        /// One of the values of the search boxes has changed
        /// </summary>
        private void OnSearchValueChanged(object sender, EventArgs e)
        {
            // This is required to make the date editor update the date condition values
            panelDate.ValidateChildren(ValidationConstraints.Visible);

            UpdateSearchOptionsCriteria();
            UpdateQueryFilter();
        }

        /// <summary>
        /// Update the query expression that represents the search options (the checkboxes)
        /// </summary>
        private void UpdateSearchOptionsCriteria()
        {
            // Create a new bucket to hold our critiera
            PredicateExpression expression = new PredicateExpression();

            // User criteria
            if (lockedUserCritiera != null)
            {
                expression.AddWithAnd(lockedUserCritiera);
            }
            else if (userBox.Checked && userCombo.SelectedIndex != -1)
            {
                expression.AddWithAnd(AuditFields.UserID == (long) ((ImageComboBoxItem) userCombo.SelectedItem).Value);
            }

            // Computer criteria
            if (computerBox.Checked && computerCombo.SelectedIndex != -1)
            {
                expression.AddWithAnd(AuditFields.ComputerID == (long) computerCombo.SelectedValue);
            }

            // Reason criteria
            if (reasonBox.Checked && reasonCombo.SelectedIndex != -1)
            {
                expression.AddWithAnd(AuditFields.Reason == (int) (AuditReasonType) reasonCombo.SelectedValue);
            }

            // Action criteria
            if (actionBox.Checked && actionCombo.SelectedIndex != -1)
            {
                expression.AddWithAnd(AuditFields.Action == (int) (AuditActionType) actionCombo.SelectedValue);
            }

            // Date criteria
            if (dateBox.Checked)
            {
                expression.AddWithAnd(GetDatePredicate());
            }

            lock (searchCriteriaLock)
            {
                searchOptionsCriteria = expression;
            }
        }

        /// <summary>
        /// Update the filter that controls what the gateway shows
        /// </summary>
        private void UpdateQueryFilter()
        {
            try
            {
                RelationPredicateBucket bucket = new RelationPredicateBucket();

                lock (searchCriteriaLock)
                {
                    // Search options critiera
                    bucket.PredicateExpression.AddWithAnd(ClonePredicate(searchOptionsCriteria));

                    // Related To Object (Search text) criteria
                    if (lockedSearchTextCriteria != null)
                    {
                        bucket.PredicateExpression.AddWithAnd(lockedSearchTextCriteria);
                    }
                    else if (relatedToBox.Checked)
                    {
                        bucket.PredicateExpression.AddWithAnd(ClonePredicate(searchTextCriteria));
                    }
                }

                // Apply the bucket to the gateway
                entityGrid.OpenGateway(new QueryableEntityGateway(entityProvider, bucket));
            }
            catch (SqlException ex)
            {
                log.Error("A SqlException, probably a Deadlock, was thrown.  Just continue and not crash so as to not make the customer angry.", ex);
            }
            catch (Exception ex)
            {
                // For some reason, the SqlException was not being caught and a customer was still getting a deadlock crash.  So checking the inner exception
                // to see if it's a SqlException and logging as above.  If it's not, just carry on.  ProcessAudits will be called from other places and exceptions 
                // handled there.
                if (ex.InnerException != null && ex.InnerException is SqlException)
                {
                    log.Error("A SqlException, probably a Deadlock, was thrown.  Just continue and not crash so as to not make the customer angry.", ex.InnerException);
                }
                else
                {
                    log.Error("An unexpected exception type was thrown.  Just continue and not crash so as to not make the customer angry.", ex);
                }
            }
        }

        /// <summary>
        /// Clones the given predicate
        /// </summary>
        private static IPredicate ClonePredicate(Predicate predicate)
        {
            // Create a bucket just b\c we have clone functionality that will let us clone the criteria
            RelationPredicateBucket bucket = new RelationPredicateBucket(predicate);

            return EntityUtility.ClonePredicateBucket(bucket).PredicateExpression;
        }

        /// <summary>
        /// Get the predicate to use to filter on date
        /// </summary>
        private IPredicate GetDatePredicate()
        {
            DateOperator effectiveOp = dateCondition.Operator;
            DateTime effectiveValue1 = dateCondition.Value1;
            DateTime effectiveValue2 = dateCondition.Value2;

            // Convert the user requested operator and dates into an absolute range
            dateCondition.GetEffectiveOperation(ref effectiveOp, ref effectiveValue1, ref effectiveValue2);

            // Now we have to apply utc offset
            effectiveValue1 = dateCondition.ConvertToUniversalTime(effectiveValue1);

            // Between \ Not Between
            if (effectiveOp == DateOperator.Between || effectiveOp == DateOperator.NotBetween)
            {
                // We'll need value2
                effectiveValue2 = dateCondition.ConvertToUniversalTime(effectiveValue2);

                ComparisonOperator op1 = ComparisonOperator.GreaterEqual;
                ComparisonOperator op2 = ComparisonOperator.LesserThan;
                string join = "AND";

                if (effectiveOp == DateOperator.NotBetween)
                {
                    op1 = ComparisonOperator.LesserThan;
                    op2 = ComparisonOperator.GreaterEqual;
                    join = "OR";
                }

                // Due to the way our inc\exc operators work, we have to bump the second value to the next day
                effectiveValue2 = effectiveValue2.AddDays(1);

                PredicateExpression expression = new PredicateExpression();
                expression.AddWithAnd(new FieldCompareValuePredicate(AuditFields.Date, null, op1, effectiveValue1));

                if (join == "AND")
                {
                    expression.AddWithAnd(new FieldCompareValuePredicate(AuditFields.Date, null, op2, effectiveValue2));
                }
                else
                {
                    expression.AddWithOr(new FieldCompareValuePredicate(AuditFields.Date, null, op2, effectiveValue2));
                }

                return expression;
            }
            else
            {
                return new FieldCompareValuePredicate(AuditFields.Date, null, GetComparisonOperator(effectiveOp), effectiveValue1);
            }
        }

        /// <summary>
        /// Get the ComparisonOperator that corresponds to the given date operator
        /// </summary>
        private ComparisonOperator GetComparisonOperator(DateOperator dateOperator)
        {
            switch (dateOperator)
            {
                case DateOperator.LessThan:
                    return ComparisonOperator.LesserThan;
                case DateOperator.GreaterThanOrEqual:
                    return ComparisonOperator.GreaterEqual;
            }

            throw new InvalidOperationException("Invalid operator evaluated in GetComparisonOperator.");
        }

        /// <summary>
        /// The text in the related to search box has changed
        /// </summary>
        private void OnSearchTextChanged(object sender, EventArgs e)
        {
            searchChangedEvent.Set();
        }

        /// <summary>
        /// The thread that runs in the background to generate the search criteria object for the related to axis
        /// </summary>
        [NDependIgnoreLongMethod]
        private void ThreadRelatedToCriteria()
        {
            try
            {
                while (true)
                {
                    searchChangedEvent.WaitOne();

                    // Have to get the text from the UI
                    string text = (string) Program.MainForm.Invoke(new Func<string>(() => 
                        { 
                            return searchTerminated ? null : searchBox.Text; 
                        }));

                    // Search was terminated
                    if (text == null)
                    {
                        break;
                    }

                    log.InfoFormat("Processing search change, {0}", text);

                    // Update the icon to searching...
                    Program.MainForm.Invoke(new MethodInvoker(() => 
                        {
                            if (!searchTerminated)
                            {
                                timer.Stop();
                                buttonSearching.ImageStates.ImageDisabled = Resources.hourglass_11;
                            }
                        }));

                    text = text.Trim();
                    Stopwatch sw = Stopwatch.StartNew();

                    // New expression
                    PredicateExpression expression = null;

                    if (text.Length > 0)
                    {
                        expression = new PredicateExpression();

                        // Add in the generic text search
                        PredicateExpression detailExpression = new PredicateExpression();

                        // Search the details for the LIKE text%
                        detailExpression.Add(AuditChangeDetailFields.VariantNew == text);

                        // See if it can be a decimal (money)
                        decimal decimalValue;
                        if (decimal.TryParse(text, NumberStyles.Any, null, out decimalValue))
                        {
                            detailExpression.AddWithOr(AuditChangeDetailFields.VariantNew == decimalValue);
                        }

                        // Add the direct details search part of the expression
                        expression.AddWithOr(new FieldCompareSetPredicate(AuditFields.AuditID, null, AuditChangeDetailFields.AuditID, null, SetOperator.In, detailExpression));

                        // If its a number, we have to search by that too.  You'd expect it to pull stuff up by order number
                        long number;
                        if (long.TryParse(text, out number))
                        {
                            // LLBL % is shorthand for a FieldLikePredicate object
                            PredicateExpression orderNumberExpression = new PredicateExpression(OrderFields.OrderNumberComplete % (text + "%"));

                            expression.AddWithOr(new FieldCompareSetPredicate(AuditFields.ObjectID, null, OrderFields.OrderID, null, SetOperator.In, orderNumberExpression));
                        }
                    }

                    // Update the expression the UI uses
                    lock (searchCriteriaLock)
                    {
                        searchTextCriteria = expression;
                    }

                    // Update the icon back to not searching
                    Program.MainForm.Invoke(new MethodInvoker(() => {
                        if (!searchTerminated) 
                        { 
                            buttonSearching.ImageStates.ImageDisabled = searchImageReady;

                            UpdateQueryFilter();

                            timer.Start();
                        } }));
                }
            }
            catch (ObjectDisposedException)
            {
                // UI went away
            }

            searchChangedEvent.Close();
        }
    }
}
