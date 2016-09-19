using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Manages all label sheets (i.e. Avery, Dymo, Custom, etc.)
    /// </summary>
    public static class LabelSheetManager
    {
        static TableSynchronizer<LabelSheetEntity> sheetSynchronizer;
        static List<LabelSheetBrand> builtinBrands = new List<LabelSheetBrand>();

        static bool needCheckForChanges = false;

        /// <summary>
        /// Static constructor
        /// </summary>
        static LabelSheetManager()
        {
            LoadBuiltinBrands();
        }

        /// <summary>
        /// Initialize LabelSheetManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            sheetSynchronizer = new TableSynchronizer<LabelSheetEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (sheetSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (sheetSynchronizer)
            {
                if (sheetSynchronizer.Synchronize())
                {
                    sheetSynchronizer.EntityCollection.Sort((int) LabelSheetFieldIndex.Name, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the current list of all custom label sheets
        /// </summary>
        public static IList<LabelSheetEntity> CustomSheets
        {
            get
            {
                lock (sheetSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return EntityUtility.CloneEntityCollection(sheetSynchronizer.EntityCollection);
                }
            }
        }

        /// <summary>
        /// The collection of brands that are built in to ShipWorks
        /// </summary>
        public static IList<LabelSheetBrand> BuiltinBrands
        {
            get { return builtinBrands; }
        }

        /// <summary>
        /// The default label sheet for templates that do not have a label sheet already set.
        /// </summary>
        public static long DefaultLabelSheetID
        {
            get
            {
                // Avery \ 8160 - Address
                return -44;
            }
        }

        /// <summary>
        /// Get the sheet with the given brand and name.  This will look in both
        /// the builtin and custom sheets.  If not found, null is returned.
        /// </summary>
        public static LabelSheetEntity GetLabelSheet(long id)
        {
            // Check builtin
            foreach (LabelSheetBrand brand in builtinBrands)
            {
                foreach (LabelSheetEntity sheet in brand.Sheets)
                {
                    if (sheet.LabelSheetID == id)
                    {
                        return sheet;
                    }
                }
            }

            // Check custom
            foreach (LabelSheetEntity sheet in CustomSheets)
            {
                if (sheet.LabelSheetID == id)
                {
                    return sheet;
                }
            }

            return null;
        }

        /// <summary>
        /// Load all of our builtin brands
        /// </summary>
        private static void LoadBuiltinBrands()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(ResourceUtility.ReadString("ShipWorks.Templates.Media.LabelSheetDefinitions.xml"));

            foreach (XmlNode sheetNode in xmlDocument.SelectNodes("//LabelSheet"))
            {
                LabelSheetEntity sheet = new LabelSheetEntity();
                sheet.LabelSheetID = long.Parse(sheetNode["LabelSheetID"].InnerText);
                sheet.Name = sheetNode["Name"].InnerText;

                sheet.PaperSizeHeight = double.Parse(sheetNode["PaperSizeHeight"].InnerText);
                sheet.PaperSizeWidth = double.Parse(sheetNode["PaperSizeWidth"].InnerText);

                sheet.MarginTop = double.Parse(sheetNode["MarginTop"].InnerText);
                sheet.MarginLeft = double.Parse(sheetNode["MarginLeft"].InnerText);

                sheet.LabelHeight = double.Parse(sheetNode["LabelHeight"].InnerText);
                sheet.LabelWidth = double.Parse(sheetNode["LabelWidth"].InnerText);

                sheet.VerticalSpacing = double.Parse(sheetNode["VerticalSpacing"].InnerText);
                sheet.HorizontalSpacing = double.Parse(sheetNode["HorizontalSpacing"].InnerText);

                sheet.Rows = int.Parse(sheetNode["Rows"].InnerText);
                sheet.Columns = int.Parse(sheetNode["Columns"].InnerText);

                string brand = sheetNode["BrandName"].InnerText;
                sheet.BrandName = brand;

                EnsureBuiltinBrand(brand).Sheets.Add(sheet);
            }

            builtinBrands.Sort(new Comparison<LabelSheetBrand>(delegate (LabelSheetBrand left, LabelSheetBrand right)
            {
                return left.Name.CompareTo(right.Name);
            }));
        }

        /// <summary>
        /// Ensure a brand by the given name exists.
        /// </summary>
        private static LabelSheetBrand EnsureBuiltinBrand(string name)
        {
            foreach (LabelSheetBrand brand in builtinBrands)
            {
                if (brand.Name == name)
                {
                    return brand;
                }
            }

            LabelSheetBrand newBrand = new LabelSheetBrand(name);
            builtinBrands.Add(newBrand);

            return newBrand;
        }

        /// <summary>
        /// Indicates if the specified label sheet is used by any templates
        /// </summary>
        public static bool IsSheetUsed(long sheetID)
        {
            return TemplateCollection.GetCount(SqlAdapter.Default, TemplateFields.LabelSheetID == sheetID) > 0;
        }
    }
}
