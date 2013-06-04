using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Templates;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.Grid.DetailView
{
    /// <summary>
    /// Transport of view settings for a grid
    /// </summary>
    public class DetailViewSettings
    {
        // How to draw the grid row
        DetailViewMode viewMode = DetailViewMode.Normal;

        // If in Detail, how many rows the detail should be.  Default to zero, which is auto.
        int detailRows = 0;

        // The template to use to display the detail
        long templateID = 0;

        // Calculate height of single rows
        static readonly int singleRowHeight = Control.DefaultFont.Height + 5;

        /// <summary>
        /// Raised when any of the settings values have changed
        /// </summary>
        public event EventHandler SettingsChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public DetailViewSettings()
        {

        }

        /// <summary>
        /// Load the settings from the given xml.  If xml is null, default settings are used.
        /// </summary>
        public DetailViewSettings(string xml)
        {
            if (xml == null)
            {
                return;
            }

            LoadSettings(xml);
        }

        /// <summary>
        /// The view mode of the grid
        /// </summary>
        public DetailViewMode DetailViewMode
        {
            get
            {
                return viewMode;
            }
            set 
            {
                if (viewMode == value)
                {
                    return;
                }

                viewMode = value;

                RaiseSettingsChanged();
            }
        }

        /// <summary>
        /// How many rows to use for the display of the details
        /// </summary>
        public int DetailRows
        {
            get
            {
                return detailRows;
            }
            set
            {
                if (detailRows == value)
                {
                    return;
                }

                if (value < 0 || value > MaxDetailRows)
                {
                    throw new ArgumentOutOfRangeException("value", value, "DetailRows must be between 0 and " + MaxDetailRows);
                }

                detailRows = value;

                RaiseSettingsChanged();
            }
        }

        /// <summary>
        /// Get the height of a single grid row in pixels
        /// </summary>
        public static int SingleRowHeight
        {
            get { return singleRowHeight; }
        }

        /// <summary>
        /// The maximum number of detail rows allowed
        /// </summary>
        public static int MaxDetailRows
        {
            get { return 30; }
        }

        /// <summary>
        /// Gets the total height a grid row should be, in pixles, based on the current settings.
        /// </summary>
        public int TotalRowsHeight
        {
            get
            {
                // When its in Auto, we can't really know what its going to be, so just default to a few rows
                int rowsToUse = (DetailRows == 0) ? 1 : DetailRows;

                // If we are in detail only the main row height does not get added in
                int height = (viewMode == DetailViewMode.DetailOnly) ? 0 : SingleRowHeight;

                // If showing any details, add in the size fo the details
                if (viewMode != DetailViewMode.Normal)
                {
                    height += (SingleRowHeight * rowsToUse);
                }

                return height;
            }
        }

        /// <summary>
        /// The template to use to generate the detail output
        /// </summary>
        public long TemplateID
        {
            get
            {
                return templateID;
            }
            set
            {
                if (templateID == value)
                {
                    return;
                }

                templateID = value;

                RaiseSettingsChanged();
            }
        }

        /// <summary>
        /// Raises the SettingsChanged event
        /// </summary>
        private void RaiseSettingsChanged()
        {
            if (SettingsChanged != null)
            {
                SettingsChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Load the given settings from the specified xml.
        /// </summary>
        private void LoadSettings(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            XElement settings = XElement.Parse(xml);

            DetailViewMode = (DetailViewMode) (int) settings.Descendants("DetailViewMode").Single();
            DetailRows = (int) settings.Descendants("DetailRows").Single();
            TemplateID = (long) settings.Descendants("TemplateID").Single();

            // Select a default Grid View template if the previous template doesnt exist
            if (TemplateManager.Tree.GetTemplate(TemplateID) == null)
            {
                TemplateFolderEntity detailViewFolder = TemplateManager.Tree.AllFolders.FirstOrDefault(f => f.Name == "Grid (Detail View)");
                if (detailViewFolder != null && detailViewFolder.Templates.Count > 0)
                {
                    TemplateID = detailViewFolder.Templates[0].TemplateID;
                }
            }
        }

        /// <summary>
        /// Serialize the settings into an xml string
        /// </summary>
        public string SerializeSettings()
        {
            XElement settings =
                new XElement("DetailViewSettings",
                    new XElement("DetailViewMode", (int) viewMode),
                    new XElement("DetailRows", detailRows),
                    new XElement("TemplateID", templateID));

            return settings.ToString();
        }
    }
}
