using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using ShipWorks.Data.Grid.Columns;
using Interapptive.Shared.IO.Zip;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Xml;
using ShipWorks.Filters.Grid;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Utility class for saving and loading environment settings
    /// </summary>
    public class EnvironmentController
    {
        WindowLayoutProvider windowLayout;
        GridMenuLayoutProvider menuLayout;

        static string windowsZipItemName = "windows.swl";
        static string menusZipItemName = "menus.xml";

        /// <summary>
        /// Constructor
        /// </summary>
        public EnvironmentController(WindowLayoutProvider windowLayout, GridMenuLayoutProvider menuLayout)
        {
            if (windowLayout == null)
            {
                throw new ArgumentNullException("windowLayout");
            }

            if (menuLayout == null)
            {
                throw new ArgumentNullException("menuLayout");
            }


            this.windowLayout = windowLayout;
            this.menuLayout = menuLayout;
        }

        /// <summary>
        /// Save the chosen environment settings ot the specified filename
        /// </summary>
        public void SaveFile(string filename, EnvironmentOptions options)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            Cursor.Current = Cursors.WaitCursor;

            ZipWriter zipWriter = new ZipWriter();

            if (options.Windows)
            {
                zipWriter.Items.Add(new ZipWriterBinaryItem(windowLayout.SerializeLayout(), windowsZipItemName));
            }

            if (options.Menus)
            {
                zipWriter.Items.Add(new ZipWriterStringItem(menuLayout.SerializeLayout(), menusZipItemName));
            }

            if (options.Columns)
            {
                throw new NotSupportedException("Grid column saving is not supported right now.");
            }

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
        /// Inspect the file to see what environment options are available
        /// </summary>
        public EnvironmentOptions InspectFile(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            bool windows = false;
            bool menus = false;

            try
            {
                using (ZipReader reader = new ZipReader(filename))
                {
                    foreach (ZipReaderItem item in reader.ReadItems())
                    {
                        if (item.Name == windowsZipItemName)
                        {
                            windows = true;
                        }

                        if (item.Name == menusZipItemName)
                        {
                            menus = true;
                        }
                    }
                }
            }
            catch (ZipException ex)
            {
                throw new AppearanceException("The file is not a valid ShipWorks environment file.", ex);
            }

            return new EnvironmentOptions(windows, menus, false);
        }

        /// <summary>
        /// Load the file and the given options from it
        /// </summary>
        public void LoadFile(string filename, EnvironmentOptions options)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (options.Columns)
            {
                throw new NotSupportedException("Grid column loading is not supported right now.");
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
                throw new AppearanceException("The file is not a valid ShipWorks environment file.", ex);
            }

            // Restore the layouts
            try
            {
                if (options.Windows)
                {
                    windowLayout.LoadFile(Path.Combine(tempPath, windowsZipItemName));
                }

                if (options.Menus)
                {
                    menuLayout.LoadLayout(File.ReadAllText(Path.Combine(tempPath, menusZipItemName)));
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new AppearanceException("The ShipWorks environment file is missing information.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new AppearanceException("The contents of ShipWorks environment file are corrupt.", ex);
            }
            catch (XmlException ex)
            {
                throw new AppearanceException("The contents of ShipWorks environment file are corrupt.", ex);
            }
        }

        /// <summary>
        /// Reset the selected settings
        /// </summary>
        public void Reset(EnvironmentOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (options.Windows)
            {
                windowLayout.LoadDefault();
            }

            if (options.Menus)
            {
                menuLayout.LoadDefault();
            }

            if (options.Columns)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    // Delete all grid layouts for this user
                    adapter.DeleteEntitiesDirectly(typeof(FilterNodeColumnSettingsEntity),
                        new RelationPredicateBucket(FilterNodeColumnSettingsFields.UserID == UserSession.User.UserID));
                }

                // Reload the cache
                FilterNodeColumnManager.InitializeForCurrentUser();
            }
        }
    }
}
