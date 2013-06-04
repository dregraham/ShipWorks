using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using ShipWorks.Templates.Processing;
using log4net;
using ShipWorks.Common.IO.Hardware.Printers;
using System.Diagnostics;
using Interapptive.Shared.Utility;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Helper for printing thermal labels.
    /// </summary>
    public static class PrintThermalHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PrintThermalHelper));

        /// <summary>
        /// Print the given template result content to the given printer with the given jobname.  If there is no template result content and
        /// the print isn't even attempted, false is returned.
        /// </summary>
        public static bool Print(TemplateResult templateResult, string printer, string jobName)
        {
            XDocument xDoc;

            try
            {
                xDoc = XDocument.Parse(templateResult.ReadResult());
            }
            catch (XmlException ex)
            {
                throw new PrintingException("Thermal label template output must be valid XML.", ex);
            }

            var xLabels = xDoc.Descendants("ThermalLabel");

            if (xLabels.Count(l => ((string) l).Trim().Length > 0) == 0)
            {
                log.WarnFormat("Skipping thermal printing for job '{0}' to printer '{1}' since template output had no non-empty ThermalLabel tags.", printer, jobName);
                return false;
            }

            foreach (var xLabel in xLabels)
            {
                string content = ((string) xLabel).Trim();

                if (string.IsNullOrEmpty(content))
                {
                    continue;
                }

                try
                {
                    RawPrinter rawPrinter = new RawPrinter(printer);

                    // Determine the thermal label data to be sent
                    byte[] labelData = Convert.FromBase64String(content);

                    // Determine the thermal format the label is in.  XA is the required label start command for every ZPL label.  We can't just convert it to 
                    // a string to search b\c it could contain binary data.
                    bool isZpl = ByteUtility.Contains(labelData, Encoding.ASCII.GetBytes("^XA")) && ByteUtility.Contains(labelData, Encoding.ASCII.GetBytes("^XZ"));

                    if (isZpl)
                    {
                        // From testing it appears ZPL works fine without having to do any resets.  The only issue i found was if you sent EPL data from UPS to the printer,
                        // then ZPL labels after that were goofed.  But the fix for that is download the appropriate ZPL format in the first place.
                    }
                    else
                    {
                        // Send a command to ensure the printer is "Printing from top of image buffer."
                        //      * When FedEx and\or Endicia labels print, they change this value from the default to "Printing from bottom of image buffer".
                        //      * This, combined with UPS sending down oversized (1600 height) label images causes UPS imags to span multiple
                        //        labels when printed after FedEx thermals.  No amount of printer resetting seems to get it back.
                        byte[] printFromTop = Encoding.ASCII.GetBytes("ZT\n");

                        // Create the new array with the new data prepended
                        byte[] tempCopy = new byte[printFromTop.Length + labelData.Length];
                        printFromTop.CopyTo(tempCopy, 0);
                        labelData.CopyTo(tempCopy, printFromTop.Length);
                        labelData = tempCopy;
                    }

                    // Now send the actual thermal data
                    rawPrinter.SendBytesToPrinter(jobName, labelData);
                }
                catch (FormatException ex)
                {
                    throw new PrintingException("Invalid Base64 thermal label data.", ex);
                }
            }

            return true;
        }
    }
}
