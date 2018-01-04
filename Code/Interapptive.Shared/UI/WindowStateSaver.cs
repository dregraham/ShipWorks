using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using log4net;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Saves the size\position of a Form.
    /// </summary>
    public class WindowStateSaver
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowStateSaver));

        // The dictionary of window state, keyed from window name
        static IDictionary<string, WindowState> windowStateMap;

        /// <summary>
        /// Initialize the WindowStateSaver with the path on disk to store the window state
        /// </summary>
        public static void Initialize(string statefile)
        {
            if (StateFile != null)
            {
                throw new InvalidOperationException("The WindowStateSaver has already been initialized.");
            }

            StateFile = MethodConditions.EnsureArgumentIsNotNull(statefile, nameof(statefile));

            windowStateMap = LoadState(statefile);

            System.Windows.Forms.Application.ApplicationExit += (s, e) => SaveState(statefile);
        }

        /// <summary>
        /// The file that the settings are saved and read from.  Only valid if Initialized
        /// </summary>
        public static string StateFile { get; private set; }

        /// <summary>
        /// Load the window state from the state file
        /// </summary>
        private static IDictionary<string, WindowState> LoadState(string fileName) =>
            HydrateState(File.Exists(fileName) ? XElement.Load(fileName) : XElement.Parse("<empty />"));

        /// <summary>
        /// Load state from root element
        /// </summary>
        private static IDictionary<string, WindowState> HydrateState(XElement root)
        {
            try
            {
                return root
                    .Elements("WindowState")
                    .Select(HydrateWindowState)
                    .ToDictionary(x => x.Name, x => x);
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

            return new Dictionary<string, WindowState>();
        }

        /// <summary>
        /// Hydrate a window state element
        /// </summary>
        private static WindowState HydrateWindowState(XElement node) =>
            new WindowState
            {
                Name = (string) node.Attribute("name"),
                FormState = (FormWindowState) (int) node.Element("FormState"),
                Bounds = GetBounds(node.Element("Bounds")),
                BoundsWpf = GetBoundsWpf(node.Element("BoundsWpf")),
                SplitterDistances = HydrateSplitterDistances(node.Element("Splitters"))
            };

        /// <summary>
        /// Hydrate splitter distances from a node
        /// </summary>
        private static IDictionary<string, int> HydrateSplitterDistances(XElement splitters) =>
            splitters == null ?
                new Dictionary<string, int>() :
                splitters.Elements("Splitter").ToDictionary(
                    x => (string) x.Attribute("name"),
                    x => (int) x.Attribute("distance"));

        /// <summary>
        /// Get a bounds rectangle
        /// </summary>
        private static Rectangle GetBounds(XElement bounds) =>
            new Rectangle(
                (int) bounds.Attribute("x"),
                (int) bounds.Attribute("y"),
                (int) bounds.Attribute("width"),
                (int) bounds.Attribute("height"));

        /// <summary>
        /// Get a bounds rectangle
        /// </summary>
        private static Rect GetBoundsWpf(XElement bounds) =>
            new Rect(
                (double) bounds.Attribute("x"),
                (double) bounds.Attribute("y"),
                (double) bounds.Attribute("width"),
                (double) bounds.Attribute("height"));

        /// <summary>
        /// Save the window state to the state file
        /// </summary>
        private static void SaveState(string fileName)
        {
            XElement root = new XElement("ShipWorks");

            foreach (var element in windowStateMap.Values.Select(SerializeWindowState))
            {
                root.Add(element);
            }

            try
            {
                root.Save(fileName);
            }
            catch (IOException ex)
            {
                Debug.Fail("Failed to save state file: " + ex.Message);
                log.Error("Failed to save state file: " + ex.Message);
            }
        }

        /// <summary>
        /// Serialize a WindowState object
        /// </summary>
        private static XElement SerializeWindowState(WindowState state)
        {
            XElement stateElement = new XElement("WindowState");

            stateElement.Add(
                new XAttribute("name", state.Name),
                new XElement("Bounds",
                    new XAttribute("x", state.Bounds.X),
                    new XAttribute("y", state.Bounds.Y),
                    new XAttribute("width", state.Bounds.Width),
                    new XAttribute("height", state.Bounds.Height)),
                new XElement("BoundsWpf",
                    new XAttribute("x", state.BoundsWpf.X),
                    new XAttribute("y", state.BoundsWpf.Y),
                    new XAttribute("width", state.BoundsWpf.Width),
                    new XAttribute("height", state.BoundsWpf.Height)),
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

            return stateElement;
        }

        /// <summary>
        /// The state saver should manage the window state of the given form
        /// </summary>
        public static IFormsWindowStateSaver Manage(Form form) =>
            Manage(form, WindowStateSaverOptions.Size);

        /// <summary>
        /// The state saver should manage the window state of the given form.  Optionally can specify that only the size, and not location, is remembered.
        /// </summary>
        public static IFormsWindowStateSaver Manage(Form form, WindowStateSaverOptions options) =>
            Manage(form, options, form.Text);

        /// <summary>
        /// The state saver should manage the window state of the given form.  Optionally can specify that only the size, and not location, is remembered.
        /// </summary>
        public static IFormsWindowStateSaver Manage(Form form, WindowStateSaverOptions options, string name) =>
            Manage((state) => new FormsWindowStateSaver(form, options, state, name), name);

        /// <summary>
        /// The state saver should manage the window state of the given window
        /// </summary>
        public static IWpfWindowStateSaver Manage(Window window) =>
            Manage(window, WindowStateSaverOptions.Size);

        /// <summary>
        /// The state saver should manage the window state of the given window.  Optionally can specify that only the size, and not location, is remembered.
        /// </summary>
        public static IWpfWindowStateSaver Manage(Window window, WindowStateSaverOptions options) =>
            Manage(window, options, window.Title);

        /// <summary>
        /// The state saver should manage the window state of the given window.  Optionally can specify that only the size, and not location, is remembered.
        /// </summary>
        public static IWpfWindowStateSaver Manage(Window window, WindowStateSaverOptions options, string name) =>
            Manage((state) => new WpfWindowStateSaver(window, options, state, name), window.Title);

        /// <summary>
        /// Manage the state for a given form or window
        /// </summary>
        /// <typeparam name="TReturn">Type of WindowStateSaver to return</typeparam>
        /// <param name="createStateSaver">Func that will create the state saver instance</param>
        /// <param name="name">Name of the form or window</param>
        /// <returns>The state saver that is being managed</returns>
        private static TReturn Manage<TReturn>(Func<GenericResult<WindowState>, TReturn> createStateSaver, string name) where TReturn : IWindowStateSaver
        {
            if (windowStateMap == null)
            {
                throw new InvalidOperationException("The WindowStateSaver has not been initialized.");
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "(Nameless Window)";
            }

            var window = createStateSaver(GetWindowState(name));

            windowStateMap[name] = window.State;

            return window;
        }

        /// <summary>
        /// Get the window state from cache
        /// </summary>
        private static GenericResult<WindowState> GetWindowState(string name) =>
            windowStateMap.TryGetValue(name, out WindowState state) ?
                state :
                GenericResult.FromError<WindowState>("No state");
    }
}
