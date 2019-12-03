using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandRibbon;
using TD.SandDock;
using System.IO;
using Interapptive.Shared;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using System.Xml;
using System.Reflection;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Interapptive.Shared.IO.Zip;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Utility class for loading and saving layouts
    /// </summary>
    public partial class WindowLayoutProvider : Component
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(WindowLayoutProvider));

        // The ribbon that will be persisted
        RibbonManager ribbonManager;

        // The docking toolbar data that will be persisted
        SandDockManager dockManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowLayoutProvider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowLayoutProvider(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// The RibbonManager that provides the ribbon window state
        /// </summary>
        public RibbonManager RibbonManager
        {
            get { return ribbonManager; }
            set { ribbonManager = value; }
        }

        /// <summary>
        /// The SandDockManager that provides the docking window state
        /// </summary>
        public SandDockManager SandDockManager
        {
            get { return dockManager; }
            set { dockManager = value; }
        }

        /// <summary>
        /// Get the binary representation of the current layout.
        /// </summary>
        public byte[] SerializeLayout()
        {
            string tempFile = Path.Combine(DataPath.ShipWorksTemp, Guid.NewGuid().ToString("N") + ".swl");
            SaveFile(tempFile);

            return File.ReadAllBytes(tempFile);
        }

        /// <summary>
        /// Load the layout from the given binary state
        /// </summary>
        public void LoadLayout(byte[] layout)
        {
            string tempFile = Path.Combine(DataPath.ShipWorksTemp, Guid.NewGuid().ToString("N") + ".swl");
            File.WriteAllBytes(tempFile, layout);

            LoadFile(tempFile);
        }

        /// <summary>
        /// Save the current layout to the given file
        /// </summary>
        public void SaveFile(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            ZipWriter zipWriter = new ZipWriter();
            zipWriter.Items.Add(new ZipWriterStringItem(StripLastFocusedInfo(dockManager.GetLayout()), Encoding.Unicode, "panels.xml"));
            zipWriter.Items.Add(new ZipWriterStringItem(ribbonManager.GetState(), Encoding.Unicode, "ribbon.xml"));
            log.Debug($"Ribbon State:\n{ribbonManager.GetState()}");

            // Use a hard-coded LastModifiedTime so that binary comparisons on the zipfile work so we know
            // if the layout has actualy changed or not, and we don't get false-positives just due to the dates changing.
            zipWriter.Items.ForEach(i => i.LastModifiedTime = new DateTime(2001, 01, 01));

            try
            {
                zipWriter.Save(filename);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new AppearanceException(ex.Message, ex);
            }
        }

        /// <summary>
        /// The docking serializer saves the last focused info for the panels.  So if all you did was click a panel, but not actually move it,
        /// the whole layout would be saved.  This eliminates that, and we don't really need that last focused info.
        /// </summary>
        private string StripLastFocusedInfo(string dockLayout)
        {
            return Regex.Replace(dockLayout, @"LastFocused=""[^\s]+""", "");
        }

        /// <summary>
        /// Load the layout specified by the given file
        /// </summary>
        public void LoadFile(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            string tempPath = DataPath.CreateUniqueTempPath();

            try
            {
                // Write all the contents out to a temporary folder
                using (ZipReader reader = new ZipReader(filename))
                {
                    foreach (ZipReaderItem item in reader.ReadItems())
                    {
                        item.Extract(Path.Combine(tempPath, item.Name));
                    }
                }
            }
            catch (ZipException ex)
            {
                throw new AppearanceException("The file is not a valid ShipWorks layout.", ex);
            }

            // Restore the layouts
            try
            {
                ribbonManager.SetState(File.ReadAllText(Path.Combine(tempPath, "ribbon.xml"), Encoding.Unicode));
                dockManager.SetLayout(File.ReadAllText(Path.Combine(tempPath, "panels.xml"), Encoding.Unicode));
            }
            catch (FileNotFoundException ex)
            {
                throw new AppearanceException("The ShipWorks layout file is missing layout information.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new AppearanceException("The contents of the ShipWorks layout file are corrupt.", ex);
            }
            catch (XmlException ex)
            {
                throw new AppearanceException("The contents of the ShipWorks layout file are corrupt.", ex);
            }
            catch (NullReferenceException ex)
            {
                log.Error("Ribbon Configuration Exception:", ex);
                throw new AppearanceException("The contents of the ShipWorks layout file are corrupt", ex);
            }
        }

        /// <summary>
        /// Load the default layout
        /// </summary>
        public void LoadDefault()
        {
            LoadLayout(GetDefaultLayout());
        }

        /// <summary>
        /// Get the default layout in binary form
        /// </summary>
        public static byte[] GetDefaultLayout()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShipWorks.ApplicationCore.Appearance.WindowLayoutDefault.swl"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return reader.ReadBytes((int) stream.Length);
                }
            }
        }
    }
}
