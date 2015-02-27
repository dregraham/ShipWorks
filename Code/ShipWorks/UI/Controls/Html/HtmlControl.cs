using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Interapptive.Shared;
using ShipWorks.UI.Controls.Html.Tables;
using ShipWorks.UI.Controls.Html.Glyphs;
using ShipWorks.UI.Controls.Html.UndoRedo;
using ShipWorks.UI.Controls.Html.Core;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// HTML control suitable for browsing and editing.
    /// 
    /// Credits:
    /// Lutz Roeder
    /// Steven Wood for the HTML Event handling idea and code.
    /// Christopher Slee for the region marking
    /// Jamie Hancock for many suggestions and improvements
    /// Nikhil Kothari for some new COM definitions and mshtml insights, see: http://www.nikhilk.net/Entry.aspx?id=11
    /// tim@itwriting.com, visit the messageboard at http://www.itwriting.com/HTMLEditorBase.php
    /// </summary>
    [Designer(typeof(HtmlControl.HtmlControlDesigner)), ComVisible(true)]
    public class HtmlControl : Control, IDisposable 
    {
        #region Designer to manage how the Control displays in Vs.net etc.

        protected internal class HtmlControlDesigner : System.Windows.Forms.Design.ControlDesigner 
        {
            protected override void PostFilterProperties(System.Collections.IDictionary properties) 
            {
                properties.Remove("Font");
                base.PostFilterProperties (properties);
            }
        }

        #endregion

        #region HtmlControlWndProc

        /// <summary>
        /// NativeWindow implementation for hooking the HtmlControl
        /// </summary>
        class HtmlControlWndProc : NativeWindow
        {
            HtmlControl htmlControl;

            /// <summary>
            /// Constructor
            /// </summary>
            public HtmlControlWndProc(HtmlControl htmlControl)
            {
                this.htmlControl = htmlControl;
            }

            /// <summary>
            /// Handle the WndProc
            /// </summary>
            protected override void WndProc(ref Message message)
            {
                base.WndProc(ref message);

                //The idea is only pass the key presses and mouse clicks (and not the right clicks) to the base form to process the events correctly.
                if ((message.Msg >= 0x100 && message.Msg <= 0x108) ||
                    (message.Msg >= 0x200 && message.Msg <= 0x020A &&
                     message.Msg != NativeMethods.WM_LBUTTONDOWN &&
                     message.Msg != NativeMethods.WM_RBUTTONDOWN &&
                     message.Msg != NativeMethods.WM_LBUTTONDBLCLK &&
                     message.Msg != NativeMethods.WM_MBUTTONDBLCLK &&
                     message.Msg != NativeMethods.WM_RBUTTONDBLCLK))
                {
                    htmlControl.InvokeWndProc(ref message);
                }
                else
                {
                    // These are separate because if you pass it to the base control then the right mouse clicks etc. don't fire.
                    switch (message.Msg)
                    {
                        case NativeMethods.WM_LBUTTONDOWN:
                            htmlControl.InvokeOnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, message.LParam.ToInt32() & 0xffff, Convert.ToInt32((message.LParam.ToInt32() & 0xffff0000) >> 16), 0));
                            break;

                        case NativeMethods.WM_RBUTTONDOWN:
                            htmlControl.InvokeOnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, message.LParam.ToInt32() & 0xffff, Convert.ToInt32((message.LParam.ToInt32() & 0xffff0000) >> 16), 0));
                            break;

                        case NativeMethods.WM_LBUTTONDBLCLK:
                        case NativeMethods.WM_MBUTTONDBLCLK:
                        case NativeMethods.WM_RBUTTONDBLCLK:
                            htmlControl.InvokeOnDoubleClick();
                            break;

                        case NativeMethods.WM_MOUSEHOVER:
                            Application.DoEvents();
                            break;

                        case NativeMethods.WM_MOUSELEAVE:
                            Application.DoEvents();
                            break;
                    }
                }

                if (message.Msg == NativeMethods.WM_LBUTTONUP)
                {
                    htmlControl.InvokeOnClick();
                }
            }
        }

        #endregion

        #region Command Status

        /// <summary>
        /// Defines the curent Command Status.
        /// </summary>
        public enum HtmlCommandStatus
        {
            /// <summary>Not supported</summary>
            Unsupported,
            /// <summary>Disabled</summary>
            Disabled,
            /// <summary>Enabled</summary>
            Enabled,
            /// <summary>Enable And Toggle Command On</summary>
            EnabledAndToggledOn,
            /// <summary>Unknown Status</summary>
            Unknown
        }

        #endregion

        // Public events
        public event NavigatedEventHandler Navigate;
        public event NavigatingEventHandler BeforeNavigate;
        public event ReadyStateChangedEventHandler ReadyStateChanged;
        public event EventHandler UpdateUI;
        public event EventHandler CurrentElementChanged;
        public event ElementSnapRectEventHandler ElementMoving;
        public event ElementSnapRectEventHandler ElementSizing;
        // public event MarkupChangedHandler MarkupChanged;

        // The last ready state that occorred
        private HtmlReadyState lastReadyState = HtmlReadyState.UnInitialized;

        // Default font and colors
        Font defaultFont = new Font("Arial", 10);
        Color defaultForeColor = Color.Black;
        Color defaultBackColor = Color.White;

        // Determines if we are in edit mode
        bool editMode = false;

        // Border style of the control
        BorderStyle borderStyle = BorderStyle.Fixed3D;

        // Clicked hyperlinks should open in a new window
        bool openLinksNewWindow = true;

        // Controls if images will be displayed
        bool showImages = true;

        // Controls if border guide lines will be shown
        bool showBorderGuides = true;

        // Controls if glyphs will be displayed
        bool showGlyphs = false;

        // Tracks if our constructor has finished running
        bool constructed = false;

        // Constrols if a context menu is allowed to display
        bool allowContextMenu = true;

        // Controls if scripts are allowed
        bool allowActiveContent = true;

        // Controls if the control can recieve focus
        bool allowActivation = true;

        // Controls if the user can select text
        bool allowTextSelection = true;

        // Controls if hyperlinks work
        bool allowNavigation = true;

        // Desired URL and content to load when the control is ready
        string desiredUrl = "";
        string desiredContent = "";

        // The HtmlSite instance
        HtmlSite htmlSite = null;

        // The external object that will be accessible to the html document via window.external
        object externalObject = null;

        // The HtmlDocument instance
        internal HtmlApi.HTMLDocument htmlDocumentClass = null;

        // The current html element
        HtmlApi.IHTMLElement currentElement = null;

        // Used for controlling undo\redo
        UndoManager undoManager;

        // For manipulating tables
        TableEditor tableEditor;

        private HtmlControlWndProc nativeDocWindow = null;

        // Minimum zoom% we go to when doing a ZoomToWidth\Fit
        const int minimumAutoZoom = 5;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlControl() 
        {
            // Double buffer redraws.
            SetStyle(ControlStyles.DoubleBuffer, true);
            				
            // Force creation of handle, needed to host mshtml
            CreateControl(); 

            // Create the undomanager
            undoManager = new UndoManager(this);

            // Create the table editor
            tableEditor = new TableEditor(this);

            constructed = true;
        }

        #region Initialization

        /// <summary>
        /// Sets up the wndProc for the native window that the theDocument is based on.
        /// </summary>
        internal void SetupWndProc(IntPtr Hwnd) 
        {
            if (nativeDocWindow != null) 
            {
                nativeDocWindow.ReleaseHandle();
            }

            nativeDocWindow = new HtmlControlWndProc(this);
            nativeDocWindow.AssignHandle(Hwnd);
        }

        /// <summary>
        /// Cleans up the WndProc for the native window that the theDocument is based on.
        /// </summary>
        internal void ReleaseWndProc() 
        {
            if (nativeDocWindow != null) 
            {
                nativeDocWindow.ReleaseHandle();
                nativeDocWindow = null;
            }
        }

        /// <summary>
        /// Reinitializes the control
        /// </summary>
        private void ReloadMshtml() 
        {
            CleanupControl();
            InitMshtml();
        }

        /// <summary>
        /// Cleans up the control completely.
        /// </summary>
        private void CleanupControl() 
        {
            if (IsCreated) 
            {
                htmlSite.Dispose();
                htmlSite = null;
                htmlDocumentClass = null;
                currentElement = null;
            }
        }

        /// <summary>
        /// Initializes the control correctly
        /// </summary>
        private void InitMshtml() 
        {
            //don't create in design mode or if the control is already created.
            if (DesignMode || IsCreated) 
            {
                return;
            }

            // force creating Host handle since we need it to parent MSHTML
            if (!IsHandleCreated) 
            {
                IntPtr hostHandle = Handle;
            }

            htmlSite = new HtmlSite(this);
            htmlSite.CreateDocument();

            htmlSite.ElementMoving += new ElementSnapRectEventHandler(OnElementMoving);
            htmlSite.ElementSizing += new ElementSnapRectEventHandler(OnElementSizing);
			
            if (Visible)
            {
                htmlSite.ShowDocument();
            }

            // Save the document
            htmlDocumentClass = (HtmlApi.HTMLDocument) htmlSite.Document;

            if (editMode) 
            {
                HtmlDocument.DesignMode = "On";
            }

            // Load the initial URL
            if (desiredUrl != "") 
            {
                LoadUrl(desiredUrl);
            } 
            else if (desiredContent != "")
            {
                LoadDocument(desiredContent); 
            }
            else
            {
                if (editMode) 
                {
                    LoadDocument(DefaultHtmlContent);
                }
                else
                {
                    LoadUrl("about:blank");
                }
            }

            desiredUrl = "";
            desiredContent = "";

            if (htmlSite != null) 
            {
                htmlSite.ResizeSite();
            }
        }

        /// <summary>
        /// Invoke the ReadyStateChanged event
        /// </summary>
        internal void OnReadyStateChanged() 
        {
            //defensive - I've known this to be called
            //after doc was deactivated
            if (htmlDocumentClass == null) 
            {
                return;
            }

            //Create a proper structure that is more .net-like.
            lastReadyState = ReadyState;

            //if changed to "COMPLETE", set edit designer
            if (lastReadyState == HtmlReadyState.Complete) 
            {
                if (editMode) 
                {
                    //Sets up the Designer
                    HtmlDocument.DesignMode = "On";

                    //Make sure everything is shown correctly.
                    if (htmlSite != null) 
                    {
                        htmlSite.ResizeSite();
                    }

                    Invalidate();

                    IComServiceProvider isp = (IComServiceProvider) htmlDocumentClass;
                    HtmlApi.IHTMLEditServices es;
                    System.Guid IHtmlEditServicesGuid = new System.Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b");
                    System.Guid SHtmlEditServicesGuid = new System.Guid(0x3050f7f9,0x98b5,0x11cf,0xbb,0x82,0x00,0xaa,0x00,0xbd,0xce,0x0b);
                    IntPtr ppv;
                    HtmlApi.IHTMLEditDesigner ds = (HtmlApi.IHTMLEditDesigner)htmlSite;
                    if (isp != null) 
                    {
                        isp.QueryService(ref SHtmlEditServicesGuid,ref IHtmlEditServicesGuid,out ppv);
                        es = (HtmlApi.IHTMLEditServices)Marshal.GetObjectForIUnknown(ppv);
                        int retval = es.AddDesigner(ds); 
                        Marshal.Release(ppv);
                    }

                    SetComposeSettings();

                    ExecCommand(HtmlApi.IDM_SHOWZEROBORDERATDESIGNTIME, showBorderGuides, false, false);

                    // Force a couple of options
                    ExecCommand(HtmlApi.IDM_KEEPSELECTION, true, false, false);
                    ExecCommand(HtmlApi.IDM_2D_POSITION, true, false, false);
                    ExecCommand(HtmlApi.IDM_LIVERESIZE, true, false, false);
                    ExecCommand(HtmlApi.IDM_ATOMICSELECTION, true, false, false);
                    ExecCommand(HtmlApi.IDM_RESPECTVISIBILITY_INDESIGN, true, false, false);

                    UpdateGlyphs();

                    // Change sink
                    /*HtmlChangeSink changeSink = new HtmlChangeSink();
                    changeSink.Install((HtmlApi.IMarkupContainer) HtmlDocument);
                    changeSink.MarkupChanged += new MarkupChangedHandler(OnMarkupChanged);*/
                }

                if (Focused && htmlSite != null) 
                {
                    htmlSite.ActivateDocument();
                }
            }

            if (ReadyStateChanged != null) 
            {			
                ReadyStateChanged(this, new ReadyStateChangedEventArgs(lastReadyState));
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing ) 
        {
            if (disposing) 
            {
                IntPtr ptr = Marshal.GetIDispatchForObject(this);

                int refCount = Marshal.Release(ptr);

                while (refCount > 0)
                {
                    refCount = Marshal.Release(ptr);
                }

                if (htmlSite != null) this.htmlSite.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Events

        /// <summary>
        /// Invokes the UpdateUI event
        /// </summary>
        internal void OnUpdateUI() 
        {
            UpdateCurrentElement();

            if (UpdateUI != null) 
            {
                UpdateUI(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The currently selected element has changed
        /// </summary>
        public void OnCurrentElementChanged()
        {
            if (currentElement != null)
            {
                Debug.WriteLine("CurrentElement: " + currentElement.TagName);
            }

            if (CurrentElementChanged != null)
            {
                CurrentElementChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called back by the sink when markup changes
        /// </summary>
        public void OnMarkupChanged(object sender, HtmlApi.IHTMLElement[] elements)
        {
            // Raise our own event
            //if (MarkupChanged != null)
            {
              //  MarkupChanged(this, elements);
            }
        }
          
        /// <summary>
        /// An element is being moved at design time
        /// </summary>
        private void OnElementMoving(object sender, ElementSnapRectEventArgs e)
        {
            if (ElementMoving != null)
            {
                ElementMoving(this, e);
            }
        }

        /// <summary>
        /// An element is being sized at design time
        /// </summary>
        private void OnElementSizing(object sender, ElementSnapRectEventArgs e)
        {
            if (ElementSizing != null)
            {
                ElementSizing(this, e);
            }
        }

        /// <summary>
        /// Invokes the Navigate event 
        /// </summary>
        public void OnNavigate(string target) 
        {
            if (Navigate != null) Navigate(this, new NavigatedEventArgs(target));
        }

        /// <summary>
        /// Fires the BeforeNavigateEvent
        /// </summary>
        public void OnBeforeNavigate(NavigatingEventArgs e) 
        {
            if (BeforeNavigate != null) BeforeNavigate(this, e);
        }

        #endregion

        #region Designer Properties

        /// <summary>
        /// Gets/Sets the default font that the editor will use.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets/Sets the default font that the editor will use.")]
        [DefaultValue(typeof(System.Drawing.Font), "Arial, 10pt")]
        public new Font DefaultFont 
        {
            get {return defaultFont;}
            set 
            {
                defaultFont = value;

                SetComposeSettings();
            }
        }

        /// <summary>
        /// Get/Sets the default ForeColor that will be used for the editor.
        /// </summary>
        [Category("Appearance")]
        [Description("Get/Sets the default ForeColor that will be used for the editor.")]
        [DefaultValue(typeof(System.Drawing.Color), "Black")]
        public new Color DefaultForeColor 
        {
            get {return defaultForeColor;}
            set 
            {
                defaultForeColor = value;
		
                SetComposeSettings();
            }
        }

        /// <summary>
        /// Get/Sets the default BackColor that will be used for the editor.
        /// </summary>
        [Category("Appearance")]
        [Description("Get/Sets the default BackColor that will be used for the editor.")]
        [DefaultValue(typeof(System.Drawing.Color), "White")]
        public new Color DefaultBackColor 
        {
            get {return defaultBackColor;}
            set 
            {
                defaultBackColor = value;
				
                SetComposeSettings();
            }
        }

        /// <summary>
        /// Gets/Sets if Images will be loaded in the editor.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if Images will be loaded in the editor.")]
        [DefaultValue(true)]
        public bool ShowImages 
        {
            get {return showImages;}
            set 
            {
                if (showImages == value) 
                {
                    return;
                }

                showImages = value;

                desiredContent = this.Html;
                this.Html = DefaultHtmlContent;

                if (IsCreated) 
                {
                    ReloadMshtml();
                }
            }
        }

        /// <summary>
        /// Gets/Sets the style of border that will be displayed around the editor.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets/Sets the style of border that will be displayed around the editor.")]
        [DefaultValue(BorderStyle.Fixed3D)]
        public BorderStyle BorderStyle 
        {
            get {return borderStyle;}
            set 
            {
                if (value == borderStyle) return;
                borderStyle = value;

                Invalidate();

                if (!IsCreated) return;

                htmlSite.ResizeSite();
            }
        }

        /// <summary>
        /// Gets/Sets if the control is in edit mode.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if the control is in edit mode.")]
        [DefaultValue(false)]
        public bool EditMode 
        {
            get {return editMode;}
            set 
            {
                editMode = value;

                if (!IsCreated) 
                {
                    return;
                }

                htmlSite.ResizeSite();

                if (editMode) 
                {
                    HtmlDocument.DesignMode = "On";
                    LoadDocument(DefaultHtmlContent);
                } 
                else 
                {
                    HtmlDocument.DesignMode = "Off";
                }
            }
        }

        /// <summary>
        /// Gets/Sets if the context menu on right clicks will be displayed.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if the context menu on right clicks will be displayed.")]
        [DefaultValue(true)]
        public bool AllowContextMenu 
        {
            get {return allowContextMenu;}
            set {allowContextMenu = value;}
        }

        /// <summary>
        /// Gets/Sets if Active Content (Scripts etc.) are enabled in the control.
        /// This must be set BEFORE the theDocument (i.e. control) is created.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if Active Content (Scripts etc.) are enabled in the control.")]
        [DefaultValue(true)]
        public bool AllowActiveContent 
        {
            get {return allowActiveContent;}
            set {allowActiveContent = value;}
        }

        /// <summary>
        /// Gets/Sets if the control can recieve focus.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if the control can recieve focus.")]
        [DefaultValue(true)]
        public bool AllowActivation
        {
            get { return allowActivation; }
            set { allowActivation = value; }
        }

        /// <summary>
        /// Gets/Sets if the control can recieve focus.
        /// This must be set BEFORE the theDocument (i.e. control) is created.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if the user can select text in the control.")]
        [DefaultValue(true)]
        public bool AllowTextSelection
        {
            get { return allowTextSelection; }
            set { allowTextSelection = value; }
        }

        /// <summary>
        /// Gets/Sets if hyperlinks work
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if the user can select text in the control.")]
        [DefaultValue(true)]
        public bool AllowNavigation
        {
            get { return allowNavigation; }
            set { allowNavigation = value; }
        }

        /// <summary>
        /// Gets/Sets if links that are clicked on in the editor will be opened in the editor or launch your default browser.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets/Sets if links that are clicked on in the editor will be opened in the editor or launch your default browser.")]
        [DefaultValue(false)]
        public bool OpenLinksInNewWindow 
        {
            get {return openLinksNewWindow;}
            set {openLinksNewWindow = value;}
        }

        /// <summary>
        /// The external object that will be accessible to the html document
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null)]
        public object ExternalObject
        {
            get { return externalObject; }
            set { externalObject = value; }
        }

        #endregion

        #region Runtime Properties

        /// <summary>
        /// Gets the Current State of the HtmlDocument.
        /// </summary>
        [Browsable(false)]
        public HtmlReadyState ReadyState 
        {
            get 
            {
                if (!IsCreated || htmlDocumentClass == null) 
                {
                    return HtmlReadyState.UnInitialized;
                }

                switch (HtmlDocument.ReadyState) 
                {
                    case "uninitialized":
                        return HtmlReadyState.UnInitialized;

                    case "loading":
                        return HtmlReadyState.Loading;

                    case "loaded":
                        return HtmlReadyState.Loaded;

                    case "interactive":
                        return HtmlReadyState.Interactive;

                    case "complete":
                        return HtmlReadyState.Complete;

                    default:
                        return HtmlReadyState.UnInitialized;
                }
            }
        }

        /// <summary>
        /// Returns the default HTML content that will be used when no content is set
        /// </summary>
        [Browsable(false)]
        public string DefaultHtmlContent
        {
            get
            {
                return "<html><head></head><body></body></html>";
            }
        }

        /// <summary>
        /// The IHTMLDocument3 instance
        /// </summary>
        [Browsable(false)]
        public HtmlApi.IHTMLDocument3 HtmlDocument 
        {
            [DebuggerStepThrough]
            get 
            {
                if (!IsCreated) 
                {
                    return null;
                } 
                else 
                {
                    return (HtmlApi.IHTMLDocument3) htmlSite.Document;
                }
            }
        }

        /// <summary>
        /// Returns the current HTML element
        /// </summary>
        public HtmlApi.IHTMLElement CurrentElement
        {
            [DebuggerStepThrough]
            get
            {
                return currentElement;
            }
        }

        /// <summary>
        /// Get the Undo\Redo manager for the control
        /// </summary>
        [Browsable(false)]
        public UndoManager UndoManager
        {
            get
            {
                return undoManager;
            }
        }

        /// <summary>
        /// Get the table editor for the control
        /// </summary>
        [Browsable(false)]
        public TableEditor TableEditor
        {
            get
            {
                return tableEditor;
            }
        }

        /// <summary>
        /// Gets/Sets the Selection's Bold State.
        /// </summary>
        [Browsable(false)]
        public bool SelectionBold 
        {
            get 
            {
                if (!IsCreated) 
                {
                    return false;
                }

                try
                {
                    return Convert.ToBoolean(HtmlDocument.QueryCommandValue("bold"));
                }
                catch 
                {
                    return false;
                }
            }
            set 
            {
                if (!IsCreated) 
                {
                    return;
                }
 
                if (value != SelectionBold)
                {
                    HtmlDocument.ExecCommand("bold", false, null);
                }
            }
        }

        /// <summary>
        /// Gets/Sets the Selection's Italic State.
        /// </summary>
        [Browsable(false)]
        public bool SelectionItalic 
        {
            get 
            {
                if (!IsCreated) 
                {
                    return false;
                }

                try
                {
                    return Convert.ToBoolean(HtmlDocument.QueryCommandValue("italic"));
                }
                catch 
                {
                    return false;
                }
            }
            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                if (value != SelectionItalic)
                {
                    HtmlDocument.ExecCommand("italic", false, null);
                }
            }
        }

        /// <summary>
        /// Gets/Sets the Selection's Italic State.
        /// </summary>
        [Browsable(false)]
        public bool SelectionUnderline 
        {
            get 
            {
                if (!IsCreated) 
                {
                    return false;
                }

                try
                {
                    return Convert.ToBoolean(HtmlDocument.QueryCommandValue("underline"));
                }
                catch 
                {
                    return false;
                }
            }
            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                if (value != SelectionUnderline)
                {
                    HtmlDocument.ExecCommand("underline", false, null);
                }
            }
        }

        /// <summary>
        /// Gets/Sets the current selection's back color.
        /// </summary>
        [Browsable(false)]
        public Color SelectionBackColor 
        {
            get 
            { 
                if (!IsCreated) 
                {
                    return Color.Empty;
                } 

                object result = HtmlDocument.QueryCommandValue("BackColor");

                try 
                {
                    return ColorTranslator.FromHtml(result.ToString());
                } 
                catch 
                {
                    return Color.Empty;
                }
            } 
            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                //need to send a COLORREF value
                ExecCommand(HtmlApi.IDM_BACKCOLOR, GetCsColor(value), false, true);

                // Have seen where these changes dont get a change on their own
                this.OnMarkupChanged(this, new HtmlApi.IHTMLElement[]{this.CurrentElement});
            }
        }

        /// <summary>
        /// Gets/Sets the color for the selected text.
        /// </summary>
        [Browsable(false)]
        public Color SelectionForeColor	
        {
            get 
            {
                if (!IsCreated) 
                {
                    return Color.Empty;
                }

                object result = HtmlDocument.QueryCommandValue("ForeColor");

                try 
                {
                    return ColorTranslator.FromHtml(result.ToString());
                } 
                catch 
                {
                    return Color.Empty;
                }
            }
            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                // need to send a COLORREF value
                ExecCommand(HtmlApi.IDM_FORECOLOR, GetCsColor(value),false,true);

                // Have seen where these changes dont get a change on their own
                this.OnMarkupChanged(this, new HtmlApi.IHTMLElement[]{this.CurrentElement});
            }
        }

        /// <summary>
        /// Gets/sets the font for the selected text
        /// </summary>
        [Browsable(false)]
        public Font SelectionFont 
        {
            get 
            {
                if (!IsCreated) 
                {
                    return null;
                }

                //This is here because of hosting issues on a UserControl 
                if (htmlDocumentClass == null) 
                {
                    return null;
                }

                System.Drawing.FontStyle fontStyle = new FontStyle();

                // Basic properties
                if (SelectionBold) fontStyle |= FontStyle.Bold;
                if (SelectionItalic) fontStyle |= FontStyle.Italic;
                if (SelectionUnderline) fontStyle |= FontStyle.Underline;

                // Size
                int fontSize = 8;

                switch(Convert.ToInt32(HtmlDocument.QueryCommandValue("FontSize"))) 
                {
                    case 1: fontSize = 8; break;
                    case 2: fontSize = 10; break;
                    case 3: fontSize = 12; break;
                    case 4: fontSize = 18; break;
                    case 5: fontSize = 24; break;
                    case 6: fontSize = 36; break;
                    case 7: fontSize = 48; break;
                }

                // Name
                string fontName = HtmlDocument.QueryCommandValue("FontName").ToString();

                // Create the font
                return new Font(fontName, fontSize, fontStyle);
            }

            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                //This is here because of hosting issues on a UserControl 
                if (htmlDocumentClass == null) 
                {
                    return;
                }

                if (value == null)
                {
                    return;
                }

                // Set the name
                HtmlDocument.ExecCommand("FontName", false, value.Name);

                // Set the size
                HtmlDocument.ExecCommand("FontSize", false, GetFontSizeFromEm(value.SizeInPoints));

                // Properties
                SelectionBold = Font.Bold;
                SelectionItalic = Font.Italic;
                SelectionUnderline = Font.Underline;

                // Have seen where these changes dont get a change on their own
                this.OnMarkupChanged(this, new HtmlApi.IHTMLElement[]{this.CurrentElement});
            }
        }

        /// <summary>
        /// Gets/sets the font for the selected text
        /// </summary>
        [Browsable(false)]
        public string SelectionFontName
        {
            get 
            {
                string fontName = "Times New Roman";

                if (!IsCreated)
                {
                    return fontName;
                }

                //This is here because of hosting issues on a UserControl 
                if (htmlDocumentClass == null) 
                {
                    return fontName;
                }

                try
                {
                    object queryResult = HtmlDocument.QueryCommandValue("FontName");

                    if (queryResult == null)
                    {
                        return fontName;
                    }

                    return queryResult.ToString();
                }
                catch
                {
                    return fontName;
                }
            }

            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                //This is here because of hosting issues on a UserControl 
                if (htmlDocumentClass == null) 
                {
                    return;
                }

                try 
                { 
                    // Set the name
                    HtmlDocument.ExecCommand("FontName", false, value);

                    // Have seen where these changes dont get a change on their own
                    this.OnMarkupChanged(this, new HtmlApi.IHTMLElement[]{this.CurrentElement});
                } 
                catch {}
            }
        }

        /// <summary>
        /// Gets/sets the font for the selected text
        /// </summary>
        [Browsable(false)]
        public int SelectionFontSize 
        {
            get 
            {
                int defaultSize = 2;

                if (!IsCreated) 
                {
                    return defaultSize;
                }

                //This is here because of hosting issues on a UserControl 
                if (htmlDocumentClass == null) 
                {
                    return defaultSize;
                }

                try 
                {
                    object queryValue = HtmlDocument.QueryCommandValue("FontSize");

                    if (queryValue is DBNull)
                    {
                        return defaultSize;
                    }

                    return Convert.ToInt32(queryValue);
                } 
                catch 
                {
                    return 2;
                }
            }

            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                //This is here because of hosting issues on a UserControl 
                if (htmlDocumentClass == null) 
                {
                    return;
                }

                try 
                {
                    // Set the size
                    HtmlDocument.ExecCommand("FontSize", false, value);

                    // Have seen where these changes dont get a change on their own
                    this.OnMarkupChanged(this, new HtmlApi.IHTMLElement[]{this.CurrentElement});
                } 
                catch {}
            }
        }

        /// <summary>
        /// Gets/Sets the alignment of the selected text.
        /// </summary>
        [Browsable(false)]
        public HorizontalAlignment SelectionAlignment 
        {
            get 
            {
                if (!IsCreated) 
                { 
                    return HorizontalAlignment.Left;
                }
 
                if (Convert.ToBoolean(HtmlDocument.QueryCommandValue("JustifyRight"))) 
                {
                    return HorizontalAlignment.Right;
                } 
                else if (Convert.ToBoolean(HtmlDocument.QueryCommandValue("JustifyCenter"))) 
                {
                    return HorizontalAlignment.Center;
                } 
                else 
                {
                    return HorizontalAlignment.Left;
                }
            }
            set 
            {
                if (!IsCreated) 
                {
                        return;
                }

                switch (value) 
                {
                    case HorizontalAlignment.Left:
                        HtmlDocument.ExecCommand("JustifyLeft", false, null);
                        break;
                    case HorizontalAlignment.Center:
                        HtmlDocument.ExecCommand("JustifyCenter", false, null);
                        break;
                    case HorizontalAlignment.Right:
                        HtmlDocument.ExecCommand("JustifyRight", false, null);
                        break;
                }
            }
        }

        /// <summary>
        /// Get/Sets if numbering is on for the selected text.
        /// </summary>
        [Browsable(false)]
        public bool SelectionNumbering 
        {
            get 
            {
                if (!IsCreated) 
                {
                    return false;
                }

                return Convert.ToBoolean(HtmlDocument.QueryCommandValue("InsertOrderedList"));
            } 
            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                if (SelectionNumbering != value)
                {
                    HtmlDocument.ExecCommand("InsertOrderedList", false, null);
                }
            }
        }

        /// <summary>
        /// Gets/Sets if bullets are on or off for the selected text.
        /// </summary>
        [Browsable(false)]
        public bool SelectionBullets 
        {
            get 
            {
                if (!IsCreated) 
                {
                    return false;
                }

                return Convert.ToBoolean(HtmlDocument.QueryCommandValue("InsertUnorderedList"));
            }
            set 
            {
                if (!IsCreated) 
                {
                    return;
                }

                if (SelectionBullets != value) 
                {
                    HtmlDocument.ExecCommand("InsertUnorderedList", false, null);
                }
            }
        }

        /// <summary>
        /// Dermines if setting the font name is available
        /// </summary>
        [Browsable(false)]
        public bool CanSetFontName
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_FONTNAME);
            }
        }

        /// <summary>
        /// Determines if setting the font size is available
        /// </summary>
        [Browsable(false)]
        public bool CanSetFontSize
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_FONTSIZE);
            }
        }

        /// <summary>
        /// Determines if setting the fore color is available
        /// </summary>
        [Browsable(false)]
        public bool CanSetForeColor
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_FORECOLOR);
            }
        }

        /// <summary>
        /// Determiens if settings the back color is available
        /// </summary>
        [Browsable(false)]
        public bool CanSetBackColor
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_BACKCOLOR);
            }
        }

        /// <summary>
        /// Determines if cut is available
        /// </summary>
        [Browsable(false)]
        public bool CanCut
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_CUT);
            }
        }

        /// <summary>
        /// Determines if copy is available
        /// </summary>
        [Browsable(false)]
        public bool CanCopy
        {
            get
            {
                if (EditMode)
                {
                    // For some reason IDM_COPY would report available
                    // while the mouse was down, causing the button to flicker
                    // as mouse clicks occurred.  Cut seems reliable.
                    return CanCut;
                }
                else
                {
                    return IsCommandEnabled(HtmlApi.IDM_COPY);
                }
            }
        }

        /// <summary>
        /// Determines if paste is available
        /// </summary>
        [Browsable(false)]
        public bool CanPaste
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_PASTE);
            }
        }

        /// <summary>
        /// Determines if bold is available
        /// </summary>
        [Browsable(false)]
        public bool CanBold
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_BOLD);
            }
        }

        /// <summary>
        /// Determines if italic is available
        /// </summary>
        [Browsable(false)]
        public bool CanItalic
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_ITALIC);
            }
        }

        /// <summary>
        /// Determines if underline is available
        /// </summary>
        [Browsable(false)]
        public bool CanUnderline
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_UNDERLINE);
            }
        }

        /// <summary>
        /// Determines if numbering is available
        /// </summary>
        [Browsable(false)]
        public bool CanNumbering
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_ORDERLIST);
            }
        }

        /// <summary>
        /// Determines if bullets is available
        /// </summary>
        [Browsable(false)]
        public bool CanBullets
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_UNORDERLIST);
            }
        }

        /// <summary>
        /// Determines if indent is available
        /// </summary>
        [Browsable(false)]
        public bool CanIndent
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_INDENT);
            }
        }

        /// <summary>
        /// Determines if unindent is available
        /// </summary>
        [Browsable(false)]
        public bool CanUnindent
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_OUTDENT);
            }
        }

        /// <summary>
        /// Determins if the specified alignment is supported
        /// </summary>
        [Browsable(false)]
        public bool CanAlign(HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left: return IsCommandEnabled(HtmlApi.IDM_JUSTIFYLEFT);
                case HorizontalAlignment.Center: return IsCommandEnabled(HtmlApi.IDM_JUSTIFYCENTER);
                case HorizontalAlignment.Right: return IsCommandEnabled(HtmlApi.IDM_JUSTIFYRIGHT);
            }

            return false;
        }

        /// <summary>
        /// Determines if inserting an image is available
        /// </summary>
        [Browsable(false)]
        public bool CanInsertImage
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_INSINPUTIMAGE);
            }
        }

        /// <summary>
        /// Determines if a hyperlink can be inserted
        /// </summary>
        [Browsable(false)]
        public bool CanInsertHyperlink
        {
            get
            {
                // IDM_HYPERLINK didnt seem to behave very well.
                return CanInsertHr;
            }
        }

        /// <summary>
        /// Determines if a horizontal line can be inserted
        /// </summary>
        [Browsable(false)]
        public bool CanInsertHr
        {
            get
            {
                return IsCommandEnabled(HtmlApi.IDM_HORIZONTALLINE);
            }
        }

        /// <summary>
        /// Deterimes if border guides are displayed at design time
        /// </summary>
        [Browsable(false)]
        public bool ShowBorderGuides
        {
            get
            {
                return showBorderGuides;
            }
            set
            {
                showBorderGuides = value;

                if (!IsCreated) 
                {
                    return;
                }

                if (htmlDocumentClass == null)
                {
                    return;
                }
                
                ExecCommand(HtmlApi.IDM_SHOWZEROBORDERATDESIGNTIME, showBorderGuides, false, false);
            }
        }

        /// <summary>
        /// Determins if glyphs are visible or not
        /// </summary>
        [Browsable(false)]
        public bool ShowGlyphs
        {
            get
            {
                return showGlyphs;
            }
            set
            {
                showGlyphs = value;

                if (ReadyState != HtmlReadyState.Complete)
                {
                    return;
                }
                
                UpdateGlyphs();
            }
        }

        /// <summary>
        /// Gets/Sets the HTML that will be displayed.
        /// </summary>
        [Browsable(false)]
        [DebuggerHidden]
        public string Html 
        {
            [DebuggerStepThrough]
            get 
            {
                if (IsCreated)
                {
                    return ((HtmlApi.IHTMLDocument3) HtmlDocument).DocumentElement.OuterHTML;
                }
                else
                {
                    return "";
                }
            }
            [DebuggerStepThrough]
            set 
            {
                try 
                {
                    if (value == null || value == "") 
                    {
                        LoadDocument(DefaultHtmlContent);
                    } 
                    else
                    {
                        LoadDocument(value);
                    }

                } 
                catch {}
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determine the current ideal height for the renerered bitmap based on HTML content and the width of the control
        /// </summary>
        public int DetermineIdealRenderedBitmapHeight()
        {
            HtmlApi.IHTMLElement bodyElement = HtmlDocument.Body;
            HtmlApi.IHTMLStyle style = bodyElement.Style;

            HtmlApi.IHTMLElement contentElement = bodyElement;
            HtmlApi.IHTMLElement zoomElement = HtmlDocument.getElementById("zoomElement") as HtmlApi.IHTMLElement;
            if (zoomElement != null)
            {
                contentElement = zoomElement;
            }
            else
            {
                // I'm not sure why, but without that zoom element it doesnt accurately get the height.  I think it may be due to the
                // way the style.SetOverflow("hidden") line works a little further down.
                throw new InvalidOperationException("Cannot accurately determine height without the zoom element.");
            }

            // No content
            if (string.IsNullOrEmpty(contentElement.InnerText))
            {
                // If there are no children, then its definitely no content.
                if (contentElement.Children.Length == 0)
                {
                    return 0;
                }

                // If all there is is a <pre> tag, then its likely our text-output template, and we since we already know innerItext is emtpy,
                // we know it is empty.
                if (contentElement.Children.Length == 1)
                {
                    HtmlApi.IHTMLElement child = contentElement.Children.Item(0) as HtmlApi.IHTMLElement;
                    if (child != null && string.Compare(child.TagName, "pre", true) == 0)
                    {
                        return 0;
                    }
                }
            }

            // This is required fo thet GetScrollXXX to work
            style.SetOverflow("hidden");

            // Get the full height that the html needs to be
            return ((HtmlApi.IHTMLElement2) bodyElement).GetScrollHeight();
        }

        /// <summary>
        /// Draws the contents of the control and returns a bitmap.  If the rendering is not big enough to take up the whole area
        /// the backColor is used as the background fill color.
        /// </summary>
        public Bitmap RenderToBitmap(Rectangle bounds, Color backColor)
        {
            IViewObject viewObject = (IViewObject) HtmlDocument;

            // Creat a bitmap of the correct size
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            HtmlApi.IHTMLBodyElement body = (HtmlApi.IHTMLBodyElement) HtmlDocument.Body;
            body.SetScroll("no");

            // Create a graphics object to draw onto the bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, new Rectangle(new Point(0, 0), bounds.Size));
                }

                // Dont need it, but needs to be passed in
                NativeMethods.RECT unusedRect = new NativeMethods.RECT();

                // Get the hdc to draw to
                IntPtr hdc = g.GetHdc();

                int offsetWidth = HtmlDocument.Body.OffsetWidth;
                int offsetHeight = HtmlDocument.Body.OffsetHeight;

                // The target bounds we will request drawing to.  We have to draw to the actual size of the html body
                // otherwise it will scale it to whatever size we specify.  It will just get clipped to the bounds of the requested bitmap.
                NativeMethods.RECT targetRect = new NativeMethods.RECT(0, 0, offsetWidth, offsetHeight);

                viewObject.Draw(1, 1, IntPtr.Zero, IntPtr.Zero,
                                       IntPtr.Zero, hdc, targetRect,
                                       ref unusedRect, IntPtr.Zero, 0);

                // Release our hdc
                g.ReleaseHdc();
            }

            return bitmap;
        }
        
        /// <summary>
        /// Update the glyph display
        /// </summary>
        private void UpdateGlyphs()
        {
            if (ReadyState != HtmlReadyState.Complete)
            {
                return;
            }

            ExecCommand(HtmlApi.IDM_EMPTYGLYPHTABLE, null, false, false);

            if (showGlyphs)
            {
                ExecCommand(HtmlApi.IDM_ADDTOGLYPHTABLE, GlyphManager.GlyphTable, false, false);
            }
        }

        /// <summary>
        /// Converts the HTML into plain text.
        /// </summary>
        public void SelectionRemoveFormat() 
        {
            try 
            {
                HtmlDocument.ExecCommand("RemoveFormat", false, null);
            } 
            catch {}
        }
	
        /// <summary>
        /// Inserts an image at the selection point.
        /// </summary>
        public bool InsertImage() 
        {
            try 
            {
                return HtmlDocument.ExecCommand("InsertImage", true, null);
            } 
            catch 
            {
                MessageBox.Show(this, "The image file was invalid.");
                return false;
            }
        }

        /// <summary>
        /// Inserts a hyperlink at the selection point.
        /// </summary>
        public bool InsertHyperlink() 
        {
            try 
            {
                return HtmlDocument.ExecCommand("CreateLink", true, null);
            } 
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// Inserts a HR at the selection point.
        /// </summary>
        public bool InsertHr() 
        {
            try 
            {
                return HtmlDocument.ExecCommand("InsertHorizontalRule", true, null);
            } 
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// Inserts the specified image at the selection point.
        /// </summary>
        public bool InsertImage(string fileName) 
        {
            try 
            {
                return HtmlDocument.ExecCommand("InsertImage", true, fileName);
            } 
            catch 
            {
                MessageBox.Show(this, "The image file was invalid.");
                return false;
            }
        }

        /// <summary>
        /// Select the entire theDocument
        /// </summary>
        public bool SelectAll() 
        {
            if (!IsCreated) 
            {
                return false;
            }

            return ExecCommand(HtmlApi.IDM_SELECTALL,null,false,true);
        }

        /// <summary>
        /// Loads the string passed as the theDocument to be displayed.
        /// </summary>
        public void LoadDocument(string htmlContents) 
        {
            if (!IsCreated)
            {
                desiredUrl = "";
                desiredContent = htmlContents;

                return;
            }

            InternalLoadUrl("about:blank");
           // WaitForDocumentComplete();

            HtmlApi.IHTMLDocument2 document2 = HtmlDocument;

            document2.Open("", "replace", null, null);
            document2.Write(htmlContents);
            document2.Close();
        }

        /// <summary>
        /// Loads the URL passed into the HTML Control
        /// </summary>
        public void LoadUrl(string url) 
        {			
            if (!IsCreated) 
            {
                desiredUrl = url;
                desiredContent = "";

                return;
            }

            //this is a workaround for a problem calling Caret.SetLocation before it
            //is ready, in UpdateUI
            htmlSite.FullyActive = false;

            InternalLoadUrl(url);

            if (this.FindForm() != null) 
            {
                //Get the focus back to where it belongs.
                Control activeControl = FindActiveControl(this.FindForm());
                if (activeControl != null) 
                {
                    activeControl.Focus();
                }
            }
        }

        /// <summary>
        /// Copies the currently selected elements.
        /// </summary>
        public bool Copy() 
        {
            if (!IsCreated) 
            {
                return false;
            }

            return ExecCommand(HtmlApi.IDM_COPY,null,false,true);
        }

        /// <summary>
        /// Pastes the contents of the clipboard at the current selection.
        /// </summary>
        public bool Paste() 
        {
            if (!IsCreated)
            {
                return false;
            }
            
            return ExecCommand(HtmlApi.IDM_PASTE,null,false,true);
        }

        /// <summary>
        /// Cuts the selection and places it in the clipboard.
        /// </summary>
        /// <returns>True if successfull, false otherwise.</returns>
        public bool Cut() 
        {
            if (!IsCreated) 
            {
                return false;
            }

            return ExecCommand(HtmlApi.IDM_CUT,null,false,true);
        }

        /// <summary>
        /// Delete everything in the current selection
        /// </summary>
        public bool Delete()
        {
            if (!IsCreated)
            {
                return false;
            }

            return ExecCommand(HtmlApi.IDM_DELETE, null, false, true);
        }

        /// <summary>
        /// Saves the current document prompting the user.
        /// </summary>
        public bool SaveAs(string defaultPath) 
        {
            return SaveAs(defaultPath, true);
        }

        /// <summary>
        /// Saves the current theDocument by prompting the user.
        /// </summary>
        /// <returns>True if successfull, false otherwise.</returns>
        public bool SaveAs() 
        {
            return SaveAs(null);
        }

        /// <summary>
        /// Saves the current theDocument using the DefaultPath as the file name or prompting the user with the default filename.
        /// </summary>
        public bool SaveAs(string defaultPath, bool showDialog) 
        {
            if (!IsCreated) 
            {
                return false;
            }

            return ExecCommand(HtmlApi.IDM_SAVEAS, defaultPath, showDialog, true);
        }

        /// <summary>
        /// Clears the formatting of the current selection.
        /// </summary>
        public bool ClearSelectionFormatting() 
        {
            if (!IsCreated) 
            {
                return false;
            }

            return ExecCommand(HtmlApi.IDM_REMOVEFORMAT,null,false,true);
        }

        /// <summary>
        /// Creates a table with the parameters specified.
        /// </summary>
        public void InsertTable() 
        {
            if (!IsCreated) 
            {
                return;
            }

            InsertTableDialog dlg = new InsertTableDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                ReplaceSelection(dlg.TableHtml);
            }
        }

        /// <summary>
        /// Indents the selection
        /// </summary>
        public bool Indent() 
        {
            if (!IsCreated) return false;

            return HtmlDocument.ExecCommand("Indent", false, null);
        }

        /// <summary>
        /// Outdents the selection
        /// </summary>
        public bool Unindent() 
        {
            if (!IsCreated) return false;

            return HtmlDocument.ExecCommand("Outdent", false, null);
        }

        /// <summary>
        /// Replaces the current selection with the given HTML 
        /// </summary>
        public void ReplaceSelection(string html)
        {
            object range = HtmlDocument.Selection.CreateRange();

            HtmlApi.IHTMLTxtRange textRange = range as HtmlApi.IHTMLTxtRange;

            if (textRange != null)
            {
                textRange.PasteHTML(html);
            }
            else
            {
                HtmlApi.IHTMLControlRange controlRange = range as HtmlApi.IHTMLControlRange;

                if (controlRange != null)
                {
                    HtmlApi.IMarkupServices ms = (HtmlApi.IMarkupServices) htmlDocumentClass;

                    HtmlApi.IMarkupPointer pointer;
                    ms.CreateMarkupPointer(out pointer);
                    pointer.MoveAdjacentToElement(controlRange.Item(0), HtmlApi.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);

                    HtmlApi.IHTMLElement current;
                    pointer.CurrentScope(out current);

                    // Delete the selection (the control range)
                    HtmlDocument.ExecCommand("Delete", false, null);

                    current.InsertAdjacentHTML("afterBegin", html);
                }
            }
        }

        /// <summary>
        /// Determine if the given command is available
        /// </summary>
        public bool IsCommandEnabled(int command) 
        {
            if (ReadyState != HtmlReadyState.Complete)
            {
                return false;
            }

            HtmlCommandStatus cs = this.GetCommandInfo(command);

            return ((cs == HtmlCommandStatus.Enabled) | (cs == HtmlCommandStatus.EnabledAndToggledOn));
        }

        /// <summary>
        /// Determines if the given command is checked
        /// </summary>
        public bool IsCommandChecked(int command)
        {
            if (ReadyState != HtmlReadyState.Complete)
            {
                return false;
            }

            HtmlCommandStatus cs = this.GetCommandInfo(command);

            return cs == HtmlCommandStatus.EnabledAndToggledOn;
        }

        /// <summary>
        /// Prints the current document
        /// </summary>
        public void Print() 
        {
            Print(string.Empty);
        }

        /// <summary>
        /// Previews the current document
        /// </summary>
        public void Preview()
        {
            Preview(string.Empty);
        }

        /// <summary>
        /// Prints the current theDocument using a template.
        /// </summary>
        public void Print(string printTemplate) 
        {
            if (string.IsNullOrEmpty(printTemplate))
            {
                printTemplate = null;
            }

            ExecCommand(HtmlApi.IDM_PRINT, printTemplate, false, true);
        }

        /// <summary>
        /// Previews the current theDocument using a template.
        /// </summary>
        public void Preview(string printTemplate)
        {
            if (string.IsNullOrEmpty(printTemplate))
            {
                printTemplate = null;
            }

            ExecCommand(HtmlApi.IDM_PRINTPREVIEW, printTemplate, false, true);
        }

        #endregion

        #region Zoom

        /// <summary>
        /// The opening Div tag that is the zoom element.
        /// </summary>
        public static string ZoomDivStartTag
        {
            get { return "<div id='zoomElement' style='zoom:100%; padding:0; margin:0; '>"; }
        }

        /// <summary>
        /// Get the current amount of zoom
        /// </summary>
        public double GetZoomPercent()
        {
            HtmlApi.IHTMLElement zoomElement = HtmlDocument.getElementById("zoomElement") as HtmlApi.IHTMLElement;
            if (zoomElement == null)
            {
                return 100;
            }

            // Reset the zoom level of this element
            HtmlApi.IHTMLStyle3 style = GetZoomStyle();

            return Convert.ToDouble(((string) style.GetZoom()).Replace("%", ""));
        }

        /// <summary>
        /// Zoom the html to be scaled based on its best fit
        /// </summary>
        public void ZoomToScale(double scale)
        {
            double zoom = ZoomToFit();

            GetZoomStyle().SetZoom(string.Format("{0}%", zoom * (scale / 100.0)));
        }

        /// <summary>
        /// Zoom the html to the exact zoom level
        /// </summary>
        public void ZoomToExact(double zoom)
        {
            GetZoomStyle().SetZoom(string.Format("{0}%", zoom));
        }

        /// <summary>
        /// Zoom the html to fit perfectly in the window
        /// </summary>
        public double ZoomToFit()
        {
            // Reset the zoom level of this element
            HtmlApi.IHTMLStyle3 style = GetZoomStyle();
            style.SetZoom("100%");

            HtmlApi.IHTMLElement2 body = (HtmlApi.IHTMLElement2) HtmlDocument.Body;
            double yRatio = (double) body.GetClientHeight() / (double) body.GetScrollHeight();
            double xRatio = (double) body.GetClientWidth() / (double) body.GetScrollWidth();

            double zoom;

            // No need to zoom
            if (xRatio >= 1 && yRatio >= 1)
            {
                zoom = 100;
            }
            else
            {
                zoom = (int) Math.Max((Math.Min(xRatio, yRatio) * 100) - 2, minimumAutoZoom);
            }

            // Update the zoom level of this element
            style.SetZoom(string.Format("{0}%", zoom));

            return zoom;
        }

        /// <summary>
        /// Zoom the html to fit in the window horizontally
        /// </summary>
        public double ZoomToWidth()
        {
            // Reset the zoom level of this element
            HtmlApi.IHTMLStyle3 style = GetZoomStyle();
            style.SetZoom("100%");

            HtmlApi.IHTMLElement2 body = (HtmlApi.IHTMLElement2) HtmlDocument.Body;
            double xRatio = (double) body.GetClientWidth() / (double) body.GetScrollWidth();

            double zoom;

            // No need to zoom
            if (xRatio >= 1)
            {
                zoom = 100;
            }
            else
            {
                zoom = (int) Math.Max(Math.Floor(xRatio * 100) - 2, minimumAutoZoom);
            }

            // Update the zoom level of this element
            style.SetZoom(string.Format("{0}%", zoom));

            return zoom;
        }

        /// <summary>
        /// Get the element we use to control zoom
        /// </summary>
        private HtmlApi.IHTMLStyle3 GetZoomStyle()
        {
            HtmlApi.IHTMLElement zoomElement = HtmlDocument.getElementById("zoomElement") as HtmlApi.IHTMLElement;

            // We couldnt find our <div> with the zoom on it, so we will insert it
            if (zoomElement == null)
            {
                HtmlDocument.Body.InnerHTML =
                    ZoomDivStartTag +
                    HtmlDocument.Body.InnerHTML +
                    "</div>";

                // Get the element we just created
                zoomElement = HtmlDocument.getElementById("zoomElement") as HtmlApi.IHTMLElement;
            }

            return (HtmlApi.IHTMLStyle3) zoomElement.Style;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// A key has been pressed, check for shortcuts
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e) 
        {
            base.OnKeyDown (e);

            if (e.Control)
            {
                DoShortCut(e.KeyCode);
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        //Delete();
                        break;
                }
            }
        }

        /// <summary>
        /// Intercept messages
        /// </summary>
        protected override void WndProc(ref Message m) 
        {
            // This is here if you want your arrow keys to work right.
            if (m.Msg == NativeMethods.WM_GETDLGCODE) 
            {
                m.Result = (IntPtr) NativeMethods.DLGC_WANTALLKEYS;
                return;
            } 

            if (m.Msg == NativeMethods.WM_MOUSEACTIVATE) 
            {
                Focus();
            }

            // Call the base
            base.WndProc(ref m);

            if (htmlSite != null) 
            {
                // Only fire these on SetFocus because it's done automatically on mouse clicking in it.
                if (m.Msg == NativeMethods.WM_SETFOCUS) 
                { 
                    htmlSite.DeactivateDocument();
                    htmlSite.ActivateDocument();
                } 
                else if (m.Msg == NativeMethods.WM_KILLFOCUS) 
                {
                    htmlSite.DeactivateDocument();
                }
            }
        }

        /// <summary>
        /// When the handle of this control is created, initialize the mshtml
        /// </summary>
        protected override void OnHandleCreated(EventArgs e) 
        {
            if (constructed) 
            {
                InitMshtml();			
            }
        }

        /// <summary>
        /// Hide and show the htmlSite as visibility changes
        /// </summary>
        protected override void OnVisibleChanged(EventArgs e) 
        {
            base.OnVisibleChanged (e);

            if (IsCreated) 
            {
                if (this.Visible) 
                {
                    htmlSite.ShowDocument();
                } 
                else 
                {
                    htmlSite.HideDocument();
                }
            }
        }

        /// <summary>
        /// When the handle of the control is destroyed, shutdown mshtml
        /// </summary>
        protected override void OnHandleDestroyed(EventArgs e) 
        {
            CleanupControl();

            base.OnHandleDestroyed(e);
        }

        /// <summary>
        /// Listen for parent changes
        /// </summary>
        protected override void OnParentChanged(System.EventArgs e) 
        {
            if (htmlSite == null) 
            {
                InitMshtml();
            }

            base.OnParentChanged(e);
        }

        /// <summary>
        /// Ensure we are redrawn when we are resized.
        /// </summary>
        protected override void OnResize(EventArgs e) 
        {
            if (htmlSite != null)
            {
                htmlSite.ResizeSite();
            }

            base.OnResize(e);
        }

        /// <summary>
        /// Draw the border around the control
        /// </summary>
        protected override void OnPaint(PaintEventArgs pevent) 
        {
            Rectangle rect = Rectangle.Empty;
			
			if (ClientRectangle.Width -1 > 0) 
            {
                rect = new Rectangle(ClientRectangle.X, ClientRectangle.Top, ClientRectangle.Width -1, ClientRectangle.Height);				
            } 
            else 
            {
                rect = ClientRectangle;
            }

            if (borderStyle == BorderStyle.Fixed3D) 
            {
                ControlPaint.DrawBorder3D(pevent.Graphics, rect, Border3DStyle.Sunken, Border3DSide.All);
            } 
            else if (borderStyle == BorderStyle.FixedSingle) 
            {
                ControlPaint.DrawBorder(pevent.Graphics, rect, Color.Black, ButtonBorderStyle.Solid);
            }

            if (DesignMode) 
            {
                if (borderStyle != BorderStyle.None && rect.X + 2 <= rect.Right && rect.Y + 2 < rect.Bottom) 
                    rect.Inflate(-2, -2);

                //Draw over the control.
                pevent.Graphics.FillRectangle(Brushes.White, rect); 
            }

            base.OnPaint(pevent);
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Load the specified URL into the browser window
        /// </summary>
        private void InternalLoadUrl(string url)
        {
            IPersistMoniker persistMoniker = (IPersistMoniker) HtmlDocument;

            IMoniker moniker;
            OleApi.CreateURLMoniker(null, url, out moniker);
	
            IBindCtx bindContext;
            OleApi.CreateBindCtx(0, out bindContext);
	
            persistMoniker.Load(0, moniker, bindContext, OleApi.STGM_READ);
			
            if (bindContext != null)
                Marshal.ReleaseComObject(bindContext);

            if (moniker != null)
                Marshal.ReleaseComObject(moniker);
        }

        /// <summary>
        /// Wait for the document to be in the ready state, if required
        /// </summary>
        public void WaitForComplete(TimeSpan timeout)
        {
            if (Program.ExecutionMode.IsUIDisplayed && !InvokeRequired)
            {
                throw new InvalidOperationException("This function can only work if it is called from a different thread than the UI thread.");
            }

            int totalMsWaited = 0;
            int loopMsWaitTime = 5;

            // We can only do this without Application.DoEvents (when running ShipWorks with a UI) because we know we are not on the UI thread. 
            while (ReadyState != HtmlReadyState.Complete && totalMsWaited < timeout.TotalMilliseconds)
            {
                if (!Program.ExecutionMode.IsUIDisplayed)
                {
                    // We need to trigger the message pump to run if ShipWorks is running as a Windows service
                    // otherwise the ReadyState never gets set to Complete
                    Application.DoEvents();
                }
                
                Thread.Sleep(loopMsWaitTime);
                totalMsWaited += loopMsWaitTime;
            }
        }

        /// <summary>
        /// Update what we think is the current element of hte document.  Returns true
        /// if the current element changed.
        /// </summary>
        public bool UpdateCurrentElement()
        {
            if (!IsCreated)
            {
                return false;
            }

            HtmlApi.IHTMLElement oldElement = currentElement;

            currentElement = null;

            object range = HtmlDocument.Selection.CreateRange();

            HtmlApi.IHTMLTxtRange textRange = range as HtmlApi.IHTMLTxtRange;

            // If the current selection is a text range, get the object at the caret
            if (textRange != null)
            {
                HtmlApi.IDisplayServices ds = (HtmlApi.IDisplayServices) htmlDocumentClass;
                HtmlApi.IMarkupServices ms = (HtmlApi.IMarkupServices) htmlDocumentClass;

                HtmlApi.IHTMLCaret caret;
                ds.GetCaret(out caret);

                HtmlApi.IMarkupPointer pointer;
                ms.CreateMarkupPointer(out pointer);
                caret.MoveMarkupPointerToCaret(pointer);

                pointer.CurrentScope(out currentElement);
            }
            // Otherwise get the first object in the control range
            else
            {
                HtmlApi.IHTMLControlRange controlRange = range as HtmlApi.IHTMLControlRange;

                currentElement = controlRange.Item(0);
            }

            if (oldElement != currentElement)
            {
                OnCurrentElementChanged();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a color as an integer value
        /// </summary>
        private static uint GetCsColor(Color color) 
        {
            return (uint) (color.R + (color.G * 256) + (color.B * 65536));
        }

        /// <summary>
        /// Set the default font, forecolore, and backcolor to use
        /// </summary>
        private void SetComposeSettings() 
        {
            if (!IsCreated || ReadyState != HtmlReadyState.Complete) 
            {
                return;
            }

            if (defaultFont == null)
            {
                return;
            }

            if (!ExecCommand(HtmlApi.IDM_HTMLEDITMODE,true,false,false))
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            // Setup basic properties
            sb.Append(defaultFont.Bold ? "1," : "0,");
		    sb.Append(defaultFont.Italic ? "1," : "0,");
            sb.Append(defaultFont.Underline ? "1," : "0,");

            // Set size
            sb.AppendFormat("{0},", GetFontSizeFromEm(defaultFont.SizeInPoints));

            // Colors
            sb.AppendFormat("{0}.{1}.{2},", defaultForeColor.R, defaultForeColor.G, defaultForeColor.B);
            sb.AppendFormat("{0}.{1}.{2},", defaultBackColor.R, defaultBackColor.G, defaultBackColor.B);

            // Name
            sb.Append(defaultFont.Name);

            object settings = sb.ToString();

            // Set the settings.
            // ExecCommand(HtmlApi.IDM_COMPOSESETTINGS, settings, false, false);
        }

        /// <summary>
        /// Get the HTML font size number from the givem em point size
        /// </summary>
        private int GetFontSizeFromEm(float emSize)
        {
            if (emSize <= 8) return 1;
            if (emSize <= 10) return 2;
            if (emSize <= 12) return 3;
            if (emSize <= 18) return 4;
            if (emSize <= 24) return 5;
            if (emSize <= 36) return 6;

            return 7;
        }

        /// <summary>
        /// Font the active control
        /// </summary>
        private Control FindActiveControl(Control Parent) 
        {
            if (Parent is Form && ((Form) Parent).ActiveControl != null) 
            {
                return FindActiveControl(((Form) Parent).ActiveControl);
            } 
            else if (Parent is ContainerControl && ((ContainerControl) Parent).ActiveControl != null) 
            {
                return FindActiveControl(((ContainerControl) Parent).ActiveControl);
            } 
            else 
            {
                return Parent;
            }
        }

        /// <summary>
        /// Execute the given MSHTML command
        /// </summary>
        public bool ExecCommand(int iCommand, object argument, bool promptUser, bool checkReadystate) 
        {
            if (htmlDocumentClass == null)
                return false;
				
            //get the command target
            IOleCommandTarget ct = (IOleCommandTarget) htmlDocumentClass;

            if (ct == null)
            {
                throw new Exception("Cannot get COM command target");
            }

            //exec the command
            System.Guid pguidCmdGroup = new Guid("DE4BA900-59CA-11CF-9592-444553540000");
			
            Object[] pvaOut = null;
            int iRetval;

            int promptUserValue = promptUser ? 
                (int) OleApi.OLECMDEXECOPT.PROMPTUSER :
                (int) OleApi.OLECMDEXECOPT.DONTPROMPTUSER;
			
            object[] args = new object[] { argument };
            iRetval = ct.Exec(ref pguidCmdGroup, iCommand, promptUserValue, args, pvaOut);
		
            return (iRetval == 0);
        }

        /// <summary>
        /// Queries the status of the specified command
        /// </summary>
        private HtmlCommandStatus GetCommandInfo(int command) 
        {
            //get the command target
            IOleCommandTarget ct = (IOleCommandTarget) HtmlDocument;

            if (ct == null)
            {
                throw new Exception("Cannot get COM command target");
            }
			
            System.Guid pguidCmdGroup = new Guid("DE4BA900-59CA-11CF-9592-444553540000");
			
            //Query the command target for the command status
            OleApi.OLECMD oleCommand = new OleApi.OLECMD();
            oleCommand.cmdID = command;
            OleApi.OLECMD[] array = new OleApi.OLECMD[] { oleCommand };

            OleApi.OLECMDTEXT olecmdtext1 = new OleApi.OLECMDTEXT();
            olecmdtext1.cwActual = 0;
 
            int hr = ct.QueryStatus(ref pguidCmdGroup, 1, array, olecmdtext1);
			
            if (hr != NativeMethods.S_OK)
            {
                return HtmlCommandStatus.Unknown;
            }

            // Necessary due to boxing - the original is not changed
            oleCommand = array[0];

            if ((oleCommand.cmdf & (int) OleApi.OLECMDF.LATCHED) == (int) OleApi.OLECMDF.LATCHED) 
            {
                return HtmlCommandStatus.EnabledAndToggledOn;
            }
            else if ((oleCommand.cmdf & (int) OleApi.OLECMDF.ENABLED) == (int) OleApi.OLECMDF.ENABLED) 
            {
                return HtmlCommandStatus.Enabled;
            }
            else if ((oleCommand.cmdf & (int) OleApi.OLECMDF.SUPPORTED) == (int) OleApi.OLECMDF.SUPPORTED) 
            {
                return HtmlCommandStatus.Disabled;
            } 
            else 
            {
                return HtmlCommandStatus.Unsupported;
            }
        }

        /// <summary>
        /// Returns if the control has been properly created.
        /// </summary>
        internal bool IsCreated 
        {
            get {return (htmlSite != null) && (htmlSite.Document != null);}
        }

        /// <summary>
        /// Executes The short cut keys that should be available and handles all of the cases of design mode versus not.
        /// </summary>
        internal void DoShortCut(Keys key) 
        {
            switch (key) 
            {
                case Keys.A:
                    SelectAll();
                    break;

                case Keys.C:
                    Copy();
                    break;

                case Keys.P:
                    // Print(false);
                    break;

                case Keys.F:
                    // Find();
                    break;
            }

            // The following are only for edit mode
            if (editMode) 
            {
                switch(key) 
                {
                    case Keys.D1:
                        HtmlDocument.ExecCommand("FontSize", false, 1);
                        break;

                    case Keys.D2:
                        HtmlDocument.ExecCommand("FontSize", false, 2);
                        break;

                    case Keys.D3:
                        HtmlDocument.ExecCommand("FontSize", false, 3);
                        break;

                    case Keys.D4:
                        HtmlDocument.ExecCommand("FontSize", false, 4);
                        break;

                    case Keys.D5:
                        HtmlDocument.ExecCommand("FontSize", false, 5);
                        break;

                    case Keys.D6:
                        HtmlDocument.ExecCommand("FontSize", false, 6);
                        break;

                    case Keys.D7:
                        HtmlDocument.ExecCommand("FontSize", false, 7);
                        break;

                    case Keys.OemOpenBrackets:
                        Unindent();;
                        break;

                    case Keys.OemCloseBrackets:
                        Indent();
                        break;

                    case Keys.B:
                        SelectionBold = !SelectionBold;
                        break;

                    case Keys.I:
                        SelectionItalic = !SelectionItalic;
                        break;

                    case Keys.U:
                        SelectionUnderline = !SelectionUnderline;
                        break;

                    case Keys.L:
                        SelectionAlignment = HorizontalAlignment.Left;
                        break;

                    case Keys.E:
                        SelectionAlignment = HorizontalAlignment.Center;
                        break;

                    case Keys.R:
                        SelectionAlignment = HorizontalAlignment.Right;
                        break;

                    case Keys.V:
                        Paste();
                        break;

                    case Keys.X:
                        Cut();
                        break;

                    case Keys.Y:
                        undoManager.Redo();
                        break;

                    case Keys.Z:
                        undoManager.Undo();
                        break;
                }
            }

            htmlSite.UpdateUI();
        }

        #endregion

        #region Interal Functions Called from other classes

        internal void InvokeOnMouseDown(MouseEventArgs e) 
        {
            this.OnMouseDown(e);
        }

        internal void InvokeOnDoubleClick() 
        {
            this.OnDoubleClick(EventArgs.Empty);
        }

        internal void InvokeOnClick() 
        {
            this.OnClick(EventArgs.Empty);
        }

        internal void InvokeWndProc(ref Message msg) 
        {
            Message message = Message.Create(this.Handle, msg.Msg, msg.WParam, msg.LParam);
            this.WndProc(ref message);
        }

        internal void InvokeMouseLeave() 
        {
            this.OnMouseLeave(EventArgs.Empty);
        }

        internal void InvokeMouseHover() 
        {
            this.OnMouseHover(EventArgs.Empty);
        }
		
        internal void InvokeMouseEnter() 
        {
            this.OnMouseEnter(EventArgs.Empty);
        }

        internal void InvokeTab() 
        {
            this.OnKeyDown(new KeyEventArgs(Keys.Tab));
        }

        #endregion

        internal void ClearSelection()
        {
            if (!IsCreated)
            {
                return;
            }

            ExecCommand(HtmlApi.IDM_CLEARSELECTION, null, false, true);
        }
    }
}
