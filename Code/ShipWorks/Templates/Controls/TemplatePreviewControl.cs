using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Templates.Processing;
using ShipWorks.UI;
using ShipWorks.UI.Utility;
using ShipWorks.Data.Connection;
using Interapptive.Shared;
using ShipWorks.UI.Controls.Html;
using ShipWorks.Data.Model;
using ShipWorks.UI.Controls.Html.Core;
using System.Data.SqlClient;
using SandContextPopup = Divelements.SandRibbon.ContextPopup;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Control for displaying the preview of a single ShipWorks template
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class TemplatePreviewControl : UserControl
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplatePreviewControl));

        TemplateEntity activeTemplate;
        HtmlControl activeHtmlControl;

        // Preview that is currently being created that will be shown soon
        PendingPreview pendingPreview;

        // For flicker reduction
        double lastAutoZoom = 0;
        Size lastClientSize = new Size();

        string activeZoom = "100%";

        // Preview source
        TemplatePreviewSource activePreviewSource = TemplatePreviewSource.Order;
        int activePreviewCount = 1;

        SandContextPopup previewSourceMenu;
        List<SandMenuItem> checkablePreviewItems = new List<SandMenuItem>();

        static string messageNoTemplate = "No template selected.";
        static string messageNoData = "The template had no input using data from the selected filter.";
        static string messageCanceled = "The template preview was canceled.";

        /// <summary>
        /// Zoom level string for fitting width
        /// </summary>
        public static string FitWidth
        {
            get { return "Fit Width"; }
        }

        /// <summary>
        /// Zoom level string for ensuring entire contents fit
        /// </summary>
        public static string FitAll
        {
            get { return "Fit All"; }
        }

        // Used to do preview processing on a background thread
        delegate IList<TemplateResult> ProcessTemplateInvoker(ProgressProvider progressProvider);

        #region Internal Classes

        class PreviewSettings
        {
            public TemplateEntity Template { get; set; }

            public FilterNodeEntity PreviewFilterNode { get; set; }
            public TemplatePreviewSource PreviewSource { get; set; }
            public int PreviewCount { get; set; }

            public bool ClearQuickFilter { get; set; }
            public bool MaintainScroll { get; set; }

            public string Zoom { get; set; }
        }

        class PendingPreview
        {
            public HtmlControl HtmlControl { get; set; }
            public PreviewSettings Settings { get; set; }

            public bool BackgroundProcessingComplete { get; set; }
            public bool Canceled { get; set; }
            public ManualResetEvent CanceledEvent { get; set; }

            public ProgressProvider ProgressProvider { get; set; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplatePreviewControl()
        {
            InitializeComponent();

            zoomInToolbar.Renderer = new NoBorderToolStripRenderer();
            zoomOutToolbar.Renderer = new NoBorderToolStripRenderer();

            panelTools.StateNormal.Draw = ThemeInformation.VisualStylesEnabled ? InheritBool.True : InheritBool.False;
        }

        /// <summary>
        /// Inialize the control.  The FilterLayoutContext is required for populating the selection for preview source.
        /// </summary>
        public void Initialize()
        {
            previewFilter.LoadLayouts(FilterTarget.Orders, FilterTarget.Customers, FilterTarget.Shipments, FilterTarget.Items);

            DisableControls();
            CreatePreviewSourceMenu();
        }

        /// <summary>
        /// Create the context menu for selecting the preview source
        /// </summary>
        private void CreatePreviewSourceMenu()
        {
            previewSourceMenu = new SandContextPopup();
            previewSourceMenu.Font = new Font(Font, FontStyle.Regular);

            SandMenu menu = new SandMenu();
            previewSourceMenu.Items.Add(menu);

            menu.Items.Add(CreatePreviewSourceCountsMenu("Customers", TemplatePreviewSource.Customer));
            menu.Items.Add(CreatePreviewSourceCountsMenu("Orders", TemplatePreviewSource.Order));
            menu.Items.Add(CreatePreviewSourceCountsMenu("Shipments", TemplatePreviewSource.Shipment));
        }

        /// <summary>
        /// Create the menu to choose the count to use for the preview
        /// </summary>
        private SandMenuItem CreatePreviewSourceCountsMenu(string name, TemplatePreviewSource source)
        {
            SandMenuItem item = new SandMenuItem(name);

            SandMenu menu = new SandMenu();
            item.Items.Add(menu);

            menu.Items.Add(CreatePreviewItem("1", source, 1));
            menu.Items.Add(CreatePreviewItem("2", source, 2));
            menu.Items.Add(CreatePreviewItem("5", source, 5));
            menu.Items.Add(CreatePreviewItem("10", source, 10));
            menu.Items.Add(CreatePreviewItem("100", source, 100));
            menu.Items.Add(CreatePreviewItem("All", source, -1));

            return item;
        }

        /// <summary>
        /// Create a single selectable preview item choice
        /// </summary>
        private SandMenuItem CreatePreviewItem(string text, TemplatePreviewSource source, int count)
        {
            SandMenuItem item = new SandMenuItem(text);
            item.Tag = new object[] { source, count };
            item.Activate += new EventHandler(OnChangePreviewSource);

            checkablePreviewItems.Add(item);

            return item;
        }

        #region Other

        /// <summary>
        /// The template being previewed, or null of none.
        /// </summary>
        [Browsable(false)]
        public TemplateEntity ActiveTemplate
        {
            get
            {
                return activeTemplate;
            }
        }

        /// <summary>
        /// Save the current preview settings to the template
        /// </summary>
        public void SaveSettingsToTemplate()
        {
            if (activeTemplate != null)
            {
                TemplateUserSettingsEntity settings = TemplateHelper.GetUserSettings(activeTemplate);
                if (panelZoomControls.Enabled)
                {
                    settings.PreviewZoom = zoomCombo.Text;
                }

                settings.PreviewSource = (int) activePreviewSource;
                settings.PreviewCount = activePreviewCount;
                settings.PreviewFilterNodeID = ActivePreviewSettings.PreviewFilterNode == null ? (long?) null : ActivePreviewSettings.PreviewFilterNode.FilterNodeID;
            }
        }

        /// <summary>
        /// Get the active preview settings displayed in the UI
        /// </summary>
        private PreviewSettings ActivePreviewSettings
        {
            get
            {
                PreviewSettings settings = new PreviewSettings();
                settings.Template = activeTemplate;
                settings.PreviewCount = activePreviewCount;
                settings.PreviewSource = activePreviewSource;
                settings.PreviewFilterNode = previewFilter.SelectedFilterNode;
                settings.Zoom = activeZoom;

                return settings;
            }
        }

        /// <summary>
        /// Clear and disable the controls
        /// </summary>
        private void DisableControls()
        {
            panelPreviewControls.Visible = false;

            DisableZoom();
        }

        /// <summary>
        /// Disable zooming
        /// </summary>
        private void DisableZoom()
        {
            panelZoomControls.Enabled = false;

            zoomBar.Value = 100;
            zoomCombo.Text = "";
        }

        #endregion

        #region Preview Processing

        /// <summary>
        /// Create a new html control in which the preview can be displayed
        /// </summary>
        private HtmlControl CreateHtmlControl()
        {
            HtmlControl htmlControl = new HtmlControl();

            htmlControl.Location = new Point(-1000, -1000);
            htmlControl.BorderStyle = BorderStyle.None;
            htmlControl.OpenLinksInNewWindow = true;

            htmlControl.AllowActivation = false;

            #if DEBUG
                htmlControl.AllowContextMenu = true;
            #else
                htmlControl.AllowContextMenu = false;
            #endif

            backgroundPanel.Controls.Add(htmlControl);

            return htmlControl;
        }

        /// <summary>
        /// Load the previewer for the specified template.  The scroll position is not maintained.
        /// </summary>
        public void LoadPreview(TemplateEntity template)
        {
            LoadPreview(template, false);
        }

        /// <summary>
        /// Load the previewer for the specified template.
        /// </summary>
        public void LoadPreview(TemplateEntity template, bool maintainScroll)
        {
            Debug.Assert(!InvokeRequired);

            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            IntPtr handle = zoomCombo.Handle;

            // Create a new local copy of preview settings
            PreviewSettings previewSettings = new PreviewSettings();
            previewSettings.Template = template;

            TemplateUserSettingsEntity templateSettings = TemplateHelper.GetUserSettings(template);
            previewSettings.Zoom = templateSettings.PreviewZoom;

            previewSettings.PreviewSource = (TemplatePreviewSource) templateSettings.PreviewSource;
            previewSettings.PreviewCount = templateSettings.PreviewCount;

            long? filterNodeID = templateSettings.PreviewFilterNodeID;
            if (filterNodeID == null || FilterLayoutContext.Current.FindNode(filterNodeID.Value) == null)
            {
                filterNodeID = BuiltinFilter.GetTopLevelKey(FilterTarget.Orders);
            }

            previewSettings.PreviewFilterNode = FilterLayoutContext.Current.FindNode(filterNodeID.Value);
            previewSettings.ClearQuickFilter = true;
            previewSettings.MaintainScroll = maintainScroll;

            UpdatePreview(previewSettings);
        }

        /// <summary>
        /// Clear the preview so no template is being previewed.
        /// </summary>
        public void ClearPreview()
        {
            CancelPendingPreview();

            activeTemplate = null;

            if (activeHtmlControl != null)
            {
                activeHtmlControl.Dispose();
                activeHtmlControl = null;
            }

            labelMessage.Text = messageNoTemplate;

            DisableControls();
        }

        /// <summary>
        /// Update the preview to reflect the given settings
        /// </summary>
        private void UpdatePreview(PreviewSettings settings)
        {
            CancelPendingPreview();

            // Create the new pending preview
            pendingPreview = new PendingPreview();
            pendingPreview.HtmlControl = CreateHtmlControl();
            pendingPreview.Settings = settings;

            // If there is no template, its easy
            if (settings.Template == null)
            {
                pendingPreview.HtmlControl.Html = "";
                pendingPreview.BackgroundProcessingComplete = true;

                ActivatePreview(pendingPreview);

                DisableControls();
            }
            else
            {
                ProgressProvider progressProvider = new ProgressProvider();
                pendingPreview.ProgressProvider = progressProvider;

                // Progress window for long running previews
                ProgressDlg progressDlg = new ProgressDlg(progressProvider);
                progressDlg.Title = "Template Preview";
                progressDlg.Description = "ShipWorks is generating the template preview.";

                ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

                Thread thread = new Thread(ExceptionMonitor.WrapThread(BackgroundProcessPreview, "preparing preview"));
                thread.Name = "Preview Processor";
                thread.Start(new object[] { delayer, progressProvider });

                // Show the progress window if enough time goes by
                delayer.ShowAfter(this, TimeSpan.FromSeconds(.4));
            }
        }

        /// <summary>
        /// Cancel the preview that is being worked in the background, if any
        /// </summary>
        private void CancelPendingPreview()
        {
            if (InvokeRequired)
            {
                throw new InvalidOperationException("This is expected to be called on the UI thread.");
            }

            if (pendingPreview == null)
            {
                return;
            }

            lock (pendingPreview)
            {
                pendingPreview.Canceled = true;

                if (!pendingPreview.BackgroundProcessingComplete)
                {
                    pendingPreview.CanceledEvent = new ManualResetEvent(false);
                    pendingPreview.ProgressProvider.Cancel();
                }
            }

            if (pendingPreview.CanceledEvent != null)
            {
                pendingPreview.CanceledEvent.WaitOne();
                pendingPreview.CanceledEvent.Close();
            }

            pendingPreview = null;
        }

        /// <summary>
        /// Make the current pending preview the active one
        /// </summary>
        [NDependIgnoreLongMethod]
        private void ActivatePreview(PendingPreview preview)
        {
            if (InvokeRequired)
            {
                throw new InvalidOperationException("Cannot activate preview from background thread.");
            }

            if (preview == null)
            {
                throw new InvalidOperationException("There is no pending preview to activate.");
            }

            lastAutoZoom = 0;
            lastClientSize = Size;

            activeTemplate = preview.Settings.Template;
            activeZoom = preview.Settings.Zoom;
            activePreviewSource = preview.Settings.PreviewSource;
            activePreviewCount = preview.Settings.PreviewCount;

            previewFilter.SelectedFilterNodeChanged -= this.OnPreviewFilterChanged;

            if (preview.Settings.ClearQuickFilter)
            {
                previewFilter.ClearQuickFilter();
                preview.Settings.ClearQuickFilter = false;
            }

            previewFilter.SelectedFilterNode = pendingPreview.Settings.PreviewFilterNode;
            previewFilter.SelectedFilterNodeChanged += this.OnPreviewFilterChanged;

            HtmlControl htmlControl = preview.HtmlControl;

            // Dont cause the flicker\scroll change if the html didn't change
            if (activeHtmlControl == null || activeHtmlControl.Html != htmlControl.Html)
            {
                htmlControl.Dock = DockStyle.Fill;
                htmlControl.BringToFront();

                int scrollTop = 0;
                int scrollLeft = 0;

                if (activeHtmlControl != null)
                {
                    if (preview.Settings.MaintainScroll)
                    {
                        scrollLeft = ((HtmlApi.IHTMLElement2) activeHtmlControl.HtmlDocument.Body).GetScrollLeft();
                        scrollTop = ((HtmlApi.IHTMLElement2) activeHtmlControl.HtmlDocument.Body).GetScrollTop();
                    }

                    activeHtmlControl.Dispose();
                }

                activeHtmlControl = htmlControl;

                if (scrollTop > 0 || scrollLeft > 0)
                {
                    ((HtmlApi.IHTMLElement2) activeHtmlControl.HtmlDocument.Body).SetScrollLeft(scrollLeft);
                    ((HtmlApi.IHTMLElement2) activeHtmlControl.HtmlDocument.Body).SetScrollTop(scrollTop);
                }
            }

            UpdatePreviewSourceUI();
        }

        /// <summary>
        /// Do the template processing and return the results.  Any progress can be reported using the specified provider.
        /// </summary>
        [NDependIgnoreLongMethod]
        void BackgroundProcessPreview(object state)
        {
            object[] asyncData = (object[]) state;
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) asyncData[0];
            ProgressProvider progressProvider = (ProgressProvider) asyncData[1];

            // Progress item for running the transform and loading the html
            ProgressItem processProgress = new ProgressItem("Process Template");

            // Add it to the progress list and start it
            progressProvider.ProgressItems.Add(processProgress);
            processProgress.Starting();

            TemplateEntity template = pendingPreview.Settings.Template;
            HtmlControl htmlControl = pendingPreview.HtmlControl;

            int inputCount = 0;
            int resultCount = 0;

            Exception error = null;

            try
            {
                processProgress.Detail = "Finding sample data using filter...";
                List<long> previewKeys = GetPreviewDataKeys(pendingPreview.Settings);
                inputCount = previewKeys.Count;

                // Process the results
                IList<TemplateResult> results = TemplateProcessor.ProcessTemplate(template, previewKeys, processProgress);
                resultCount = results.Count;

                if (template.Type == (int) TemplateType.Thermal)
                {
                    htmlControl.Html = TemplateHelper.ThermalTemplateDisplayHtml;
                }
                else if (template.IsSnippet)
                {
                    htmlControl.Html = TemplateHelper.SnippetTemplateDisplayHtml;
                }
                else if (results.Count > 0)
                {
                    // Update the html and wait for it to complete
                    processProgress.Detail = "Loading results...";

                    htmlControl.Html = TemplateResultFormatter.FormatHtml(results, TemplateResultUsage.ShipWorksDisplay, TemplateResultFormatSettings.FromTemplate(template));
                    htmlControl.WaitForComplete(TimeSpan.FromSeconds(3));

                    // Do SureSize
                    TemplateSureSizeProcessor.Process(htmlControl);

                    // Do zoom
                    ApplyHtmlZoom(pendingPreview.Settings.Zoom, htmlControl);
                }
                else
                {
                    htmlControl.Html = "";
                }

                processProgress.Completed();
            }
            catch (TemplateCancelException ex)
            {
                error = ex;
            }
            catch (Exception ex)
            {
                processProgress.Failed(ex);
                error = ex;
            }

            lock (pendingPreview)
            {
                if (pendingPreview.Canceled)
                {
                    pendingPreview.CanceledEvent.Set();
                }
                else
                {
                    pendingPreview.BackgroundProcessingComplete = true;
                }

                BeginInvoke(new MethodInvoker<PendingPreview, int, int, Exception, ProgressDisplayDelayer>(BackgroundProcessPreviewComplete),
                    pendingPreview, inputCount, resultCount, error, delayer);
            }
        }

        /// <summary>
        /// The processing completed
        /// </summary>
        void BackgroundProcessPreviewComplete(PendingPreview preview, int inputCount, int resultCount, Exception error, ProgressDisplayDelayer delayer)
        {
            // Notified that the action has completed and the ProgressDlg can close
            delayer.NotifyComplete();

            if (preview.Canceled)
            {
                return;
            }

            // Activate the pending preview as now being the current
            ActivatePreview(preview);

            // Check for known errors
            TemplateException templateEx = error as TemplateException;
            if (templateEx != null)
            {
                // Canceled
                if (templateEx is TemplateCancelException)
                {
                    activeHtmlControl.Html = "";
                    activeHtmlControl.Visible = false;
                    labelMessage.Text = messageCanceled;

                    // Don't allow zooming
                    panelPreviewControls.Visible = true;
                    DisableZoom();
                }
                else
                {
                    // Show the error
                    activeHtmlControl.Visible = true;
                    activeHtmlControl.Html = TemplateResultFormatter.FormatError(templateEx, TemplateOutputFormat.Html);

                    // Don't allow zooming
                    DisableControls();
                }
            }
            // Rethrow an error we dont know how to handle
            else if (error != null)
            {
                throw new ApplicationException(error.Message, error);
            }
            else
            {
                if (activeTemplate.Type == (int) TemplateType.Thermal || activeTemplate.IsSnippet)
                {
                    activeHtmlControl.Visible = true;
                    DisableControls();
                }
                else if (inputCount > 0 && resultCount > 0)
                {
                    activeHtmlControl.Visible = true;

                    // Allow zooming
                    panelPreviewControls.Visible = true;
                    panelZoomControls.Enabled = true;
                }
                else
                {
                    activeHtmlControl.Visible = false;
                    labelMessage.Text = inputCount == 0 ? messageNoData : TemplateHelper.NoResultsErrorMessage;

                    // Don't allow zooming
                    panelPreviewControls.Visible = true;
                    DisableZoom();
                }
            }

            UpdateZoom();
        }

        /// <summary>
        /// Get the list of primary keys to use as the source of the preview
        /// </summary>
        private List<long> GetPreviewDataKeys(PreviewSettings settings)
        {
            Stopwatch sw = Stopwatch.StartNew();

            List<long> keys = new List<long>();

            FilterNodeEntity filterNode = settings.PreviewFilterNode;
            if (filterNode != null)
            {
                FilterTarget filterTarget = (FilterTarget) filterNode.Filter.FilterTarget;

                // The entity whose keys we are retrieving
                EntityType previewEntityType = GetPreviewEntityType(settings.PreviewSource);
                EntityField2 previewPkField = EntityUtility.GetPrimaryKeyField(previewEntityType);

                // The entity whose keys are in the filter counts
                EntityType filterEntityType = FilterHelper.GetEntityType(filterTarget);
                EntityField2 filterPkField = EntityUtility.GetPrimaryKeyField(filterEntityType);

                // Create the bucket, we are going to filter on our node count id
                RelationPredicateBucket bucket = new RelationPredicateBucket();

                if (!BuiltinFilter.IsTopLevelKey(filterNode.FilterNodeID))
                {
                    // We need the filter object pk type to be the object id in the count match
                    // This does a WHERE (FilterPKField(i.e. OrderID) IN SELECT (ObjectID FROM FilterNodeContentDetail WHERE FilterNodeContentID = filterNode.FilterNodeContentID))
                    bucket.PredicateExpression.AddWithAnd(new FieldCompareSetPredicate(filterPkField, null, FilterNodeContentDetailFields.ObjectID, null, SetOperator.In,
                        FilterNodeContentDetailFields.FilterNodeContentID == filterNode.FilterNodeContentID));
                }

                // We need to get from the desired pk, to the pk type of the filter
                bucket.Relations.AddRange(EntityUtility.FindRelationChain(previewEntityType, filterEntityType));

                // We need to try to pull the ids of the context type
                ResultsetFields resultFields = new ResultsetFields(1);
                resultFields.DefineField(previewPkField, 0, "EntityID", "");

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    using (SqlDataReader reader = (SqlDataReader) adapter.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, settings.PreviewCount < 0 ? 0 : settings.PreviewCount, null, false))
                    {
                        while (reader.Read())
                        {
                            keys.Add(reader.GetInt64(0));
                        }
                    }
                }
            }

            log.DebugFormat("Get preview keys: {0}", sw.Elapsed.TotalSeconds);

            return keys;
        }

        #endregion

        #region Preview Source

        /// <summary>
        /// The data source for the preview is changing
        /// </summary>
        private void OnChangePreviewSource(object sender, EventArgs e)
        {
            SandMenuItem menuItem = (SandMenuItem) sender;
            object[] tag = (object[]) menuItem.Tag;

            activePreviewSource = (TemplatePreviewSource) tag[0];
            activePreviewCount = (int) tag[1];

            UpdatePreview(ActivePreviewSettings);
        }

        /// <summary>
        /// The filter selected for previewing has changed
        /// </summary>
        private void OnPreviewFilterChanged(object sender, EventArgs e)
        {
            // We have to BeginInvoke this, because if we didnt, when the filter combo box popup returned we would be disabled
            // due to the waiting on the preview to update, and the focus cant get set back, and then focus was going to another app.
            BeginInvoke((MethodInvoker) delegate
            {
                UpdatePreview(ActivePreviewSettings);
            });
        }

        /// <summary>
        /// Update the UI for the preview source based on the template settings
        /// </summary>
        private void UpdatePreviewSourceUI()
        {
            if (activeTemplate != null)
            {
                string type = EnumHelper.GetDescription(activePreviewSource).ToLowerInvariant();

                // All, or > 1
                if (activePreviewCount != 1)
                {
                    type += "s";
                }

                string text = string.Format("{0} {1}", activePreviewCount == -1 ? "All" : activePreviewCount.ToString(), type);

                previewSource.Text = text;

                panelPreviewFilter.Visible = true;
                panelPreviewFilter.Left = previewSource.Right - 4;
                previewFilter.Parent.Refresh();

                panelPreviewFilter.Width = previewFilter.Right + 2;
                panelPreviewControls.Width = panelPreviewFilter.Right;

                UpdateCheckedPreviewMenuItem();
            }

            UpdateToolPanelPositioning();
            panelPreviewControls.Refresh();
        }

        /// <summary>
        /// Update the menu item that is checked in the preview source menu
        /// </summary>
        private void UpdateCheckedPreviewMenuItem()
        {
            foreach (SandMenuItem item in checkablePreviewItems)
            {
                object[] tag = item.Tag as object[];

                if (activePreviewSource == (TemplatePreviewSource) tag[0] &&
                    activePreviewCount == (int) tag[1])
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }
        }

        /// <summary>
        /// Show the preview source menu
        /// </summary>
        private void OnPreviewSourceMouseDown(object sender, MouseEventArgs e)
        {
            previewSourceMenu.ShowStandalone(previewSource, new Point(0, previewSource.Height), false);
        }

        /// <summary>
        /// Get the EntityType that is the subject of the given source
        /// </summary>
        private EntityType GetPreviewEntityType(TemplatePreviewSource previewSource)
        {
            switch (previewSource)
            {
                case TemplatePreviewSource.Customer:
                    return EntityType.CustomerEntity;

                case TemplatePreviewSource.Order:
                    return EntityType.OrderEntity;

                case TemplatePreviewSource.Shipment:
                    return EntityType.ShipmentEntity;

            }

            throw new InvalidOperationException("Unhandled previewSource in GetPreviewEntityType");
        }

        #endregion

        #region Zoom

        /// <summary>
        /// Update the zoom level of the browser, and the zoom controls, to match the current zoom settings.
        /// </summary>
        private void UpdateZoom()
        {
            if (activeTemplate == null)
            {
                return;
            }

            // This will be the case when we are in an error state
            if (!panelZoomControls.Enabled)
            {
                return;
            }

            // Flicker reduction - if we are already at 100%, and we are getting bigger, no need.
            if (lastAutoZoom >= 100 && lastClientSize.Width < Size.Width)
            {
                lastClientSize = Size;
                return;
            }

            lastClientSize = Size;

            ApplyHtmlZoom(activeZoom, activeHtmlControl);
            UpdateZoomControls();
        }

        /// <summary>
        /// Apply the given html zoom to the html control
        /// </summary>
        private void ApplyHtmlZoom(string currentZoom, HtmlControl htmlControl)
        {
            if (currentZoom == FitAll)
            {
                lastAutoZoom = htmlControl.ZoomToFit();
            }
            else if (currentZoom == FitWidth)
            {
                lastAutoZoom = htmlControl.ZoomToWidth();
            }
            else
            {
                int zoom;
                if (!Int32.TryParse(currentZoom.Replace("%", ""), out zoom))
                {
                    zoom = 100;
                }

                htmlControl.ZoomToExact(zoom);
                lastAutoZoom = 0;
            }
        }        
        
        /// <summary>
        /// Update the GUI controls to reflect the current state of the zoom
        /// </summary>
        private void UpdateZoomControls()
        {
            int zoom = (int) Math.Round(activeHtmlControl.GetZoomPercent());

            zoomBar.Value = Math.Min(Math.Max(zoom, zoomBar.Minimum), zoomBar.Maximum);

            // Not sure why, but sometimes this wasnt repainting properly
            panelZoomControls.Refresh();

            zoomout.Enabled = (zoom > zoomBar.Minimum);
            zoomin.Enabled = (zoom < zoomBar.Maximum);

            zoomCombo.SelectedIndexChanged -= new EventHandler(OnZoomSpecific);

            if (!activeZoom.StartsWith("Fit"))
            {
                zoomCombo.Text = string.Format("{0}%", zoom);
            }
            else
            {
                zoomCombo.Text = activeZoom;
            }

            zoomCombo.SelectedIndexChanged += new EventHandler(OnZoomSpecific);
        }
    
        /// <summary>
        /// User want to zoom
        /// </summary>
        private void OnZoomButton(object sender, EventArgs e)
        {
            double zoom = activeHtmlControl.GetZoomPercent();

            if (sender == zoomin)
            {
                zoom = Math.Min(zoomBar.Maximum, zoom + 25);
            }
            else
            {
                zoom = Math.Max(zoomBar.Minimum, zoom - 25);
            }

            // Save the new value
            activeZoom = string.Format("{0}%", (int) Math.Round(zoom));

            // Update the zoom and the controls
            UpdateZoom();
        }

        /// <summary>
        /// User is zooming.
        /// </summary>
        private void OnZoomSlider(object sender, System.EventArgs e)
        {
            // Save the new value
            activeZoom = string.Format("{0}%", zoomBar.Value);

            // Update the zoom and the controls
            UpdateZoom();
        }

        /// <summary>
        /// User used the combo to specify a specific zoom level
        /// </summary>
        private void OnZoomSpecific(object sender, System.EventArgs e)
        {
            if (zoomCombo.Text == FitWidth || zoomCombo.Text == FitAll)
            {
                activeZoom = zoomCombo.Text;
            }
            else
            {
                int zoom;
                if (Int32.TryParse(zoomCombo.Text.Replace("%", ""), out zoom))
                {
                    zoom = (int) Math.Min(Math.Max(zoom, 1), 800);

                    // Save the new value
                    activeZoom = string.Format("{0}%", zoom);
                }
            }

            // Update the zoom and the controls
            UpdateZoom();
        }

        /// <summary>
        /// Leaving the Zoom Specific control
        /// </summary>
        private void OnValidatingZoomSpecific(object sender, CancelEventArgs e)
        {
            OnZoomSpecific(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Trap the enter key
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (zoomCombo.Focused)
                {
                    OnZoomSpecific(null, EventArgs.Empty);

                    return true;
                }
            }

            return base.ProcessDialogKey(keyData);
        }


        #endregion

        #region Tool Panel Positioning

        /// <summary>
        /// Size of this control is changing
        /// </summary>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            UpdateToolPanelPositioning();
            UpdateZoom();
        }

        /// <summary>
        /// The size of the preview panel is changing, so we have to update the tool panel positioning
        /// </summary>
        private void OnPreviewPanelSizeChanged(object sender, EventArgs e)
        {
            UpdateToolPanelPositioning();
        }

        /// <summary>
        /// The position of the preview panel is changing, so we have to update the tool panel positioning
        /// </summary>
        private void OnPreviewPanelVisibleChanged(object sender, EventArgs e)
        {
            UpdateToolPanelPositioning();
        }

        /// <summary>
        /// Update the tool panel height based on the control state
        /// </summary>
        private void UpdateToolPanelPositioning()
        {
            // Overlapping
            if (panelPreviewControls.Visible && panelZoomControls.Left <= panelPreviewControls.Right)
            {
                panelZoomControls.Top = panelPreviewControls.Bottom + 5;
            }
            else
            {
                panelZoomControls.Top = panelPreviewControls.Top;
            }

            panelTools.Height = panelZoomControls.Bottom + 5;
        }

        #endregion
    }
}
