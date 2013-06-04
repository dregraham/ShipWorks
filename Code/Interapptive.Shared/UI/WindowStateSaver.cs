using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using Interapptive.Shared;
using System.ComponentModel;
using Interapptive.Shared.Win32;
using System.Xml.Linq;
using System.Diagnostics;
using log4net;
using System.Xml;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Saves the size\position of a Form.
    /// </summary>
    public class WindowStateSaver
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowStateSaver));

        // Controls where the window state information is saved.  null until initialized
        static string statefile;
        
        // The dictionary of window state, keyed from window name
        static Dictionary<string, WindowState> windowStateMap;

        // The form whose state is being managed
        Form form;

        // The waved state of the given form
        WindowState state;

        // What state to restore
        WindowStateSaverOptions options = WindowStateSaverOptions.SizeOnly;

        // Any splitters we are maintaining state for
        Dictionary<string, SplitContainer> managedSplitters;

        /// <summary>
        /// Initialize the WindowStateSaver with the path on disk to store the window state
        /// </summary>
        public static void Initialize(string statefile)
        {
            if (WindowStateSaver.statefile != null)
            {
                throw new InvalidOperationException("The WindowStateSaver has already been initialized.");
            }

            if (statefile == null)
            {
                throw new ArgumentNullException("statefile");
            }

            WindowStateSaver.statefile = statefile;

            LoadState();

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
        }

        /// <summary>
        /// The file that the settings are saved and read from.  Only valid if Initialized
        /// </summary>
        public static string StateFile
        {
            get { return statefile; }
        }

        /// <summary>
        /// Raised when the applicatino is about to exit.  We use it to persist the window state.
        /// </summary>
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            SaveState();
        }

        /// <summary>
        /// Load the window state from the state file
        /// </summary>
        private static void LoadState()
        {
            windowStateMap = new Dictionary<string, WindowState>();

            if (!File.Exists(statefile))
            {
                return;
            }

            try
            {
                XElement root = XElement.Load(statefile);
                foreach (var node in root.Elements("WindowState"))
                {
                    WindowState state = new WindowState { Name = (string)node.Attribute("name") };

                    var bounds = node.Element("Bounds");
                    var formState = node.Element("FormState");
                    var splitters = node.Element("Splitters");

                    state.Bounds = new Rectangle(
                        (int)bounds.Attribute("x"),
                        (int)bounds.Attribute("y"),
                        (int)bounds.Attribute("width"),
                        (int)bounds.Attribute("height"));

                    state.FormState = (FormWindowState)(int)formState;

                    if (splitters != null)
                    {
                        foreach (var splitter in splitters.Elements("Splitter"))
                        {
                            state.SplitterDistances[(string)splitter.Attribute("name")] = (int)splitter.Attribute("distance");
                        }
                    }

                    windowStateMap[state.Name] = state;
                }
            }
            catch (IOException ex)
            {
                Debug.Fail("Failed to read state file: " + ex.Message);
                log.Error("Failed to read state file: " + ex.Message);
            }
            catch (XmlException ex)
            {
                Debug.Fail("Failed to read state file: " + ex.Message);
                log.Error("Failed to read state file: " + ex.Message);
            }
        }

        /// <summary>
        /// Save the window state to the state file
        /// </summary>
        private static void SaveState()
        {
            XElement root = new XElement("ShipWorks");

            foreach (WindowState state in windowStateMap.Values)
            {
                XElement stateElement = new XElement("WindowState");
                root.Add(stateElement);

                stateElement.Add(
                    new XAttribute("name", state.Name),
                    new XElement("Bounds",
                        new XAttribute("x", state.Bounds.X),
                        new XAttribute("y", state.Bounds.Y),
                        new XAttribute("width", state.Bounds.Width),
                        new XAttribute("height", state.Bounds.Height)),
                    new XElement("FormState", (int) state.FormState));

                if (state.SplitterDistances.Count > 0)
                {
                    XElement splitters = new XElement("Splitters");
                    stateElement.Add(splitters);

                    foreach (var pair in state.SplitterDistances)
                    {
                        splitters.Add(new XElement("Splitter",
                            new XAttribute("name", pair.Key),
                            new XAttribute("distance", pair.Value)));
                    }
                }
            }

            try
            {
                root.Save(statefile);
            }
            catch (IOException ex)
            {
                Debug.Fail("Failed to save state file: " + ex.Message);
                log.Error("Failed to save state file: " + ex.Message);
            }
        }

        /// <summary>
        /// The state saver should manage the window state of the given form
        /// </summary>
        public static void Manage(Form form)
        {
            Manage(form, WindowStateSaverOptions.SizeOnly);
        }

        /// <summary>
        /// The state saver should manage the window state of the given form.  Optionaly can specify that only the size, and not location, is remembered.
        /// </summary>
        public static void Manage(Form form, WindowStateSaverOptions options)
        {
            // This variable will be kept alive by the events that get attached to the form
            WindowStateSaver saver = new WindowStateSaver(form, options);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowStateSaver(Form form)
            : this(form, WindowStateSaverOptions.SizeOnly, form.Text)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowStateSaver(Form form, WindowStateSaverOptions options)
            : this(form, options, form.Text)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowStateSaver(Form form, WindowStateSaverOptions options, string windowName)
        {
            if (windowStateMap == null)
            {
                throw new InvalidOperationException("The WindowStateSaver has not been initialized.");
            }

            this.form = form;
            this.options = options;

            // Load the state
            RestoreWindowState(windowName);

            // Subscribe to parent form's events
            form.Closing += new CancelEventHandler(OnClosing);
            form.Resize += new EventHandler(OnResize);
            form.Move += new EventHandler(OnMove);
        }

        /// <summary>
        /// Load the state of the window from the registry
        /// </summary>
        private void RestoreWindowState(string windowName)
        {
            if (string.IsNullOrEmpty(windowName))
            {
                windowName = "(Nameless Window)";
            }

            // See if we have a previous entry for this window
            if (windowStateMap.TryGetValue(windowName, out state))
            {
                Rectangle currentBounds = form.DesktopBounds;
                Rectangle savedBounds = state.Bounds;

                if (options == WindowStateSaverOptions.SizeOnly)
                {
                    savedBounds.X = currentBounds.X;
                    savedBounds.Y = currentBounds.Y;
                }

                // In case of multi screen desktops, check if we got the screen the form was when closed.
                Rectangle screenBounds = Screen.GetBounds(new Point(savedBounds.Left, savedBounds.Top));
                if (savedBounds.Left > screenBounds.Right ||
                    savedBounds.Right < screenBounds.Left ||
                    savedBounds.Top > screenBounds.Bottom ||
                    savedBounds.Bottom < screenBounds.Top)
                {
                    // The form would be off-screen - just get out and let it open where it would be default
                    return;
                }

                // Restore size and state
                form.DesktopBounds = savedBounds;
                form.WindowState = state.FormState;

                // Since we have loaded the state ourselves
                if (options == WindowStateSaverOptions.FullState)
                {
                    form.StartPosition = FormStartPosition.Manual;
                }
            }
            // No entry yet for this saved state
            else
            {
                state = new WindowState
                    {
                        Name = windowName,
                        Bounds = form.DesktopBounds,
                        FormState = form.WindowState
                    };

                windowStateMap[windowName] = state;
            }
        }
        /// <summary>
        /// Adds the SplitContainer to the elements being remembered
        /// </summary>
        public void ManageSplitter(SplitContainer splitContainer)
        {
            ManageSplitter(splitContainer, "Splitter");
        }

        /// <summary>
        /// Adds the SplitContainer to the elements being remembered
        /// </summary>
        public void ManageSplitter(SplitContainer splitContainer, string name)
        {
            if (splitContainer == null)
            {
                throw new ArgumentNullException("splitContainer");
            }

            if (managedSplitters == null)
            {
                managedSplitters = new Dictionary<string, SplitContainer>();
            }

            managedSplitters.Add(name, splitContainer);

            int distance;
            if (state.SplitterDistances.TryGetValue(name, out distance))
            {
                splitContainer.SplitterDistance = distance;
            }
        }
                
        /// <summary>
        /// Saves the Form size
        /// </summary>
        private void OnResize(object sender, System.EventArgs e)
        {
            if (form.WindowState == FormWindowState.Normal)
            {
                state.Bounds = form.DesktopBounds;
            }
        }

        /// <summary>
        /// Saves the Form position
        /// </summary>
        private void OnMove(object sender, System.EventArgs e)
        {
            if (form.WindowState == FormWindowState.Normal)
            {
                state.Bounds = form.DesktopBounds;
            }

            // Don't be saved in a minimized state, force it to normal
            state.FormState = (form.WindowState == FormWindowState.Minimized) ? FormWindowState.Normal : form.WindowState;
        }

        /// <summary>
        /// Form is closing, save its position information.
        /// </summary>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (managedSplitters != null)
            {
                foreach (KeyValuePair<string, SplitContainer> pair in managedSplitters)
                {
                    state.SplitterDistances[pair.Key] = pair.Value.SplitterDistance;
                }
            }

            form.Closing -= new CancelEventHandler(OnClosing);
            form.Resize -= new EventHandler(OnResize);
            form.Move -= new EventHandler(OnMove);

            form = null;
            managedSplitters = null;
        }
    }
}
