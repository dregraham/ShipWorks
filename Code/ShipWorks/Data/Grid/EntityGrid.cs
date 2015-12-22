using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid.Rendering;
using System.Drawing;
using System.ComponentModel;
using ShipWorks.Data.Grid.DetailView;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.UI.Controls.SandGrid;
using System.Windows.Forms;
using ShipWorks.UI;
using System.Diagnostics;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared;
using ShipWorks.Templates.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.Threading;
using System.Runtime.InteropServices;
using ShipWorks.Data.Grid.Paging;
using System.Threading;
using ShipWorks.Templates.Processing;
using ShipWorks.UI.Controls.Html;
using Interapptive.Shared.Utility;
using ShipWorks.Templates;
using System.Collections;
using Interapptive.Shared.Win32;
using Interapptive.Shared.UI;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using log4net;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// A custom grid that can be adorned by an outsider
    /// </summary>
    public partial class EntityGrid : SandGrid
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EntityGrid));

        DetailViewSettings viewSettings;

        // Use to render the the detail view
        DetailViewRenderer detailRenderer;

        // Responsible for loading, saving, and manipulating grid columns
        EntityGridColumnStrategy columnStrategy;

        // Provides notification that we should show the column context menu
        ColumnContextMenuProvider contextMenuProvider;

        // Allow for suspending of sort processing while messing with columns
        bool suspendSortProcessing = false;

        // Field source for nested error display.  If null then nested error display is off
        EntityGridRowErrorProvider errorProvider;

        // The minimum size the control needs to be to not show any scroll bars.
        Size minimumNoScrollSize = new Size(50, 25);

        // Used to know which grid cell was clicked, for purposed of Copy operations.
        EntityGridColumn contextMenuColumn;

        // Exposes the grids current selection state
        EntityGridSelection selection;

        /// <summary>
        /// Raised when a column-specific action occurs in one of the cells of an actionable column.
        /// </summary>
        public event GridHyperlinkClickEventHandler GridCellLinkClicked;

        /// <summary>
        /// Raised when the minimum display size to not show scroll bars has changed
        /// </summary>
        public event EventHandler MinimumNoScrollSizeChanged;

        #region class SortMaintainer

        class SortMaintainer : IDisposable
        {
            Guid sortColumnID = Guid.Empty;
            ListSortDirection sortOrder = ListSortDirection.Descending;

            EntityGrid grid;

            /// <summary>
            /// Constructor
            /// </summary>
            public SortMaintainer(EntityGrid parent)
            {
                this.grid = parent;

                if (grid.SortColumn != null)
                {
                    sortColumnID = ((EntityGridColumn) grid.SortColumn).ColumnGuid;
                    sortOrder = grid.SortDirection;
                }
            }

            public void Dispose()
            {
                if (sortColumnID != Guid.Empty)
                {
                    EntityGridColumn column = grid.Columns.OfType<EntityGridColumn>().SingleOrDefault(c => c.ColumnGuid == sortColumnID);

                    // Should always be able to find it, based on how we use this thing.
                    Debug.Assert(column != null);

                    if (column != null)
                    {
                        grid.SortColumn = column;
                        grid.SortDirection = sortOrder;
                    }
                }
            }
        }

        #endregion

        #region class CopyWithTemplateData

        class CopyWithTemplateData
        {
            public HtmlControl HtmlControl { get; set; }
            public string PlainText { get; set; }
            public bool ResultsTruncated { get; set; }
            public TemplateException Error { get; set; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityGrid()
        {
            LiveResize = false;

            selection = new EntityGridSelection(this);
        }

        #region Columns

        /// <summary>
        /// Initialize the strategy the controls how columns are loaded, manipulated, and saved.
        /// </summary>
        public void InitializeColumns(EntityGridColumnStrategy columnStrategy)
        {
            if (columnStrategy == null)
            {
                throw new ArgumentNullException("columnStrategy");
            }

            // Create the provider to allow us to have a context menu for the grid column headers
            if (contextMenuProvider == null)
            {
                contextMenuProvider = new ColumnContextMenuProvider(this);
                contextMenuProvider.ShowContextMenu += new ColumnContextMenuEventHandler(OnGridColumnContextMenu);
            }

            this.columnStrategy = columnStrategy;

            // Load the columns into the grid
            columnStrategy.LoadColumns(this);
            columnStrategy.ApplyInitialSort(this);
        }

        /// <summary>
        /// Tells the grid that the colunm state should be automatically persisted when the given Form closes.
        /// </summary>
        public void SaveColumnsOnClose(Form form)
        {
            form.FormClosing += delegate { columnStrategy.SaveColumns(this); };
        }

        /// <summary>
        /// The configured grid column strategy
        /// </summary>
        public EntityGridColumnStrategy ColumnStrategy
        {
            get { return columnStrategy; }
        }

        /// <summary>
        /// Show the grid column editor
        /// </summary>
        void OnGridColumnContextMenu(object sender, ColumnContextMenuEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Save the state of the columns before we start messing with them
            columnStrategy.SaveColumns(this);

            // Create the popup editor that will be used for interactive editing
            UserControl editor = columnStrategy.CreatePopupEditor(this, OnContextGridColumnsChanged);

            // Check if context editing is not supported
            if (editor == null)
            {
                return;
            }

            editor.BackColor = Color.Transparent;
            editor.Height = 400;

            PopupController popupController = new PopupController(editor);
            popupController.Size = editor.Size;

            // Show the poup
            Point clientPoint = e.ClientPoint;
            popupController.ShowDropDown(new Rectangle(clientPoint.X, clientPoint.Y, 10, 1), this);

            // Kill the editor
            editor.Dispose();

            // We have to make sure we still are sorted on a valid column.  User may have made the sort column invisible.
            if (SortColumn == null || !SortColumn.Visible)
            {
                columnStrategy.ApplyInitialSort(this);
            }
        }

        /// <summary>
        /// The layout has been changed via the context editor
        /// </summary>
        void OnContextGridColumnsChanged()
        {
            ReloadColumns();
        }

        /// <summary>
        /// Show the modal dialog for editing the grid columns
        /// </summary>
        public void ShowColumnEditorDialog()
        {
            Cursor.Current = Cursors.WaitCursor;

            columnStrategy.SaveColumns(this);

            // Show the modal editing window
            using (Form dlg = columnStrategy.CreateModalEditor())
            {
                dlg.ShowDialog(this);
            }

            // Reload the latest column settings
            ReloadColumns();

            // We have to make sure we still are sorted on a valid column.  User may have made the sort column invisible.
            if (SortColumn == null || !SortColumn.Visible)
            {
                columnStrategy.ApplyInitialSort(this);
            }
        }

        /// <summary>
        /// Save the current state of grid columns
        /// </summary>
        public void SaveColumns()
        {
            ColumnStrategy.SaveColumns(this);
        }

        /// <summary>
        /// Reload the column set while maintaining the current sort
        /// </summary>
        public void ReloadColumns()
        {
            SuspendSortProcessing = true;

            using (SortMaintainer sortMaintainer = new SortMaintainer(this))
            {
                columnStrategy.LoadColumns(this);
            }

            SuspendSortProcessing = false;
        }

        /// <summary>
        /// Raises the LinkClicked event.  Intended to be called from EntityGridColumn derived classes to inform
        /// the grid that some action specific to the column has been performed in a cell.  This allows for centralized
        /// handling of cell actions through the grid.
        /// </summary>
        internal void OnGridCellLinkClicked(EntityGridRow row, EntityGridColumn column, MouseEventArgs mouseArgs)
        {
            if (GridCellLinkClicked != null)
            {
                GridCellLinkClicked(this, new GridHyperlinkClickEventArgs(row, column, mouseArgs));
            }
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Allows derived classed to suspend processing sort changes. All this does is prevent the SortChanged event from being raised.
        /// </summary>
        protected bool SuspendSortProcessing
        {
            get { return suspendSortProcessing; }
            set { suspendSortProcessing = value; }
        }

        /// <summary>
        /// Override to prevent the sort event from being raised when sort processing is suspeneded.
        /// </summary>
        protected override void OnSortChanged(GridEventArgs e)
        {
            if (suspendSortProcessing)
            {
                return;
            }

            base.OnSortChanged(e);
        }

        #endregion

        #region Detail View

        /// <summary>
        /// Exposes the DetailViewRenderer that should be used to draw the detail mode for the grid.  Not really intended
        /// to be used by anyone except the EntityGridRow.
        /// </summary>
        [Browsable(false)]
        internal DetailViewRenderer DetailViewRenderer
        {
            get
            {
                // Moved this to be lazy created.  The constructor of it creates a new background thread.  Which was causing
                // problems at design time when we created one in our constructor.
                if (detailRenderer == null)
                {
                    detailRenderer = new DetailViewRenderer();
                }

                return detailRenderer;
            }
        }

        /// <summary>
        /// Apply the view settings that control the detail mode the grid rows are drawn in.
        /// </summary>
        public DetailViewSettings DetailViewSettings
        {
            get
            {
                return viewSettings;
            }
            set
            {
                if (viewSettings == value)
                {
                    return;
                }

                // Unhook the events for the old settings
                ClearActiveSettings();

                viewSettings = value;

                // Hook the events for the new settings.
                if (viewSettings != null)
                {
                    viewSettings.SettingsChanged += new EventHandler(OnDetailViewSettingsChanged);
                }

                ApplyDetailViewSettings();
            }
        }

        /// <summary>
        /// The current detail view settings have changed and we need to apply them.
        /// </summary>
        void OnDetailViewSettingsChanged(object sender, EventArgs e)
        {
            ApplyDetailViewSettings();
        }

        /// <summary>
        /// Apply the current detail view settings
        /// </summary>
        private void ApplyDetailViewSettings()
        {
            /* BN: LiveResize is always off now.
             * 
            DetailViewMode effectiveMode = (viewSettings == null) ? DetailViewMode.Normal : viewSettings.DetailViewMode;

            // Turn off LiveResize if not in normal mode
            LiveResize = (effectiveMode == DetailViewMode.Normal);
            */

            Invalidate();
        }

        /// <summary>
        /// Clear the active settings
        /// </summary>
        private void ClearActiveSettings()
        {
            if (viewSettings != null)
            {
                viewSettings.SettingsChanged -= new EventHandler(OnDetailViewSettingsChanged);
            }
        }

        #endregion

        #region Display

        /// <summary>
        /// The minimum size the grid must be to not show scrollbars.
        /// </summary>
        [Browsable(false)]
        public Size MinimumNoScrollSize
        {
            get { return minimumNoScrollSize; }
        }

        /// <summary>
        /// Field source for nested error display.  If null then nested error display is off.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null)]
        public EntityGridRowErrorProvider ErrorProvider
        {
            get
            {
                return errorProvider;
            }
            set
            {
                errorProvider = value;

                PerformElementLayout();
                Refresh();
            }
        }

        /// <summary>
        /// The grid is being painted
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
            }
            catch (InvalidOperationException ex)
            {
                log.Error("SandGrid failed in OnPaint!", ex);
            }

            UpdateMinimumNoScrollSize();
        }

        /// <summary>
        /// Update the minimum size the grid needs to be to display no scrolls
        /// </summary>
        protected void UpdateMinimumNoScrollSize()
        {
            int noScrollHeight = 0;
            int noScrollWidth = 0;

            if (Rows.Count == 0)
            {
                if (ShowColumnHeaders && Columns.DisplayColumns.Length > 0)
                {
                    noScrollHeight = Columns.DisplayColumns[0].Bounds.Bottom;

                    if (!string.IsNullOrEmpty(EmptyText))
                    {
                        noScrollHeight += 18;
                    }
                }
            }
            else
            {
                noScrollHeight = Rows[Rows.Count - 1].Bounds.Bottom;
            }

            List<GridColumn> visibleColumns = Columns.Cast<GridColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
            if (visibleColumns.Count > 0)
            {
                noScrollWidth = visibleColumns[visibleColumns.Count - 1].Bounds.Right;
            }

            // The Size minus the display area is how much adornment space is needed (like scrollbars)
            noScrollHeight += (Size.Height - DisplayRectangle.Height);
            // noScrollWidth += (Size.Width - DisplayRectangle.Width);

            noScrollHeight = Math.Max(25, noScrollHeight);
            noScrollWidth = Math.Max(50, noScrollWidth);

            // See if the minimum height to not show scroll bars has changed
            if (noScrollHeight != minimumNoScrollSize.Height || noScrollWidth != minimumNoScrollSize.Width)
            {
                minimumNoScrollSize = new Size(noScrollWidth, noScrollHeight);

                if (MinimumNoScrollSizeChanged != null)
                {
                    MinimumNoScrollSizeChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Copy

        /// <summary>
        /// Create the menu items used to copy data out of the grid
        /// </summary>
        public ToolStripItem[] CreateCopyMenuItems(bool includeCopyWithTemplate)
        {
            List<ToolStripItem> items = new List<ToolStripItem>();

            ToolStripMenuItem copyColumn = new ToolStripMenuItem("Copy Column");
            copyColumn.Click += new EventHandler(OnCopyColumn);
            copyColumn.Tag = "Copy '{0}'";

            ToolStripMenuItem copyRow = new ToolStripMenuItem("Copy Row");
            copyRow.Click += new EventHandler(OnCopyRow);

            ToolStripMenuItem copyRowWithHeader = new ToolStripMenuItem("Copy Row with Headers");
            copyRowWithHeader.Click += new EventHandler(OnCopyRowWithHeaders);

            items.Add(copyColumn);
            items.Add(new ToolStripSeparator());
            items.Add(copyRow);
            items.Add(copyRowWithHeader);

            if (includeCopyWithTemplate)
            {
                items.Add(new ToolStripSeparator());

                ToolStripMenuItem copyWithTemplate = new ToolStripMenuItem("Copy With Template");
                copyWithTemplate.DropDownOpening += new EventHandler(OnCopyWithTemplateOpening);
                copyWithTemplate.DropDownItems.Add(new ToolStripMenuItem("Placeholder"));
                items.Add(copyWithTemplate);
            }

            // Listen for when the owner of the menu items get set.  Then we hookup to listen for the owner opening, so we can update the status
            // of the menu items.
            copyColumn.OwnerChanged += (object sender, EventArgs e) =>
            {
                ToolStripDropDown dropDown = copyColumn.Owner as ToolStripDropDown;
                if (dropDown != null)
                {
                    dropDown.Opening += delegate { OnCopyMenuItemsOpening(items); };
                }
            };

            return items.ToArray();
        }

        /// <summary>
        /// The given list of menu items is being opened.
        /// </summary>
        private void OnCopyMenuItemsOpening(List<ToolStripItem> items)
        {
            foreach (ToolStripMenuItem item in items.OfType<ToolStripMenuItem>())
            {
                item.Enabled = Selection.Count > 0;

                // If it's tagged, its the column specific column
                if (item.Tag != null)
                {
                    // Disable it if no selected column
                    item.Enabled = item.Enabled & contextMenuColumn != null;

                    // Update the text
                    item.Text = string.Format((string) item.Tag, contextMenuColumn != null ? contextMenuColumn.HeaderText : "Column");
                }
            }
        }

        /// <summary>
        /// Copy with the user's chosen template
        /// </summary>
        void OnCopyWithTemplate(object sender, TemplateNodeChangedEventArgs e)
        {
            // Needed due to the way we get the selected keys and create the html control
            Debug.Assert(!InvokeRequired);

            TemplateEntity template = e.NewNode.Template;

            // If there is nothing selected, get out
            if (Selection.Count == 0)
            {
                return;
            }

            List<long> orderedInputKeys = Selection.OrderedKeys.ToList();

            // Create the HtmlControl that will format the results
            HtmlControl htmlControl = TemplateOutputUtility.CreateUIBoundHtmlControl(null);

            // Set the size based on the template desired print size
            using (Graphics g = CreateGraphics())
            {
                htmlControl.Width = (int) Math.Ceiling(g.DpiX * template.PageWidth);
            }

            BackgroundExecutor<IList<TemplateResult>> executor = new BackgroundExecutor<IList<TemplateResult>>(this,
                    "Copying",
                    "ShipWorks is copying the selection.",
                    "Copying result...");

            executor.ExecuteCompleted += OnCopyWithTemplateCompleted;

            executor.ExecuteAsync(

                (IProgressReporter progress) =>
                {
                    try
                    {
                        // Process the template
                        IList<TemplateResult> results = TemplateProcessor.ProcessTemplate(template, orderedInputKeys, progress);

                        // Return one item so the worker get's called once
                        return new List<IList<TemplateResult>> { results };
                    }
                    catch (TemplateCancelException)
                    {
                        return null;
                    }
                },

                (results, state, issueAdder) =>
                {
                    CopyWithTemplateData data = (CopyWithTemplateData) state;

                    TemplateResultFormatSettings settings = TemplateResultFormatSettings.FromTemplate(template);

                    try
                    {
                        htmlControl.Html = TemplateResultFormatter.FormatHtml(
                            results,
                            TemplateResultUsage.Copy,
                            settings);
                    }
                    catch (TemplateException ex)
                    {
                        data.Error = ex;
                        return;
                    }

                    htmlControl.WaitForComplete(TimeSpan.FromSeconds(5));

                    // Process sure size now that its ready
                    TemplateSureSizeProcessor.Process(htmlControl);

                    // If its plain text, then use the plain text directly
                    if (settings.OutputFormat != TemplateOutputFormat.Html)
                    {
                        data.PlainText = string.Join(
                            "\r\n\r\n    -------------- Page Break --------------\r\n\r\n",
                            results.Take(settings.NextResultIndex).Select(r => r.ReadResult()).ToArray());
                    }
                    // Otherwise, convert from HTML
                    else
                    {
                        data.PlainText = HtmlUtility.GetPlainText(htmlControl.Html);
                    }

                    // Determine if we were able to use all the results (maybe not, due to memory issues)
                    data.ResultsTruncated = settings.NextResultIndex < results.Count;
                },

                new CopyWithTemplateData { HtmlControl = htmlControl }
                );
        }

        /// <summary>
        /// Copy operation has completed
        /// </summary>
        void OnCopyWithTemplateCompleted(object sender, BackgroundExecutorCompletedEventArgs<IList<TemplateResult>> e)
        {
            CopyWithTemplateData data = (CopyWithTemplateData) e.UserState;

            try
            {
                if (e.Canceled)
                {
                    return;
                }

                if (data.Error != null)
                {
                    MessageHelper.ShowError(this, data.Error.Message);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    Clipboard.Clear();
                    IDataObject clipboardData = new DataObject();

                    HtmlControl htmlControl = data.HtmlControl;

                    if (!string.IsNullOrEmpty(data.PlainText))
                    {
                        clipboardData.SetData(DataFormats.Text, true, data.PlainText);
                    }

                    // Determine the ideal height of the bitmap
                    // GDI+ does... but GDI, and thus the clipboard, does not support bitmap heights
                    // greater than 
                    int idealHeight = Math.Min(32767, htmlControl.DetermineIdealRenderedBitmapHeight());

                    htmlControl.Height = idealHeight + 10;

                    Rectangle bounds = new Rectangle(0, 0, htmlControl.Width, idealHeight);

                    Bitmap bitmap = null;

                    try
                    {
                        // Generate the bitmap (if it would be of significant enough size).  And, any smaller than that,
                        // and the htmlcontrol thows an exception.
                        if (bounds.Width > 5 && bounds.Height > 5)
                        {
                            bitmap = htmlControl.RenderToBitmap(bounds, Color.White);
                            clipboardData.SetData(DataFormats.Bitmap, true, bitmap);
                        }

                        if (clipboardData.GetFormats().Length > 0)
                        {
                            Clipboard.SetDataObject(clipboardData, true);
                        }

                        if (data.ResultsTruncated)
                        {
                            MessageHelper.ShowInformation(this, "Some of the selection was not copied since the results were too big for the Windows clipboard.");
                        }
                    }
                    finally
                    {
                        if (bitmap != null)
                        {
                            bitmap.Dispose();
                        }
                    }
                }
                catch (ExternalException ex)
                {
                    log.Error("Copy", ex);

                    MessageHelper.ShowError(this, "ShipWorks could not copy to the clipboard because it is in use by another application.");
                }
            }
            finally
            {
                data.HtmlControl.Dispose();
            }
        }

        /// <summary>
        /// Initiate the copy operation.
        /// </summary>
        private void InitiateTextCopy(List<EntityGridColumn> columnsToCopy, bool includeHeaders)
        {
            // If there is nothing selected, get out
            if (Selection.Count == 0)
            {
                return;
            }

            // If there are no columns, get out
            if (columnsToCopy.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            // Add headers if necessary
            if (includeHeaders)
            {
                for (int i = 0; i < columnsToCopy.Count; i++)
                {
                    EntityGridColumn column = columnsToCopy[i];

                    if (i > 0)
                    {
                        sb.Append("\t");
                    }

                    sb.Append(column.HeaderText);
                }
            }

            // This is kind of cheating - but there likely won't be any other derived types of EntityGrid, and this makes 
            // putting the copy functionality of the grid all in one place much easier.
            PagedEntityGrid pagedGrid = this as PagedEntityGrid;
            if (pagedGrid != null)
            {
                BackgroundExecutor<long> executor = new BackgroundExecutor<long>(this,
                        "Copying",
                        "ShipWorks is copying the selection.",
                        "Copying row {0} of {1}");

                executor.ExecuteCompleted += OnTextCopyCompleted;

                executor.ExecuteAsync(

                    (entityID, state, issueAdder) =>
                    {
                        EntityBase2 entity = pagedGrid.EntityGateway.GetEntityFromKey(entityID);

                        // If it had been deleted, skip it
                        if (entity != null)
                        {
                            CopyEntityText(entity, columnsToCopy, sb);
                        }
                    },

                    pagedGrid.Selection.OrderedKeys,

                    sb);
            }
            else
            {
                BackgroundExecutor<EntityBase2> executor = new BackgroundExecutor<EntityBase2>(this,
                    "Copying",
                    "ShipWorks is copying the selection.",
                    "Copying row {0} of {1}");

                executor.ExecuteCompleted += OnTextCopyCompleted;

                executor.ExecuteAsync(

                    (entity, state, issueAdder) =>
                    {
                        CopyEntityText(entity, columnsToCopy, sb);
                    },

                    // We don't pull from SelectedElements, as they may not bein order.
                    Rows.OfType<EntityGridRow>().Where(r => r.Selected && r.Entity != null).Select(r => r.Entity),

                    sb);
            }
        }

        /// <summary>
        /// Copy the specified columns from the given entity to the StringBuilder output
        /// </summary>
        private void CopyEntityText(EntityBase2 entity, List<EntityGridColumn> columnsToCopy, StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Append("\r\n");
            }

            for (int i = 0; i < columnsToCopy.Count; i++)
            {
                EntityGridColumn column = columnsToCopy[i];

                if (i > 0)
                {
                    sb.Append("\t");
                }

                GridColumnFormattedValue formattedValue = column.Definition.DisplayType.FormatValue(entity);

                sb.Append(formattedValue.Text);
            }
        }

        /// <summary>
        /// Copy operation has completed
        /// </summary>
        void OnTextCopyCompleted<T>(object sender, BackgroundExecutorCompletedEventArgs<T> e)
        {
            if (e.Canceled)
            {
                return;
            }

            try
            {
                Clipboard.Clear();

                StringBuilder sb = (StringBuilder) e.UserState;

                // Clipboard throws if empty
                if (sb.Length != 0)
                {
                    Clipboard.SetText(sb.ToString());
                }
            }
            catch (ExternalException ex)
            {
                log.Error("Copy", ex);

                MessageHelper.ShowError(this, "ShipWorks could not copy to the clipboard because it is in use by another application.");
            }
        }

        /// <summary>
        /// Copy the data out of the selected column
        /// </summary>
        void OnCopyColumn(object sender, EventArgs e)
        {
            InitiateTextCopy(new List<EntityGridColumn> { contextMenuColumn }, false);
        }

        /// <summary>
        /// Copy the data of the selected rows
        /// </summary>
        void OnCopyRow(object sender, EventArgs e)
        {
            InitiateTextCopy(Columns.OfType<EntityGridColumn>().Where(c => c.Visible).ToList(), false);
        }

        /// <summary>
        /// Copy the data of the selected rows, including column headers
        /// </summary>
        void OnCopyRowWithHeaders(object sender, EventArgs e)
        {
            InitiateTextCopy(Columns.OfType<EntityGridColumn>().Where(c => c.Visible).ToList(), true);
        }

        /// <summary>
        /// The copy with template menu item is opening
        /// </summary>
        void OnCopyWithTemplateOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(FindForm(), (ToolStripMenuItem) sender, OnCopyWithTemplate);
        }

        /// <summary>
        /// Handle key presses
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == (Keys.C | Keys.Control))
            {
                // Sand grid's default handling of ctrl-c wasn't copying correctly, and if there was a clipboard exception,
                // it wasn't being properly handled. So we'll just funnel ctrl-c into our CopyRows functionality
                InitiateTextCopy(Columns.OfType<EntityGridColumn>().Where(c => c.Visible).ToList(), false);
            }
            else
            {
                base.OnKeyDown(e);   
            }
        }

        #endregion

        #region Common

        /// <summary>
        /// The grid selection
        /// </summary>
        public virtual IGridSelection Selection
        {
            get
            {
                return selection;
            }
        }

        /// <summary>
        /// Disposing
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearActiveSettings();
                viewSettings = null;

                if (detailRenderer != null)
                {
                    detailRenderer.Dispose();
                    detailRenderer = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Intercept messages
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_CONTEXTMENU)
            {
                Point clickLocation = PointToGrid(PointToClient(Cursor.Position));

                // See which column was clicked
                contextMenuColumn = Columns.OfType<EntityGridColumn>().SingleOrDefault(
                    c => c.Bounds.Left < clickLocation.X && c.Bounds.Right > clickLocation.X);
            }

            // For some reason if you drag a column on the grid and 
            // put it back in the same position it crashes
            // logging crash for now
            try
            {
                base.WndProc(ref m);
            }
            catch (InvalidOperationException ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion
    }
}
