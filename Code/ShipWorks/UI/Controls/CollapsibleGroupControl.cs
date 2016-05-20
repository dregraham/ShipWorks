using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ComponentFactory.Krypton.Toolkit;
using log4net;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.Controls
{
    [Designer(typeof(CollapsibleGroupControlDesigner))]
    public partial class CollapsibleGroupControl : UserControl, ISupportInitialize
    {
        static readonly ILog log = LogManager.GetLogger(typeof(CollapsibleGroupControl));

        // Controls where the window state information is saved.  null until initialized
        static string statefile;

        // The dictionary of state, keyed from group key to if its collapsed or not
        static Dictionary<string, bool> collapsedState;

        string groupKey = Guid.NewGuid().ToString("B");

        bool initializing = false;
        bool initialCollapsed = false;
        bool settingCollapsed = false;

        int collapsedHeight;
        int expandedHeight;

        /// <summary>
        /// Initialize the control with the location of where to save it's state
        /// </summary>
        public static void Initialize(string statefile)
        {
            if (CollapsibleGroupControl.statefile != null)
            {
                throw new InvalidOperationException("The CollapsibleGroupControl has already been initialized.");
            }

            if (statefile == null)
            {
                throw new ArgumentNullException("statefile");
            }

            CollapsibleGroupControl.statefile = statefile;
            collapsedState = new Dictionary<string, bool>();

            if (File.Exists(statefile))
            {
                try
                {
                    XElement root = XElement.Load(statefile);
                    foreach (var group in root.Elements("Group"))
                    {
                        collapsedState[(string) group.Attribute("key")] = (bool) group.Attribute("collapsed");
                    }
                }
                catch (XmlException ex)
                {
                    log.Error("Invalid XML in collapsible state file: " + ex.Message);
                }
                catch (IOException ex)
                {
                    Debug.Fail("Failed to read collapsible state file: " + ex.Message);
                    log.Error("Failed to read collapsible state file: " + ex.Message);
                }
            }


            Application.ApplicationExit += new EventHandler(OnApplicationExit);
        }

        /// <summary>
        /// Raised when the applicatino is about to exit.  We use it to persist the window state.
        /// </summary>
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (!CrashDialog.IsApplicationCrashed)
                {
                    XElement root = new XElement("ShipWorks");
                    foreach (var group in collapsedState)
                    {
                        root.Add(new XElement("Group",
                            new XAttribute("key", group.Key),
                            new XAttribute("collapsed", group.Value)));
                    }

                    root.Save(statefile);
                }
            }
            catch (IOException ex)
            {
                Debug.Fail("Failed to save collapsible state file: " + ex.Message);
                log.Error("Failed to save collapsible state file: " + ex.Message);
            }
        }

        /// <summary>
        /// Get the collapsed state of the given group, using the default if no state is found
        /// </summary>
        private static bool GetCollapsedState(string groupKey, bool defaultValue)
        {
            if (collapsedState == null)
            {
                throw new InvalidOperationException("Collapsed state management has not been initialized.");
            }

            bool value;
            if (collapsedState.TryGetValue(groupKey, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Save the collapsed state of the given group
        /// </summary>
        private static void SaveCollapsedState(string groupKey, bool value)
        {
            if (collapsedState == null)
            {
                throw new InvalidOperationException("Collapsed state management has not been initialized.");
            }

            collapsedState[groupKey] = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CollapsibleGroupControl()
        {
            InitializeComponent();

            buttonSpecCollapse.Click += new EventHandler(OnExpandCollapse);

            collapsedHeight = collapseControl.Height - collapseControl.Panel.Height - 2;
        }

        /// <summary>
        /// BeginInit
        /// </summary>
        public void BeginInit()
        {
            initializing = true;
        }

        /// <summary>
        /// EndInit
        /// </summary>
        public void EndInit()
        {
            // Due to differences in OS, the collapsed height may have been serialized differently than we are running now.  So if the designer had us marked as
            // collapsed, make sure we are the correct collapsed height.
            if (initialCollapsed)
            {
                Height = collapsedHeight;
            }

            initializing = false;

            // Will not be initialized in design mode
            if (collapsedState != null)
            {
                Collapsed = GetCollapsedState(groupKey, initialCollapsed);
            }
        }

        /// <summary>
        /// Get the panel that contains the actual control content
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(false)]
        [Description("The internal panel that contains group content.")]
        public KryptonPanel ContentPanel
        {
            get { return collapseControl.Panel; }
        }

        /// <summary>
        /// The primary text of the panel
        /// </summary>
        [DefaultValue("Section Name")]
        [Category("Appearance")]
        public string SectionName
        {
            get
            {
                return collapseControl.ValuesPrimary.Heading;
            }
            set
            {
                collapseControl.ValuesPrimary.Heading = value;
            }
        }

        /// <summary>
        /// Exra text that can be displayed next to the section name
        /// </summary>
        [DefaultValue("Extra Text")]
        [Category("Appearance")]
        public string ExtraText
        {
            get
            {
                return collapseControl.ValuesPrimary.Description;
            }
            set
            {
                collapseControl.ValuesPrimary.Description = value;
            }
        }

        /// <summary>
        /// This is used so the designer will write out how tall we are supposed to be.  Not intended to be
        /// called directly from user code.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int ExpandedHeight
        {
            get
            {
                return expandedHeight;
            }
            set
            {
                expandedHeight = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string SettingsKey
        {
            get { return groupKey; }
            set { groupKey = value; }
        }

        /// <summary>
        /// Indicates to the designer if the expanded height value should be serialized.
        /// </summary>
        private bool ShouldSerializeExpandedHeight()
        {
            return Collapsed;
        }

        /// <summary>
        /// Controls if the content area of the control is visible
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        public bool Collapsed
        {
            get
            {
                if (initializing)
                {
                    return initialCollapsed;
                }

                return Height == collapsedHeight;
            }
            set
            {
                if (value == Collapsed)
                {
                    return;
                }

                collapseControl.ButtonSpecs[0].Type = value ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;

                if (initializing)
                {
                    initialCollapsed = value;
                    return;
                }

                if (value)
                {
                    expandedHeight = Height;

                    Height = collapsedHeight;
                }
                else
                {
                    settingCollapsed = true;
                    Height = expandedHeight;
                    settingCollapsed = false;
                }

                // Will not be initialized in design mode
                if (collapsedState != null)
                {
                    SaveCollapsedState(groupKey, Collapsed);

                    ScrollableControl scrollable = Parent as ScrollableControl;
                    if (scrollable != null)
                    {
                        scrollable.ScrollControlIntoView(this);
                    }
                }
            }
        }

        /// <summary>
        /// Intercept height changes while we are collapsed
        /// </summary>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (!initializing)
            {
                // If its collapsed, and we are changing the height, then just update out expand\collapse height
                if (Collapsed && (specified & BoundsSpecified.Height) != 0)
                {
                    if (!settingCollapsed && height != collapsedHeight)
                    {
                        // Remember what to be once we expand
                        expandedHeight = height;

                        // But keep it the collapsed height for now
                        height = collapsedHeight;
                    }
                }
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        /// <summary>
        /// Change the expand\collapse state of the control
        /// </summary>
        void OnExpandCollapse(object sender, EventArgs e)
        {
            Collapsed = !Collapsed;
        }

        /// <summary>
        /// User has double clicked the control, which basically just means the header, since the content area
        /// is a different panel.  It also includes the border, but that's not very likely to be clicked, and its
        /// ok even if they do.
        /// </summary>
        private void OnClick(object sender, EventArgs e)
        {
            if (PointToClient(Cursor.Position).Y < 25)
            {
                Collapsed = !Collapsed;
            }
        }

        /// <summary>
        /// Make the cursor a hand when over the title area
        /// </summary>
        public override Cursor Cursor
        {
            get
            {
                if (PointToClient(Cursor.Position).Y < 25)
                {
                    return Cursors.Hand;
                }

                return base.Cursor;
            }
            set
            {
                base.Cursor = value;
            }
        }
    }
}