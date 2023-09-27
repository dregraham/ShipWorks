using ShipWorks.UI.Controls.SandRibbon;

namespace ShipWorks
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Divelements.SandRibbon.StatusBarStrip statusBarStrip;
            Divelements.SandRibbon.StripLayout stripLayout6;
            Divelements.SandRibbon.StripLayout stripLayoutDatabase;
            Divelements.SandRibbon.StripLayout dataViewDetailSettingsStrip;
            Divelements.SandRibbon.StripLayout stripLayout2;
            Divelements.SandRibbon.StripLayout stripLayout1;
            Divelements.SandRibbon.StripLayout stripLayout3;
            Divelements.SandRibbon.Menu menuFedExEndOfDay;
            Divelements.SandRibbon.Menu menuOrderLookupViewFedExEndOfDay;
            Divelements.SandRibbon.Shortcut shortcut1;
            Divelements.SandRibbon.Menu menu1;
            Divelements.SandRibbon.Menu menu2;
            Divelements.SandRibbon.Rendering.RibbonRenderer ribbonRenderer1 = new Divelements.SandRibbon.Rendering.RibbonRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelStatusTotal = new Divelements.SandRibbon.Label();
            this.labelStatusSelected = new Divelements.SandRibbon.Label();
            this.statusStretcherPlaceholder = new Divelements.SandRibbon.Label();
            this.downloadingStatusLabel = new ShipWorks.UI.Controls.SandRibbon.ImageLabel();
            this.emailingStatusLabel = new ShipWorks.UI.Controls.SandRibbon.ImageLabel();
            this.buttonNewCustomer = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonDeleteCustomer = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonChangeConnection = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonFirewall = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.labelDetailViewDetailView = new Divelements.SandRibbon.Label();
            this.detailViewDetailTemplate = new Divelements.SandRibbon.ComboBox();
            this.detailViewDetailHeight = new Divelements.SandRibbon.WindowsComboBox();
            this.dataViewHeightButtons = new Divelements.SandRibbon.ButtonGroup();
            this.buttonDetailViewHeightIncrease = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonDetailViewHeightDecrease = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonManageFilters = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonManageTemplates = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonManageActions = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonShippingSettings = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonShippingProfiles = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonSettings = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewFields = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonManageStores = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonManageUsers = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonEmailAccounts = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.menuFedExEndDayClose = new Divelements.SandRibbon.MenuItem();
            this.menuFedExEndDayPrint = new Divelements.SandRibbon.MenuItem();
            this.menuFedExPrintReports = new Divelements.SandRibbon.Menu();
            this.menuFedExEndDayPrintPlaceholder = new Divelements.SandRibbon.MenuItem();
            this.menuFedExSmartPostClose = new Divelements.SandRibbon.MenuItem();
            this.menuItemViewHelp = new RibbonMenuItem();
            this.menuItemSupportForum = new RibbonMenuItem();
            this.menuItemRequestHelp = new RibbonMenuItem();
            this.menuItemRemoteAssistance = new RibbonMenuItem();
            this.menuItemBuySupplies = new RibbonMenuItem();
            this.menuItemHelpAbout = new RibbonMenuItem();
            this.mainMenuItemSetupDatabase = new RibbonMainMenuItem();
            this.mainMenuItemBackupDatabase = new RibbonMainMenuItem();
            this.uiModeMenu = new Divelements.SandRibbon.Menu();
            this.mainMenuItemOrderLookup = new RibbonMainMenuItem();
            this.mainMenuItemBatchGrid = new RibbonMainMenuItem();
            this.mainMenuItemProducts = new RibbonMainMenuItem();
            this.menuOrderLookupViewFedExEndDayClose = new Divelements.SandRibbon.MenuItem();
            this.menuOrderLookupViewFedExEndDayPrint = new Divelements.SandRibbon.MenuItem();
            this.menuOrderLookupViewFedExPrintReports = new Divelements.SandRibbon.Menu();
            this.menuOrderLookupViewFedExEndDayPrintPlaceholder = new Divelements.SandRibbon.MenuItem();
            this.menuOrderLookupViewFedExSmartPostClose = new Divelements.SandRibbon.MenuItem();
            this.buttonRestore = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonArchive = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonSetupDatabase = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.stripLayoutModifyOrders = new Divelements.SandRibbon.StripLayout();
            this.buttonNewOrder = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonDeleteOrders = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.stripLayoutCombineSplitOrders = new Divelements.SandRibbon.StripLayout();
            this.buttonCombine = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonSplit = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonEditCustomer = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonEditOrder = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.panelArchiveNotification = new System.Windows.Forms.Panel();
            this.dashboardBarSample = new ShipWorks.ApplicationCore.Dashboard.DashboardBar();
            this.panelDockingArea = new System.Windows.Forms.Panel();
            this.gridControl = new ShipWorks.ApplicationCore.MainGridControl();
            this.dockContainer1 = new TD.SandDock.DockContainer();
            this.dockableWindowOrders = new TD.SandDock.DockableWindow();
            this.panelOrders = new ShipWorks.Stores.Content.Panels.OrdersPanel();
            this.dockableWindowItems = new TD.SandDock.DockableWindow();
            this.panelItems = new ShipWorks.Stores.Content.Panels.OrderItemsPanel();
            this.dockableWindowCharges = new TD.SandDock.DockableWindow();
            this.panelCharges = new ShipWorks.Stores.Content.Panels.OrderChargesPanel();
            this.dockableWindowMap = new TD.SandDock.DockableWindow();
            this.panelMap = new ShipWorks.Stores.Content.Panels.MapPanel();
            this.dockableWindowStreetView = new TD.SandDock.DockableWindow();
            this.panelStreetView = new ShipWorks.Stores.Content.Panels.MapPanel();
            this.dockableWindowPaymentDetails = new TD.SandDock.DockableWindow();
            this.panelPaymentDetail = new ShipWorks.Stores.Content.Panels.PaymentDetailsPanel();
            this.dockableWindowShipments = new TD.SandDock.DockableWindow();
            this.panelShipments = new ShipWorks.Stores.Content.Panels.ShipmentsPanel();
            this.dockableWindowEmail = new TD.SandDock.DockableWindow();
            this.panelEmail = new ShipWorks.Stores.Content.Panels.EmailOutboundPanel();
            this.dockableWindowPrinted = new TD.SandDock.DockableWindow();
            this.panelPrinted = new ShipWorks.Stores.Content.Panels.PrintResultsPanel();
            this.sandDockManager = new TD.SandDock.SandDockManager();
            this.dockContainer = new TD.SandDock.DockContainer();
            this.dockableWindowOrderFilters = new TD.SandDock.DockableWindow();
            this.orderFilterTree = new ShipWorks.Filters.Controls.FilterTree();
            this.dockableWindowCustomerFilters = new TD.SandDock.DockableWindow();
            this.customerFilterTree = new ShipWorks.Filters.Controls.FilterTree();
            this.dockableWindowNotes = new TD.SandDock.DockableWindow();
            this.panelNotes = new ShipWorks.Stores.Content.Panels.NotesPanel();
            this.ribbonManager = new Divelements.SandRibbon.RibbonManager(this.components);
            this.statusBar = new Divelements.SandRibbon.StatusBar();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOpenShipWorks = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonManager = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.contextMenuOrderGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextOrderEditOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderCombineOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSplitOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOrderLocalStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.stuffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderOnlineUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.orderContextUpdateOnlineEbay = new System.Windows.Forms.ToolStripMenuItem();
            this.updateShipmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsShippedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsNotShippedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderContextUpdateOnlineOsCommerce = new System.Windows.Forms.ToolStripMenuItem();
            this.orderContextUpdateOnlineOsCommercePlaceholder = new System.Windows.Forms.ToolStripMenuItem();
            this.commonYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.storeAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specificZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderCustomActions = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSep6 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOrderEditCustomer = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderLookupCustomer = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOrderShipOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderTrackShipments = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderInsuranceClaim = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOrderCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSep7 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOrderQuickPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuTemplatesPlaceholder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuTemplatesPlaceholderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOrderEmailNow = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderComposeEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSep5 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOrderSave = new System.Windows.Forms.ToolStripMenuItem();
            this.contextOrderSaveOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerSaveOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerEmailNow = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerSave = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerComposeEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuCustomerGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextCustomerEditCustomer = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerCustomActionsSep = new System.Windows.Forms.ToolStripSeparator();
            this.contextCustomerCustomActions = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextCustomerNewOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerLookupOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextCustomerCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCustomerSep5 = new System.Windows.Forms.ToolStripSeparator();
            this.contextCustomerSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextCustomerSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.gridMenuLayoutProvider = new ShipWorks.ApplicationCore.Appearance.GridMenuLayoutProvider(this.components);
            this.windowLayoutProvider = new ShipWorks.ApplicationCore.Appearance.WindowLayoutProvider(this.components);
            this.selectionDependentEnabler = new ShipWorks.ApplicationCore.Interaction.SelectionDependentEnabler(this.components);
            this.buttonShipOrders = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonTrackOrders = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonLocalStatus = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupLocalStatus = new Divelements.SandRibbon.Popup();
            this.buttonUpdateOnline = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupUpdateOnline = new Divelements.SandRibbon.Popup();
            this.buttonPrint = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupPrint = new Divelements.SandRibbon.Popup();
            this.buttonPreview = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupPreview = new Divelements.SandRibbon.Popup();
            this.buttonEmailSend = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupEmailSend = new Divelements.SandRibbon.Popup();
            this.buttonEmailCompose = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupEmailCompose = new Divelements.SandRibbon.Popup();
            this.buttonSave = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupSave = new Divelements.SandRibbon.Popup();
            this.buttonSaveOpen = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupSaveOpen = new Divelements.SandRibbon.Popup();
            this.popupAddProduct = new Divelements.SandRibbon.Popup();
            this.buttonQuickPrint = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupQuickPrint = new Divelements.SandRibbon.Popup();
            this.buttonInsuranceClaim = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbon = new Divelements.SandRibbon.Ribbon();
            this.applicationMenu = new Divelements.SandRibbon.ApplicationMenu();
            this.mainMenuItemOptions = new RibbonMainMenuItem();
            this.mainMenuItemSupport = new Divelements.SandRibbon.MainMenuItem();
            this.mainMenuItemDatabase = new Divelements.SandRibbon.MainMenuItem();
            this.mainMenuLogon = new Divelements.SandRibbon.MainMenuItem();
            this.ribbonTabGridViewHome = new Divelements.SandRibbon.RibbonTab();
            this.ribbonChunkOrders = new Divelements.SandRibbon.RibbonChunk();
            this.ribbonChunkOrdersSep1 = new Divelements.SandRibbon.Separator();
            this.ribbonChunkCustomers = new Divelements.SandRibbon.RibbonChunk();
            this.ribbonChunkShipping = new Divelements.SandRibbon.RibbonChunk();
            this.buttonFedExClose = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonAsendiaClose = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonDhlEcommerceManifest = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupDhlEcommerceManifest = new Divelements.SandRibbon.Popup();
            this.popupOrderLookupViewDhlEcommerceManifest = new Divelements.SandRibbon.Popup();
            this.menuDhlEcommerceManifest = new Divelements.SandRibbon.Menu();
            this.menuOrderLookupViewDhlEcommerceManifest = new Divelements.SandRibbon.Menu();
            this.menuItemCreateDhlEcommerceManifest = new Divelements.SandRibbon.MenuItem();
            this.menuItemOrderLookupViewCreateDhlEcommerceManifest = new Divelements.SandRibbon.MenuItem();
            this.menuItemPrintDhlEcommerceManifest = new Divelements.SandRibbon.MenuItem();
            this.menuItemOrderLookupViewPrintDhlEcommerceManifest = new Divelements.SandRibbon.MenuItem();
            this.menuPrintDhlEcommerceManifest = new Divelements.SandRibbon.Menu();
            this.menuOrderLookupViewPrintDhlEcommerceManifest = new Divelements.SandRibbon.Menu();
            this.menuItemDhlEcommerceManifestNone = new Divelements.SandRibbon.MenuItem();
            this.menuItemOrderLookupViewDhlEcommerceManifestNone = new Divelements.SandRibbon.MenuItem();
            this.popupFedExEndOfDay = new Divelements.SandRibbon.Popup();
            this.buttonEndiciaSCAN = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupPostalScanForm = new Divelements.SandRibbon.Popup();
            this.menuEndiciaScanForm = new Divelements.SandRibbon.Menu();
            this.menuOrderLookupViewEndiciaScanForm = new Divelements.SandRibbon.Menu();
            this.menuCreateEndiciaScanForm = new Divelements.SandRibbon.MenuItem();
            this.menuOrderLookupViewCreateEndiciaScanForm = new Divelements.SandRibbon.MenuItem();
            this.menuPrintScanForm = new Divelements.SandRibbon.MenuItem();
            this.menuOrderLookupViewPrintScanForm = new Divelements.SandRibbon.MenuItem();
            this.menuPrintEndiciaScanForm = new Divelements.SandRibbon.Menu();
            this.menuOrderLookupViewPrintEndiciaScanForm = new Divelements.SandRibbon.Menu();
            this.menuEndiciaScanFormNone = new Divelements.SandRibbon.MenuItem();
            this.menuOrderLookupViewEndiciaScanFormNone = new Divelements.SandRibbon.MenuItem();
            this.ribbonChunkManageEmail = new Divelements.SandRibbon.RibbonChunk();
            this.buttonEmailMessages = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonChunkDownload = new Divelements.SandRibbon.RibbonChunk();
            this.buttonDownload = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonChunkQuickStart= new Divelements.SandRibbon.RibbonChunk();
            this.buttonQuickStart = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonChunkPickList = new Divelements.SandRibbon.RibbonChunk();
            this.buttonPickList = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonTabGridOutput = new Divelements.SandRibbon.RibbonTab();
            this.ribbonChunkPrint = new Divelements.SandRibbon.RibbonChunk();
            this.ribbonChunkSendEmail = new Divelements.SandRibbon.RibbonChunk();
            this.ribbonChunkFile = new Divelements.SandRibbon.RibbonChunk();
            this.ribbonTabProducts = new Divelements.SandRibbon.RibbonTab();
            this.ribbonTabOrderLookupViewShipping = new Divelements.SandRibbon.RibbonTab();
            this.ribbonChunkOrderLookupViewShipping = new Divelements.SandRibbon.RibbonChunk();
            this.ribbonChunkProductCatalogViewActions = new Divelements.SandRibbon.RibbonChunk();
            this.buttonOrderLookupViewCreateLabel = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewNewShipment = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewShipShipAgain = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewReprint2 = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewUnverify = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonProductCatalogAddProduct = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonProductCatalogEditProduct = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonProductCatalogExportProduct = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.menuAddProduct = new Divelements.SandRibbon.Menu();
            this.addProductMenuItemFromFile = new ShipWorks.UI.Controls.SandRibbon.RibbonMainMenuItem(); 
            this.addProductMenuItemNewProduct = new ShipWorks.UI.Controls.SandRibbon.RibbonMainMenuItem();
            this.addProductMenuItemVariant = new ShipWorks.UI.Controls.SandRibbon.RibbonMainMenuItem();
            this.ribbonChunkOrderLookupViewProfiles = new Divelements.SandRibbon.RibbonChunk();
            this.buttonOrderLookupViewApplyProfile = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewManageProfiles = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonTabOrderLookupViewShipmentHistory = new Divelements.SandRibbon.RibbonTab();
            this.ribbonChunkOrderLookupViewActions = new Divelements.SandRibbon.RibbonChunk();
            this.buttonOrderLookupViewVoid = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewReprint = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewShipAgain = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewSCANForm = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupOrderLookupViewSCANForm = new Divelements.SandRibbon.Popup();
            this.buttonOrderLookupViewFedExClose = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewAsendiaClose = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonOrderLookupViewDhlEcommerceManifest = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupOrderLookupViewFedExEndOfDay = new Divelements.SandRibbon.Popup();
            this.ribbonTabAdmin = new Divelements.SandRibbon.RibbonTab();
            this.ribbonChunkConfiguration = new Divelements.SandRibbon.RibbonChunk();
            this.ribbonChunkHistory = new Divelements.SandRibbon.RibbonChunk();
            this.buttonAudit = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonDownloadHistory = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonChunkAdminDatabase = new Divelements.SandRibbon.RibbonChunk();
            this.separator2 = new Divelements.SandRibbon.Separator();
            this.buttonBackup = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonTabView = new Divelements.SandRibbon.RibbonTab();
            this.ribbonChunkDataViews = new Divelements.SandRibbon.RibbonChunk();
            this.buttonDetailViewNormal = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonDetailViewNormalDetail = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonDetailViewDetail = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.separatorDataViews = new Divelements.SandRibbon.Separator();
            this.ribbonChunkGridSettings = new Divelements.SandRibbon.RibbonChunk();
            this.buttonEditGridColumns = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonEditGridMenus = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonChunkPanels = new Divelements.SandRibbon.RibbonChunk();
            this.buttonShowPanels = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.popupShowPanels = new Divelements.SandRibbon.Popup();
            this.menuShowPanels = new Divelements.SandRibbon.Menu();
            this.menuItemShowFiltersPanel = new Divelements.SandRibbon.MenuItem();
            this.ribbonChunkEnvironment = new Divelements.SandRibbon.RibbonChunk();
            this.buttonSaveEnvironment = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonLoadEnvironment = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonResetEnvironment = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonTabHelp = new Divelements.SandRibbon.RibbonTab();
            this.ribbonChunkSupport = new Divelements.SandRibbon.RibbonChunk();
            this.buttonHelpView = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonHelpForum = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonRequestHelp = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.separator1 = new Divelements.SandRibbon.Separator();
            this.buttonHelpRemote = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonChunkSupplies = new Divelements.SandRibbon.RibbonChunk();
            this.buttonUship = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.buttonBuySupplies = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.ribbonChunkAbout = new Divelements.SandRibbon.RibbonChunk();
            this.buttonHelpAbout = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            this.quickAccessToolBar = new Divelements.SandRibbon.QuickAccessToolBar();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.dashboardArea = new System.Windows.Forms.Panel();
            this.ribbonSecurityProvider = new ShipWorks.Users.Security.RibbonSecurityProvider(this.components);
            this.editionGuiHelper = new ShipWorks.Editions.EditionGuiHelper(this.components);
            this.menuItem1 = new Divelements.SandRibbon.MenuItem();
            this.popup1 = new Divelements.SandRibbon.Popup();
            this.popup2 = new Divelements.SandRibbon.Popup();
            this.buttonCreateLabel = new ShipWorks.UI.Controls.SandRibbon.RibbonButton();
            statusBarStrip = new Divelements.SandRibbon.StatusBarStrip();
            stripLayout6 = new Divelements.SandRibbon.StripLayout();
            stripLayoutDatabase = new Divelements.SandRibbon.StripLayout();
            dataViewDetailSettingsStrip = new Divelements.SandRibbon.StripLayout();
            stripLayout2 = new Divelements.SandRibbon.StripLayout();
            stripLayout1 = new Divelements.SandRibbon.StripLayout();
            stripLayout3 = new Divelements.SandRibbon.StripLayout();
            menuFedExEndOfDay = new Divelements.SandRibbon.Menu();
            menuOrderLookupViewFedExEndOfDay = new Divelements.SandRibbon.Menu();
            shortcut1 = new Divelements.SandRibbon.Shortcut();
            menu1 = new Divelements.SandRibbon.Menu();
            menu2 = new Divelements.SandRibbon.Menu();
            ((System.ComponentModel.ISupportInitialize) (this.downloadingStatusLabel.PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.emailingStatusLabel.PictureBox)).BeginInit();
            this.panelDockingArea.SuspendLayout();
            this.dockContainer1.SuspendLayout();
            this.dockableWindowOrders.SuspendLayout();
            this.dockableWindowItems.SuspendLayout();
            this.dockableWindowCharges.SuspendLayout();
            this.dockableWindowMap.SuspendLayout();
            this.dockableWindowStreetView.SuspendLayout();
            this.dockableWindowPaymentDetails.SuspendLayout();
            this.dockableWindowShipments.SuspendLayout();
            this.dockableWindowEmail.SuspendLayout();
            this.dockableWindowPrinted.SuspendLayout();
            this.dockContainer.SuspendLayout();
            this.dockableWindowOrderFilters.SuspendLayout();
            this.dockableWindowCustomerFilters.SuspendLayout();
            this.dockableWindowNotes.SuspendLayout();
            this.notifyIconMenuStrip.SuspendLayout();
            this.contextMenuOrderGrid.SuspendLayout();
            this.contextMenuTemplatesPlaceholder.SuspendLayout();
            this.contextMenuCustomerGrid.SuspendLayout();
            this.ribbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox8)).BeginInit();
            this.dashboardArea.SuspendLayout();
            this.SuspendLayout();
            //
            // statusBarStrip
            //
            statusBarStrip.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.labelStatusTotal,
            this.labelStatusSelected,
            this.statusStretcherPlaceholder,
            this.downloadingStatusLabel,
            this.emailingStatusLabel});
            //
            // labelStatusTotal
            //
            this.labelStatusTotal.Text = "Orders: 0";
            //
            // labelStatusSelected
            //
            this.labelStatusSelected.Text = "Selected: 0";
            //
            // statusStretcherPlaceholder
            //
            this.statusStretcherPlaceholder.Margin = new Divelements.SandRibbon.WidgetEdges(0, 0, 9, 0);
            this.statusStretcherPlaceholder.Stretch = true;
            //
            // downloadingStatusLabel
            //
            this.downloadingStatusLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.downloadingStatusLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.downloadingStatusLabel.Margin = new Divelements.SandRibbon.WidgetEdges(0, 0, 9, 0);
            //
            //
            //
            this.downloadingStatusLabel.PictureBox.BackColor = System.Drawing.Color.Transparent;
            this.downloadingStatusLabel.PictureBox.Image = global::ShipWorks.Properties.Resources.arrows_greengray;
            this.downloadingStatusLabel.PictureBox.Location = new System.Drawing.Point(856, 4);
            this.downloadingStatusLabel.PictureBox.Name = "";
            this.downloadingStatusLabel.PictureBox.Size = new System.Drawing.Size(14, 14);
            this.downloadingStatusLabel.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.downloadingStatusLabel.PictureBox.TabIndex = 0;
            this.downloadingStatusLabel.PictureBox.TabStop = false;
            this.downloadingStatusLabel.Text = "Downloading";
            this.downloadingStatusLabel.Visible = false;
            this.downloadingStatusLabel.Click += new System.EventHandler(this.OnClickDownloadStatus);
            //
            // emailingStatusLabel
            //
            this.emailingStatusLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.emailingStatusLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailingStatusLabel.Margin = new Divelements.SandRibbon.WidgetEdges(0, 0, 9, 0);
            //
            //
            //
            this.emailingStatusLabel.PictureBox.BackColor = System.Drawing.Color.Transparent;
            this.emailingStatusLabel.PictureBox.Image = global::ShipWorks.Properties.Resources.arrows_greengray;
            this.emailingStatusLabel.PictureBox.Location = new System.Drawing.Point(930, 4);
            this.emailingStatusLabel.PictureBox.Name = "";
            this.emailingStatusLabel.PictureBox.Size = new System.Drawing.Size(14, 14);
            this.emailingStatusLabel.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.emailingStatusLabel.PictureBox.TabIndex = 1;
            this.emailingStatusLabel.PictureBox.TabStop = false;
            this.emailingStatusLabel.Text = "Emailing";
            this.emailingStatusLabel.Visible = false;
            this.emailingStatusLabel.Click += new System.EventHandler(this.OnClickEmailStatus);
            //
            // stripLayout6
            //
            stripLayout6.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonNewCustomer,
            this.buttonDeleteCustomer});
            stripLayout6.ItemSpacing = 6;
            stripLayout6.LayoutDirection = Divelements.SandRibbon.LayoutDirection.Vertical;
            stripLayout6.Margin = new Divelements.SandRibbon.WidgetEdges(0, 3, 0, 0);
            //
            // buttonNewCustomer
            //
            this.buttonNewCustomer.Guid = new System.Guid("e4349726-f217-41a6-b703-9c5ba735dbb5");
            this.buttonNewCustomer.Image = global::ShipWorks.Properties.Resources.customer_add;
            this.ribbonSecurityProvider.SetPermission(this.buttonNewCustomer, ShipWorks.Users.Security.PermissionType.CustomersCreateEdit);
            this.buttonNewCustomer.QuickAccessKey = "CN";
            this.buttonNewCustomer.Text = "New";
            this.buttonNewCustomer.Activate += new System.EventHandler(this.OnNewCustomer);
            //
            // buttonDeleteCustomer
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonDeleteCustomer, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreCustomers);
            this.buttonDeleteCustomer.Guid = new System.Guid("acf571fa-b3ae-4e65-a734-a35a44d36aaf");
            this.buttonDeleteCustomer.Image = global::ShipWorks.Properties.Resources.customer_delete;
            this.ribbonSecurityProvider.SetPermission(this.buttonDeleteCustomer, ShipWorks.Users.Security.PermissionType.CustomersDelete);
            this.buttonDeleteCustomer.QuickAccessKey = "CD";
            this.buttonDeleteCustomer.Text = "Delete";
            this.buttonDeleteCustomer.Activate += new System.EventHandler(this.OnDelete);
            //
            // stripLayoutDatabase
            //
            stripLayoutDatabase.CenterLayout = true;
            stripLayoutDatabase.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonChangeConnection,
            this.buttonFirewall});
            stripLayoutDatabase.ItemSpacing = 1;
            stripLayoutDatabase.LayoutDirection = Divelements.SandRibbon.LayoutDirection.Vertical;
            //
            // buttonChangeConnection
            //
            this.buttonChangeConnection.Guid = new System.Guid("46ef3a52-8a80-49a1-a078-29a6cd8d83a9");
            this.buttonChangeConnection.Image = ((System.Drawing.Image) (resources.GetObject("buttonChangeConnection.Image")));
            this.ribbonSecurityProvider.SetPermission(this.buttonChangeConnection, ShipWorks.Users.Security.PermissionType.DatabaseSetup);
            this.buttonChangeConnection.QuickAccessKey = "L";
            this.buttonChangeConnection.Text = "Change SQL Login";
            this.buttonChangeConnection.Activate += new System.EventHandler(this.OnChangeDatabaseLogon);
            //
            // buttonFirewall
            //
            this.buttonFirewall.Guid = new System.Guid("69f14b3b-b3d0-497e-b887-22824d9f8fbc");
            this.buttonFirewall.Image = ((System.Drawing.Image) (resources.GetObject("buttonFirewall.Image")));
            this.ribbonSecurityProvider.SetPermission(this.buttonFirewall, ShipWorks.Users.Security.PermissionType.DatabaseSetup);
            this.buttonFirewall.QuickAccessKey = "W";
            this.buttonFirewall.Text = "Windows Firewall";
            this.buttonFirewall.Activate += new System.EventHandler(this.OnWindowsFirewall);
            //
            // dataViewDetailSettingsStrip
            //
            dataViewDetailSettingsStrip.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.labelDetailViewDetailView,
            this.detailViewDetailTemplate,
            stripLayout2});
            dataViewDetailSettingsStrip.ItemSpacing = 1;
            dataViewDetailSettingsStrip.LayoutDirection = Divelements.SandRibbon.LayoutDirection.Vertical;
            //
            // labelDetailViewDetailView
            //
            this.labelDetailViewDetailView.Text = "Detail View";
            //
            // detailViewDetailTemplate
            //
            this.detailViewDetailTemplate.DisplaySize = new System.Drawing.Size(130, 20);
            this.detailViewDetailTemplate.Guid = new System.Guid("967b9471-3cbe-446e-83f9-8d9d432cb67c");
            this.detailViewDetailTemplate.QuickAccessKey = "V";
            //
            // stripLayout2
            //
            stripLayout2.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.detailViewDetailHeight,
            this.dataViewHeightButtons});
            //
            // detailViewDetailHeight
            //
            this.detailViewDetailHeight.ControlWidth = 60;
            this.detailViewDetailHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.detailViewDetailHeight.QuickAccessKey = "H";
            this.detailViewDetailHeight.Text = "Height:";
            this.detailViewDetailHeight.SelectedIndexChanged += new System.EventHandler(this.OnChangeDetailViewDetailHeight);
            //
            // dataViewHeightButtons
            //
            this.dataViewHeightButtons.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonDetailViewHeightIncrease,
            this.buttonDetailViewHeightDecrease});
            //
            // buttonDetailViewHeightIncrease
            //
            this.buttonDetailViewHeightIncrease.Guid = new System.Guid("71bb764c-5382-4848-8a81-9341a29dddec");
            this.buttonDetailViewHeightIncrease.Image = global::ShipWorks.Properties.Resources.navigate_plus;
            this.buttonDetailViewHeightIncrease.QuickAccessKey = "+";
            this.buttonDetailViewHeightIncrease.Activate += new System.EventHandler(this.OnDetailViewDetailHeightIncrease);
            //
            // buttonDetailViewHeightDecrease
            //
            this.buttonDetailViewHeightDecrease.Guid = new System.Guid("3c2e115b-fce0-4dda-873c-0960cffd9c3f");
            this.buttonDetailViewHeightDecrease.Image = global::ShipWorks.Properties.Resources.navigate_minus;
            this.buttonDetailViewHeightDecrease.QuickAccessKey = "-";
            this.buttonDetailViewHeightDecrease.Activate += new System.EventHandler(this.OnDetailViewDetailHeightDecrease);
            //
            // stripLayout1
            //
            stripLayout1.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            stripLayout1.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonManageFilters,
            this.buttonManageTemplates,
            this.buttonManageActions,
            this.buttonShippingSettings,
            this.buttonShippingProfiles,
            this.buttonSettings,
            this.buttonOrderLookupViewFields});
            //
            // buttonManageFilters
            //
            this.buttonManageFilters.Guid = new System.Guid("cb059887-49a5-44df-8401-57f0cc27bf88");
            this.buttonManageFilters.Image = global::ShipWorks.Properties.Resources.funnel;
            this.buttonManageFilters.QuickAccessKey = "F";
            this.buttonManageFilters.Text = "Filters";
            this.buttonManageFilters.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonManageFilters.Activate += new System.EventHandler(this.OnManageFilters);
            //
            // buttonManageTemplates
            //
            this.buttonManageTemplates.Guid = new System.Guid("387a6baf-0aef-4b81-95a8-3d2aba637530");
            this.buttonManageTemplates.Image = global::ShipWorks.Properties.Resources.template_general_32;
            this.buttonManageTemplates.QuickAccessKey = "T";
            this.buttonManageTemplates.Text = "Templates";
            this.buttonManageTemplates.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonManageTemplates.Activate += new System.EventHandler(this.OnManageTemplates);
            //
            // buttonManageActions
            //
            this.buttonManageActions.Guid = new System.Guid("e8136a37-72ce-49d4-8245-6893d126b23a");
            this.buttonManageActions.Image = global::ShipWorks.Properties.Resources.gear_run32;
            this.ribbonSecurityProvider.SetPermission(this.buttonManageActions, ShipWorks.Users.Security.PermissionType.ManageActions);
            this.buttonManageActions.QuickAccessKey = "A";
            this.buttonManageActions.Text = "Actions";
            this.buttonManageActions.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonManageActions.Activate += new System.EventHandler(this.OnManageActions);
            //
            // buttonShippingSettings
            //
            this.buttonShippingSettings.Guid = new System.Guid("0e161e23-f9ba-4f01-899d-4e4c908735f6");
            this.buttonShippingSettings.Image = global::ShipWorks.Properties.Resources.box_preferences32;
            this.ribbonSecurityProvider.SetPermission(this.buttonShippingSettings, ShipWorks.Users.Security.PermissionType.ShipmentsManageSettings);
            this.buttonShippingSettings.QuickAccessKey = "SS";
            this.buttonShippingSettings.Text = "Shipping\r\nSettings";
            this.buttonShippingSettings.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonShippingSettings.Activate += new System.EventHandler(this.OnManageShippingSettings);
            //
            // buttonShippingProfiles
            //
            this.buttonShippingProfiles.Guid = new System.Guid("13ba3580-9867-41a9-91b4-200e7bd1639b");
            this.buttonShippingProfiles.Image = global::ShipWorks.Properties.Resources.box_closed_with_label_32_32;
            this.ribbonSecurityProvider.SetPermission(this.buttonShippingProfiles, ShipWorks.Users.Security.PermissionType.ShipmentsManageSettings);
            this.buttonShippingProfiles.QuickAccessKey = "SP";
            this.buttonShippingProfiles.Text = "Shipping\r\nProfiles";
            this.buttonShippingProfiles.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonShippingProfiles.Activate += new System.EventHandler(this.OnManageShippingProfiles);
            //
            // buttonOptions
            //
            this.buttonSettings.Guid = new System.Guid("cf121122-bbfb-4eb0-aa60-6d41d1128b12");
            this.buttonSettings.Image = global::ShipWorks.Properties.Resources.preferences;
            this.buttonSettings.QuickAccessKey = "O";
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonSettings.Activate += new System.EventHandler(this.OnShowSettings);
            //
            // buttonOrderLookupViewFields
            //
            this.buttonOrderLookupViewFields.Guid = new System.Guid("A9CE2A60-E87B-4D22-B5E6-A3D02BF5ECCB");
            this.buttonOrderLookupViewFields.Image = global::ShipWorks.Properties.Resources.form_blue_edit;
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewFields, ShipWorks.Users.Security.PermissionType.ManageFields);
            this.buttonOrderLookupViewFields.QuickAccessKey = "I";
            this.buttonOrderLookupViewFields.Text = "Fields";
            this.buttonOrderLookupViewFields.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewFields.Activate += new System.EventHandler(this.OnManageOrderLookupFields);
            //
            // stripLayout3
            //
            stripLayout3.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            stripLayout3.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonManageStores,
            this.buttonManageUsers,
            this.buttonEmailAccounts});
            //
            // buttonManageStores
            //
            this.buttonManageStores.Guid = new System.Guid("c76be2e7-9f6d-41dc-9fd3-7027357cd54d");
            this.buttonManageStores.Image = global::ShipWorks.Properties.Resources.school32;
            this.ribbonSecurityProvider.SetPermission(this.buttonManageStores, ShipWorks.Users.Security.PermissionType.ManageStores);
            this.buttonManageStores.QuickAccessKey = "ST";
            this.buttonManageStores.Text = "Stores";
            this.buttonManageStores.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonManageStores.Activate += new System.EventHandler(this.OnManageStores);
            //
            // buttonManageUsers
            //
            this.buttonManageUsers.Guid = new System.Guid("9df0024a-179c-4326-9416-d2fe9b49b66b");
            this.buttonManageUsers.Image = global::ShipWorks.Properties.Resources.users3;
            this.ribbonSecurityProvider.SetPermission(this.buttonManageUsers, ShipWorks.Users.Security.PermissionType.ManageUsers);
            this.buttonManageUsers.QuickAccessKey = "U";
            this.buttonManageUsers.Text = "Users";
            this.buttonManageUsers.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonManageUsers.Activate += new System.EventHandler(this.OnManageUsers);
            //
            // buttonEmailAccounts
            //
            this.buttonEmailAccounts.Guid = new System.Guid("89c530bf-cddf-4ed2-9c31-a1055be8fd3c");
            this.buttonEmailAccounts.Image = global::ShipWorks.Properties.Resources.mail_server32;
            this.ribbonSecurityProvider.SetPermission(this.buttonEmailAccounts, ShipWorks.Users.Security.PermissionType.ManageEmailAccounts);
            this.buttonEmailAccounts.QuickAccessKey = "E";
            this.buttonEmailAccounts.Text = "Email\r\nAccounts";
            this.buttonEmailAccounts.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonEmailAccounts.Activate += new System.EventHandler(this.OnEmailAccounts);
            //
            // menuFedExEndOfDay
            //
            menuFedExEndOfDay.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuFedExEndDayClose,
            this.menuFedExEndDayPrint});
            //
            // menuFedExEndDayClose
            //
            this.menuFedExEndDayClose.Guid = new System.Guid("d943382b-8c2e-42b1-bb63-0c4de3f9f16c");
            this.menuFedExEndDayClose.Text = "Ground Close";
            this.menuFedExEndDayClose.Activate += new System.EventHandler(this.OnFedExGroundClose);
            //
            // menuFedExEndDayPrint
            //
            this.menuFedExEndDayPrint.Guid = new System.Guid("2c9ae5ac-7d4c-493e-9120-19647aec7f72");
            this.menuFedExEndDayPrint.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuFedExPrintReports});
            this.menuFedExEndDayPrint.Text = "Print Report";
            //
            // menuFedExPrintReports
            //
            this.menuFedExPrintReports.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuFedExEndDayPrintPlaceholder});
            //
            // menuFedExEndDayPrintPlaceholder
            //
            this.menuFedExEndDayPrintPlaceholder.Enabled = false;
            this.menuFedExEndDayPrintPlaceholder.Guid = new System.Guid("385a39fa-3e1f-4120-a489-23be3ebdcf64");
            this.menuFedExEndDayPrintPlaceholder.Text = "(None)";
            //
            // menuFedExSmartPostClose
            //
            this.menuFedExSmartPostClose.GroupName = "FedEx Ground® Economy";
            this.menuFedExSmartPostClose.Guid = new System.Guid("7292d100-9bf0-4c76-a25a-9122a8ac140c");
            this.menuFedExSmartPostClose.Text = "FedEx Ground® Economy Close";
            this.menuFedExSmartPostClose.Activate += new System.EventHandler(this.OnFedExSmartPostClose);
            //
            // shortcut1
            //
            shortcut1.Target = this.menuFedExEndDayPrint;
            //
            // menu1
            //
            menu1.DrawMargin = false;
            menu1.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuItemViewHelp,
            this.menuItemSupportForum,
            this.menuItemRequestHelp,
            this.menuItemRemoteAssistance,
            this.menuItemBuySupplies,
            this.menuItemHelpAbout});
            //
            // menuItemViewHelp
            //
            this.menuItemViewHelp.Guid = new System.Guid("6ad9da77-b616-4bf4-a6ac-358f0e0fc7ca");
            this.menuItemViewHelp.Image = global::ShipWorks.Properties.Resources.help2;
            this.menuItemViewHelp.Padding = new Divelements.SandRibbon.WidgetEdges(40, 3, 20, 3);
            this.menuItemViewHelp.Text = "View Help";
            this.menuItemViewHelp.Activate += new System.EventHandler(this.OnViewHelp);
            //
            // menuItemSupportForum
            //
            this.menuItemSupportForum.GroupName = "Support";
            this.menuItemSupportForum.Guid = new System.Guid("81a59a4d-531f-441f-afca-b02f072082ca");
            this.menuItemSupportForum.Image = global::ShipWorks.Properties.Resources.help_earth;
            this.menuItemSupportForum.Padding = new Divelements.SandRibbon.WidgetEdges(40, 3, 20, 3);
            this.menuItemSupportForum.Text = "Visit Forum";
            this.menuItemSupportForum.Activate += new System.EventHandler(this.OnSupportForum);
            //
            // menuItemRequestHelp
            //
            this.menuItemRequestHelp.GroupName = "Support";
            this.menuItemRequestHelp.Guid = new System.Guid("738d24aa-fd88-4a64-b6be-836d8f21a7a6");
            this.menuItemRequestHelp.Image = global::ShipWorks.Properties.Resources.user_headset;
            this.menuItemRequestHelp.Padding = new Divelements.SandRibbon.WidgetEdges(40, 3, 20, 3);
            this.menuItemRequestHelp.Text = "Submit Support Request";
            this.menuItemRequestHelp.Activate += new System.EventHandler(this.OnRequestHelp);
            //
            // menuItemRemoteAssistance
            //
            this.menuItemRemoteAssistance.GroupName = "Support";
            this.menuItemRemoteAssistance.Guid = new System.Guid("cd90e5ec-14c9-4a0b-b592-e1eee22176c1");
            this.menuItemRemoteAssistance.Image = global::ShipWorks.Properties.Resources.remote_assist32;
            this.menuItemRemoteAssistance.Padding = new Divelements.SandRibbon.WidgetEdges(40, 3, 20, 3);
            this.menuItemRemoteAssistance.Text = "Generate PIN";
            this.menuItemRemoteAssistance.Activate += new System.EventHandler(this.OnRemoteAssistance);
            //
            // menuItemBuySupplies
            //
            this.menuItemBuySupplies.GroupName = "Supplies";
            this.menuItemBuySupplies.Guid = new System.Guid("e94bb4a5-5459-4007-a743-8e9cd2e0c47e");
            this.menuItemBuySupplies.Image = global::ShipWorks.Properties.Resources.shoppingcart_full;
            this.menuItemBuySupplies.Text = "Buy Supplies";
            this.menuItemBuySupplies.Activate += new System.EventHandler(this.OnBuySupplies);
            //
            // menuItemHelpAbout
            //
            this.menuItemHelpAbout.Guid = new System.Guid("7d7755cb-fedf-4881-bfe8-77b28c53f449");
            this.menuItemHelpAbout.Image = global::ShipWorks.Properties.Resources.about;
            this.menuItemHelpAbout.Padding = new Divelements.SandRibbon.WidgetEdges(40, 3, 20, 3);
            this.menuItemHelpAbout.Text = "About ShipWorks";
            this.menuItemHelpAbout.Activate += new System.EventHandler(this.OnAboutShipWorks);
            //
            // menu2
            //
            menu2.DrawMargin = false;
            menu2.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.mainMenuItemSetupDatabase,
            this.mainMenuItemBackupDatabase});
            //
            // mainMenuItemSetupDatabase
            //
            this.mainMenuItemSetupDatabase.Guid = new System.Guid("1b4ae72d-4bde-4914-96ce-1dd4f1f162fe");
            this.mainMenuItemSetupDatabase.Image = ((System.Drawing.Image) (resources.GetObject("mainMenuItemSetupDatabase.Image")));
            this.ribbonSecurityProvider.SetPermission(this.mainMenuItemSetupDatabase, ShipWorks.Users.Security.PermissionType.DatabaseSetup);
            this.mainMenuItemSetupDatabase.QuickAccessKey = "D";
            this.mainMenuItemSetupDatabase.Text = "Configuration";
            this.mainMenuItemSetupDatabase.Activate += new System.EventHandler(this.OnDatabaseConfiguration);
            //
            // mainMenuItemBackupDatabase
            //
            this.mainMenuItemBackupDatabase.GroupName = "Backup";
            this.mainMenuItemBackupDatabase.Guid = new System.Guid("bdac750b-3734-456f-b682-5cbaf47f6bd6");
            this.mainMenuItemBackupDatabase.Image = global::ShipWorks.Properties.Resources.data_disk32;
            this.ribbonSecurityProvider.SetPermission(this.mainMenuItemBackupDatabase, ShipWorks.Users.Security.PermissionType.DatabaseBackup);
            this.mainMenuItemBackupDatabase.QuickAccessKey = "B";
            this.mainMenuItemBackupDatabase.Text = "&Backup";
            this.mainMenuItemBackupDatabase.Activate += new System.EventHandler(this.OnBackupShipWorks);
            //
            // uiModeMenu
            //
            this.uiModeMenu.DrawMargin = false;
            this.uiModeMenu.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.mainMenuItemOrderLookup,
            this.mainMenuItemBatchGrid,
            this.mainMenuItemProducts});
            //
            // mainMenuItemOrderLookup
            //
            this.mainMenuItemOrderLookup.Guid = new System.Guid("072bf177-36c3-4809-9297-1c1e62a92e4b");
            this.mainMenuItemOrderLookup.Image = global::ShipWorks.Properties.Resources.box_view32;
            this.mainMenuItemOrderLookup.QuickAccessKey = "O";
            this.mainMenuItemOrderLookup.Text = "Scan-to-Ship";
            this.mainMenuItemOrderLookup.Activate += new System.EventHandler(this.OnShowOrderLookupView);
            //
            // mainMenuItemBatchGrid
            //
            this.mainMenuItemBatchGrid.GroupName = "batch";
            this.mainMenuItemBatchGrid.Guid = new System.Guid("138732e5-e3fe-4ae6-ae4b-edd7f28590ee");
            this.mainMenuItemBatchGrid.Image = global::ShipWorks.Properties.Resources.table_sql32;
            this.mainMenuItemBatchGrid.QuickAccessKey = "B";
            this.mainMenuItemBatchGrid.Text = "Classic";
            this.mainMenuItemBatchGrid.Activate += new System.EventHandler(this.OnShowBatchView);
            //
            // mainMenuItemProducts
            //
            this.mainMenuItemProducts.GroupName = "products";
            this.mainMenuItemProducts.Guid = new System.Guid("F70670F1-A330-4C3E-BDE7-8CEDDB7B6A0D");
            this.mainMenuItemProducts.Image = global::ShipWorks.Properties.Resources.box_tall_32;
            this.mainMenuItemProducts.QuickAccessKey = "P";
            this.mainMenuItemProducts.Text = "Products";
            this.mainMenuItemProducts.Activate += new System.EventHandler(this.OnShowProductsView);
            //
            // menuOrderLookupViewFedExEndOfDay
            //
            menuOrderLookupViewFedExEndOfDay.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
                this.menuOrderLookupViewFedExEndDayClose,
                this.menuOrderLookupViewFedExEndDayPrint,
                this.menuOrderLookupViewFedExSmartPostClose});
            //
            // menuOrderLookupViewFedExEndDayClose
            //
            this.menuOrderLookupViewFedExEndDayClose.Guid = new System.Guid("1B3D49A4-1779-48A5-8F47-786E63BBEDA5");
            this.menuOrderLookupViewFedExEndDayClose.Text = "Ground Close";
            this.menuOrderLookupViewFedExEndDayClose.Activate += new System.EventHandler(this.OnFedExGroundClose);
            //
            // menuOrderLookupViewFedExEndDayPrint
            //
            this.menuOrderLookupViewFedExEndDayPrint.Guid = new System.Guid("BAD86337-173F-4568-B061-CF1CB9159E52");
            this.menuOrderLookupViewFedExEndDayPrint.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
                this.menuOrderLookupViewFedExPrintReports});
            this.menuOrderLookupViewFedExEndDayPrint.Text = "Print Report";
            //
            // menuOrderLookupViewFedExPrintReports
            //
            this.menuOrderLookupViewFedExPrintReports.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
                this.menuOrderLookupViewFedExEndDayPrintPlaceholder});
            //
            // menuOrderLookupViewFedExEndDayPrintPlaceholder
            //
            this.menuOrderLookupViewFedExEndDayPrintPlaceholder.Enabled = false;
            this.menuOrderLookupViewFedExEndDayPrintPlaceholder.Guid = new System.Guid("6942C18F-E7FA-4F53-BD99-6A801B96EB68");
            this.menuOrderLookupViewFedExEndDayPrintPlaceholder.Text = "(None)";
            //
            // menuOrderLookupViewFedExSmartPostClose
            //
            this.menuOrderLookupViewFedExSmartPostClose.GroupName = "SmartPost";
            this.menuOrderLookupViewFedExSmartPostClose.Guid = new System.Guid("B04EEF5B-851F-4B43-B2F3-DB47CDDEE610");
            this.menuOrderLookupViewFedExSmartPostClose.Text = "SmartPost Close";
            this.menuOrderLookupViewFedExSmartPostClose.Activate += new System.EventHandler(this.OnFedExSmartPostClose);
            //
            // buttonRestore
            //
            this.buttonRestore.Guid = new System.Guid("a77a0044-436b-4602-bf52-62db00244224");
            this.buttonRestore.Image = global::ShipWorks.Properties.Resources.data_into;
            this.ribbonSecurityProvider.SetPermission(this.buttonRestore, ShipWorks.Users.Security.PermissionType.DatabaseRestore);
            this.buttonRestore.QuickAccessKey = "R";
            this.buttonRestore.Text = "Restore";
            this.buttonRestore.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonRestore.Activate += new System.EventHandler(this.OnRestoreBackup);
            //
            // buttonArchive
            //
            this.buttonArchive.Guid = new System.Guid("457547e9-dc3a-4cd9-a196-864c43cd1249");
            this.buttonArchive.Image = global::ShipWorks.Properties.Resources.data_time;
            this.ribbonSecurityProvider.SetPermission(this.buttonArchive, ShipWorks.Users.Security.PermissionType.DatabaseArchive);
            this.buttonArchive.QuickAccessKey = "K";
            this.buttonArchive.Text = "Archive";
            this.buttonArchive.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonArchive.Activate += new System.EventHandler(this.OnArchive);
            //
            // buttonSetupDatabase
            //
            this.buttonSetupDatabase.Guid = new System.Guid("dd98aefb-3dc0-4ecc-bfea-a2e7f1f5f2ad");
            this.buttonSetupDatabase.Image = global::ShipWorks.Properties.Resources.data_gear1;
            this.ribbonSecurityProvider.SetPermission(this.buttonSetupDatabase, ShipWorks.Users.Security.PermissionType.DatabaseSetup);
            this.buttonSetupDatabase.QuickAccessKey = "D";
            this.buttonSetupDatabase.Text = "Configuration";
            this.buttonSetupDatabase.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonSetupDatabase.Activate += new System.EventHandler(this.OnDatabaseConfiguration);
            //
            // stripLayoutModifyOrders
            //
            this.stripLayoutModifyOrders.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonNewOrder,
            this.buttonDeleteOrders});
            this.stripLayoutModifyOrders.ItemSpacing = 6;
            this.stripLayoutModifyOrders.LayoutDirection = Divelements.SandRibbon.LayoutDirection.Vertical;
            this.stripLayoutModifyOrders.Margin = new Divelements.SandRibbon.WidgetEdges(0, 3, 0, 0);
            //
            // buttonNewOrder
            //
            this.buttonNewOrder.Guid = new System.Guid("e4349726-f217-41a6-b703-9c5ba735dbb5");
            this.buttonNewOrder.Image = global::ShipWorks.Properties.Resources.order_add;
            this.ribbonSecurityProvider.SetPermission(this.buttonNewOrder, ShipWorks.Users.Security.PermissionType.CustomersAddOrder);
            this.buttonNewOrder.QuickAccessKey = "ON";
            this.buttonNewOrder.Text = "New";
            this.buttonNewOrder.Activate += new System.EventHandler(this.OnNewOrder);
            //
            // buttonDeleteOrders
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonDeleteOrders, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.buttonDeleteOrders.Guid = new System.Guid("acf571fa-b3ae-4e65-a734-a35a44d36aaf");
            this.buttonDeleteOrders.Image = global::ShipWorks.Properties.Resources.order_delete;
            this.ribbonSecurityProvider.SetPermission(this.buttonDeleteOrders, ShipWorks.Users.Security.PermissionType.OrdersModify);
            this.buttonDeleteOrders.QuickAccessKey = "OD";
            this.buttonDeleteOrders.Text = "Delete";
            this.buttonDeleteOrders.Activate += new System.EventHandler(this.OnDelete);
            //
            // stripLayoutCombineSplitOrders
            //
            this.stripLayoutCombineSplitOrders.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonCombine,
            this.buttonSplit});
            this.stripLayoutCombineSplitOrders.ItemSpacing = 6;
            this.stripLayoutCombineSplitOrders.LayoutDirection = Divelements.SandRibbon.LayoutDirection.Vertical;
            this.stripLayoutCombineSplitOrders.Margin = new Divelements.SandRibbon.WidgetEdges(0, 3, 0, 0);
            //
            // buttonCombine
            //
            this.buttonCombine.Guid = new System.Guid("a8bf47aa-a2e2-4b59-b871-e5ff993623e6");
            this.buttonCombine.Image = global::ShipWorks.Properties.Resources.merge_icon_16x16;
            this.ribbonSecurityProvider.SetPermission(this.buttonCombine, ShipWorks.Users.Security.PermissionType.OrdersModify);
            this.buttonCombine.QuickAccessKey = "OC";
            this.buttonCombine.Text = "Combine";
            this.buttonCombine.Activate += new System.EventHandler(this.OnCombineOrders);
            //
            // buttonSplit
            //
            this.buttonSplit.Guid = new System.Guid("911aac51-83d9-4aec-b1ed-5a3b1f9f51a7");
            this.buttonSplit.Image = global::ShipWorks.Properties.Resources.split_icon_16x16;
            this.ribbonSecurityProvider.SetPermission(this.buttonSplit, ShipWorks.Users.Security.PermissionType.OrdersModify);
            this.buttonSplit.QuickAccessKey = "OS";
            this.buttonSplit.Text = "Split";
            this.buttonSplit.Activate += new System.EventHandler(this.OnSplitOrder);
            //
            // buttonEditCustomer
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonEditCustomer, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneCustomer);
            this.buttonEditCustomer.Guid = new System.Guid("287f6db0-b93d-4db6-bbb1-9d0905704b59");
            this.buttonEditCustomer.Image = global::ShipWorks.Properties.Resources.customer_edit32;
            this.buttonEditCustomer.Margin = new Divelements.SandRibbon.WidgetEdges(3, 0, 3, 0);
            this.buttonEditCustomer.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonEditCustomer.QuickAccessKey = "CE";
            this.buttonEditCustomer.Text = "Edit";
            this.buttonEditCustomer.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonEditCustomer.Activate += new System.EventHandler(this.OnEditCustomer);
            //
            // buttonEditOrder
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonEditOrder, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrder);
            this.buttonEditOrder.Guid = new System.Guid("AFFAB209-DE03-4A93-AFFE-00399F409269");
            this.buttonEditOrder.Image = global::ShipWorks.Properties.Resources.form_blue_edit;
            this.buttonEditOrder.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonEditOrder.QuickAccessKey = "OE";
            this.buttonEditOrder.Text = "Edit";
            this.buttonEditOrder.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonEditOrder.Activate += new System.EventHandler(this.OnEditOrder);
            //
            // panelArchiveNotification
            //
            this.panelArchiveNotification.BackColor = System.Drawing.Color.Transparent;
            this.panelArchiveNotification.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelArchiveNotification.Location = new System.Drawing.Point(3, 177);
            this.panelArchiveNotification.Name = "panelArchiveNotification";
            this.panelArchiveNotification.Size = new System.Drawing.Size(969, 52);
            this.panelArchiveNotification.TabIndex = 6;
            this.panelArchiveNotification.Visible = false;
            //
            // dashboardBarSample
            //
            this.dashboardBarSample.BackColor = System.Drawing.Color.Transparent;
            this.dashboardBarSample.CanUserDismiss = true;
            this.dashboardBarSample.Dock = System.Windows.Forms.DockStyle.Top;
            this.dashboardBarSample.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dashboardBarSample.Image = ((System.Drawing.Image) (resources.GetObject("dashboardBarSample.Image")));
            this.dashboardBarSample.Location = new System.Drawing.Point(0, 0);
            this.dashboardBarSample.Name = "dashboardBarSample";
            this.dashboardBarSample.PrimaryText = "Prmary Text";
            this.dashboardBarSample.SecondaryText = "Secondary Text";
            this.dashboardBarSample.Size = new System.Drawing.Size(969, 26);
            this.dashboardBarSample.TabIndex = 0;
            //
            // panelDockingArea
            //
            this.panelDockingArea.Controls.Add(this.gridControl);
            this.panelDockingArea.Controls.Add(this.dockContainer1);
            this.panelDockingArea.Controls.Add(this.dockContainer);
            this.panelDockingArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDockingArea.Location = new System.Drawing.Point(3, 229);
            this.panelDockingArea.Name = "panelDockingArea";
            this.panelDockingArea.Padding = new System.Windows.Forms.Padding(2, 0, 2, 4);
            this.panelDockingArea.Size = new System.Drawing.Size(969, 498);
            this.panelDockingArea.TabIndex = 5;
            //
            // gridControl
            //
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridControl.Location = new System.Drawing.Point(220, 0);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(747, 275);
            this.gridControl.TabIndex = 0;
            this.gridControl.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridControl.SortChanged += new System.EventHandler(this.OnGridSortChanged);
            this.gridControl.DeleteKeyPressed += new System.EventHandler(this.OnDelete);
            this.gridControl.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnGridRowActivated);
            this.gridControl.SearchActiveChanged += new System.EventHandler(this.OnGridSearchActiveChanged);
            this.gridControl.SearchQueryChanged += new System.EventHandler(this.OnGridSearchQueryChanged);
            this.gridControl.FilterSaved += OnGridControlFilterSaved;
            //
            // dockContainer1
            //
            this.dockContainer1.ContentSize = 215;
            this.dockContainer1.Controls.Add(this.dockableWindowOrders);
            this.dockContainer1.Controls.Add(this.dockableWindowItems);
            this.dockContainer1.Controls.Add(this.dockableWindowCharges);
            this.dockContainer1.Controls.Add(this.dockableWindowMap);
            this.dockContainer1.Controls.Add(this.dockableWindowStreetView);
            this.dockContainer1.Controls.Add(this.dockableWindowPaymentDetails);
            this.dockContainer1.Controls.Add(this.dockableWindowShipments);
            this.dockContainer1.Controls.Add(this.dockableWindowEmail);
            this.dockContainer1.Controls.Add(this.dockableWindowPrinted);
            this.dockContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dockContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Vertical, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(349.4023F, 400F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dockableWindowOrders)),
                        ((TD.SandDock.DockControl)(this.dockableWindowItems)),
                        ((TD.SandDock.DockControl)(this.dockableWindowCharges)),
                        ((TD.SandDock.DockControl)(this.dockableWindowMap)),
                        ((TD.SandDock.DockControl)(this.dockableWindowStreetView)),
                        ((TD.SandDock.DockControl)(this.dockableWindowPaymentDetails))}, this.dockableWindowStreetView))),
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(337.3859F, 400F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dockableWindowShipments)),
                        ((TD.SandDock.DockControl)(this.dockableWindowEmail)),
                        ((TD.SandDock.DockControl)(this.dockableWindowPrinted))}, this.dockableWindowShipments)))});
            this.dockContainer1.Location = new System.Drawing.Point(220, 275);
            this.dockContainer1.Manager = this.sandDockManager;
            this.dockContainer1.Name = "dockContainer1";
            this.dockContainer1.Size = new System.Drawing.Size(747, 219);
            this.dockContainer1.TabIndex = 7;
            //
            // dockableWindowOrders
            //
            this.dockableWindowOrders.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowOrders.Controls.Add(this.panelOrders);
            this.dockableWindowOrders.Guid = new System.Guid("60992767-2cdf-45cc-9868-e404a8cc6dae");
            this.dockableWindowOrders.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowOrders.Name = "dockableWindowOrders";
            this.dockableWindowOrders.Size = new System.Drawing.Size(378, 170);
            this.dockableWindowOrders.TabImage = global::ShipWorks.Properties.Resources.order16;
            this.dockableWindowOrders.TabIndex = 0;
            this.dockableWindowOrders.Text = "Orders";
            this.dockableWindowOrders.Visible = false;
            //
            // panelOrders
            //
            this.panelOrders.BackColor = System.Drawing.Color.White;
            this.panelOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOrders.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelOrders.Location = new System.Drawing.Point(1, 1);
            this.panelOrders.Name = "panelOrders";
            this.panelOrders.Size = new System.Drawing.Size(376, 168);
            this.panelOrders.TabIndex = 0;
            this.panelOrders.OrderDeleted += new System.EventHandler(this.OnPanelDataChanged);
            this.panelOrders.AddOrderClicked += new System.EventHandler(this.OnNewOrder);
            //
            // dockableWindowItems
            //
            this.dockableWindowItems.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowItems.Controls.Add(this.panelItems);
            this.dockableWindowItems.Guid = new System.Guid("39104285-819c-467e-8318-7b6350ef852a");
            this.dockableWindowItems.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowItems.Name = "dockableWindowItems";
            this.dockableWindowItems.ShowOptions = false;
            this.dockableWindowItems.Size = new System.Drawing.Size(378, 170);
            this.dockableWindowItems.TabImage = global::ShipWorks.Properties.Resources.shoppingcart16;
            this.dockableWindowItems.TabIndex = 0;
            this.dockableWindowItems.Text = "Items";
            //
            // panelItems
            //
            this.panelItems.BackColor = System.Drawing.Color.White;
            this.panelItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelItems.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelItems.Location = new System.Drawing.Point(1, 1);
            this.panelItems.Name = "panelItems";
            this.panelItems.Size = new System.Drawing.Size(376, 168);
            this.panelItems.TabIndex = 0;
            this.panelItems.ItemsChanged += new System.EventHandler(this.OnPanelDataChanged);
            //
            // dockableWindowCharges
            //
            this.dockableWindowCharges.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowCharges.Controls.Add(this.panelCharges);
            this.dockableWindowCharges.Guid = new System.Guid("7d4b4e89-211e-4d19-b98a-7bcc4b30a8e7");
            this.dockableWindowCharges.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowCharges.Name = "dockableWindowCharges";
            this.dockableWindowCharges.ShowOptions = false;
            this.dockableWindowCharges.Size = new System.Drawing.Size(378, 170);
            this.dockableWindowCharges.TabImage = global::ShipWorks.Properties.Resources.currency_dollar16;
            this.dockableWindowCharges.TabIndex = 0;
            this.dockableWindowCharges.Text = "Charges";
            this.dockableWindowCharges.Visible = false;
            //
            // panelCharges
            //
            this.panelCharges.BackColor = System.Drawing.Color.White;
            this.panelCharges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCharges.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelCharges.Location = new System.Drawing.Point(1, 1);
            this.panelCharges.Name = "panelCharges";
            this.panelCharges.Size = new System.Drawing.Size(376, 168);
            this.panelCharges.TabIndex = 0;
            this.panelCharges.ChargesChanged += new System.EventHandler(this.OnPanelDataChanged);
            //
            // dockableWindowMap
            //
            this.dockableWindowMap.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowMap.Controls.Add(this.panelMap);
            this.dockableWindowMap.Guid = new System.Guid("988996c1-c313-42fc-918c-e17dfdf172a4");
            this.dockableWindowMap.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowMap.Name = "dockableWindowMap";
            this.dockableWindowMap.ShowOptions = false;
            this.dockableWindowMap.Size = new System.Drawing.Size(378, 170);
            this.dockableWindowMap.TabImage = global::ShipWorks.Properties.Resources.googleMapsFav16;
            this.dockableWindowMap.TabIndex = 0;
            this.dockableWindowMap.Text = "Map";
            //
            // panelMap
            //
            this.panelMap.BackColor = System.Drawing.Color.White;
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelMap.Location = new System.Drawing.Point(1, 1);
            this.panelMap.MapType = ShipWorks.Stores.Content.Panels.MapPanelType.Satellite;
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(376, 168);
            this.panelMap.TabIndex = 1;
            //
            // dockableWindowStreetView
            //
            this.dockableWindowStreetView.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowStreetView.Controls.Add(this.panelStreetView);
            this.dockableWindowStreetView.Guid = new System.Guid("f4aeeb98-50bf-4fe1-bdd6-0ed890af0c81");
            this.dockableWindowStreetView.Location = new System.Drawing.Point(0, 25);
            this.dockableWindowStreetView.Name = "dockableWindowStreetView";
            this.dockableWindowStreetView.ShowOptions = false;
            this.dockableWindowStreetView.Size = new System.Drawing.Size(378, 170);
            this.dockableWindowStreetView.TabImage = global::ShipWorks.Properties.Resources.viewofstreet;
            this.dockableWindowStreetView.TabIndex = 0;
            this.dockableWindowStreetView.Text = "Street Level View";
            //
            // panelStreetView
            //
            this.panelStreetView.BackColor = System.Drawing.Color.White;
            this.panelStreetView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStreetView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelStreetView.Location = new System.Drawing.Point(1, 1);
            this.panelStreetView.MapType = ShipWorks.Stores.Content.Panels.MapPanelType.StreetView;
            this.panelStreetView.Name = "panelStreetView";
            this.panelStreetView.Size = new System.Drawing.Size(376, 168);
            this.panelStreetView.TabIndex = 1;
            //
            // dockableWindowPaymentDetails
            //
            this.dockableWindowPaymentDetails.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowPaymentDetails.Controls.Add(this.panelPaymentDetail);
            this.dockableWindowPaymentDetails.Guid = new System.Guid("d543ebe5-dc50-407e-bc66-c30b8a850b54");
            this.dockableWindowPaymentDetails.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowPaymentDetails.Name = "dockableWindowPaymentDetails";
            this.dockableWindowPaymentDetails.ShowOptions = false;
            this.dockableWindowPaymentDetails.Size = new System.Drawing.Size(378, 170);
            this.dockableWindowPaymentDetails.TabImage = global::ShipWorks.Properties.Resources.creditcards;
            this.dockableWindowPaymentDetails.TabIndex = 1;
            this.dockableWindowPaymentDetails.Text = "Payment Details";
            this.dockableWindowPaymentDetails.Visible = false;
            //
            // panelPaymentDetail
            //
            this.panelPaymentDetail.BackColor = System.Drawing.Color.White;
            this.panelPaymentDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPaymentDetail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelPaymentDetail.Location = new System.Drawing.Point(1, 1);
            this.panelPaymentDetail.Name = "panelPaymentDetail";
            this.panelPaymentDetail.Size = new System.Drawing.Size(376, 168);
            this.panelPaymentDetail.TabIndex = 0;
            //
            // dockableWindowShipments
            //
            this.dockableWindowShipments.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowShipments.Controls.Add(this.panelShipments);
            this.dockableWindowShipments.Guid = new System.Guid("b65cc6d7-1b93-43ad-b0e6-23bc4b1ec699");
            this.dockableWindowShipments.Location = new System.Drawing.Point(382, 25);
            this.dockableWindowShipments.Name = "dockableWindowShipments";
            this.dockableWindowShipments.ShowOptions = false;
            this.dockableWindowShipments.Size = new System.Drawing.Size(365, 170);
            this.dockableWindowShipments.TabImage = global::ShipWorks.Properties.Resources.box_closed16;
            this.dockableWindowShipments.TabIndex = 0;
            this.dockableWindowShipments.Text = "Shipments";
            //
            // panelShipments
            //
            this.panelShipments.AllowRatesPanelLink = true;
            this.panelShipments.AutoScroll = true;
            this.panelShipments.BackColor = System.Drawing.Color.White;
            this.panelShipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShipments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelShipments.Location = new System.Drawing.Point(1, 1);
            this.panelShipments.Name = "panelShipments";
            this.panelShipments.Size = new System.Drawing.Size(363, 168);
            this.panelShipments.TabIndex = 1;
            //
            // dockableWindowEmail
            //
            this.dockableWindowEmail.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowEmail.Controls.Add(this.panelEmail);
            this.dockableWindowEmail.Guid = new System.Guid("b33f5fc1-c720-4046-81d6-728355f64c81");
            this.dockableWindowEmail.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowEmail.Name = "dockableWindowEmail";
            this.dockableWindowEmail.ShowOptions = false;
            this.dockableWindowEmail.Size = new System.Drawing.Size(365, 170);
            this.dockableWindowEmail.TabImage = global::ShipWorks.Properties.Resources.mail2;
            this.dockableWindowEmail.TabIndex = 0;
            this.dockableWindowEmail.Text = "Emails";
            this.dockableWindowEmail.Visible = false;
            //
            // panelEmail
            //
            this.panelEmail.BackColor = System.Drawing.Color.White;
            this.panelEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelEmail.Location = new System.Drawing.Point(1, 1);
            this.panelEmail.Name = "panelEmail";
            this.panelEmail.Size = new System.Drawing.Size(363, 168);
            this.panelEmail.TabIndex = 0;
            //
            // dockableWindowPrinted
            //
            this.dockableWindowPrinted.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowPrinted.Controls.Add(this.panelPrinted);
            this.dockableWindowPrinted.Guid = new System.Guid("5d33c695-a829-47fa-9eaa-3c2bd9ac5ab5");
            this.dockableWindowPrinted.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowPrinted.Name = "dockableWindowPrinted";
            this.dockableWindowPrinted.ShowOptions = false;
            this.dockableWindowPrinted.Size = new System.Drawing.Size(365, 170);
            this.dockableWindowPrinted.TabImage = global::ShipWorks.Properties.Resources.printer1;
            this.dockableWindowPrinted.TabIndex = 0;
            this.dockableWindowPrinted.Text = "Printed";
            this.dockableWindowPrinted.Visible = false;
            //
            // panelPrinted
            //
            this.panelPrinted.BackColor = System.Drawing.Color.White;
            this.panelPrinted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrinted.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelPrinted.Location = new System.Drawing.Point(1, 1);
            this.panelPrinted.Name = "panelPrinted";
            this.panelPrinted.Size = new System.Drawing.Size(363, 168);
            this.panelPrinted.TabIndex = 0;
            //
            // sandDockManager
            //
            this.sandDockManager.DockSystemContainer = this.panelDockingArea;
            this.sandDockManager.MaximumDockContainerSize = 1500;
            this.sandDockManager.OwnerForm = this;
            this.sandDockManager.Renderer = new TD.SandDock.Rendering.Office2007Renderer();
            this.sandDockManager.DockControlActivated += new TD.SandDock.DockControlEventHandler(this.OnDockControlActivated);
            this.sandDockManager.DockControlClosing += new TD.SandDock.DockControlClosingEventHandler(this.OnDockControlClosing);
            //
            // dockContainer
            //
            this.dockContainer.ContentSize = 214;
            this.dockContainer.Controls.Add(this.dockableWindowOrderFilters);
            this.dockContainer.Controls.Add(this.dockableWindowCustomerFilters);
            this.dockContainer.Controls.Add(this.dockableWindowNotes);
            this.dockContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockContainer.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(250F, 406.0914F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dockableWindowOrderFilters)),
                        ((TD.SandDock.DockControl)(this.dockableWindowCustomerFilters))}, this.dockableWindowOrderFilters))),
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(156.6059F, 393.9086F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.dockableWindowNotes))}, this.dockableWindowNotes)))});
            this.dockContainer.Location = new System.Drawing.Point(2, 0);
            this.dockContainer.Manager = this.sandDockManager;
            this.dockContainer.Name = "dockContainer";
            this.dockContainer.Size = new System.Drawing.Size(218, 494);
            this.dockContainer.TabIndex = 6;
            //
            // dockableWindowOrderFilters
            //
            this.dockableWindowOrderFilters.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowOrderFilters.Controls.Add(this.orderFilterTree);
            this.dockableWindowOrderFilters.Guid = new System.Guid("14a0dd81-b0e7-475b-aa54-aef3c97eb515");
            this.dockableWindowOrderFilters.Location = new System.Drawing.Point(0, 21);
            this.dockableWindowOrderFilters.Name = "dockableWindowOrderFilters";
            this.dockableWindowOrderFilters.ShowOptions = false;
            this.dockableWindowOrderFilters.Size = new System.Drawing.Size(214, 204);
            this.dockableWindowOrderFilters.TabImage = global::ShipWorks.Properties.Resources.filter;
            this.dockableWindowOrderFilters.TabIndex = 0;
            this.dockableWindowOrderFilters.Text = "Orders";
            //
            // orderFilterTree
            //
            this.orderFilterTree.AlwaysShowMyFilters = false;
            this.orderFilterTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.orderFilterTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderFilterTree.FilterScope = ShipWorks.Filters.Controls.FilterScope.Any;
            this.orderFilterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.orderFilterTree.HideDisabledFilters = true;
            this.orderFilterTree.HotTrackNode = null;
            this.orderFilterTree.Location = new System.Drawing.Point(1, 1);
            this.orderFilterTree.Name = "orderFilterTree";
            this.orderFilterTree.Size = new System.Drawing.Size(212, 202);
            this.orderFilterTree.TabIndex = 0;
            this.orderFilterTree.SelectedFilterNodeChanged += new System.EventHandler(this.OnSelectedFilterNodeChanged);
            this.orderFilterTree.LoadAsAdvancedSearch += OnFilterTreeLoadAsAdvancedSearch;
            //
            // dockableWindowCustomerFilters
            //
            this.dockableWindowCustomerFilters.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowCustomerFilters.Controls.Add(this.customerFilterTree);
            this.dockableWindowCustomerFilters.Guid = new System.Guid("5f3097be-c6e4-4f85-b9ff-24844749ae44");
            this.dockableWindowCustomerFilters.Location = new System.Drawing.Point(0, 0);
            this.dockableWindowCustomerFilters.Name = "dockableWindowCustomerFilters";
            this.dockableWindowCustomerFilters.ShowOptions = false;
            this.dockableWindowCustomerFilters.Size = new System.Drawing.Size(214, 231);
            this.dockableWindowCustomerFilters.TabImage = global::ShipWorks.Properties.Resources.customer16;
            this.dockableWindowCustomerFilters.TabIndex = 0;
            this.dockableWindowCustomerFilters.Text = "Customers";
            //
            // customerFilterTree
            //
            this.customerFilterTree.AlwaysShowMyFilters = false;
            this.customerFilterTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customerFilterTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customerFilterTree.FilterScope = ShipWorks.Filters.Controls.FilterScope.Any;
            this.customerFilterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.customerFilterTree.HideDisabledFilters = true;
            this.customerFilterTree.HotTrackNode = null;
            this.customerFilterTree.Location = new System.Drawing.Point(1, 1);
            this.customerFilterTree.Name = "customerFilterTree";
            this.customerFilterTree.Size = new System.Drawing.Size(212, 229);
            this.customerFilterTree.TabIndex = 0;
            this.customerFilterTree.SelectedFilterNodeChanged += new System.EventHandler(this.OnSelectedFilterNodeChanged);
            this.customerFilterTree.LoadAsAdvancedSearch += OnFilterTreeLoadAsAdvancedSearch;
            //
            // dockableWindowNotes
            //
            this.dockableWindowNotes.BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat;
            this.dockableWindowNotes.Controls.Add(this.panelNotes);
            this.dockableWindowNotes.Guid = new System.Guid("a965dab5-1007-4f3a-b48c-4f3dd12df869");
            this.dockableWindowNotes.Location = new System.Drawing.Point(0, 274);
            this.dockableWindowNotes.Name = "dockableWindowNotes";
            this.dockableWindowNotes.ShowOptions = false;
            this.dockableWindowNotes.Size = new System.Drawing.Size(214, 196);
            this.dockableWindowNotes.TabImage = global::ShipWorks.Properties.Resources.note16;
            this.dockableWindowNotes.TabIndex = 0;
            this.dockableWindowNotes.Text = "Notes";
            //
            // panelNotes
            //
            this.panelNotes.BackColor = System.Drawing.Color.White;
            this.panelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNotes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.panelNotes.Location = new System.Drawing.Point(1, 1);
            this.panelNotes.Name = "panelNotes";
            this.panelNotes.Size = new System.Drawing.Size(212, 194);
            this.panelNotes.TabIndex = 0;
            this.panelNotes.NotesChanged += new System.EventHandler(this.OnPanelDataChanged);
            //
            // ribbonManager
            //
            this.ribbonManager.Customization = Divelements.SandRibbon.RibbonCustomizationType.Full;
            this.ribbonManager.OwnerForm = this;
            ribbonRenderer1.ColorScheme = Divelements.SandRibbon.Rendering.Office2007ColorScheme.LunaBlue;
            this.ribbonManager.Renderer = ribbonRenderer1;
            //
            // statusBar
            //
            this.statusBar.ExtendedStrip = null;
            this.statusBar.Location = new System.Drawing.Point(3, 727);
            this.statusBar.MainStrip = statusBarStrip;
            this.statusBar.Manager = this.ribbonManager;
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(969, 22);
            this.statusBar.TabIndex = 1;
            //
            // notifyIcon
            //
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon) (resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "ShipWorks";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.OnDoubleClickTrayIcon);
            //
            // notifyIconMenuStrip
            //
            this.notifyIconMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenShipWorks});
            this.notifyIconMenuStrip.Name = "notifyIconMenuStrip";
            this.notifyIconMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.notifyIconMenuStrip.Size = new System.Drawing.Size(163, 26);
            //
            // toolStripMenuItemOpenShipWorks
            //
            this.toolStripMenuItemOpenShipWorks.Name = "toolStripMenuItemOpenShipWorks";
            this.toolStripMenuItemOpenShipWorks.Size = new System.Drawing.Size(162, 22);
            this.toolStripMenuItemOpenShipWorks.Text = "Open ShipWorks";
            this.toolStripMenuItemOpenShipWorks.Click += new System.EventHandler(this.OnTaskTrayMenuOpenShipWorks);
            //
            // kryptonManager
            //
            this.kryptonManager.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Blue;
            //
            // label14
            //
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.Color.Blue;
            this.label14.Location = new System.Drawing.Point(121, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "( 0 )";
            //
            // label15
            //
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(30, 49);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(92, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Pinned Customers";
            //
            // label16
            //
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.ForeColor = System.Drawing.Color.Blue;
            this.label16.Location = new System.Drawing.Point(102, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "(145 )";
            //
            // label17
            //
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(30, 29);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Pinned Orders";
            //
            // label18
            //
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.ForeColor = System.Drawing.Color.Blue;
            this.label18.Location = new System.Drawing.Point(96, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(28, 13);
            this.label18.TabIndex = 2;
            this.label18.Text = "(12 )";
            //
            // label19
            //
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Location = new System.Drawing.Point(28, 7);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 13);
            this.label19.TabIndex = 1;
            this.label19.Text = "My Shipments";
            //
            // contextMenuOrderGrid
            //
            this.contextMenuOrderGrid.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuOrderGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextOrderEditOrder,
            this.contextOrderCombineOrder,
            this.contextOrderSplitOrder,
            this.contextOrderSep1,
            this.contextOrderLocalStatus,
            this.contextOrderOnlineUpdate,
            this.contextOrderCustomActions,
            this.contextOrderSep6,
            this.contextOrderEditCustomer,
            this.contextOrderLookupCustomer,
            this.contextOrderSep2,
            this.contextOrderShipOrders,
            this.contextOrderTrackShipments,
            this.contextOrderInsuranceClaim,
            this.contextOrderSep3,
            this.contextOrderCopy,
            this.contextOrderSep7,
            this.contextOrderQuickPrint,
            this.contextOrderPrint,
            this.contextOrderPreview,
            this.contextOrderSep4,
            this.contextOrderEmailNow,
            this.contextOrderComposeEmail,
            this.contextOrderSep5,
            this.contextOrderSave,
            this.contextOrderSaveOpen});
            this.contextMenuOrderGrid.Name = "contextMenuOrderGrid";
            this.contextMenuOrderGrid.Size = new System.Drawing.Size(170, 464);
            //
            // contextOrderEditOrder
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderEditOrder, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrder);
            this.contextOrderEditOrder.Image = global::ShipWorks.Properties.Resources.order_edit;
            this.contextOrderEditOrder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderEditOrder, new System.Guid("e17b12c8-b5b7-4b48-a8c1-c51f108f57e0"));
            this.contextOrderEditOrder.Name = "contextOrderEditOrder";
            this.contextOrderEditOrder.Size = new System.Drawing.Size(169, 22);
            this.contextOrderEditOrder.Text = "Edit Order";
            this.contextOrderEditOrder.Click += new System.EventHandler(this.OnEditOrder);
            //
            // contextOrderCombineOrder
            //
            this.contextOrderCombineOrder.Image = global::ShipWorks.Properties.Resources.merge_icon_16x16;
            this.contextOrderCombineOrder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderCombineOrder, new System.Guid("e537a928-2b0c-4453-8f7d-1c1f1fd7d0fd"));
            this.contextOrderCombineOrder.Name = "contextOrderCombineOrder";
            this.contextOrderCombineOrder.Size = new System.Drawing.Size(169, 22);
            this.contextOrderCombineOrder.Text = "Combine Orders";
            this.contextOrderCombineOrder.Click += new System.EventHandler(this.OnCombineOrders);
            //
            // contextOrderSplitOrder
            //
            this.contextOrderSplitOrder.Image = global::ShipWorks.Properties.Resources.split_icon_16x16;
            this.contextOrderSplitOrder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSplitOrder, new System.Guid("69d57a7f-c089-4b43-a0c2-a6ac5f8dabfe"));
            this.contextOrderSplitOrder.Name = "contextOrderSplitOrder";
            this.contextOrderSplitOrder.Size = new System.Drawing.Size(169, 22);
            this.contextOrderSplitOrder.Text = "Split Order";
            this.contextOrderSplitOrder.Click += new System.EventHandler(this.OnSplitOrder);
            //
            // contextOrderSep1
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSep1, new System.Guid("25a89661-e7e7-46b6-8e76-dc2a3357d2a0"));
            this.contextOrderSep1.Name = "contextOrderSep1";
            this.contextOrderSep1.Size = new System.Drawing.Size(166, 6);
            //
            // contextOrderLocalStatus
            //
            this.contextOrderLocalStatus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stuffToolStripMenuItem});
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderLocalStatus, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderLocalStatus.Image = global::ShipWorks.Properties.Resources.document_pulse;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderLocalStatus, new System.Guid("00fdea99-3f34-49bc-87de-92fc0636b5b8"));
            this.contextOrderLocalStatus.Name = "contextOrderLocalStatus";
            this.gridMenuLayoutProvider.SetPermission(this.contextOrderLocalStatus, ShipWorks.Users.Security.PermissionType.OrdersEditStatus);
            this.contextOrderLocalStatus.Size = new System.Drawing.Size(169, 22);
            this.contextOrderLocalStatus.Text = "Local Status";
            this.contextOrderLocalStatus.DropDownOpening += new System.EventHandler(this.OnLocalStatusMenuOpening);
            //
            // stuffToolStripMenuItem
            //
            this.stuffToolStripMenuItem.Name = "stuffToolStripMenuItem";
            this.stuffToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.stuffToolStripMenuItem.Text = "Stuff";
            //
            // contextOrderOnlineUpdate
            //
            this.contextOrderOnlineUpdate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orderContextUpdateOnlineEbay,
            this.orderContextUpdateOnlineOsCommerce});
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderOnlineUpdate, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderOnlineUpdate.Image = global::ShipWorks.Properties.Resources.server_from_client;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderOnlineUpdate, new System.Guid("16ff9a2f-0a14-4efc-acac-f872844d18e7"));
            this.contextOrderOnlineUpdate.Name = "contextOrderOnlineUpdate";
            this.gridMenuLayoutProvider.SetPermission(this.contextOrderOnlineUpdate, ShipWorks.Users.Security.PermissionType.OrdersEditStatus);
            this.contextOrderOnlineUpdate.Size = new System.Drawing.Size(169, 22);
            this.contextOrderOnlineUpdate.Text = "Update Store";
            this.contextOrderOnlineUpdate.DropDownOpening += new System.EventHandler(this.OnUpdateOnlineMenuOpening);
            //
            // orderContextUpdateOnlineEbay
            //
            this.orderContextUpdateOnlineEbay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateShipmentToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripMenuItem1,
            this.markAsShippedToolStripMenuItem,
            this.toolStripSeparator4,
            this.toolStripMenuItem2,
            this.markAsNotShippedToolStripMenuItem});
            this.orderContextUpdateOnlineEbay.Name = "orderContextUpdateOnlineEbay";
            this.orderContextUpdateOnlineEbay.Size = new System.Drawing.Size(145, 22);
            this.orderContextUpdateOnlineEbay.Text = "eBay";
            //
            // updateShipmentToolStripMenuItem
            //
            this.updateShipmentToolStripMenuItem.Name = "updateShipmentToolStripMenuItem";
            this.updateShipmentToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.updateShipmentToolStripMenuItem.Text = "Update Shipment";
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem1.Text = "Mark as Paid";
            //
            // markAsShippedToolStripMenuItem
            //
            this.markAsShippedToolStripMenuItem.Name = "markAsShippedToolStripMenuItem";
            this.markAsShippedToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.markAsShippedToolStripMenuItem.Text = "Mark as Shipped";
            //
            // toolStripSeparator4
            //
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(181, 6);
            //
            // toolStripMenuItem2
            //
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem2.Text = "Mark as Not Paid";
            //
            // markAsNotShippedToolStripMenuItem
            //
            this.markAsNotShippedToolStripMenuItem.Name = "markAsNotShippedToolStripMenuItem";
            this.markAsNotShippedToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.markAsNotShippedToolStripMenuItem.Text = "Mark as Not Shipped";
            //
            // orderContextUpdateOnlineOsCommerce
            //
            this.orderContextUpdateOnlineOsCommerce.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orderContextUpdateOnlineOsCommercePlaceholder,
            this.commonYToolStripMenuItem,
            this.storeAToolStripMenuItem});
            this.orderContextUpdateOnlineOsCommerce.Name = "orderContextUpdateOnlineOsCommerce";
            this.orderContextUpdateOnlineOsCommerce.Size = new System.Drawing.Size(145, 22);
            this.orderContextUpdateOnlineOsCommerce.Text = "osCommerce";
            //
            // orderContextUpdateOnlineOsCommercePlaceholder
            //
            this.orderContextUpdateOnlineOsCommercePlaceholder.Name = "orderContextUpdateOnlineOsCommercePlaceholder";
            this.orderContextUpdateOnlineOsCommercePlaceholder.Size = new System.Drawing.Size(135, 22);
            this.orderContextUpdateOnlineOsCommercePlaceholder.Text = "Common X";
            //
            // commonYToolStripMenuItem
            //
            this.commonYToolStripMenuItem.Name = "commonYToolStripMenuItem";
            this.commonYToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.commonYToolStripMenuItem.Text = "Common Y";
            //
            // storeAToolStripMenuItem
            //
            this.storeAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.specificZToolStripMenuItem});
            this.storeAToolStripMenuItem.Name = "storeAToolStripMenuItem";
            this.storeAToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.storeAToolStripMenuItem.Text = "Store A";
            //
            // specificZToolStripMenuItem
            //
            this.specificZToolStripMenuItem.Name = "specificZToolStripMenuItem";
            this.specificZToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.specificZToolStripMenuItem.Text = "Specific Z";
            //
            // contextOrderCustomActions
            //
            this.contextOrderCustomActions.Image = global::ShipWorks.Properties.Resources.gear_run16;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderCustomActions, new System.Guid("3263d695-fa30-4738-81b5-1dc3bb18d82c"));
            this.contextOrderCustomActions.Name = "contextOrderCustomActions";
            this.contextOrderCustomActions.Size = new System.Drawing.Size(169, 22);
            this.contextOrderCustomActions.Text = "Custom Actions";
            //
            // contextOrderSep6
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSep6, new System.Guid("f1427a3c-d0d9-4c81-9827-e5061191d232"));
            this.contextOrderSep6.Name = "contextOrderSep6";
            this.gridMenuLayoutProvider.SetPermission(this.contextOrderSep6, ShipWorks.Users.Security.PermissionType.OrdersEditStatus);
            this.contextOrderSep6.Size = new System.Drawing.Size(166, 6);
            //
            // contextOrderEditCustomer
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderEditCustomer, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrder);
            this.contextOrderEditCustomer.Image = global::ShipWorks.Properties.Resources.customer_edit;
            this.contextOrderEditCustomer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderEditCustomer, new System.Guid("62812a8e-8883-4b6a-95d3-fe840ad7f92b"));
            this.contextOrderEditCustomer.Name = "contextOrderEditCustomer";
            this.contextOrderEditCustomer.Size = new System.Drawing.Size(169, 22);
            this.contextOrderEditCustomer.Text = "Edit Customer";
            this.contextOrderEditCustomer.Visible = false;
            this.contextOrderEditCustomer.Click += new System.EventHandler(this.OnContextOrderEditCustomer);
            //
            // contextOrderLookupCustomer
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderLookupCustomer, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrder);
            this.contextOrderLookupCustomer.Image = ((System.Drawing.Image) (resources.GetObject("contextOrderLookupCustomer.Image")));
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderLookupCustomer, new System.Guid("4cff76a9-1bdb-498e-8198-aa7d87adae45"));
            this.contextOrderLookupCustomer.Name = "contextOrderLookupCustomer";
            this.contextOrderLookupCustomer.Size = new System.Drawing.Size(169, 22);
            this.contextOrderLookupCustomer.Text = "Lookup Customer";
            this.contextOrderLookupCustomer.Visible = false;
            this.contextOrderLookupCustomer.Click += new System.EventHandler(this.OnLookupCustomer);
            //
            // contextOrderSep2
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSep2, new System.Guid("9d041d8f-93b9-4676-a924-ea27a0f11f05"));
            this.contextOrderSep2.Name = "contextOrderSep2";
            this.contextOrderSep2.Size = new System.Drawing.Size(166, 6);
            this.contextOrderSep2.Visible = false;
            //
            // contextOrderShipOrders
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderShipOrders, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderShipOrders.Image = global::ShipWorks.Properties.Resources.box_closed16;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderShipOrders, new System.Guid("af9da9af-d77f-4994-99cc-da3e1fbca237"));
            this.contextOrderShipOrders.Name = "contextOrderShipOrders";
            this.contextOrderShipOrders.Size = new System.Drawing.Size(169, 22);
            this.contextOrderShipOrders.Text = "Ship Orders";
            this.contextOrderShipOrders.Click += new System.EventHandler(this.OnShipOrders);
            //
            // contextOrderTrackShipments
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderTrackShipments, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderTrackShipments.Image = global::ShipWorks.Properties.Resources.box_view16;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderTrackShipments, new System.Guid("7b697c6c-60cf-460f-b2d6-8bb2be1abbc4"));
            this.contextOrderTrackShipments.Name = "contextOrderTrackShipments";
            this.contextOrderTrackShipments.Size = new System.Drawing.Size(169, 22);
            this.contextOrderTrackShipments.Text = "Track Shipments";
            this.contextOrderTrackShipments.Click += new System.EventHandler(this.OnTrackShipments);
            //
            // contextOrderInsuranceClaim
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderInsuranceClaim, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderInsuranceClaim.Image = global::ShipWorks.Properties.Resources.message;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderInsuranceClaim, new System.Guid("ce0039b3-5b6e-4e8b-9a5d-d01ed5047a9e"));
            this.contextOrderInsuranceClaim.Name = "contextOrderInsuranceClaim";
            this.contextOrderInsuranceClaim.Size = new System.Drawing.Size(169, 22);
            this.contextOrderInsuranceClaim.Text = "File Claim";
            this.contextOrderInsuranceClaim.Click += new System.EventHandler(this.OnSubmitClaim);
            //
            // contextOrderSep3
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSep3, new System.Guid("07f0b3c6-ef59-4867-aeb8-2d7baaaf2e2a"));
            this.contextOrderSep3.Name = "contextOrderSep3";
            this.contextOrderSep3.Size = new System.Drawing.Size(166, 6);
            //
            // contextOrderCopy
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderCopy, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderCopy, new System.Guid("3789e9b4-00cb-4736-95b6-b9a0605da02b"));
            this.contextOrderCopy.Name = "contextOrderCopy";
            this.contextOrderCopy.Size = new System.Drawing.Size(169, 22);
            this.contextOrderCopy.Text = "Copy";
            //
            // contextOrderSep7
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSep7, new System.Guid("20b62d4d-2fd0-4103-a02f-353022dc6630"));
            this.contextOrderSep7.Name = "contextOrderSep7";
            this.contextOrderSep7.Size = new System.Drawing.Size(166, 6);
            //
            // contextOrderQuickPrint
            //
            this.contextOrderQuickPrint.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderQuickPrint, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderQuickPrint.Image = global::ShipWorks.Properties.Resources.printer_ok;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderQuickPrint, new System.Guid("43becaa8-f22c-4e38-b0b6-5a2060a4aba5"));
            this.contextOrderQuickPrint.Name = "contextOrderQuickPrint";
            this.contextOrderQuickPrint.Size = new System.Drawing.Size(169, 22);
            this.contextOrderQuickPrint.Text = "Quick Print";
            this.contextOrderQuickPrint.DropDownOpening += new System.EventHandler(this.OnQuickPrintMenuOpening);
            //
            // contextMenuTemplatesPlaceholder
            //
            this.contextMenuTemplatesPlaceholder.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuTemplatesPlaceholder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuTemplatesPlaceholderItem});
            this.contextMenuTemplatesPlaceholder.Name = "contextMenuTemplatesPlaceholder";
            this.contextMenuTemplatesPlaceholder.OwnerItem = this.contextCustomerComposeEmail;
            this.contextMenuTemplatesPlaceholder.Size = new System.Drawing.Size(137, 26);
            //
            // contextMenuTemplatesPlaceholderItem
            //
            this.contextMenuTemplatesPlaceholderItem.Name = "contextMenuTemplatesPlaceholderItem";
            this.contextMenuTemplatesPlaceholderItem.Size = new System.Drawing.Size(136, 22);
            this.contextMenuTemplatesPlaceholderItem.Text = "Placeholder";
            //
            // contextCustomerPrint
            //
            this.contextCustomerPrint.DropDown = this.contextMenuTemplatesPlaceholder;
            this.contextCustomerPrint.Image = global::ShipWorks.Properties.Resources.printer1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerPrint, new System.Guid("38cce3e9-137e-4dba-8f11-f88b0637b4f9"));
            this.contextCustomerPrint.Name = "contextCustomerPrint";
            this.contextCustomerPrint.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerPrint.Text = "Print";
            this.contextCustomerPrint.Visible = false;
            this.contextCustomerPrint.DropDownOpening += new System.EventHandler(this.OnPrintMenuOpening);
            //
            // contextOrderPrint
            //
            this.contextOrderPrint.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderPrint, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderPrint.Image = global::ShipWorks.Properties.Resources.printer1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderPrint, new System.Guid("d35c6b65-49ff-4486-beb4-41d619e06e9a"));
            this.contextOrderPrint.Name = "contextOrderPrint";
            this.contextOrderPrint.Size = new System.Drawing.Size(169, 22);
            this.contextOrderPrint.Text = "Print";
            this.contextOrderPrint.DropDownOpening += new System.EventHandler(this.OnPrintMenuOpening);
            //
            // contextOrderPreview
            //
            this.contextOrderPreview.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderPreview, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderPreview.Image = global::ShipWorks.Properties.Resources.printer_view16;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderPreview, new System.Guid("11333623-2689-4994-899a-fe26ac9603a7"));
            this.contextOrderPreview.Name = "contextOrderPreview";
            this.contextOrderPreview.Size = new System.Drawing.Size(169, 22);
            this.contextOrderPreview.Text = "Preview";
            this.contextOrderPreview.DropDownOpening += new System.EventHandler(this.OnPreviewMenuOpening);
            //
            // contextOrderSep4
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSep4, new System.Guid("70c83341-0ea7-4a44-bbb8-0201387498fc"));
            this.contextOrderSep4.Name = "contextOrderSep4";
            this.contextOrderSep4.Size = new System.Drawing.Size(166, 6);
            //
            // contextOrderEmailNow
            //
            this.contextOrderEmailNow.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderEmailNow, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderEmailNow.Image = global::ShipWorks.Properties.Resources.mail_forward1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderEmailNow, new System.Guid("492594ee-bbff-40d9-ad5f-21ddc30b7514"));
            this.contextOrderEmailNow.Name = "contextOrderEmailNow";
            this.gridMenuLayoutProvider.SetPermission(this.contextOrderEmailNow, ShipWorks.Users.Security.PermissionType.OrdersSendEmail);
            this.contextOrderEmailNow.Size = new System.Drawing.Size(169, 22);
            this.contextOrderEmailNow.Text = "Email Now";
            this.contextOrderEmailNow.DropDownOpening += new System.EventHandler(this.OnEmailNowMenuOpening);
            //
            // contextOrderComposeEmail
            //
            this.contextOrderComposeEmail.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderComposeEmail, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderComposeEmail.Image = global::ShipWorks.Properties.Resources.mail_write1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderComposeEmail, new System.Guid("d90d9f70-3a46-41f9-b9ff-59f444c995a7"));
            this.contextOrderComposeEmail.Name = "contextOrderComposeEmail";
            this.gridMenuLayoutProvider.SetPermission(this.contextOrderComposeEmail, ShipWorks.Users.Security.PermissionType.OrdersSendEmail);
            this.contextOrderComposeEmail.Size = new System.Drawing.Size(169, 22);
            this.contextOrderComposeEmail.Text = "Compose Email";
            this.contextOrderComposeEmail.DropDownOpening += new System.EventHandler(this.OnComposeEmailMenuOpening);
            //
            // contextOrderSep5
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSep5, new System.Guid("93585d26-f5f9-46af-8a7d-389b21701b21"));
            this.contextOrderSep5.Name = "contextOrderSep5";
            this.gridMenuLayoutProvider.SetPermission(this.contextOrderSep5, ShipWorks.Users.Security.PermissionType.OrdersSendEmail);
            this.contextOrderSep5.Size = new System.Drawing.Size(166, 6);
            this.contextOrderSep5.Visible = false;
            //
            // contextOrderSave
            //
            this.contextOrderSave.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderSave, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderSave.Image = global::ShipWorks.Properties.Resources.disk_blue1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSave, new System.Guid("2ddae66f-6e3f-4cd3-8f3f-6e993340d720"));
            this.contextOrderSave.Name = "contextOrderSave";
            this.contextOrderSave.Size = new System.Drawing.Size(169, 22);
            this.contextOrderSave.Text = "Save";
            this.contextOrderSave.Visible = false;
            this.contextOrderSave.DropDownOpening += new System.EventHandler(this.OnSaveMenuOpening);
            //
            // contextOrderSaveOpen
            //
            this.contextOrderSaveOpen.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextOrderSaveOpen, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.contextOrderSaveOpen.Image = global::ShipWorks.Properties.Resources.disk_blue_window1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextOrderSaveOpen, new System.Guid("45740634-1267-49c5-ba6c-ee0f5f5be1d7"));
            this.contextOrderSaveOpen.Name = "contextOrderSaveOpen";
            this.contextOrderSaveOpen.Size = new System.Drawing.Size(169, 22);
            this.contextOrderSaveOpen.Text = "Save and Open";
            this.contextOrderSaveOpen.Visible = false;
            this.contextOrderSaveOpen.DropDownOpening += new System.EventHandler(this.OnSaveAndOpenMenuOpening);
            //
            // contextCustomerSaveOpen
            //
            this.contextCustomerSaveOpen.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerSaveOpen, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreCustomers);
            this.contextCustomerSaveOpen.Image = global::ShipWorks.Properties.Resources.disk_blue_window1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerSaveOpen, new System.Guid("a51ed164-e911-4e9c-9b6c-ead7435456de"));
            this.contextCustomerSaveOpen.Name = "contextCustomerSaveOpen";
            this.contextCustomerSaveOpen.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerSaveOpen.Text = "Save and Open";
            this.contextCustomerSaveOpen.Visible = false;
            this.contextCustomerSaveOpen.DropDownOpening += new System.EventHandler(this.OnSaveAndOpenMenuOpening);
            //
            // contextCustomerPreview
            //
            this.contextCustomerPreview.DropDown = this.contextMenuTemplatesPlaceholder;
            this.contextCustomerPreview.Image = global::ShipWorks.Properties.Resources.printer_view16;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerPreview, new System.Guid("bc35d3c7-5a87-4cc1-8a2b-1d6cba92f22a"));
            this.contextCustomerPreview.Name = "contextCustomerPreview";
            this.contextCustomerPreview.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerPreview.Text = "Preview";
            this.contextCustomerPreview.Visible = false;
            this.contextCustomerPreview.DropDownOpening += new System.EventHandler(this.OnPreviewMenuOpening);
            //
            // contextCustomerEmailNow
            //
            this.contextCustomerEmailNow.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerEmailNow, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreCustomers);
            this.contextCustomerEmailNow.Image = global::ShipWorks.Properties.Resources.mail_forward1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerEmailNow, new System.Guid("8a832f81-a470-44cb-a984-7e7e4f93c2f1"));
            this.contextCustomerEmailNow.Name = "contextCustomerEmailNow";
            this.gridMenuLayoutProvider.SetPermission(this.contextCustomerEmailNow, ShipWorks.Users.Security.PermissionType.CustomersSendEmail);
            this.contextCustomerEmailNow.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerEmailNow.Text = "Email Now";
            this.contextCustomerEmailNow.Visible = false;
            this.contextCustomerEmailNow.DropDownOpening += new System.EventHandler(this.OnEmailNowMenuOpening);
            //
            // contextCustomerSave
            //
            this.contextCustomerSave.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerSave, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreCustomers);
            this.contextCustomerSave.Image = global::ShipWorks.Properties.Resources.disk_blue1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerSave, new System.Guid("2ea96f33-c914-40ff-9828-deb1556e1b4e"));
            this.contextCustomerSave.Name = "contextCustomerSave";
            this.contextCustomerSave.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerSave.Text = "Save";
            this.contextCustomerSave.Visible = false;
            this.contextCustomerSave.DropDownOpening += new System.EventHandler(this.OnSaveMenuOpening);
            //
            // contextCustomerComposeEmail
            //
            this.contextCustomerComposeEmail.DropDown = this.contextMenuTemplatesPlaceholder;
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerComposeEmail, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreCustomers);
            this.contextCustomerComposeEmail.Image = global::ShipWorks.Properties.Resources.mail_write1;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerComposeEmail, new System.Guid("403d8e5c-5669-4fa0-bc7f-041bc60bdb28"));
            this.contextCustomerComposeEmail.Name = "contextCustomerComposeEmail";
            this.gridMenuLayoutProvider.SetPermission(this.contextCustomerComposeEmail, ShipWorks.Users.Security.PermissionType.CustomersSendEmail);
            this.contextCustomerComposeEmail.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerComposeEmail.Text = "Compose Email";
            this.contextCustomerComposeEmail.Visible = false;
            this.contextCustomerComposeEmail.DropDownOpening += new System.EventHandler(this.OnComposeEmailMenuOpening);
            //
            // contextMenuCustomerGrid
            //
            this.contextMenuCustomerGrid.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuCustomerGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextCustomerEditCustomer,
            this.contextCustomerCustomActionsSep,
            this.contextCustomerCustomActions,
            this.contextCustomerSep1,
            this.contextCustomerNewOrder,
            this.contextCustomerLookupOrders,
            this.contextCustomerSep2,
            this.contextCustomerCopy,
            this.contextCustomerSep5,
            this.contextCustomerPrint,
            this.contextCustomerPreview,
            this.contextCustomerSep3,
            this.contextCustomerEmailNow,
            this.contextCustomerComposeEmail,
            this.contextCustomerSep4,
            this.contextCustomerSave,
            this.contextCustomerSaveOpen});
            this.contextMenuCustomerGrid.Name = "contextMenuCustomerGrid";
            this.contextMenuCustomerGrid.Size = new System.Drawing.Size(160, 282);
            //
            // contextCustomerEditCustomer
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerEditCustomer, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneCustomer);
            this.contextCustomerEditCustomer.Image = global::ShipWorks.Properties.Resources.customer_edit;
            this.contextCustomerEditCustomer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerEditCustomer, new System.Guid("0a6cb638-1ff9-489d-b325-2ef305c5e460"));
            this.contextCustomerEditCustomer.Name = "contextCustomerEditCustomer";
            this.contextCustomerEditCustomer.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerEditCustomer.Text = "Edit Customer";
            this.contextCustomerEditCustomer.Click += new System.EventHandler(this.OnEditCustomer);
            //
            // contextCustomerCustomActionsSep
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerCustomActionsSep, new System.Guid("c145fc99-b215-47a8-b496-25d0d7a2dc27"));
            this.contextCustomerCustomActionsSep.Name = "contextCustomerCustomActionsSep";
            this.contextCustomerCustomActionsSep.Size = new System.Drawing.Size(156, 6);
            //
            // contextCustomerCustomActions
            //
            this.contextCustomerCustomActions.Image = global::ShipWorks.Properties.Resources.gear_run16;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerCustomActions, new System.Guid("3263d695-fa30-4738-81b5-1dc3bb18d82d"));
            this.contextCustomerCustomActions.Name = "contextCustomerCustomActions";
            this.contextCustomerCustomActions.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerCustomActions.Text = "Custom Actions";
            //
            // contextCustomerSep1
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerSep1, new System.Guid("c145fc99-b215-47a8-b496-25d0d7a2dc28"));
            this.contextCustomerSep1.Name = "contextCustomerSep1";
            this.contextCustomerSep1.Size = new System.Drawing.Size(156, 6);
            //
            // contextCustomerNewOrder
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerNewOrder, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneCustomer);
            this.contextCustomerNewOrder.Image = global::ShipWorks.Properties.Resources.order_add;
            this.contextCustomerNewOrder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerNewOrder, new System.Guid("a06e626d-7849-46e5-9526-a644e67f525e"));
            this.contextCustomerNewOrder.Name = "contextCustomerNewOrder";
            this.gridMenuLayoutProvider.SetPermission(this.contextCustomerNewOrder, ShipWorks.Users.Security.PermissionType.CustomersAddOrder);
            this.contextCustomerNewOrder.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerNewOrder.Text = "New Order";
            this.contextCustomerNewOrder.Click += new System.EventHandler(this.OnNewOrder);
            //
            // contextCustomerLookupOrders
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerLookupOrders, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneCustomer);
            this.contextCustomerLookupOrders.Image = ((System.Drawing.Image) (resources.GetObject("contextCustomerLookupOrders.Image")));
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerLookupOrders, new System.Guid("5dc6ab77-dc4e-41c6-b535-082207de7e20"));
            this.contextCustomerLookupOrders.Name = "contextCustomerLookupOrders";
            this.contextCustomerLookupOrders.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerLookupOrders.Text = "Lookup Orders";
            this.contextCustomerLookupOrders.Click += new System.EventHandler(this.OnLookupOrders);
            //
            // contextCustomerSep2
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerSep2, new System.Guid("be42fcd9-10da-45ac-abdd-9b9d39673f30"));
            this.contextCustomerSep2.Name = "contextCustomerSep2";
            this.contextCustomerSep2.Size = new System.Drawing.Size(156, 6);
            //
            // contextCustomerCopy
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.contextCustomerCopy, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreCustomers);
            this.contextCustomerCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerCopy, new System.Guid("b29c29bf-0ea0-428e-b2cd-4bca4006366e"));
            this.contextCustomerCopy.Name = "contextCustomerCopy";
            this.contextCustomerCopy.Size = new System.Drawing.Size(159, 22);
            this.contextCustomerCopy.Text = "Copy";
            //
            // contextCustomerSep5
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerSep5, new System.Guid("5864689c-6b8e-4a20-9b6b-97fa88de8dfd"));
            this.contextCustomerSep5.Name = "contextCustomerSep5";
            this.contextCustomerSep5.Size = new System.Drawing.Size(156, 6);
            this.contextCustomerSep5.Visible = false;
            //
            // contextCustomerSep3
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerSep3, new System.Guid("38668a60-0ccd-4e3e-9a1d-4ced8733b07d"));
            this.contextCustomerSep3.Name = "contextCustomerSep3";
            this.contextCustomerSep3.Size = new System.Drawing.Size(156, 6);
            this.contextCustomerSep3.Visible = false;
            //
            // contextCustomerSep4
            //
            this.gridMenuLayoutProvider.SetLayoutGuid(this.contextCustomerSep4, new System.Guid("53c6005e-3b7c-458e-951d-a5bb37f63c09"));
            this.contextCustomerSep4.Name = "contextCustomerSep4";
            this.gridMenuLayoutProvider.SetPermission(this.contextCustomerSep4, ShipWorks.Users.Security.PermissionType.CustomersSendEmail);
            this.contextCustomerSep4.Size = new System.Drawing.Size(156, 6);
            this.contextCustomerSep4.Visible = false;
            //
            // gridMenuLayoutProvider
            //
            this.gridMenuLayoutProvider.CustomerGridMenu = this.contextMenuCustomerGrid;
            this.gridMenuLayoutProvider.OrderGridMenu = this.contextMenuOrderGrid;
            //
            // windowLayoutProvider
            //
            this.windowLayoutProvider.RibbonManager = this.ribbonManager;
            this.windowLayoutProvider.SandDockManager = this.sandDockManager;
            //
            // buttonShipOrders
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonShipOrders, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.buttonShipOrders.Guid = new System.Guid("21097c48-9a4b-4722-8ec3-b9e2afec93c2");
            this.buttonShipOrders.Image = ((System.Drawing.Image) (resources.GetObject("buttonShipOrders.Image")));
            this.buttonShipOrders.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonShipOrders.QuickAccessKey = "S";
            this.buttonShipOrders.Text = "Ship Orders";
            this.buttonShipOrders.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonShipOrders.Activate += new System.EventHandler(this.OnShipOrders);
            //
            // buttonTrackOrders
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonTrackOrders, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.buttonTrackOrders.Guid = new System.Guid("3d49345d-8c97-4a62-9e3b-24acb41ba469");
            this.buttonTrackOrders.Image = global::ShipWorks.Properties.Resources.box_view32;
            this.buttonTrackOrders.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonTrackOrders.QuickAccessKey = "T";
            this.buttonTrackOrders.Text = "Track";
            this.buttonTrackOrders.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonTrackOrders.Activate += new System.EventHandler(this.OnTrackShipments);
            //
            // buttonLocalStatus
            //
            this.buttonLocalStatus.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonLocalStatus, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.buttonLocalStatus.Guid = new System.Guid("020d3d77-23ad-4391-9bcc-60be5437b4aa");
            this.buttonLocalStatus.Image = ((System.Drawing.Image) (resources.GetObject("buttonLocalStatus.Image")));
            this.buttonLocalStatus.Padding = new Divelements.SandRibbon.WidgetEdges(8, 2, 8, 2);
            this.ribbonSecurityProvider.SetPermission(this.buttonLocalStatus, ShipWorks.Users.Security.PermissionType.OrdersEditStatus);
            this.buttonLocalStatus.PopupWidget = this.popupLocalStatus;
            this.buttonLocalStatus.QuickAccessKey = "L";
            this.buttonLocalStatus.Text = "Local\r\nStatus";
            this.buttonLocalStatus.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupLocalStatus
            //
            this.popupLocalStatus.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnLocalStatusRibbonOpening);
            //
            // buttonUpdateOnline
            //
            this.buttonUpdateOnline.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonUpdateOnline, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.buttonUpdateOnline.Guid = new System.Guid("e9d81cb9-ec6b-44f6-bdfa-46c2f61811f9");
            this.buttonUpdateOnline.Image = ((System.Drawing.Image) (resources.GetObject("buttonUpdateOnline.Image")));
            this.buttonUpdateOnline.Padding = new Divelements.SandRibbon.WidgetEdges(8, 2, 8, 2);
            this.ribbonSecurityProvider.SetPermission(this.buttonUpdateOnline, ShipWorks.Users.Security.PermissionType.OrdersEditStatus);
            this.buttonUpdateOnline.PopupWidget = this.popupUpdateOnline;
            this.buttonUpdateOnline.QuickAccessKey = "U";
            this.buttonUpdateOnline.Text = "Update\r\nOnline";
            this.buttonUpdateOnline.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupUpdateOnline
            //
            this.popupUpdateOnline.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnUpdateOnlineRibbonOpening);
            //
            // buttonPrint
            //
            this.buttonPrint.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonPrint, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreRows);
            this.buttonPrint.Guid = new System.Guid("7c807fb3-5927-4863-a5b0-df4304072d0d");
            this.buttonPrint.Image = global::ShipWorks.Properties.Resources.printer;
            this.buttonPrint.PopupWidget = this.popupPrint;
            this.buttonPrint.QuickAccessKey = "P";
            this.buttonPrint.Text = "Print";
            this.buttonPrint.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupPrint
            //
            this.popupPrint.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnPrintRibbonPopup);
            //
            // buttonPreview
            //
            this.buttonPreview.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonPreview, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreRows);
            this.buttonPreview.Guid = new System.Guid("89968f25-4ba5-4ea9-98a2-f9ff5388848a");
            this.buttonPreview.Image = global::ShipWorks.Properties.Resources.printer_view32;
            this.buttonPreview.PopupWidget = this.popupPreview;
            this.buttonPreview.QuickAccessKey = "V";
            this.buttonPreview.Text = "Preview";
            this.buttonPreview.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupPreview
            //
            this.popupPreview.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnPrintPreviewRibbonPopup);
            //
            // buttonEmailSend
            //
            this.buttonEmailSend.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonEmailSend, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreRows);
            this.buttonEmailSend.Guid = new System.Guid("ab2f4742-e91d-460c-8bde-6dff7fa7a0ea");
            this.buttonEmailSend.Image = global::ShipWorks.Properties.Resources.mail_forward;
            this.ribbonSecurityProvider.SetPermission(this.buttonEmailSend, ShipWorks.Users.Security.PermissionType.EntityTypeSendEmail);
            this.buttonEmailSend.PopupWidget = this.popupEmailSend;
            this.buttonEmailSend.QuickAccessKey = "E";
            this.buttonEmailSend.Text = "Send Now";
            this.buttonEmailSend.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupEmailSend
            //
            this.ribbonSecurityProvider.SetPermission(this.popupEmailSend, ShipWorks.Users.Security.PermissionType.EntityTypeSendEmail);
            this.popupEmailSend.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnEmailSendNowRibbonPopup);
            //
            // buttonEmailCompose
            //
            this.buttonEmailCompose.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonEmailCompose, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreRows);
            this.buttonEmailCompose.Guid = new System.Guid("e9dcfc44-0b5f-4986-abbd-2ba5ebd70880");
            this.buttonEmailCompose.Image = global::ShipWorks.Properties.Resources.mail_write;
            this.ribbonSecurityProvider.SetPermission(this.buttonEmailCompose, ShipWorks.Users.Security.PermissionType.EntityTypeSendEmail);
            this.buttonEmailCompose.PopupWidget = this.popupEmailCompose;
            this.buttonEmailCompose.QuickAccessKey = "C";
            this.buttonEmailCompose.Text = "Compose";
            this.buttonEmailCompose.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupEmailCompose
            //
            this.popupEmailCompose.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnEmailComposeRibbonPopup);
            //
            // buttonSave
            //
            this.buttonSave.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonSave, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreRows);
            this.buttonSave.Guid = new System.Guid("8add2e06-529e-46b6-83d9-28e1d38bbf2a");
            this.buttonSave.Image = global::ShipWorks.Properties.Resources.disk_blue;
            this.buttonSave.PopupWidget = this.popupSave;
            this.buttonSave.QuickAccessKey = "S";
            this.buttonSave.Text = "Save";
            this.buttonSave.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupSave
            //
            this.popupSave.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnSaveRibbonPopup);
            //
            // buttonSaveOpen
            //
            this.buttonSaveOpen.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonSaveOpen, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreRows);
            this.buttonSaveOpen.Guid = new System.Guid("2e88ebc9-2b6a-4b87-8136-89fb142a9658");
            this.buttonSaveOpen.Image = global::ShipWorks.Properties.Resources.disk_blue_window;
            this.buttonSaveOpen.PopupWidget = this.popupSaveOpen;
            this.buttonSaveOpen.QuickAccessKey = "O";
            this.buttonSaveOpen.Text = "Save && Open";
            this.buttonSaveOpen.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupSaveOpen
            //
            this.popupSaveOpen.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnSaveOpenRibbonPopup);
            //
            // buttonQuickPrint
            //
            this.buttonQuickPrint.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonQuickPrint, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreRows);
            this.buttonQuickPrint.Guid = new System.Guid("b1074a38-344c-473d-80c4-c3cc5c71b196");
            this.buttonQuickPrint.Image = global::ShipWorks.Properties.Resources.printer_ok1;
            this.buttonQuickPrint.PopupWidget = this.popupQuickPrint;
            this.buttonQuickPrint.QuickAccessKey = "Q";
            this.buttonQuickPrint.Text = "Quick Print";
            this.buttonQuickPrint.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupQuickPrint
            //
            this.popupQuickPrint.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnQuickPrintRibbonPopup);
            //
            // buttonInsuranceClaim
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonInsuranceClaim, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.buttonInsuranceClaim.Guid = new System.Guid("e21c1c0c-938a-4f01-a9fe-0b02075221bd");
            this.buttonInsuranceClaim.Image = global::ShipWorks.Properties.Resources.message32;
            this.buttonInsuranceClaim.Text = "File Claim";
            this.buttonInsuranceClaim.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonInsuranceClaim.Activate += new System.EventHandler(this.OnSubmitClaim);
            //
            // ribbon
            //
            this.ribbon.ApplicationImage = global::ShipWorks.Properties.Resources.sw_cubes_32x32;
            this.ribbon.ApplicationMenu = this.applicationMenu;
            this.ribbon.ApplicationToolTip = new Divelements.SandRibbon.SuperToolTip("Application", "Click here to manage ShipWorks.", null, false);
            this.ribbon.Controls.Add(this.ribbonTabGridViewHome);
            this.ribbon.Controls.Add(this.ribbonTabOrderLookupViewShipping);
            this.ribbon.Controls.Add(this.ribbonTabOrderLookupViewShipmentHistory);
            this.ribbon.Controls.Add(this.ribbonTabProducts);
            this.ribbon.Controls.Add(this.ribbonTabGridOutput);
            this.ribbon.Controls.Add(this.ribbonTabAdmin);
            this.ribbon.Controls.Add(this.ribbonTabView);
            this.ribbon.Controls.Add(this.ribbonTabHelp);
            this.ribbon.Location = new System.Drawing.Point(3, 3);
            this.ribbon.Manager = this.ribbonManager;
            this.ribbon.Name = "ribbon";
            this.ribbon.Size = new System.Drawing.Size(969, 146);
            this.ribbon.TabIndex = 0;
            this.ribbon.ToolBar = this.quickAccessToolBar;
            this.ribbon.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.OnHelpRequested);
            this.ribbon.SelectedTabChanged += OnRibbonSelectedTabChanged;
            //
            // applicationMenu
            //
            this.applicationMenu.DisplayAreaVisible = false;
            this.applicationMenu.DisplayAreaWidth = 100;
            this.applicationMenu.ExitButtonImage = ((System.Drawing.Image) (resources.GetObject("applicationMenu.ExitButtonImage")));
            this.applicationMenu.ExitButtonText = "E&xit ShipWorks";
            this.applicationMenu.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.mainMenuItemOptions,
            this.mainMenuItemSupport,
            this.mainMenuItemDatabase,
            this.mainMenuLogon});
            this.applicationMenu.OptionsButtonImage = ((System.Drawing.Image) (resources.GetObject("applicationMenu.OptionsButtonImage")));
            this.applicationMenu.OptionsButtonText = "ShipWorks Opt&ions";
            this.applicationMenu.OptionsButtonVisible = false;
            this.applicationMenu.RecentDocumentsHeading = "";
            this.applicationMenu.ShowOptions += new System.EventHandler(this.OnShowSettings);
            this.applicationMenu.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnBeforePopupApplicationMenu);
            //
            // mainMenuItemOptions
            //
            this.mainMenuItemOptions.Guid = new System.Guid("defc7604-28f0-4810-a047-949bfd4a6433");
            this.mainMenuItemOptions.Image = global::ShipWorks.Properties.Resources.preferences;
            this.mainMenuItemOptions.QuickAccessKey = "O";
            this.mainMenuItemOptions.Text = "&Settings";
            this.mainMenuItemOptions.Activate += new System.EventHandler(this.OnShowSettings);
            //
            // mainMenuItemSupport
            //
            this.mainMenuItemSupport.Guid = new System.Guid("9fa006eb-ba63-44d1-9bc2-fd9fae722b18");
            this.mainMenuItemSupport.Image = global::ShipWorks.Properties.Resources.help2;
            this.mainMenuItemSupport.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            menu1});
            this.mainMenuItemSupport.Text = "Help";
            //
            // mainMenuItemDatabase
            //
            this.mainMenuItemDatabase.GroupName = "Database";
            this.mainMenuItemDatabase.Guid = new System.Guid("7369df7b-d202-4f8f-b3f9-85107d641908");
            this.mainMenuItemDatabase.Image = global::ShipWorks.Properties.Resources.data;
            this.mainMenuItemDatabase.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            menu2});
            this.ribbonSecurityProvider.SetPermission(this.mainMenuItemDatabase, ShipWorks.Users.Security.PermissionType.DatabaseSetup);
            this.mainMenuItemDatabase.Text = "Database";
            //
            // mainMenuLogon
            //
            this.mainMenuLogon.GroupName = "LogOnOff";
            this.mainMenuLogon.Guid = new System.Guid("f39c30dd-6dae-4643-927b-4566b5619ce9");
            this.mainMenuLogon.Image = global::ShipWorks.Properties.Resources.user_lock_32;
            this.mainMenuLogon.QuickAccessKey = "L";
            this.mainMenuLogon.Text = "&Log On \\ Log Off";
            this.mainMenuLogon.Activate += new System.EventHandler(this.OnLogonLogoff);
            //
            // ribbonTabGridViewHome
            //
            this.ribbonTabGridViewHome.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkOrders,
            this.ribbonChunkCustomers,
            this.ribbonChunkShipping,
            this.ribbonChunkPickList,
            this.ribbonChunkManageEmail,
            this.ribbonChunkDownload,
            this.ribbonChunkQuickStart,
            });
            this.ribbonTabGridViewHome.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabGridViewHome.Manager = this.ribbonManager;
            this.ribbonTabGridViewHome.Name = "ribbonTabGridViewHome";
            this.ribbonTabGridViewHome.Size = new System.Drawing.Size(967, 90);
            this.ribbonTabGridViewHome.TabIndex = 0;
            this.ribbonTabGridViewHome.Text = "&Home";
            //
            // ribbonChunkOrders
            //
            this.ribbonChunkOrders.FurtherOptions = false;
            this.ribbonChunkOrders.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkOrders.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonEditOrder,
            this.stripLayoutModifyOrders,
            this.stripLayoutCombineSplitOrders,
            this.ribbonChunkOrdersSep1,
            this.buttonLocalStatus,
            this.buttonUpdateOnline});
            this.ribbonChunkOrders.Text = "Orders";
            //
            // ribbonChunkOrdersSep1
            //
            this.ribbonSecurityProvider.SetPermission(this.ribbonChunkOrdersSep1, ShipWorks.Users.Security.PermissionType.OrdersEditStatus);
            this.ribbonChunkOrdersSep1.Shortcut = System.Windows.Forms.Keys.None;
            this.ribbonChunkOrdersSep1.Size = 54;
            //
            // ribbonChunkCustomers
            //
            this.ribbonChunkCustomers.FurtherOptions = false;
            this.ribbonChunkCustomers.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkCustomers.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonEditCustomer,
            stripLayout6});
            this.ribbonChunkCustomers.Padding = new Divelements.SandRibbon.WidgetEdges(6, 4, 6, 4);
            this.ribbonChunkCustomers.Text = "Customers";
            //
            // ribbonChunkShipping
            //
            this.ribbonChunkShipping.FurtherOptions = false;
            this.ribbonChunkShipping.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkShipping.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonShipOrders,
            this.buttonTrackOrders,
            this.buttonInsuranceClaim,
            this.buttonFedExClose,
            this.buttonAsendiaClose,
            this.buttonDhlEcommerceManifest,
            this.buttonEndiciaSCAN});
            this.ribbonChunkShipping.Text = "Shipping";
            //
            // buttonFedExClose
            //
            this.buttonFedExClose.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonFedExClose.Guid = new System.Guid("a75086b1-ccbe-4fbd-846f-1bb1c40350fc");
            this.buttonFedExClose.Image = ((System.Drawing.Image) (resources.GetObject("buttonFedExClose.Image")));
            this.ribbonSecurityProvider.SetPermission(this.buttonFedExClose, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonFedExClose.PopupWidget = this.popupFedExEndOfDay;
            this.buttonFedExClose.QuickAccessKey = "F";
            this.buttonFedExClose.Text = "FedEx Close";
            this.buttonFedExClose.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupFedExEndOfDay
            //
            this.popupFedExEndOfDay.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            menuFedExEndOfDay});
            this.popupFedExEndOfDay.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnFedExClosePopupOpening);
            //
            // buttonAsendiaClose
            //
            this.buttonAsendiaClose.Guid = new System.Guid("1A47D1AA-92F7-4A2A-980D-C8C2CA72B0A9");
            this.buttonAsendiaClose.Image = global::ShipWorks.Properties.Resources.element_into;
            this.ribbonSecurityProvider.SetPermission(this.buttonAsendiaClose, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonAsendiaClose.Activate += new System.EventHandler(this.OnAsendiaManifest);
            this.buttonAsendiaClose.Text = "Asendia Close";
            this.buttonAsendiaClose.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonDhlEcommerceManifest
            //
            this.buttonDhlEcommerceManifest.Guid = new System.Guid("F27BEBA4-B9D2-41CE-BDE9-F38828C1C654");
            this.buttonDhlEcommerceManifest.Image = global::ShipWorks.Properties.Resources.element_into;
            this.ribbonSecurityProvider.SetPermission(this.buttonDhlEcommerceManifest, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonDhlEcommerceManifest.Text = "DHL eCommerce Manifest";
            this.buttonDhlEcommerceManifest.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonDhlEcommerceManifest.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonDhlEcommerceManifest.PopupWidget = this.popupDhlEcommerceManifest;
            //
            // popupDhlEcommerceManifest
            //
            this.popupDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuDhlEcommerceManifest});
            this.popupDhlEcommerceManifest.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnDhlEcommerceManifestOpening);
            //
            // menuDhlEcommerceManifest
            //
            this.menuDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuItemCreateDhlEcommerceManifest,
            this.menuItemPrintDhlEcommerceManifest});
            //
            // menuItemCreateDhlEcommerceManifest
            //
            this.menuItemCreateDhlEcommerceManifest.Guid = new System.Guid("D47353F7-44C7-4E5F-808F-7DA37C30D6D7");
            this.menuItemCreateDhlEcommerceManifest.Text = "Create Manifest...";
            //
            // menuItemPrintDhlEcommerceManifest
            //
            this.menuItemPrintDhlEcommerceManifest.GroupName = "Reprint";
            this.menuItemPrintDhlEcommerceManifest.Guid = new System.Guid("38041BB9-EDA3-4360-ADF3-92287F8BF233");
            this.menuItemPrintDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuPrintDhlEcommerceManifest});
            this.menuItemPrintDhlEcommerceManifest.Text = "Print";
            //
            // menuPrintDhlEcommerceManifest
            //
            this.menuPrintDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuItemDhlEcommerceManifestNone});
            //
            // menuItemDhlEcommerceManifestNone
            //
            this.menuItemDhlEcommerceManifestNone.Enabled = false;
            this.menuItemDhlEcommerceManifestNone.Guid = new System.Guid("C9A440C3-2135-40EA-BC1A-38594A24A873");
            this.menuItemDhlEcommerceManifestNone.Text = "(none)";
            //
            // buttonEndiciaSCAN
            //
            this.buttonEndiciaSCAN.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonEndiciaSCAN.Guid = new System.Guid("fc07d31c-fd5b-4324-9515-81cb1619447e");
            this.buttonEndiciaSCAN.Image = global::ShipWorks.Properties.Resources.graphics_tablet;
            this.ribbonSecurityProvider.SetPermission(this.buttonEndiciaSCAN, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonEndiciaSCAN.PopupWidget = this.popupPostalScanForm;
            this.buttonEndiciaSCAN.QuickAccessKey = "N";
            this.buttonEndiciaSCAN.Text = "SCAN Form";
            this.buttonEndiciaSCAN.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupPostalScanForm
            //
            this.popupPostalScanForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuEndiciaScanForm});
            this.popupPostalScanForm.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnPostalScanFormOpening);
            //
            // menuEndiciaScanForm
            //
            this.menuEndiciaScanForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuCreateEndiciaScanForm,
            this.menuPrintScanForm});
            //
            // menuCreateEndiciaScanForm
            //
            this.menuCreateEndiciaScanForm.Guid = new System.Guid("788e0b68-4f17-44d1-b94c-f8137eab0bf2");
            this.menuCreateEndiciaScanForm.Text = "Create SCAN Form...";
            //
            // menuPrintScanForm
            //
            this.menuPrintScanForm.GroupName = "Reprint";
            this.menuPrintScanForm.Guid = new System.Guid("6e658a22-d649-4640-a333-4360d008260f");
            this.menuPrintScanForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuPrintEndiciaScanForm});
            this.menuPrintScanForm.Text = "Print";
            //
            // menuPrintEndiciaScanForm
            //
            this.menuPrintEndiciaScanForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuEndiciaScanFormNone});
            //
            // menuEndiciaScanFormNone
            //
            this.menuEndiciaScanFormNone.Enabled = false;
            this.menuEndiciaScanFormNone.Guid = new System.Guid("5624c19e-b90d-4d3f-af85-801d8ef47003");
            this.menuEndiciaScanFormNone.Text = "(none)";
            //
            // ribbonChunkManageEmail
            //
            this.ribbonChunkManageEmail.FurtherOptions = false;
            this.ribbonChunkManageEmail.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkManageEmail.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonEmailMessages});
            this.ribbonChunkManageEmail.Text = "Email";
            //
            // buttonEmailMessages
            //
            this.buttonEmailMessages.Guid = new System.Guid("c1957d56-70bc-496c-8617-2f8d8856695e");
            this.buttonEmailMessages.Image = global::ShipWorks.Properties.Resources.mail21;
            this.buttonEmailMessages.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonEmailMessages.QuickAccessKey = "E";
            this.buttonEmailMessages.Text = "Messages";
            this.buttonEmailMessages.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonEmailMessages.Activate += new System.EventHandler(this.OnEmailMessages);
            //
            // ribbonChunkDownload
            //
            this.ribbonChunkDownload.FurtherOptions = false;
            this.ribbonChunkDownload.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkDownload.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonDownload});
            this.ribbonChunkDownload.QuickAccessKey = " ";
            this.ribbonChunkDownload.Text = "Download";
            //
            // buttonDownload
            //
            this.buttonDownload.Guid = new System.Guid("c644b527-e841-484c-ba10-f308fc51438e");
            this.buttonDownload.Image = ((System.Drawing.Image) (resources.GetObject("buttonDownload.Image")));
            this.buttonDownload.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonDownload.QuickAccessKey = "D";
            this.buttonDownload.Text = "Download";
            this.buttonDownload.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonDownload.Activate += new System.EventHandler(this.OnDownloadOrders);
            //
            // ribbonChunkQuickStart
            //
            this.ribbonChunkQuickStart.FurtherOptions = false;
            this.ribbonChunkQuickStart.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkQuickStart.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
                this.buttonQuickStart});
            this.ribbonChunkQuickStart.QuickAccessKey = " ";
            this.ribbonChunkQuickStart.Text = "Quick Start";
            //
            // buttonDownload
            //
            this.buttonQuickStart.Guid = new System.Guid("83184C65-03F7-4D9D-9901-9C2EFA53FCAE");
            this.buttonQuickStart.Image = global::ShipWorks.Properties.Resources.window_gear;
            this.buttonQuickStart.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonQuickStart.QuickAccessKey = "Q";
            this.buttonQuickStart.Text = "Quick Start";
            this.buttonQuickStart.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonQuickStart.Activate += new System.EventHandler((sender, e) => this.ShowQuickStart());
            //
            // ribbonChunkPickList
            //
            this.ribbonChunkPickList.FurtherOptions = false;
            this.ribbonChunkPickList.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkPickList.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
                this.buttonPickList});
            this.ribbonChunkPickList.QuickAccessKey = " ";
            this.ribbonChunkPickList.Text = "Pick List";
            //
            // buttonPickList
            //
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonPickList, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            this.buttonPickList.Guid = new System.Guid("23265a10-f7d6-48d9-b394-c35627760632");
            this.buttonPickList.Image = global::ShipWorks.Properties.Resources.document_out1;
            this.buttonPickList.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonPickList.QuickAccessKey = "PL";
            this.buttonPickList.Text = "Print Pick List";
            this.buttonPickList.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonPickList.Activate += new System.EventHandler(this.OnPrintPickList);
            //
            // ribbonTabGridViewCreate
            //
            this.ribbonTabGridOutput.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkPrint,
            this.ribbonChunkSendEmail,
            this.ribbonChunkFile});
            this.ribbonTabGridOutput.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabGridOutput.Manager = this.ribbonManager;
            this.ribbonTabGridOutput.Name = "ribbonTabGridViewCreate";
            this.ribbonTabGridOutput.Size = new System.Drawing.Size(967, 90);
            this.ribbonTabGridOutput.TabIndex = 5;
            this.ribbonTabGridOutput.Text = "&Output";
            //
            // ribbonChunkPrint
            //
            this.ribbonChunkPrint.FurtherOptions = false;
            this.ribbonChunkPrint.ItemJustification = Divelements.SandRibbon.ItemJustification.Stretch;
            this.ribbonChunkPrint.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonQuickPrint,
            this.buttonPrint,
            this.buttonPreview});
            this.ribbonChunkPrint.Text = "Print";
            //
            // ribbonChunkSendEmail
            //
            this.ribbonChunkSendEmail.FurtherOptions = false;
            this.ribbonChunkSendEmail.ItemJustification = Divelements.SandRibbon.ItemJustification.Stretch;
            this.ribbonChunkSendEmail.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonEmailSend,
            this.buttonEmailCompose});
            this.ribbonChunkSendEmail.Text = "Email";
            //
            // ribbonChunkFile
            //
            this.ribbonChunkFile.FurtherOptions = false;
            this.ribbonChunkFile.ItemJustification = Divelements.SandRibbon.ItemJustification.Stretch;
            this.ribbonChunkFile.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonSave,
            this.buttonSaveOpen});
            this.ribbonChunkFile.Text = "File";
            //
            // ribbonTabProducts
            //
            this.ribbonTabProducts.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkProductCatalogViewActions});
            this.ribbonTabProducts.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabProducts.Manager = this.ribbonManager;
            this.ribbonTabProducts.Name = "ribbonTabProducts";
            this.ribbonTabProducts.Size = new System.Drawing.Size(967, 90);
            this.ribbonTabProducts.TabIndex = 0;
            this.ribbonTabProducts.Text = "Prod&ucts";
            //
            // ribbonTabOrderLookupViewShipping
            //
            this.ribbonTabOrderLookupViewShipping.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkOrderLookupViewShipping,
            this.ribbonChunkOrderLookupViewProfiles});
            this.ribbonTabOrderLookupViewShipping.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabOrderLookupViewShipping.Manager = this.ribbonManager;
            this.ribbonTabOrderLookupViewShipping.Name = "ribbonTabOrderLookupViewShipping";
            this.ribbonTabOrderLookupViewShipping.Size = new System.Drawing.Size(1267, 90);
            this.ribbonTabOrderLookupViewShipping.TabIndex = 0;
            this.ribbonTabOrderLookupViewShipping.Text = "&Scan-to-Ship";
            //
            // ribbonChunkOrderLookupViewShipping
            //
            this.ribbonChunkOrderLookupViewShipping.FurtherOptions = false;
            this.ribbonChunkOrderLookupViewShipping.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkOrderLookupViewShipping.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonOrderLookupViewCreateLabel,
            this.buttonOrderLookupViewReprint2,
            this.buttonOrderLookupViewNewShipment,
            this.buttonOrderLookupViewShipShipAgain,
            this.buttonOrderLookupViewUnverify
            });
            this.ribbonChunkOrderLookupViewShipping.Text = "Shipping";
            //
            // ribbonChunkProductCatalogViewActions
            //
            this.ribbonChunkProductCatalogViewActions.FurtherOptions = false;
            this.ribbonChunkProductCatalogViewActions.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkProductCatalogViewActions.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonProductCatalogAddProduct,
            this.buttonProductCatalogEditProduct,
            this.buttonProductCatalogExportProduct
            });
            this.ribbonChunkProductCatalogViewActions.Text = "Actions";
            //
            // buttonProductCatalogAddProduct
            //
            this.buttonProductCatalogAddProduct.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonProductCatalogAddProduct.Guid = new System.Guid("A7AFBDD6-4ED2-447C-90BD-309131882332");
            this.buttonProductCatalogAddProduct.Image = global::ShipWorks.Properties.Resources.box_closed_add32;
            this.buttonProductCatalogAddProduct.PopupWidget = this.popupAddProduct;
            this.buttonProductCatalogAddProduct.Text = "Add";
            this.buttonProductCatalogAddProduct.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // addProductMenuItemNewProduct
            //
            this.addProductMenuItemNewProduct.Text = "New Product";
            //
            // addProductMenuItemFromFile
            //
            this.addProductMenuItemFromFile.Text = "From File";
            //
            // addProductMenuItemVariant
            //
            this.addProductMenuItemVariant.Text = "Variant";
            this.addProductMenuItemVariant.Enabled = false;
            //
            // menuAddProduct
            //
            this.menuAddProduct.Items.AddRange(new Divelements.SandRibbon.WidgetBase[]
            {
                this.addProductMenuItemNewProduct,
                this.addProductMenuItemFromFile,
                this.addProductMenuItemVariant
            });
            //
            // popupAddProduct
            //
            this.popupAddProduct.Items.Add(menuAddProduct);
            //
            // buttonProductCatalogEditProduct
            //
            this.buttonProductCatalogEditProduct.Guid = new System.Guid("268C999B-A2A6-4D02-B2F8-40669284C08A");
            this.buttonProductCatalogEditProduct.Image = global::ShipWorks.Properties.Resources.form_blue_edit;
            this.buttonProductCatalogEditProduct.Text = "Edit";
            this.buttonProductCatalogEditProduct.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonProductCatalogEditProduct.Enabled = false;
            //
            // buttonProductCatalogExportProduct
            //
            this.buttonProductCatalogExportProduct.Guid = new System.Guid("822773F9-51DD-4699-A3E7-18E9BE40FAF6");
            this.buttonProductCatalogExportProduct.Image = global::ShipWorks.Properties.Resources.document_out1;
            this.buttonProductCatalogExportProduct.Text = "Export";
            this.buttonProductCatalogExportProduct.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonOrderLookupViewCreateLabel
            //
            this.buttonOrderLookupViewCreateLabel.Guid = new System.Guid("287f6db0-b93d-4db6-bbb1-9d0905704b59");
            this.buttonOrderLookupViewCreateLabel.Image = global::ShipWorks.Properties.Resources.box_next_32_32;
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewCreateLabel, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewCreateLabel.QuickAccessKey = "C";
            this.buttonOrderLookupViewCreateLabel.Text = "Create\r\nLabel";
            this.buttonOrderLookupViewCreateLabel.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewCreateLabel.Activate += OnButtonOrderLookupViewCreateLabel;
            //
            // buttonOrderLookupViewNewShipment
            //
            this.buttonOrderLookupViewNewShipment.Guid = new System.Guid("1617A3C0-6162-49F0-B32E-2F65CEB15B15");
            this.buttonOrderLookupViewNewShipment.Image = global::ShipWorks.Properties.Resources.box_closed_add32;
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewNewShipment, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewNewShipment.QuickAccessKey = "N";
            this.buttonOrderLookupViewNewShipment.Text = "New\r\nShipment";
            this.buttonOrderLookupViewNewShipment.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewNewShipment.Activate += OnButtonOrderLookupViewNewShipment;
            //
            // buttonOrderLookupViewShipShipAgain
            //
            this.buttonOrderLookupViewShipShipAgain.Guid = new System.Guid("edb00c75-1df8-43e1-9d27-cf949d5fe8e2");
            this.buttonOrderLookupViewShipShipAgain.Image = global::ShipWorks.Properties.Resources.box_closed_redo_32_32;
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewShipShipAgain, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewShipShipAgain.QuickAccessKey = "S";
            this.buttonOrderLookupViewShipShipAgain.Text = "Ship\r\nAgain";
            this.buttonOrderLookupViewShipShipAgain.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewShipShipAgain.Activate += OnButtonOrderLookupViewShipAgain;
            //
            // buttonOrderLookupViewReprint2
            //
            this.buttonOrderLookupViewReprint2.Guid = new System.Guid("6c84fb90-12c4-11e1-840d-7b25c5ee775a");
            this.buttonOrderLookupViewReprint2.Image = global::ShipWorks.Properties.Resources.printer;
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewReprint2, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewReprint2.QuickAccessKey = "R";
            this.buttonOrderLookupViewReprint2.Text = "Reprint\r\nLabel";
            this.buttonOrderLookupViewReprint2.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewReprint2.Activate += OnButtonOrderLookupViewReprint;
            //
            // buttonOrderLookupViewUnverify
            //
            this.buttonOrderLookupViewUnverify.Guid = new System.Guid("11d0afcb-9e08-4f4a-9596-00be8a8937ac");
            this.buttonOrderLookupViewUnverify.Image = global::ShipWorks.Properties.Resources.checkbox_circle_32;
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewUnverify, ShipWorks.Users.Security.PermissionType.OrdersModify);
            this.buttonOrderLookupViewUnverify.QuickAccessKey = "U";
            this.buttonOrderLookupViewUnverify.Text = "Un-verify";
            this.buttonOrderLookupViewUnverify.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewUnverify.Activate += OnButtonOrderLookupViewUnverify;

            //
            // ribbonChunkOrderLookupViewProfiles
            //
            this.ribbonChunkOrderLookupViewProfiles.FurtherOptions = false;
            this.ribbonChunkOrderLookupViewProfiles.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkOrderLookupViewProfiles.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonOrderLookupViewApplyProfile,
            this.buttonOrderLookupViewManageProfiles
    });
            this.ribbonChunkOrderLookupViewProfiles.Text = "Profiles";
            //
            // buttonOrderLookupViewApplyProfile
            //
            this.buttonOrderLookupViewApplyProfile.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonOrderLookupViewApplyProfile.Guid = new System.Guid("020d3d77-23ad-4391-9bcc-60be5437b4aa");
            this.buttonOrderLookupViewApplyProfile.Image = global::ShipWorks.Properties.Resources.document_out1;
            this.buttonOrderLookupViewApplyProfile.Padding = new Divelements.SandRibbon.WidgetEdges(8, 2, 8, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewApplyProfile, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewApplyProfile.QuickAccessKey = "A";
            this.buttonOrderLookupViewApplyProfile.Text = "Apply";
            this.buttonOrderLookupViewApplyProfile.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.selectionDependentEnabler.SetEnabledWhen(this.buttonOrderLookupViewApplyProfile, ShipWorks.ApplicationCore.Interaction.SelectionDependentType.OneOrMoreOrders);
            //
            // buttonOrderLookupViewManageProfiles
            //
            this.buttonOrderLookupViewManageProfiles.Guid = new System.Guid("1A9E0525-CDF7-4327-8861-CB3DDFDF22CA");
            this.buttonOrderLookupViewManageProfiles.Image = global::ShipWorks.Properties.Resources.box_closed_with_label_32_32;
            this.buttonOrderLookupViewManageProfiles.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonOrderLookupViewManageProfiles.QuickAccessKey = "M";
            this.buttonOrderLookupViewManageProfiles.Text = "Manage";
            this.buttonOrderLookupViewManageProfiles.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewManageProfiles.Activate += new System.EventHandler(this.OnManageShippingProfiles);
            //
            // ribbonTabOrderLookupViewShipmentHistory
            //
            this.ribbonTabOrderLookupViewShipmentHistory.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkOrderLookupViewActions});
            this.ribbonTabOrderLookupViewShipmentHistory.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabOrderLookupViewShipmentHistory.Manager = this.ribbonManager;
            this.ribbonTabOrderLookupViewShipmentHistory.Name = "ribbonTabOrderLookupViewShipmentHistory";
            this.ribbonTabOrderLookupViewShipmentHistory.Size = new System.Drawing.Size(967, 90);
            this.ribbonTabOrderLookupViewShipmentHistory.TabIndex = 0;
            this.ribbonTabOrderLookupViewShipmentHistory.Text = "H&istory";
            //
            // ribbonChunkOrderLookupViewActions
            //
            this.ribbonChunkOrderLookupViewActions.FurtherOptions = false;
            this.ribbonChunkOrderLookupViewActions.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkOrderLookupViewActions.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonOrderLookupViewVoid,
            this.buttonOrderLookupViewReprint,
            this.buttonOrderLookupViewShipAgain,
            this.buttonOrderLookupViewSCANForm,
            this.buttonOrderLookupViewFedExClose,
            this.buttonOrderLookupViewAsendiaClose,
            this.buttonOrderLookupViewDhlEcommerceManifest});
            this.ribbonChunkOrderLookupViewActions.Text = "Actions";
            //
            // buttonOrderLookupViewVoid
            //
            this.buttonOrderLookupViewVoid.Guid = new System.Guid("46F7AC0D-1DD5-41B7-93AC-E0E1DD1DD2E0");
            this.buttonOrderLookupViewVoid.Image = global::ShipWorks.Properties.Resources.box_void_32_32;
            this.buttonOrderLookupViewVoid.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewVoid, ShipWorks.Users.Security.PermissionType.ShipmentsVoidDelete);
            this.buttonOrderLookupViewVoid.QuickAccessKey = "V";
            this.buttonOrderLookupViewVoid.Text = "Void";
            this.buttonOrderLookupViewVoid.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonOrderLookupViewReprint
            //
            this.buttonOrderLookupViewReprint.Guid = new System.Guid("11DA60BB-CF89-493F-85C0-6095666292AC");
            this.buttonOrderLookupViewReprint.Image = global::ShipWorks.Properties.Resources.printer;
            this.buttonOrderLookupViewReprint.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewReprint, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewReprint.QuickAccessKey = "R";
            this.buttonOrderLookupViewReprint.Text = "Reprint";
            this.buttonOrderLookupViewReprint.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonOrderLookupViewShipAgain
            //
            this.buttonOrderLookupViewShipAgain.Guid = new System.Guid("6EBBD6E5-72EE-4BEE-9CDF-4301A55BDC70");
            this.buttonOrderLookupViewShipAgain.Image = global::ShipWorks.Properties.Resources.box_next_32_32;
            this.buttonOrderLookupViewShipAgain.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewShipAgain, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewShipAgain.QuickAccessKey = "S";
            this.buttonOrderLookupViewShipAgain.Text = "Ship Again";
            this.buttonOrderLookupViewShipAgain.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonOrderLookupViewSCANForm
            //
            this.buttonOrderLookupViewSCANForm.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonOrderLookupViewSCANForm.Guid = new System.Guid("020d3d77-23ad-4391-9bcc-60be5437b4aa");
            this.buttonOrderLookupViewSCANForm.Image = global::ShipWorks.Properties.Resources.graphics_tablet;
            this.buttonOrderLookupViewSCANForm.Padding = new Divelements.SandRibbon.WidgetEdges(8, 2, 8, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewSCANForm, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewSCANForm.PopupWidget = this.popupOrderLookupViewSCANForm;
            this.buttonOrderLookupViewSCANForm.QuickAccessKey = "N";
            this.buttonOrderLookupViewSCANForm.Text = "SCAN Form";
            this.buttonOrderLookupViewSCANForm.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonOrderLookupViewFedExClose
            //
            this.buttonOrderLookupViewFedExClose.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonOrderLookupViewFedExClose.Guid = new System.Guid("55C4B661-51F4-4B54-8D36-25902918DECE");
            this.buttonOrderLookupViewFedExClose.Image = ((System.Drawing.Image) (resources.GetObject("buttonFedExClose.Image")));
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewFedExClose, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewFedExClose.PopupWidget = this.popupOrderLookupViewFedExEndOfDay;
            this.buttonOrderLookupViewFedExClose.QuickAccessKey = "F";
            this.buttonOrderLookupViewFedExClose.Text = "FedEx Close";
            this.buttonOrderLookupViewFedExClose.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonOrderLookupViewAsendiaClose
            //
            this.buttonOrderLookupViewAsendiaClose.Guid = new System.Guid("B5962343-E43E-40A7-98B3-84F6869FC2A9");
            this.buttonOrderLookupViewAsendiaClose.Image = global::ShipWorks.Properties.Resources.element_into;
            this.buttonOrderLookupViewAsendiaClose.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewAsendiaClose, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewAsendiaClose.Activate += new System.EventHandler(this.OnAsendiaManifest);
            this.buttonOrderLookupViewAsendiaClose.Text = "Asendia Close";
            this.buttonOrderLookupViewAsendiaClose.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // buttonOrderLookupViewDhlEcommerceManifest
            //
            this.buttonOrderLookupViewDhlEcommerceManifest.Guid = new System.Guid("875EC51A-AF5D-42A3-9F4E-7AEEA1CB58EF");
            this.buttonOrderLookupViewDhlEcommerceManifest.Image = global::ShipWorks.Properties.Resources.element_into;
            this.buttonOrderLookupViewDhlEcommerceManifest.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonOrderLookupViewDhlEcommerceManifest, ShipWorks.Users.Security.PermissionType.ShipmentsCreateEditProcess);
            this.buttonOrderLookupViewDhlEcommerceManifest.Text = "DHL eCommerce Manifest";
            this.buttonOrderLookupViewDhlEcommerceManifest.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonOrderLookupViewDhlEcommerceManifest.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonOrderLookupViewDhlEcommerceManifest.PopupWidget = this.popupOrderLookupViewDhlEcommerceManifest;
            //
            // popupOrderLookupViewDhlEcommerceManifest
            //
            this.popupOrderLookupViewDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuOrderLookupViewDhlEcommerceManifest});
            this.popupOrderLookupViewDhlEcommerceManifest.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnOrderLookupViewDhlEcommerceManifestOpening);
            //
            // menuOrderLookupViewDhlEcommerceManifest
            //
            this.menuOrderLookupViewDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuItemOrderLookupViewCreateDhlEcommerceManifest,
            this.menuItemOrderLookupViewPrintDhlEcommerceManifest});
            //
            // menuItemOrderLookupViewCreateDhlEcommerceManifest
            //
            this.menuItemOrderLookupViewCreateDhlEcommerceManifest.Guid = new System.Guid("0566D42F-5FCB-4555-8503-4D9FF5B2C12A");
            this.menuItemOrderLookupViewCreateDhlEcommerceManifest.Text = "Create Manifest...";
            //
            // menuItemOrderLookupViewPrintDhlEcommerceManifest
            //
            this.menuItemOrderLookupViewPrintDhlEcommerceManifest.GroupName = "Reprint";
            this.menuItemOrderLookupViewPrintDhlEcommerceManifest.Guid = new System.Guid("41691AE0-A7BB-4311-AEC1-9C1640C33310");
            this.menuItemOrderLookupViewPrintDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuOrderLookupViewPrintDhlEcommerceManifest});
            this.menuItemOrderLookupViewPrintDhlEcommerceManifest.Text = "Print";
            //
            // menuOrderLookupViewPrintDhlEcommerceManifest
            //
            this.menuOrderLookupViewPrintDhlEcommerceManifest.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuItemOrderLookupViewDhlEcommerceManifestNone});
            //
            // menuItemOrderLookupViewDhlEcommerceManifestNone
            //
            this.menuItemOrderLookupViewDhlEcommerceManifestNone.Enabled = false;
            this.menuItemOrderLookupViewDhlEcommerceManifestNone.Guid = new System.Guid("DA8ECFF6-5F4B-4DB4-932D-64517449948F");
            this.menuItemOrderLookupViewDhlEcommerceManifestNone.Text = "(none)";
            //
            // popupOrderLookupViewFedExEndOfDay
            //
            this.popupOrderLookupViewFedExEndOfDay.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] { menuOrderLookupViewFedExEndOfDay
});
            this.popupOrderLookupViewFedExEndOfDay.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnOrderLookupViewFedExClosePopupOpening);
            //
            // popupPostalScanForm
            //
            this.popupOrderLookupViewSCANForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuOrderLookupViewEndiciaScanForm});
            this.popupOrderLookupViewSCANForm.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnOrderLookupViewPostalScanFormOpening);
            //
            // menuOrderLookupViewEndiciaScanForm
            //
            this.menuOrderLookupViewEndiciaScanForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuOrderLookupViewCreateEndiciaScanForm,
            this.menuOrderLookupViewPrintScanForm});
            //
            // menuOrderLookupViewCreateEndiciaScanForm
            //
            this.menuOrderLookupViewCreateEndiciaScanForm.Guid = new System.Guid("3494C176-AF18-4D60-8853-77027C1DB9B2");
            this.menuOrderLookupViewCreateEndiciaScanForm.Text = "Create SCAN Form...";
            //
            // menuOrderLookupViewPrintScanForm
            //
            this.menuOrderLookupViewPrintScanForm.GroupName = "Reprint";
            this.menuOrderLookupViewPrintScanForm.Guid = new System.Guid("0D98E502-C129-4802-834E-87BEE24192B5");
            this.menuOrderLookupViewPrintScanForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuOrderLookupViewPrintEndiciaScanForm});
            this.menuOrderLookupViewPrintScanForm.Text = "Print";
            //
            // menuOrderLookupViewPrintEndiciaScanForm
            //
            this.menuOrderLookupViewPrintEndiciaScanForm.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuOrderLookupViewEndiciaScanFormNone});
            //
            // menuOrderLookupViewEndiciaScanFormNone
            //
            this.menuOrderLookupViewEndiciaScanFormNone.Enabled = false;
            this.menuOrderLookupViewEndiciaScanFormNone.Guid = new System.Guid("961F1C13-E3FE-49BF-A2A8-BB7BEAD18B14");
            this.menuOrderLookupViewEndiciaScanFormNone.Text = "(none)";
            //
            // ribbonTabAdmin
            //
            this.ribbonTabAdmin.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkConfiguration,
            this.ribbonChunkHistory,
            this.ribbonChunkAdminDatabase});
            this.ribbonTabAdmin.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabAdmin.Manager = this.ribbonManager;
            this.ribbonTabAdmin.Name = "ribbonTabAdmin";
            this.ribbonTabAdmin.Size = new System.Drawing.Size(967, 90);
            this.ribbonTabAdmin.TabIndex = 3;
            this.ribbonTabAdmin.Text = "&Manage";
            //
            // ribbonChunkConfiguration
            //
            this.ribbonChunkConfiguration.FurtherOptions = false;
            this.ribbonChunkConfiguration.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkConfiguration.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            stripLayout3,
            stripLayout1});
            this.ribbonChunkConfiguration.Text = "Configuration";
            //
            // ribbonChunkHistory
            //
            this.ribbonChunkHistory.CenterLayout = false;
            this.ribbonChunkHistory.FurtherOptions = false;
            this.ribbonChunkHistory.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkHistory.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonAudit,
            this.buttonDownloadHistory});
            this.ribbonChunkHistory.ItemSpacing = 2;
            this.ribbonChunkHistory.Text = "History";
            //
            // buttonAudit
            //
            this.buttonAudit.Guid = new System.Guid("a1105d49-ae6f-4754-8244-b17474f9222e");
            this.buttonAudit.Image = global::ShipWorks.Properties.Resources.surveillance_camera1;
            this.buttonAudit.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.ribbonSecurityProvider.SetPermission(this.buttonAudit, ShipWorks.Users.Security.PermissionType.ManageUsers);
            this.buttonAudit.QuickAccessKey = "J";
            this.buttonAudit.Text = "Audit";
            this.buttonAudit.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonAudit.Activate += new System.EventHandler(this.OnAudit);
            //
            // buttonDownloadHistory
            //
            this.buttonDownloadHistory.Guid = new System.Guid("4845160a-04d1-40f2-a1b6-76472293ae17");
            this.buttonDownloadHistory.Image = global::ShipWorks.Properties.Resources.download_history32;
            this.buttonDownloadHistory.Padding = new Divelements.SandRibbon.WidgetEdges(3, 2, 4, 14);
            this.buttonDownloadHistory.QuickAccessKey = "H";
            this.buttonDownloadHistory.Text = "Download Log";
            this.buttonDownloadHistory.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonDownloadHistory.Activate += new System.EventHandler(this.OnDownloadHistory);
            //
            // ribbonChunkAdminDatabase
            //
            this.ribbonChunkAdminDatabase.FurtherOptions = false;
            this.ribbonChunkAdminDatabase.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkAdminDatabase.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonSetupDatabase,
            this.separator2,
            this.buttonArchive,
            this.buttonBackup,
            this.buttonRestore,
            stripLayoutDatabase});
            this.ribbonChunkAdminDatabase.Text = "Database";
            //
            // separator2
            //
            this.ribbonSecurityProvider.SetPermission(this.separator2, ShipWorks.Users.Security.PermissionType.DatabaseSetup);
            this.separator2.Shortcut = System.Windows.Forms.Keys.None;
            this.separator2.Size = 54;
            //
            // buttonBackup
            //
            this.buttonBackup.Guid = new System.Guid("11920151-61ba-449f-896d-19be8a3d8712");
            this.buttonBackup.Image = global::ShipWorks.Properties.Resources.data_disk32;
            this.ribbonSecurityProvider.SetPermission(this.buttonBackup, ShipWorks.Users.Security.PermissionType.DatabaseBackup);
            this.buttonBackup.QuickAccessKey = "B";
            this.buttonBackup.Text = "Backup";
            this.buttonBackup.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonBackup.Activate += new System.EventHandler(this.OnBackupShipWorks);
            //
            // ribbonTabView
            //
            this.ribbonTabView.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkDataViews,
            this.ribbonChunkGridSettings,
            this.ribbonChunkPanels,
            this.ribbonChunkEnvironment});
            this.ribbonTabView.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabView.Manager = this.ribbonManager;
            this.ribbonTabView.Name = "ribbonTabView";
            this.ribbonTabView.Size = new System.Drawing.Size(967, 90);
            this.ribbonTabView.TabIndex = 2;
            this.ribbonTabView.Text = "&View";
            //
            // ribbonChunkDataViews
            //
            this.ribbonChunkDataViews.FurtherOptions = false;
            this.ribbonChunkDataViews.ItemJustification = Divelements.SandRibbon.ItemJustification.Stretch;
            this.ribbonChunkDataViews.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonDetailViewNormal,
            this.buttonDetailViewNormalDetail,
            this.buttonDetailViewDetail,
            this.separatorDataViews,
            dataViewDetailSettingsStrip});
            this.ribbonChunkDataViews.Padding = new Divelements.SandRibbon.WidgetEdges(4, 2, 4, 2);
            this.ribbonChunkDataViews.Text = "Data Views";
            //
            // buttonDetailViewNormal
            //
            this.buttonDetailViewNormal.AutoToggle = Divelements.SandRibbon.AutoToggleType.Radio;
            this.buttonDetailViewNormal.GroupName = "View";
            this.buttonDetailViewNormal.Guid = new System.Guid("f5545469-d000-48bf-96a3-e829961db126");
            this.buttonDetailViewNormal.Image = ((System.Drawing.Image) (resources.GetObject("buttonDetailViewNormal.Image")));
            this.buttonDetailViewNormal.QuickAccessKey = "N";
            this.buttonDetailViewNormal.Text = "Normal";
            this.buttonDetailViewNormal.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonDetailViewNormal.Activate += new System.EventHandler(this.OnChangeDetailViewDetailMode);
            //
            // buttonDetailViewNormalDetail
            //
            this.buttonDetailViewNormalDetail.AutoToggle = Divelements.SandRibbon.AutoToggleType.Radio;
            this.buttonDetailViewNormalDetail.GroupName = "View";
            this.buttonDetailViewNormalDetail.Guid = new System.Guid("266781fc-e800-4ee4-9900-8268098f5cea");
            this.buttonDetailViewNormalDetail.Image = ((System.Drawing.Image) (resources.GetObject("buttonDetailViewNormalDetail.Image")));
            this.buttonDetailViewNormalDetail.QuickAccessKey = "D";
            this.buttonDetailViewNormalDetail.Text = "Normal with\r\nDetail";
            this.buttonDetailViewNormalDetail.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonDetailViewNormalDetail.Activate += new System.EventHandler(this.OnChangeDetailViewDetailMode);
            //
            // buttonDetailViewDetail
            //
            this.buttonDetailViewDetail.AutoToggle = Divelements.SandRibbon.AutoToggleType.Radio;
            this.buttonDetailViewDetail.GroupName = "View";
            this.buttonDetailViewDetail.Guid = new System.Guid("d92b00d4-3d6b-47de-a54d-a318445be23a");
            this.buttonDetailViewDetail.Image = ((System.Drawing.Image) (resources.GetObject("buttonDetailViewDetail.Image")));
            this.buttonDetailViewDetail.Padding = new Divelements.SandRibbon.WidgetEdges(6, 2, 6, 2);
            this.buttonDetailViewDetail.QuickAccessKey = "B";
            this.buttonDetailViewDetail.Text = "Detail\r\nOnly";
            this.buttonDetailViewDetail.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonDetailViewDetail.Activate += new System.EventHandler(this.OnChangeDetailViewDetailMode);
            //
            // separatorDataViews
            //
            this.separatorDataViews.Margin = new Divelements.SandRibbon.WidgetEdges(1, 3, 1, 3);
            this.separatorDataViews.Shortcut = System.Windows.Forms.Keys.None;
            //
            // ribbonChunkGridSettings
            //
            this.ribbonChunkGridSettings.FurtherOptions = false;
            this.ribbonChunkGridSettings.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkGridSettings.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonEditGridColumns,
            this.buttonEditGridMenus});
            this.ribbonChunkGridSettings.Padding = new Divelements.SandRibbon.WidgetEdges(4, 2, 4, 2);
            this.ribbonChunkGridSettings.Text = "Grid Settings";
            //
            // buttonEditGridColumns
            //
            this.buttonEditGridColumns.Guid = new System.Guid("513573ad-671d-41ca-a251-e2c16bd703cb");
            this.buttonEditGridColumns.Image = ((System.Drawing.Image) (resources.GetObject("buttonEditGridColumns.Image")));
            this.buttonEditGridColumns.QuickAccessKey = "C";
            this.buttonEditGridColumns.Text = "Grid Columns";
            this.buttonEditGridColumns.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonEditGridColumns.Activate += new System.EventHandler(this.OnEditGridColumns);
            //
            // buttonEditGridMenus
            //
            this.buttonEditGridMenus.Guid = new System.Guid("501aea60-0a53-42ee-96f3-1f4663dbd1e8");
            this.buttonEditGridMenus.Image = ((System.Drawing.Image) (resources.GetObject("buttonEditGridMenus.Image")));
            this.buttonEditGridMenus.QuickAccessKey = "M";
            this.buttonEditGridMenus.Text = "Context\r\nMenus";
            this.buttonEditGridMenus.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonEditGridMenus.Activate += new System.EventHandler(this.OnEditGridContextMenu);
            //
            // ribbonChunkPanels
            //
            this.ribbonChunkPanels.FurtherOptions = false;
            this.ribbonChunkPanels.ItemJustification = Divelements.SandRibbon.ItemJustification.Stretch;
            this.ribbonChunkPanels.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonShowPanels});
            this.ribbonChunkPanels.Text = "Panels";
            //
            // buttonShowPanels
            //
            this.buttonShowPanels.DropDownStyle = Divelements.SandRibbon.DropDownStyle.Integral;
            this.buttonShowPanels.Guid = new System.Guid("a5653e06-8492-4d02-a135-e5fdd99e540d");
            this.buttonShowPanels.Image = ((System.Drawing.Image) (resources.GetObject("buttonShowPanels.Image")));
            this.buttonShowPanels.Padding = new Divelements.SandRibbon.WidgetEdges(7, 2, 7, 2);
            this.buttonShowPanels.PopupWidget = this.popupShowPanels;
            this.buttonShowPanels.QuickAccessKey = "P";
            this.buttonShowPanels.Stretch = true;
            this.buttonShowPanels.Text = "Show Panels";
            this.buttonShowPanels.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // popupShowPanels
            //
            this.popupShowPanels.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuShowPanels});
            this.popupShowPanels.BeforePopup += new Divelements.SandRibbon.BeforePopupEventHandler(this.OnBeforePopupShowPanels);
            //
            // menuShowPanels
            //
            this.menuShowPanels.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.menuItemShowFiltersPanel});
            //
            // menuItemShowFiltersPanel
            //
            this.menuItemShowFiltersPanel.Guid = new System.Guid("9886fd8e-79c4-435b-ad4e-4ef43c2de27e");
            this.menuItemShowFiltersPanel.Image = global::ShipWorks.Properties.Resources.filter;
            this.menuItemShowFiltersPanel.Tag = "";
            this.menuItemShowFiltersPanel.Text = "Filters";
            //
            // ribbonChunkEnvironment
            //
            this.ribbonChunkEnvironment.FurtherOptions = false;
            this.ribbonChunkEnvironment.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonSaveEnvironment,
            this.buttonLoadEnvironment,
            this.buttonResetEnvironment});
            this.ribbonChunkEnvironment.Text = "Environment";
            //
            // buttonSaveEnvironment
            //
            this.buttonSaveEnvironment.Guid = new System.Guid("922f2a8b-e9ef-426a-9c3b-19cb7bfff28f");
            this.buttonSaveEnvironment.Image = global::ShipWorks.Properties.Resources.window_save;
            this.buttonSaveEnvironment.QuickAccessKey = "S";
            this.buttonSaveEnvironment.Text = "Save";
            this.buttonSaveEnvironment.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonSaveEnvironment.Activate += new System.EventHandler(this.OnSaveEnvironmentSettings);
            //
            // buttonLoadEnvironment
            //
            this.buttonLoadEnvironment.Guid = new System.Guid("63801a16-e0e2-487b-813b-b4ecd88a8e3f");
            this.buttonLoadEnvironment.Image = global::ShipWorks.Properties.Resources.window_open;
            this.buttonLoadEnvironment.QuickAccessKey = "L";
            this.buttonLoadEnvironment.Text = "Load";
            this.buttonLoadEnvironment.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonLoadEnvironment.Activate += new System.EventHandler(this.OnLoadEnvironmentSettings);
            //
            // buttonResetEnvironment
            //
            this.buttonResetEnvironment.Guid = new System.Guid("397280c0-8747-4f33-b1cb-bdfd8b589e91");
            this.buttonResetEnvironment.Image = global::ShipWorks.Properties.Resources.window_refresh;
            this.buttonResetEnvironment.QuickAccessKey = "R";
            this.buttonResetEnvironment.Text = "Reset";
            this.buttonResetEnvironment.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonResetEnvironment.Activate += new System.EventHandler(this.OnResetEnvironmentSettings);
            //
            // ribbonTabHelp
            //
            this.ribbonTabHelp.Chunks.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.ribbonChunkSupport,
            this.ribbonChunkSupplies,
            this.ribbonChunkAbout});
            this.ribbonTabHelp.Location = new System.Drawing.Point(1, 53);
            this.ribbonTabHelp.Manager = this.ribbonManager;
            this.ribbonTabHelp.Name = "ribbonTabHelp";
            this.ribbonTabHelp.Size = new System.Drawing.Size(967, 90);
            this.ribbonTabHelp.TabIndex = 6;
            this.ribbonTabHelp.Text = "He&lp";
            //
            // ribbonChunkSupport
            //
            this.ribbonChunkSupport.FurtherOptions = false;
            this.ribbonChunkSupport.ItemJustification = Divelements.SandRibbon.ItemJustification.Near;
            this.ribbonChunkSupport.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonHelpView,
            this.buttonHelpForum,
            this.buttonRequestHelp,
            this.separator1,
            this.buttonHelpRemote});
            this.ribbonChunkSupport.ItemSpacing = 12;
            this.ribbonChunkSupport.Text = "Support";
            //
            // buttonHelpView
            //
            this.buttonHelpView.Guid = new System.Guid("191a7d17-9f82-48e0-8ce6-7d81843a04b9");
            this.buttonHelpView.Image = global::ShipWorks.Properties.Resources.help2;
            this.buttonHelpView.Text = "View Help";
            this.buttonHelpView.QuickAccessKey = "L";
            this.buttonHelpView.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonHelpView.Activate += new System.EventHandler(this.OnViewHelp);
            //
            // buttonHelpForum
            //
            this.buttonHelpForum.Guid = new System.Guid("cf72da78-5b7c-463f-85e6-cbf919891936");
            this.buttonHelpForum.Image = global::ShipWorks.Properties.Resources.help_earth;
            this.buttonHelpForum.QuickAccessKey = "F";
            this.buttonHelpForum.Text = "Visit Forum";
            this.buttonHelpForum.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonHelpForum.Activate += new System.EventHandler(this.OnSupportForum);
            //
            // buttonRequestHelp
            //
            this.buttonRequestHelp.Guid = new System.Guid("774ff044-900b-4b99-93bb-a52469b4bebe");
            this.buttonRequestHelp.Image = global::ShipWorks.Properties.Resources.user_headset;
            this.buttonRequestHelp.QuickAccessKey = "S";
            this.buttonRequestHelp.Text = "Submit Support \r\nRequest";
            this.buttonRequestHelp.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonRequestHelp.Activate += new System.EventHandler(this.OnRequestHelp);
            //
            // separator1
            //
            this.separator1.Shortcut = System.Windows.Forms.Keys.None;
            this.separator1.Size = 54;
            //
            // buttonHelpRemote
            //
            this.buttonHelpRemote.Guid = new System.Guid("6cf9bab1-98a8-42ca-b15f-0bf198f5f6ff");
            this.buttonHelpRemote.Image = global::ShipWorks.Properties.Resources.remote_assist32;
            this.buttonHelpRemote.QuickAccessKey = "P";
            this.buttonHelpRemote.Text = "Generate PIN";
            this.buttonHelpRemote.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonHelpRemote.Activate += new System.EventHandler(this.OnRemoteAssistance);
            //
            // ribbonChunkSupplies
            //
            this.ribbonChunkSupplies.FurtherOptions = false;
            this.ribbonChunkSupplies.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonUship,
            this.buttonBuySupplies});
            this.ribbonChunkSupplies.Text = "Services";
            //
            // buttonUship
            //
            this.buttonUship.Guid = new System.Guid("d81ade92-db8c-4f88-9f00-ea3bd217f280");
            this.buttonUship.Image = ((System.Drawing.Image) (resources.GetObject("buttonUship.Image")));
            this.buttonUship.QuickAccessKey = "I";
            this.buttonUship.Text = "uShip LTL Rates";
            this.buttonUship.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonUship.Activate += new System.EventHandler(this.OnUShip);
            //
            // buttonBuySupplies
            //
            this.buttonBuySupplies.Guid = new System.Guid("b40dbc3d-62af-45f4-88a8-38dc1e2f6c0d");
            this.buttonBuySupplies.Image = global::ShipWorks.Properties.Resources.shoppingcart_full;
            this.buttonBuySupplies.QuickAccessKey = "U";
            this.buttonBuySupplies.Text = "Buy Supplies";
            this.buttonBuySupplies.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonBuySupplies.Activate += new System.EventHandler(this.OnBuySupplies);
            //
            // ribbonChunkAbout
            //
            this.ribbonChunkAbout.FurtherOptions = false;
            this.ribbonChunkAbout.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            this.buttonHelpAbout});
            this.ribbonChunkAbout.Text = "About";
            //
            // buttonHelpAbout
            //
            this.buttonHelpAbout.Guid = new System.Guid("f09c2eca-c334-4d7c-bc16-9e34d6452ce4");
            this.buttonHelpAbout.Image = global::ShipWorks.Properties.Resources.about;
            this.buttonHelpAbout.QuickAccessKey = "A";
            this.buttonHelpAbout.Text = "About ShipWorks";
            this.buttonHelpAbout.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            this.buttonHelpAbout.Activate += new System.EventHandler(this.OnAboutShipWorks);
            //
            // quickAccessToolBar
            //
            this.quickAccessToolBar.Items.AddRange(new Divelements.SandRibbon.WidgetBase[] {
            shortcut1});
            //
            // pictureBox6
            //
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(11, 48);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(16, 16);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 6;
            this.pictureBox6.TabStop = false;
            //
            // pictureBox7
            //
            this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox7.Image = global::ShipWorks.Properties.Resources.pin_blue;
            this.pictureBox7.Location = new System.Drawing.Point(11, 28);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(16, 16);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 3;
            this.pictureBox7.TabStop = false;
            //
            // pictureBox8
            //
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox8.Image = global::ShipWorks.Properties.Resources.cubes_yellow;
            this.pictureBox8.Location = new System.Drawing.Point(11, 6);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(16, 16);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox8.TabIndex = 0;
            this.pictureBox8.TabStop = false;
            //
            // dashboardArea
            //
            this.dashboardArea.BackColor = System.Drawing.Color.Transparent;
            this.dashboardArea.Controls.Add(this.dashboardBarSample);
            this.dashboardArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.dashboardArea.Location = new System.Drawing.Point(3, 149);
            this.dashboardArea.Name = "dashboardArea";
            this.dashboardArea.Size = new System.Drawing.Size(969, 28);
            this.dashboardArea.TabIndex = 6;
            //
            // ribbonSecurityProvider
            //
            this.ribbonSecurityProvider.MainGridControl = this.gridControl;
            this.ribbonSecurityProvider.Ribbon = this.ribbon;
            //
            // menuItem1
            //
            this.menuItem1.Image = global::ShipWorks.Properties.Resources.help_earth;
            this.menuItem1.Padding = new Divelements.SandRibbon.WidgetEdges(40, 3, 20, 3);
            this.menuItem1.Text = "Support Forum";
            //
            // buttonCreateLabel
            //
            this.buttonCreateLabel.Image = global::ShipWorks.Properties.Resources.box_next_32_32;
            this.buttonCreateLabel.QuickAccessKey = "OE";
            this.buttonCreateLabel.Text = "Create\r\nLabel";
            this.buttonCreateLabel.TextContentRelation = Divelements.SandRibbon.TextContentRelation.Underneath;
            //
            // MainForm
            //
            this.ApplicationText = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 750);
            this.Controls.Add(this.panelDockingArea);
            this.Controls.Add(this.panelArchiveNotification);
            this.Controls.Add(this.dashboardArea);
            this.Controls.Add(this.ribbon);
            this.Controls.Add(this.statusBar);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(310, 300);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowText = "ShipWorks";
            this.Activated += new System.EventHandler(this.OnActivated);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShown);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.OnHelpRequested);
            this.Resize += new System.EventHandler(this.OnResize);
            ((System.ComponentModel.ISupportInitialize) (this.downloadingStatusLabel.PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.emailingStatusLabel.PictureBox)).EndInit();
            this.panelDockingArea.ResumeLayout(false);
            this.dockContainer1.ResumeLayout(false);
            this.dockableWindowOrders.ResumeLayout(false);
            this.dockableWindowItems.ResumeLayout(false);
            this.dockableWindowCharges.ResumeLayout(false);
            this.dockableWindowMap.ResumeLayout(false);
            this.dockableWindowStreetView.ResumeLayout(false);
            this.dockableWindowPaymentDetails.ResumeLayout(false);
            this.dockableWindowShipments.ResumeLayout(false);
            this.dockableWindowEmail.ResumeLayout(false);
            this.dockableWindowPrinted.ResumeLayout(false);
            this.dockContainer.ResumeLayout(false);
            this.dockableWindowOrderFilters.ResumeLayout(false);
            this.dockableWindowCustomerFilters.ResumeLayout(false);
            this.dockableWindowNotes.ResumeLayout(false);
            this.notifyIconMenuStrip.ResumeLayout(false);
            this.contextMenuOrderGrid.ResumeLayout(false);
            this.contextMenuTemplatesPlaceholder.ResumeLayout(false);
            this.contextMenuCustomerGrid.ResumeLayout(false);
            this.ribbon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox8)).EndInit();
            this.dashboardArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Set button telemetry values
        /// </summary>
        private void SetButtonTelemetry()
        {
            buttonShowPanels.TelemetryEventName = "ShowPanels";
            buttonSaveEnvironment.TelemetryEventName = "Environment.Save";
            buttonLoadEnvironment.TelemetryEventName = "Environment.Load";
            buttonResetEnvironment.TelemetryEventName = "Environment.Reset";
            buttonManageStores.TelemetryEventName = "Stores.Manage";
            buttonBackup.TelemetryEventName = "Database.Backup";
            buttonRestore.TelemetryEventName = "Database.Restore";
            buttonArchive.TelemetryEventName = "Database.Archive";
            buttonSetupDatabase.TelemetryEventName = "Database.Setup";
            buttonChangeConnection.TelemetryEventName = "Database.ChangeConnection";
            buttonManageUsers.TelemetryEventName = "Users.Manage";
            buttonFirewall.TelemetryEventName = "Firewall";
            buttonDownload.TelemetryEventName = "Download";
            buttonPickList.TelemetryEventName = "PickList";
            buttonManageFilters.TelemetryEventName = "Filters.Manage";
            buttonManageTemplates.TelemetryEventName = "Templates.Manage";
            buttonManageActions.TelemetryEventName = "Actions.Manage";
            buttonPrint.TelemetryEventName = "Print";
            buttonPreview.TelemetryEventName = "Preview";
            buttonEmailSend.TelemetryEventName = "Email.Send";
            buttonEmailCompose.TelemetryEventName = "Email.Compose";
            buttonSave.TelemetryEventName = "Save";
            buttonSaveOpen.TelemetryEventName = "SaveOpen";
            buttonNewOrder.TelemetryEventName = "Order.New";
            buttonEditOrder.TelemetryEventName = "Order.Edit";
            buttonDeleteOrders.TelemetryEventName = "Order.Delete";
            buttonCombine.TelemetryEventName = "Order.Combine";
            buttonSplit.TelemetryEventName = "Order.Split";
            buttonNewCustomer.TelemetryEventName = "Customer.New";
            buttonEditCustomer.TelemetryEventName = "Customer.Edit";
            buttonDeleteCustomer.TelemetryEventName = "Customer.Delete";
            buttonShipOrders.TelemetryEventName = "Order.Ship";
            buttonTrackOrders.TelemetryEventName = "Order.Track";
            buttonSettings.TelemetryEventName = "Settings.";
            buttonEditGridColumns.TelemetryEventName = "GridColumns.Edit";
            buttonLocalStatus.TelemetryEventName = "Order.LocalStatus";
            buttonUpdateOnline.TelemetryEventName = "Order.UpdateOnline";
            buttonEditGridMenus.TelemetryEventName = "GridMenus.Edit";
            buttonDetailViewDetail.TelemetryEventName = "DetailView.Detail";
            buttonDetailViewNormal.TelemetryEventName = "DetailView.Normal";
            buttonDetailViewNormalDetail.TelemetryEventName = "DetailView.NormalDetail";
            buttonDetailViewHeightIncrease.TelemetryEventName = "DetailView.Height.Increase";
            buttonDetailViewHeightDecrease.TelemetryEventName = "DetailView.Height.Decrease";
            buttonQuickPrint.TelemetryEventName = "QuickPrint";
            buttonEmailAccounts.TelemetryEventName = "Email.Accounts";
            buttonDownloadHistory.TelemetryEventName = "DownloadHistory";
            buttonEmailMessages.TelemetryEventName = "Email.Messages";
            buttonAudit.TelemetryEventName = "Audit";
            buttonShippingSettings.TelemetryEventName = "Shipping.Settings";
            buttonShippingProfiles.TelemetryEventName = "Shipping.Profiles";
            buttonFedExClose.TelemetryEventName = "FedExClose";
            buttonAsendiaClose.TelemetryEventName = "AsendiaClose";
            buttonDhlEcommerceManifest.TelemetryEventName = "DhlEcommerceManifest";
            buttonEndiciaSCAN.TelemetryEventName = "EndiciaScan";
            buttonHelpForum.TelemetryEventName = "Help.Forum";
            buttonHelpRemote.TelemetryEventName = "Help.Remote";
            buttonHelpView.TelemetryEventName = "Help.View";
            buttonHelpAbout.TelemetryEventName = "Help.About";
            buttonRequestHelp.TelemetryEventName = "Help.Request";
            buttonBuySupplies.TelemetryEventName = "Supplies.Buy";
            buttonUship.TelemetryEventName = "Uship";
            buttonInsuranceClaim.TelemetryEventName = "Insurance.Claim";
            buttonOrderLookupViewCreateLabel.TelemetryEventName = "OrderLookup.Label.Create";
            buttonOrderLookupViewApplyProfile.TelemetryEventName = "OrderLookup.Profiles.Apply";
            buttonOrderLookupViewManageProfiles.TelemetryEventName = "OrderLookup.Profiles.Manage";
            buttonOrderLookupViewVoid.TelemetryEventName = "OrderLookup.Order.Void";
            buttonOrderLookupViewReprint.TelemetryEventName = "OrderLookup.Reprint";
            buttonOrderLookupViewReprint2.TelemetryEventName = "OrderLookup.Reprint";
            buttonOrderLookupViewShipAgain.TelemetryEventName = "OrderLookup.History.ShipAgain";
            buttonOrderLookupViewShipShipAgain.TelemetryEventName = "OrderLookup.Ship.ShipAgain";
            buttonOrderLookupViewNewShipment.TelemetryEventName = "OrderLookup.Ship.NewShipment";
            buttonOrderLookupViewUnverify.TelemetryEventName = "OrderLookup.Unverify";
            buttonOrderLookupViewSCANForm.TelemetryEventName = "OrderLookup.ScanForm";
            buttonOrderLookupViewFedExClose.TelemetryEventName = "OrderLookup.FedExClose";
            buttonOrderLookupViewAsendiaClose.TelemetryEventName = "OrderLookup.AsendiaClose";
            buttonOrderLookupViewDhlEcommerceManifest.TelemetryEventName = "OrderLookup.DhlEcommerceManifest";
            buttonCreateLabel.TelemetryEventName = "CreateLabel";
            buttonOrderLookupViewFields.TelemetryEventName = "OrderLookup.Fields";
            buttonProductCatalogEditProduct.TelemetryEventName = "Product.Edit";
            buttonProductCatalogExportProduct.TelemetryEventName = "Product.Export";

            addProductMenuItemNewProduct.TelemetryEventName = "Product.New";
            addProductMenuItemFromFile.TelemetryEventName = "Product.Import";
            addProductMenuItemVariant.TelemetryEventName = "Product.Variant";

            menuItemViewHelp.TelemetryEventName = "Logo.Help.View";
            menuItemSupportForum.TelemetryEventName = "Logo.Support.Forum";
            menuItemRequestHelp.TelemetryEventName = "Logo.Help.Request";
            menuItemRemoteAssistance.TelemetryEventName = "Logo.RemoteAssistance";
            menuItemBuySupplies.TelemetryEventName = "Logo.Supplies.Buy";
            menuItemHelpAbout.TelemetryEventName = "Logo.Help.About";

            mainMenuItemOptions.TelemetryEventName = "Logo.Settings";
            mainMenuItemSetupDatabase.TelemetryEventName = "Logo.Database.Setup";
            mainMenuItemBackupDatabase.TelemetryEventName = "Logo.Database.Backup";

            mainMenuItemProducts.TelemetryEventName = "Logo.Mode.Switch.Products";
            mainMenuItemBatchGrid.TelemetryEventName = "Logo.Mode.Switch.Batch";
            mainMenuItemOrderLookup.TelemetryEventName = "Logo.Mode.Switch.OrderLookup";
        }

        private Divelements.SandRibbon.RibbonManager ribbonManager;
        private Divelements.SandRibbon.Ribbon ribbon;
        private Divelements.SandRibbon.ApplicationMenu applicationMenu;
        private Divelements.SandRibbon.MainMenuItem mainMenuLogon;
        private Divelements.SandRibbon.RibbonTab ribbonTabOrderLookupViewShipping;
        private Divelements.SandRibbon.RibbonTab ribbonTabOrderLookupViewShipmentHistory;
        private Divelements.SandRibbon.RibbonTab ribbonTabProducts;
        private Divelements.SandRibbon.RibbonTab ribbonTabGridViewHome;
        private Divelements.SandRibbon.StatusBar statusBar;
        private Divelements.SandRibbon.RibbonTab ribbonTabView;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkPanels;
        private Divelements.SandRibbon.RibbonTab ribbonTabAdmin;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkConfiguration;
        private RibbonMainMenuItem mainMenuItemSetupDatabase;
        private RibbonMainMenuItem mainMenuItemOrderLookup;
        private RibbonMainMenuItem mainMenuItemBatchGrid;
        private RibbonMainMenuItem mainMenuItemProducts;
        private RibbonMainMenuItem mainMenuItemBackupDatabase;
        private Divelements.SandRibbon.QuickAccessToolBar quickAccessToolBar;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonShowPanels;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonSaveEnvironment;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonLoadEnvironment;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonResetEnvironment;
        private Divelements.SandRibbon.Popup popupShowPanels;
        private Divelements.SandRibbon.MenuItem menuItemShowFiltersPanel;
        private Divelements.SandRibbon.Menu menuShowPanels;
        private System.Windows.Forms.Panel panelArchiveNotification;
        private System.Windows.Forms.Panel panelDockingArea;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenShipWorks;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonManageStores;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkAdminDatabase;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonBackup;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonRestore;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonArchive;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonSetupDatabase;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonChangeConnection;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonManageUsers;
        private ShipWorks.Filters.Controls.FilterTree orderFilterTree;
        private ShipWorks.Filters.Controls.FilterTree customerFilterTree;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonFirewall;
        private ShipWorks.ApplicationCore.MainGridControl gridControl;
        private Divelements.SandRibbon.Label labelStatusTotal;
        private Divelements.SandRibbon.Label labelStatusSelected;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkDownload;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDownload;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkQuickStart;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonQuickStart;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkPickList;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonPickList;
        private RibbonMainMenuItem mainMenuItemOptions;
        private Divelements.SandRibbon.Label statusStretcherPlaceholder;
        private ShipWorks.UI.Controls.SandRibbon.ImageLabel downloadingStatusLabel;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonManageFilters;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonManageTemplates;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonManageActions;
        private Divelements.SandRibbon.RibbonTab ribbonTabGridOutput;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkPrint;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonPrint;
        private Divelements.SandRibbon.Popup popupPrint;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonPreview;
        private Divelements.SandRibbon.Popup popupPreview;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkSendEmail;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEmailSend;
        private Divelements.SandRibbon.Popup popupEmailSend;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEmailCompose;
        private Divelements.SandRibbon.Popup popupEmailCompose;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkFile;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonSave;
        private Divelements.SandRibbon.Popup popupSave;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonSaveOpen;
        private Divelements.SandRibbon.Popup popupSaveOpen;
        private Divelements.SandRibbon.Popup popupAddProduct;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkOrders;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkCustomers;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonNewOrder;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEditOrder;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDeleteOrders;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonCombine;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonSplit;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonNewCustomer;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEditCustomer;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDeleteCustomer;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.PictureBox pictureBox8;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkShipping;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonShipOrders;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonTrackOrders;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonSettings;
        private TD.SandDock.DockableWindow dockableWindowOrderFilters;
        private TD.SandDock.DockableWindow dockableWindowCustomerFilters;
        private TD.SandDock.SandDockManager sandDockManager;
        private TD.SandDock.DockContainer dockContainer;
        private System.Windows.Forms.ContextMenuStrip contextMenuOrderGrid;
        private System.Windows.Forms.ToolStripMenuItem contextOrderEditOrder;
        private System.Windows.Forms.ToolStripMenuItem contextOrderCombineOrder;
        private System.Windows.Forms.ToolStripMenuItem contextOrderSplitOrder;
        private System.Windows.Forms.ToolStripMenuItem contextOrderEditCustomer;
        private System.Windows.Forms.ToolStripMenuItem contextOrderLookupCustomer;
        private System.Windows.Forms.ToolStripSeparator contextOrderSep2;
        private System.Windows.Forms.ToolStripMenuItem contextOrderShipOrders;
        private System.Windows.Forms.ToolStripMenuItem contextOrderTrackShipments;
        private System.Windows.Forms.ToolStripSeparator contextOrderSep3;
        private System.Windows.Forms.ToolStripMenuItem contextOrderPrint;
        private System.Windows.Forms.ToolStripMenuItem contextOrderPreview;
        private System.Windows.Forms.ToolStripSeparator contextOrderSep4;
        private System.Windows.Forms.ToolStripMenuItem contextOrderEmailNow;
        private System.Windows.Forms.ToolStripMenuItem contextOrderComposeEmail;
        private System.Windows.Forms.ToolStripSeparator contextOrderSep5;
        private System.Windows.Forms.ToolStripMenuItem contextOrderSave;
        private System.Windows.Forms.ToolStripMenuItem contextOrderSaveOpen;
        private System.Windows.Forms.ToolStripSeparator contextOrderSep6;
        private System.Windows.Forms.ToolStripMenuItem contextOrderLocalStatus;
        private System.Windows.Forms.ToolStripMenuItem contextOrderOnlineUpdate;
        private System.Windows.Forms.ToolStripSeparator contextOrderSep1;
        private System.Windows.Forms.ContextMenuStrip contextMenuCustomerGrid;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerEditCustomer;
        private System.Windows.Forms.ToolStripSeparator contextCustomerSep1;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerNewOrder;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerLookupOrders;
        private System.Windows.Forms.ToolStripSeparator contextCustomerSep2;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerPrint;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerPreview;
        private System.Windows.Forms.ToolStripSeparator contextCustomerSep3;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerEmailNow;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerComposeEmail;
        private System.Windows.Forms.ToolStripSeparator contextCustomerSep4;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerSave;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerSaveOpen;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkEnvironment;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkDataViews;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEditGridColumns;
        private ShipWorks.ApplicationCore.Appearance.GridMenuLayoutProvider gridMenuLayoutProvider;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonLocalStatus;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonUpdateOnline;
        private Divelements.SandRibbon.Popup popupLocalStatus;
        private Divelements.SandRibbon.Popup popupUpdateOnline;
        private System.Windows.Forms.ToolStripMenuItem stuffToolStripMenuItem;
        private ShipWorks.ApplicationCore.Appearance.WindowLayoutProvider windowLayoutProvider;
        private Divelements.SandRibbon.Separator ribbonChunkOrdersSep1;
        private System.Windows.Forms.ToolStripMenuItem orderContextUpdateOnlineEbay;
        private System.Windows.Forms.ContextMenuStrip contextMenuTemplatesPlaceholder;
        private System.Windows.Forms.ToolStripMenuItem contextMenuTemplatesPlaceholderItem;
        private System.Windows.Forms.ToolStripMenuItem orderContextUpdateOnlineOsCommerce;
        private System.Windows.Forms.ToolStripMenuItem updateShipmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem markAsShippedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem markAsNotShippedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderContextUpdateOnlineOsCommercePlaceholder;
        private ShipWorks.ApplicationCore.Interaction.SelectionDependentEnabler selectionDependentEnabler;
        private System.Windows.Forms.ToolStripMenuItem commonYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem storeAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specificZToolStripMenuItem;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEditGridMenus;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDetailViewDetail;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDetailViewNormal;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDetailViewNormalDetail;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkGridSettings;
        private Divelements.SandRibbon.Separator separatorDataViews;
        private Divelements.SandRibbon.Label labelDetailViewDetailView;
        private Divelements.SandRibbon.ComboBox detailViewDetailTemplate;
        private Divelements.SandRibbon.WindowsComboBox detailViewDetailHeight;
        private Divelements.SandRibbon.ButtonGroup dataViewHeightButtons;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDetailViewHeightIncrease;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDetailViewHeightDecrease;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonQuickPrint;
        private Divelements.SandRibbon.Popup popupQuickPrint;
        private System.Windows.Forms.ToolStripMenuItem contextOrderQuickPrint;
        private ShipWorks.UI.Controls.SandRibbon.ImageLabel emailingStatusLabel;
        private System.Windows.Forms.Panel dashboardArea;
        private ShipWorks.ApplicationCore.Dashboard.DashboardBar dashboardBarSample;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEmailAccounts;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkHistory;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDownloadHistory;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkManageEmail;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEmailMessages;
        private TD.SandDock.DockableWindow dockableWindowNotes;
        private TD.SandDock.DockableWindow dockableWindowItems;
        private TD.SandDock.DockableWindow dockableWindowCharges;
        private TD.SandDock.DockableWindow dockableWindowPaymentDetails;
        private TD.SandDock.DockableWindow dockableWindowShipments;
        private TD.SandDock.DockableWindow dockableWindowMap;
        private TD.SandDock.DockableWindow dockableWindowStreetView;
        private TD.SandDock.DockContainer dockContainer1;
        private ShipWorks.Stores.Content.Panels.NotesPanel panelNotes;
        private System.Windows.Forms.ToolStripSeparator contextOrderSep7;
        private System.Windows.Forms.ToolStripMenuItem contextOrderCopy;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerCopy;
        private System.Windows.Forms.ToolStripSeparator contextCustomerSep5;
        private ShipWorks.Stores.Content.Panels.OrderChargesPanel panelCharges;
        private ShipWorks.Stores.Content.Panels.PaymentDetailsPanel panelPaymentDetail;
        private ShipWorks.Stores.Content.Panels.OrderItemsPanel panelItems;
        private ShipWorks.Stores.Content.Panels.ShipmentsPanel panelShipments;
        private ShipWorks.Stores.Content.Panels.MapPanel panelMap;
        private ShipWorks.Stores.Content.Panels.MapPanel panelStreetView;
        private TD.SandDock.DockableWindow dockableWindowEmail;
        private ShipWorks.Stores.Content.Panels.EmailOutboundPanel panelEmail;
        private TD.SandDock.DockableWindow dockableWindowPrinted;
        private TD.SandDock.DockableWindow dockableWindowOrders;
        private ShipWorks.Stores.Content.Panels.OrdersPanel panelOrders;
        private ShipWorks.Stores.Content.Panels.PrintResultsPanel panelPrinted;
        private Divelements.SandRibbon.StripLayout stripLayoutModifyOrders;
        private Divelements.SandRibbon.StripLayout stripLayoutCombineSplitOrders;
        private ShipWorks.Users.Security.RibbonSecurityProvider ribbonSecurityProvider;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonAudit;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonShippingSettings;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonShippingProfiles;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonFedExClose;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonAsendiaClose;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonDhlEcommerceManifest;
        private Divelements.SandRibbon.Popup popupDhlEcommerceManifest;
        private Divelements.SandRibbon.Popup popupOrderLookupViewDhlEcommerceManifest;
        private Divelements.SandRibbon.Menu menuDhlEcommerceManifest;
        private Divelements.SandRibbon.Menu menuOrderLookupViewDhlEcommerceManifest;
        private Divelements.SandRibbon.MenuItem menuItemCreateDhlEcommerceManifest;
        private Divelements.SandRibbon.MenuItem menuItemOrderLookupViewCreateDhlEcommerceManifest;
        private Divelements.SandRibbon.MenuItem menuItemPrintDhlEcommerceManifest;
        private Divelements.SandRibbon.MenuItem menuItemOrderLookupViewPrintDhlEcommerceManifest;
        private Divelements.SandRibbon.Menu menuPrintDhlEcommerceManifest;
        private Divelements.SandRibbon.Menu menuOrderLookupViewPrintDhlEcommerceManifest;
        private Divelements.SandRibbon.MenuItem menuItemDhlEcommerceManifestNone;
        private Divelements.SandRibbon.MenuItem menuItemOrderLookupViewDhlEcommerceManifestNone;
        private Divelements.SandRibbon.Popup popupFedExEndOfDay;
        private Divelements.SandRibbon.MenuItem menuFedExEndDayClose;
        private Divelements.SandRibbon.MenuItem menuFedExEndDayPrint;
        private Divelements.SandRibbon.MenuItem menuFedExEndDayPrintPlaceholder;
        private Divelements.SandRibbon.Menu menuFedExPrintReports;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonEndiciaSCAN;
        private Divelements.SandRibbon.Popup popupPostalScanForm;
        private Divelements.SandRibbon.MenuItem menuCreateEndiciaScanForm;
        private Divelements.SandRibbon.MenuItem menuOrderLookupViewCreateEndiciaScanForm;
        private Divelements.SandRibbon.MenuItem menuPrintScanForm;
        private Divelements.SandRibbon.MenuItem menuOrderLookupViewPrintScanForm;
        private Divelements.SandRibbon.MenuItem menuEndiciaScanFormNone;
        private Divelements.SandRibbon.MenuItem menuOrderLookupViewEndiciaScanFormNone;
        private Divelements.SandRibbon.Menu menuEndiciaScanForm;
        private Divelements.SandRibbon.Menu menuOrderLookupViewEndiciaScanForm;
        private Divelements.SandRibbon.Menu menuPrintEndiciaScanForm;
        private Divelements.SandRibbon.Menu menuOrderLookupViewPrintEndiciaScanForm;
        private Divelements.SandRibbon.MenuItem menuFedExSmartPostClose;
        private Divelements.SandRibbon.MainMenuItem mainMenuItemSupport;
        private RibbonMenuItem menuItemSupportForum;
        private RibbonMenuItem menuItemRemoteAssistance;
        private Divelements.SandRibbon.MenuItem menuOrderLookupViewFedExEndDayClose;
        private Divelements.SandRibbon.MenuItem menuOrderLookupViewFedExEndDayPrint;
        private Divelements.SandRibbon.MenuItem menuOrderLookupViewFedExEndDayPrintPlaceholder;
        private Divelements.SandRibbon.MenuItem menuOrderLookupViewFedExSmartPostClose;
        private Divelements.SandRibbon.Menu menuOrderLookupViewFedExPrintReports;
        private Editions.EditionGuiHelper editionGuiHelper;
        private Divelements.SandRibbon.RibbonTab ribbonTabHelp;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonHelpForum;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonHelpRemote;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonHelpView;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonHelpAbout;
        private Divelements.SandRibbon.MenuItem menuItem1;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkSupport;
        private RibbonMenuItem menuItemViewHelp;
        private RibbonMenuItem menuItemHelpAbout;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonRequestHelp;
        private RibbonMenuItem menuItemRequestHelp;
        private Divelements.SandRibbon.Separator separator1;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkAbout;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkSupplies;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonBuySupplies;
        private RibbonMenuItem menuItemBuySupplies;
        private Divelements.SandRibbon.Separator separator2;
        private Divelements.SandRibbon.MainMenuItem mainMenuItemDatabase;
        private System.Windows.Forms.ToolStripMenuItem contextOrderCustomActions;
        private System.Windows.Forms.ToolStripSeparator contextCustomerCustomActionsSep;
        private System.Windows.Forms.ToolStripMenuItem contextCustomerCustomActions;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonUship;
        private System.Windows.Forms.ToolStripMenuItem contextOrderInsuranceClaim;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonInsuranceClaim;
        private Divelements.SandRibbon.Menu uiModeMenu;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkOrderLookupViewShipping;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkOrderLookupViewProfiles;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkOrderLookupViewActions;
        private Divelements.SandRibbon.RibbonChunk ribbonChunkProductCatalogViewActions;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewCreateLabel;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewNewShipment;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewShipShipAgain;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewUnverify;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewApplyProfile;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewManageProfiles;
        private Divelements.SandRibbon.Popup popup1;
        private Divelements.SandRibbon.Popup popup2;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewVoid;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewReprint;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewReprint2;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewShipAgain;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewSCANForm;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewFedExClose;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewAsendiaClose;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewDhlEcommerceManifest;
        private Divelements.SandRibbon.Popup popupOrderLookupViewSCANForm;
        private Divelements.SandRibbon.Popup popupOrderLookupViewFedExEndOfDay;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonCreateLabel;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonOrderLookupViewFields;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonProductCatalogAddProduct;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonProductCatalogEditProduct;
        private ShipWorks.UI.Controls.SandRibbon.RibbonButton buttonProductCatalogExportProduct;
        private Divelements.SandRibbon.Menu menuAddProduct;
        private ShipWorks.UI.Controls.SandRibbon.RibbonMainMenuItem addProductMenuItemNewProduct;
        private ShipWorks.UI.Controls.SandRibbon.RibbonMainMenuItem addProductMenuItemFromFile;
        private ShipWorks.UI.Controls.SandRibbon.RibbonMainMenuItem addProductMenuItemVariant;
    }
}

