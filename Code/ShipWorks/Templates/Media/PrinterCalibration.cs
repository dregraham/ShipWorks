using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Defines offsets for printers for printing accurate label sheets
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what ShipWorks currently uses")]
    public class PrinterCalibration
    {
        // Printer and tray configured
        string printer;
        int paperSource;

        // Offsets to use when printing to the printer
        decimal yOffset;
        decimal xOffset;

        // The filename the settings were loaded from and should be saved to
        string filename;

        /// <summary>
        /// Cannot instantiate directly.  Used Load instead.
        /// </summary>
        private PrinterCalibration(string printer, int paperSource)
        {
            this.printer = printer;
            this.paperSource = paperSource;

            // Create the unique file name to store the given printer and paper settings in
            this.filename = GetStorageFileName(printer, paperSource);

            // Load the settings from an existing file
            if (File.Exists(this.filename))
            {
                Load();
            }

            // Use the default
            else
            {
                this.xOffset = 0;
                this.yOffset = 0;
            }
        }

        /// <summary>
        /// Loads existing calibration data for the given printer and source, or returns the default if it does not exist.
        /// </summary>
        public static PrinterCalibration Load(string printer, int paperSource)
        {
            return new PrinterCalibration(printer, paperSource);
        }

        /// <summary>
        /// Load the calibration data for this instance
        /// </summary>
        private void Load()
        {
            XDocument xDocument = XDocument.Load(filename);

            xOffset = (decimal) xDocument.Descendants("XOffset").Single();
            yOffset = (decimal) xDocument.Descendants("YOffset").Single();
        }

        /// <summary>
        /// Save the settings of this calibration to file
        /// </summary>
        public void Save()
        {
            XDocument xDocument = new XDocument(
                new XElement("Calibration",
                    new XAttribute("Printer", printer),
                    new XAttribute("PaperSource", paperSource),
                    new XElement("XOffset", xOffset),
                    new XElement("YOffset", yOffset)));

            xDocument.Save(filename);
        }

        /// <summary>
        /// Returns a filename based on the printer and papersource that are unique and will be used to save the settings.
        /// </summary>
        private static string GetStorageFileName(string printer, int paperSource)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(printer + paperSource);

            MD5 md5 = new MD5CryptoServiceProvider();
            string hash = Convert.ToBase64String(md5.ComputeHash(plainBytes));

            // Remove the only possible illegal char in result
            hash = hash.Replace('/', '!');

            // Remove any illegal characters in the printer name
            foreach (char invalid in Path.GetInvalidFileNameChars())
            {
                printer = printer.Replace(invalid.ToString(), "");
            }

            // Create the unique file name
            string file = string.Format("{0} ({1}) [{2}].xml",
                printer, paperSource, hash);

            // Ensure the director exists
            string folder = Path.Combine(DataPath.SharedSettings, "PrinterCalibration");
            Directory.CreateDirectory(folder);

            return Path.Combine(folder, file);
        }

        /// <summary>
        /// The name of the printer this calibration is for
        /// </summary>
        public string Printer
        {
            get { return printer; }
        }

        /// <summary>
        /// The paper source this calibration is for
        /// </summary>
        public int PaperSource
        {
            get { return paperSource; }
        }

        /// <summary>
        /// The X-Offset in tenths of an inch
        /// </summary>
        public decimal XOffset
        {
            get { return xOffset; }
            set { xOffset = value; }
        }

        /// <summary>
        /// The Y-Offset in tenths of an inch
        /// </summary>
        public decimal YOffset
        {
            get { return yOffset; }
            set { yOffset = value; }
        }
    }
}
